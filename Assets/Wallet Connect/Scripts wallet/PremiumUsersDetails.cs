using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class PremiumUsersDetails : MonoBehaviour
{
    public static PremiumUsersDetails Instance;
    public string getGroupDetailsAPI = ConstantsGod.API_BASEURL + ConstantsGod.GETSETS;
    public MainClass SetMainObj;
    public MainClass alphaPassObj;
    public Dictionary<string, bool> combinedUserFeatures = new Dictionary<string, bool>();
    public GameObject PremiumUserUI, PremiumUserUILandscape;
    //public GameObject comingSoonPanel;
    public GameObject PremiumUserUIDJEvent;
    public GameObject vipPassUI;
    private string AuthToken;
    public string PremiumUserType;
    public bool testing;

    private string alphaPassRedirectLink = "";

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            SetMainObj = new MainClass();
            DontDestroyOnLoad(gameObject);
            PremiumUserType = "";
        }
        else
        if (Instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        /*if(SNSComingSoonManager.Instance.snsComingSoonScreen!=null)
        {
            comingSoonPanel = SNSComingSoonManager.Instance.snsComingSoonScreen;
        }*/
    }


    public bool CheckSpecificItem(string getName, bool enablePopupHere = true)
    {

        if (testing)
            return true;

        getName = getName.Trim();
        getName = getName.ToLower();
        bool EnabledFeaturebool = false;
        if (combinedUserFeatures.ContainsKey(getName))
        {
            EnabledFeaturebool = combinedUserFeatures[getName];
            if (EnabledFeaturebool)
            {
                return true;
            }
            else
            {
                if (enablePopupHere)
                {
                    if (ConstantsGod.UserPriorityRole == "alpha-pass")
                    {
                        if (SNSNotificationManager.Instance != null && enablePopupHere)
                        {
                            SNSNotificationManager.Instance.ShowNotificationMsg("This features is coming soon");//this method is used to show Coming Soon notification.......                                            
                        }
                        return false;
                    }
                    else
                    {
                        if (Screen.orientation == ScreenOrientation.Portrait)
                        {
                            ShowNotAvailablePanel(PremiumUserUI);
                        }
                        else
                        {
                            ShowNotAvailablePanel(PremiumUserUILandscape);
                        }
                    }
                }
                return false;
            }
        }
        else
        {
            if (ConstantsGod.UserPriorityRole == "alpha-pass")
            {
                if (SNSNotificationManager.Instance != null && enablePopupHere)
                {
                    SNSNotificationManager.Instance.ShowNotificationMsg("This features is coming soon");//this method is used to show Coming Soon notification.......                                            
                }
                return false;
            }
            else
            {
                if (enablePopupHere)
                    if (Screen.orientation == ScreenOrientation.Portrait)
                    {
                        ShowNotAvailablePanel(PremiumUserUI);
                    }
                    else
                    {
                        ShowNotAvailablePanel(PremiumUserUILandscape);
                    }
                return false;
            }

        }
    }



    void ShowNotAvailablePanel(GameObject panel)
    {

        panel.SetActive(true);

    }





    [System.Serializable]
    public class MyClassOfPostingName
    {
        public string name;
        public MyClassOfPostingName GetEmaildata(string _name)
        {
            MyClassOfPostingName myObj = new MyClassOfPostingName();
            myObj.name = _name;
            return myObj;
        }
    }

    public void GetGroupDetails(string _groupName = "")
    {
        PlayerPrefs.SetString("PremiumUserType", _groupName);
        PlayerPrefs.Save();
        switch (_groupName)
        {
            case "djevent":
                {
                    print("djevent");
                    _groupName = "DJ Event";
                    AuthToken = PlayerPrefs.GetString("LoginToken");
                    break;
                }
            case "Access Pass":
                {
                    print("Access Path");
                    _groupName = "Set3";
                    AuthToken = PlayerPrefs.GetString("LoginToken");
                    break;
                }
            case "Extra NFT":
                {
                    print("Extra NFT");
                    _groupName = "Set2";
                    AuthToken = PlayerPrefs.GetString("LoginToken");
                    break;
                }
            case "guest":
                {
                    print("Guest");
                    _groupName = "Set1";
                    AuthToken = PlayerPrefs.GetString("GuestToken");
                    break;
                }
            case "freeuser":
                {
                    print("Freeuser");
                    _groupName = "Set1";
                    AuthToken = PlayerPrefs.GetString("LoginToken");
                    break;
                }
            case "vip-pass":
                {
                    print("Freeuser");
                    _groupName = "vip-pass";
                    AuthToken = PlayerPrefs.GetString("LoginToken");
                    break;
                }
        }
        PremiumUserType = PlayerPrefs.GetString("PremiumUserType");

        MyClassOfPostingName myobjectOfEmail = new MyClassOfPostingName();
        string bodyJson = JsonUtility.ToJson(myobjectOfEmail.GetEmaildata(_groupName));
        StartCoroutine(HitGetGroupDetails(ConstantsGod.API_BASEURL + ConstantsGod.GetGroupDetailsAPI, bodyJson, (SetMainObj) =>
        {
            this.SetMainObj = SetMainObj;
            for (int i = 0; i < SetMainObj.data.relationList.Count; i++)
            {
                SetMainObj.data.relationList[i].feature = SetMainObj.data.relationList[i].feature.Trim();
                SetMainObj.data.relationList[i].feature = SetMainObj.data.relationList[i].feature.ToLower();
                if (!combinedUserFeatures.ContainsKey(SetMainObj.data.relationList[i].feature))
                    combinedUserFeatures.Add(SetMainObj.data.relationList[i].feature, SetMainObj.data.relationList[i].isEnabled);
                else if (combinedUserFeatures[SetMainObj.data.relationList[i].feature] == false)
                    combinedUserFeatures[SetMainObj.data.relationList[i].feature] = SetMainObj.data.relationList[i].isEnabled;

            }
        }));
    }
    public IEnumerator HitGetGroupDetails(string url, string Jsondata, Action<MainClass> callback)
    {
        //    print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", AuthToken);
        yield return request.SendWebRequest();
        // Debug.LogError(request.downloadHandler.text);
        MainClass mainClassObj = new MainClass();
        mainClassObj = JsonUtility.FromJson<MainClass>(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                //if (myObject1.success == "true")
                if (mainClassObj.success)
                {
                    Debug.Log("Success in getting Group Data ");
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                Debug.LogError("Error in getting Group Details");
            }
            else
            {
                if (request.error != null)
                {
                    if (!mainClassObj.success)
                    {
                        Debug.LogError("Hey success false in Getting Group data " + mainClassObj.msg);
                    }
                }
            }
        }

        callback(mainClassObj);
        request.Dispose();
    }


    public void ReDirectToLink(string url)
    {
        Application.OpenURL(url);
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    [System.Serializable]
    public class RelationListClass
    {
        public bool isEnabled;
        public string feature;
    }

    [System.Serializable]
    public class DataClass
    {
        public string name;
        public List<RelationListClass> relationList;
    }

    [System.Serializable]
    public class MainClass
    {
        public bool success;
        public DataClass data;
        public string msg;
    }

}
