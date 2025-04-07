using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupClick : MonoBehaviour
{
    public GameObject VersionPopup;
    public GameObject FooterTabs;
    public GameObject SkipButton;
    public string bundleIdofLunchingApp;
    bool CheckLunchingFail = false;
    bool skipClick = false;
    
   
    // Update is called once per frame
//    void Update()
//    {
//#if UNITY_ANDROID && !UNITY_EDITOR
//        if (!skipClick)
//        {
//            string[] appversion = Application.version.Split(char.Parse("."));

            //string[] serverVersion = LoadingSavingAvatar.version.Split('.');
            //if (int.Parse(serverVersion[0].ToString()) > int.Parse(appversion[0].ToString()) ||
            //    int.Parse(serverVersion[1].ToString()) > int.Parse(appversion[1].ToString()) ||
            //    int.Parse(serverVersion[2].ToString()) > int.Parse(appversion[2].ToString()) && !string.IsNullOrEmpty(LoadingSavingAvatar.version))
            //{
            //    if (FooterTabs.activeInHierarchy)
            //    {
            //        skipClick = false;
            //        FooterTabs.SetActive(false);
            //        VersionPopup.SetActive(true);
                  //  StartCoroutine(versionCheck());
//            string[] serverVersion = LoadingSavingAvatar.version.Split(char.Parse("."));
//            if (int.Parse(serverVersion[0].ToString()) > int.Parse(appversion[0].ToString()) ||
//                int.Parse(serverVersion[1].ToString()) > int.Parse(appversion[1].ToString()) ||
//                int.Parse(serverVersion[2].ToString()) > int.Parse(appversion[2].ToString()) && !string.IsNullOrEmpty(LoadingSavingAvatar.version))
//            {
//                if (FooterTabs.activeInHierarchy)
//                {
//                    skipClick = false;
//                    FooterTabs.SetActive(false);
//                    VersionPopup.SetActive(true);
//                  //  StartCoroutine(versionCheck());

//                }
//            }
//        }
//#elif UNITY_IOS
//     if (!skipClick)
//        {
//               string[] appversion = Application.version.Split('.');
//               string[] serverVersion = LoadingSavingAvatar.version.Split('.');
////                }
////            }
//        }
//#elif UNITY_IOS
//     if (!skipClick)
//        {
//              string[] appversion = Application.version.Split(char.Parse("."));

//            string[] serverVersion = LoadingSavingAvatar.version.Split(char.Parse("."));
//              if (int.Parse(serverVersion[0].ToString()) > int.Parse(appversion[0].ToString()) ||
//                int.Parse(serverVersion[1].ToString()) > int.Parse(appversion[1].ToString()) ||
//                int.Parse(serverVersion[2].ToString()) > int.Parse(appversion[2].ToString()) && !string.IsNullOrEmpty(LoadingSavingAvatar.version))
//            {
//                if (FooterTabs.activeInHierarchy)
//                {
//                    skipClick = false;
//                    FooterTabs.SetActive(false);
//                    VersionPopup.SetActive(true);
//                  //  StartCoroutine(versionCheck());

//                }
//            }
//        }
//#endif


//    }

    IEnumerator versionCheck()
    {
        yield return new WaitForSeconds(10f);
       // SkipButton.SetActive(true);
    }

    public void skip()
    {
        skipClick = true;
        FooterTabs.SetActive(true);
        VersionPopup.SetActive(false);
    }

    public void Play_AppStore()
    {
#if UNITY_ANDROID
        OpenAppForAndroid();
        //Debug.Log("App update");
       // Application.OpenURL("https://www.apple.com/in/app-store/");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/in/app/xana/id1524283847");
        Debug.Log("App update");
#endif


    }

    void OpenAppForAndroid()
    {
        string message = "https://play.google.com/store/apps/details?id=com.nbi.xana";
      //  message += AppID.ToString();
        //print(message);
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", "https://play.google.com/store/apps/details?id=com.nbi.xana");
            // launchIntent.Call<AndroidJavaObject>("putExtra", "arguments", message);
            ca.Call("startActivity", launchIntent);
        }
        catch (System.Exception e)
        {
            CheckLunchingFail = true;
        }
        print("app not found bool" + CheckLunchingFail);
        if (CheckLunchingFail)
        {
            print("app not found");
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "https://play.google.com/store/apps/details?id=com.nbi.xana");

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject(
                            "android.content.Intent",
                            intentClass.GetStatic<string>("ACTION_VIEW"),
                            uriObject
            );

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);
            // Application.OpenURL(message);

        }
        else
        {
            ca.Call("startActivity", launchIntent);
        }
        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
        //checkPackageAppIsPresent("com.xanaliaApp");

    }
}