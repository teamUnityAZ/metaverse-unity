using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class YTStreamAPI : MonoBehaviour
{
    public static YTStreamAPI instance;
    public YTStreamResponse response;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine(YTStreamCall());
    }

    public void Recheck()
    {
        StartCoroutine(YTStreamCall());
    }

    public IEnumerator YTStreamCall()
    {
        WWWForm form = new WWWForm();

        form.AddField("token", "piyush55");
       
        using (UnityWebRequest www = UnityWebRequest.Post(ConstantsGod.API+"xanaServer/getStream", form))
        {
         
            www.timeout = 10;

            yield return www.SendWebRequest();

            while (!www.isDone)
            {
                //Debug.Log(www.downloadProgress);
                yield return null;
            }
            if (www.isHttpError || www.isNetworkError)
            {
                //Debug.LogError("Network Error");
            }
            else
            {                                
                response = Gods.DeserializeJSON<YTStreamResponse>(www.downloadHandler.text.Trim());

                if (www.isDone)
                {
                    Gamemanager._InstanceGM.youtubecheck();
                    //Debug.Log(response.msg);
                    //Debug.Log(response.data[0].streamName);
                    //Debug.Log(response.data[0].streamLink);
                }         

            }
        }
     
    }

}



public partial class YTStreamResponse
{   
    public bool success { get; set; }
   
    public string msg { get; set; }
    
    public List<VideoData> data { get; set; }
}

public partial class VideoData
{  
    public string _id { get; set; }
    
    public object createdBy { get; set; }
   
    public string streamName { get; set; }
   
    public string streamLink { get; set; }
}