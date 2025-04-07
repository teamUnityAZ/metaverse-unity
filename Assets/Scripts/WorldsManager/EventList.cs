using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun.Demo.PunBasics;
using UnityEditor;
using WebSocketSharp;
using UnityEngine.SceneManagement;
using System;
using SuperStar.Helpers;

public class EventList : MonoBehaviour
{
    public GameObject eventPrefab;
    public Transform ListContent;
    public EnvList envList;
    public static EventList instance;
   // public bool orientationchanged = false;
    [Header("isDebug")]
    public bool isDebug = false;
    // Private Variables
    private GameObject eventPrefabObject;
    private int eventsCount;
    public static int downloadCounter = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    private IEnumerator Start()
    {

        //AssetCache.Instance.RemoveAllFiles();
        Resources.UnloadUnusedAssets();
        Caching.ClearCache();

        yield return new WaitForSeconds(.2f);

        CheckInternet.instance.onConnected.AddListener(ReloadWorlds);
    }

    void ReloadWorlds()
    {
        int childCount = ListContent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(ListContent.transform.GetChild(i).gameObject);
        }
        GetWorldAPISNew();
    }


    public void GetWorldAPISNew()
    {
        StartCoroutine(GetEnvAPINew());
    }
    /// <Irfan New API Starts here>
    private GetAllInfoEnvironment ObjofEnv, localObjOfEnv;
    private GetAllInfoMuseum ObjofMuseum, localObjOfMuseum;
    public GetAllInfoEnvironment GetAllData(string m_JsonData)
    {
        GetAllInfoEnvironment JsonDataObj = new GetAllInfoEnvironment();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoEnvironment>(m_JsonData);
        return JsonDataObj;
    }
    public GetAllInfoMuseum GetAllMuseumData(string m_JsonData)
    {
        GetAllInfoMuseum JsonDataObj = new GetAllInfoMuseum();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoMuseum>(m_JsonData);
        return JsonDataObj;
    }

    [System.Serializable]
    public class GetAllInfoEnvironment
    {
        public bool success;
        public DataClass data;
        public string msg;
    }

    [System.Serializable]
    public class GetAllInfoMuseum
    {
        public bool success;
        public DataClassMuseum data;
        public string msg;
    }
    [System.Serializable]
    public class DataClass
    {
        public int count;
        public List<RowList> rows;
    }
    [System.Serializable]
    public class DataClassMuseum
    {
        public int count;
        public List<MuseumInfo> rows;
    }
    [System.Serializable]
    public class RowList
    {
        public string id;
        public string environment_name;
        public string user_limit;
        public string thumbnail;
        public string android_file;
        public string upload_timestamp;
        public string standalone_file;
        public string ios_file;
        public string model_file;
        public string banner;
        public string description;
    }
    [System.Serializable]
    public class MuseumInfo
    {
        public string id;
        public string name;
        public string user_limit;
        public string thumbnail;
        public string android_file;
        public string upload_timestamp;
        public string standalone_file;
        public string ios_file;
        public string model_file;
        public string banner;
        public string description;
    }
    public IEnumerator GetEnvAPINew()
    {
        int i = 0;
        XanaConstants.xanaConstants.setIdolVillaPosition = true;
        if (!PlayerPrefs.HasKey("Filesystem"))
        {
            if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/"))
            {
                localObjOfEnv = GetAllData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + "ObjofEnv.json"));
                i = localObjOfEnv.data.rows.Count - 1;
                while (i >= 0)
                {
                    if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + localObjOfEnv.data.rows[i].id))
                    {
                        string[] filesInDirectory = System.IO.Directory.GetFiles(Application.persistentDataPath + "/MainMenuData/" + localObjOfEnv.data.rows[i].id);
                        foreach (string str in filesInDirectory)
                        {
                            File.Delete(str);
                        }
                        // Remove data from directory first then remove from json
                        System.IO.Directory.Delete(Application.persistentDataPath + "/MainMenuData/" + localObjOfEnv.data.rows[i].id);
                    }
                    localObjOfEnv.data.rows.RemoveAt(i);
                    i = localObjOfEnv.data.rows.Count - 1;
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + "ObjofEnv.json", JsonUtility.ToJson(localObjOfEnv));
            }
            PlayerPrefs.SetInt("Filesystem", 1);
        }


        using (UnityWebRequest request = UnityWebRequest.Get(ConstantsGod.API_BASEURL+ ConstantsGod.GetAllMuseumsAPI))
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            request.SendWebRequest();
            while(!request.isDone)
            {
                yield return null;
            }
            ObjofMuseum = GetAllMuseumData(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    if (ObjofMuseum.success == true)
                    {
                        yield return StartCoroutine(GetMuseumsListNew());
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
                        if (ObjofEnv.success == false)
                        {
                            print("Hey success false " + ObjofEnv.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }


        using (UnityWebRequest request = UnityWebRequest.Get(ConstantsGod.API_BASEURL+ ConstantsGod.GETENVIRONMENTSAPINew +"/"+APIBaseUrlChange.instance.apiversion+"/1/25"))
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            request.SendWebRequest();
            while(!request.isDone)
            {
                yield return null;
            }
            ObjofEnv = GetAllData(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    if (ObjofEnv.success == true)
                    {
                        yield return StartCoroutine(GetWorldsListNew(1));
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
                        if (ObjofEnv.success == false)
                        {
                            print("Hey success false " + ObjofEnv.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }
    public bool localFileExist = false;
    public IEnumerator GetWorldsListNew(int a)
    {
        yield return null;
        localObjOfEnv = ObjofEnv;
        string fileName, folderName;
        if (a == 0)
        {
            fileName = "museums.json";
            folderName = "museums";
        }
        else
        {
            fileName = "ObjofEnv.json";
            folderName = "ObjofEnv";
        }

        //string potion = JsonUtility.ToJson(ObjofEnv);
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData") && System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName))
        {
            if (File.Exists(Application.persistentDataPath + "/MainMenuData/" + fileName))
            {
                localFileExist = true;
                localObjOfEnv = GetAllData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                // code for verification of data if excess data available at device which is not on server
                print(localObjOfEnv.data.count);
                int i = 0;

                bool noUpdate = true, noDelete = true;
                while (i < localObjOfEnv.data.rows.Count)
                {
                    int temp = i;
                    for (int j = 0; j < ObjofEnv.data.rows.Count; j++)
                    {
                        if (localObjOfEnv.data.rows[i].id == ObjofEnv.data.rows[j].id)
                        {
                            i++;
                            break;
                        }
                    }
                    if (temp == i)
                    {
                        noUpdate = false;
                        if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + localObjOfEnv.data.rows[i].id))
                        {
                            string[] filesInDirectory = System.IO.Directory.GetFiles(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + localObjOfEnv.data.rows[i].id);
                            foreach (string str in filesInDirectory)
                            {
                                File.Delete(str);
                            }
                            // Remove data from directory first then remove from json
                            System.IO.Directory.Delete(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + localObjOfEnv.data.rows[i].id);
                        }
                        localObjOfEnv.data.rows.RemoveAt(i);
                        i++;
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
                // code for verification of data if data is missing on device but available on device
                i = 0;
                while (i < ObjofEnv.data.rows.Count)
                {
                    int temp = i;
                    for (int j = 0; j < localObjOfEnv.data.rows.Count; j++)
                    {
                        if (ObjofEnv.data.rows[i].id == localObjOfEnv.data.rows[j].id)
                        {
                            // check for timestamp of both index to update data or continue as it is
                            i++;
                            break;
                        }
                    }
                    if (temp == i)
                    {
                        noDelete = false;
                        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofEnv.data.rows[i].id);
                        // Remove data from directory first then remove from json
                        localObjOfEnv.data.rows.Add(ObjofEnv.data.rows[i]);
                        i++;
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
                localObjOfEnv = GetAllData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                for (int j = 0; j < ObjofEnv.data.rows.Count; j++)
                {
                    for (int k = 0; k < localObjOfEnv.data.rows.Count; k++)
                    {
                        if (ObjofEnv.data.rows[j].id == localObjOfEnv.data.rows[k].id)
                        {
                            if (j != k)
                            {
                                RowList row = localObjOfEnv.data.rows[j];
                                localObjOfEnv.data.rows[j] = localObjOfEnv.data.rows[k];
                                localObjOfEnv.data.rows[k] = row;
                            }
                        }
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
                localObjOfEnv = GetAllData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                i = 0;
                while (i < ObjofEnv.data.rows.Count)
                {
                    int temp = i;
                    for (int j = 0; j < localObjOfEnv.data.rows.Count; j++)
                    {
                        if (ObjofEnv.data.rows[i].id == localObjOfEnv.data.rows[j].id)
                        {
                            if (ObjofEnv.data.rows[i].upload_timestamp != localObjOfEnv.data.rows[j].upload_timestamp)
                            {
                                if (!System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofEnv.data.rows[i].id))
                                {
                                    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofEnv.data.rows[i].id);
                                }
                                yield return StartCoroutine(GetDataFromLinkAndSaveInLocalDirctory(ObjofEnv.data.rows[i], folderName));
                            }
                            else
                            {
                                if (folderName == "museums")
                                {
                                    CreateEventPrefab(ObjofEnv.data.rows[i], true);
                                }
                                else
                                {
                                    CreateEventPrefab(ObjofEnv.data.rows[i], false);
                                }
                                //CreateEventPrefab(ObjofEnv.data.rows[i]);
                            }
                            i++;
                            break;
                        }
                    }
                    if (temp == i)
                    {
                        noDelete = false;
                        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofEnv.data.rows[i].id);
                        // Remove data from directory first then remove from json
                        localObjOfEnv.data.rows.Add(ObjofEnv.data.rows[i]);
                        i++;
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
                localObjOfEnv = GetAllData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
            }
            else
            {
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
                localObjOfEnv = GetAllData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                foreach (RowList row in localObjOfEnv.data.rows)
                {
                    row.upload_timestamp = "0000";
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
                yield return StartCoroutine(GetWorldsListNew(a));
            }
        }
        else
        {
            if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData"))
            {
                if (!System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName))
                {
                    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName);
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData");
                if (!System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName))
                {
                    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName);
                }
            }

            System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
            localObjOfEnv = GetAllData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
            foreach (RowList row in localObjOfEnv.data.rows)
            {
                row.upload_timestamp = "0000";
            }
            System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfEnv));
            yield return StartCoroutine(GetWorldsListNew(a));
        }
    }

    //-------------------------------------------------------------------
    public IEnumerator GetMuseumsListNew()
    {
        yield return null;
        localObjOfMuseum = ObjofMuseum;
        string fileName, folderName;
        fileName = "museums.json";
        folderName = "museums";
        //string potion = JsonUtility.ToJson(ObjofEnv);
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData") && System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName))
        {
            if (File.Exists(Application.persistentDataPath + "/MainMenuData/" + fileName))
            {
                localFileExist = true;
                localObjOfMuseum = GetAllMuseumData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                // code for verification of data if excess data available at device which is not on server
                print(localObjOfMuseum.data.count);
                int i = 0;
                bool noUpdate = true, noDelete = true;
                while (i < localObjOfMuseum.data.rows.Count)
                {
                    int temp = i;
                    for (int j = 0; j < ObjofMuseum.data.rows.Count; j++)
                    {
                        if (localObjOfMuseum.data.rows[i].id == ObjofMuseum.data.rows[j].id)
                        {
                            i++;
                            break;
                        }
                    }
                    if (temp == i)
                    {
                        noUpdate = false;
                        if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + localObjOfMuseum.data.rows[i].id))
                        {
                            string[] filesInDirectory = System.IO.Directory.GetFiles(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + localObjOfMuseum.data.rows[i].id);
                            foreach (string str in filesInDirectory)
                            {
                                File.Delete(str);
                            }
                            // Remove data from directory first then remove from json
                            System.IO.Directory.Delete(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + localObjOfMuseum.data.rows[i].id);
                        }
                        localObjOfMuseum.data.rows.RemoveAt(i);
                        i++;
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
                // code for verification of data if data is missing on device but available on device
                i = 0;
                while (i < ObjofMuseum.data.rows.Count)
                {
                    int temp = i;
                    for (int j = 0; j < localObjOfMuseum.data.rows.Count; j++)
                    {
                        if (ObjofMuseum.data.rows[i].id == localObjOfMuseum.data.rows[j].id)
                        {
                            // check for timestamp of both index to update data or continue as it is
                            i++;
                            break;
                        }
                    }
                    if (temp == i)
                    {
                        noDelete = false;
                        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofMuseum.data.rows[i].id);
                        // Remove data from directory first then remove from json
                        localObjOfMuseum.data.rows.Add(ObjofMuseum.data.rows[i]);
                        i++;
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
                localObjOfMuseum = GetAllMuseumData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                for (int j = 0; j < ObjofMuseum.data.rows.Count; j++)
                {
                    for (int k = 0; k < localObjOfMuseum.data.rows.Count; k++)
                    {
                        if (ObjofMuseum.data.rows[j].id == localObjOfMuseum.data.rows[k].id)
                        {
                            if (j != k)
                            {
                                MuseumInfo row = localObjOfMuseum.data.rows[j];
                                localObjOfMuseum.data.rows[j] = localObjOfMuseum.data.rows[k];
                                localObjOfMuseum.data.rows[k] = row;
                            }
                        }
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
                localObjOfMuseum = GetAllMuseumData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                i = 0;
                while (i < ObjofMuseum.data.rows.Count)
                {
                    int temp = i;
                    for (int j = 0; j < localObjOfMuseum.data.rows.Count; j++)
                    {
                        if (ObjofMuseum.data.rows[i].id == localObjOfMuseum.data.rows[j].id)
                        {
                            if (ObjofMuseum.data.rows[i].upload_timestamp != localObjOfMuseum.data.rows[j].upload_timestamp)
                            {
                                if (!System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofMuseum.data.rows[i].id))
                                {
                                    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofMuseum.data.rows[i].id);
                                }
                                yield return StartCoroutine(GetDataFromLinkAndSaveInLocalDirctory(ObjofMuseum.data.rows[i], folderName));
                            }
                            else
                            {
                                if (folderName == "museums")
                                {
                                    CreateEventPrefab(ObjofMuseum.data.rows[i], true);
                                }
                                else
                                {
                                    CreateEventPrefab(ObjofMuseum.data.rows[i], false);
                                }
                                //CreateEventPrefab(ObjofEnv.data.rows[i]);
                            }
                            i++;
                            break;
                        }
                    }
                    if (temp == i)
                    {
                        noDelete = false;
                        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + ObjofMuseum.data.rows[i].id);
                        // Remove data from directory first then remove from json
                        localObjOfMuseum.data.rows.Add(ObjofMuseum.data.rows[i]);
                        i++;
                    }
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
                localObjOfMuseum = GetAllMuseumData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
            }
            else
            {
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
                localObjOfMuseum = GetAllMuseumData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
                foreach (MuseumInfo row in localObjOfMuseum.data.rows)
                {
                    row.upload_timestamp = "0000";
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
                yield return StartCoroutine(GetMuseumsListNew());
            }
        }
        else
        {
            if (System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData"))
            {
                if (!System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName))
                {
                    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName);
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData");
                if (!System.IO.Directory.Exists(Application.persistentDataPath + "/MainMenuData/" + folderName))
                {
                    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/MainMenuData/" + folderName);
                }
            }

            System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
            localObjOfMuseum = GetAllMuseumData(File.ReadAllText(Application.persistentDataPath + "/MainMenuData/" + fileName));
            foreach (MuseumInfo row in localObjOfMuseum.data.rows)
            {
                row.upload_timestamp = "0000";
            }
            System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + fileName, JsonUtility.ToJson(localObjOfMuseum));
            yield return StartCoroutine(GetMuseumsListNew());
        }
    }



    private IEnumerator GetDataFromLinkAndSaveInLocalDirctory(MuseumInfo museumInfo, string folderName)
    {
        downloadCounter++;
        string fileName = "thumbnail.jpg";
        string url = museumInfo.thumbnail;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.LogError("Network Error");
            }
            else
            {
                byte[] fileData = www.downloadHandler.data;
                File.WriteAllBytes(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + museumInfo.id + "/" + fileName, fileData);
                bool updated = false;
                for (int j = 0; j < localObjOfMuseum.data.rows.Count; j++)
                {
                    if (localObjOfMuseum.data.rows[j].id == museumInfo.id)
                    {
                        updated = true;
                        localObjOfMuseum.data.rows[j].upload_timestamp = museumInfo.upload_timestamp;
                    }
                }
                if (!updated)
                {
                    localObjOfMuseum.data.rows.Add(museumInfo);
                }
                if (folderName == "museums")
                {
                    CreateEventPrefab(museumInfo, true);
                }
                else
                {
                    CreateEventPrefab(museumInfo, false);
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + fileName, JsonUtility.ToJson(localObjOfEnv));
            }
            www.Dispose();
        }
        downloadCounter--;
    }

    //-------------------------------------------------------------------
    private void CreateEventPrefab(MuseumInfo museumInfo, bool isMuseum)
    {
        if (UsePlatformEventDataNewUsman(museumInfo))
        {
            eventPrefabObject = Instantiate(eventPrefab);
            eventPrefabObject.transform.SetParent(ListContent);
            FeedEventPrefab _event = eventPrefabObject.GetComponent<FeedEventPrefab>();
            _event.isMuseum = isMuseum;
            _event.idOfObject = museumInfo.id;
            _event.uploadTimeStamp = museumInfo.upload_timestamp;
            _event.m_EnvironmentName = museumInfo.name;
            if (!_event.m_EnvironmentName.IsNullOrEmpty())
            {
#if UNITY_EDITOR
                //CreateLightingAsset(_event); //Create Lighting Asset and Sky Box if it doesn't Exist
#endif
            }
            _event.m_ThumbnailDownloadURL = museumInfo.thumbnail;
            _event.m_BannerLink = museumInfo.banner;
            _event.m_WorldDescription = museumInfo.description;
            int.TryParse(museumInfo.id, out _event.m_PressedIndex);
            CheckPlatformEventFileLinkNew(_event, museumInfo);
            eventPrefabObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void CreateEventPrefab(RowList row, bool isMuseum)
    {
        if (UsePlatformEventDataNewUsman(row))
        {
            eventPrefabObject = Instantiate(eventPrefab);
            eventPrefabObject.transform.SetParent(ListContent);
            FeedEventPrefab _event = eventPrefabObject.GetComponent<FeedEventPrefab>();
            _event.isMuseum = isMuseum;
            _event.idOfObject = row.id;
            _event.uploadTimeStamp = row.upload_timestamp;
            _event.m_EnvironmentName = row.environment_name;
            if (!_event.m_EnvironmentName.IsNullOrEmpty())
            {
#if UNITY_EDITOR
                CreateLightingAsset(_event); //Create Lighting Asset and Sky Box if it doesn't Exist
#endif
            }
            _event.m_ThumbnailDownloadURL = row.thumbnail;
            _event.m_BannerLink = row.banner;
            _event.m_WorldDescription = row.description;
            int.TryParse(row.id, out _event.m_PressedIndex);
            CheckPlatformEventFileLinkNew(_event, row);
            eventPrefabObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    public IEnumerator GetDataFromLinkAndSaveInLocalDirctory(RowList row, string folderName)
    {
        downloadCounter++;
        string fileName = "thumbnail.jpg";
        string url = row.thumbnail;

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.LogError("Network Error");
            }
            else
            {
                byte[] fileData = www.downloadHandler.data;
                File.WriteAllBytes(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + row.id + "/" + fileName, fileData);
                bool updated = false;
                for (int j = 0; j < localObjOfEnv.data.rows.Count; j++)
                {
                    if (localObjOfEnv.data.rows[j].id == row.id)
                    {
                        updated = true;
                        localObjOfEnv.data.rows[j].upload_timestamp = row.upload_timestamp;
                    }
                }
                if (!updated)
                {
                    localObjOfEnv.data.rows.Add(row);
                }
                if (folderName == "museums")
                {
                    CreateEventPrefab(row, true);
                }
                else
                {
                    CreateEventPrefab(row, false);
                }
                System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + fileName, JsonUtility.ToJson(localObjOfEnv));
            }
            www.Dispose();
        }
        downloadCounter--;
    }

    private void CreateLightingAsset(FeedEventPrefab _event)
    {
        string path = "Assets/Resources/Environment Data/" + _event.m_EnvironmentName + " Data";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        EnvironmentProperties Prop = null;
        if (!Directory.Exists(path + "/LightingData"))
        {
            Directory.CreateDirectory(path + "/LightingData");
            Prop = ScriptableObject.CreateInstance<EnvironmentProperties>();
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(Prop, path + "/LightingData/LightingData.asset");
            AssetDatabase.SaveAssets();
#endif
        }
    }

    void CheckPlatformEventFileLinkNew(FeedEventPrefab _event, RowList row)
    {
#if UNITY_ANDROID
        //eventPrefabObject.gameObject.GetComponent<FeedEventPrefab>().m_FileLink = envList.data[i].android_file;
        _event.m_FileLink = row.android_file;
#endif
#if UNITY_IOS
                _event.m_FileLink = row.ios_file;
#endif
#if UNITY_STANDALONE
                _event.m_FileLink = row.standalone_file;
#endif
    }

    void CheckPlatformEventFileLinkNew(FeedEventPrefab _event, MuseumInfo row)
    {
#if UNITY_ANDROID
        //eventPrefabObject.gameObject.GetComponent<FeedEventPrefab>().m_FileLink = envList.data[i].android_file;
        _event.m_FileLink = row.android_file;
#endif
#if UNITY_IOS
                _event.m_FileLink = row.ios_file;
#endif
#if UNITY_STANDALONE
                _event.m_FileLink = row.standalone_file;
#endif
    }

    bool UsePlatformEventDataNewUsman(RowList row)
    {
#if UNITY_STANDALONE
            if (row.standalone_file != null)
            return true;
#endif
#if UNITY_ANDROID
        //if (envList.data[i].android_file != null)
        if (row.android_file != null)
            return true;
#endif
#if UNITY_IOS
               if (row.ios_file != null)
               return true;
#endif
        return false;
    }

    bool UsePlatformEventDataNewUsman(MuseumInfo row)
    {
#if UNITY_STANDALONE
            if (row.standalone_file != null)
            return true;
#endif
#if UNITY_ANDROID
        //if (envList.data[i].android_file != null)
        if (row.android_file != null)
            return true;
#endif
#if UNITY_IOS
               if (row.ios_file != null)
               return true;
#endif
        return false;
    }

    bool UsePlatformEventDataNew(int currentIndex)
    {
#if UNITY_STANDALONE
            if (ObjofEnv.data.rows[currentIndex].standalone_file != null)
            return true;
#endif
#if UNITY_ANDROID
        //if (envList.data[i].android_file != null)
        if (ObjofEnv.data.rows[currentIndex].android_file != null)
            return true;
#endif
#if UNITY_IOS
               if (ObjofEnv.data.rows[currentIndex].ios_file != null)
               return true;
#endif
        return false;
    }
    /// <Irfan New API ENDS here>



    /////// ***************************  START OlD APIs  ************************////////
    public void CallGetEnvAPI()
    {
        StartCoroutine(GetEnvAPI());
    }
    public IEnumerator GetEnvAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", "piyush55");
        form.AddField("isMine", "false");
        form.AddField("limit", "50");
        form.AddField("page", "1");
        using (UnityWebRequest www = UnityWebRequest.Post(ConstantsGod.API_BASEURL+ ConstantsGod.GETENVIRONMENTSAPI, form))
        {
            //Loading.Instance.ShowLoading();
            www.SetRequestHeader("Authorization", ConstantsGod.JWTTOKEN);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                yield return null;
            }
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                print("" + Gods.DeserializeJSON<EnvList>(www.downloadHandler.text.Trim()).ToString());
                envList = Gods.DeserializeJSON<EnvList>(www.downloadHandler.text.Trim());
                eventsCount = envList.data.Count;
                if (operation.isDone)
                {
                    //Loading.Instance.HideLoading();
                    GetWorldsList();
                }
            }
        }
    }
    public void GetWorldsList()
    {
        for (int i = 0; i < eventsCount; i++)
        {
            if (UsePlatformEventData(i))
            {
                eventPrefabObject = Instantiate(eventPrefab);
                eventPrefabObject.transform.SetParent(ListContent);
                FeedEventPrefab _event = eventPrefabObject.GetComponent<FeedEventPrefab>();
                _event.m_EnvironmentName = envList.data[i].environment_name;
                _event.m_ThumbnailDownloadURL = envList.data[i].thumbnail;
                _event.m_BannerLink = envList.data[i].banner;
                _event.m_WorldDescription = envList.data[i].description;
                if (_event.m_EnvironmentName.Contains("CRYPTO NINJA VILLAGE"))
                {
                    _event.eviroment_Name.GetComponent<FeedEventTextClipper>().PreferredLength = 22;
                    Debug.Log(_event.m_WorldDescription);
                }
                _event.m_PressedIndex = i;
                CheckPlatformEventFileLink(_event, i);
                eventPrefabObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    void CheckPlatformEventFileLink(FeedEventPrefab _event, int currentIndex)
    {
#if UNITY_ANDROID
        //eventPrefabObject.gameObject.GetComponent<FeedEventPrefab>().m_FileLink = envList.data[i].android_file;
        _event.m_FileLink = envList.data[currentIndex].ios_file;
#endif
#if UNITY_IOS
                _event.m_FileLink = envList.data[currentIndex].ios_file;
#endif
#if UNITY_STANDALONE
                _event.m_FileLink = envList.data[currentIndex].standalone_file;
#endif
    }
    bool UsePlatformEventData(int currentIndex)
    {
#if UNITY_STANDALONE
            if (envList.data[currentIndex].standalone_file != null)
            return true;
#endif
#if UNITY_ANDROID
        //if (envList.data[i].android_file != null)
        if (envList.data[currentIndex].ios_file != null)
            return true;
#endif
#if UNITY_IOS
               if (envList.data[currentIndex].ios_file != null)
               return true;
#endif
        return false;
    }
    /////// *************************** END OlD APIs  ************************////////
    public IEnumerator DownloadFile()
    {
        string url = FeedEventPrefab.m_EnvDownloadLink;
        string nameOfFile = FeedEventPrefab.m_EnvDownloadLink.Replace('/', '0');
        nameOfFile = nameOfFile.Replace(':', '0');
        nameOfFile = nameOfFile.Replace('-', '0');
        nameOfFile = nameOfFile.Replace('.', '0');
        nameOfFile = nameOfFile.Replace('+', '0');
        string uploadTimeStampInfo = FeedEventPrefab.m_timestamp;
        bool downloadFile = true;
        if (File.Exists(Application.persistentDataPath + "/" + nameOfFile + ".txt"))
        {
            string fileData = File.ReadAllText(Application.persistentDataPath + "/" + nameOfFile + ".txt");
            if (fileData == uploadTimeStampInfo)
            {
                downloadFile = false;
            }
        }
        if (File.Exists(Application.persistentDataPath + "/" + nameOfFile) && !downloadFile)
        {
            //File.ReadAllText(Application.persistentDataPath + "/" + nameOfFile + ".txt");									 
            //LoadingManager.Instance.ShowLoading();
            LoadingHandler.Instance.ShowLoading();
            //LoadingManager.Instance.FillLoading(0);
            LoadingHandler.Instance.UpdateLoadingSlider(0);
            LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");
            yield return new WaitForSeconds(.3f);
            //LoadingManager.Instance.FillLoading(.5f);
            LoadingHandler.Instance.UpdateLoadingSlider(0.3f, true);
            yield return new WaitForSeconds(.2f);
            //LoadingManager.Instance.FillLoading(.7f);
            LoadingHandler.Instance.UpdateLoadingSlider(.5f, true);
            print("file Found");
            if (File.Exists(Application.persistentDataPath + "/" + "Environment"))
            {
                File.WriteAllBytes(Application.persistentDataPath + "/" + /*EventPrefabClick.EnvName*/ "Environment", File.ReadAllBytes(Application.persistentDataPath + "/" + nameOfFile));
                //FileUtil.ReplaceFile(Application.persistentDataPath + "/" + nameOfFile, Application.persistentDataPath + "/" + /*EventPrefabClick.EnvName*/ "Environment");
            }
            //File.Copy(Application.persistentDataPath + "/" + nameOfFile, Application.persistentDataPath + "/" + /*EventPrefabClick.EnvName*/ "Environment");
            //File.Replace(Application.persistentDataPath + "/" + nameOfFile, Application.persistentDataPath + "/" + /*EventPrefabClick.EnvName*/ "Environment", "DeleteNow");
            yield return new WaitForSeconds(1f);
            LoadingHandler.Instance.UpdateLoadingSlider(.7f, true);
            GameManager.Instance.DestroyAllLoadedTexture();
            print("Call Addressable scene here after getting enviornment from local memory");
            XanaConstants.xanaConstants.EnviornmentName = FeedEventPrefab.m_EnvName;
            if (XanaConstants.xanaConstants.buttonClicked.GetComponent<FeedEventPrefab>().isMuseum)
            {
                XanaConstants.xanaConstants.IsMuseum = true;
                //SceneManager.LoadScene("DynamicMuseum");
            }
            else
            {
                XanaConstants.xanaConstants.IsMuseum = false;
                //SceneManager.LoadScene(1);
            }
            SceneManager.LoadScene(1);
        }
        else
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                var operation = www.SendWebRequest();
                //LoadingManager.Instance.ShowLoading();
                LoadingHandler.Instance.ShowLoading();
                LoadingHandler.Instance.UpdateLoadingStatusText("Downloading World");
                while (!operation.isDone)
                {
                    //  Debug.Log(www.downloadProgress);
                    //LoadingManager.Instance.FillLoading(www.downloadProgress);
                    LoadingHandler.Instance.UpdateLoadingSlider(www.downloadProgress * 0.7f);
                    yield return null;
                }
                if (www.isHttpError || www.isNetworkError)
                {
                    //LoadingManager.Instance.HideLoading();

                    //LoadingHandler.Instance.HideLoading();
                    Screen.orientation = ScreenOrientation.Portrait;
                    StoreManager.instance.BackToHomeFromCharCustomization();
                    Debug.LogError("Network Error");
                }
                else
                {
                    byte[] fileData = www.downloadHandler.data;
                    string filePath = Application.persistentDataPath + "/" + /*EventPrefabClick.EnvName*/ "Environment";
                    File.WriteAllBytes(filePath, fileData);
                    File.WriteAllBytes(Application.persistentDataPath + "/" + nameOfFile, fileData);
                    File.WriteAllText(Application.persistentDataPath + "/" + nameOfFile + ".txt", uploadTimeStampInfo);
                    if (operation.isDone)
                    {
                        GameManager.Instance.DestroyAllLoadedTexture();
                        print("Call Addressable scene here after getting enviornment from Internet First Time");
                        XanaConstants.xanaConstants.EnviornmentName = FeedEventPrefab.m_EnvName;
                        if (XanaConstants.xanaConstants.buttonClicked.GetComponent<FeedEventPrefab>().isMuseum)
                        {
                            XanaConstants.xanaConstants.IsMuseum = true;
                            //SceneManager.LoadScene("DynamicMuseum");
                        }
                        else
                        {
                            XanaConstants.xanaConstants.IsMuseum = false;
                            //SceneManager.LoadScene(1);
                        }
                        SceneManager.LoadScene(1);
                    }
                }
            }
        }
    }

    public void JoinEvent()
    {
        if (!UserRegisterationManager.instance.LoggedIn && PlayerPrefs.GetInt("IsLoggedIn") == 0)
        {
            UIManager.Instance.LoginRegisterScreen.transform.SetAsLastSibling();
            UIManager.Instance.LoginRegisterScreen.SetActive(true);
        }
        else
        {
            Debug.LogError(Launcher.sceneName);
            LoadingHandler.Instance.Loading_WhiteScreen.SetActive(true);
            Screen.orientation = ScreenOrientation.LandscapeLeft;
#if UNITY_EDITOR
            XanaConstants.xanaConstants.orientationchanged = true;
            StartCoroutine(DownloadFile());
#else
            InvokeRepeating("Check_Orientation", 1, 1);
#endif

        }
    }


    private void Check_Orientation()
    {
        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            XanaConstants.xanaConstants.orientationchanged = true;
        }
        if (XanaConstants.xanaConstants.orientationchanged)
        {
            StartCoroutine(DownloadFile());
            LoadingHandler.Instance.Loading_WhiteScreen.SetActive(false);
            CancelInvoke("Check_Orientation");
        }
    }

    public void PlayWorld()
    {
        LoadingHandler.Instance.Loading_WhiteScreen.SetActive(true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
#if UNITY_EDITOR
        XanaConstants.xanaConstants.orientationchanged = true;
        StartCoroutine(DownloadFile());
#else
            InvokeRepeating("Check_Orientation", 1, 1);
#endif
    }
}

public partial class EnvList
{
    public bool success { get; set; }
    public List<EnvDatum> data { get; set; }
}

public partial class EnvDatum
{
    public string _id { get; set; }
    public string username_db { get; set; }
    public string environment_name { get; set; }
    public string user_limit { get; set; }
    public string event_listing { get; set; }
    public string thumbnail { get; set; }
    public string android_file { get; set; }
    public string standalone_file { get; set; }
    public string ios_file { get; set; }
    public string banner { get; set; }
    public string description { get; set; }
}