using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EmoteFilterManager : MonoBehaviour
{
    public Button GestureBtn;
    public Button PoseBtn;
    public GameObject popUpPenal;
    private GameObject animObject;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public string animationTabName;
    public string animationTabNameLang;
    private Sprite sprite;
    public bool taboneclick = false;
    public bool tabtwoclick = false;
    public GameObject NoDataFound;
    public GameObject ScrollViewAnimation;
    public int Counter = 0;
    public List<Button> buttonList = new List<Button>();
    private int CounterValue=0;
    private bool onceforapi = false;
    public GameObject progressbar;
    private bool valueget=false;
    public static bool TouchDisable = false;
    public  AnimationDetails bean;
    public Color[] emoteBGs;


    private void OnEnable()
    {
        ResetHighligt();
        if(GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnEmoteSelectionClose += OnEmoteSelectionClose;
    }

    public void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnEmoteSelectionClose -= OnEmoteSelectionClose;
    }


    // Start is called before the first frame update
    void Start()
    {
        animationTabName = "Dance";
        animationTabNameLang = "Dance";
        getLocalStorageEmote();
    }

    // Update is called once per frame
    void Update()
    {
        HideIfClickedOutside(popUpPenal);
    }

    public void GusterBtnClick()
    {
        Debug.Log("local manage====");
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        //EmoteAnimationPlay.Instance.alreadyRuning = true;

        taboneclick = true;
        tabtwoclick = false;
        NoDataFound.SetActive(false);
        ScrollViewAnimation.SetActive(true);
        animationTabName = GestureBtn.GetComponent<Text>().text;
        animationTabNameLang = GestureBtn.transform.name;
        GestureBtn.GetComponent<Text>().color = Color.black;
        PoseBtn.GetComponent<Text>().color = new Color32(142, 142, 142, 255);
        callobjects();


    }

    public void PoseBtnClick()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        tabtwoclick = true;
        taboneclick = false;
        NoDataFound.SetActive(false);
        ScrollViewAnimation.SetActive(true);
        animationTabName = PoseBtn.GetComponent<Text>().text;
        animationTabNameLang = PoseBtn.transform.name;
        PoseBtn.GetComponent<Text>().color = Color.black;
        GestureBtn.GetComponent<Text>().color = new Color32(142, 142, 142, 255);
        callobjects();
    }

    public void PopupTextClik(Button TextBtn)
    {
       // EmoteAnimationPlay.Instance.alreadyRuning = true;
        Caching.ClearCache();
        Debug.Log("text btn===="+ TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text + "GestureBtn===="+ GestureBtn.GetComponent<Text>().text);
        Debug.Log("text btn2===="+ TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text + "GestureBtn 2===="+ PoseBtn.GetComponent<Text>().text);
        if (!GestureBtn.GetComponent<Text>().text.Equals(TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text)&&
            !PoseBtn.GetComponent<Text>().text.Equals(TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text))
        {
            if (taboneclick)
            {
                GestureBtn.GetComponent<Text>().text = TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text;
                GestureBtn.transform.name = TextBtn.gameObject.transform.name;
            }
            else if (tabtwoclick)
            {
                PoseBtn.GetComponent<Text>().text = TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text;
                PoseBtn.transform.name = TextBtn.gameObject.transform.name;
            }
            popUpPenal.SetActive(false);
            TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.black;
            animationTabName = TextBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text;
            animationTabNameLang = TextBtn.gameObject.transform.name;

            NoDataFound.SetActive(false);
            ScrollViewAnimation.SetActive(true);
            callobjects();
        }
    }

    public void seeAllClick()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        if (!tabtwoclick)
        {
            tabtwoclick = true;
        }

        popUpPenal.SetActive(true);

        for(int i = 0; i < buttonList.Count; i++)
        {
            if (animationTabName.Equals(buttonList[i].gameObject.transform.GetChild(0).GetComponent<Text>().text))
            {
                
                buttonList[i].gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                valueget = false;
            }
            else
            {
                buttonList[i].gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color32(142, 142, 142, 255);
            }
        }
      
    }
    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            panel.SetActive(false);
        }
    }
    public void getLocalStorageEmote()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();

        Debug.Log("Response Emote===" + EmoteAnimationPlay.Instance.emoteAnim.Count);
            if (EmoteAnimationPlay.Instance.emoteAnim.Count > 0)
            {
                for (int i = 0; i < EmoteAnimationPlay.Instance.emoteAnim.Count; i++)
                {
                    //Debug.Log("GROUP NAME====" + EmoteAnimationPlay.Instance.emoteAnim[i].group);
                    //Debug.Log("GROUP NAME MY====" + animationTabNameLang);
                        valueget = true;
                        //Debug.Log("data for load==" + animationTabNameLang + "group name===" + EmoteAnimationPlay.Instance.emoteAnim[i].group);

                        //Debug.Log("animation count===" + EmoteAnimationPlay.Instance.emoteAnim.Count);

                        animObject = Instantiate(ListItemPrefab);
                        animObject.transform.SetParent(ContentPanel.transform);
                        animObject.transform.localPosition = Vector3.zero;
                        animObject.transform.localScale = Vector3.one;
                        animObject.transform.localRotation = Quaternion.identity;
                        animObject.transform.name = EmoteAnimationPlay.Instance.emoteAnim[i].name;

                        animObject.GetComponent<Image>().color= emoteBGs[(UnityEngine.Random.Range(0, emoteBGs.Length))];

                        animObject.transform.GetChild(2).gameObject.SetActive(false);

                        StartCoroutine(LoadSpriteEnv(EmoteAnimationPlay.Instance.emoteAnim[i].thumbnail, animObject.transform.GetChild(1).gameObject, i));

                        LoadButtonClick LBC = animObject.GetComponent<LoadButtonClick>();

                        if (LBC == null)
                        {
                            LBC = animObject.AddComponent<LoadButtonClick>();
                        }

#if UNITY_ANDROID

                        LBC.Initializ(EmoteAnimationPlay.Instance.emoteAnim[i].android_file, EmoteAnimationPlay.Instance.emoteAnim[i].name, this, ContentPanel.gameObject, EmoteAnimationPlay.Instance.emoteAnim[i].thumbnail);
#elif UNITY_IOS
                                LBC.Initializ(EmoteAnimationPlay.Instance.emoteAnim[i].ios_file, EmoteAnimationPlay.Instance.emoteAnim[i].name, this, ContentPanel.gameObject, EmoteAnimationPlay.Instance.emoteAnim[i].thumbnail);

#endif 
            }

            callobjects();
        }
            else
            {
                NoDataFound.SetActive(true);
                ScrollViewAnimation.SetActive(false);
            }
    }


    private void OnEmoteSelectionClose()
    {
        closeWindow(gameObject);
    }

    public void closeWindow(GameObject panel)
    {
        panel.SetActive(false);
        EmoteAnimationPlay.Instance.isEmoteActive = false;
        LoadFromFile.animClick = false;
        GamePlayButtonEvents.inst.AllAnimationsPanelUpdate(true);
        //// old code before action button disable 
        //if (animObject != null && animObject.transform.GetChild(3).gameObject.activeInHierarchy)
        //{
        //    animObject.transform.GetChild(3).gameObject.SetActive(false);
        //}
        //if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AllAnimationsPanelUpdate(false);

    }
    public void ResetHighligt()
    {
        if (ContentPanel.transform.childCount > 0)
        {
            foreach (Transform child in ContentPanel.transform)
            {
                if (child.transform.GetChild(2).gameObject.activeInHierarchy)
                {
                    child.transform.GetChild(2).gameObject.SetActive(false);
                }
               
               
                //Invoke("resetObject", 1f);
            }

          //  Invoke("callobjects", 1f);
        }
    }
    private void callobjects()
    {
        for (int i = 0; i < ContentPanel.transform.childCount; i++)
        {

            if (animationTabNameLang.Equals("Dance"))
            {
                if (ContentPanel.transform.GetChild(i).transform.name.Contains("Full"))
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(true);
                    NoDataFound.SetActive(false);
                }
                else
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            else if (animationTabNameLang.Equals("Idle"))
            {
                if (ContentPanel.transform.GetChild(i).transform.name.Contains("Idle"))
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(true);
                    NoDataFound.SetActive(false);
                }
                else
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            else if (animationTabNameLang.Equals("Jump"))
            {
                if (!ContentPanel.transform.GetChild(i).transform.name.Contains("Jump"))
                {

                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                    NoDataFound.SetActive(true);
                }

            }
            else if (animationTabNameLang.Equals("Kick"))
            {
                if (!ContentPanel.transform.GetChild(i).transform.name.Contains("Kick"))
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                    NoDataFound.SetActive(true);
                }

            }
            else if (animationTabNameLang.Equals("Reaction"))
            {
                if (ContentPanel.transform.GetChild(i).transform.name.Contains("React"))
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(true);
                    NoDataFound.SetActive(false);
                }
                else
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            else if (animationTabNameLang.Equals("Run"))
            {
                if (!ContentPanel.transform.GetChild(i).transform.name.Contains("Run"))
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                    NoDataFound.SetActive(true);
                }
            }
            else if (animationTabNameLang.Equals("Walk"))
            {
                if (ContentPanel.transform.GetChild(i).transform.name.Contains("Walk"))
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(true);
                    NoDataFound.SetActive(false);
                }
                else
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            else if (animationTabNameLang.Equals("Moves"))
            {
                if (ContentPanel.transform.GetChild(i).transform.name.Contains("Break"))
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(true);
                    NoDataFound.SetActive(false);
                }
                else
                {
                    ContentPanel.transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }

        }

    }
   
  
    IEnumerator LoadSpriteEnv(string ImageUrl, GameObject thumbnail, int i)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //ConnectionPopup.SetActive(false);
            //OnOpenPopUp("No internet connection", true);
            //Loader.SetActive(false);
        }
        else
        {
            if (ImageUrl.Equals(""))
            {
               // Loader.SetActive(false);
            }
            else
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(ImageUrl);
                www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }
                Texture2D thumbnailTexture = DownloadHandlerTexture.GetContent(www);
                thumbnailTexture.Compress(true);
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    //Loader.SetActive(false);
                    //ConnectionPopup.SetActive(false);
                    //OnOpenPopUp("No internet connection", true);
                }
                else
                {
                    sprite = Sprite.Create(thumbnailTexture, new Rect(0, 0, thumbnailTexture.width, thumbnailTexture.height), new Vector2(0, 0));
                    if (thumbnail != null)
                    {
                        progressbar.SetActive(false);
                        thumbnail.GetComponent<Image>().sprite = sprite;
                        //Loader.SetActive(false);
                    }
                    else
                    {
                       // Loader.SetActive(false);
                    }
                }
                www.Dispose();
            }
        }
    }


    //GetAllAnimations
    [Serializable]
    public class AnimationList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string group { get; set; }
        public string thumbnail { get; set; }
        public string android_file { get; set; }
        public string ios_file { get; set; }
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Data
    {
        public List<AnimationList> animationList { get; set; }
    }

    public class AnimationDetails
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public string msg { get; set; }
    }

}
