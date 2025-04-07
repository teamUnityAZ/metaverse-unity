using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class LoadingSavingAvatar : MonoBehaviour
{
    public Image LoadingImg;
    private bool Once;
    private float time;
    public float TotalTimer;
    public bool callFirst = false;
    public static string version = "";

    // Start is called before the first frame update
    void Start()
    {
        Once = false;
        LoadingImg.fillAmount = 0;
        time = 0;


    }


    // Update is called once per frame
    void Update()
    {
        if (!callFirst)
        {
            if (APIBaseUrlChange.instance.IsXanaLive)
            {
                callFirst = true;
                StartCoroutine(getVersion());
            }
            else
            {
                callFirst = true;
                StartCoroutine(getVersion());
            }

        }


        if (!Once)
        {
            time += Time.deltaTime;
            LoadingImg.fillAmount = time / TotalTimer;
            if (time >= TotalTimer)
            {
                Once = true;
                CallWelcome();
                // StartCoroutine(versionCheck());

            }
        }
    }


    public void CallWelcome()
    {
        if (UserRegisterationManager.instance != null)
            UserRegisterationManager.instance.ShowWelcomeScreen();
    }

    public IEnumerator getVersion()
    {
        Debug.Log("url====" + ConstantsGod.API_BASEURL);
        UnityWebRequest uwr = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.GetVersion);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {

            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            try
            {
                VersionDetails bean = Gods.DeserializeJSON<VersionDetails>(uwr.downloadHandler.text.ToString().Trim());
                if (bean.success)
                {
                    version = bean.data.name;
                }
            }
            catch
            {

            }
        }
    }

    public class VersionData
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class VersionDetails
    {
        public bool success { get; set; }
        public VersionData data { get; set; }
        public string msg { get; set; }
    }

}
