﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using UnityEditor;
using SuperStar.Helpers;
using RenderHeads.Media.AVProVideo;
using AdvancedInputFieldPlugin;
using SoftMasking;

public class MessageController : MonoBehaviour
{
    public static MessageController Instance;

    [Header("-------FooterCan-------")]
    public GameObject footerCan;

    [Header("-------API Controller Message References-------")]
    public Transform followingUserParent;
    public Transform conversationPrefabParent;
    public Transform selectedFriendItemPrefabParent;
    public Transform chatPrefabParent;
    public Transform chatShareAttechmentparent;
    public Transform chatShareAttechmentPhotoPanel;
    public Transform chatShareAttechmentMainPanel;
    public Transform chooseAttechmentparent;
    public GameObject chatShareAttechmentPanel;
    public Transform chatMemberParent;
    public Transform chatTimeParent;
    public Transform saveAttechmentParent;

    [Header("-------MessageScreens-------")]
    public GameObject SelectFriendScreen;
    public GameObject MessageListScreen;
    public GameObject ChooseScreen;
    public GameObject MessageDetailScreen;
    public GameObject ChatScreen;
    public GameObject NoAttechmentScreen;
    public GameObject AttechmentDownloadScreen;
    public List<GameObject> chatVoiceButtonObj = new List<GameObject>();
    public List<GameObject> chatSendButtonObj = new List<GameObject>();

    public List<int> conversationUserList = new List<int>();
    public List<string> CreateNewMessageUserList = new List<string>();
    public List<Sprite> createNewMessageUserAvatarSPList = new List<Sprite>();

    public TextMeshProUGUI startAndWaitMessageText;
    public GameObject startConversationPopup;

    [Header("Top Main screen references")]
    public GetUserDetailData myUserData = new GetUserDetailData();
    public Image TopUserProfileImage;

    [Header("SelectFriendScreen")]
    public int tottleFollowing = 0;
    public TextMeshProUGUI tottleFollowingText;
    public GameObject selectionScrollView;
    public GameObject findFriendNextBtn;

    public bool isSelectFriendDataLoaded = false;
    public int selectFriendPaginationPageNum = 1;

    [Header("SetGroupNameScreen")]
    public GameObject setGroupNameScreen;
    public GameObject setGroupNameMediaOptionScreen;
    public Image groupAvatarImage;
    public Sprite defaultGroupAvatarSP;
    public GameObject groupNameValidationObj;
    public TMP_InputField setGroupNameInputField;
    public TextMeshProUGUI setGroupNameParticipantsText;
    //public GameObject setGroupNameLoader;
    public Transform setGroupMemberContainer;
    public TextMeshProUGUI setGroupNameTitleText;
    public TextMeshProUGUI setGroupNameSubTitleText;
    public TextMeshProUGUI setGroupNameNextOrDoneText;
    public TextMeshProUGUI setGroupNameDescriptionText;
    public GameObject setGroupNameMemberPanelObj;
    public int setGroupNameScreenIndex = 0;

    [Header("ChatScreen")]
    public RectTransform chatScrollMain;
    public RectTransform chatInputFieldMainObj;
    public TMP_InputField typeMessageText;
    public AdvancedInputField chatTypeMessageInputfield;
    public SNSChatView sNSChatView;
    public ChatGetConversationDatum allChatGetConversationDatum;
    public TextMeshProUGUI chatTitleText;
    public Image chatScreenTopUserImage, chatScreenTopUserBGImage;
    public Sprite defaultChatScreenTopUserImage, defaultUserImageRound, defaultUserImageSquare, defaultWhiteSquare, defaultWhiteCircle;
    public bool isDirectMessage = false;
    public int currentChatPage = 1;
    public bool isChatDataLoaded = false;
    public AllConversationData currentConversationData;

    [Header("chat Screen video or image view screen")]
    public GameObject chatScreenViewImageOrVideoScreen;
    public GameObject chatScreenViewVideoDisplayObj;
    public MediaPlayer chatScreenViewVideoMediaPlayer;
    public Image chatScreenViewImageObj;

    [Header("MessageDetails Screen Reference")]
    public List<GameObject> messageDetailsLeaveGroupFalseObjList = new List<GameObject>();
    public Toggle messageDetailsAllowNotificationToggle;
    public GameObject removeGroupmemberConfirmationScreen;
    public ChatMemberData currentSelectedGroupMemberDataScript;

    [Header("Attachment Download Screen References")]
    public TextMeshProUGUI saveAttachmentSenderNameText;
    public TextMeshProUGUI saveAttachmentDateTimeText;

    [Header("Delete Confirmation Screen References")]
    public GameObject deleteConfirmationScreen;
    public Text deleteConfirmationTitleText;
    public TextMeshProUGUI deleteConfirmationDescriptionText;
    public AllConversationData deleteConfirmationCurrentConversationDataScript;

    public GameObject groupDeletedShowPopupForOtherUser;
    //[Header("Set Token Screen")]
    //public GameObject setTokenScreen;

    //[Header("-------Dummy Token Set Data-------")]
    //public TMP_InputField tokenInput;
    //public TMP_InputField userIdInput;

    [Header("Search conversation and friends References")]
    public SearchManager searchManagerAllConversation;
    public SearchManager searchManagerFindFriends;

    [Header("ApiLoader Reference")]
    public SNSAPILoaderController apiLoaderController;
    //public TextMeshProUGUI postUploadProgressText;

    public bool isNeedToRefreshConversationAPI = false;

    public bool isChatDetailsScreenDeactive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        string saveDir = Path.Combine(Application.persistentDataPath, "XanaChat");
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        if (!APIManager.Instance.isTestDefaultToken)
        {
            //set token screen check.......
            /*if (string.IsNullOrEmpty(PlayerPrefs.GetString("LoginToken")) || string.IsNullOrEmpty(PlayerPrefs.GetString("UserName")))//new comment.......
            {
                APIManager.Instance.userAuthorizeToken = "";
                setTokenScreen.SetActive(true);
                return;
            }
            else
            {
                APIManager.Instance.userAuthorizeToken = PlayerPrefs.GetString("LoginToken");
                APIManager.Instance.userId = int.Parse(PlayerPrefs.GetString("UserName"));
                setTokenScreen.SetActive(false);
            }*/
            APIManager.Instance.userAuthorizeToken = PlayerPrefs.GetString("LoginToken");
            APIManager.Instance.userId = int.Parse(PlayerPrefs.GetString("UserName"));
        }

        //rik for start of the Message scene default api calling....... 
        OnClickMessageButton(); //rik

        StartCoroutine(WaitToCallRequestGetAllFollowing());//rik.......

