using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatScreenDataScript : MonoBehaviour
{
    public static ChatScreenDataScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public ChatGetConversationDatum allChatGetConversationDatum;
    private void OnEnable()
    {
        // InvokeRepeating("OnRefereshGetMessageApi", 5f,2f);
    }

    private void OnDisable()
    {
        // CancelInvoke("OnRefereshGetMessageApi");
    }

    public void OnRefereshGetMessageApi()
    {
        APIManager.Instance.r_isCreateMessage = true;
        if (allChatGetConversationDatum.receiverId != 0)
        {
            if (allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
            {
                APIManager.Instance.RequestChatGetMessages(1, 50, allChatGetConversationDatum.senderId, 0, "");
            }
            else
            {
                APIManager.Instance.RequestChatGetMessages(1, 50, allChatGetConversationDatum.receiverId, 0, "");
            }
            //Debug.LogError("receiverId" + allChatGetConversationDatum.receiverId);
        }
        else if (allChatGetConversationDatum.receivedGroupId != 0)
        {
            //Debug.LogError("receivedGroupId" + allChatGetConversationDatum.receivedGroupId);
            APIManager.Instance.RequestChatGetMessages(1, 50, 0, allChatGetConversationDatum.receivedGroupId, "");
        }
        MessageController.Instance.allChatGetConversationDatum = allChatGetConversationDatum;
    }
}