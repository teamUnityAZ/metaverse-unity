﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MaintenanceScreenHandler : MonoBehaviour
{

    public GameObject m_ScreenUI;
    public GameObject updatePanel;
    public static bool checkOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        if (checkOnce)
        {
            checkOnce = false;
            string platform;
#if UNITY_ANDROID
            platform = "android";
#endif
#if UNITY_IOS
            platform = "ios";
#endif
            StartCoroutine(CheckForMaintenanceScreen("xana", platform, Application.version));
        }
    }

    public IEnumerator CheckForMaintenanceScreen(string type, string platform, string version)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.MaintenanceAPI + type + "/" + platform + "/" + version);

        uwr.SendWebRequest();
        while (!uwr.isDone)
        {
            yield return null;
        }

        //Debug.LogError(uwr.downloadHandler.text);

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            try
            {
                MaintenanceData response = JsonUtility.FromJson<MaintenanceData>(uwr.downloadHandler.text);
                if (response.success)
                {
                    if (response.data.isMaintenance)
                    {
                        m_ScreenUI.SetActive(true);
                    }
                    else if(response.data.isUpdate)
                    {
                        updatePanel.SetActive(true);
                    }
                }
            }
            catch
            {

            }
        }
    }


    [System.Serializable]
    public class MaintenanceData
    {
        public bool success;
        public Data data;
        public string msg;
    }

    [System.Serializable]
    public class Data
    {
        public int id;
        public string name;
        public bool isLive;
        public bool isMaintenance;
        public bool isUpdate;
        public string platfrom;
    }
}
