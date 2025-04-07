using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LoadPlayerAvatar : ServerSIdeCharacterHandling
{
    public GameObject mainPanel;
    public ScrollRect avatarScrollRect;
    public GameObject avatarPrefab;
    public GameObject contentParent;

    public Button avatarButton;
    public Button continueButton;
    public Button deleteButton;

    public GameObject playerNamePanel;
    public TMP_InputField playerNameInputField;
    public Button PlayerPanelSaveButton;
    public Button updateExistingAvatar;

    public GameObject loader;
    public GameObject screenShotloader;

    public static string avatarId = null;
    public static string avatarName = "Test";
    public static string avatarThumbnailUrl = "";
    public static string avatarJson = null;
    public static GameObject currentSelected = null;

    public static LoadPlayerAvatar instance_loadplayer;

    private int currentpageNum = 1;
    private int pageSize = 15;

    private void Awake()
    {
        instance_loadplayer = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //avatarButton.gameObject.SetActive(false);  //disable the button for current release enable this feature later.
        if (PlayerPrefs.GetInt("IsLoggedIn") == 0)
        {
            avatarButton.gameObject.SetActive(false);
        }

        //PlayerPanelSaveButton.onClick.AddListener(() => ClosePlayerNamePanel()); //added persistent listener 
        avatarButton.onClick.AddListener(OpenAvatarPanel);

        //loadAllAvatar += (pageNo, NoOfRecords) => { LoadPlayerAvatar_onAvatarSaved(pageNo, NoOfRecords); };
    }

    //Event will be called when user loged In and new Avatar is saved by user.
    public void LoadPlayerAvatar_onAvatarSaved(int pageSize, int noOfRecords)
    {
        //disable the button for current release enable this feature later.

        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            avatarButton.gameObject.SetActive(true);
        }
        StartCoroutine(GetAvatarData_Server(pageSize, noOfRecords));
    }

    private void OnEnable()
    {
        if (currentSelected == null)
        {
            continueButton.interactable = false;
            deleteButton.interactable = false;
        }

    }

    public void OpenAvatarPanel()
    {
        mainPanel.SetActive(true);
    }

    public void CloseAvatarPanel()
    {
        mainPanel.SetActive(false);
    }

    public void OpenPlayerNamePanel()
    {
        if (avatarId == null && contentParent.transform.childCount == 0)
        {
            updateExistingAvatar.interactable = false;
        }
        else
            updateExistingAvatar.interactable = true;
        playerNameInputField.text = string.Empty;
        playerNamePanel.SetActive(true);
    }

    public void ClosePlayerNamePanel()
    {
        playerNamePanel.SetActive(false);
    }


    public void OnEditingEnd()
    {
        avatarName = playerNameInputField.text;

        if (string.IsNullOrEmpty(playerNameInputField.text))
        {
            PlayerPanelSaveButton.interactable = false;
        }
        else
        {
            PlayerPanelSaveButton.interactable = true;
        }

    }

    public void HighLightSelected(GameObject clickObject)
    {
        continueButton.interactable = true;
        deleteButton.interactable = true;
        for (int i = 0; i < contentParent.transform.childCount; i++)
        {
            contentParent.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        //enable prefab hightlight image
        clickObject.transform.GetChild(0).gameObject.SetActive(true);
    }



    IEnumerator GetAvatarData_Server(int pageNo, int noOfRecords)   // check if  data Exist
    {
        //UnityWebRequest www = UnityWebRequest.Get("https://app-api.xana.net/item/get-user-occupied-asset/1/50");
        UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.OCCUPIDEASSETS + pageNo + "/" + noOfRecords);
        www.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);

        //DefaultEnteriesforManican.instance.DefaultReset();
        if (!www.isHttpError && !www.isNetworkError)
        {
            if (getdata.success)
            {

                // its a new user so create file 
                if (getdata.data.count == 0)
                {
                    // do nothing
                }
                else
                {
                    // write latest json data to file
                    for (int c = 0; c < getdata.data.rows.Count; c++)
                    {
                        if (getdata.data.rows[c].id.ToString() != null && getdata.data.rows[c].thumbnail != null)
                        {
                            GameObject avatarInstance = Instantiate(avatarPrefab);
                            avatarInstance.transform.SetParent(contentParent.transform);
                            avatarInstance.transform.localPosition = Vector3.zero;
                            avatarInstance.transform.localScale = Vector3.one;
                            avatarInstance.transform.localRotation = Quaternion.identity;
                            
                            if (pageNo == 1 && noOfRecords == 1)
                            {
                                avatarInstance.transform.SetAsFirstSibling();
                                avatarId = getdata.data.rows[c].id.ToString();
                            }
                            
                            avatarInstance.GetComponent<SavedPlayerDataJson>().id = getdata.data.rows[c].id.ToString();
                            avatarInstance.GetComponent<SavedPlayerDataJson>().name = getdata.data.rows[c].name;
                            avatarInstance.GetComponent<SavedPlayerDataJson>().playerName.text = getdata.data.rows[c].name;
                            avatarInstance.GetComponent<SavedPlayerDataJson>().avatarJson = JsonUtility.ToJson(getdata.data.rows[c].json);
                            string thumbnailLink = getdata.data.rows[c].thumbnail;
                            avatarInstance.GetComponent<SavedPlayerDataJson>().avatarThumbnailLink = thumbnailLink;

                            GameObject imageObject = avatarInstance.GetComponent<SavedPlayerDataJson>().ImageObject;
                            if (!string.IsNullOrEmpty(thumbnailLink))
                                StartCoroutine(DownloadThumbnail(thumbnailLink, imageObject));

                            avatarInstance.GetComponent<Button>().onClick.AddListener(() => HighLightSelected(avatarInstance));
                            if (pageNo == 1 && noOfRecords == 1)
                            {
                                HighLightSelected(avatarInstance);
                            }
                                avatarInstance.gameObject.name = getdata.data.rows[c].id.ToString();
                        }
                    }


                    //File.WriteAllText((Application.persistentDataPath + "/SavingReoPreset.json"), JsonUtility.ToJson(getdata.data.rows[0].json));
                    yield return new WaitForSeconds(0.1f);
                    currentpageNum++;
                    loadNewPage = true;
                    www.Dispose();
                }
            }
        }
        else
            Debug.LogError("NetWorkissue");
    }


    IEnumerator DownloadThumbnail(string ImageUrl, GameObject thumbnail)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
        }
        else
        {
            if (ImageUrl.Equals(""))
            {
                yield return null;
            }
            else
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(ImageUrl);
                www.SendWebRequest();
                //WWW www = new WWW(ImageUrl);
                //www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }
                yield return www;
                //Debug.LogError(ImageUrl+"------"+www.downloadHandler.text);
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                texture.Compress(true);
                thumbnail.GetComponent<RawImage>().texture = texture;
                www.Dispose();
                //if (Application.internetReachability == NetworkReachability.NotReachable)
                //{

                //}
                //else
                //{
                //    //Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                //    if (thumbnail != null)
                //    {
                //        thumbnail.GetComponent<Image>().sprite = sprite;
                //        //Loader.SetActive(false);
                //    }
                //    else
                //    {
                //        // Loader.SetActive(false);
                //    }
                //}
            }
        }
    }




    bool isAlreadyRunning = true;
    public void LoadAvatarOnContinue()
    {
        try
        {
            if (currentSelected != null && isAlreadyRunning)
            {
                isAlreadyRunning = false;

                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(avatarJson);
                //Lips Default
                _CharacterData.myItemObj[5].ItemLinkAndroid = "";
                _CharacterData.myItemObj[5].ItemName = "";
                _CharacterData.myItemObj[5].ItemID = 0;
                //Lips
                _CharacterData.BodyFat = 0;
                File.WriteAllText((Application.persistentDataPath + "/SavingCharacterDataClass.json"), JsonUtility.ToJson(_CharacterData));

                //DefaultEnteriesforManican.instance.ResetForPresets();
                DownloadPlayerAssets();
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
                SavaCharacterProperties.instance.LoadMorphsfromFile();
                //StoreManager.instance.UndoSelection();

                isAlreadyRunning = true;

                //Enable save button
                //if (StoreManager.instance.StartPanel_PresetParentPanel.activeSelf)
                //{

                //    if (PlayerPrefs.GetInt("iSignup") == 1)
                //    {

                //        Invoke("abcd", 2.0f);

                //        StoreManager.instance.StartPanel_PresetParentPanel.SetActive(false);
                //    }
                //    else                // as a guest
                //    {


                //        StoreManager.instance.StartPanel_PresetParentPanel.SetActive(false);
                //        UserRegisterationManager.instance.usernamePanal.SetActive(true);
                //        // enable check so that it will know that index is comming from start of the game
                //        UserRegisterationManager.instance.checkbool_preser_start = false;
                //    }
                //}
                //else
                //{
                //    StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                //    StoreManager.instance.GreyRibbonImage.SetActive(false);
                //    StoreManager.instance.WhiteRibbonImage.SetActive(true);
                //}
            }
        }
        catch (Exception)
        {
            isAlreadyRunning = true;
#if UNITY_EDITOR
            Debug.Break();
#endif
        }
        mainPanel.SetActive(false);

    }

    void DownloadPlayerAssets()
    {
        StartCoroutine(WaitAndDownloadFromRevert(0));
    }

    IEnumerator WaitAndDownloadFromRevert(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
            {

                string currentlink = "";
#if UNITY_ANDROID
                currentlink = _CharacterData.myItemObj[i].ItemLinkAndroid;
#else
currentlink = _CharacterData.myItemObj[i].ItemLinkIOS;
#endif

                if (_CharacterData.myItemObj[i].ItemID == 0)
                {
                    ItemDatabase.instance.BindDefaultItems(_CharacterData.myItemObj[i]);
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(currentlink))   // if link is empty thn dont call it
                        {
                            //  Debug.Log("Downloading --- " + _CharacterData.myItemObj[i].ItemLink + " Link " + _CharacterData.myItemObj[i].ItemType);
                            string _temptype = _CharacterData.myItemObj[i].Slug;

                            ItemDetail itemobj = new ItemDetail();
                            itemobj.name = _CharacterData.myItemObj[i].ItemName.ToLower();
                            itemobj.id = _CharacterData.myItemObj[i].ItemID.ToString();
                            itemobj.assetLinkIos = _CharacterData.myItemObj[i].ItemLinkIOS;
                            itemobj.assetLinkAndroid = _CharacterData.myItemObj[i].ItemLinkAndroid;

                            StoreManager.instance._DownloadRigClothes.NeedToDownloadOrNot(itemobj, _CharacterData.myItemObj[i].ItemLinkAndroid, _CharacterData.myItemObj[i].ItemLinkIOS, _CharacterData.myItemObj[i].ItemType, _CharacterData.myItemObj[i].ItemName.ToLower(), _CharacterData.myItemObj[i].ItemID);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
                yield return new WaitForSeconds(.05f);
            }
        }
    }


    //update existing user 
    public void UpdateExistingUserData()
    {
        if (avatarId != null)
        {
            PlayerPrefs.SetInt("presetPanel", 0);
            SavaCharacterProperties.instance.SavePlayerPropertiesInClassObj();
            OnUpdateExistingRemoveOld(avatarId);
            ServerSIdeCharacterHandling.Instance.UpdateUserOccupiedAsset(avatarId);
        }
    }


    public void OnUpdateExistingRemoveOld(string _avatarID)
    {
        for(int i=0;i< contentParent.transform.childCount;i++)
        {
            if(contentParent.transform.GetChild(i).name==_avatarID)
            {
                Destroy(contentParent.transform.GetChild(i).gameObject);
                break;
            }
        }
    }


    //Delete Avatar From Server
    public void DeleteAvatar()
    {
        if (currentSelected != null)
        {
            string token = PlayerPrefs.GetString("LoginToken");
            DeleteAvatarDataFromServer(token, avatarId);

            Destroy(currentSelected.gameObject, 0f);

            Invoke("SelectFirstAvatarOnDelete",1);

            continueButton.interactable = false;
            deleteButton.interactable = false;
        }
    }


    void SelectFirstAvatarOnDelete()
    {
        if(contentParent.transform.childCount>0)
        {
            contentParent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
            if (currentSelected != null && isAlreadyRunning)
            {
                isAlreadyRunning = false;

                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(avatarJson);
                //Lips Default
                _CharacterData.myItemObj[5].ItemLinkAndroid = "";
                _CharacterData.myItemObj[5].ItemName = "";
                _CharacterData.myItemObj[5].ItemID = 0;
                //Lips
                _CharacterData.BodyFat = 0;
                File.WriteAllText((Application.persistentDataPath + "/SavingCharacterDataClass.json"), JsonUtility.ToJson(_CharacterData));
                DownloadPlayerAssets();
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
                SavaCharacterProperties.instance.LoadMorphsfromFile();
               
                isAlreadyRunning = true;
            }
        }
        else
        {
            avatarId = null;
        }

    }

    bool loadNewPage = true;
    public void CheckForPagination()
    {
        if (loadNewPage && avatarScrollRect.verticalNormalizedPosition < -0f)
        {
            loadNewPage = false;
            StartCoroutine(GetAvatarData_Server(currentpageNum, pageSize));
        }
    }
}
