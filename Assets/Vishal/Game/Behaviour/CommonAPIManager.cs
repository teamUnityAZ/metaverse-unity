﻿using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Events;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommonAPIManager : MonoBehaviour
{
    public static CommonAPIManager Instance;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("APIManager Start UserToken:" + PlayerPrefs.GetString("LoginToken") + "    :userID:" + PlayerPrefs.GetString("UserName"));

        ConnetSocketManagerAndListener();
    }


    #region Common Socket Handler Event.......
    [Header("Common Socket Handler")]
    public SocketManager Manager;
    public string address;

    public void ConnetSocketManagerAndListener()
    {
        address = ConstantsGod.API_BASEURL;
        Debug.LogError("ConnetSocketManagerAndListener Address:" + address);
        if (!address.EndsWith("/"))
        {
            address = address + "/";
        }
        Debug.LogError("ConnetSocketManagerAndListener Address:" + address);
        Manager = new SocketManager(new Uri((address)));
        Manager.Socket.On<ConnectResponse>(SocketIOEventTypes.Connect, OnConnected);
        Manager.Socket.On<CustomError>(SocketIOEventTypes.Error, OnError);

        ResetListener();
    }

    public void Connect()
    {
        Debug.Log("hi from server");
    }

    void OnConnected(ConnectResponse resp)
    {
        //Debug.LogError("Connect");
        //Manager.Socket.Emit("hi", "hiiii");
    }

    void OnError(CustomError args)
    {
        //Debug.LogError(string.Format("Error: {0}", args.ToString()));
    }

    void Onresult(CustomError args)
    {
        //Debug.LogError(string.Format("Error: {0}", args.ToString()));
    }

    public void ResetListener()
    {
        Debug.Log("Listen");
        //Manager.Socket.On<string>("FeedComment", FeedCommentResponse);
        Manager.Socket.On<string>("MessageReceived", MessageReceivedResponse);
    }

    public void MessageReceivedResponse(string s)
    {
        Debug.LogError("Common Socket Handler MessageReceivedResponce.......");
        if (MessageController.Instance != null)
        {
            if (MessageController.Instance.ChatScreen.activeInHierarchy)
            {
                return;
            }

            if (!MessageController.Instance.gameObject.activeInHierarchy)//this condition is used to once message screen open then go to another screen then back to message screen refresh conversation list.......
            {
                MessageController.Instance.isNeedToRefreshConversationAPI = true;
            }
        }
        Debug.LogError("Common Socket Handler MessageReceivedResponce111111111.......");
        RequestGetAllChatUnReadMessagesCount();//For Get All Chat UnRead Message Count.......
    }

    #endregion

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    #region Common APi Handler.......

    #region SNS Module APis.......
    [Space]
    [Header("Comman APi Handler")]
    BottomTabManager[] bottomTabManagers;
    //this api is used to get all UnRead Messages Count.......
    public void RequestGetAllChatUnReadMessagesCount()
    {
        Debug.LogError("RequestGetAllChatUnReadMessagesCount0000000.......");
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LoginToken")))
        {
            //Debug.LogError("RequestGetAllChatUnReadMessagesCount1111111.......");
            if (IERequestGetAllChatUnReadMessagesCountCo != null)
            {
                StopCoroutine(IERequestGetAllChatUnReadMessagesCountCo);
            }
            IERequestGetAllChatUnReadMessagesCountCo = StartCoroutine(IERequestGetAllChatUnReadMessagesCount());
        }
    }
    Coroutine IERequestGetAllChatUnReadMessagesCountCo;
    public IEnumerator IERequestGetAllChatUnReadMessagesCount()
    {
        using (UnityWebRequest www = UnityWebRequest.Get((ConstantsGod.API_BASEURL + ConstantsGod.r_url_GetAllChatUnReadMessagesCount)))
        {
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Get UnReadMessagesCount Success!");
                string data = www.downloadHandler.text;
                Debug.LogError("data" + data);
                MessageUnreadCountRoot myDeserializedClass = JsonConvert.DeserializeObject<MessageUnreadCountRoot>(data);

                SetUpBottomUnReadCount(myDeserializedClass.data);
            }
        }
    }

    //This method is used to setup footer message unread count setup.......
    public void SetUpBottomUnReadCount(int count)
    {
        bottomTabManagers = Resources.FindObjectsOfTypeAll<BottomTabManager>();
        for (int i = 0; i < bottomTabManagers.Length; i++)
        {
            bottomTabManagers[i].MessageUnReadCountSetUp(count);
        }
    }
    #endregion

    #endregion
}

public class MessageUnreadCountRoot
{
    public bool success;
    public int data;
    public string msg;
}