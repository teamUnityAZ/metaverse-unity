using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class AssetBundleManager : MonoBehaviour
{
   // private string test = "https://www.dropbox.com/s/2x3gttux3udlu4v/test?dl=1";
    
            private string test = "https://cdn.xana.net/unitydata/Clothes/GirlC16hair/girlc16hairs.ios";

    private string ShoeIOS = "https://cdn.xana.net/unitydata/Clothes/ShoesC8/shoesc8.ios";
    public GameObject CurrentStoreObj;
    public Button DownloadBtn;
    public Button LoadBtn;
    public string AssetName;
    public Text LoadingTxt;
    private bool LoadBool;
    // Start is called before the first frame update
    void Start()
    {
        LoadBool = false;
     }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFileFtn()
    {
      if( !LoadFile(AssetName))
        {
            StartCoroutine(DownloadFile(test, AssetName));
         }
    }
     
     public IEnumerator DownloadFile(string link ,string _AssetName)
      {
             print("Now download file not exist in directory");
            using (UnityWebRequest www = UnityWebRequest.Get(link))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    Debug.Log(www.downloadProgress);
                    yield return null;
                }
                if (www.isHttpError || www.isNetworkError)
                {
                    Debug.LogError("Network Error");
                }
                else
                {
                   byte[] fileData = www.downloadHandler.data;
                    string filePath = Application.persistentDataPath + "/" + /*EventPrefabClick.EnvName*/ _AssetName;
                     File.WriteAllBytes(filePath, fileData);
                    if (operation.isDone)
                    {
                    LoadBool = true;
                    if (LoadBool)
                    {
                        LoadingTxt.text = "Downloaded and than Load";
                    }
                    print("store File Saved");
                    LoadFile(_AssetName);
                    }
                }
            }  
     }
    public void deleteDirectory()
    {
        DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
        dataDir.Delete(true);
    }
    bool LoadFile(string _Assetname )
    {
        string filePath = Application.persistentDataPath + "/" + _Assetname;
          print("loading file....");
        if (!File.Exists(filePath))
        {
            print("exists");
            return false;
        }
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + "MySavedShirt");
   
        print(Application.persistentDataPath);
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return false;
        }
        print(myLoadedAssetBundle.name);
         var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(myLoadedAssetBundle.name) as GameObject;
        CurrentStoreObj = prefab;  
          Instantiate(prefab);
        if(!LoadBool)
        {
            LoadingTxt.text = "Load From Memory";
         }
         return true;
    }  
    public void CreateFile()
    {
        string file = Application.persistentDataPath + "/" + "AssetFolder";
        DirectoryInfo dataDir = new DirectoryInfo(file);
        if (!dataDir.Exists)
        {
            dataDir.Create();
            print("created");
        }   
        else
        {
            print("Already Existed");
        }
     }
    public void Delete2()
    {
        string file = Application.persistentDataPath + "/" + "AssetFolder";
         DirectoryInfo dataDir = new DirectoryInfo(file);
        if (dataDir.Exists)
             dataDir.Delete(true);
      }
}
  