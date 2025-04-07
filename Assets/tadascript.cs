using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tadascript : MonoBehaviour
{
    public GameObject tada1;
    public GameObject tada2;
        // Start is called before the first frame update
    
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (GameManager.currentLanguage == "ja")
            {
                tada1.SetActive(false);
                tada2.SetActive(true);
            }
            else if (GameManager.currentLanguage == "en")
            {
                tada1.SetActive(true);
                tada2.SetActive(false);
            }
        }
    }

    public static string GetLanguage()
    {
#if UNITY_ANDROID
     try
     {
         var locale = new AndroidJavaClass("java.util.Locale");
         var localeInst = locale.CallStatic<AndroidJavaObject>("getDefault");
         var name = localeInst.Call<string>("getLanguage");
         return name;
     }
     catch(System.Exception e)
     {
         return "Error";
     }
#else
#if UNITY_IOS
        string newLanguage = Application.systemLanguage.ToString();

        if (newLanguage == "English")
        {
            return "en";
        }
        else if (newLanguage == "Japanese")
        {
            return "ja";
        }

        else
            return "";
#endif
#endif
    }
    // returns "eng" / "deu" / ...
    public static string GetISO3Language()
    {
#if UNITY_ANDROID
     try
     {
         var locale = new AndroidJavaClass("java.util.Locale");
         var localeInst = locale.CallStatic<AndroidJavaObject>("getDefault");
         var name = localeInst.Call<string>("getISO3Language");
         return name;
     }
     catch(System.Exception e)
     {
         return "Error";
     }
#else
#if UNITY_IOS
        string newLanguage = Application.systemLanguage.ToString();

        if (newLanguage == "English")
        {
            return "en";
        }
        else if (newLanguage == "Japanese")
        {
            return "ja";
        }

        else
            return "";
#endif
#endif
    }
}
