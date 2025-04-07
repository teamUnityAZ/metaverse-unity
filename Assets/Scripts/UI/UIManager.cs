using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject LoginRegisterScreen, SignUpScreen, HomePage, Canvas;
    public GameObject _SplashScreen;
    private bool orientationchanged = false;

    [Header("Footer Reference")]//rik
    public GameObject _footerCan;

    public enum Panel
    {
        SplashPage, StartPage, MainSignUpPage, SignUps, LoginPage, ForgotPasswordPage, HomePage,
        CharacterCustomizationPage,
        DressCustomizationPage,
        IconsCustomizationPage,
        UpdatesCustomizationPage,
        CustomFaceShapePage, NotificationsPage, ChatPage, CameraViewPage, PosePage, AvatarScreen, LiveFeedPage, WorldPage, CreatePage, SearchPage, FeedPage, ProfilePage,
        FollowersPage, AddFriendPage, SettingPage, Footer, mainCanvas
    }
    public Panel m_PanelName_Enum;

    public GameObject faceMorphPanel;
  

    private void Awake()
    {
        Instance = this;
//#if UNITY_ANDROID && !UNITY_EDITOR
//    Screen.fullScreen = false; //Should be unnecessary unless you changed it
//    Color32 color = new Color(0, 0, 0, 0.7f);
//    AndroidUtility.ShowStatusBar(color);
//#endif
    }
    private void Start()
    {
        if (SavaCharacterProperties.NeedToShowSplash == 1)
        {
            StartCoroutine(IsSplashEnable(false, 4f));

        }
        else
        {
            StartCoroutine(IsSplashEnable(false, 0f));
            StartCoroutine(LoadingHandler.Instance.ShowLoadingForCharacterUpdation(4));
        }
    }
    public void OnClickMessageOrFeedBtnClick()
    {
        Initiate.Fade("SNSFeedModuleScene", Color.black, 1.0f, true);        
    }

    public void AvaterButtonCustomPushed()
    {
        LoginPageManager.m_WorldIsClicked = false;
        LoginPageManager.m_MuseumIsClicked = false;
    }


    IEnumerator startMuseums()
    {
        yield return new WaitForSeconds(0.2f);
        //XanaConstants.xanaConstants.museumDownloadLink = "";
        
        switch (PlayerPrefs.GetString("ScenetoLoad"))
        {

            case "Aurora":
                //XanaConstants.xanaConstants.EnviornmentName = SceneManager.GetSceneByBuildIndex(2).name;
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Aurora_Art_Museum/auroramuseum.android";
                } else// if(Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Aurora_Art_Museum/auroramuseum.ios";
                }
                //SceneManager.LoadSceneAsync(2);
                break;
            case "GekkoSan":
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Gekko_San_Museum/gekkosanmuseum.android";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Gekko_San_Museum/gekkosanmuseum.ios";
                }
                //XanaConstants.xanaConstants.EnviornmentName = SceneManager.GetSceneByBuildIndex(3).name;
                //SceneManager.LoadSceneAsync(3);
                break;
            case "Hokusai":
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Hokusai_Museum/hokusaimuseum.android";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Hokusai_Museum/hokusaimuseum.ios";
                }
                //XanaConstants.xanaConstants.EnviornmentName = SceneManager.GetSceneByBuildIndex(4).name;
                //SceneManager.LoadSceneAsync(4);
                break;
            case "Yukinori":
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Yukinori_Museum/yukinorimuseum.android";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Yukinori_Museum/yukinorimuseum.ios";
                }
                //XanaConstants.xanaConstants.EnviornmentName = SceneManager.GetSceneByBuildIndex(5).name;
                //SceneManager.LoadSceneAsync(5);
                break;
            case "GOZMuseum":
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Goz_Museum/gozmuseum.android";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Goz_Museum/gozmuseum.ios";
                }
                //XanaConstants.xanaConstants.EnviornmentName = SceneManager.GetSceneByBuildIndex(6).name;
                //SceneManager.LoadSceneAsync(6);
                break;
            case "NFTMuseum":
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Nft_Museum/nftmuseum.android";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    XanaConstants.xanaConstants.museumDownloadLink = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Nft_Museum/nftmuseum.ios";
                }
                //XanaConstants.xanaConstants.EnviornmentName = SceneManager.GetSceneByBuildIndex(7).name;
                //SceneManager.LoadSceneAsync(7);
                break;
        }
        LoadingHandler.Instance.ShowLoading();
        yield return new WaitForEndOfFrame();
        //SceneManager.LoadSceneAsync("DynamicMuseum");
        SceneManager.LoadSceneAsync(1);
    }

    public void IsWorldClicked()
    {

        if (LoginPageManager.m_MuseumIsClicked || UserRegisterationManager.instance.LoggedIn)
        {
            FindObjectOfType<EventList>().PlayWorld();
            //            LoadingHandler.Instance.Loading_WhiteScreen.SetActive(true);
            //            Screen.orientation = ScreenOrientation.LandscapeLeft;
            //#if UNITY_EDITOR
            //            orientationchanged = true;
            //            LoadingHandler.Instance.ShowLoading();
            //            StartCoroutine(startMuseums());
            //#else
            //            InvokeRepeating("Check_Orientation", 1, 1);
            //#endif
        }
        else if (LoginPageManager.m_WorldIsClicked || UserRegisterationManager.instance.LoggedIn)
        {
            FindObjectOfType<EventList>().PlayWorld();
        }
       
    }

    private void Check_Orientation()
    {
        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            orientationchanged = true;
        }
        if (orientationchanged)
        {
            LoadingHandler.Instance.ShowLoading();
            StartCoroutine(startMuseums());
            LoadingHandler.Instance.Loading_WhiteScreen.SetActive(false);
            CancelInvoke("Check_Orientation");
        }
    }

    public IEnumerator IsSplashEnable(bool _state, float _time)
    {
        SavaCharacterProperties.NeedToShowSplash = 2;
        yield return new WaitForSeconds(_time);
        _SplashScreen.SetActive(_state);
        ShowFooter(!_state);//rik
    }

    public void ShowFooter(bool _state)//rik
    {
        _footerCan.SetActive(_state);
    }
   
    public void ExitApllication()
    {
        Application.Quit();
        print("Working");
    }
    
}
