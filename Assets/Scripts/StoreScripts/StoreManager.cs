using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public DownloadandRigClothes _DownloadRigClothes;
    public static StoreManager instance;
    [Header("Main Panels Store")]
    public GameObject StoreItemsPanel;
    public GameObject CheckOutBuyItemPanel;
    public GameObject ShowSignUpPanel;
    public GameObject LowCoinsPanel;
    public GameObject ShopBuyCoinsPanel;
    public EnumClass.CategoryEnum CategoriesEnumVar;
    public Text textskin;

    public GameObject ItemsBtnPrefab;
    [FormerlySerializedAs("oval")] public GameObject GreyRibbonImage;
    public GameObject WhiteRibbonImage;
    public Color HighlightedColor;
    public Color NormalColor;
    [Header("Total Panels Cloths")]
    public GameObject MainPanelCloth;
    public GameObject[] ClothsPanel;
    public GameObject BtnsPanelCloth;
    public GameObject ClothBtnLine;
    public Text ClothBtnText;
    [Header("Total Panels Avatar")]
    public GameObject MainPanelAvatar;

    public GameObject[] AvatarPanel;
    public GameObject BtnsPanelAvatar;
    public GameObject AvatarBtnLine;
    public Text AvatarBtnText;

    [Header("Total Buying Btns")]
    public GameObject BuyStoreBtn;
    //open a panel for player name 
    public GameObject SaveStoreBtn;
    //save player data to server
    public GameObject saveButton;
    [Header("Total Texts money Display")]
    public Text BuyCountertxt;
    public Text TotalGameCoins;
    public List<ItemDetail> TotalBtnlist;
    public List<ItemDetail> CategorieslistHeads;
    public List<ItemDetail> CategorieslistFace;
    public List<ItemDetail> CategorieslistInner;
    public List<ItemDetail> CategorieslistOuter;
    public List<ItemDetail> CategorieslistAccesary;
    public List<ItemDetail> CategorieslistBottom;
    public List<ItemDetail> CategorieslistSocks;
    public List<ItemDetail> CategorieslistShoes;
    public List<ItemDetail> CategorieslistEyesColor;
    public List<ItemDetail> CategorieslistLipsColor;
    public List<ItemDetail> CategorieslistSkinToneColor;
    [Header("Categories Panel Cloths")]
    public Transform ParentOfBtnsForHeads;
    public Transform ParentOfBtnsForFace;
    public Transform ParentOfBtnsForInner;
    public Transform ParentOfBtnsForOuter;
    public Transform ParentOfBtnsForAccesary;
    public Transform ParentOfBtnsForBottom;
    public Transform ParentOfBtnsForSocks;
    public Transform ParentOfBtnsForShoes;
    [Header("Categories Panel Avatar")]
    public Transform ParentOfBtnsAvatarHairs;
    public Transform ParentOfBtnsAvatarFace;
    public Transform ParentOfBtnsAvatarEyeBrows;
    public Transform ParentOfBtnsAvatarEyes;
    public Transform ParentOfBtnsAvatarNose;
    public Transform ParentOfBtnsAvatarLips;
    public Transform ParentOfBtnsAvatarBody;
    public Transform ParentOfBtnsAvatarSkin;
    [Header("Categories Color Customizations")]
    public Transform ParentOfBtnsCustomFace;
    public Transform ParentOfBtnsCustomEyes;
    public Transform ParentOfBtnsCustomLips;
    public Transform ParentOfBtnsCustomSkin;

    public List<ItemDetail> CategorieslistHairs;

    [Space(10f)]
    public GameObject colorCustomizationPrefabBtn;

    [Header("Buy Panel")]
    public GameObject BuyItemPrefab;
    public Transform BuyPanelParentOfBtns;
    public List<GameObject> TotalObjectsInBuyPanel;
    public List<GameObject> TotalSelectedInBuyPanel;
    public Text TotalPriceBuyPanelTxt;
    public Text TotalItemsBuyPanelTxt;
    public GameObject BuyBtnCheckOut;
    public string[] ArrayofBuyItems;
    private int TotalItemPriceCheckOut;

    

    [Header("Color Customizations")]
    public GameObject colorBtn;
    public BodyColorCustomization bodyColorCustomization;
    public CustomFakeStore fakeStore;
    // Get Data FromJsonFiles
    [HideInInspector]
    public GetAllInfo JsonDataObj;

    // New APIS Integration //
    // APIS
    public string GetAllCategoriesAPI;
    public string GetAllSubCategoriesAPI;
    public string GetAllItems;

    private bool Clothdatabool;
    private int IndexofPanel;
    private int PreviousSelectionCount;


    // Containers
    GetAllInfoMainCategories ObjofMainCategory;
    string[] ArrayofMainCategories;
    List<ItemsofSubCategories> SubCategoriesList;
    private bool CheckAPILoaded;
    List<TotaItemsClass> dataListOfItems = new List<TotaItemsClass>();

    public GameObject[] faceAvatarButton, eyeAvatarButton,
                  lipAvatarButton, eyeBrowsAvatarButton,
                   noseAvatarButton;

    public bool checkforSavebutton;  // its just for to check if item is downloaded or not
    public GameObject UndoBtn, RedoBtn, AvatarSaved,AvatarSavedGuest,AvatarUpdated;
    public GameObject Defaultreset, LastSavedreset, PanelResetDefault;
    List<GameObject> itemButtonsPool = new List<GameObject>();
    // public GameObject ButtonFor_Preset;
    public GameObject StartPanel_PresetParentPanel, PresetArrayContent;
    public GameObject backbutton_preset;

    public GameObject faceTapButton;
    public GameObject eyeBrowTapButton;
    public GameObject eyeTapButton;
    public GameObject noseTapButton;
    public GameObject lipTapButton;

    int panelIndex = 0;
    int buttonIndex = -1;
    bool saveButtonPressed = false;


    [Header("Preset Data")]
    public GameObject[] presets;
    private void Awake()
    {
        
        instance = this;
        checkforSavebutton = false;

        DisableColorPanels();

        //for (int i = 0; i < 20; i++) { itemButtonsPool.Add( Instantiate(ItemsBtnPrefab)); }
    }



    void Start()
    {
        
        CheckAPILoaded = false;
        GetAllMainCategories();
        Clothdatabool = false;
        dataListOfItems = new List<TotaItemsClass>();
        PreviousSelectionCount = -1;
        StartCoroutine(WaitForInstance());
        if (AvatarSaved)
            AvatarSaved.SetActive(false);


        SetPresetValue();
        //    if(UserRegisterationManager.instance.LoggedInAsGuest)
        //  Invoke("Character_DefaultReset",2.0f);
    }
    // Using accessory panel as preset jsons
    void SetPresetValue()
    {
        //  prefabbutton_preset;
        ////  GameObject contentparent = ClothsPanel[4].GetComponent<ScrollRect>().content.gameObject;

        ////  for(int x=0;x<contentparent.transform.childCount;x++)
        //// {
        ////     contentparent.transform.GetChild(x).gameObject.name = "Preset" + (1+x).ToString();
        //    contentparent.transform.GetChild(x).gameObject.GetComponent<Image>().color = Color.black;
        //   contentparent.transform.GetChild(x).GetComponent<Button>().onClick.AddListener(ChangecharacterOnCLickFromserver);
        //// }

        //    GameObject button = (GameObject)Instantiate(ButtonFor_Preset);
        //     button.transform.parent = contentparent.transform;
    }


    //void ChangecharacterOnCLickFromserver()
    //{
    //    print("Calling cloths");
    //    PlayerPrefs.SetInt("IsLoggedIn", 2);
    //        DefaultEnteriesforManican.instance.DefaultReset();
    //    GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
    //    SavaCharacterProperties.instance.LoadMorphsfromFile();
    //}
    private void Update()
    {

        // Quick fix AKA ElFY
        if (SaveStoreBtn.GetComponent<Image>().color == Color.white)
            SaveStoreBtn.GetComponent<Button>().interactable = false;
        else
            SaveStoreBtn.GetComponent<Button>().interactable = true;

    }

    public void CheckWhenUserLogin()
    {
        //SaveStoreBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        //saveButton.GetComponent<Button>().onClick.RemoveAllListeners();

        //if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        //{
        //    SaveStoreBtn.GetComponent<Button>().onClick.AddListener(() => AvatarSelfie.instance.TakeScreenShoot());
        //    SaveStoreBtn.GetComponent<Button>().onClick.AddListener(() => LoadPlayerAvatar.instance_loadplayer.OpenPlayerNamePanel());
        //    saveButton.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);
        //}
        //else
        //{
            //SaveStoreBtn.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);
        //}
    }


    IEnumerator WaitForInstance()
    {
        yield return new WaitForSeconds(.1f);
        //SaveStoreBtn.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            SaveStoreBtn.GetComponent<Button>().onClick.AddListener(() => AvatarSelfie.instance.TakeScreenShoot());
            SaveStoreBtn.GetComponent<Button>().onClick.AddListener(() => LoadPlayerAvatar.instance_loadplayer.OpenPlayerNamePanel());
            saveButton.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);
        }
        else
        {
            SaveStoreBtn.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);
        }

        if (Defaultreset)
            Defaultreset.GetComponent<Button>().onClick.AddListener(Character_DefaultReset);
        LastSavedreset.GetComponent<Button>().onClick.AddListener(Character_ResettoLastSaved);
        if (UndoBtn)
        {
            UndoBtn.GetComponent<Button>().onClick.AddListener(UndoFunc);
            UndoBtn.GetComponent<Button>().interactable = false;
        }
        if (RedoBtn)
        {
            RedoBtn.GetComponent<Button>().interactable = false;
            RedoBtn.GetComponent<Button>().onClick.AddListener(RedoFunc);
        }
        backbutton_preset.GetComponent<Button>().onClick.AddListener(BackTrackPreset);
    }
    void BackTrackPreset()
    {

        if (PlayerPrefs.GetInt("IsProcessComplete") == 0)

            UserRegisterationManager.instance.ShowWellComeCloseRetrack();
    }
    void Character_DefaultReset()
    {
        
        PlayerPrefs.SetInt("Loaded", 0);
        //       DefaultEnteriesforManican.instance.DefaultReset();
        DefaultEnteriesforManican.instance.DefaultReset_HAck();
        //   OnSaveBtnClicked();
        GameManager.Instance.mainCharacter.GetComponent<Equipment>().SaveDefaultValues();
        ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
        Default_LastSaved_PanelDisabler();
        UndoSelection();
        PlayerPrefs.SetInt("PresetValue", -1);
        GameManager.Instance.mainCharacter.GetComponent<Equipment>().UpdateStoreList();
        if(XanaConstants.xanaConstants._secondLastClickedBtn)
            XanaConstants.xanaConstants._secondLastClickedBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        XanaConstants.xanaConstants._lastClickedBtn = null;

        UpdateStoreSelection(XanaConstants.xanaConstants.currentButtonIndex);
    }
    void Character_ResettoLastSaved()
    {
        UndoSelection();
        // PlayerPrefs.SetInt("Loaded", 0); 
        // apply last saved values from local

        if (PlayerPrefs.GetInt("presetPanel") == 1)
            PlayerPrefs.SetInt("presetPanel", 0);
        DefaultEnteriesforManican.instance.ResetForLastSaved(); // ResetForPresets();
        GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
        SavaCharacterProperties.instance.LoadMorphsfromFile();
        Default_LastSaved_PanelDisabler();


        StoreManager.instance.GreyRibbonImage.SetActive(true);
        StoreManager.instance.WhiteRibbonImage.SetActive(false);
        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
        PresetData_Jsons test;
        if (FindObjectOfType<PresetData_Jsons>())
        {
            test = FindObjectOfType<PresetData_Jsons>();
            test.callit(); // = ""; // null;
        }// null;
         // DefaultEnteriesforManican.instance.LastSaved_Reset();
         // Default_LastSaved_PanelDisabler();
        StartCoroutine(StoreSelection());
    }
    IEnumerator StoreSelection()
    {
        yield return new WaitForSeconds(0.5f);

        UpdateStoreSelection(XanaConstants.xanaConstants.currentButtonIndex);
    }
    void Default_LastSaved_PanelDisabler()
    {
        PanelResetDefault.SetActive(false);
    }
    public void SavePresetValue()
    {
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].transform.GetChild(0).gameObject.activeSelf)
            {
                PlayerPrefs.SetInt("PresetValue", i);
                Debug.Log("Preset Value Saved");
            }
        }
    }
    public void SelectSavedPreset()
    {
        if (PlayerPrefs.GetInt("PresetValue") <= -1)
            return;
        for (int i = 1; i < presets.Length; i++)
        {
            if (presets[i].transform.GetChild(0).gameObject.activeSelf)
            {
                presets[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        presets[PlayerPrefs.GetInt("PresetValue")].transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("Selected Preset");
    }
    public void OnSaveBtnClicked()
    {

        // print("ppp");
        if (ItemDatabase.instance.gameObject != null)

        {
            //  print("ppp+");
            if (PlayerPrefs.GetInt("presetPanel") == 1)// preset panel is enable so saving preset to account 
            {
                SavePresetValue();
                PlayerPrefs.SetInt("presetPanel", 0);
            }

            PlayerPrefs.SetInt("Loaded", 1);
            if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
                AvatarSaved.SetActive(true);
            else
            {
                if (PlayerPrefs.GetInt("FirstTimeInstall")==0)
                    AvatarSavedGuest.SetActive(false);
                else
                    AvatarSavedGuest.SetActive(true);

            }
            ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
            XanaConstants.xanaConstants._secondLastClickedBtn = XanaConstants.xanaConstants._lastClickedBtn;
            XanaConstants.xanaConstants.isItemChanged = false;
            saveButtonPressed = true;
            ResetMorphBooleanValues();
            if(!ParentOfBtnsAvatarFace.gameObject.activeSelf)
                ParentOfBtnsAvatarFace.gameObject.SetActive(true);
        }
    }
    /// <New APIS>
    IEnumerator WaitForAPICallCompleted(int m_GetIndex)
    {
        print("wait Until");
        yield return new WaitUntil(() => CheckAPILoaded == true);
        if (CheckAPILoaded)
        {
            print("wait Until completed");
            if (SubCategoriesList.Count > 0)
            {
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex].id);
            }
        }
    }
    // **************************** Get Items by Sub categories ******************************//
    private string StringIndexofSubcategories(int _index)
    {
        var result = string.Join(",", _index.ToString());
        result = "[" + result + "]";
        return result;
    }
    [System.Serializable]
    public class ConvertSubCategoriesToJsonObj
    {
        public string subCategories;
        public int pageNumber;
        public int pageSize;
        public string version;
        public ConvertSubCategoriesToJsonObj CreateTOJSON(string jsonString, int _pageNumber, int _PageSize, string _vesrion)
        {
            ConvertSubCategoriesToJsonObj myObj = new ConvertSubCategoriesToJsonObj();
            myObj.subCategories = jsonString;
            myObj.pageNumber = _pageNumber;
            myObj.pageSize = _PageSize;
            myObj.version = _vesrion;
            return myObj;
        }
    }
    private void SubmitAllItemswithSpecificSubCategory(int GetCategoryIndex)
    {
        bool Once = false;
        if (PreviousSelectionCount != IndexofPanel)
        {
            PreviousSelectionCount = IndexofPanel;
            Once = true;
        }
        if (Once)
        {
            string result = StringIndexofSubcategories(GetCategoryIndex);
            ConvertSubCategoriesToJsonObj SubCatString = new ConvertSubCategoriesToJsonObj();
            string bodyJson = JsonUtility.ToJson(SubCatString.CreateTOJSON(result, 1, 50,APIBaseUrlChange.instance.apiversion));
            if (hitAllItemAPICorountine != null)
                StopCoroutine(hitAllItemAPICorountine);
            hitAllItemAPICorountine = StartCoroutine(HitALLItemsAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMS, bodyJson));
        }
    }
    public bool loadingItems = false;
    Coroutine itemLoading, hitAllItemAPICorountine;
    IEnumerator HitALLItemsAPI(string url, string Jsondata)
    {

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if (UserRegisterationManager.instance.LoggedIn)
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        }
        else
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
        }
        request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }
        GetItemInfoNewAPI JsonDataObj = new GetItemInfoNewAPI();
        JsonDataObj = JsonUtility.FromJson<GetItemInfoNewAPI>(request.downloadHandler.text);

        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (JsonDataObj.success == true)
                {
                    dataListOfItems.Clear();
                    dataListOfItems = JsonDataObj.data[0].items;
                    PutDataInOurAPPNewAPI();
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                print("Network Error");
            }
            else
            {
                if (request.error != null)
                {
                    if (JsonDataObj.success == false)
                    {
                        print("Hey success false " + JsonDataObj.msg);
                    }
                }
            }
        }
        request.Dispose();
    }

    [System.Serializable]
    public class GetItemInfoNewAPI
    {
        public bool success;
        public List<DataList> data;
        public string msg;
    }
    [System.Serializable]
    public class DataList
    {
        public int id;
        public int categoryId;
        public string name;
        public string createdAt;
        public string updatedAt;
        public List<TotaItemsClass> items;
    }

    [System.Serializable]
    public class TotaItemsClass
    {
        public int id;
        public string assetLinkAndroid;
        public string assetLinkIos;
        public string assetLinkWindows;
        public string iconLink;
        public int categoryId;
        public int subCategoryId;
        public string name;
        public bool isPaid;
        public string price;
        public bool isPurchased;
        public bool isFavourite;
        public bool isOccupied;
        public bool isDeleted;
        public string createdBy;
        public string createdAt;
        public string updatedAt;
        public string[] itemTags;
    }
    // **************************** Get Items by Sub categories ENDSSSSS ******************************//

    // **************************************************************************************************//
    ////////////////////////// <MAIN Category Started Here> ///////////////////////////////////////////////
    public void GetAllMainCategories()
    {
        StartCoroutine(HitAllMainCategoriesAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMCATEGORY, ""));
    }
    IEnumerator HitAllMainCategoriesAPI(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            request.SendWebRequest();
            while(!request.isDone)
            {
                yield return null;
            }
            ObjofMainCategory = GetAllDataNewAPI(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    if (ObjofMainCategory.success == true)
                    {
                        SaveAllMainCategoriesToArray();
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    print("Network Error");
                }
                else
                {
                    if (request.error != null)
                    {
                        if (ObjofMainCategory.success == false)
                        {
                            print("Hey success false " + ObjofMainCategory.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }
    public void SaveAllMainCategoriesToArray()
    {
        List<string> purchaseItemsIDs = new List<string>();
        for (int i = 0; i < ObjofMainCategory.data.Count; i++)
        {
            purchaseItemsIDs.Add(ObjofMainCategory.data[i].id);
        }
        ArrayofMainCategories = purchaseItemsIDs.ToArray();
        GetAllSubCategories();
    }

    public GetAllInfoMainCategories GetAllDataNewAPI(string m_JsonData)
    {
        GetAllInfoMainCategories JsonDataObj = new GetAllInfoMainCategories();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoMainCategories>(m_JsonData);
        return JsonDataObj;
    }
    [System.Serializable]
    public class GetAllInfoMainCategories
    {
        public bool success;
        public List<ItemsParentsNewAPI> data;
        public string msg;
    }
    [System.Serializable]
    public class ItemsParentsNewAPI
    {
        public string id;
        public string name;
        public string createdAt;
        public string updatedAt;
    }
    ////////////////////////// <MAIN Category ENDS here> ///////////////////////////////////////////////

    ///////////////////////// ************************** //////////////////////////////////////////////
    ////////////////////////// <SUB Category STARTS here> ///////////////////////////////////////////////
    private string AccessIndexOfSpecificCategory()
    {
        var result = string.Join(",", ArrayofMainCategories);
        result = "[" + result + "]";
        return result;
    }
    [System.Serializable]
    public class ConvertMainCat_Index_ToJson
    {
        public string categories;
        public ConvertMainCat_Index_ToJson CreateTOJSON(string jsonString)
        {
            ConvertMainCat_Index_ToJson myObj = new ConvertMainCat_Index_ToJson();
            myObj.categories = jsonString;
            return myObj;
        }
    }
    public void GetAllSubCategories()
    {
        string result = AccessIndexOfSpecificCategory();
        ConvertMainCat_Index_ToJson MainCatString = new ConvertMainCat_Index_ToJson();
        string bodyJson = JsonUtility.ToJson(MainCatString.CreateTOJSON(result));
        StartCoroutine(HitSUBCategoriesAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMSUBCATEGORY, bodyJson));
    }

    IEnumerator HitSUBCategoriesAPI(string url, string Jsondata)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
        request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }
        GetAllInfoSUBOFCategories JsonDataObj = new GetAllInfoSUBOFCategories();
        JsonDataObj = GetDataofSUBCategories(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (JsonDataObj.success == true)
                {
                    SubCategoriesList = JsonDataObj.data;
                    CheckAPILoaded = true;
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                CheckAPILoaded = true;
            }
            else
            {
                if (request.error != null)
                {
                    if (JsonDataObj.success == false)
                    {
                        CheckAPILoaded = true;
                    }
                }
            }
        }
        request.Dispose();
    }
    public GetAllInfoSUBOFCategories GetDataofSUBCategories(string m_JsonData)
    {
        GetAllInfoSUBOFCategories JsonDataObj = new GetAllInfoSUBOFCategories();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoSUBOFCategories>(m_JsonData);
        return JsonDataObj;
    }

    [System.Serializable]
    public class GetAllInfoSUBOFCategories
    {
        public bool success;
        public List<ItemsofSubCategories> data;
        public string msg;
    }
    [System.Serializable]
    public class ItemsofSubCategories
    {
        public int id;
        public int categoryId;
        public string name;
        public string createdAt;
        public string updatedAt;
    }
    //***************************** Get All Sub Categories END here **************************//

    /// </END APIS>

    public void GetDataofGuestUser()
    {
        BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(0);
        SelectPanel(1);
        PlayerPrefs.SetInt("TotalCoins", 0);
        UpdateUserCoins();
        if (!GameManager.Instance.OnceGuestBool)
        {
            RefreshDefault();
            GameManager.Instance.OnceGuestBool = true;
        }
        BuyCountertxt.text = "0";
    }
    public void GetDataAfterLogin()
    {
        BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(0);
        SelectPanel(1);
        SubmitUserDetailAPI();
        if (!GameManager.Instance.OnceLoginBool)
        {
            RefreshDefault();
            GameManager.Instance.OnceLoginBool = true;
        }
        BuyCountertxt.text = "0";
    }

    public void SignUpAndLoginPanel(int TakeString)
    {
        switch (TakeString)
        {
            case 0:
                {
                    StoreItemsPanel.SetActive(false);
                    ShowSignUpPanel.SetActive(false);
                    GameManager.Instance.BGPlane.SetActive(false);
                    UIManager.Instance.HomePage.SetActive(true);
                    UserRegisterationManager.instance.OpenUIPanal(6);
                    break;
                }
            case 1:
                {
                    StoreItemsPanel.SetActive(false);
                    ShowSignUpPanel.SetActive(false);
                    GameManager.Instance.BGPlane.SetActive(false);
                    UIManager.Instance.HomePage.SetActive(true);
                    UserRegisterationManager.instance.OpenUIPanal(1);
                    break;
                }
            case 2:
                {
                    OpenMainPanel("StoreItemsPanel");
                    UndoSelection();
                    GetDataofGuestUser();

                    GameManager.Instance.ShadowPlane.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, 0.3137f));
                    break;
                }
            case 3:
                {
                    OpenMainPanel("StoreItemsPanel");

                    GetDataAfterLogin();
                    break;
                }
        }
    }
    public void OpenMainPanel(string TakePanel)
    {
        StoreItemsPanel.SetActive(false);
        CheckOutBuyItemPanel.SetActive(false);
        ShowSignUpPanel.SetActive(false);
        LowCoinsPanel.SetActive(false);
        ShopBuyCoinsPanel.SetActive(false);
        switch (TakePanel)
        {
            case "StoreItemsPanel":
                {
                    StoreItemsPanel.SetActive(true);

                    break;
                }
            case "CheckOutBuyItemPanel":
                {
                    CheckOutBuyItemPanel.SetActive(true);
                    break;
                }
            case "ShowSignUpPanel":
                {
                    StoreItemsPanel.SetActive(true);
                    ShowSignUpPanel.SetActive(true);
                    break;
                }
            case "LowCoinsPanel":
                {
                    CheckOutBuyItemPanel.SetActive(true);
                    LowCoinsPanel.SetActive(true);
                    break;
                }
            case "ShopBuyCoinsPanel":
                {
                    ShopBuyCoinsPanel.SetActive(true);
                    break;
                }
        }
    }
    public void BackToHomeFromCharCustomization()
    {
        // if user is getting back fromaccessory panel/preset panel
        if (PlayerPrefs.GetInt("presetPanel") == 1)
        {

            //  if (GameManager.Instance.UserStatus_)
            PlayerPrefs.SetInt("presetPanel", 0);  // was loggedin as account 
                                                   //  else
                                                   //     PlayerPrefs.SetInt("IsLoggedIn", 0);  // was as a guest
            StoreManager.instance.GreyRibbonImage.SetActive(true);
            StoreManager.instance.WhiteRibbonImage.SetActive(false);
            StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
            print("-----");
            DefaultEnteriesforManican.instance.DefaultReset();
            GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
            SavaCharacterProperties.instance.LoadMorphsfromFile();
           
        }

        CharacterCustomizationUIManager.Instance.LoadMyClothCustomizationPanel();
        GameManager.Instance.ShadowPlane.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, 0.7843f));

        SavaCharacterProperties.instance.LoadMorphsfromFile();
        GameManager.Instance.mainCharacter.GetComponent<Equipment>().UpdateStoreList();

        if (ItemDatabase.instance != null)
        {
            ItemDatabase.instance.RevertSavedCloths();
          
        }
        SavaCharacterProperties.instance.AssignCustomSlidersData();
        SavaCharacterProperties.instance.AssignSavedPresets();
        GameManager.Instance.BlendShapeObj.DismissPoints();
        //if (MainPanelCloth.activeInHierarchy)
        //{
        //GameManager.Instance.BackFromStoreofCharacterCustom();
        //MainPanelCloth.SetActive(false);
        //StoreItemsPanel.SetActive(false);
       
        //UndoSelection();

        //}
        //else
        //{
        GameManager.Instance.BackFromStoreofCharacterCustom();
        MainPanelCloth.SetActive(false);
        StoreItemsPanel.SetActive(false);
        UndoSelection();
            //SelectPanel(0);
          
        //}
    }


    public void SelectPanel(int TakeIndex)
    {
        panelIndex = TakeIndex;

        if (TakeIndex == 0)
        {
            // CLoth
            buttonIndex = 3;
            XanaConstants.xanaConstants.currentButtonIndex = buttonIndex;
            MainPanelCloth.SetActive(true);
            MainPanelAvatar.SetActive(false);
            OpenClothContainerPanel(0);
            BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(3);
            ClothBtnLine.SetActive(true);
            AvatarBtnLine.SetActive(false);
            ClothBtnText.color = HighlightedColor;
            AvatarBtnText.color = NormalColor;
            UpdateStoreSelection(3);
        }
        else
        {
            // Avatar
            buttonIndex = 0;
            XanaConstants.xanaConstants.currentButtonIndex = buttonIndex;
            MainPanelCloth.SetActive(false);
            MainPanelAvatar.SetActive(true);
            OpenAvatarContainerPanel(0);
            BtnsPanelAvatar.GetComponent<SubBottons>().ClickBtnFtn(0);
            ClothBtnLine.SetActive(false);
            AvatarBtnLine.SetActive(true);
            ClothBtnText.color = NormalColor;
            AvatarBtnText.color = HighlightedColor;
            UpdateStoreSelection(0);
        }

        // reset cloths if getting back from accessories/presetpanel


        if (PlayerPrefs.GetInt("presetPanel") == 1)
        {


            PlayerPrefs.SetInt("presetPanel", 0);  // was loggedin as account 

            StoreManager.instance.GreyRibbonImage.SetActive(true);
            StoreManager.instance.WhiteRibbonImage.SetActive(false);
            StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
            print("-----");
            DefaultEnteriesforManican.instance.DefaultReset();
            GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
            SavaCharacterProperties.instance.LoadMorphsfromFile();
        }
        //

        DisableColorPanels();
    }
    public void UpdateUserCoins()
    {
        string totalCoins = PlayerPrefs.GetInt("TotalCoins").ToString();
        double coins = Double.Parse(totalCoins);
        totalCoins = String.Format("{0:n0}", coins);
        TotalGameCoins.text = totalCoins;
        CreditShopManager.instance.TotalCoins.text = totalCoins;
    }
    // Update is called once per frame

    public void OpenClothContainerPanel(int m_GetIndex)
    {
        buttonIndex = m_GetIndex;
        Clothdatabool = true;
        IndexofPanel = m_GetIndex;
        for (int i = 0; i < ClothsPanel.Length; i++)
        {
            if (m_GetIndex != i)
                ClothsPanel[i].SetActive(false);
        }
        ClothsPanel[m_GetIndex].SetActive(true);

        if (CheckAPILoaded)
        {
            if (SubCategoriesList.Count > 0)
            {
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex].id);
            }
        }
        else
        {
            StartCoroutine(WaitForAPICallCompleted(m_GetIndex));
        }
    }
    public void OpenAvatarContainerPanel(int m_GetIndex)
    {
        buttonIndex = m_GetIndex;
        Clothdatabool = false;
        IndexofPanel = m_GetIndex + 8;


        for (int i = 0; i < AvatarPanel.Length; i++)
        {
            AvatarPanel[i].SetActive(false);
        }
        AvatarPanel[m_GetIndex].SetActive(true);
        if (m_GetIndex == 8) //its a preset do nothing
            return;
        if (CheckAPILoaded)
        {
            if (SubCategoriesList.Count > 0)
            {
                print(SubCategoriesList[m_GetIndex + 8].id);
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex + 8].id);
            }
        }
        else
        {
            StartCoroutine(WaitForAPICallCompleted(m_GetIndex));
        }
        CheckColorProperty(m_GetIndex);
    }

    void CheckColorProperty(int _index)
    {
        if (_index == 3 || _index == 5 || _index == 7)
        {
            SwitchColorMode(_index);
            if (ParentOfBtnsAvatarSkin.GetComponent<Transform>().childCount == 0 && _index==7)
            {
                colorBtn.SetActive(false);
            }
            else
            {
                colorBtn.SetActive(true);
                colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                Debug.Log("Call hua refresh");
                colorBtn.GetComponent<Button>().onClick.AddListener(() => OnColorButtonClicked(_index));
            }
           
        }
        else
        {
            colorBtn.SetActive(false);
        }
    }

    void SwitchColorMode(int index)
    {
        Debug.Log("ColorBtn");
        Debug.Log("ColorBtn call hua click"+ index);
        textskin.enabled = false;
        colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        colorBtn.GetComponent<Button>().onClick.AddListener(() => OnColorButtonClicked(index));

        switch (index)
        {
            // 23june2022 Hardik change true false issue from case 2 to  case 7:
            case 1:
                ParentOfBtnsAvatarFace.gameObject.SetActive(true);
                ParentOfBtnsCustomFace.gameObject.SetActive(false);

                SetContentOnScroll(AvatarPanel[1], (RectTransform)ParentOfBtnsAvatarFace);
                break;
            case 3:
                ParentOfBtnsAvatarEyes.gameObject.SetActive(true);
                ParentOfBtnsCustomEyes.gameObject.SetActive(false);

                SetContentOnScroll(AvatarPanel[3], (RectTransform)ParentOfBtnsAvatarEyes);
                break;
            case 5:
                ParentOfBtnsAvatarLips.gameObject.SetActive(true);
                ParentOfBtnsCustomLips.gameObject.SetActive(false);

                SetContentOnScroll(AvatarPanel[5], (RectTransform)ParentOfBtnsAvatarLips);
                break;
            case 7:
                Debug.Log("Print hua skin ka");

               
                ParentOfBtnsCustomSkin.gameObject.SetActive(true);
                ParentOfBtnsAvatarSkin.gameObject.SetActive(false);

                SetContentOnScroll(AvatarPanel[7], (RectTransform)ParentOfBtnsCustomSkin);
                break;
        }

        UpdateStoreSelection(index);
    }
    void OnColorButtonClicked(int _index)
    {
        if (!AvatarSaved.activeInHierarchy)
        {
            Debug.Log("call hua refersh click=====");
            colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            colorBtn.GetComponent<Button>().onClick.AddListener(() => SwitchColorMode(_index));

            switch (_index)
            {
                //23june2022 Hardik change true false issue from case 7 :
                case 1:
                    ParentOfBtnsAvatarFace.gameObject.SetActive(false);
                    ParentOfBtnsCustomFace.gameObject.SetActive(true);

                    SetContentOnScroll(AvatarPanel[1], (RectTransform)ParentOfBtnsCustomFace);
                    break;
                case 3:
                    if (!AvatarSaved.activeInHierarchy)
                    {
                        ParentOfBtnsAvatarEyes.gameObject.SetActive(false);
                        ParentOfBtnsCustomEyes.gameObject.SetActive(true);

                        UpdateColor(_index);

                        SetContentOnScroll(AvatarPanel[3], (RectTransform)ParentOfBtnsCustomEyes);
                    }

                    break;
                case 5:
                    if (!AvatarSaved.activeInHierarchy)
                    {
                        ParentOfBtnsAvatarLips.gameObject.SetActive(false);
                        ParentOfBtnsCustomLips.gameObject.SetActive(true);

                        UpdateColor(_index);

                        SetContentOnScroll(AvatarPanel[5], (RectTransform)ParentOfBtnsCustomLips);
                    }
                    break;
                case 7:
                    if (!AvatarSaved.activeInHierarchy)
                    {
                        Debug.Log("Skin issue");
                        ParentOfBtnsCustomSkin.gameObject.SetActive(true);
                        ParentOfBtnsAvatarSkin.gameObject.SetActive(false);


                        UpdateColor(_index);

                        SetContentOnScroll(AvatarPanel[7], (RectTransform)ParentOfBtnsCustomSkin);
                    }
                    break;
            }

        }
    
    }

    public void UpdateColor(int _index)
    {
        Debug.Log("Update Color");
        textskin.enabled = true;
        switch (_index)
        {
            case 1:
                
                //Nothing

                break;

            case 3:
                if (XanaConstants.xanaConstants.eyeColor != "")
                {
                    for (int i = 0; i < ParentOfBtnsCustomEyes.transform.childCount; i++)
                    {
                        if (ParentOfBtnsCustomEyes.transform.GetChild(i).GetComponent<ItemDetail>().id == XanaConstants.xanaConstants.eyeColor)
                        {
                            ParentOfBtnsCustomEyes.transform.GetChild(i).GetComponent<Image>().enabled = true;
                            XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsCustomEyes.transform.GetChild(i).gameObject;
                            XanaConstants.xanaConstants.colorSelection[0] = ParentOfBtnsCustomEyes.transform.GetChild(i).gameObject;

                            CheckForItemDetail(XanaConstants.xanaConstants.eyeColor, 4);

                            break;
                        }
                    }
                }
                break;

            case 5:
                if (XanaConstants.xanaConstants.lipColor != "")
                {
                    for (int i = 0; i < ParentOfBtnsCustomLips.transform.childCount; i++)
                    {
                        if (ParentOfBtnsCustomLips.transform.GetChild(i).GetComponent<ItemDetail>().id == XanaConstants.xanaConstants.lipColor)
                        {
                            ParentOfBtnsCustomLips.transform.GetChild(i).GetComponent<Image>().enabled = true;
                            XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsCustomLips.transform.GetChild(i).gameObject;
                            XanaConstants.xanaConstants.colorSelection[1] = ParentOfBtnsCustomLips.transform.GetChild(i).gameObject;

                            CheckForItemDetail(XanaConstants.xanaConstants.lipColor, 5);

                            break;
                        }
                    }
                }
                break;

            case 7:
                if (XanaConstants.xanaConstants.skinColor != "")
                {
                    for (int i = 0; i < ParentOfBtnsCustomSkin.transform.childCount; i++)
                    {
                        if (ParentOfBtnsCustomSkin.transform.GetChild(i).GetComponent<ItemDetail>().id == XanaConstants.xanaConstants.skinColor)
                        {
                            ParentOfBtnsCustomSkin.transform.GetChild(i).GetComponent<Image>().enabled = true;
                            XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsCustomSkin.transform.GetChild(i).gameObject;
                            XanaConstants.xanaConstants.avatarStoreSelection[7] = ParentOfBtnsCustomSkin.transform.GetChild(i).gameObject;

                            CheckForItemDetail(XanaConstants.xanaConstants.skinColor, 6);

                            break;
                        }
                    }
                }
                break;
               

        }
    }

    public void SetContentOnScroll(GameObject _scrollView, RectTransform _content)
    {
        _scrollView.GetComponent<ScrollRect>().content = _content;
    }

    public void ClearBuyItems()
    {
        for (int i = 0; i < TotalBtnlist.Count; i++)
        {
            TotalBtnlist[i].SelectedBool = false;
            TotalBtnlist[i].gameObject.GetComponent<Image>().color = TotalBtnlist[i].NormalColor;
        }
        TotalBtnlist.Clear();
    }
    public void GetSelectedBtn(int getIndex, EnumClass.CategoryEnum selectedEnum)
    {

        switch (selectedEnum)
        {
            case EnumClass.CategoryEnum.Head:
                {
                    for (int i = 0; i < CategorieslistHeads.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistHeads[i].SelectedBool = true;
                            CategorieslistHeads[i].gameObject.GetComponent<Image>().color = CategorieslistHeads[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistHeads[i]);
                        }
                        else
                        {
                            CategorieslistHeads[i].SelectedBool = false;
                            CategorieslistHeads[i].gameObject.GetComponent<Image>().color = CategorieslistHeads[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistHeads[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Face:
                {
                    for (int i = 0; i < CategorieslistFace.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistFace[i].SelectedBool = true;
                            CategorieslistFace[i].GetComponent<Image>().color = CategorieslistFace[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistFace[i]);
                        }
                        else
                        {
                            CategorieslistFace[i].SelectedBool = false;
                            CategorieslistFace[i].GetComponent<Image>().color = CategorieslistFace[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistFace[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Inner:
                {
                    for (int i = 0; i < CategorieslistInner.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistInner[i].SelectedBool = true;
                            CategorieslistInner[i].GetComponent<Image>().color = CategorieslistInner[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistInner[i]);

                        }
                        else
                        {
                            CategorieslistInner[i].SelectedBool = false;
                            CategorieslistInner[i].GetComponent<Image>().color = CategorieslistInner[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistInner[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Outer:
                {
                    for (int i = 0; i < CategorieslistOuter.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistOuter[i].SelectedBool = true;
                            CategorieslistOuter[i].GetComponent<Image>().color = CategorieslistOuter[i].HighlightedColor;
                            TotalBtnlist.Add(CategorieslistOuter[i]);
                        }
                        else
                        {
                            CategorieslistOuter[i].SelectedBool = false;
                            CategorieslistOuter[i].GetComponent<Image>().color = CategorieslistOuter[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistOuter[i]);
                        }
                    }
                    break;
                }

            case EnumClass.CategoryEnum.Accesary:
                {
                    for (int i = 0; i < CategorieslistAccesary.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistAccesary[i].SelectedBool = true;
                            CategorieslistAccesary[i].GetComponent<Image>().color = CategorieslistAccesary[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistAccesary[i]);
                        }
                        else
                        {
                            CategorieslistAccesary[i].SelectedBool = false;
                            CategorieslistAccesary[i].GetComponent<Image>().color = CategorieslistAccesary[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistAccesary[i]);
                        }
                    }
                    break;
                }

            case EnumClass.CategoryEnum.Bottom:
                {
                    for (int i = 0; i < CategorieslistBottom.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistBottom[i].SelectedBool = true;
                            CategorieslistBottom[i].GetComponent<Image>().color = CategorieslistBottom[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistBottom[i]);
                        }
                        else
                        {
                            CategorieslistBottom[i].SelectedBool = false;
                            CategorieslistBottom[i].GetComponent<Image>().color = CategorieslistBottom[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistBottom[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Socks:
                {
                    for (int i = 0; i < CategorieslistSocks.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistSocks[i].SelectedBool = true;
                            CategorieslistSocks[i].GetComponent<Image>().color = CategorieslistSocks[i].HighlightedColor;
                            TotalBtnlist.Add(CategorieslistSocks[i]);
                        }
                        else
                        {
                            CategorieslistSocks[i].SelectedBool = false;
                            CategorieslistSocks[i].GetComponent<Image>().color = CategorieslistSocks[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistSocks[i]);
                        }
                    }
                    break;
                }
            // CategorieslistShoes
            case EnumClass.CategoryEnum.Shoes:
                {
                    for (int i = 0; i < CategorieslistShoes.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistShoes[i].SelectedBool = true;
                            CategorieslistShoes[i].GetComponent<Image>().color = CategorieslistShoes[i].HighlightedColor;
                            TotalBtnlist.Add(CategorieslistShoes[i]);
                        }
                        else
                        {
                            CategorieslistShoes[i].SelectedBool = false;
                            CategorieslistShoes[i].GetComponent<Image>().color = CategorieslistShoes[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistShoes[i]);
                        }
                    }
                    break;
                }
        }
        SelectBtnObjs();
    }

    public void SelectBtnObjs()
    {
        int TotalPrice = 0;
        for (int i = 0; i < TotalBtnlist.Count; i++)
        {
            if (TotalBtnlist[i].SelectedBool)
            {
                TotalPrice += int.Parse(TotalBtnlist[i].PriceTxt.text);
            }
        }
        BuyCountertxt.text = TotalBtnlist.Count.ToString();
    }
    public void ClearParentFromObjs()
    {
        if (BuyPanelParentOfBtns.childCount > 0)
        {
            for (int i = 0; i < BuyPanelParentOfBtns.childCount; i++)
            {
                Destroy(BuyPanelParentOfBtns.GetChild(i).gameObject);
            }
        }
    }
    public void GoToCheckOut()
    {
        int Counter = 0;
        int TotalPrice = 0;
        TotalObjectsInBuyPanel.Clear();
        if (BuyPanelParentOfBtns.childCount > 0)
        {
            ClearParentFromObjs();
        }
        for (int i = 0; i < TotalBtnlist.Count; i++)
        {
            if (TotalBtnlist[i].SelectedBool)
            {
                Counter += 1;
                TotalPrice += int.Parse(TotalBtnlist[i].PriceTxt.text);
                print(Counter);
                TotalObjectsInBuyPanel.Add(TotalBtnlist[i].gameObject);
            }
        }
        if (Counter > 0)
        {
            if (!UserRegisterationManager.instance.LoggedIn)
            {
                OpenMainPanel("ShowSignUpPanel");
            }
            else
            {
                TotalItemPriceCheckOut = 0;
                TotalSelectedInBuyPanel.Clear();
                StoreItemsPanel.SetActive(false);
                CheckOutBuyItemPanel.SetActive(true);
                for (int i = 0; i < TotalObjectsInBuyPanel.Count; i++)
                {
                    GameObject L_ItemBtnObj = Instantiate(BuyItemPrefab, BuyPanelParentOfBtns.transform);
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().PriceTxt.text = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().PriceTxt.text;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().CategoryTxt.text = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().CategoriesEnumVar.ToString();

                    // Add All Value from One Button to Buy Checkout Btn
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().assetLinkAndroid = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().assetLinkAndroid;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().assetLinkIos = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().assetLinkIos;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().assetLinkWindows = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().assetLinkWindows;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().createdAt = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().createdAt;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().createdBy = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().createdBy;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().iconLink = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().iconLink;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().id = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().id;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isFavourite = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isFavourite;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isOccupied = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isOccupied;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isPaid = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isPaid;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isPurchased = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isPurchased;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().name = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().name;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().price = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().price;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().categoryId = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().categoryId;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().subCategory = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().subCategory;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().updatedAt = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().updatedAt;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().itemTags = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().itemTags;
                    L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().CategoriesEnumVar = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().CategoriesEnumVar;
                    TotalSelectedInBuyPanel.Add(L_ItemBtnObj.gameObject);
                    TotalItemPriceCheckOut += int.Parse(L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().PriceTxt.text);
                }
                TotalPriceBuyPanelTxt.text = TotalItemPriceCheckOut.ToString();
                TotalItemsBuyPanelTxt.text = TotalObjectsInBuyPanel.Count.ToString();
            }
        }
    }
    public void BuyItems()
    {
        if (PlayerPrefs.GetInt("TotalCoins") < TotalItemPriceCheckOut)
        {
            print("Price is less than Selected Items");
        }
        else if (PlayerPrefs.GetInt("TotalCoins") >= TotalItemPriceCheckOut)
        {
            if (TotalObjectsInBuyPanel.Count > 0)
            {
                for (int i = 0; i < TotalObjectsInBuyPanel.Count; i++)
                {
                    EnumClass.CategoryEnum selectedEnum = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().CategoriesEnumVar;
                    switch (selectedEnum)
                    {
                        case EnumClass.CategoryEnum.Head:
                            {
                                if (CategorieslistHeads.Contains(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>()))
                                {
                                    int Getindex = CategorieslistHeads.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                    CategorieslistHeads[Getindex].price = "0";
                                    CategorieslistHeads[Getindex].isPaid = "true";
                                    CategorieslistHeads[Getindex].isPurchased = "true";
                                }
                                break;
                            }
                        case EnumClass.CategoryEnum.Face:
                            {
                                int Getindex = CategorieslistFace.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                CategorieslistFace[Getindex].price = "0";
                                CategorieslistFace[Getindex].isPaid = "true";
                                CategorieslistFace[Getindex].isPurchased = "true";


                                break;
                            }
                        case EnumClass.CategoryEnum.Inner:
                            {
                                int Getindex = CategorieslistInner.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                CategorieslistInner[Getindex].price = "0";
                                CategorieslistInner[Getindex].isPaid = "true";
                                CategorieslistInner[Getindex].isPurchased = "true";
                                break;
                            }
                        case EnumClass.CategoryEnum.Outer:
                            {
                                int Getindex = CategorieslistOuter.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                CategorieslistOuter[Getindex].price = "0";
                                CategorieslistOuter[Getindex].isPaid = "true";
                                CategorieslistOuter[Getindex].isPurchased = "true";
                                break;
                            }
                        case EnumClass.CategoryEnum.Accesary:
                            {
                                int Getindex = CategorieslistAccesary.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                CategorieslistAccesary[Getindex].price = "0";
                                CategorieslistAccesary[Getindex].isPaid = "true";
                                CategorieslistAccesary[Getindex].isPurchased = "true";
                                break;
                            }
                        case EnumClass.CategoryEnum.Bottom:
                            {
                                int Getindex = CategorieslistBottom.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                CategorieslistBottom[Getindex].price = "0";
                                CategorieslistBottom[Getindex].isPaid = "true";
                                CategorieslistBottom[Getindex].isPurchased = "true";
                                break;
                            }
                        case EnumClass.CategoryEnum.Socks:
                            {
                                int Getindex = CategorieslistSocks.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                CategorieslistSocks[Getindex].price = "0";
                                CategorieslistSocks[Getindex].isPaid = "true";
                                CategorieslistSocks[Getindex].isPurchased = "true";
                                break;
                            }
                        case EnumClass.CategoryEnum.Shoes:
                            {
                                int Getindex = CategorieslistShoes.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
                                CategorieslistShoes[Getindex].price = "0";
                                CategorieslistShoes[Getindex].isPaid = "true";
                                CategorieslistShoes[Getindex].isPurchased = "true";
                                break;
                            }
                    }
                }
                PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - TotalItemPriceCheckOut);
            }
            UpdateItemsDetails();
            CloseCheckOutPanel();
        }
    }
    void UpdateItemsDetails()
    {
        UpdateUserCoins();
        for (int i = 0; i < TotalObjectsInBuyPanel.Count; i++)
        {
            TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().DeSelectAfterBuying();
        }
        TotalObjectsInBuyPanel.Clear();
        // ShirtsSelection
        for (int i = 0; i < CategorieslistHeads.Count; i++)
        {
            CategorieslistHeads[i].UpdateValues();
        }
        // PantsSelection
        for (int i = 0; i < CategorieslistFace.Count; i++)
        {
            CategorieslistFace[i].UpdateValues();
        }
        // CategorieslistShoes
        for (int i = 0; i < CategorieslistInner.Count; i++)
        {
            CategorieslistInner[i].UpdateValues();
        }
        // CategorieslistHairs
        for (int i = 0; i < CategorieslistOuter.Count; i++)
        {
            CategorieslistOuter[i].UpdateValues();
        }
        // CategorieslistGlasses
        for (int i = 0; i < CategorieslistAccesary.Count; i++)
        {
            CategorieslistAccesary[i].UpdateValues();
        }
        // CategorieslistHats
        for (int i = 0; i < CategorieslistBottom.Count; i++)
        {
            CategorieslistBottom[i].UpdateValues();
        }
        //CategorieslistBags
        for (int i = 0; i < CategorieslistSocks.Count; i++)
        {
            CategorieslistSocks[i].UpdateValues();
        }
        //CategorieslistBags
        for (int i = 0; i < CategorieslistShoes.Count; i++)
        {
            CategorieslistShoes[i].UpdateValues();
        }
    }
    public void RemoveItemsFromCheckOut(GameObject itemDetailsBtn)
    {
        if (TotalSelectedInBuyPanel.Contains(itemDetailsBtn))
        {
            TotalSelectedInBuyPanel.Remove(itemDetailsBtn);
            UpdateCheckOutCash();
        }
        // TotalObjectsInBuyPanel
    }
    public void AddItemsFromCheckOut(GameObject itemDetailsBtn)
    {
        if (!TotalSelectedInBuyPanel.Contains(itemDetailsBtn))
        {
            TotalSelectedInBuyPanel.Add(itemDetailsBtn);
            UpdateCheckOutCash();
        }
        // TotalObjectsInBuyPanel
    }
    public void BuyItemsbyPurchaseAPI()
    {
        List<string> purchaseItemsIDs = new List<string>();
        if (PlayerPrefs.GetInt("TotalCoins") < TotalItemPriceCheckOut)
        {
            OpenMainPanel("LowCoinsPanel");
        }
        else if (PlayerPrefs.GetInt("TotalCoins") >= TotalItemPriceCheckOut)
        {
            if (TotalSelectedInBuyPanel.Count > 0)
            {
                for (int i = 0; i < TotalSelectedInBuyPanel.Count; i++)
                {
                    purchaseItemsIDs.Add(TotalSelectedInBuyPanel[i].GetComponent<ItemDetailBuyItem>().id.ToString());
                }
            }
            ArrayofBuyItems = purchaseItemsIDs.ToArray();
            SubmitPurchaseAPI(ArrayofBuyItems);
        }
    }
    public void UpdateCheckOutCash()
    {
        TotalItemPriceCheckOut = 0;
        if (TotalSelectedInBuyPanel.Count == 0)
        {
            TotalItemPriceCheckOut = 0;
            BuyBtnCheckOut.GetComponent<Button>().interactable = false;
        }
        else
        {
            BuyBtnCheckOut.GetComponent<Button>().interactable = true;
            for (int i = 0; i < TotalSelectedInBuyPanel.Count; i++)
            {
                TotalItemPriceCheckOut += int.Parse(TotalSelectedInBuyPanel[i].GetComponent<ItemDetailBuyItem>().PriceTxt.text);
            }
        }
        TotalPriceBuyPanelTxt.text = TotalItemPriceCheckOut.ToString();
        TotalItemsBuyPanelTxt.text = TotalSelectedInBuyPanel.Count.ToString();
    }
    public void CloseCheckOutPanel()
    {
        CheckOutBuyItemPanel.SetActive(false);
    }
    public class EnumClass : MonoBehaviour
    {
        public enum CategoryEnum
        {
            Head,
            Face,
            Inner,
            Outer,
            Accesary,
            Bottom,
            Socks,
            Shoes,
            HairAvatar,
            LipsAvatar,
            EyesAvatar,
            SkinToneAvatar,
            Presets
        }
    }

    // Purchase Item Starts Here
    private void SubmitPurchaseAPI(string[] TakeArrayofBuyItems)
    {
        var result = string.Join(",", TakeArrayofBuyItems);
        result = "[" + result + "]";
        ClassforPurchaseAPI purchaseCLassObj = new ClassforPurchaseAPI();
        string bodyJson = JsonUtility.ToJson(purchaseCLassObj.CreateTOJSON(result)); ;
        StartCoroutine(HitPurchaseAPI(ConstantsGod.API_BASEURL + ConstantsGod.PurchasedAPI, bodyJson));
    }
    IEnumerator HitPurchaseAPI(string url, string Jsondata)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        request.SendWebRequest();
        while(request.isDone)
        {
            yield return null;
        }
        ClassforPurchaseDataExtract PurchaseDataExtractObj = new ClassforPurchaseDataExtract();
        PurchaseDataExtractObj = PurchaseDataExtractObj.CreateFromJSON(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (PurchaseDataExtractObj.success == true)
                {
                    RefreshDefault();
                    SubmitUserDetailAPI();
                    SubmitDefaultAPI();
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                print("Error Accured " + request.error.ToUpper());
            }
            else
            {
                if (request.error != null)
                {
                    if (PurchaseDataExtractObj.success == false)
                    {
                        print("Hey success false " + PurchaseDataExtractObj.msg);
                    }
                }
            }
        }
        request.Dispose();

    }
    void RefreshDefault()
    {
        CategorieslistHeads.Clear();
        CategorieslistFace.Clear();
        CategorieslistInner.Clear();
        CategorieslistOuter.Clear();
        CategorieslistAccesary.Clear();
        CategorieslistBottom.Clear();
        CategorieslistSocks.Clear();
        CategorieslistShoes.Clear();
        CategorieslistHairs.Clear();
        CategorieslistLipsColor.Clear();
        CategorieslistSkinToneColor.Clear();
        CategorieslistEyesColor.Clear();
        TotalBtnlist.Clear();
        foreach (GameObject temp in itemButtonsPool)
        {
            temp.SetActive(false);
        }
        CloseCheckOutPanel();
        BuyCountertxt.text = "0";
    }
    [System.Serializable]
    public class ClassforPurchaseAPI
    {
        public string ItemIds;
        public ClassforPurchaseAPI CreateTOJSON(string jsonString)
        {
            ClassforPurchaseAPI myObj = new ClassforPurchaseAPI();
            myObj.ItemIds = jsonString;
            return myObj;
        }
    }
    [System.Serializable]
    public class ClassforPurchaseDataExtract
    {
        public bool success;
        public string data;
        public string msg;
        public ClassforPurchaseDataExtract CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassforPurchaseDataExtract>(jsonString);
        }
    }
    // Purchase Item End Here


    //  UserDetails Starts here ************************************************************************
    private void SubmitUserDetailAPI()
    {
        StartCoroutine(HitGetUserDetails(ConstantsGod.API_BASEURL + ConstantsGod.GetUserDetailsAPI, ""));
    }
    // Submit GetUser Details        
    IEnumerator HitGetUserDetails(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
            
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            ClassforUserDetails myObjectOfUserDetail = new ClassforUserDetails();
            myObjectOfUserDetail = myObjectOfUserDetail.CreateFromJSON(request.downloadHandler.text);

            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    if (PlayerPrefs.GetInt("IsChanged") == 0)
                    {
                        PlayerPrefs.SetInt("IsChanged", 1);
                        UndoSelection();
                        StartCoroutine(CharacterChange());
                    }

                    if (myObjectOfUserDetail.success == true)
                    {
                        decimal CoinsInDecimal = decimal.Parse(myObjectOfUserDetail.data.coins);
                        int Coinsint = (int)CoinsInDecimal;
                        PlayerPrefs.SetInt("TotalCoins", Coinsint);
                        UpdateUserCoins();
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    print(request.error.ToUpper());
                }
                else
                {
                    if (request.error != null)
                    {
                        if (myObjectOfUserDetail.success == false)
                        {
                            print("Hey success false " + myObjectOfUserDetail.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }

    [System.Serializable]
    public class ClassforUserDetails
    {
        public bool success;
        public JsondataOfUserDetails data;
        public string msg;
        public ClassforUserDetails CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassforUserDetails>(jsonString);
        }
    }

    [System.Serializable]
    public class JsondataOfUserDetails
    {
        public int id;
        public string name;
        public string dob;
        public string phoneNumber;
        public string email;
        public string avatar;
        public int role;
        public string coins;
        public bool isVerified;
        public bool isRegister;
        public bool isDeleted;
        public string createdAt;
        public string updatedAt;
        public UserProfileForUserDetails userProfile;
        public static JsondataOfUserDetails CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<JsondataOfUserDetails>(jsonString);
        }
    }
    [System.Serializable]
    public class UserProfileForUserDetails
    {
        public string id;
        public string userId;
        public string gender;
        public string job;
        public string country;
        public string bio;
        public string isDeleted;
        public string createdAt;
        public string updatedAt;
        public static JsondataOfUserDetails CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<JsondataOfUserDetails>(jsonString);
        }
    }
    //  UserDetails End here -------------------------------------------------------------

    //********** Get Guest Issues ******

    public void SubmitDefaultAPIForGuest()
    {
        StartCoroutine(HitDefaultAPIforGuest(ConstantsGod.API_BASEURL + ConstantsGod.GetDefaultAPI+"/"+APIBaseUrlChange.instance.apiversion+"/1/50", ""));
    }
    IEnumerator HitDefaultAPIforGuest(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            request.SendWebRequest();
            while(!request.isDone)
            {
                yield return null;
            }
            JsonDataObj = GetAllData(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    Debug.Log(request.downloadHandler.text);
                    if (JsonDataObj.success == true)
                    {
                        print("Success True All Default Data Fetched for Guest");
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    print("Network Error");
                }
                else
                {
                    if (request.error != null)
                    {
                        if (JsonDataObj.success == false)
                        {
                            print("Hey success false " + JsonDataObj.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }

    //  ******************************************** Get Default Starts Here ********************************************
    public void SubmitDefaultAPI()
    {
        StartCoroutine(HitDefaultAPI(ConstantsGod.API_BASEURL + ConstantsGod.GetDefaultAPI + "/" + APIBaseUrlChange.instance.apiversion + "/1/50", ""));
    }
    IEnumerator HitDefaultAPI(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
            request.SendWebRequest();
            while(!request.isDone)
            {
                yield return null;
            }
            JsonDataObj = GetAllData(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    Debug.Log(request.downloadHandler.text);
                    if (JsonDataObj.success == true)
                    {
                        print("Success True All Default Data Fetched");
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    print("Network Error");
                }
                else
                {
                    if (request.error != null)
                    {
                        if (JsonDataObj.success == false)
                        {
                            print("Hey success false " + JsonDataObj.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }

    public GetAllInfo GetAllData(string m_JsonData)
    {
        JsonDataObj = JsonUtility.FromJson<GetAllInfo>(m_JsonData);
        return JsonDataObj;
    }
    [System.Serializable]
    public class GetAllInfo
    {
        public bool success;
        public ItemClass data;
        public string msg;
    }
    [System.Serializable]
    public class ItemClass
    {
        public CategoryClass Items;
    }
    [System.Serializable]
    public class CategoryClass
    {
        public ToalClothItemsClass Cloth;
        public ToalAvatarItemsClass Avatar;
    }

    [System.Serializable]
    public class ToalClothItemsClass
    {
        public List<Head> Head;
        public List<Face> Face;
        public List<Inner> Inner;
        public List<Outer> Outer;
        public List<Accesary> Accesary;
        public List<Bottom> Bottom;
        public List<Socks> Socks;
        public List<Shoes> Shoes;
    }
    [System.Serializable]
    public class ToalAvatarItemsClass
    {
        public List<Hair> HairSelection;
        public List<FaceAvatar> FaceAvatarSelection;
        public List<EyeBrow> EyeBrowSelection;
        public List<Eyes> EyesSelection;
        public List<Nose> NoseSelection;
        public List<Lip> LipSelection;
        public List<Body> BodySelection;
        public List<Skin> SkinSelection;
    }
    // Cloth Customization

    // PantsSelection
    [System.Serializable]
    public class Head : ItemsParents
    {

    }
    //ShirtsSelection  
    [System.Serializable]
    public class Face : ItemsParents
    {

    }
    //HairsSelection 
    [System.Serializable]
    public class Inner : ItemsParents
    {

    }
    //ShoesSelection
    [System.Serializable]
    public class Outer : ItemsParents
    {


    }
    //Glasses
    [System.Serializable]
    public class Accesary : ItemsParents
    {
    }
    //Hats
    [System.Serializable]
    public class Bottom : ItemsParents
    {


    }
    //Bags
    [System.Serializable]
    public class Socks : ItemsParents
    {

    }
    //CategoriesDetails
    [System.Serializable]
    public class Shoes : ItemsParents
    {

    }

    // Avatar Customization

    // HairSelection
    [System.Serializable]
    public class Hair : ItemsParents
    {

    }
    // FaceAvatarSelection  
    [System.Serializable]
    public class FaceAvatar : ItemsParents
    {

    }
    // EyeBrowSelection 
    [System.Serializable]
    public class EyeBrow : ItemsParents
    {

    }
    //  EyesSelection
    [System.Serializable]
    public class Eyes : ItemsParents
    {


    }
    // NoseSelection
    [System.Serializable]
    public class Nose : ItemsParents
    {
    }
    // LipSelection
    [System.Serializable]
    public class Lip : ItemsParents
    {

    }
    //  BodySelection
    [System.Serializable]
    public class Body : ItemsParents
    {

    }
    // SkinSelection
    [System.Serializable]
    public class Skin : ItemsParents
    {

    }
    public class ItemsParents
    {
        public string assetLinkAndroid;
        public string assetLinkIos;
        public string assetLinkWindows;
        public string createdAt;
        public string createdBy;
        public string iconLink;
        public string id;
        public string isFavourite;
        public string isOccupied;
        public string isPaid;
        public string isPurchased;
        public string name;
        public string price;
        public string categoryId;
        public string subCategory;
        public string updatedAt;
        public string[] itemTags;
    }
    // ----------------------------------------- Get Default ENDS Here -----------------------------------------
    //  *************************************** Start Send Coins to Server ********************************************
    public void SubmitSendCoinstoServer(int getCoinsAfterInApp)
    {
        decimal CoinsInDecimal = Convert.ToDecimal(getCoinsAfterInApp);
        CoinsInDecimal = CoinsInDecimal + 0.00m;
        ClassofSendCoins sendcoinsObj = new ClassofSendCoins();
        string bodyJson = JsonUtility.ToJson(sendcoinsObj.CreateTOJSON(CoinsInDecimal.ToString()));
        StartCoroutine(HitSendCoinsAPI(ConstantsGod.API_BASEURL + ConstantsGod.SendCoinsAPI, bodyJson));
    }
    IEnumerator HitSendCoinsAPI(string url, string Jsondata)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));

        request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }
        ClassforSendCoinsExtraction SendCoinsDataExtractObj = new ClassforSendCoinsExtraction();
        SendCoinsDataExtractObj = SendCoinsDataExtractObj.CreateFromJSON(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (SendCoinsDataExtractObj.success == true)
                {
                    SubmitUserDetailAPI();
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                print("Error Accured " + request.error.ToUpper());
            }
            else
            {
                if (request.error != null)
                {
                    if (SendCoinsDataExtractObj.success == false)
                    {
                        print("Hey success false " + SendCoinsDataExtractObj.msg);
                    }
                }
            }
        }
        request.Dispose();
    }

    [System.Serializable]
    public class ClassofSendCoins
    {
        public string coins;
        public ClassofSendCoins CreateTOJSON(string jsonString)
        {
            ClassofSendCoins myObj = new ClassofSendCoins();
            myObj.coins = jsonString;
            return myObj;
        }
    }
    [System.Serializable]
    public class ClassforSendCoinsExtraction
    {
        public bool success;
        public string data;
        public string msg;
        public ClassforSendCoinsExtraction CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassforSendCoinsExtraction>(jsonString);
        }
    }

    //  *************************************** End Coins to Server ********************************************
    public EnumClass.CategoryEnum TempEnumVar;

    public void PutDataInOurAPPNewAPI()
    {
        if (itemLoading != null)
            StopCoroutine(itemLoading);
        itemLoading = StartCoroutine(PutDataInOurAPPNewAPICoroutine());

    }
    public IEnumerator PutDataInOurAPPNewAPICoroutine()
    {
        yield return null;
        RefreshDefault();
        List<ItemDetail> TempitemDetail;
        TempitemDetail = new List<ItemDetail>();
        Transform TempSubcategoryParent = null;
        switch (IndexofPanel)
        {
            case 0:
                {
                    //TempSubcategoryParent = ParentOfBtnsForHeads;
                    //TempEnumVar = EnumClass.CategoryEnum.Head;
                    break;
                }
            case 1:
                {
                    //TempSubcategoryParent = ParentOfBtnsForFace;
                    //TempEnumVar = EnumClass.CategoryEnum.Face;

                    break;
                }
            case 2:
                {
                    //TempSubcategoryParent = ParentOfBtnsForInner;
                    //TempEnumVar = EnumClass.CategoryEnum.Inner;

                    break;
                }
            case 3:
                {
                    TempSubcategoryParent = ParentOfBtnsForOuter;
                    TempEnumVar = EnumClass.CategoryEnum.Outer;
                    break;
                }
            case 4:
                {
                    TempSubcategoryParent = ParentOfBtnsForAccesary;
                    TempEnumVar = EnumClass.CategoryEnum.Accesary;
                    break;
                }
            case 5:
                {
                    TempSubcategoryParent = ParentOfBtnsForBottom;
                    TempEnumVar = EnumClass.CategoryEnum.Bottom;
                    break;
                }
            case 6:
                {
                    //TempSubcategoryParent = ParentOfBtnsForSocks;
                    //TempEnumVar = EnumClass.CategoryEnum.Socks;
                    break;
                }
            case 7:
                {
                    TempSubcategoryParent = ParentOfBtnsForShoes;
                    TempEnumVar = EnumClass.CategoryEnum.Shoes;
                    break;
                }
            case 8:
                {
                    TempSubcategoryParent = ParentOfBtnsAvatarHairs;
                    TempEnumVar = EnumClass.CategoryEnum.HairAvatar;
                    break;
                }
            case 11:
                {
                    TempSubcategoryParent = ParentOfBtnsCustomEyes;
                    TempEnumVar = EnumClass.CategoryEnum.EyesAvatar;
                    break;
                }
            case 13:
                {
                    TempSubcategoryParent = ParentOfBtnsCustomLips;
                    TempEnumVar = EnumClass.CategoryEnum.LipsAvatar;
                    break;
                }
            case 15:
                {
                    TempSubcategoryParent = ParentOfBtnsCustomSkin;
                    TempEnumVar = EnumClass.CategoryEnum.SkinToneAvatar;
                    break;
                }
            case 16:
                {
                    TempSubcategoryParent = ParentOfBtnsCustomSkin;
                    TempEnumVar = EnumClass.CategoryEnum.Presets;
                    break;
                }
        }
        if (TempSubcategoryParent != null && TempSubcategoryParent.childCount == 0)
        {
            //return;

            //if(dataListOfItems.Count > itemButtonsPool.Count)
            //{
            //    while(dataListOfItems.Count > itemButtonsPool.Count)
            //    {
            //        itemButtonsPool.Add(Instantiate(ItemsBtnPrefab));
            //    }
            //}
            //foreach(GameObject temp in itemButtonsPool)
            //{
            //    temp.SetActive(false);
            //}
            // HeadSelection
            for (int i = 0; i < dataListOfItems.Count; i++)
            {
                GameObject L_ItemBtnObj = Instantiate(ItemsBtnPrefab, TempSubcategoryParent.transform);
                //GameObject L_ItemBtnObj = itemButtonsPool[i];
                L_ItemBtnObj.transform.parent = TempSubcategoryParent.transform;
                L_ItemBtnObj.transform.localScale = new Vector3(1, 1, 1);
                //AWAIS//
                //if (dataListOfItems[i].iconLink != null)
                //    yield return StartCoroutine(addsprite(L_ItemBtnObj.GetComponent<ItemDetail>()._iconImg, dataListOfItems[i].iconLink));
                ItemDetail abc = L_ItemBtnObj.GetComponent<ItemDetail>();
                //AWAIS//
                abc.assetLinkAndroid = dataListOfItems[i].assetLinkAndroid;
                abc.assetLinkIos = dataListOfItems[i].assetLinkIos;
                abc.assetLinkWindows = dataListOfItems[i].assetLinkWindows;
                abc.createdAt = dataListOfItems[i].createdAt;
                abc.createdBy = dataListOfItems[i].createdBy;
                abc.iconLink = dataListOfItems[i].iconLink;
                abc.id = dataListOfItems[i].id.ToString();
                abc.isFavourite = dataListOfItems[i].isFavourite.ToString();
                abc.isOccupied = dataListOfItems[i].isOccupied.ToString();
                abc.isPaid = dataListOfItems[i].isPaid.ToString();
                abc.isPurchased = dataListOfItems[i].isPurchased.ToString();
                abc.name = dataListOfItems[i].name;
                abc.price = dataListOfItems[i].price;
                abc.categoryId = dataListOfItems[i].categoryId.ToString();
                abc.subCategory = dataListOfItems[i].subCategoryId.ToString();
                abc.isDeleted = dataListOfItems[i].isDeleted;
                abc.updatedAt = dataListOfItems[i].updatedAt;
                abc.itemTags = dataListOfItems[i].itemTags;
                abc.MyIndex = i;
                abc.CategoriesEnumVar = TempEnumVar;
                TempitemDetail.Add(abc);
                L_ItemBtnObj.SetActive(true);
                if (abc.transform.parent.gameObject.activeSelf)
                {

                    abc.StartRun();
                    abc.enableUpdate = true;
                }
                else
                {
                    abc.enableUpdate = true;
                }
            }

            if (TempitemDetail.Count > 0)
            {
                switch (TempEnumVar)
                {
                    case EnumClass.CategoryEnum.Head:
                        {
                            CategorieslistHeads = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.Face:
                        {
                            CategorieslistFace = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.Inner:
                        {
                            CategorieslistInner = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.Outer:
                        {
                            CategorieslistOuter = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.Accesary:
                        {
                            CategorieslistAccesary = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.Bottom:
                        {
                            CategorieslistBottom = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.Socks:
                        {
                            CategorieslistSocks = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.Shoes:
                        {
                            CategorieslistShoes = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.HairAvatar:
                        {
                            CategorieslistHairs = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.EyesAvatar:
                        {
                            CategorieslistEyesColor = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.LipsAvatar:
                        {
                            CategorieslistLipsColor = TempitemDetail;
                            break;
                        }
                    case EnumClass.CategoryEnum.SkinToneAvatar:
                        {
                            CategorieslistSkinToneColor = TempitemDetail;
                            break;
                        }
                }

                if (buttonIndex != -1)
                {
                    Debug.Log("Panel Selected: " + panelIndex);
                    UpdateStoreSelection(buttonIndex);
                }
            }
        }

        //else
        //{
        //    UpdateStoreSelection(0);
        //}
    }

    //UNDO REDO FUNCTIONALITY------------------

    public List<UndoRedoDataClass> UndoRedoList = new List<UndoRedoDataClass>();
    public int CurrentIndex = -1;
    [Serializable]
    public class UndoRedoDataClass
    {
        public Item ClothTex_Item = new Item();
    }

    IEnumerator CharacterChange()
    {
        yield return new WaitForSeconds(3.5f);

        UpdateStoreSelection(0);
    }

    public void UndoFunc()
    {
        UndoSelection();
        RedoBtn.GetComponent<Button>().interactable = true;
        StoreManager.instance.SaveStoreBtn.SetActive(true);
        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
        StoreManager.instance.GreyRibbonImage.SetActive(false);
        StoreManager.instance.WhiteRibbonImage.SetActive(true);

        if (CurrentIndex != 0)
        {
            if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType != UndoRedoList[CurrentIndex - 1].ClothTex_Item.ItemType)
                CurrentIndex--;
            else
                CurrentIndex--;
        }
        else
        {
            CurrentIndex--;
        }

        if (CurrentIndex < 0)
            CurrentIndex = 0;


        if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
        {
            _DownloadRigClothes.BindExistingClothes(UndoRedoList[CurrentIndex].ClothTex_Item.ItemType, UndoRedoList[CurrentIndex].ClothTex_Item.ItemName);

            if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip")
            {
                XanaConstants.xanaConstants.lipColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
            }

            else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes")
            {
                XanaConstants.xanaConstants.eyeColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
            }

            else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
            {
                XanaConstants.xanaConstants.skinColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
            }
        }
        else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "BodyFat")
        {

            CharacterCustomizationManager.Instance.UpdateChBodyShape(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID);
            XanaConstants.xanaConstants.bodyNumber = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID;
        }
        else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Preset")
        {
            if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "Zero")
            {
                GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
            }
            else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "One")
            {
                GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID, 100);
                GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
            }
            else
            {
                UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().CustomizationTriggerTwo();
            }
            GameObject tmp = UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab;

            if (tmp)
            {
                if (tmp.transform.IsChildOf(ParentOfBtnsAvatarFace))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.faceIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.faceIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyeBrows))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.eyeBrowIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.eyeBrowIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyes))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.eyeIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.eyeIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarNose))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.noseIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.noseIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarLips))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.lipIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.lipIndex = 0;
                }
            }

        }
        else
        {
            GameManager.Instance.EquipUiObj.ChangeCostume(UndoRedoList[CurrentIndex].ClothTex_Item.ItemName.ToLower());
        }

        if (CurrentIndex == 0)
        {
            UndoBtn.GetComponent<Button>().interactable = false;
        }

        if (ParentOfBtnsCustomEyes.gameObject.activeInHierarchy)
            UpdateColor(XanaConstants.xanaConstants.currentButtonIndex);
        else if(ParentOfBtnsCustomLips.gameObject.activeInHierarchy)
            UpdateColor(XanaConstants.xanaConstants.currentButtonIndex);
        else if(ParentOfBtnsCustomSkin.gameObject.activeInHierarchy)
            UpdateColor(XanaConstants.xanaConstants.currentButtonIndex);
        else
            UpdateStoreSelection(XanaConstants.xanaConstants.currentButtonIndex);

        
    }
    public void RedoFunc()
    {
        UndoSelection();
        StoreManager.instance.SaveStoreBtn.SetActive(true);
        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
        StoreManager.instance.GreyRibbonImage.SetActive(false);
        StoreManager.instance.WhiteRibbonImage.SetActive(true);

        if (CurrentIndex < UndoRedoList.Count - 1)
        {
            if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType != UndoRedoList[CurrentIndex + 1].ClothTex_Item.ItemType)
                CurrentIndex++;
            else
                CurrentIndex++;
        }

        else
        {
            CurrentIndex++;
        }

        if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
        {
            _DownloadRigClothes.BindExistingClothes(UndoRedoList[CurrentIndex].ClothTex_Item.ItemType, UndoRedoList[CurrentIndex].ClothTex_Item.ItemName);

            if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip")
            {
                XanaConstants.xanaConstants.lipColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
            }

            else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes")
            {
                XanaConstants.xanaConstants.eyeColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
            }

            else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
            {
                XanaConstants.xanaConstants.skinColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
            }
        }
        else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "BodyFat")
        {
            CharacterCustomizationManager.Instance.UpdateChBodyShape(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID);
            XanaConstants.xanaConstants.bodyNumber = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID;
        }
        else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Preset")
        {
            if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "Zero")
            {
                GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
            }
            else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "One")
            {
                GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID, 100);
                GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
            }
            else
            {
                UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().CustomizationTriggerTwo();
            }

            GameObject tmp = UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab;

            if (tmp)
            {
                if (tmp.transform.IsChildOf(ParentOfBtnsAvatarFace))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.faceIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.faceIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyeBrows))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.eyeBrowIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.eyeBrowIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyes))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.eyeIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.eyeIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarNose))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.noseIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.noseIndex = 0;
                }

                else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarLips))
                {
                    if (tmp.GetComponent<BodyCustomizationTrigger>())
                        XanaConstants.xanaConstants.lipIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                    else
                        XanaConstants.xanaConstants.lipIndex = 0;
                }
            }
        }
        else
        {
            GameManager.Instance.EquipUiObj.ChangeCostume(UndoRedoList[CurrentIndex].ClothTex_Item.ItemName.ToLower());
        }

        if (CurrentIndex == UndoRedoList.Count - 1)
        {
            RedoBtn.GetComponent<Button>().interactable = false;
        }

        UndoBtn.GetComponent<Button>().interactable = true;

        if (ParentOfBtnsCustomEyes.gameObject.activeInHierarchy)
            UpdateColor(XanaConstants.xanaConstants.currentButtonIndex);
        else if (ParentOfBtnsCustomLips.gameObject.activeInHierarchy)
            UpdateColor(XanaConstants.xanaConstants.currentButtonIndex);
        else if (ParentOfBtnsCustomSkin.gameObject.activeInHierarchy)
            UpdateColor(XanaConstants.xanaConstants.currentButtonIndex);
        else
            UpdateStoreSelection(XanaConstants.xanaConstants.currentButtonIndex);
    }

    public void UpdateStoreSelection(int index)
    {
        if(index==8 && panelIndex == 1)
        {
            if (PlayerPrefs.GetInt("FirstTimeInstall") == 0)
            {
                try {
                    if (PresetData_Jsons.lastSelectedPreset == null)
                    {
                        PlayerPrefs.SetInt("PresetValue", -1);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("PresetValue", Int32.Parse(PresetData_Jsons.lastSelectedPreset.transform.parent.name));
                    }
                }
                catch
                {

                }
                PlayerPrefs.SetInt("FirstTimeInstall", 1);
                
            }
            Debug.LogError("Preset:" + PlayerPrefs.GetInt("PresetValue"));
            if(PlayerPrefs.GetInt("PresetValue") >= 0)
                PresetData_Jsons.lastSelectedPreset = presets[PlayerPrefs.GetInt("PresetValue")].transform.GetChild(0).gameObject;
            SelectSavedPreset();
        }
        switch (index)
        {
            case 0:
                if (XanaConstants.xanaConstants.hair != "")
                {
                    for (int i = 0; i < ParentOfBtnsAvatarHairs.transform.childCount; i++)
                    {
                        if (ParentOfBtnsAvatarHairs.transform.GetChild(i).GetComponent<ItemDetail>().id == XanaConstants.xanaConstants.hair)
                        {
                            ParentOfBtnsAvatarHairs.transform.GetChild(i).GetComponent<Image>().enabled = true;
                            XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarHairs.transform.GetChild(i).gameObject;
                            XanaConstants.xanaConstants.avatarStoreSelection[0] = ParentOfBtnsAvatarHairs.transform.GetChild(i).gameObject;

                            CheckForItemDetail(XanaConstants.xanaConstants.hair, 3);

                            break;
                        }
                    }
                }
                break;

            case 1:
                if (!XanaConstants.xanaConstants.isFaceMorphed)
                {
                    if (XanaConstants.xanaConstants.faceIndex != 0)
                    {
                        for (int i = 0; i < faceAvatarButton.Length; i++)
                        {
                            if (faceAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne == XanaConstants.xanaConstants.faceIndex)
                            {
                                faceAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                XanaConstants.xanaConstants._lastClickedBtn = faceAvatarButton[i];
                                XanaConstants.xanaConstants.avatarStoreSelection[1] = faceAvatarButton[i];

                                CheckForAvatarBtn(XanaConstants.xanaConstants.faceIndex, "face");

                                break;
                            }
                        }
                    }

                    else
                    {

                        int childNumber = ParentOfBtnsAvatarFace.transform.childCount - 1;

                        ParentOfBtnsAvatarFace.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarFace.transform.GetChild(childNumber).gameObject;
                        XanaConstants.xanaConstants.avatarStoreSelection[1] = ParentOfBtnsAvatarFace.transform.GetChild(childNumber).gameObject;

                        CheckForAvatarBtn(0, "face");
                    }
                }

                else
                {
                    faceTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                    if (XanaConstants.xanaConstants._lastClickedBtn)
                    {
                        if (XanaConstants.xanaConstants._lastClickedBtn.name == faceTapButton.name)
                        {
                            XanaConstants.xanaConstants._lastClickedBtn = null;
                            XanaConstants.xanaConstants.avatarStoreSelection[1] = null;
                        }
                    }

                    saveButtonPressed = false;
                }

                break;

            case 2:
                if (!XanaConstants.xanaConstants.isEyebrowMorphed)
                {
                    if (XanaConstants.xanaConstants.eyeBrowIndex != 0)
                    {
                        for (int i = 0; i < eyeBrowsAvatarButton.Length; i++)
                        {
                            if (eyeBrowsAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne == XanaConstants.xanaConstants.eyeBrowIndex)
                            {
                                eyeBrowsAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                XanaConstants.xanaConstants._lastClickedBtn = eyeBrowsAvatarButton[i];
                                XanaConstants.xanaConstants.avatarStoreSelection[2] = eyeBrowsAvatarButton[i];

                                CheckForAvatarBtn(XanaConstants.xanaConstants.eyeBrowIndex, "eyeBrow");

                                break;
                            }
                        }
                    }

                    else
                    {

                        int childNumber = ParentOfBtnsAvatarEyeBrows.transform.childCount - 1;

                        ParentOfBtnsAvatarEyeBrows.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarEyeBrows.transform.GetChild(childNumber).gameObject;
                        XanaConstants.xanaConstants.avatarStoreSelection[2] = ParentOfBtnsAvatarEyeBrows.transform.GetChild(childNumber).gameObject;

                        CheckForAvatarBtn(0, "eyeBrow");
                    }
                }

                else
                {
                    eyeBrowTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                    if (XanaConstants.xanaConstants._lastClickedBtn)
                    {
                        if (XanaConstants.xanaConstants._lastClickedBtn.name == eyeBrowTapButton.name)
                        {
                            XanaConstants.xanaConstants._lastClickedBtn = null;
                            XanaConstants.xanaConstants.avatarStoreSelection[2] = null;
                        }
                    }

                    saveButtonPressed = false;
                }

                break;

            case 3:
                if (panelIndex == 1)
                {
                    if (!XanaConstants.xanaConstants.isEyeMorphed)
                    {
                        if (XanaConstants.xanaConstants.eyeIndex != 0)
                        {
                            for (int i = 0; i < eyeAvatarButton.Length; i++)
                            {
                                if (eyeAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne == XanaConstants.xanaConstants.eyeIndex)
                                {
                                    eyeAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    XanaConstants.xanaConstants._lastClickedBtn = eyeAvatarButton[i];
                                    XanaConstants.xanaConstants.avatarStoreSelection[3] = eyeAvatarButton[i];

                                    CheckForAvatarBtn(XanaConstants.xanaConstants.eyeIndex, "eye");

                                    break;
                                }
                            }
                        }

                        else
                        {

                            int childNumber = ParentOfBtnsAvatarEyes.transform.childCount - 1;

                            ParentOfBtnsAvatarEyes.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                            XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarEyes.transform.GetChild(childNumber).gameObject;
                            XanaConstants.xanaConstants.avatarStoreSelection[3] = ParentOfBtnsAvatarEyes.transform.GetChild(childNumber).gameObject;

                            CheckForAvatarBtn(0, "eye");
                        }
                    }

                    else
                    {
                        eyeTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                        if (XanaConstants.xanaConstants._lastClickedBtn)
                        {
                            if (XanaConstants.xanaConstants._lastClickedBtn.name == eyeTapButton.name)
                            {
                                XanaConstants.xanaConstants._lastClickedBtn = null;
                                XanaConstants.xanaConstants.avatarStoreSelection[3] = null;
                            }
                        }

                        saveButtonPressed = false;
                    }
                }

                else
                {
                    if (XanaConstants.xanaConstants.shirt != "")
                    {
                        for (int i = 0; i < ParentOfBtnsForOuter.transform.childCount; i++)
                        {
                            if (ParentOfBtnsForOuter.transform.GetChild(i).GetComponent<ItemDetail>().id == XanaConstants.xanaConstants.shirt)
                            {
                                ParentOfBtnsForOuter.transform.GetChild(i).GetComponent<Image>().enabled = true;
                                XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsForOuter.transform.GetChild(i).gameObject;
                                XanaConstants.xanaConstants.wearableStoreSelection[0] = ParentOfBtnsForOuter.transform.GetChild(i).gameObject;

                                CheckForItemDetail(XanaConstants.xanaConstants.shirt, 1);

                                break;
                            }
                        }
                    }
                }

                break;

            case 4:
                if (!XanaConstants.xanaConstants.isNoseMorphed)
                {
                    if (XanaConstants.xanaConstants.noseIndex != 0)
                    {
                        for (int i = 0; i < noseAvatarButton.Length; i++)
                        {
                            if (noseAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne == XanaConstants.xanaConstants.noseIndex)
                            {
                                noseAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                XanaConstants.xanaConstants._lastClickedBtn = noseAvatarButton[i];
                                XanaConstants.xanaConstants.avatarStoreSelection[4] = noseAvatarButton[i];

                                CheckForAvatarBtn(XanaConstants.xanaConstants.noseIndex, "nose");

                                break;
                            }
                        }
                    }

                    else
                    {

                        int childNumber = ParentOfBtnsAvatarNose.transform.childCount - 1;

                        ParentOfBtnsAvatarNose.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarNose.transform.GetChild(childNumber).gameObject;
                        XanaConstants.xanaConstants.avatarStoreSelection[4] = ParentOfBtnsAvatarNose.transform.GetChild(childNumber).gameObject;

                        CheckForAvatarBtn(0, "nose");
                    }
                }

                else
                {
                    noseTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                    if (XanaConstants.xanaConstants._lastClickedBtn)
                    {
                        if (XanaConstants.xanaConstants._lastClickedBtn.name == noseTapButton.name)
                        {
                            XanaConstants.xanaConstants._lastClickedBtn = null;
                            XanaConstants.xanaConstants.avatarStoreSelection[4] = null;
                        }
                    }

                    saveButtonPressed = false;
                }

                break;

            case 5:
                if (panelIndex == 1)
                {
                    if (!XanaConstants.xanaConstants.isLipMorphed)
                    {
                        if (XanaConstants.xanaConstants.lipIndex != -0)
                        {
                            for (int i = 0; i < lipAvatarButton.Length; i++)
                            {
                                if (lipAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne == XanaConstants.xanaConstants.lipIndex)
                                {
                                    lipAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    XanaConstants.xanaConstants._lastClickedBtn = lipAvatarButton[i];
                                    XanaConstants.xanaConstants.avatarStoreSelection[5] = lipAvatarButton[i];

                                    CheckForAvatarBtn(XanaConstants.xanaConstants.lipIndex, "lip");

                                    break;
                                }
                            }
                        }

                        else
                        {

                            int childNumber = ParentOfBtnsAvatarLips.transform.childCount - 1;

                            ParentOfBtnsAvatarLips.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                            XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarLips.transform.GetChild(childNumber).gameObject;
                            XanaConstants.xanaConstants.avatarStoreSelection[5] = ParentOfBtnsAvatarLips.transform.GetChild(childNumber).gameObject;

                            CheckForAvatarBtn(0, "lip");
                        }
                    }

                    else
                    {
                        lipTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                        if (XanaConstants.xanaConstants._lastClickedBtn)
                        {
                            if (XanaConstants.xanaConstants._lastClickedBtn.name == lipTapButton.name)
                            {
                                XanaConstants.xanaConstants._lastClickedBtn = null;
                                XanaConstants.xanaConstants.avatarStoreSelection[5] = null;
                            }
                        }

                        saveButtonPressed = false;
                    }
                }

                else
                {
                    if (XanaConstants.xanaConstants.pants != "")
                    {
                        Debug.Log(ParentOfBtnsForOuter.transform.childCount);

                        for (int i = 0; i < ParentOfBtnsForBottom.transform.childCount; i++)
                        {
                            if (ParentOfBtnsForBottom.transform.GetChild(i).GetComponent<ItemDetail>().id == XanaConstants.xanaConstants.pants)
                            {
                                ParentOfBtnsForBottom.transform.GetChild(i).GetComponent<Image>().enabled = true;
                                XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsForBottom.transform.GetChild(i).gameObject;
                                XanaConstants.xanaConstants.wearableStoreSelection[1] = ParentOfBtnsForBottom.transform.GetChild(i).gameObject;

                                CheckForItemDetail(XanaConstants.xanaConstants.pants, 0);

                                break;
                            }
                        }
                    }
                }

                break;

            case 6:
                if (XanaConstants.xanaConstants.bodyNumber != -1)
                {
                    for (int i = 0; i < ParentOfBtnsAvatarBody.transform.childCount; i++)
                    {
                        if (ParentOfBtnsAvatarBody.transform.GetChild(i).GetComponent<AvatarBtn>()._Bodyint == XanaConstants.xanaConstants.bodyNumber)
                        {
                            ParentOfBtnsAvatarBody.transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                            XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarBody.transform.GetChild(i).gameObject;
                            XanaConstants.xanaConstants.avatarStoreSelection[6] = ParentOfBtnsAvatarBody.transform.GetChild(i).gameObject;
                            break;
                        }
                    }
                }

                break;

            case 7:
                if (panelIndex == 0)
                {
                    if (XanaConstants.xanaConstants.shoes != "")
                    {
                        for (int i = 0; i < ParentOfBtnsForShoes.transform.childCount; i++)
                        {
                            if (ParentOfBtnsForShoes.transform.GetChild(i).GetComponent<ItemDetail>().id == XanaConstants.xanaConstants.shoes)
                            {
                                ParentOfBtnsForShoes.transform.GetChild(i).GetComponent<Image>().enabled = true;
                                XanaConstants.xanaConstants._lastClickedBtn = ParentOfBtnsForShoes.transform.GetChild(i).gameObject;
                                XanaConstants.xanaConstants.wearableStoreSelection[2] = ParentOfBtnsForShoes.transform.GetChild(i).gameObject;

                                CheckForItemDetail(XanaConstants.xanaConstants.shoes, 2);

                                break;
                            }
                        }
                    }
                }

                break;
        }
    }

    public void UndoSelection()
    {
        if(PlayerPrefs.GetInt("PresetValue")>0 && XanaConstants.xanaConstants.currentButtonIndex==8 && panelIndex == 1)
        {
            presets[PlayerPrefs.GetInt("PresetValue")].transform.GetChild(0).gameObject.SetActive(false);
            
        }
        for (int i = 0; i < XanaConstants.xanaConstants.avatarStoreSelection.Length; i++)
        {
            if (XanaConstants.xanaConstants.avatarStoreSelection[i])
            {
                if (XanaConstants.xanaConstants.avatarStoreSelection[i].GetComponent<ItemDetail>())
                {
                    XanaConstants.xanaConstants.avatarStoreSelection[i].GetComponent<Image>().enabled = false;
                }

                else
                {
                    XanaConstants.xanaConstants.avatarStoreSelection[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }

                XanaConstants.xanaConstants.avatarStoreSelection[i] = null;
            }
        }

        for (int i = 0; i < XanaConstants.xanaConstants.wearableStoreSelection.Length; i++)
        {
            if (XanaConstants.xanaConstants.wearableStoreSelection[i])
            {
                if (XanaConstants.xanaConstants.wearableStoreSelection[i].GetComponent<ItemDetail>())
                {
                    XanaConstants.xanaConstants.wearableStoreSelection[i].GetComponent<Image>().enabled = false;
                }

                else
                {
                    XanaConstants.xanaConstants.wearableStoreSelection[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }

                XanaConstants.xanaConstants.wearableStoreSelection[i] = null;
            }
        }

        for (int i = 0; i < XanaConstants.xanaConstants.colorSelection.Length; i++)
        {
            if (XanaConstants.xanaConstants.colorSelection[i])
            {
                if (XanaConstants.xanaConstants.colorSelection[i].GetComponent<ItemDetail>())
                {
                    XanaConstants.xanaConstants.colorSelection[i].GetComponent<Image>().enabled = false;
                }

                else
                {
                    XanaConstants.xanaConstants.colorSelection[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }

                XanaConstants.xanaConstants.colorSelection[i] = null;
            }
        }
    }

    public void ResetMorphBooleanValues()
    {
        XanaConstants.xanaConstants.isFaceMorphed = SavaCharacterProperties.instance.SaveItemList.faceMorphed;
        XanaConstants.xanaConstants.isEyebrowMorphed = SavaCharacterProperties.instance.SaveItemList.eyeBrowMorphed;
        XanaConstants.xanaConstants.isEyeMorphed = SavaCharacterProperties.instance.SaveItemList.eyeMorphed;
        XanaConstants.xanaConstants.isNoseMorphed = SavaCharacterProperties.instance.SaveItemList.noseMorphed;
        XanaConstants.xanaConstants.isLipMorphed = SavaCharacterProperties.instance.SaveItemList.lipMorphed;

        if (XanaConstants.xanaConstants._lastClickedBtn)
        {
            if (XanaConstants.xanaConstants._lastClickedBtn.GetComponent<AvatarBtn>())
                XanaConstants.xanaConstants._lastClickedBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

           // XanaConstants.xanaConstants._lastClickedBtn = null;

            if (XanaConstants.xanaConstants.currentButtonIndex >= 0 && XanaConstants.xanaConstants.currentButtonIndex < XanaConstants.xanaConstants.avatarStoreSelection.Length)
                XanaConstants.xanaConstants.avatarStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = null;
        }

        if (!ParentOfBtnsCustomEyes.gameObject.activeSelf && !ParentOfBtnsCustomLips.gameObject.activeSelf && !ParentOfBtnsCustomSkin.gameObject.activeSelf)
            UpdateStoreSelection(XanaConstants.xanaConstants.currentButtonIndex);

        else
            OnColorButtonClicked(XanaConstants.xanaConstants.currentButtonIndex);
    }

    public void DisableColorPanels()
    {
        ParentOfBtnsCustomEyes.gameObject.SetActive(false);
        ParentOfBtnsCustomLips.gameObject.SetActive(false);
        ParentOfBtnsCustomSkin.gameObject.SetActive(true);
    }

    public void CheckForItemDetail(string currentId, int idIndex)
    {
        if (!saveButtonPressed)
        {
            if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
            {
                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                if (_CharacterData.myItemObj.Count != 0)
                {
                    if (currentId == _CharacterData.myItemObj[idIndex].ItemID.ToString())
                    {
                        //ActivateSaveButton(false);
                    }

                    else
                    {
                        //ActivateSaveButton(true);
                    }
                }
            }
        }

        else
        {
            saveButtonPressed = false;
        }
    }

    public void CheckForAvatarBtn(int currentId, string itemType)
    {
        if (!saveButtonPressed)
        {
            if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
            {
                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                bool itemIsSaved = false;

                switch (itemType)
                {
                    case "face":
                        if (currentId == _CharacterData.FacePresets.f_BlendShapeOne)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "eyeBrow":

                        if (currentId == _CharacterData.EyeBrowPresets.f_BlendShapeOne)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "eye":
                        if (currentId == _CharacterData.EyePresets.f_BlendShapeOne)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "nose":
                        if (currentId == _CharacterData.NosePresets.f_BlendShapeOne)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "lip":
                        if (currentId == _CharacterData.LipsPresets.f_BlendShapeOne)
                        {
                            itemIsSaved = true;
                        }
                        break;
                }

                if (itemIsSaved)
                {
                    //ActivateSaveButton(false);
                }

                else
                {
                    //ActivateSaveButton(true);
                }
            }
        }

        else
        {
            saveButtonPressed = false;
        }
    }

    public void ActivateSaveButton(bool activate)
    {
        if (!activate)
        {
            SaveStoreBtn.SetActive(true);
            SaveStoreBtn.GetComponent<Button>().interactable = false;
            SaveStoreBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            GreyRibbonImage.SetActive(true);
            WhiteRibbonImage.SetActive(false);
        }

        else
        {
            SaveStoreBtn.GetComponent<Button>().interactable = true;
            SaveStoreBtn.SetActive(true);
            SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
            GreyRibbonImage.SetActive(false);
            WhiteRibbonImage.SetActive(true);
        }
    }
}