using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class APIBaseUrlChange : MonoBehaviour
{
    public bool IsXanaLive = false;
    public static APIBaseUrlChange instance;
    private bool firsttimecallXanaLive = false;
    public string apiversion ="";
    // Start is called before the first frame update
    void Start()
    {
      

    }

    private void Awake()
    {
        instance = this;
        // DontDestroyOnLoad(this);
        if (IsXanaLive)
        {
            ConstantsGod.API_BASEURL = "https://app-api.xana.net";
            ConstantsGod.API_BASEURL_XANALIA = "https://api.xanalia.com";

            
        }
        else
        {
            ConstantsGod.API_BASEURL = "https://api-test.xana.net";
            ConstantsGod.API_BASEURL_XANALIA = "https://testapi.xanalia.com";
        }
        

    }

    // Update is called once per frame
    void Update()
    {

        if (IsXanaLive)
        {
            if (!firsttimecallXanaLive)
            {
                firsttimecallXanaLive = true;
                ConstantsGod.API_BASEURL = "https://app-api.xana.net";
                ConstantsGod.API_BASEURL_XANALIA = "https://api.xanalia.com";
            }

        }
        else
        {
            if (firsttimecallXanaLive)
            {
                firsttimecallXanaLive = false;
                ConstantsGod.API_BASEURL = "https://api-test.xana.net";
                ConstantsGod.API_BASEURL_XANALIA = "https://testapi.xanalia.com";
            }

        }
    }

    IEnumerator getServerStatus()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("https://api-test.xana.net/auth/get-server-status");
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {

            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            try
            {
                Debug.Log("Response server===" + uwr.downloadHandler.text);
                GetServerDetils bean = Gods.DeserializeJSON<GetServerDetils>(uwr.downloadHandler.text.ToString().Trim());
                if (bean.success)
                {
                    IsXanaLive = bean.data.isServerLive;
                }
                else
                {
                    IsXanaLive = false;
                }
            }
            catch
            {

            }
        }
    }

    public class Data
    {
        public int id { get; set; }
        public bool isServerLive { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class GetServerDetils
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public string msg { get; set; }
    }
}
