using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;

public class YoutubeAPIHandler : MonoBehaviour
{

    private StreamResponse _response;

    private int DataIndex = 4;
    public StreamData Data;

    public IEnumerator GetStream()
    {
        WWWForm form = new WWWForm();

        //form.AddField("token", "piyush55");
        if (FeedEventPrefab.m_EnvName.Contains("DJ Event"))
        {
            using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.SHARELINKS))
            {

                www.timeout = 10;

                yield return www.SendWebRequest();

                while (!www.isDone)
                {
                    yield return null;
                }
                if (www.isHttpError || www.isNetworkError)
                {
                    _response = null;
                    Data = null;
                    Debug.LogError("Youtube API returned no result");
                }
                else
                {
                    _response = Gods.DeserializeJSON<StreamResponse>(www.downloadHandler.text.Trim());
                    if (_response != null)
                    {
                        string incominglink = _response.data.link;
                        if (!incominglink.Equals(" "))
                        {
                            Data = new StreamData(incominglink, _response.data.isLive, _response.data.isPlaying);
                        }
                        else
                        {
                            Debug.Log("No Link Found Turning off player");
                            Data = null;
                        }
                    }

                }
            }
        }
        else if (FeedEventPrefab.m_EnvName.Contains("XANA Festival Stage")) {
            using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.SHAREDEMOS))
            {

                www.timeout = 10;

                yield return www.SendWebRequest();

                while (!www.isDone)
                {
                    yield return null;
                }
                if (www.isHttpError || www.isNetworkError)
                {
                    _response = null;
                    Data = null;
                    Debug.LogError("Youtube API returned no result");
                }
                else
                {
                    _response = Gods.DeserializeJSON<StreamResponse>(www.downloadHandler.text.Trim());
                    if (_response != null)
                    {
                        string incominglink = _response.data.link;
                        if (!incominglink.Equals(" "))
                        {
                            Data = new StreamData(incominglink, _response.data.isLive, _response.data.isPlaying);
                           // print("Stage 3 video link:" + Data);
                        }
                        else
                        {
                            Debug.Log("No Link Found Turning off player");
                            Data = null;
                        }
                    }

                }
            }




        }
    }
}

public class StreamData
{
    public string URL;
    public bool IsLive;
    public bool isPlaying;

    public StreamData(string URL, bool isLive,bool isPlaying)
    {
        this.URL = URL;
        this.IsLive = isLive;
        this.isPlaying = isPlaying;
    }

}

public partial class StreamResponse
{   
    public bool success { get; set; }
    public string msg { get; set; }
    public IncomingData data { get; set; }
    
    
}

public partial class IncomingData
{  
    public long id { get; set; }
    public string link { get; set; }
    public bool isLive { get; set; }
    public object createdAt { get; set; }
    public object updatedAt { get; set; }
    public bool isPlaying { get; set; }
}
