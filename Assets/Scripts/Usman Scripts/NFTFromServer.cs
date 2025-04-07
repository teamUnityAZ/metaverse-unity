using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using XanaNFT;
using System;

public class NFTFromServer : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> spawnPoints;
    public Transform picsSpawnPoints;
    public bool updateVar;
    public List<MetaData> metaData;
    public List<CreatorDetails> creatorDetails;
    public List<TokenDetails> tokenDetails;
    public string nftLink;
    public string creatorLink;
    public static int allObjects;
    public static int currDownloadedNo=1;
    int totalNft=0;
    public static Action<int> OnImageDownload;
    void Start()
    {
        //picsSpawnPoints = transform.GetChild(0);
        int i = 0;
        while(picsSpawnPoints.childCount > i)
        {
            spawnPoints.Add( picsSpawnPoints.GetChild(i).gameObject);
            picsSpawnPoints.GetChild(i).gameObject.SetActive(false);
            i++;
        }
        Invoke("GetNFTDataDetails", .1f);


        OnImageDownload += x =>
        {
            if (x == totalNft)
            {
                AssetBundle.UnloadAllAssetBundles(false);
                Resources.UnloadUnusedAssets();
            }
        };
    }

  
    public void GetNFTDataDetails() {
        StartCoroutine(GetNftData());
    }

    XanaNftDetails nftDetails;
   
    IEnumerator GetNftData()
    {
        while(Application.internetReachability == NetworkReachability.NotReachable)
        {
            yield return new WaitForEndOfFrame();
            print("Internet Not Reachable");
        }
        yield return null;
        // = new XanaNftDetails();
        using (UnityWebRequest request = UnityWebRequest.Get("https://api.xanalia.com/xana-museum/nfts"))
        {
            //print("request");
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            //print("Sending Web Request");
            yield return request.SendWebRequest();
            //print("Web Request completed");
            
           
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    nftDetails = GetAllData(request.downloadHandler.text);
                    yield return new WaitForEndOfFrame();
                    if (nftDetails.success == true)
                    {
                        //CreatorDetails creatorDetails = new CreatorDetails();
                        int i = 0;
                        totalNft = 0;
                        //print(nftDetails.data.Count);
                        foreach (Datum d in nftDetails.data)
                        {
                            if (d.tokenDetails.metaData.thumbnft != null) 
                            {
                                allObjects++;
                                totalNft++;
                            }
                        }
                        //foreach (Datum d in nftDetails.data)
                        //{
                        //    if (d.tokenDetails.creatorDetails.username != null)
                        //    {
                        //        print("my user name " + d.tokenDetails.creatorDetails.username);
                        //    }
                        //}
                        foreach (Datum d in nftDetails.data)
                        {
                            if(i>=spawnPoints.Count)
                            break;
                            metaData.Add(d.tokenDetails.metaData);
                            //print(d.tokenDetails.metaData.thumbnft);
                            //print(d.tokenDetails.metaData.thumbnft.EndsWith(".mp4"));
                            spawnPoints[i].SetActive(true);
                            spawnPoints[i].AddComponent<MetaDataInPrefab>().metaData = d.tokenDetails.metaData;
                            spawnPoints[i].GetComponent<MetaDataInPrefab>().creatorDetails = d.creatorDetails;
                            spawnPoints[i].GetComponent<MetaDataInPrefab>().nftLink = d.nftLink;
                            spawnPoints[i].GetComponent<MetaDataInPrefab>().creatorLink = d.creatorLink;
                            spawnPoints[i].GetComponent<MetaDataInPrefab>().tokenDetails = d.tokenDetails;
                            //CreatorDetails cd = d.tokenDetails.creatorDetails;
                            // print("usman testing "+ d.tokenDetails.metaData.name);
                            spawnPoints[i].GetComponent<MetaDataInPrefab>().StartNow();
                            i++;
                          // print("my user name nothing" + d.creatorDetails.username);
                            // create prefabs of meta data recieved
                        }
                        //Resources.UnloadUnusedAssets();
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    yield return StartCoroutine(GetNftData());
                    print("Network Error");
                }
                else
                {
                    if (request.error != null)
                    {
                        yield return StartCoroutine(GetNftData());
                        if (nftDetails.success == false)
                        {
                            print("Hey success false " + nftDetails);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }
    public static void RemoveOne()
    {
        allObjects--;
        if (allObjects == 0)
        {
            print("Scene Is Ready To Display");
        }
    }

    public XanaNftDetails GetAllData(string m_JsonData)
    {
        print(m_JsonData);
       
        XanaNftDetails JsonDataObj = new XanaNftDetails();
        JsonDataObj = JsonUtility.FromJson<XanaNftDetails>(m_JsonData);
        return JsonDataObj;
    }
}
