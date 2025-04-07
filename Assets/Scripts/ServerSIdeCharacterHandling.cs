
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerSIdeCharacterHandling : MonoBehaviour
{
    public static ServerSIdeCharacterHandling Instance;

    //Event will be called when user loged In and new Avatar is saved by user. Event is created for multiple avatar saving.
    //public static event Action<int, int> loadAllAvatar;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;


    }

    private void Start()
    {
        //  StartCoroutine(UpdateUserData());
    }


    public void CreateUserOccupiedAsset()
    {
        StartCoroutine(CreateUserData());
    }
    public void GetDataFromServer()
    {
        print(PlayerPrefs.GetString("LoginToken"));
        StartCoroutine(GetUserData(PlayerPrefs.GetString("LoginToken")));
        //   StartCoroutine(AddingEnteries(UserIDfromServer));
    }

    public IEnumerator CreateUserData()   // send json data with user id but first check if user already exist or not   user ID
    {

        string url = ConstantsGod.API_BASEURL + ConstantsGod.CREATEOCCUPIDEUSER;
        //  string urlwithId = url + UserIDfromServer; //Adding user ID
        //Get data from file 
        // need to add check if file exists
        Json json = new Json();
        json = json.CreateFromJSON(File.ReadAllText(GetStringFolderPath()));
        //StartCoroutine(AddingEnteries(UserIDfromServer));
        SendUpdateData senddata = new SendUpdateData();
        senddata.name = LoadPlayerAvatar.avatarName;
        senddata.json = JsonUtility.ToJson(json);
        senddata.thumbnail = LoadPlayerAvatar.avatarThumbnailUrl;
        senddata.description = "None";
        string bodyJson = JsonUtility.ToJson(senddata);
        //print(bodyJson);
        UnityWebRequest www = new UnityWebRequest(url, "Post");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        //Debug.LogError(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);
        //  print(getdata.success);
        if (!www.isHttpError && !www.isNetworkError)
        {
            if (getdata.success)
            {

                print("DataUpdated");
                //   GetResponseupdatedata getdata = new GetResponseupdatedata();
                // getdata = JsonUtility.FromJson<GetResponseupdatedata>(www.downloadHandler.text);
                //   print(getdata.msg);
                //   print( "Data Sent : "+getdata.success);
                //  print(JsonUtility.ToJson(getdata.data.json));
            }
        }
        //if (loadAllAvatar != null)
        //    loadAllAvatar(1, 1);
        LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 1);
    }

    public void UpdateUserOccupiedAsset(string avatarID)
    {
        StartCoroutine(UpdateExistingUserData(avatarID));
    }

    IEnumerator UpdateExistingUserData(string avatarID)
    {
        
        Json json = new Json();
        json = json.CreateFromJSON(File.ReadAllText(GetStringFolderPath()));

        SendUpdateData senddata = new SendUpdateData();
        senddata.name = LoadPlayerAvatar.avatarName;
        senddata.json = JsonUtility.ToJson(json);
        senddata.thumbnail = LoadPlayerAvatar.avatarThumbnailUrl;
        senddata.description = "avatar updated at :-" + DateTime.Now.ToString();
        string bodyJson = JsonUtility.ToJson(senddata);

        //WWWForm formData = new WWWForm();
        //formData.AddField("name", senddata.name);
        //formData.AddField("thumbnail", senddata.thumbnail);
        //formData.AddField("json", senddata.json);
        //formData.AddField("description", senddata.description);

        Debug.LogError(LoadPlayerAvatar.avatarId + "--" + LoadPlayerAvatar.avatarName + "--" + LoadPlayerAvatar.avatarThumbnailUrl + "--" + senddata.json);

        //UnityWebRequest www =UnityWebRequest.Post(ConstantsGod.API_BASEURL + ConstantsGod.UPDATEOCCUPIDEUSER + avatarID,formData);

        UnityWebRequest www = new UnityWebRequest(ConstantsGod.API_BASEURL + ConstantsGod.UPDATEOCCUPIDEUSER + avatarID, "Post");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        Debug.LogError(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);

        if (!www.isHttpError && !www.isNetworkError)
        {
            if (getdata.success)
            {

                print("DataUpdated");
                if (StoreManager.instance.AvatarUpdated != null)
                    StoreManager.instance.AvatarUpdated.SetActive(true);

                LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 1);
                //   GetResponseupdatedata getdata = new GetResponseupdatedata();
                // getdata = JsonUtility.FromJson<GetResponseupdatedata>(www.downloadHandler.text);
                //   print(getdata.msg);
                //   print( "Data Sent : "+getdata.success);
                //  print(JsonUtility.ToJson(getdata.data.json));
            }
        }

        //LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 1);
    }


    IEnumerator CreateUserData_FirstTime(string token)   // send json data first time only but first check if that user exists or not
    {

        //string url = "https://app-api.xana.net/item/create-user-occupied-asset";
        ////Get data from file 
        //// need to add check if file exists
        //if (File.Exists(GetStringFolderPath()) && File.ReadAllText(GetStringFolderPath()) != "")
        //{
        //    Json json = new Json();
        //    json = json.CreateFromJSON(File.ReadAllText(GetStringFolderPath()));
        //    SendUpdateData senddata = new SendUpdateData();
        //    senddata.name = "7270";
        //    senddata.json = JsonUtility.ToJson(json);
        //    senddata.description = "None";
        //    string bodyJson = JsonUtility.ToJson(senddata);
        //    print(bodyJson);

        //    UnityWebRequest www = new UnityWebRequest(url, "Post");
        //    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson);
        //    www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        //    www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //    www.SetRequestHeader("Authorization", token);
        //    www.SetRequestHeader("Content-Type", "application/json");
        //    yield return www.SendWebRequest();
        //    //  print(www.downloadHandler.text);
        //    if (!www.isHttpError && !www.isNetworkError)
        //    {
        //        GetResponseupdatedata getdata = new GetResponseupdatedata();
        //        getdata = JsonUtility.FromJson<GetResponseupdatedata>(www.downloadHandler.text);
        //        if (getdata.msg == "Occupied asset created successfully")
        //        {
        //            print("Data Updated Successfully");
        //            if (getdata.data != null) // File data from server
        //            {
        //                // Json JsonData = new Json();
        //                string jsonbody = JsonUtility.ToJson(getdata.data.json);
        //                ///Check if file exists
        //                File.WriteAllText(GetStringFolderPath(), jsonbody);
        //                // after filing push it to characters
        //                print("File written");
        //                print(jsonbody);
        //            }
        //        }
        //        else if (getdata.msg == "You have already an occuied asset , Please use Update method")
        //        {
        //            print("Already have account use update method to get data");
        //        }
        //        print(JsonUtility.ToJson(getdata.data.json));
        //        //Root jsonreturn = new Root();
        //    }
        //    else
        //        Debug.LogError("Network Error Try again later bro");
        //}
        //else
        //{
        //    SavaCharacterProperties.instance.SetDefaultData();
        //    print("Creating file with dummy data");
        //    //Json json = new Json();
        //    //string jsonbody = JsonUtility.ToJson(json);
        //    //File.WriteAllText(GetStringFolderPath(), jsonbody);
        //    //  print(jsonbody);
        //}
        yield return null;
    }

    public void DeleteAvatarDataFromServer(string token, string UserId)
    {
        StartCoroutine(DeleteUserData(token, UserId));
    }


    IEnumerator DeleteUserData(string token, string userID)   // delete data if Exist
    {
        //  print("Token " + PlayerPrefs.GetString("LoginToken"));
        UnityWebRequest www = UnityWebRequest.Delete(ConstantsGod.API_BASEURL + ConstantsGod.DELETEOCCUPIDEUSER + userID);
        www.SetRequestHeader("Authorization", token);
        yield return www.SendWebRequest();

        if (www.responseCode == 200)
        {
            Debug.LogError("Occupied Asset Delete Successfully");
        }

        //string str = www..text;
        //Root db = new Root();
        //db = JsonUtility.FromJson<Root>(str);
        ////print(db.success);
        //// print(db.data);
        ////print(db.msg);
        //if (db.msg == "Occupied Asset Delete Successfully")
        //{
        //    print("data Deleted successfully");

        //}
        //else if (db.msg == "Occupied Asset get successfully")
        //{
        //    print("data Received Successfully");
        //}
    }
    [HideInInspector]
    public int UserIDfromServer;
    IEnumerator GetUserData(string token)   // check if  data Exist
    {

        UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.OCCUPIDEASSETS + "1/1");
        www.SetRequestHeader("Authorization", token);
        www.SendWebRequest();
        while (!www.isDone)
        {
            yield return null;
        }
        Debug.Log(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);
        // DefaultEnteriesforManican.instance.DefaultReset();
        //  print(getdata.success);
        if (!www.isHttpError && !www.isNetworkError)
        {
            if (getdata.success)
            {

                // its a new user so create file 
                if (getdata.data.count == 0)
                {
                    SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
                    SubCatString.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
                    string jbody = JsonUtility.ToJson(SubCatString);
                    File.WriteAllText(GameManager.Instance.GetStringFolderPath(), jbody);
                    StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
                }
                else
                {
                    // write latest json data to file
                    if (File.Exists(GameManager.Instance.GetStringFolderPath()))
                    {
                        Debug.Log("Load previous player");
                        SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                        _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
                        LoadPlayerAvatar.avatarId = _CharacterData.id;
                        LoadPlayerAvatar.avatarName = _CharacterData.name;
                        LoadPlayerAvatar.avatarThumbnailUrl = _CharacterData.thumbnail;
                    }
                    else
                    {
                        string jsonbody = JsonUtility.ToJson(getdata.data.rows[0].json);
                        LoadPlayerAvatar.avatarId = getdata.data.rows[0].id.ToString();
                        LoadPlayerAvatar.avatarName = getdata.data.rows[0].name;
                        LoadPlayerAvatar.avatarThumbnailUrl = getdata.data.rows[0].thumbnail;

                        File.WriteAllText(GetStringFolderPath(), jsonbody);
                        yield return new WaitForSeconds(0.1f);
                    }
                    loadprevious();
                    StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));

                }
            }
        }
        else
            Debug.LogError("NetWorkissue");
        www.Dispose();

        //if (loadAllAvatar != null)
        //    loadAllAvatar(1,15);
        LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 15);

    }
    //For Preset account to get presets
    public void getPresetDataFromServer()
    {
        //  StartCoroutine(GetPresetData_Server());
    }
    IEnumerator GetPresetData_Server()   // check if  data Exist
    {
        UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.OCCUPIDEASSETS + "1/50");
        www.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken_Preset"));
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
                    GameObject contentparent = StoreManager.instance.ClothsPanel[4].GetComponent<ScrollRect>().content.gameObject;
                    GameObject contentStartUpPanel = StoreManager.instance.PresetArrayContent;

                    for (int c = 0; c < getdata.data.count - 1; c++)
                    {
                        contentparent.transform.GetChild(c).GetComponent<PresetData_Jsons>().JsonDataPreset =
                         JsonUtility.ToJson(getdata.data.rows[c].json);
                        // Populating Panel For the FirstTimeOnly
                        //Kindly add a check to populate it once 
                        contentStartUpPanel.transform.GetChild(c).GetComponent<PresetData_Jsons>().JsonDataPreset =
                        JsonUtility.ToJson(getdata.data.rows[c].json);
                        //              string jsonbody = JsonUtility.ToJson(getdata.data.rows[0].json);
                    }
                    File.WriteAllText((Application.persistentDataPath + "/SavingReoPreset.json"), JsonUtility.ToJson(getdata.data.rows[0].json));
                    yield return new WaitForSeconds(0.1f);
                    //               loadprevious();
                }
            }
        }
        else
            Debug.LogError("NetWorkissue");

    }


















    public int localindexfilebuffer;

    public void loadprevious()
    {
        GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
        SavaCharacterProperties.instance.LoadMorphsfromFile(); // loading morohs 
                                                               // DefaultEnteriesforManican.instance.LastSaved_Reset();
    }
    public string GetStringFolderPath()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)  // loged from account)
        {

            return (Application.persistentDataPath + "/SavingCharacterDataClass.json");
        }
        else
        {

            return (Application.persistentDataPath + "/loginAsGuestClass.json");
        }
    }


    [Serializable]
    public class SendUpdateData
    {
        public string name;
        public string json;
        public string thumbnail;
        public string description;
    }
    [Serializable]
    public class GetResponseupdatedata
    {
        public string success;
        public Data data;
        public string msg;
    }

    [Serializable]
    public class ItemPrefab
    {
        public int instanceID;
    }

    [Serializable]
    public class ItemIcon
    {
        public int instanceID;
    }

    [Serializable]
    public class MyItemObj
    {
        public string Slug;
        public string ItemType;
        public ItemPrefab ItemPrefab;
        public int ItemID;
        public string ItemName;
        public string ItemDescription;
        //  public string ItemLink;
        public string ItemLinkAndroid;
        public string ItemLinkIOS;
        public string SubCategoryname;
        public bool Stackable;
        public ItemIcon ItemIcon;
    }

    public class MAnimCurve
    {
        public string serializedVersion;
        public List<object> m_Curve;
        public int m_PreInfinity;
        public int m_PostInfinity;
        public int m_RotationOrder;
    }

    [Serializable]
    public class EyePresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class NosePresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class LipsPresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class EyeBrowPresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class FacePresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class Json
    {
        public string id;
        public string name;
        public string thumbnail;
        public List<MyItemObj> myItemObj;
        public EyePresets EyePresets;
        public NosePresets NosePresets;
        public LipsPresets LipsPresets;
        public EyeBrowPresets EyeBrowPresets;
        public FacePresets FacePresets;
        public int BodyFat = -1;
        public List<int> FaceBlendsShapes;
        public Json CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Json>(jsonString);
        }
    }

    [Serializable]
    public class User
    {
        public int id;
        public string name;
        public string email;
        public string avatar;
    }

    [Serializable]
    public class Row
    {
        public int id;
        public string name;
        public string thumbnail;
        public Json json;
        public string description;
        public bool isDeleted;
        public int createdBy;
        public DateTime createdAt;
        public DateTime updatedAt;
        public User user;
    }
    [Serializable]
    public class Root
    {
        public bool success;
        public Data data;
        public string msg;
    }

    [Serializable]
    public class Data
    {
        public int count;
        public List<Row> rows;
    }
}


