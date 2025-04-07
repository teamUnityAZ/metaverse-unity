using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadAndShowMuseum : MonoBehaviour
{
    AssetBundle museums;

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        //museums = AssetBundle.loa
        Invoke( "DownloadMuseum",.5f);
        //museums = AssetBundle.LoadFromFile("D:/Enviornment Checking/AssetBundles/AndroidLevel1/mainscene");
        //XanaConstants.xanaConstants.museumAssetLoaded = museums;
        //string[] scenesInList = museums.GetAllScenePaths();
        //print("Loading Scene " + scenesInList[0]);
        //LoadingHandler.Instance.UpdateLoadingStatusText("Loading Scene");
        //SceneManager.LoadScene(scenesInList[0]);
    }

    public  void DownloadMuseum()
    {
        print("DownloadStart");
        string nameOfFile = XanaConstants.xanaConstants.museumDownloadLink;
        nameOfFile = nameOfFile.Replace('/', '0');
        nameOfFile = nameOfFile.Replace(':', '0');
        nameOfFile = nameOfFile.Replace('-', '0');
        nameOfFile = nameOfFile.Replace('.', '0');
        nameOfFile = nameOfFile.Replace('+', '0');
        if(File.Exists(Application.persistentDataPath + "/" + nameOfFile))
        {
            print("File Exist");
            // load asset and open scene
            museums = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + nameOfFile);
            XanaConstants.xanaConstants.museumAssetLoaded = museums;
            string[] scenesInList = museums.GetAllScenePaths();
            print("Loading Scene " + scenesInList[0]);
            LoadingHandler.Instance.UpdateLoadingStatusText("Loading Scene");
            SceneManager.LoadScene(scenesInList[0]);
        }
        else
        {
            // Download Asset and open
            print("Starting Download ");
            StartCoroutine(StartEnviornmentDownload(nameOfFile));
        }
        
    }

    private void OnDestroy()
    {
        
    }


    public IEnumerator StartEnviornmentDownload(string nameOfFile)
    {
        
        yield return new WaitForEndOfFrame();
        //UnityWebRequest uwr = XanaConstants.xanaConstants.museumDownloadLink;
        using (UnityWebRequest request = UnityWebRequest.Get(XanaConstants.xanaConstants.museumDownloadLink))
        {
            //request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            // async progress = request.SendWebRequest();
            LoadingHandler.Instance.UpdateLoadingStatusText("Downloading World");
            yield return request.SendWebRequest();
            print("WebRequest COmpleted");
            //ObjofMainCategory = GetAllDataNewAPI(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    byte[] fileData = request.downloadHandler.data;
                    print("Saving file in memory");
                    File.WriteAllBytes(Application.persistentDataPath + "/" + nameOfFile, fileData);
                    LoadingHandler.Instance.UpdateLoadingStatusText("Downloading Completed");
                    DownloadMuseum();
                    //if (ObjofMainCategory.success == true)
                    //{
                    //    SaveAllMainCategoriesToArray();
                    //}
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
                        //if (ObjofMainCategory.success == false)
                        //{
                        //    print("Hey success false " + ObjofMainCategory.msg);
                        //}
                    }
                }
            }
            request.Dispose();
        }
    }
}
