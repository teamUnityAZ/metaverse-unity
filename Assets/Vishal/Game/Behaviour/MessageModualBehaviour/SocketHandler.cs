using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;

public class SocketHandler : MonoBehaviour
{
    public string address = ConstantsGod.API_BASEURL;
    public static SocketHandler Instance;
    public SocketManager Manager;

    public bool isSNSFeedSocketEvent = false;

    private SocketResponce msgResponce = new SocketResponce();
    private GroupLeaveResponceRoot leaveGroupResponce = new GroupLeaveResponceRoot();

    private void Awake()
    {
        /*if (Instance == null)
        {
            Instance = this;
        }*/
    }

    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        address = ConstantsGod.API_BASEURL;
        Debug.LogError("Address:" + address);
        if (!address.EndsWith("/"))
        {
            address = address + "/";
        }
        Debug.LogError("Address:" + address);
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
        Debug.LogError("SocketHandler OnConnected:"+isSNSFeedSocketEvent);
        Manager.Socket.Emit("hi", "hiiii");
        if (!isSNSFeedSocketEvent)
        {
            OnGetMessageAfterReconnectSocket();
        }
    }

    void OnError(CustomError args)
    {
        Debug.LogError("SocketHandler OnError");
        Debug.LogError(string.Format("Error: {0}", args.ToString()));
    }

    void Onresult(CustomError args)
    {
        Debug.LogError("SocketHandler Onresult");
        Debug.LogError(string.Format("Error: {0}", args.ToString()));
    }

    public void ResetListener()
    {
        Debug.Log("Listen");
        if (isSNSFeedSocketEvent)
        {
            Manager.Socket.On<string>("FeedComment", FeedCommentResponse);

            Manager.Socket.On<string>("FeedLike", FeedLikeResponse);
        }
        else
        {
            Manager.Socket.On<string>("MessageReceived", MessageReceivedResponse);

            Manager.Socket.On<string>("GroupCreated", GroupCreatedResponse);

            Manager.Socket.On<string>("GroupLeaved", GroupLeaveUserResponse);

            Manager.Socket.On<string>("GroupDeleted", GroupDeleteResponse);
        }
    }

    #region SNS Message Module Socket Events.......
    public void GroupDeleteResponse(string s)
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        Debug.LogError("Group delete response:" + s);
        DeleteGroupRoot deleteGroupResponce = JsonConvert.DeserializeObject<DeleteGroupRoot>(s);

        if (MessageController.Instance.allChatGetConversationDatum != null)//user in chat or details screen show popup and ok press goto conversation screen and clear data 
        {
            Debug.LogError("GroupDeleted:" + MessageController.Instance.allChatGetConversationDatum.group.createdBy);
            if(MessageController.Instance.allChatGetConversationDatum.receivedGroupId == deleteGroupResponce.groupId && MessageController.Instance.allChatGetConversationDatum.group.createdBy != APIManager.Instance.userId)
            {
                MessageController.Instance.groupDeletedShowPopupForOtherUser.SetActive(true);
            }
        }
        else
        {
            //check group id on conversation list is available then clear require data and destroy object.......
            MessageController.Instance.ResetAndRefreshMessageModule();
        }
    }

    //this method is used to Group leave user response.......
    public void GroupLeaveUserResponse(string s)
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        Debug.LogError("Group Leave Responce Data:" + s);
        leaveGroupResponce = JsonConvert.DeserializeObject<GroupLeaveResponceRoot>(s);
        MessageController.Instance.LeaveGroupAfterRemoveMemberFromCurrentConversation(leaveGroupResponce);
    }

    public void GroupCreatedResponse(string s)
    {
        //if (!this.gameObject.activeInHierarchy)
            //return;

        Debug.LogError("socket Group Created Response Data:" + s);
        msgResponce = JsonConvert.DeserializeObject<SocketResponce>(s);
        for (int i = 0; i < msgResponce.userList.Count; i++)
        {
            if (int.Parse(msgResponce.userList[i]) == APIManager.Instance.userId)
            {
                APIManager.Instance.RequestChatGetConversation();
            }
        }
    }

    //this method is used to Reconnect socket event and chat screen open then call get message api.......
    bool isReconnectGetmessage = false;
    void OnGetMessageAfterReconnectSocket()
    {
        Debug.LogError("OnGetMessageAfterReconnectSocket0000000.......:"+ isReconnectGetmessage);
        if (!isReconnectGetmessage && MessageController.Instance != null && MessageController.Instance.allChatGetConversationDatum != null)
        {
            if (!MessageController.Instance.ChatScreen.activeSelf)
            {
                Debug.LogError("Refresh  Conversation call.......");
                return;
            }
            Debug.LogError("OnGetMessageAfterReconnectSocket1111111.......");
            // Debug.LogError("2");
            if (MessageController.Instance.allChatGetConversationDatum.receivedGroupId != 0)
            {
                //Debug.LogError("3");
                RequestChatGetMessagesSocket(1, 50, 0, MessageController.Instance.allChatGetConversationDatum.receivedGroupId);
            }
            else if (MessageController.Instance.allChatGetConversationDatum.receiverId != 0)
            {
                //Debug.LogError("4");
                if (MessageController.Instance.allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
                {
                    // Debug.LogError("5");
                    RequestChatGetMessagesSocket(1, 50, MessageController.Instance.allChatGetConversationDatum.senderId, 0);
                }
                else
                {
                    //   Debug.LogError("6");
                    RequestChatGetMessagesSocket(1, 50, MessageController.Instance.allChatGetConversationDatum.receiverId, 0);
                }
            }
        }
    }

    //this method is used to message received response.......
    public void MessageReceivedResponse(string s)
    {
        Debug.LogError("socket MessageReceivedResponce Data:" + s + ":Name:"+this.transform.parent.parent.name);
        //if (!this.gameObject.activeInHierarchy)
            //return;

        //  APIManager.Instance.isCreateMessage = true;
        msgResponce = JsonConvert.DeserializeObject<SocketResponce>(s);

        //Debug.LogError("MessageReceivedResponse:" + msgResponce.userList.Count);
        for (int i = 0; i < msgResponce.userList.Count; i++)
        {
            //Debug.LogError("MessageReceivedResponce:"+msgResponce.userList[i]);
            if (int.Parse(msgResponce.userList[i]) == APIManager.Instance.userId)
            {
                if (MessageController.Instance != null && !MessageController.Instance.ChatScreen.activeSelf)
                {
                    Debug.LogError("Refresh  Conversation call.......");
                    APIManager.Instance.RequestChatGetConversation();
                }

                if (!this.gameObject.activeInHierarchy)
                    return;

                if (MessageController.Instance.allChatGetConversationDatum != null)
                {
                    // Debug.LogError("2");
                    if (MessageController.Instance.allChatGetConversationDatum.receivedGroupId != 0)
                    {
                        //Debug.LogError("3");
                        RequestChatGetMessagesSocket(1, 50, 0, MessageController.Instance.allChatGetConversationDatum.receivedGroupId);
                    }
                    else if (MessageController.Instance.allChatGetConversationDatum.receiverId != 0)
                    {
                        //Debug.LogError("4");
                        if (MessageController.Instance.allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
                        {
                            // Debug.LogError("5");
                            RequestChatGetMessagesSocket(1, 50, MessageController.Instance.allChatGetConversationDatum.senderId, 0);
                        }
                        else
                        {
                            //   Debug.LogError("6");
                            RequestChatGetMessagesSocket(1, 50, MessageController.Instance.allChatGetConversationDatum.receiverId, 0);
                        }
                    }
                }
                break;
            }
        }
    }

    public void RequestChatGetMessagesSocket(int message_pageNumber, int message_pageSize, int message_receiverId, int message_receivedGroupId)
    {
        isReconnectGetmessage = true;
        APIManager.Instance.r_isCreateMessage = true;
        StartCoroutine(IERequestChatGetMessagesSocket(message_pageNumber, message_pageSize, message_receiverId, message_receivedGroupId));
    }
    public IEnumerator IERequestChatGetMessagesSocket(int message_pageNumber, int message_pageSize, int message_receiverId, int message_receivedGroupId)
    {
        WWWForm form = new WWWForm();
        form.AddField("pageNumber", message_pageNumber);
        form.AddField("pageSize", message_pageSize);
        if (message_receivedGroupId != 0)
        {
            form.AddField("receivedGroupId", message_receivedGroupId);
        }
        else if (message_receiverId != 0)
        {
            form.AddField("receiverId", message_receiverId);
        }

        using (UnityWebRequest www = UnityWebRequest.Post((address + ConstantsGod.r_url_ChatGetMessages), form))
        {
            www.SetRequestHeader("Authorization", APIManager.Instance.userAuthorizeToken);

            yield return www.SendWebRequest();

            isReconnectGetmessage = false;

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (message_receivedGroupId != 0)
                {
                    APIManager.Instance.r_isCreateMessage = false;
                }
            }
            else
            {
                // Debug.Log("Form upload complete!");
                string data = www.downloadHandler.text;
                Debug.LogError("socket Message Chat: " + data);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                APIManager.Instance.allChatMessagesRoot = JsonConvert.DeserializeObject<ChatGetMessagesRoot>(data, settings);

                if (CommonAPIManager.Instance != null)//For Get All Chat UnRead Message Count.......
                {
                    CommonAPIManager.Instance.RequestGetAllChatUnReadMessagesCount();
                }
                yield return new WaitForSeconds(0.03f);
                APIController.Instance.GetAllChat(message_pageNumber, "SocketHandler");
            }
        }
    }
    #endregion

    #region SNS Feed Module Socket Events.......
    public void FeedCommentResponse(string s)
    {
        if (!this.gameObject.activeInHierarchy)
            return;
        Debug.LogError("Feed Comment response:" + s + ":Name:" + this.transform.parent.parent.name);

        FeedCommentSocketRoot feedCommentSocketRoot = JsonConvert.DeserializeObject<FeedCommentSocketRoot>(s);

        FeedUIController.Instance.FeedCommentAndLikeSocketEventSuccessAfterUpdateRequireFeedResponse(feedCommentSocketRoot.feedId, feedCommentSocketRoot.feedCreatedBy, 0, feedCommentSocketRoot.commentCount, false);
    }

    public void FeedLikeResponse(string s)
    {
        if (!this.gameObject.activeInHierarchy)
            return;
        Debug.LogError("Feed Like response:" + s + ":Name:" + this.transform.parent.parent.name);

        FeedLikeSocketRoot feedLikeSocketRoot = JsonConvert.DeserializeObject<FeedLikeSocketRoot>(s);

        if (feedLikeSocketRoot.createdBy == APIManager.Instance.userId)
            return;

        FeedUIController.Instance.FeedCommentAndLikeSocketEventSuccessAfterUpdateRequireFeedResponse(feedLikeSocketRoot.feedId, feedLikeSocketRoot.feedCreatedBy, feedLikeSocketRoot.likeCount, 0, true);
    }
    #endregion
}


class CustomError : Error
{
    public ErrorData data;

    public override string ToString()
    {
        return $"[CustomError {message}, {data?.code}, {data?.content}]";
    }
}

class ErrorData
{
    public int code;
    public string content;
}

[System.Serializable]
public class SocketResponce
{
    public List<string> userList;
}

[System.Serializable]
public class GroupLeaveResponceRoot
{
    public List<int> userList = new List<int>();
    public int groupId; 
}

[System.Serializable]
public class DeleteGroupRoot
{
    public int groupId;
}

[System.Serializable]
public class FeedLikeSocketRoot
{
    public int createdBy;
    public int feedId;
    public int likeCount;
    public int feedCreatedBy;
}

[System.Serializable]
public class FeedCommentSocketRoot
{
    public int createdBy;
    public int feedId;
    public int commentCount;
    public int feedCreatedBy;
}