        GetUserDetailsAPI();//get user details.......
    }

    private void OnEnable()
    {
        if (callCount > 0)
        {
            StartCoroutine(WaitToStartCallForMessageScene());
            return;
        }
        callCount += 1;
    }

    public int callCount = 0;
    IEnumerator WaitToStartCallForMessageScene()
    {
        bool isGroupChatRefresh = false;
        bool isTempNeedToRefreshConversation = false;
        if (OtherPlayerProfileData.Instance != null)
        {
            Debug.LogError("MessageDetailScreen :" + MessageDetailScreen.activeSelf + " :isChatDetailsScreenDeactive:" + isChatDetailsScreenDeactive);
            if (MessageDetailScreen.activeSelf && isChatDetailsScreenDeactive)
            {
                isChatDetailsScreenDeactive = false;
                /*if (OtherPlayerProfileData.Instance.backKeyManageList.Contains("GroupDetailsScreen"))
                {
                    OtherPlayerProfileData.Instance.backKeyManageList.Remove("GroupDetailsScreen");
                }*/
                if (!OtherPlayerProfileData.Instance.isProfiletranzistFromMessage)
                {
                    MessageDetailScreen.SetActive(false);
                    MessageListScreen.SetActive(true);
                }
                else
                {
                    isGroupChatRefresh = true;                    
                }
                OtherPlayerProfileData.Instance.isProfiletranzistFromMessage = false;
            }
            else
            {
                isTempNeedToRefreshConversation = true;
            }
        }
        else
        {
            isTempNeedToRefreshConversation = true;
        }
        yield return new WaitForSeconds(0.1f);
        Debug.LogError("MessageController isLoginFromDifferentId:" + APIManager.Instance.isLoginFromDifferentId);
        if (APIManager.Instance.isLoginFromDifferentId)
        {
            APIController.Instance.allFollowingUserList.Clear();
            APIController.Instance.allChatMemberList.Clear();
            APIController.Instance.allConversationList.Clear();
            APIController.Instance.chatTimeList.Clear();

            foreach (Transform item in conversationPrefabParent)
            {
                Destroy(item.gameObject);
            }
            foreach (Transform item in chatPrefabParent)
            {
                Destroy(item.gameObject);
            }
            //rik for start of the Message scene default api calling....... 
            OnClickMessageButton(); //rik

            StartCoroutine(WaitToCallRequestGetAllFollowing());//rik.......

            isTempNeedToRefreshConversation = false;
        }
        else
        {
            if (isGroupChatRefresh)
            {
                if (allChatGetConversationDatum != null)
                {
                    if (allChatGetConversationDatum.receivedGroupId != 0)
                    {
                        Debug.LogError("Group then refresh group message");
                        SocketHandler.Instance.RequestChatGetMessagesSocket(1, 50, 0, allChatGetConversationDatum.receivedGroupId);
                    }
                }
            }
        }

        if (isTempNeedToRefreshConversation && isNeedToRefreshConversationAPI)
        {
            Debug.LogError("Need To Refresh Conversation APi.......");
            APIManager.Instance.RequestChatGetConversation();
        }
        isTempNeedToRefreshConversation = false;
        isNeedToRefreshConversationAPI = false;

        isGroupChatRefresh = false;

        GetUserDetailsAPI();//get user details.......
    }


    //set dummy token data.......
    public void OnClickSetTokenButton()
    {
        /*if (!string.IsNullOrEmpty(tokenInput.text))
        {
            APIManager.Instance.userAuthorizeToken = tokenInput.text;
            APIManager.Instance.userId = int.Parse(userIdInput.text);

            setTokenScreen.SetActive(false);

            OnClickMessageButton();

            StartCoroutine(WaitToCallRequestGetAllFollowing());//rik.......

            GetUserDetailsAPI();
        }*/
    }

    #region Reset and Refresh message module.......
    public void ResetAndRefreshMessageModule()
    {
        if (allChatGetConversationDatum != null)
        {
            if (APIManager.Instance.allChatGetConversationRoot.data.Contains(allChatGetConversationDatum))
            {
                int index = APIManager.Instance.allChatGetConversationRoot.data.IndexOf(allChatGetConversationDatum);
                APIManager.Instance.allChatGetConversationRoot.data.Remove(allChatGetConversationDatum);
                if (index >= 0)
                {
                    APIController.Instance.allConversationList.RemoveAt(index);
                    DestroyImmediate(searchManagerAllConversation.searchContainer.transform.GetChild(index).gameObject);
                    DestroyImmediate(conversationPrefabParent.GetChild(index).gameObject);
                }
            }
        }

        allChatGetConversationDatum = null;
        currentConversationData = null;

        if (ChatScreen.activeSelf)
        {
            ChatScreen.SetActive(false);
        }

        if (MessageDetailScreen.activeSelf)
        {
            MessageDetailScreen.SetActive(false);
        }

        if (ChooseScreen.activeSelf)
        {
            ChooseScreen.SetActive(false);
        }

        if (AttechmentDownloadScreen.activeSelf)
        {
            AttechmentDownloadScreen.SetActive(false);
        }

        if (groupDeletedShowPopupForOtherUser.activeSelf)
        {
            groupDeletedShowPopupForOtherUser.SetActive(false);
        }

        OnClickMessageButton();//active message list screen and refreshing list api
    }
    #endregion

    #region Get User Details api and set user avatar
    public void GetUserDetailsAPI()
    {
        APIManager.Instance.RequestGetUserDetails("messageScreen");//Get My Profile data 
    }

    public void GetSuccessUserDetails(GetUserDetailData userdata)
    {
        myUserData = userdata;
        if (!string.IsNullOrEmpty(myUserData.avatar))//set avatar image.......
        {
            bool isAvatarUrlFromDropbox = CheckUrlDropboxOrNot(myUserData.avatar);
            //Debug.LogError("isAvatarUrlFromDropbox: " + isAvatarUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);

            if (isAvatarUrlFromDropbox)
            {
                AssetCache.Instance.EnqueueOneResAndWait(myUserData.avatar, myUserData.avatar, (success) =>
                {
                    if (success)
                        AssetCache.Instance.LoadSpriteIntoImage(TopUserProfileImage, myUserData.avatar, changeAspectRatio: true);
                    // FeedUIController.Instance.ShowLoader(false);
                });
            }
            else
            {
                GetImageFromAWS(myUserData.avatar, TopUserProfileImage);//Get image from aws and save/load into asset cache.......
                                                                        // FeedUIController.Instance.ShowLoader(false);
            }
        }
    }
    #endregion

    public void LoaderShow(bool isActive)
    {
        apiLoaderController.ShowApiLoader(isActive);
    }

    //rik this method are used to Go back to SNS Feed Module Scene....... 
    public void OnGoBackToFeedBtnClick()
    {
        Initiate.Fade("SNSFeedModuleScene", Color.black, 1.0f, true);
    }

    public int addFrindCallingScreenIndex;
    public void OnClickAddFriendOnMessageScreen(int callingFromScreenIndex)
    {
        FindFriendUiRefreshAfterCloseScreen();

        addFrindCallingScreenIndex = callingFromScreenIndex;//check Add friend screen open from home or group add member.

        if (addFrindCallingScreenIndex == 0)
        {
            MessageListScreen.SetActive(false);
        }

        tottleFollowingText.text = TextLocalization.GetLocaliseTextByKey("Add participants");//default text
        SelectFriendScreen.SetActive(true);
        CreateNewMessageUserList.Clear();
        createNewMessageUserAvatarSPList.Clear();
        APIController.Instance.allChatMemberList.Clear();

        LoaderShow(true);//active api loader.

        if (GameManager.currentLanguage == "ja")
        {
            tottleFollowingText.gameObject.SetActive(false);
        }
        else
        {
            tottleFollowingText.gameObject.SetActive(true);
            //tottleFollowingText.GetComponent<TextLocalization>().LocalizeTextText();
        }

        GetAllFollowingForSelectFriends();//request Get All Following api call.......
        //MessageController.Instance.SelectFriendFollowinPaginationResetData();//Reset select friends following api pagination.......
        //APIManager.Instance.RequestGetAllFollowing(1, 100, "message");
    }

    //this method are used to Add Friends Close Button Click...
    public void OnCloseSelectFriendBtnClick()
    {
        if (addFrindCallingScreenIndex == 0)
        {
            MessageListScreen.SetActive(true);
        }
        addFrindCallingScreenIndex = 0;//set default to 0

        FindFriendUiRefreshAfterCloseScreen();
        CreateNewMessageUserList.Clear();
        createNewMessageUserAvatarSPList.Clear();
    }

    public void FindFriendUiRefreshAfterCloseScreen()
    {
        foreach (Transform item in searchManagerFindFriends.searchContainer)
        {
            Destroy(item.gameObject);
        }
        searchManagerFindFriends.allMessageUserDataList.Clear();
        searchManagerFindFriends.searchHistoryAllMessageUserData.Clear();

        APIController.Instance.allFollowingUserList.Clear();
        foreach (Transform item in followingUserParent)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in selectedFriendItemPrefabParent)
        {
            Destroy(item.gameObject);
        }
        APIController.Instance.allChatMemberList.Clear();
        ActiveSelectionScroll();
    }

    public void OnClickMessageButton()
    {
        StartAndWaitMessageTextActive(true, TextLocalization.GetLocaliseTextByKey("please wait"));//start and wait message text show.......
        MessageListScreen.SetActive(true);
        APIManager.Instance.RequestChatGetConversation();
    }

    public void StartAndWaitMessageTextActive(bool isActive, string message)
    {
        startAndWaitMessageText.text = message;
        startAndWaitMessageText.gameObject.SetActive(isActive);
    }

    public void ChatScreenTopUserProfileSetUp(Sprite proifle, bool isGroup)
    {
        if (isGroup)
        {
            chatScreenTopUserBGImage.sprite = defaultWhiteSquare;
        }
        else
        {
            chatScreenTopUserBGImage.sprite = defaultWhiteCircle;
        }
        chatScreenTopUserImage.sprite = proifle;
    }

    public void ChatScreenTopUserProfileBGSetUp(bool isGroup)
    {
        chatScreenTopUserBGImage.GetComponent<SoftMask>().enabled = false;
        if (isGroup)
        {
            chatScreenTopUserBGImage.sprite = defaultWhiteSquare;
        }
        else
        {
            chatScreenTopUserBGImage.sprite = defaultWhiteCircle;
        }
        StartCoroutine(WaitToActiveSoftMask());
    }

    IEnumerator WaitToActiveSoftMask()
    {
        yield return new WaitForSeconds(0.07f);
        chatScreenTopUserBGImage.GetComponent<SoftMask>().enabled = true;
    }


    #region select friends Following List API methods references.......
    IEnumerator WaitToCallRequestGetAllFollowing()
    {
        yield return new WaitForSeconds(1);
        GetAllFollowingForSelectFriends();
    }

    //this method is used to get all following api calling.......
    public void GetAllFollowingForSelectFriends()
    {
        SelectFriendFollowinPaginationResetData();//Reset select friends following api pagination.......
        APIManager.Instance.RequestGetAllFollowing(1, 100, "Message");
    }

    //this method is used to select friend screen scroll pagination apis.......
    public void SelectFriendsFollowingListPagination(ScrollRect scrollRect)
    {
        //return;
        //Debug.LogError("Scrollview verticalNormalPos:" + scrollRect.verticalNormalizedPosition);
        if (scrollRect.verticalNormalizedPosition <= 0.01f && isSelectFriendDataLoaded)
        {
            if (APIManager.Instance.allFollowingRoot.data.rows.Count > 0)
            {
                //Debug.LogError("Select friend following pagination api call.......");
                isSelectFriendDataLoaded = false;
                selectFriendPaginationPageNum += 1;
                APIManager.Instance.RequestGetAllFollowing(selectFriendPaginationPageNum, 100, "Message");
            }
        }
    }

    public void SelectFriendFollowinPaginationResetData()
    {
        isSelectFriendDataLoaded = false;
        selectFriendPaginationPageNum = 1;
    }
    #endregion

    public void OnClickChatScreenBackButton()
    {
        if (currentConversationData != null)
        {
            currentConversationData.ResetUnReadMessageCount();
        }
        ChatScreenDataScript.Instance.allChatGetConversationDatum = null;
        currentConversationData = null;
        allChatGetConversationDatum = null;
        ChatScreen.SetActive(false);        
        MessageListScreen.SetActive(true);
        isDirectMessage = false;
        if (OtherPlayerProfileData.Instance != null)
        {
            if (OtherPlayerProfileData.Instance.isDirectMessageScreenOpen)
            {
                MessageController.Instance.isDirectMessageFirstTimeRecivedID = "";
                OtherPlayerProfileData.Instance.isDirectMessageScreenOpen = false;
                footerCan.GetComponent<BottomTabManager>().OnClickFeedButton();
            }
            else
            {
                APIManager.Instance.RequestChatGetConversation();
            }
        }
        else
        {
            APIManager.Instance.RequestChatGetConversation();
        }
    }

    public void ActiveSelectionScroll()
    {
        StartCoroutine(IEActiveSelectionScroll());

        if (CreateNewMessageUserList.Count > 1 && addFrindCallingScreenIndex == 0)//rik for group next button active
        {
            findFriendNextBtn.SetActive(true);
        }
        else
        {
            findFriendNextBtn.SetActive(false);
        }

        if (CreateNewMessageUserList.Count > 0)
        {
            if (GameManager.currentLanguage == "ja")
            {
                tottleFollowingText.text = (tottleFollowing + " " + TextLocalization.GetLocaliseTextByKey("of") + " " + CreateNewMessageUserList.Count + " " + TextLocalization.GetLocaliseTextByKey("selected"));
            }
            else
            {
                tottleFollowingText.text = (CreateNewMessageUserList.Count + " " + TextLocalization.GetLocaliseTextByKey("of") + " " + tottleFollowing + " " + TextLocalization.GetLocaliseTextByKey("selected"));
            }
        }
        else
        {
            tottleFollowingText.text = TextLocalization.GetLocaliseTextByKey("Add participants");
            //tottleFollowingText.GetComponent<TextLocalization>().LocalizeTextText();
        }
    }

    public IEnumerator IEActiveSelectionScroll()    //.......selectfriendscreen.........
    {
        yield return new WaitForSeconds(0.01f);
        //  Debug.LogError("looggggg :" + APIController.Instance.selectedFriendItemPrefabParent.childCount);

        if (selectedFriendItemPrefabParent.childCount > 0)
        {
            // Debug.LogError("looggggg" + APIController.Instance.selectedFriendItemPrefabParent.childCount);
            if (!selectionScrollView.gameObject.activeSelf)
            {
                //Debug.LogError("enable");
                selectionScrollView.SetActive(true);
            }
        }
        else
        {
            // Debug.LogError("desable");
            selectionScrollView.SetActive(false);
        }
        yield return new WaitForSeconds(0.05f);
        selectionScrollView.transform.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        //yield return new WaitForSeconds(0.01f);
        //selectionScrollView.transform.parent.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.01f);
        selectionScrollView.transform.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        //yield return new WaitForSeconds(0.01f);
        //selectionScrollView.transform.parent.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        //   Invoke("ResetScrollView", 0.1f);

        if (selectionScrollView.activeSelf)
        {
            searchManagerFindFriends.mainScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -600f);
            searchManagerFindFriends.searchScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -600f);
        }
        else
        {
            searchManagerFindFriends.mainScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -333f);
            searchManagerFindFriends.searchScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -333f);
        }
    }

    public void OnClickSelectFriendScreenOkButton()
    {
        CreateChatTitleString(APIController.Instance.allChatMemberList);
        if (selectedFriendItemPrefabParent.childCount > 0)
        {
            if (addFrindCallingScreenIndex != 1)
            {
                foreach (Transform item in chatPrefabParent)
                {
                    Destroy(item.gameObject);
                }
                APIController.Instance.chatTimeList.Clear();
            }
            if (CreateNewMessageUserList.Count > 0)
            {
                Debug.LogError("group addFrindCallingScreenIndex:" + addFrindCallingScreenIndex);
                if (addFrindCallingScreenIndex == 1)
                {
                    AddmeberOnGroup();
                    SelectFriendScreen.SetActive(false);
                    FindFriendUiRefreshAfterCloseScreen();
                    return;
                }
                if (CreateNewMessageUserList.Count > 1)
                {
                    SetUpGroupNameData();
                    return;
                }
                else
                {
                    APIController.Instance.allChatMessageId.Clear();
                    for (int i = 0; i < APIManager.Instance.allChatGetConversationRoot.data.Count; i++)
                    {
                        if (APIManager.Instance.allChatGetConversationRoot.data[i].receivedGroupId == 0)
                        {
                            // Debug.LogError("id" + int.Parse(CreateNewMessageUserList[0]));
                            if (APIManager.Instance.allChatGetConversationRoot.data[i].receiverId == int.Parse(CreateNewMessageUserList[0]))
                            {
                                APIManager.Instance.RequestChatGetMessages(1, 50, APIManager.Instance.allChatGetConversationRoot.data[i].receiverId, 0, "");
                            }
                            else if (APIManager.Instance.allChatGetConversationRoot.data[i].senderId == int.Parse(CreateNewMessageUserList[0]))
                            {
                                APIManager.Instance.RequestChatGetMessages(1, 50, APIManager.Instance.allChatGetConversationRoot.data[i].senderId, 0, "");
                            }
                        }
                    }

                    if (createNewMessageUserAvatarSPList.Count > 0)
                    {
                        ChatScreenTopUserProfileSetUp(createNewMessageUserAvatarSPList[0], false);
                    }
                }
            }

            isDirectMessage = true;
            ChatScreen.SetActive(true);

            string TempChatTitle = chatTitleText.text.ToString();

            chatTitleText.text = chatTitlestr;

            if (chatTitleText.text != TempChatTitle)
            {
                //typeMessageText.text = "";
                chatTypeMessageInputfield.Text = "";
                OnChatVoiceOrSendButtonEnable();
            }
            SelectFriendScreen.SetActive(false);
            //FindFriendUiRefreshAfterCloseScreen();
        }
    }

    //this method is used to create direct message from SNS Other user Profile screen....... 
    public void OnDirectMessageFromOtherUserProfile(int userID, string userName, Sprite avatarSP)
    {
        CreateNewMessageUserList.Clear();
        createNewMessageUserAvatarSPList.Clear();

        APIController.Instance.allChatMessageId.Clear();
        APIController.Instance.chatTimeList.Clear();
        currentChatPage = 1;
        isChatDataLoaded = false;

        foreach (Transform item in chatPrefabParent)
        {
            Destroy(item.gameObject);
        }

        CreateNewMessageUserList.Add(userID.ToString());
        createNewMessageUserAvatarSPList.Add(avatarSP);
        APIController.Instance.allChatMemberList.Add(userName);
        CreateChatTitleString(APIController.Instance.allChatMemberList);

        isDirectMessageFirstTimeRecivedID = CreateNewMessageUserList[0];
        APIManager.Instance.RequestChatGetMessages(1, 50, userID, 0, "Conversation");

        if (createNewMessageUserAvatarSPList.Count > 0)
        {
            ChatScreenTopUserProfileSetUp(createNewMessageUserAvatarSPList[0], false);
        }

        isDirectMessage = true;
        ChatScreen.SetActive(true);

        string TempChatTitle = chatTitleText.text.ToString();

        chatTitleText.text = chatTitlestr;

        if (chatTitleText.text != TempChatTitle)
        {
            chatTypeMessageInputfield.Text = "";
            OnChatVoiceOrSendButtonEnable();
        }

        APIController.Instance.allChatMemberList.Clear();

        allChatGetConversationDatum = null;
        if (APIManager.Instance.allChatGetConversationRoot.data != null)
        {
            for (int i = 0; i < APIManager.Instance.allChatGetConversationRoot.data.Count; i++)
            {
                if (APIManager.Instance.allChatGetConversationRoot.data[i].receiverId == userID || APIManager.Instance.allChatGetConversationRoot.data[i].senderId == userID)
                {
                    allChatGetConversationDatum = APIManager.Instance.allChatGetConversationRoot.data[i];
                    break;
                }
            }
            /*int index = APIManager.Instance.allChatGetConversationRoot.data.FindIndex(x => x.receiverId == userID);
            if(index >= 0 && index < APIManager.Instance.allChatGetConversationRoot.data.Count)
            {
                allChatGetConversationDatum = APIManager.Instance.allChatGetConversationRoot.data[index];
            }*/
        }
    }

    #region Set Group Name.......
    public void SetUpGroupNameData()
    {
        setGroupNameScreenIndex = 0;//setup Group User Name Screen.

        setGroupNameParticipantsText.text = TextLocalization.GetLocaliseTextByKey("Participants:") + " " + CreateNewMessageUserList.Count;
        //setGroupNameScreen.SetActive(true);

        SetGroupNameScreenSetup();//setup group name screen.

        foreach (Transform item in setGroupMemberContainer)
        {
            Destroy(item.gameObject);
        }

        Debug.LogError("setupGroupData:" + selectedFriendItemPrefabParent.childCount);
        for (int i = 0; i < selectedFriendItemPrefabParent.childCount; i++)
        {
            GameObject g = Instantiate(selectedFriendItemPrefabParent.GetChild(i).gameObject, setGroupMemberContainer);
            g.GetComponent<Button>().interactable = false;
            g.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //this method is used to set group name submit button click.
    public void OnSetGroupNameBtnClick()//rik start
    {
        string tempStr = setGroupNameInputField.text.TrimStart(' ');
        Debug.LogError("groupname str:" + tempStr + ":length:" + tempStr.Length);

        setGroupNameInputField.text = tempStr;

        Debug.LogError("OnSetGroupNameBtnClick.......:" + setGroupNameInputField.text);
        if (!string.IsNullOrEmpty(setGroupNameInputField.text))
        {
            if (!string.IsNullOrEmpty(setGroupAvatarTempPath))
            {
                //SetGroupNameLoaderActive(true);
                LoaderShow(true);
                //byte[] bytes = setGroupTempAvatarTexture.EncodeToPNG();
                //File.WriteAllBytes(setGroupAvatarTempPath, bytes);
                AWSHandler.Instance.PostAvatarObject(setGroupAvatarTempPath, setGroupAvatarTempFilename, "CreateGroupAvatar");//upload avatar image on AWS.
            }
            else
            {
                //create group with empty avatar url
                CreateNewGroup("");
            }
        }
        else
        {
            Debug.LogError("Please enter group name");

            if (groupnameErrorCo != null)
            {
                StopCoroutine(groupnameErrorCo);
                groupNameValidationObj.SetActive(false);
            }
            groupNameValidationObj.SetActive(true);
            groupnameErrorCo = StartCoroutine(WaitUntilGroupNameErrorAnimationFinished(groupNameValidationObj.GetComponent<Animator>()));
        }
    }

    Coroutine groupnameErrorCo;
    IEnumerator WaitUntilGroupNameErrorAnimationFinished(Animator MyAnim)
    {
        yield return new WaitForSeconds(1.5f);
        //MyAnim.SetBool("playAnim", false);
        groupNameValidationObj.SetActive(false);
    }

    //this method is used to create group.
    public void CreateNewGroup(string avatarUrl)
    {
        //SetGroupNameLoaderActive(false);//false group name loader.
        LoaderShow(false);

        if (setGroupNameScreenIndex == 1)//update group info
        {
            //update Group name and avatar url
            if (string.IsNullOrEmpty(avatarUrl))
            {
                UpdateGroupInfo(allChatGetConversationDatum.group.avatar);
            }
            else
            {
                UpdateGroupInfo(avatarUrl);
            }
            return;
        }

        CreateMemberString(CreateNewMessageUserList);
        Debug.LogError("Create New Group:" + setGroupNameInputField.text + " :GroupID:" + groupMembersStr + "   :AvatarUrl:" + avatarUrl);
        isDirectCreateFirstTimeGroupName = setGroupNameInputField.text;
        APIManager.Instance.RequestChatCreateGroup(setGroupNameInputField.text, groupMembersStr, avatarUrl);

        isDirectMessage = true;
        setGroupNameScreen.SetActive(false);
        groupAvatarImage.sprite = defaultGroupAvatarSP;

        SelectFriendScreen.SetActive(false);
        findFriendNextBtn.SetActive(false);

        if (setGroupTempAvatarTexture != null)
        {
            Sprite TempSP = Sprite.Create(setGroupTempAvatarTexture, new Rect(0, 0, setGroupTempAvatarTexture.width, setGroupTempAvatarTexture.height), new Vector2(0, 0));
            ChatScreenTopUserProfileSetUp(TempSP, true);
        }
        else
        {
            defaultChatScreenTopUserImage = defaultUserImageSquare;
            ChatScreenTopUserProfileSetUp(defaultChatScreenTopUserImage, true);
        }

        ChatScreen.SetActive(true);
        chatTitleText.text = setGroupNameInputField.text;

        FindFriendUiRefreshAfterCloseScreen();

        //Delete Temp File store storage
        /*if (setGroupFromCamera)
        {
            File.Delete(setGroupAvatarTempPath);
        }
        setGroupFromCamera = false;*/
        if (File.Exists(setGroupAvatarTempPath))
        {
            File.Delete(setGroupAvatarTempPath);
        }
        setGroupAvatarTempPath = "";
        setGroupAvatarTempFilename = "";
    }

    public void UpdateGroupInfo(string avatarUrl)
    {
        string tempStr = setGroupNameInputField.text.TrimStart(' ');
        Debug.LogError("groupname str:" + tempStr + ":length:" + tempStr.Length);

        setGroupNameInputField.text = tempStr;

        Debug.LogError("UpdateGroupInfo.......:" + setGroupNameInputField.text);
        if (!string.IsNullOrEmpty(setGroupNameInputField.text))
        {
            LoaderShow(true);//active api loader.
            setGroupNameScreen.SetActive(false);
            groupAvatarImage.sprite = defaultGroupAvatarSP;
            APIManager.Instance.RequestUpdateGroupInfo(allChatGetConversationDatum.group.id.ToString(), setGroupNameInputField.text, avatarUrl);
            chatTitleText.text = setGroupNameInputField.text;
            setGroupNameScreenIndex = 0;
        }
        else
        {
            Debug.LogError("Please enter group name");

            if (groupnameErrorCo != null)
            {
                StopCoroutine(groupnameErrorCo);
                groupNameValidationObj.SetActive(false);
            }
            groupNameValidationObj.SetActive(true);
            groupnameErrorCo = StartCoroutine(WaitUntilGroupNameErrorAnimationFinished(groupNameValidationObj.GetComponent<Animator>()));
        }       
    }

    public void UpdateGroupInFoSuccessResponce()
    {
        if (setGroupTempAvatarTexture != null)
        {
            Sprite TempSP = Sprite.Create(setGroupTempAvatarTexture, new Rect(0, 0, setGroupTempAvatarTexture.width, setGroupTempAvatarTexture.height), new Vector2(0, 0));
            ChatScreenTopUserProfileSetUp(TempSP, true);

            if (AssetCache.Instance.HasFile(setGroupAvatarTempFilename))
            {
                Debug.LogError("IOS update Group Avatar Delete");
                AssetCache.Instance.DeleteAsset(setGroupAvatarTempFilename);
            }

            if (File.Exists(setGroupAvatarTempPath))
            {
                File.Delete(setGroupAvatarTempPath);
            }
            setGroupAvatarTempPath = "";
            setGroupAvatarTempFilename = "";
            //Destroy(setGroupTempAvatarTexture);
        }
    }

    //this is active or deactive loader for set group name.
    /*public void SetGroupNameLoaderActive(bool isActive)
    {
        setGroupNameLoader.SetActive(isActive);
    }*/

    //this method is used to Close Group name screen.
    public void OnClickBackGroupNameBtn()
    {
        if (setGroupNameScreenIndex == 0)
        {
            SelectFriendScreen.SetActive(true);
        }
        else
        {
            setGroupNameScreenIndex = 0;
        }

        if (groupnameErrorCo != null)
        {
            StopCoroutine(groupnameErrorCo);
            groupNameValidationObj.SetActive(false);
        }
        if (File.Exists(setGroupAvatarTempPath))
        {
            File.Delete(setGroupAvatarTempPath);
        }
    }

    public void SetGroupNameScreenSetup()
    {
        setGroupNameScreen.SetActive(true);
        if (setGroupNameScreenIndex == 0)
        {
            groupAvatarImage.sprite = defaultGroupAvatarSP;
            setGroupNameTitleText.text = TextLocalization.GetLocaliseTextByKey("New Group");
            setGroupNameSubTitleText.text = TextLocalization.GetLocaliseTextByKey("Add Subject");
            setGroupNameNextOrDoneText.text = TextLocalization.GetLocaliseTextByKey("Create");
            setGroupNameDescriptionText.text = TextLocalization.GetLocaliseTextByKey("Provide a group subject and optional group icon");
            //setGroupNameTitleText.GetComponent<TextLocalization>().LocalizeTextText();
            //setGroupNameSubTitleText.GetComponent<TextLocalization>().LocalizeTextText();
            setGroupNameInputField.text = "";
            setGroupNameParticipantsText.transform.parent.gameObject.SetActive(true);
            setGroupNameMemberPanelObj.SetActive(true);
        }
        else
        {
            if (chatScreenTopUserImage.sprite == defaultChatScreenTopUserImage)
            {
                groupAvatarImage.sprite = defaultGroupAvatarSP;
            }
            else
            {
                groupAvatarImage.sprite = chatScreenTopUserImage.sprite;
            }
            setGroupNameTitleText.text = TextLocalization.GetLocaliseTextByKey("Group1");
            setGroupNameSubTitleText.text = TextLocalization.GetLocaliseTextByKey("Edit Group Name");
            setGroupNameNextOrDoneText.text = TextLocalization.GetLocaliseTextByKey("Done");
            setGroupNameDescriptionText.text = TextLocalization.GetLocaliseTextByKey("Please change the group name and icon image");
            //setGroupNameSubTitleText.text = TextLocalization.GetLocaliseTextByKey("Update Subject");
            //setGroupNameTitleText.GetComponent<TextLocalization>().LocalizeTextText();
            //setGroupNameSubTitleText.GetComponent<TextLocalization>().LocalizeTextText();
            setGroupNameInputField.text = allChatGetConversationDatum.group.name;
            setGroupNameParticipantsText.transform.parent.gameObject.SetActive(false);
            setGroupNameMemberPanelObj.SetActive(false);
        }
    }

    //this method is used to Update group name btn click.
    public void UpdateGroupNameBtnClick()
    {
        setGroupNameScreenIndex = 1;//setup Group User Name Screen.
        SetGroupNameScreenSetup();
    }

    //this method is used to Add member on Group
    public void AddmeberOnGroup()
    {
        LoaderShow(true);//active api loader.......
        SelectFriendScreen.SetActive(false);
        findFriendNextBtn.SetActive(false);

        CreateMemberString(CreateNewMessageUserList);
        APIManager.Instance.RequestAddGroupMember(allChatGetConversationDatum.group.id.ToString(), allChatGetConversationDatum.id.ToString(), groupMembersStr);
    }

    public string setGroupAvatarTempPath = "";
    public string setGroupAvatarTempFilename = "";
    //private bool setGroupFromCamera = false;
    //[Space]
    public Texture2D setGroupTempAvatarTexture;

    //this method is used to pick group avatar from gellery for group avatar.
    public void OnPickGroupAvatarFromGellery(int maxSize)
    {
#if UNITY_IOS
        if (permissionCheck == "false")
        {
            string url = MyNativeBindings.GetSettingsURL();
            Debug.Log("the settings url is:" + url);
            Application.OpenURL(url);
        }
        else
        {
            iOSCameraPermission.VerifyPermission(gameObject.name, "SampleCallback");
        }
         
#elif UNITY_ANDROID
        RequestPermission();
#endif

        setGroupAvatarTempPath = "";
        setGroupAvatarTempFilename = "";
        //setGroupFromCamera = false;

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                if (setGroupNameMediaOptionScreen.activeSelf)//false meadia option screen.
                {
                    setGroupNameMediaOptionScreen.SetActive(false);
                }

                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                setGroupTempAvatarTexture = texture;

                Debug.LogError("OnPickGroupAvatarFromGellery path: " + path);

                string[] pathArry = path.Split('/');

                //string fileName = pathArry[pathArry.Length - 1];
                string fileName = Path.GetFileName(path);
                Debug.LogError("OnPickGroupAvatarFromGellery FileName: " + fileName);

                string[] fileNameArray = fileName.Split('.');
                string str = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".";
                fileName = fileNameArray[0] + str + fileNameArray[1];

                string filePath = Path.Combine(Application.persistentDataPath, "XanaChat", fileName);

                Debug.LogError("Camera filePath:" + filePath + "    :filename:" + fileName + "   :texture width:" + texture.width + " :height:" + texture.height);

                setGroupAvatarTempPath = filePath;
                setGroupAvatarTempFilename = fileName;

                Crop(texture, setGroupAvatarTempPath);
                //AWSHandler.Instance.PostAvatarObject(path, fileName, "CreateGroupAvatar");                
            }
        });
        Debug.Log("Permission result: " + permission);
    }

    //this method is used to take picture from camera for group avatar.
    public void OnPickGroupAvatarFromCamera(int maxSize)
    {
        setGroupAvatarTempPath = "";
        setGroupAvatarTempFilename = "";
        //setGroupFromCamera = false;
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                if (setGroupNameMediaOptionScreen.activeSelf)//false meadia option screen.
                {
                    setGroupNameMediaOptionScreen.SetActive(false);
                }
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                setGroupTempAvatarTexture = texture;

                Debug.LogError("OnGroupAvatarTakePicture Camera ImagePath : " + path);

                string[] pathArry = path.Split('/');

                //string fileName = pathArry[pathArry.Length - 1];
                string fileName = Path.GetFileName(path);
                Debug.LogError("Camera filename : " + fileName);

                string[] fileNameArray = fileName.Split('.');
                string str = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".";
                fileName = fileNameArray[0] + str + fileNameArray[1];

                string filePath = Path.Combine(Application.persistentDataPath, "XanaChat", fileName);

                Debug.LogError("Camera filePath:" + filePath + "    :filename:" + fileName + "   :texture width:" + texture.width + " :height:" + texture.height);

                setGroupAvatarTempPath = filePath;
                setGroupAvatarTempFilename = fileName;

                //setGroupFromCamera = true;

                Crop(texture, setGroupAvatarTempPath);
            }
        }, maxSize);

        Debug.Log("Permission result: " + permission);
    }
    #endregion

    public string groupMembersStr;
    public void CreateMemberString(List<string> memberList)
    {
        groupMembersStr = "[";
        for (int i = 0; i < memberList.Count; i++)
        {
            if (i < memberList.Count - 1)
            {
                groupMembersStr += memberList[i] + ",";
            }
            else
            {
                groupMembersStr += memberList[i];
            }
        }
        groupMembersStr += "]";
    }

    public string chatTitlestr;
    public void CreateChatTitleString(List<string> memberList)
    {
        chatTitlestr = "";
        for (int i = 0; i < memberList.Count; i++)
        {
            if (i < memberList.Count - 1)
            {
                chatTitlestr += memberList[i] + ",";
            }
            else
            {
                chatTitlestr += memberList[i];
            }
        }
    }

    //this method is used to change Chat Voice or Send Button.......
    public void OnChatVoiceOrSendButtonEnable()
    {
        //if (string.IsNullOrEmpty(typeMessageText.text))
        if (string.IsNullOrEmpty(chatTypeMessageInputfield.Text))
        {
            for (int i = 0; i < chatVoiceButtonObj.Count; i++)
            {
                chatVoiceButtonObj[i].SetActive(true);
            }
            for (int i = 0; i < chatSendButtonObj.Count; i++)
            {
                chatSendButtonObj[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < chatSendButtonObj.Count; i++)
            {
                chatSendButtonObj[i].SetActive(true);
            }
            for (int i = 0; i < chatVoiceButtonObj.Count; i++)
            {
                chatVoiceButtonObj[i].SetActive(false);
            }
        }
    }

    public string isDirectCreateFirstTimeGroupName = "";
    public string isDirectMessageFirstTimeRecivedID = "";

    public bool OnClcikSendMessageButtonbool = false;
    public void OnClcikSendMessageButton(int index)
    {
        Debug.LogError("OnSend Button click:" + OnClcikSendMessageButtonbool);
        if (OnClcikSendMessageButtonbool)
        {
            return;
        }
        OnClcikSendMessageButtonbool = true;
        APIManager.Instance.r_isCreateMessage = true;
        Debug.LogError("isDirectMessage : " + isDirectMessage);
        if (isDirectMessage)
        {
            Debug.LogError("isDirectMessageisDirectMessage : " + isDirectMessage);
            Debug.LogError("count : " + CreateNewMessageUserList.Count);
            if (CreateNewMessageUserList.Count > 0)
            {
                if (CreateNewMessageUserList.Count > 1)
                {
                    Debug.LogError("counts : " + CreateNewMessageUserList.Count);
                    CreateMemberString(CreateNewMessageUserList);
                    Debug.LogError("id : " + APIManager.Instance.ChatCreateGroupRoot.data.id);
                    // APIManager.Instance.RequestChatCreateGroup("New Group", groupMembersStr);
                    Debug.LogError("group iD" + APIManager.Instance.ChatCreateGroupRoot.data.id);
                    //APIManager.Instance.RequestChatCreateMessage(0, APIManager.Instance.ChatCreateGroupRoot.data.id, typeMessageText.text, "", "");
                    APIManager.Instance.RequestChatCreateMessage(0, APIManager.Instance.ChatCreateGroupRoot.data.id, chatTypeMessageInputfield.RichText, "", "");
                    OnClcikSendMessageButtonbool = true;
                    //APIManager.Instance.RequestChatGetMessages(1, 50, 0, APIManager.Instance.ChatCreateGroupRoot.data.id);
                }
                else
                {
                    isDirectMessageFirstTimeRecivedID = CreateNewMessageUserList[0];
                    //APIManager.Instance.RequestChatCreateMessage(int.Parse(CreateNewMessageUserList[0]), 0, typeMessageText.text, "", "");
                    APIManager.Instance.RequestChatCreateMessage(int.Parse(CreateNewMessageUserList[0]), 0, chatTypeMessageInputfield.RichText, "", "");
                    OnClcikSendMessageButtonbool = true;
                }
                SelectFriendScreen.SetActive(false);
                ChatScreen.SetActive(true);
            }
        }
        else
        {
            if (allChatGetConversationDatum.receivedGroupId != 0)
            {
                Debug.LogError("id : " + allChatGetConversationDatum.receivedGroupId + "msg: " + chatTypeMessageInputfield.RichText);
                //Debug.LogError("id : " + allChatGetConversationDatum.receivedGroupId + "msg: " + typeMessageText.text + "string :" + "");
                //APIManager.Instance.RequestChatCreateMessage(0, allChatGetConversationDatum.receivedGroupId, typeMessageText.text, "", "");
                APIManager.Instance.RequestChatCreateMessage(0, allChatGetConversationDatum.receivedGroupId, chatTypeMessageInputfield.RichText, "", "");
            }
            else
            {
                if (allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
                {
                    //APIManager.Instance.RequestChatCreateMessage(allChatGetConversationDatum.senderId, 0, typeMessageText.text, "", "");
                    APIManager.Instance.RequestChatCreateMessage(allChatGetConversationDatum.senderId, 0, chatTypeMessageInputfield.RichText, "", "");
                }
                else
                {
                    APIManager.Instance.RequestChatCreateMessage(allChatGetConversationDatum.receiverId, 0, chatTypeMessageInputfield.RichText, "", "");
                    //APIManager.Instance.RequestChatCreateMessage(allChatGetConversationDatum.receiverId, 0, typeMessageText.text, "", "");
                }
                //Debug.LogError("id : " + allChatGetConversationDatum.receiverId + "msg: " + typeMessageText.text + "string :" + "");
                Debug.LogError("id : " + allChatGetConversationDatum.receiverId + "msg: " + chatTypeMessageInputfield.RichText);
            }
        }
    }
    public void ApiPagination(Vector2 scrollPos)
    {
        //Debug.LogError("ApiPagination Pos:" + scrollPos.y);
        if (scrollPos.y > 0.9 && !isChatDataLoaded && allChatGetConversationDatum != null)//rik last condition added
        {
            isChatDataLoaded = true;
            currentChatPage += 1;
            //if (currentConversationData.allChatGetConversationDatum.receivedGroupId != 0)//rik
            if (allChatGetConversationDatum.receivedGroupId != 0)
            {
                //   Debug.LogError("receivedGroupId" + APIManager.Instance.AllChatCreateMessageRoot.data.receivedGroupId);
                //APIManager.Instance.RequestChatGetMessages(currentChatPage, 50, 0, currentConversationData.allChatGetConversationDatum.receivedGroupId);//rik
                APIManager.Instance.RequestChatGetMessages(currentChatPage, 50, 0, allChatGetConversationDatum.receivedGroupId, "");
            }
            else
            {
                // Debug.LogError("currentChatPage " + currentChatPage);
                //  Debug.LogError("receiverId " + currentConversationData.allChatGetConversationDatum.receiverId);
                //  Debug.LogError("senderId " + currentConversationData.allChatGetConversationDatum.senderId);
                //if (currentConversationData.allChatGetConversationDatum.receiverId == APIManager.Instance.userId)//rik
                if (allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
                {
                    //APIManager.Instance.RequestChatGetMessages(currentChatPage, 50, currentConversationData.allChatGetConversationDatum.senderId, 0);//rik
                    APIManager.Instance.RequestChatGetMessages(currentChatPage, 50, allChatGetConversationDatum.senderId, 0, "");
                }
                else
                {
                    //APIManager.Instance.RequestChatGetMessages(currentChatPage, 50, currentConversationData.allChatGetConversationDatum.receiverId, 0);//rik
                    APIManager.Instance.RequestChatGetMessages(currentChatPage, 50, allChatGetConversationDatum.receiverId, 0, "");
                }
            }
        }
    }    

    #region chat screen view video or image screen methods.......
    public bool isVideo = false;
    public void OnShowChatVideoOrImage(Sprite imageSP, MediaPlayer currentMediaPlayer)
    {
        Debug.LogError("on chat view image or video");
        if (imageSP != null)
        {
            isVideo = false;
            chatScreenViewImageOrVideoScreen.SetActive(true);
            chatScreenViewImageObj.gameObject.SetActive(true);
            chatScreenViewImageObj.sprite = imageSP;
        }
        else
        {
            isVideo = true;
            chatScreenViewImageOrVideoScreen.SetActive(true);
            chatScreenViewVideoDisplayObj.SetActive(true);
            //chatScreenViewVideoMediaPlayer.SetActive(true);
            chatScreenViewVideoMediaPlayer = currentMediaPlayer;
            chatScreenViewVideoDisplayObj.GetComponent<DisplayUGUI>().Player = currentMediaPlayer;
            currentMediaPlayer.Play();
        }
    }

    public void OnClickChatViewVideoOrImageCloseButton() 
    {
        if (!isVideo)
        {
            chatScreenViewImageObj.sprite = null;
        }
        else
        {
            chatScreenViewVideoMediaPlayer.Stop();
        }
        chatScreenViewImageObj.gameObject.SetActive(false);
        chatScreenViewVideoDisplayObj.SetActive(false);
        chatScreenViewImageOrVideoScreen.SetActive(false);
    }
    #endregion

    #region Chat screen select image or video.......
    //This method are used to pick image from Gallery in chat screen.......
    public void OnSelectImage()
    {
        //PickImage(1024);
        PickImageOrVideo();
    }

    public string attechmentstr;
    public string attechmentArraystr;
    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                Debug.LogError("ImagePath : " + path);
                attechmentstr = "[" + attechmentArraystr + path + attechmentArraystr + "]";
                APIManager.Instance.r_isCreateMessage = true;

                string[] pathArry = path.Split('/');

                //string fileName = pathArry[pathArry.Length - 1];
                string fileName = Path.GetFileName(path);
                Debug.LogError("filename : " + fileName);

                AWSHandler.Instance.PostObject(path, fileName);
            }
        });
        Debug.Log("Permission result: " + permission);
    }

    private void PickImageOrVideo()
    {
        if (NativeGallery.CanSelectMultipleMediaTypesFromGallery())
        {
            NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
            {
                Debug.Log("Media path: " + path);
                if (path != null)
                {
                    // Determine if user has picked an image, video or neither of these
                    switch (NativeGallery.GetMediaTypeOfFile(path))
                    {
                        case NativeGallery.MediaType.Image: Debug.Log("Picked image"); break;
                        case NativeGallery.MediaType.Video: Debug.Log("Picked video"); break;
                        default: Debug.Log("Probably picked something else"); break;
                    }
                    attechmentstr = "[" + attechmentArraystr + path + attechmentArraystr + "]";
                    APIManager.Instance.r_isCreateMessage = true;

                    string[] pathArry = path.Split('/');

                    //string fileName = pathArry[pathArry.Length - 1];
                    string fileName = Path.GetFileName(path);
                    Debug.LogError("filename : " + fileName);

                    AWSHandler.Instance.PostObject(path, (Time.time + fileName));
                }
            }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

            Debug.Log("Permission result: " + permission);
        }
    }

    //this method are used to chat screen camera button click....... 
    public void OnClickTakePictureBtnClick()
    {
#if UNITY_IOS
        if (permissionCheck == "false")
        {
            string url = MyNativeBindings.GetSettingsURL();
            Debug.Log("the settings url is:" + url);
            Application.OpenURL(url);
        }
        else
        {
            iOSCameraPermission.VerifyPermission(gameObject.name, "SampleCallback");
        }
         
#elif UNITY_ANDROID
        RequestPermission();
#endif
        TakePicture(1024);
    }

    public RawImage tempRawImg;
    private void TakePicture(int maxSize)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Camera Image path: " + path);
            if (path != null)
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                Debug.LogError("Camera ImagePath : " + path);
                attechmentstr = "[" + attechmentArraystr + path + attechmentArraystr + "]";
                APIManager.Instance.r_isCreateMessage = true;

                string[] pathArry = path.Split('/');

                //string fileName = pathArry[pathArry.Length - 1];
                string fileName = Path.GetFileName(path);
                Debug.LogError("Camera filename : " + fileName);

                string[] fileNameArray = fileName.Split('.');
                string str = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".";
                fileName = fileNameArray[0] + str + fileNameArray[1];

                Debug.LogError("Camera filename11212 : " + fileName + "   :texture width:" + texture.width + " :height:" + texture.height);
                tempRawImg.texture = texture;

                //byte[] bytes = File.ReadAllBytes(path);
                byte[] bytes = texture.EncodeToPNG();
                Debug.LogError("camera total bytes:" + bytes);

                /*Texture2D tttt = new Texture2D(2, 2, TextureFormat.RGB24, false);
                tttt.filterMode = FilterMode.Bilinear;
                tttt.LoadImage(bytes);
                Debug.LogError("size w:" + tttt.width + " :h:" + tttt.height + "    :bytes:"+ bytes.Length);*/

                string filePath = Path.Combine(Application.persistentDataPath, "XanaChat", fileName + "png");

                /*string saveDir = Path.Combine(Application.persistentDataPath, "XanaChat");
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }*/

                File.WriteAllBytes(filePath, bytes);//Delete karvani baki che

                AWSHandler.Instance.PostObject(filePath, fileName);
                //AWSHandler.Instance.PostObject(path, fileName);
            }
        }, maxSize);

        Debug.Log("Permission result: " + permission);
    }

    private void RecordVideo()
    {
        NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // Play the recorded video
                Handheld.PlayFullScreenMovie("file://" + path);
            }
        });
    }
    #endregion

    #region Chat Details Screen Methods.......
    public void OnClickChatScreenMenuButton()//old
    {
        MessageDetailScreen.SetActive(true);
    }

    //this method is used to chat screen top user button click.......
    public void OnClickChatMenuButton()
    {
        if (allChatGetConversationDatum == null || allChatGetConversationDatum.id == 0 && allChatGetConversationDatum.receiverId == 0 && allChatGetConversationDatum.receivedGroupId == 0 && allChatGetConversationDatum.senderId == 0)
        {
            Debug.LogError("Attachment Data Is null");
            chatShareAttechmentparent.gameObject.SetActive(false);
            APIController.Instance.SetChatMember();
        }
        else
        {
            if (allChatGetConversationDatum.receivedGroupId != 0)
            {
                APIManager.Instance.RequestChatGetAttachments(1, 50, 0, allChatGetConversationDatum.receivedGroupId, 0);
            }
            else
            {
                if (allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
                {
                    APIManager.Instance.RequestChatGetAttachments(1, 50, allChatGetConversationDatum.senderId, 0, 0);
                }
                else
                {
                    APIManager.Instance.RequestChatGetAttachments(1, 50, allChatGetConversationDatum.receiverId, 0, 0);
                }
            }
        }        
    }

    //this method is used to setup details screen.......
    public void MessageDetailsSceenLeaveChatActive()
    {
        //check for group or personal.......
        //Debug.LogError("allChatGetConversationDatum:" + allChatGetConversationDatum.group.name);        

        if (allChatGetConversationDatum != null)
        {
            Debug.LogError("MessageDetailsSceenLeaveChatActive allChatGetConversationDatum not null");
            if (string.IsNullOrEmpty(allChatGetConversationDatum.group.name))
            {
                for (int i = 0; i < messageDetailsLeaveGroupFalseObjList.Count; i++)
                {
                    messageDetailsLeaveGroupFalseObjList[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < messageDetailsLeaveGroupFalseObjList.Count; i++)
                {
                    messageDetailsLeaveGroupFalseObjList[i].SetActive(true);
                }

                if (allChatGetConversationDatum.group.createdBy != APIManager.Instance.userId)
                {
                    messageDetailsLeaveGroupFalseObjList[1].SetActive(false);//select member button
                    messageDetailsLeaveGroupFalseObjList[5].SetActive(false);//update group info button
                    messageDetailsLeaveGroupFalseObjList[6].SetActive(false);//delete group button
                }
                else
                {
                    messageDetailsLeaveGroupFalseObjList[4].SetActive(false);//leave group button
                }
            }
        }
        else
        {
            Debug.LogError("MessageDetailsSceenLeaveChatActive allChatGetConversationDatum null");
            for (int i = 0; i < messageDetailsLeaveGroupFalseObjList.Count; i++)
            {
                messageDetailsLeaveGroupFalseObjList[i].SetActive(false);
            }
        }
    }

    //this method is used to details screen allow notification button click.......
    public void OnClickChatDetailsScreenAllowNotification()
    {
        if (allChatGetConversationDatum != null)
        {
            if (MessageDetailScreen.activeSelf)
            {
                APIManager.Instance.RequestChatMuteUnMuteConversation(allChatGetConversationDatum.id);
            }
            else
            {
                messageDetailsAllowNotificationToggle.isOn = !allChatGetConversationDatum.isMutedConversations;
            }
        }
        else
        {
            messageDetailsAllowNotificationToggle.isOn = true;
        }
    }

    //this method is used to details screen back button click.......
    public void OnClickDetailScreenBackButton()
    {
        Debug.LogError("click");
        MessageDetailScreen.SetActive(false);
        ChatScreen.SetActive(true);
        foreach (Transform item in chatShareAttechmentparent)
        {
            Destroy(item.gameObject);
        }
    }

    //this method is used to add group member button click........
    public void OnClickAddChatMemberButton()
    {
        if (allChatGetConversationDatum.receivedGroupId != 0)
        {
            for (int i = 0; i < allChatGetConversationDatum.group.groupUsers.Count; i++)
            {
                if (allChatGetConversationDatum.group.groupUsers[i].user.id == APIManager.Instance.userId)
                {
                }
                else
                {
                    APIController.Instance.allChatMemberList.Add(allChatGetConversationDatum.group.groupUsers[i].user.name);
                }
            }
        }
        else
        {
            if (allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
            {
                APIController.Instance.allChatMemberList.Add(allChatGetConversationDatum.ConSender.name);
            }
            else
            {
                APIController.Instance.allChatMemberList.Add(allChatGetConversationDatum.ConReceiver.name);
            }
        }
    }
    #endregion

    #region Level The chat details screen methods.......
    //this method is used to Leave the chat for detail screen.......
    public bool isLeaveGroup = false;
    public void OnClickLeaveTheChatButton()
    {
        Debug.LogError("OnLeaveChat calling.......:" + allChatGetConversationDatum.group.id);
        //calling leave the api here.......
        APIManager.Instance.RequestLeaveTheChat(allChatGetConversationDatum.group.id.ToString(), "DetailsScreen");
    }

    public void LeaveGroupAfterRemoveMemberFromCurrentConversation(GroupLeaveResponceRoot groupLeaveResponceRoot)
    {
        for (int i = 0; i < groupLeaveResponceRoot.userList.Count; i++)
        {
            for (int j = 0; j < allChatGetConversationDatum.group.groupUsers.Count; j++)
            {
                Debug.LogError("after leave id:" + allChatGetConversationDatum.group.groupUsers[j].id + "    user:" + groupLeaveResponceRoot.userList[i]);
                if (allChatGetConversationDatum.group.groupUsers[j].id == groupLeaveResponceRoot.userList[i])
                {
                    allChatGetConversationDatum.group.groupUsers.Remove(allChatGetConversationDatum.group.groupUsers[j]);
                    break;
                }
            }
        }
    }
    #endregion

    #region chat attachment Screen methods.......
    //this method is used to open photo or video arrow button click.......
    public void OnClickOpenAttechmentScreenButton()
    {
        foreach (Transform item in chooseAttechmentparent)
        {
            Destroy(item.gameObject);
        }

        if (allChatGetConversationDatum == null || allChatGetConversationDatum.id == 0 && allChatGetConversationDatum.receiverId == 0 && allChatGetConversationDatum.receivedGroupId == 0 && allChatGetConversationDatum.senderId == 0)
        {
            NoAttechmentScreen.SetActive(true);
            chatShareAttechmentparent.gameObject.SetActive(false);
        }
        else
        {
            if (allChatGetConversationDatum.receivedGroupId != 0)
            {
                APIManager.Instance.RequestChatGetAttachments(1, 50, 0, allChatGetConversationDatum.receivedGroupId, 1);
            }
            else
            {
                if (allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
                {
                    APIManager.Instance.RequestChatGetAttachments(1, 50, allChatGetConversationDatum.senderId, 0, 1);
                }
                else
                {
                    APIManager.Instance.RequestChatGetAttachments(1, 50, allChatGetConversationDatum.receiverId, 0, 1);
                }
            }
        }

        MessageDetailScreen.SetActive(false);
        ChooseScreen.SetActive(true);
    }

    //this method is used to choose screen back button click........
    public void OnClickCloseAttechmentScreenBUtton()
    {
        MessageDetailScreen.SetActive(true);

        ChooseScreen.SetActive(false);

        foreach (Transform item in chooseAttechmentparent)
        {
            Destroy(item.gameObject);
        }
    }

    //this method is used to save attachment screen back button click....... 
    public void OnClickSaveAttachmentScreenBackButton()
    {
        AttechmentDownloadScreen.SetActive(false);
        foreach (Transform item in saveAttechmentParent)
        {
            Destroy(item.gameObject);
        }
    }

    public void SaveAttachmentDetailsSetup(int index)
    {
        //Debug.LogError("SaveAttachmentDetailsSetup.......:" + index);
        saveAttachmentSenderNameText.text = APIManager.Instance.AllChatAttachmentsRoot.data.rows[index].message.messageRecipient.sender.name;
        DateTime today = TimeZoneInfo.ConvertTimeFromUtc(APIManager.Instance.AllChatAttachmentsRoot.data.rows[index].updatedAt, TimeZoneInfo.Local);
        //Debug.LogError("Uploaded Time" + today.Day + " " + today.ToString("MMM") + " " + today.Year);
        string dateSTR = today.Day + " " + today.ToString("MMM") + " " + today.Year;
        saveAttachmentDateTimeText.text = dateSTR;
    }
    #endregion

    #region Delete Conversation, Delete Group and Remove member methods reference.......
    //this method is used to remove member confirmation screen ok button.......    
    public void OnClickRemoveMemberConfirmationOkButton()
    {
        removeGroupmemberConfirmationScreen.SetActive(false);
        APIManager.Instance.RequestRemoveGroupMember(currentSelectedGroupMemberDataScript.chatGetConversationUser.groupId, currentSelectedGroupMemberDataScript.chatGetConversationUser.userId);
    }

    //this method is used to remove member api response success.......
    public void RemoveMemberApiResponseSuccess()
    {
        if (currentSelectedGroupMemberDataScript != null)
        {
            if (allChatGetConversationDatum.group != null)
            {
                allChatGetConversationDatum.group.groupUsers.Remove(currentSelectedGroupMemberDataScript.chatGetConversationUser);
            }
            DestroyImmediate(currentSelectedGroupMemberDataScript.gameObject);
            currentSelectedGroupMemberDataScript = null;

            SNSNotificationManager.Instance.ShowNotificationMsg("Removed success");//this method is used to show SNS notification.......

            StartCoroutine(APIController.Instance.WaitToSetDetailsScreen());
        }
    }

    //this method is used to remove member confirmation screen cancel button.......
    public void OnClickRemoveMemberConfirmationCancelButton()
    {
        if (currentSelectedGroupMemberDataScript != null)
        {
            currentSelectedGroupMemberDataScript = null;
        }
        removeGroupmemberConfirmationScreen.SetActive(false);
    }

    //this method is used to Show and Setup delete confirmation screen.......
    public void ShowSetupDeleteConfirmationScreen(string titleText, string descriptionText)
    {
        deleteConfirmationTitleText.text = TextLocalization.GetLocaliseTextByKey(titleText);
        deleteConfirmationDescriptionText.text = TextLocalization.GetLocaliseTextByKey(descriptionText);
        deleteConfirmationScreen.SetActive(true);
    }

    //this method is used to delete confirmation ok button click.......
    public void OnClickDeleteConfirmationOkButton()
    {
        deleteConfirmationScreen.SetActive(false);
        if (deleteConfirmationCurrentConversationDataScript != null)
        {
            if (deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum.group != null && deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum.receivedGroupId != 0)
            {
                if (deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum.group.createdBy == APIManager.Instance.userId)//group admin is this user.......
                {
                    Debug.LogError("delete group chat calling.......");
                    APIManager.Instance.RequestDeleteChatGroup(deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum.receivedGroupId, "ConversationScreen");
                }
                else//group admin is not this user.......
                {
                    Debug.LogError("Leave Group calling.......");
                    APIManager.Instance.RequestLeaveTheChat(deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum.receivedGroupId.ToString(), "ConversationScreen");
                }
            }
            else//one to one conversation.......
            {
                Debug.LogError("delete one to one conversation calling.......");
                APIManager.Instance.RequestDeleteConversation(deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum.id);
            }
        }
    }

    //this method is used to delete conversation api response success and refresh data.......
    public void DeleteConversationApiResponseSuccess(string notificationMessage)
    {
        if (deleteConfirmationCurrentConversationDataScript != null)
        {
            Debug.LogError("DeleteConversationApiResponseSuccess.......");
            if (deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum != null)
            {
                searchManagerAllConversation.allConversationDatasList.Remove(deleteConfirmationCurrentConversationDataScript);
                if (APIManager.Instance.allChatGetConversationRoot.data.Contains(deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum))
                {
                    int index = APIManager.Instance.allChatGetConversationRoot.data.IndexOf(deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum);
                    APIManager.Instance.allChatGetConversationRoot.data.Remove(deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum);
                    Debug.LogError("Index:" + index);
                    if (index >= 0)
                    {
                        APIController.Instance.allConversationList.RemoveAt(index);
                        DestroyImmediate(searchManagerAllConversation.searchContainer.transform.GetChild(index).gameObject);
                    }
                }
            }

            DestroyImmediate(deleteConfirmationCurrentConversationDataScript.gameObject);
            deleteConfirmationCurrentConversationDataScript = null;

            SNSNotificationManager.Instance.ShowNotificationMsg(notificationMessage);//this method is used to show SNS notification.......
        }
    }

    //this method is used to delete conversation with leave group api response success and refresh data.......
    public void DeleteConversationWithLeaveGroupApiResponseSuccess(string groupId)
    {
        if (deleteConfirmationCurrentConversationDataScript != null)
        {
            ChatGetConversationGroupUser etc = deleteConfirmationCurrentConversationDataScript.allChatGetConversationDatum.group.groupUsers.Find((x) => x.userId == APIManager.Instance.userId);

            isLeaveGroup = true;
            //after leave group then create leave user msg on this group.......
            APIManager.Instance.r_isCreateMessage = true;
            Debug.LogError("DeleteConversationWithLeaveGroupApiResponseSuccess removed User Name:" + etc.user.name);
            string messageStr = etc.user.name + " Left";
            APIManager.Instance.RequestChatCreateMessage(0, int.Parse(groupId), messageStr, "LeaveGroup", "");

            DeleteConversationApiResponseSuccess("Leaved Group");
        }
    }

    //this method is used to delete confirmation cancel button click.......
    public void OnClickDeleteConfirmationCancelButton()
    {
        if (deleteConfirmationCurrentConversationDataScript != null)
        {
            deleteConfirmationCurrentConversationDataScript = null;
        }
        deleteConfirmationScreen.SetActive(false);
    }

    //this method is used to details screen delete group button click.......
    public void OnClickDeleteGroupChatButton()
    {
        Debug.LogError("Detele group button click.......");
        APIManager.Instance.RequestDeleteChatGroup(allChatGetConversationDatum.receivedGroupId, "DetailsScreen");
    }

    public void DeleteGroupChatApiResponseSuccess()
    {
        allChatGetConversationDatum = null;
        Instance.currentConversationData = null;
        MessageDetailScreen.SetActive(false);
        OnClickMessageButton();//active message list screen and refreshing list api

        SNSNotificationManager.Instance.ShowNotificationMsg("Group Deleted");//this method is used to show SNS notification.......

        //if user fire message then create message with group deleted no longer you send message
        /*MessageController.Instance.isLeaveGroup = true;
        //after leave group then create leave user msg on this group.......
        APIManager.Instance.r_isCreateMessage = true;
        Debug.LogError("removed User Name:" + etc.user.name);
        string messageStr = etc.user.name + " Left";
        APIManager.Instance.RequestChatCreateMessage(0, int.Parse(groupId), messageStr, "LeaveGroup", "");*/
    }
    #endregion

    #region Get Image From AWS
    public void GetImageFromAWS(string key, Image mainImage)
    {
        //Debug.LogError("GetImageFromAWS key:" + key);
        GetExtentionType(key);
        if (AssetCache.Instance.HasFile(key))
        {
            //Debug.LogError("Image Available on Disk");
            AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
            return;
        }
        else
        {
            AWSHandler.Instance.Client.GetObjectAsync(AWSHandler.Instance.Bucketname, key, (responseObj) =>
            {
                byte[] data = null;
                var response = responseObj.Response;
                Debug.LogError("GetImageFromAWS Response:" + response.ResponseStream);
                if (response.ResponseStream != null)
                {
                    using (StreamReader reader = new StreamReader(response.ResponseStream))
                    {
                        using (var memstream = new MemoryStream())
                        {
                            var buffer = new byte[512];
                            var bytesRead = default(int);
                            while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                memstream.Write(buffer, 0, bytesRead);
                            data = memstream.ToArray();
                        }
                        if (currentExtention == ExtentionType.Image)
                        {
                            Texture2D texture = new Texture2D(2, 2);
                            texture.LoadImage(data);
                            Debug.LogError("key " + key + " :texture width:" + texture.width + "  height:" + texture.height);

                            AssetCache.Instance.SaveImageEnqueueOneResAndWait(key, key, data, (success) =>
                            {
                                if (success)
                                    Debug.LogError("Save on Aws sucess");
                                //AssetCache.Instance.LoadSpriteIntoImage(profileImage, allChatGetConversationDatum.ConReceiver.avatar, changeAspectRatio: true);
                            });
                            mainImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                        }
                    }
                }
            });
        }
    }


    public static ExtentionType currentExtention;
    public static ExtentionType GetExtentionType(string path)
    {
        if (string.IsNullOrEmpty(path))
            return (ExtentionType)0;

        string extension = Path.GetExtension(path);
        if (string.IsNullOrEmpty(extension))
            return (ExtentionType)0;

        if (extension[0] == '.')
        {
            if (extension.Length == 1)
                return (ExtentionType)0;

            extension = extension.Substring(1);
        }

        extension = extension.ToLowerInvariant();
        //Debug.LogError("ExtentionType " + extension);
        if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "bmp" || extension == "tiff" || extension == "heic")
        {
            currentExtention = ExtentionType.Image;
            return ExtentionType.Image;
        }
        else if (extension == "mp4" || extension == "mov" || extension == "wav" || extension == "avi")
        {
            currentExtention = ExtentionType.Video;
            return ExtentionType.Video;
        }
        else if (extension == "mp3" || extension == "aac" || extension == "flac")
        {
            currentExtention = ExtentionType.Audio;
            return ExtentionType.Audio;
        }
        return (ExtentionType)0;
    }

    public bool CheckUrlDropboxOrNot(string url)
    {
        string[] splitArray = url.Split(':');
        if (splitArray.Length > 0)
        {
            if (splitArray[0] == "https" || splitArray[0] == "http")
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    #region Profile Image Crop.......
    //crop mate .......
    public void Crop(Texture2D LoadedTexture, string path)
    {
        // If image cropper is already open, do nothing
        if (ImageCropper.Instance.IsOpen)
            return;

        StartCoroutine(SetImageCropper(LoadedTexture, path));
    }

    private IEnumerator SetImageCropper(Texture2D screenshot, string path)
    {
        yield return new WaitForEndOfFrame();

        bool ovalSelection = true;
        bool autoZoom = true;

        float minAspectRatio = 1, maxAspectRatio = 1;

        ImageCropper.Instance.Show(screenshot, (bool result, Texture originalImage, Texture2D croppedImage) =>
        {
            // If screenshot was cropped successfully
            if (result)
            {
                Sprite s = Sprite.Create(croppedImage, new Rect(0, 0, croppedImage.width, croppedImage.height), new Vector2(0, 0), 1);
                groupAvatarImage.sprite = s;
                try
                {
                    setGroupTempAvatarTexture = croppedImage;
                    byte[] bytes = croppedImage.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);
                    Debug.LogError("File SAVE");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                //croppedImageHolder.enabled = false;
                //croppedImageSize.enabled = false;
            }

            // Destroy the screenshot as we no longer need it in this case
            //Destroy(screenshot);
        },
        settings: new ImageCropper.Settings()
        {
            //ovalSelection = ovalSelection,
            ovalSelection = false,
            autoZoomEnabled = autoZoom,
            imageBackground = Color.clear, // transparent background
            selectionMinAspectRatio = minAspectRatio,
            selectionMaxAspectRatio = maxAspectRatio,
            markTextureNonReadable = false

        },
        croppedImageResizePolicy: (ref int width, ref int height) =>
        {
            // uncomment lines below to save cropped image at half resolution
            //width /= 2;
            //height /= 2;
        });
    }
    #endregion

    #region Permission Methods
    public void RequestPermission()
    {
        if (UniAndroidPermission.IsPermitted(AndroidPermission.CAMERA))
        {
            Debug.Log("CAMERA is already permitted!!");
            return;
        }

        UniAndroidPermission.RequestPermission(AndroidPermission.CAMERA, OnAllow, OnDeny, OnDenyAndNeverAskAgain);
    }

    private void OnAllow()
    {
        Debug.Log("CAMERA is permitted NOW!!");
    }

    private void OnDeny()
    {
        Debug.Log("CAMERA is NOT permitted...");
    }

    private void OnDenyAndNeverAskAgain()
    {
        Debug.Log("CAMERA is NOT permitted and checked never ask again option");

        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            string packageName = currentActivityObject.Call<string>("getPackageName");

            using (var uriClass = new AndroidJavaClass("android.net.Uri"))
            using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
            using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
            {
                intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                currentActivityObject.Call("startActivity", intentObject);
            }
        }
    }

    public string permissionCheck = "";
    private void SampleCallback(string permissionWasGranted)
    {
        Debug.Log("Callback.permissionWasGranted = " + permissionWasGranted);

        if (permissionWasGranted == "true")
        {
            // You can now use the device camera.
        }
        else
        {
            permissionCheck = permissionWasGranted;

            // permission denied, no access should be visible, when activated when requested permission
            return;

            // You cannot use the device camera.  You may want to display a message to the user
            // about changing the camera permission in the Settings app.
            // You may want to re-enable the button to display the Settings message again.
        }
    }
    #endregion
}