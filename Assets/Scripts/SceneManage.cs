using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using Metaverse;
using System.Collections;

public class SceneManage : MonoBehaviourPunCallbacks
{
    public Button m_JoinEventBtn;
    public Button m_JoinEventBtn1;
    public Button m_JoinEventBtn2;
    public Button m_JoinEventBtn3;
    public Button m_JoinEventBtn4;
    public Button m_JoinEventBtn5;
    public static bool leftRoom;
    public static bool callRemove;
    public GameObject AnimHighlight;
    public GameObject popupPenal;
    public GameObject spawnCharacterObject;
    public GameObject spawnCharacterObjectRemote;

    public string mainScene;

    private AsyncOperation asyncLoading;

    bool exitOnce = true;

    private void OnEnable()
    {

        if (SceneManager.GetActiveScene().name == "Main")
        {
            AvatarManager.sendDataValue = false;
            //m_JoinEventBtn.onClick.AddListener(() => LoadWorld());
            //m_JoinEventBtn1.onClick.AddListener(() => LoadWorld());
            //m_JoinEventBtn2.onClick.AddListener(() => LoadWorld());
            //m_JoinEventBtn3.onClick.AddListener(() => LoadWorld());
            //m_JoinEventBtn4.onClick.AddListener(() => LoadWorld());
            //m_JoinEventBtn5.onClick.AddListener(() => LoadWorld());

        
        }
        else
        {
           

        }



    }



    public void OpenARScene()
    {
        SceneManager.LoadScene("ARHeadWebCamTextureExample");
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
            PlayerPrefs.SetInt("RequestSend", 1);
        }
    }

    public void LoadMain()
    {
       // if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnEmoteAnimationStop();
        if (exitOnce)
        {
            exitOnce = false;

            if (GameManager.currentLanguage == "ja")
            {
                LoadingHandler.Instance.UpdateLoadingStatusText("ホームに戻っています");
            }
            else if (GameManager.currentLanguage == "en")
            {
                LoadingHandler.Instance.UpdateLoadingStatusText("Going Back to Home");
            }

            try
            {
                ChracterPosition.currSpwanPos = "";
            }
            catch (System.Exception e)
            {
                Debug.LogError("error :--- chracterposition script not found handled for tif2021.");
            }
            LoadingHandler.Instance.ShowLoading();
            StartCoroutine(LoadMainEnumerator());
        }

    }

    IEnumerator LoadMainEnumerator()
    {
        yield return new WaitForSeconds(.5f);
        if (XanaConstants.xanaConstants.museumAssetLoaded != null)
            XanaConstants.xanaConstants.museumAssetLoaded.Unload(true);
        LeaveRoom();
    }


    public void LoadWorld()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 0)
        {
            UIManager.Instance.LoginRegisterScreen.transform.SetAsLastSibling();
            UIManager.Instance.LoginRegisterScreen.SetActive(true);
        }
        else
        {
            GameManager.Instance.DestroyAllLoadedTexture();
            UIManager.Instance.IsWorldClicked();
        }

    }
    public void LeaveRoom()
    {
        callRemove = true;
        Launcher.instance.working = ScenesList.MainMenu;
        PhotonNetwork.LeaveRoom(false);
        PhotonNetwork.LeaveLobby();
        StartSceneLoading();
    }

    public void StartSceneLoading()
    {
        print("Hello Scene Manager");
        // string unit = "Going Back to Home";
        //string a= TextLocalization.GetLocaliseTextByKey();
        // LoadingHandler.Instance.UpdateLoadingStatusText("Going Back to Home");
        asyncLoading = SceneManager.LoadSceneAsync(mainScene);
        InvokeRepeating("AsyncProgress", 0.1f, 0.1f);

    }

    void AsyncProgress()
    {
        LoadingHandler.Instance.UpdateLoadingSlider(asyncLoading.progress * 1.1f);
    }

    // photon callback
    public override void OnLeftRoom()
    {

    }

    //public void AnimClick()
    //{
    //    EmoteAnimationPlay.Instance.animationClick();
    //}
}
