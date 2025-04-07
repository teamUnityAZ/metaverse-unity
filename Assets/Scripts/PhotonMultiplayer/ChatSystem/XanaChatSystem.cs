﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using Photon.Chat.Demo;
using UnityEngine.AI;
using WebSocketSharp;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using TMPro;


#endif


/// <summary>
/// This simple Chat UI demonstrate basics usages of the Chat Api
/// </summary>
/// <remarks>
/// The ChatClient basically lets you create any number of channels.
///
/// some friends are already set in the Chat demo "DemoChat-Scene", 'Joe', 'Jane' and 'Bob', simply log with them so that you can see the status changes in the Interface
///
/// Workflow:
/// Create ChatClient, Connect to a server with your AppID, Authenticate the user (apply a unique name,)
/// and subscribe to some channels.
/// Subscribe a channel before you publish to that channel!
///
///
/// Note:
/// Don't forget to call ChatClient.Service() on Update to keep the Chatclient operational.
/// </remarks>
public class XanaChatSystem : MonoBehaviour, IChatClientListener
{
    public static XanaChatSystem instance;
    public GameObject chatOutPutPenal;
    public bool helpChecked=false;
    public GameObject HelpScreenObject;
    private const string UsernamePrefs = "UsernamePref";

    public string ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.

    public string[] FriendsList;

    public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context

    [SerializeField]
    public string UserName { get; set; }

    private string selectedChannelName; // mainly used for GUI/input

    public ChatClient chatClient;

    [Header("UI Elements")]
    public GameObject chatButton;
    public GameObject chatNotificationIcon;

    public GameObject chatDialogBox;

#if !PHOTON_UNITY_NETWORKING
        [SerializeField]
#endif
    protected internal ChatAppSettings chatAppSettings;


    public GameObject missingAppIdErrorPanel;
    public GameObject ConnectingLabel;

    public RectTransform ChatPanel;     // set in inspector (to enable/disable panel)
    public GameObject UserIdFormPanel;
    public InputField InputFieldChat;   // set in inspector
    public TextMeshProUGUI CurrentChannelText;     // set in inspector
    public Toggle ChannelToggleToInstantiate; // set in inspector


    public GameObject FriendListUiItemtoInstantiate;

    private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();

    private readonly Dictionary<string, FriendItem> friendListItemLUT = new Dictionary<string, FriendItem>();

    public bool ShowState = true;
    public GameObject Title;
    public Text StateText; // set in inspector
    public Text UserIdText; // set in inspector
    public bool sayHiOnJoiningChannel;
    public ScrollRect ChatScrollRect;
    private bool isQuitGame;

    public RectTransform outline;
    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        //DontDestroyOnLoad(this.gameObject);

        CheckIfDeviceHasNotch();

        CheckPlayerPrefItems();

        this.UserIdText.text = "";
        this.StateText.text = "";
        this.StateText.gameObject.SetActive(true);
        this.UserIdText.gameObject.SetActive(true);
        //this.Title.SetActive(true);
        this.ChatPanel.gameObject.SetActive(false);
        this.ConnectingLabel.SetActive(false);
        //IKMuseum.instance.nametext.text = UserName;



        if (string.IsNullOrEmpty(PlayerPrefs.GetString(ConstantsGod.GUSTEUSERNAME)))
        {

            if (string.IsNullOrEmpty(this.UserName))
            {
                this.UserName = "user" + Environment.TickCount % 99; //made-up username
                Debug.Log("username chat===" + this.UserName);
            }
        }
        else
        {
            this.UserName = PlayerPrefs.GetString(ConstantsGod.GUSTEUSERNAME);
            Debug.Log("username chat1===" + this.UserName);
        }

    

#if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
#endif

        bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);

        this.missingAppIdErrorPanel.SetActive(!appIdPresent);
        this.UserIdFormPanel.gameObject.SetActive(appIdPresent);

        if (!appIdPresent)
        {
            Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
        }

        Connect();
    }

    public void helpScreenOnOff()
    {
        if (chatOutPutPenal.activeInHierarchy)
        {
            if (HelpScreenObject.activeInHierarchy)
            {
                helpChecked = true;
                chatOutPutPenal.SetActive(false);
            }
        }
        else
        {
            if (helpChecked && !HelpScreenObject.activeInHierarchy)
            {
                helpChecked = false;
                chatOutPutPenal.SetActive(true);
                HelpScreenObject.SetActive(false);
            }
        }
    }
   
    void CheckIfDeviceHasNotch()
    {
        outline.offsetMin = new Vector2((Screen.safeArea.xMin / (float)(Screen.width / 800f)), outline.offsetMin.y);
    }

    void CheckPlayerPrefItems()
    {
        if (PlayerPrefs.HasKey(UsernamePrefs))
        {
            UserName = PlayerPrefs.GetString(UsernamePrefs);
            if (UserName.Contains("Guest") || UserName.Contains("ゲスト"))
            {
                if (GameManager.currentLanguage == "ja")
                {
                    UserName = "ゲスト" + UserName.Substring(UserName.Length - 4);
                }
                else if (GameManager.currentLanguage == "en")
                {
                    UserName = "Guest" + UserName.Substring(UserName.Length - 4);
                }
            }
            //else
            //{
            //    if (GameManager.currentLanguage == "en")
            //    {
            //        UserName = "Guest" + UserName.Substring(UserName.Length - 4);
            //    }
            //}
        }
        //else
        //{
        //    if (GameManager.currentLanguage == "en")
        //        UserName = "Guest" + UnityEngine.Random.Range(1111, 9999);
        //    else if (GameManager.currentLanguage == "ja")
        //        UserName = "ゲスト" + UnityEngine.Random.Range(1111, 9999);
        //}
        //if (PlayerPrefs.HasKey(UsernamePrefs))
        //{
        //    UserName = PlayerPrefs.GetString(UsernamePrefs);

        //}
        //else
        //{
        //    if (GameManager.currentLanguage == "en")
        //        UserName = "Guest" + UnityEngine.Random.Range(1111, 9999);
        //    else
        //        UserName = "ゲスト" + UnityEngine.Random.Range(1111, 9999);

        //}

        Debug.Log("IS LOGGED IN: " + PlayerPrefs.GetInt("IsLoggedIn"));

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                if (PlayerPrefs.GetInt("IsLoggedIn") == 0)
                {
                    if (GameManager.currentLanguage == "en")
                    {
                        //if (string.IsNullOrEmpty(PlayerPrefs.GetString(ConstantsGod.GUSTEUSERNAME)))
                        //{
                        //    this.UserName = "user" + Environment.TickCount % 99; //made-up username
                        //}
                        //else
                        //{
                        //    this.UserName = PlayerPrefs.GetString(ConstantsGod.GUSTEUSERNAME);
                        //}


                    }

                    ////UserName = "Guest" + UnityEngine.Random.Range(1111, 9999);
                    //UserName = "user" + Environment.TickCount % 99;
                    else if(GameManager.currentLanguage == "ja")
                    {
                        //if (string.IsNullOrEmpty(PlayerPrefs.GetString(ConstantsGod.GUSTEUSERNAME)))
                        //{
                        //    UserName = "ユーザー" + Environment.TickCount % 99;
                        //}
                        //else
                        //{
                        //    UserName = PlayerPrefs.GetString(ConstantsGod.GUSTEUSERNAME);
                        //}

                    }
                    //UserName = "ゲスト" + UnityEngine.Random.Range(1111, 9999);
                  

                }
                else
                {
                    string playerName = PlayerPrefs.GetString("PlayerName");

                    if (playerName == "" || playerName == null)
                    {
                        playerName = "NewPlayer";
                    }

                    UserName = playerName;
                    

                }

            }
        }
        
    }
    

    public void Connect()
    {
        this.UserIdFormPanel.gameObject.SetActive(false);

        this.chatClient = new ChatClient(this);
#if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
#endif
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
            this.UserName = PlayerPrefs.GetString("PlayerName");

        this.chatClient.AuthValues = new AuthenticationValues(this.UserName);

        this.chatClient.ConnectUsingSettings(this.chatAppSettings);

        this.ChannelToggleToInstantiate.gameObject.SetActive(false);
        Debug.Log("Connecting as: " + this.UserName);
        PlayerPrefs.SetString(UsernamePrefs, this.UserName);
        PlayerPrefs.SetString(ConstantsGod.PLAYERNAME, this.UserName);
        Debug.Log(this.chatClient.AuthValues.AuthGetParameters);

        this.ConnectingLabel.SetActive(true);

    }

    /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnDestroy.</summary>
    public void OnDestroy()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnApplicationQuit.</summary>
    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
        }

        // check if we are missing context, which means we got kicked out to get back to the Photon Demo hub.
        if (this.StateText == null)
        {
            Destroy(this.gameObject);
            return;
        }

        helpScreenOnOff();

        //this.StateText.gameObject.SetActive(this.ShowState); // this could be handled more elegantly, but for the demo it's ok.
    }


    public void OnEnterSend()
    {
        if (!PremiumUsersDetails.Instance.CheckSpecificItem("Message Option/Chat option"))
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }
          
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendChatMessage(this.InputFieldChat.text);
            PlayerPrefs.SetString(ConstantsGod.SENDMESSAGETEXT, this.InputFieldChat.text);
            Debug.Log("text msg====" + PlayerPrefs.GetString(ConstantsGod.SENDMESSAGETEXT));
            this.InputFieldChat.text = "";
        }

        if (InputFieldChat.touchScreenKeyboard != null)
        {
            if (InputFieldChat.touchScreenKeyboard.status == TouchScreenKeyboard.Status.Done)
            {
                this.SendChatMessage(this.InputFieldChat.text);
                PlayerPrefs.SetString(ConstantsGod.SENDMESSAGETEXT, this.InputFieldChat.text);
                Debug.Log("text msg====" + PlayerPrefs.GetString(ConstantsGod.SENDMESSAGETEXT));
                this.InputFieldChat.text = "";
            }
        }

        ArrowManager.OnInvokeCommentButtonClickEvent(PlayerPrefs.GetString(ConstantsGod.SENDMESSAGETEXT));
    }

    public void OnClickSend()
    {
        if (this.InputFieldChat != null)
        {
            this.SendChatMessage(this.InputFieldChat.text);
           
            this.InputFieldChat.text = "";
        }
    }


    public int TestLength = 2048;
    private byte[] testBytes = new byte[2048];

    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }
        if ("test".Equals(inputLine))
        {
            if (this.TestLength != this.testBytes.Length)
            {
                this.testBytes = new byte[this.TestLength];
            }

            this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, this.testBytes, true);
        }

        if (inputLine.Trim() != "")
            this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnConnected()
    {
        CheckPlayerPrefItems();

        if (this.ChannelsToJoinOnConnect != null)
        {
            if (SceneManager.GetActiveScene().name == "AddressableScene")
            {
                this.ChannelsToJoinOnConnect = FeedEventPrefab.m_EnvName;// FeedEventPrefab
            }
            else
            {
                this.ChannelsToJoinOnConnect = SceneManager.GetActiveScene().name;// FeedEventPrefab
            }
            this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
            //Debug.LogError(this.ChannelsToJoinOnConnect);
        }

        this.ConnectingLabel.SetActive(false);

        //this.UserIdText.text = "Connected as " + this.UserName;

        this.ChatPanel.gameObject.SetActive(true);

        #region Friend List - Might be useful in a later update
        //if (this.FriendsList != null && this.FriendsList.Length > 0)
        //{
        //    this.chatClient.AddFriends(this.FriendsList); // Add some users to the server-list to get their status updates

        //    // add to the UI as well
        //    foreach (string _friend in this.FriendsList)
        //    {
        //        if (this.FriendListUiItemtoInstantiate != null && _friend != this.UserName)
        //        {
        //            this.InstantiateFriendButton(_friend);
        //        }

        //    }

        //}

        //if (this.FriendListUiItemtoInstantiate != null)
        //{
        //    this.FriendListUiItemtoInstantiate.SetActive(false);
        //}
        #endregion

        this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
    }

    public void OnDisconnected()
    {
        //this.ConnectingLabel.SetActive(false);
        if (!isQuitGame)
            Connect();
    }

    public void OnChatStateChange(ChatState state)
    {
        // use OnConnected() and OnDisconnected()
        // this method might become more useful in the future, when more complex states are being used.

        this.StateText.text = state.ToString();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        // in this demo, we simply send a message into each channel. This is NOT a must have!
        foreach (string channel in channels)
        {
            if (sayHiOnJoiningChannel)
            {
                Debug.Log("Currently Joined Channel: " + channel);

                 if (Application.systemLanguage == SystemLanguage.English)
                    this.chatClient.PublishMessage(channel, " has joined."); // you don't HAVE to send a msg on join but you could.
                 else
                     this.chatClient.PublishMessage(channel, " 参加しました");
            }

            if (this.ChannelToggleToInstantiate != null)
            {
                this.InstantiateChannelButton(channel);

            }
        }

        Debug.Log("OnSubscribed: " + string.Join(", ", channels));

        /*
        // select first subscribed channel in alphabetical order
        if (this.chatClient.PublicChannels.Count > 0)
        {
            var l = new List<string>(this.chatClient.PublicChannels.Keys);
            l.Sort();
            string selected = l[0];
            if (this.channelToggles.ContainsKey(selected))
            {
                ShowChannel(selected);
                foreach (var c in this.channelToggles)
                {
                    c.Value.isOn = false;
                }
                this.channelToggles[selected].isOn = true;
                AddMessageToSelectedChannel(WelcomeText);
            }
        }
        */

        // Switch to the first newly created channel
        this.ShowChannel(channels[0]);
    }

    /// <inheritdoc />
    public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
    {
        Debug.LogFormat("OnSubscribed: {0}, users.Count: {1} Channel-props: {2}.", channel, users.Length, properties.ToStringFull());
    }

    private void InstantiateChannelButton(string channelName)
    {
        if (this.channelToggles.ContainsKey(channelName))
        {
            Debug.Log("Skipping creation for an existing channel toggle.");
            return;
        }

        Toggle cbtn = (Toggle)Instantiate(this.ChannelToggleToInstantiate);
        cbtn.gameObject.SetActive(true);
        cbtn.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
        cbtn.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);

        this.channelToggles.Add(channelName, cbtn);
    }

    // Might be useful in a later update. If we want to add friends to chat with.
    private void InstantiateFriendButton(string friendId)
    {
        GameObject fbtn = (GameObject)Instantiate(this.FriendListUiItemtoInstantiate);
        fbtn.gameObject.SetActive(true);
        FriendItem _friendItem = fbtn.GetComponent<FriendItem>();

        _friendItem.FriendId = friendId;

        fbtn.transform.SetParent(this.FriendListUiItemtoInstantiate.transform.parent, false);

        this.friendListItemLUT[friendId] = _friendItem;
    }


    public void OnUnsubscribed(string[] channels)
    {
        foreach (string channelName in channels)
        {
            if (this.channelToggles.ContainsKey(channelName))
            {
                Toggle t = this.channelToggles[channelName];
                Destroy(t.gameObject);

                this.channelToggles.Remove(channelName);

                Debug.Log("Unsubscribed from channel '" + channelName + "'.");

                // Showing another channel if the active channel is the one we unsubscribed from before
                if (channelName == this.selectedChannelName && this.channelToggles.Count > 0)
                {
                    IEnumerator<KeyValuePair<string, Toggle>> firstEntry = this.channelToggles.GetEnumerator();
                    firstEntry.MoveNext();

                    this.ShowChannel(firstEntry.Current.Key);

                    firstEntry.Current.Value.isOn = true;
                }
            }
            else
            {
                Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
            }
        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(this.selectedChannelName))
        {
            // update text
            this.ShowChannel(this.selectedChannelName);

            if (GameManager.currentLanguage == "en")
            {
                this.CurrentChannelText.text=
                    senders[senders.Length - 1] + " : " + (string)messages[messages.Length - 1] + "\n" + this.CurrentChannelText.text;
            }
            else
            if (GameManager.currentLanguage == "ja")
            {
                //this.CurrentChannelText.text +=
                //    senders[senders.Length - 1] + " : " + (string)messages[messages.Length - 1] + "\n";
                // commented by Usman Aslam
                string lastMessage = (string)messages[messages.Length - 1];
                if (lastMessage.Contains("has joined"))
                {
                    print(senders[senders.Length - 1]);
                    SetJapaneseText(senders[senders.Length - 1], true, lastMessage);
                }
                else
                {
                    SetJapaneseText(senders[senders.Length - 1], false, lastMessage);
                }

            }

            if (!chatDialogBox.activeSelf && senders[senders.Length - 1] != UserName)
            {
                chatNotificationIcon.SetActive(true);
            }
        }


        StartCoroutine(Delay());
    }


    System.Collections.IEnumerator Delay()
    {
        yield return new WaitForSeconds(.3f);
        ChatScrollRect.verticalNormalizedPosition = 1f;
        //Debug.LogError("================"+ ChatScrollRect.verticalNormalizedPosition);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
        // you also get messages that you sent yourself. in that case, the channelName is determinded by the target of your msg
        this.InstantiateChannelButton(channelName);

        byte[] msgBytes = message as byte[];
        if (msgBytes != null)
        {
            Debug.Log("Message with byte[].Length: " + msgBytes.Length);
        }
        if (this.selectedChannelName.Equals(channelName))
        {
            this.ShowChannel(channelName);
        }
    }

    /// <summary>
    /// New status of another user (you get updates for users set in your friends list).
    /// </summary>
    /// <param name="user">Name of the user.</param>
    /// <param name="status">New status of that user.</param>
    /// <param name="gotMessage">True if the status contains a message you should cache locally. False: This status update does not include a
    /// message (keep any you have).</param>
    /// <param name="message">Message that user set.</param>
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

        Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));

        if (this.friendListItemLUT.ContainsKey(user))
        {
            FriendItem _friendItem = this.friendListItemLUT[user];
            if (_friendItem != null) _friendItem.OnFriendStatusUpdate(status, gotMessage, message);
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    /// <inheritdoc />
    public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
    {
        //Debug.LogFormat("OnChannelPropertiesChanged: {0} by {1}. Props: {2}.", channel, userId, Extensions.ToStringFull(properties));
    }

    public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
    {
        //Debug.LogFormat("OnUserPropertiesChanged: (channel:{0} user:{1}) by {2}. Props: {3}.", channel, targetUserId, senderUserId, Extensions.ToStringFull(properties));
    }

    /// <inheritdoc />
    public void OnErrorInfo(string channel, string error, object data)
    {
        Debug.LogFormat("OnErrorInfo for channel {0}. Error: {1} Data: {2}", channel, error, data);
    }

    public void AddMessageToSelectedChannel(string msg)
    {
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(this.selectedChannelName, out channel);
        if (!found)
        {
            Debug.Log("AddMessageToSelectedChannel failed to find channel: " + this.selectedChannelName);
            return;
        }

        if (channel != null)
        {
            channel.Add("Bot", msg, 0); //TODO: how to use msgID?
        }
    }

     
    private void SetJapaneseText(string UserID, bool IsInfoMessage, string message)
    {
        string temp = null;
        if (IsInfoMessage)
        {
            if (!UserID.IsNullOrEmpty())
            {
                if (PlayerPrefs.GetInt("IsLoggedIn") == 0)
                {
                    temp = "ゲスト:参加しました";
                    string[] GuestIDs = Regex.Split(UserID, @"\D+");

                    if (!GuestIDs[1].IsNullOrEmpty())
                    {
                        string final = temp.Insert(3, GuestIDs[1]);
                        CurrentChannelText.text = final + '\n' + CurrentChannelText.text;
                    }
                }

                else
                {
                    temp = UserID + ":参加しました";
                    CurrentChannelText.text = temp + '\n' + CurrentChannelText.text;
                }
            }
            else
            {

            }
        }
        else
        {
            
            temp = "ゲスト";
            string[] GuestIDs = Regex.Split(UserID, @"\D+");
            Debug.LogError(GuestIDs.Length);
            if (!GuestIDs[0].IsNullOrEmpty()) 
            {
                temp += GuestIDs[0] + ": ";
                string final = temp + message;
                print(final);
                CurrentChannelText.text = final + '\n' + CurrentChannelText.text;
            }
            else
            {
                temp = UserID;
                temp +=  ": ";
                string final = temp + message;
                CurrentChannelText.text = final + '\n'+ CurrentChannelText.text;
            }
        }

    }

    public void ShowChannel(string channelName)
    {


        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        this.selectedChannelName = channelName;


        Debug.Log("ShowChannel: " + this.selectedChannelName);


        foreach (KeyValuePair<string, Toggle> pair in this.channelToggles)
        {
            pair.Value.isOn = pair.Key == channelName ? true : false;
        }
    }


    private bool isChatOpen;
   

    public void OpenCloseChatDialog()
    {
        isChatOpen = !isChatOpen;

        if (isChatOpen)
        {
            chatDialogBox.SetActive(true);
            chatNotificationIcon.SetActive(false);
            chatButton.GetComponent<Image>().enabled = true;
        }
        else
        {
            chatDialogBox.SetActive(false);
            chatNotificationIcon.SetActive(false);
            chatButton.GetComponent<Image>().enabled = false;
        }
    }

    public void OpenDashboard()
    {
        Application.OpenURL("https://dashboard.photonengine.com");
    }
}
