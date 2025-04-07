﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.IO;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI.Extensions;
using UnityEngine.Video;
using System;
using UnityEngine.Networking;
using AdvancedInputFieldPlugin;
using SuperStar.Helpers;
using Amazon.S3.Model;

public class FeedUIController : MonoBehaviour
{
    public static FeedUIController Instance;

    [Header("-------FooterCan-------")]
    public GameObject footerCan;

    [Space]
    [Header("-------API Controller Feed References-------")]
    public Transform followingFeedTabLeftContainer;
    public Transform followingFeedTabRightContainer;
    public Transform followingFeedTabContainer;
    public Transform forYouFeedTabContainer;
    public Transform hotTabContainer;
    public Transform videofeedParent;
    public GameObject followingFeedMainContainer;

    //[Header("-------Dummy Token Set Data-------")]
    //public TMP_InputField tokenInput;
    //public TMP_InputField userIdInput;
    //public GameObject setTokenScreen;

    [Space]
    [Header("-------All Screens-------")]
    public GameObject feedUiScreen;
    public GameObject otherPlayerProfileScreen;
    public GameObject giftItemScreens;
    public GameObject feedVideoScreen;
    public SNSAPILoaderController apiLoaderController;

    [Space]
    [Header("Top Story Panel")]
    public GameObject TopPanelMainObj;
    public GameObject TopPanelMainStoryObj;
    public Transform TopPanelMainContainerObj;

    [Header("GiftScreen")]
    //  public Transform fashionCetegoryContent;
    public Sprite selectedSprite, unSelectedSprite;

    [Space]
    [Header("HorizontalScrollSnap")]
    public HorizontalScrollSnap feedUiHorizontalSnap;

    public GameObject feedUiSelectionLine;
    public Transform[] feedUiSelectionTab;
    public TextMeshProUGUI[] feedUiTabTitleText;
    public Color selectedColor, unSelectedColor;

    [Space]
    [Header("ScrollRectFaster")]
    public ScrollRectFasterEx[] allFeedScrollRectFasterEx;
    public ScrollRectFasterEx feedUiScrollRectFasterEx;

    [Space]
    [Header("AllFeedScreen")]
    public GameObject[] allFeedPanel;
    public GameObject videoFeedRect;
    public List<TextMeshProUGUI> allFeedMessageTextList = new List<TextMeshProUGUI>();

    public int allFeedCurrentpage, followingUserCurrentpage;
    public bool isDataLoad = false;

    public bool isAnyUserFollow = false;

    [Space]
    [Header("Feed All Tabs Loaded checking Variable")]
    public int followingFeedInitiateTotalCount;
    //public int followingFeedImageLoadedCount;
    public int hotFeedInitiateTotalCount;
    //public int HotFeedImageLoadedCount;
    public int hotForYouFeedInitiateTotalCount;
    //public int hotForYouFeedImageLoadedCount;

    [Space]
    //this list is used to unfollowed user feed removed from following tab.......
    public List<int> unFollowedUserListForFollowingTab = new List<int>();

    //public GameObject fingerTouch;
    [Space]
    [Header("FeedVideo Screen")]
    public RectTransform feedVideoButtonPanelImage;
    public string feedFullViewScreenCallingFrom = "";

    [Space]
    [Header("Find Friend screen References")]
    public GameObject findFriendScreen;
    public Transform findFriendContainer;
    public TMP_InputField findFriendInputField;
    public AdvancedInputField findFriendInputFieldAdvanced;

    [Space]
    [Header("Create Feed Screen References")]
    public GameObject createFeedScreen;
    public TMP_InputField createFeedTitle;
    public AdvancedInputField createFeedTitleAdvanced;
    public TMP_InputField createFeedDescription;
    public AdvancedInputField createFeedDescriptionAdvanced;
    public Image createFeedImage;
    public GameObject createFeedVideoObj;
    public MediaPlayer createFeedMediaPlayer;

    [Space]
    [Header("Edit Delete Feed Screen Reference")]
    public FeedEditOrDeleteData feedEditOrDeleteData;
    public GameObject editDeleteFeedScreen;
    public TextMeshProUGUI editDeleteFeedUserNameText;
    public TextMeshProUGUI editDeleteFeedDateTimeText;
    public Image editDeleteCurrentFeedImage;
    public GameObject editDeleteVideoDisplay;
    //public MediaPlayer editDeleteMideaPlayer;
    public PostFeedVideoItem editDeleteCurrentPostFeedVideoItem;

    [Space]
    [Header("Edit Feed Screen Reference")]
    public GameObject editFeedScreen;
    public AdvancedInputField editFeedDescriptionInputField;
    public Image editFeedCurrentFeedImage;
    public GameObject editFeedCurrentVideoDisplay;

    [Space]
    [Header("Delete Feed Confirmation Screen")]
    public GameObject deleteFeedConfirmationScreen;

    [Space]
    [Header("Feed Comment Screen Reference")]
    public GameObject commentPanel;
    public AdvancedInputField commentInputFieldAdvanced;
    public Text commentFitertextDropdown;
    public ScrollRect commentScrollPosition;

    public GameObject commentContentPanel;
    public GameObject commentListItemPrefab;
    public Text CommentCount;

    [Space]
    public string attechmentArraystr;

    [Space]
    [Header("FadeInOut Screen Reference")]
    public GameObject fadeInOutScreen;
    public Color fadeInOutColor;

    [Space]
    [Header("Profile Follower and Following list Reference")]
    public GameObject profileFollowerFollowingListScreen;
    public Transform profileFollowerListContainer;
    public Transform profileFollowingListContainer;
    public GameObject followerPrefab;
    public GameObject followingPrefab;
    public TextMeshProUGUI profileFFScreenTitleText;
    public Transform profileFFLineSelection;
    public Transform[] profileFFSelectionTab;
    public HorizontalScrollSnap profileFollowerFollowingHorizontalScroll;
    public ScrollRectFasterEx[] profileFFScreenScrollrectFasterEXList;
    public int profileFollowerPaginationPageNo = 1;
    public int profileFollowingPaginationPageNo = 1;
    public bool isProfileFollowerDataLoaded = false;
    public bool isProfileFollowingDataLoaded = false;

    private List<int> profileFollowerLoadedItemIDList = new List<int>();
    private List<int> profileFollowingLoadedItemIDList = new List<int>();
    public List<FollowerItemController> profileFollowerItemControllersList = new List<FollowerItemController>();
    public List<FollowingItemController> profileFollowingItemControllersList = new List<FollowingItemController>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [Space]
    public int callCount = 0;
    private void OnEnable()
    {
        if (callCount > 0)
        {
            StartCoroutine(WaitToStartCallForFeedScene());
            return;
        }
        callCount += 1;
    }

    IEnumerator WaitToStartCallForFeedScene()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.LogError("FeedUIController isLoginFromDifferentId:" + APIManager.Instance.isLoginFromDifferentId);
        if (APIManager.Instance.isLoginFromDifferentId)
        {
            Debug.LogError("FeedUI Controller new user login and calling feed start function");
            ResetAllFeedScreen(false);
            StartMethodCalling();//Start Function Calling.......
        }
    }

    private void Start()
    {
        StartMethodCalling();//Start Function Calling.......
    }

    //this method calling start of the scene.......
    public void StartMethodCalling()
    {
        Debug.LogError("FeedController Start UserToken:" + PlayerPrefs.GetString("LoginToken") + "    :userID:" + PlayerPrefs.GetString("UserName"));
        Debug.LogError("Apimanager UserToken:" + APIManager.Instance.userAuthorizeToken + "    :userID:" + APIManager.Instance.userId);
        Debug.LogError("ApiBaseUrl:" + ConstantsGod.API_BASEURL);
        /*if (!APIManager.Instance.isTestDefaultToken)//compulsory close this check
        {
            //set token screen check.......
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("LoginToken")) || string.IsNullOrEmpty(PlayerPrefs.GetString("UserName")))
            {
                APIManager.Instance.userAuthorizeToken = "";
                setTokenScreen.SetActive(true);
            }
            else
            {
                setTokenScreen.SetActive(false);
            }
        }*/

        if (GlobalVeriableClass.callingScreen == "Feed")
        {
            StartCoroutine(WaitToSceneLoad());
        }

        SetupFollowerAndFeedScreen(false);
    }

    IEnumerator WaitToSceneLoad()
    {
        yield return new WaitForSeconds(0);

        SetupLineSelectionPosition();//move Selection Line

        //APIManager.Instance.RequestGetAllFollowers(1, 10, "FeedStart");//Get All Follower

        for (int i = 0; i < allFeedMessageTextList.Count; i++)
        {
            AllFeedScreenMessageTextActive(true, i, TextLocalization.GetLocaliseTextByKey("please wait"));
        }

        Debug.LogError("FeedUIController Start:" + Application.internetReachability);
        //rik for start of the feed scene load data and default api calling....... 
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (File.Exists(Application.persistentDataPath + "/FeedData.json"))
            {
                APIManager.Instance.LoadJson();
            }

            if (File.Exists(Application.persistentDataPath + "/FeedFollowingData.json"))
            {
                APIManager.Instance.LoadJsonFollowingFeed();
            }
        }
        else
        {
            // Debug.LogError("dfdfsd");
            OnFeedButtonTabBtnClick();
            /*APIManager.Instance.RequestGetAllUsersWithFeeds(1, 10);
            APIManager.Instance.RequestGetFeedsByFollowingUser(1, 10);*/
        }
    }

    //set dummy token data.......
    public void OnClickSetTokenButton()
    {
        /*if (!string.IsNullOrEmpty(tokenInput.text))
        {
            APIManager.Instance.userAuthorizeToken = tokenInput.text;
            APIManager.Instance.userId = int.Parse(userIdInput.text);

            setTokenScreen.SetActive(false);
            if (GlobalVeriableClass.callingScreen == "Feed")
            {
                APIManager.Instance.RequestGetAllFollowers(1, 10, "FeedStart");//Get All Follower

                OnFeedButtonTabBtnClick();
            }
            else if (GlobalVeriableClass.callingScreen == "Profile")
            {
                MyProfileDataManager.Instance.ProfileTabButtonClick();
            }
        }*/
    }

    //this method are used to Message button click.......
    public void OnClickMessageButton()
    {
        Initiate.Fade("SNSMessageModuleScene", Color.black, 1.0f, true);
    }

    public void ShowLoader(bool isActive)
    {
        apiLoaderController.ShowApiLoader(isActive);
    }

    private void Update()
    {
        APiPagination();

        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveUnFollowedUserFromFollowingTab();
        }
    }

    public void OnFeedButtonTabBtnClick()
    {
        Debug.LogError("OnFeedButtonTabBtnClick.......");
        APIManager.Instance.OnFeedAPiCalling();
        feedUiScreen.SetActive(true);
    }

    public void AllFeedScreenMessageTextActive(bool isActive, int index, string message)
    {
        allFeedMessageTextList[index].text = message;
        allFeedMessageTextList[index].gameObject.SetActive(isActive);
    }

    public void OnClickFollowingTabBtnClick()
    {
        RemoveUnFollowedUserFromFollowingTab();

        //if (isAnyUserFollow)
        //{
        //isAnyUserFollow = false;
        //APIManager.Instance.RequestGetFeedsByFollowingUser(1, 10);
        //}
    }

    public void OnClickHotAndDiscoverTabBtnClick()
    {
        //if (isAnyUserFollow)
        //{
            //isAnyUserFollow = false;
            //APIManager.Instance.RequestGetAllUsersWithFeeds(1, 5);
        //}
    }

    public void ResetAllFeedScreen(bool isFeedScreen)
    {
        if (isFeedScreen && APIManager.Instance.allUserRootList.Count == 0)
        {
            Debug.LogError("Feed Data Load");
            StartCoroutine(WaitToSceneLoad());
        }

        feedUiScreen.SetActive(isFeedScreen);
        otherPlayerProfileScreen.SetActive(false);
        giftItemScreens.SetActive(false);
        feedVideoScreen.SetActive(false);
        findFriendScreen.SetActive(false);
        createFeedScreen.SetActive(false);

        profileFollowerFollowingListScreen.SetActive(false);
        OtherPlayerProfileData.Instance.backKeyManageList.Clear();

        SNSSettingController.Instance.myAccountScreen.SetActive(false);
        SNSSettingController.Instance.myAccountPersonalInfoScreen.SetActive(false);
        //StartCoroutine(WaitToResetAllFeedScreen(isFeedScreen));        
    }

    /*public IEnumerator WaitToResetAllFeedScreen(bool isFeedScreen)
    {
        yield return new WaitForSeconds(0f);

        if(isFeedScreen && APIManager.Instance.allUserRootList.Count == 0)
        {
            Debug.LogError("Feed Data Load");
            StartCoroutine(WaitToSceneLoad());
        }

        feedUiScreen.SetActive(isFeedScreen);
        otherPlayerProfileScreen.SetActive(false);
        giftItemScreens.SetActive(false);
        feedVideoScreen.SetActive(false);
        findFriendScreen.SetActive(false);
        createFeedScreen.SetActive(false);

        SNSSettingController.Instance.myAccountScreen.SetActive(false);                
    }*/

    public void ClearAllFeedDataAfterLogOut()
    {
        APIManager.Instance.ClearAllFeedDataForLogout();

        APIController.Instance.feedFollowingIdList.Clear();
        APIController.Instance.feedForYouIdList.Clear();
        APIController.Instance.feedHotIdList.Clear();
        APIController.Instance.feedHotIdList.Clear();
        MyProfileDataManager.Instance.myProfileData = new GetUserDetailData();
        MyProfileDataManager.Instance.allMyFeedImageRootDataList.Clear();
        MyProfileDataManager.Instance.allMyFeedVideoRootDataList.Clear();
        OtherPlayerProfileData.Instance.allMyFeedImageRootDataList.Clear();
        OtherPlayerProfileData.Instance.allMyFeedVideoRootDataList.Clear();
        OtherPlayerProfileData.Instance.backKeyManageList.Clear();
        foreach (Transform item in followingFeedTabContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in followingFeedTabLeftContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in followingFeedTabRightContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in forYouFeedTabContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in hotTabContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in videofeedParent)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in MyProfileDataManager.Instance.allPhotoContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in MyProfileDataManager.Instance.allMovieContainer)
        {
            Destroy(item.gameObject);
        }
    }

    #region fade In Out screen methods.......
    public void FadeInOutScreenShow()
    {
        fadeInOutScreen.GetComponent<Image>().color = fadeInOutColor;
        fadeInOutScreen.SetActive(true);

        fadeInOutScreen.GetComponent<Image>().DOFade(1.0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
           FadeOutStart()
        );
    }

    Coroutine waitToFadeOutCo;
    void FadeOutStart()
    {
        if (waitToFadeOutCo != null)
        {
            StopCoroutine(waitToFadeOutCo);
        }
        waitToFadeOutCo = StartCoroutine(WaitToFadeOut());
    }

    IEnumerator WaitToFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        fadeInOutScreen.GetComponent<Image>().DOFade(0.0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
                fadeInOutScreen.SetActive(false)
        );
    }
    #endregion

    #region Set All Follower And Initiate.......


    #endregion

    #region Setup position and size Feed and Top Follower Panel.......
    public void SetupFollowerAndFeedScreen(bool isStoryAvailable)
    {
        if (isStoryAvailable)
        {
            if (TopPanelMainStoryObj.activeSelf)
            {
                return;
            }
            TopPanelMainStoryObj.SetActive(true);

            TopPanelMainObj.GetComponent<RectTransform>().sizeDelta = new Vector2(TopPanelMainObj.GetComponent<RectTransform>().sizeDelta.x, TopPanelMainObj.GetComponent<RectTransform>().sizeDelta.y + TopPanelMainStoryObj.GetComponent<RectTransform>().sizeDelta.y);
            TopPanelMainObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (float)(TopPanelMainObj.GetComponent<RectTransform>().anchoredPosition.y - (TopPanelMainStoryObj.GetComponent<RectTransform>().sizeDelta.y / 2)));
            for (int i = 0; i < allFeedPanel.Length; i++)
            {
                allFeedPanel[i].GetComponent<RectTransform>().offsetMax = new Vector2(0, allFeedPanel[i].GetComponent<RectTransform>().offsetMax.y - TopPanelMainStoryObj.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
        else
        {
            if (!TopPanelMainStoryObj.activeSelf)
            {
                return;
            }
            TopPanelMainStoryObj.SetActive(false);

            TopPanelMainObj.GetComponent<RectTransform>().sizeDelta = new Vector2(TopPanelMainObj.GetComponent<RectTransform>().sizeDelta.x, TopPanelMainObj.GetComponent<RectTransform>().sizeDelta.y - TopPanelMainStoryObj.GetComponent<RectTransform>().sizeDelta.y);
            TopPanelMainObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (float)(TopPanelMainObj.GetComponent<RectTransform>().anchoredPosition.y + (TopPanelMainStoryObj.GetComponent<RectTransform>().sizeDelta.y / 2)));

            for (int i = 0; i < allFeedPanel.Length; i++)
            {
                allFeedPanel[i].GetComponent<RectTransform>().offsetMax = new Vector2(0, allFeedPanel[i].GetComponent<RectTransform>().offsetMax.y + TopPanelMainStoryObj.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
    }
    #endregion

    public void OnBackToMainXanaBtnClick()
    {
        Initiate.Fade("Main", Color.black, 1.0f);
    }

    public void OnClickCheckOtherPlayerProfile()
    {
        otherPlayerProfileScreen.SetActive(true);

        if (OtherPlayerProfileData.Instance.backKeyManageList.Count > 0)
        {
            switch (OtherPlayerProfileData.Instance.backKeyManageList[OtherPlayerProfileData.Instance.backKeyManageList.Count - 1])
            {
                case "FollowerFollowingListScreen":
                    MyProfileDataManager.Instance.myProfileScreen.SetActive(false);
                    profileFollowerFollowingListScreen.SetActive(false);
                    footerCan.GetComponent<BottomTabManager>().SetDefaultButtonSelection(3);
                    break;
                case "HotTabScreen":
                    Debug.LogError("Comes from Hot or Discover tab full feed screen");
                    //disable feed full screen after click on profile button and open other user profile.......
                    OnClickVideoItemBackButton();
                    break;
                case "FollowingTabScreen":
                    Debug.LogError("Comes from Following tab full feed screen");
                    //disable feed full screen after click on profile button and open other user profile.......
                    OnClickVideoItemBackButton();
                    break;
                default:
                    feedUiScreen.SetActive(false);
                    break;
            }
        }
        else
        {
            feedUiScreen.SetActive(false);
        }
    }

    public void OnClickProfileGiftBOxButton()
    {
        otherPlayerProfileScreen.SetActive(false);
        giftItemScreens.SetActive(true);
    }

    public void OnClickFashionCetegoryItem(int index)
    {
        /* for (int i = 0; i < fashionCetegoryContent.childCount; i++)
         {
             if (i == index)
             {
                 fashionCetegoryContent.GetChild(i).gameObject.GetComponent<Image>().sprite = selectedSprite;
                 fashionCetegoryContent.GetChild(i).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
                 fashionCetegoryContent.GetChild(i).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedColor;
             }
             else
             {
                 fashionCetegoryContent.GetChild(i).gameObject.GetComponent<Image>().sprite = unSelectedSprite;
                 fashionCetegoryContent.GetChild(i).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                 fashionCetegoryContent.GetChild(i).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = unSelectedColor;
             }
         }*/
    }

    public void Swipe(Vector2 value)
    {
        // Debug.LogError(" value " + value.y);
        /* if (value.x > 500)
         {
             if (feedUiScreen.activeSelf)
             {
                 allScrollSnapRect[0].PreviousScreen();
             }
             else if (otherPlayerProgileScreen.activeSelf)
             {
                 allScrollSnapRect[1].PreviousScreen();
             }
             else if (giftItemScreens.activeSelf)
             {
                 allScrollSnapRect[2].PreviousScreen();
             }
         }
         else if (value.x < -500)
         {
             if (feedUiScreen.activeSelf)
             {
                 allScrollSnapRect[0].NextScreen();
             }
             else if (otherPlayerProgileScreen.activeSelf)
             {
                 allScrollSnapRect[1].NextScreen();
             }
             else if (giftItemScreens.activeSelf)
             {
                 allScrollSnapRect[2].NextScreen();
             }
         }*/
    }

    public void OnSetSelectionLine()
    {
        if (feedUiScreen.activeSelf)
        {
            SetupLineSelectionPosition();//move Selection Line

            //feedUiSelectionLine.transform.DOMove(new Vector3((feedUiSelectionTab[feedUiHorizontalSnap.CurrentPage].position.x), feedUiSelectionLine.transform.position.y, feedUiSelectionLine.transform.position.z), .2f);
            /*for (int i = 0; i < feedUiTabTitleText.Length; i++)
            {
                if (i == feedUiHorizontalSnap.CurrentPage)
                {
                    feedUiTabTitleText[i].color = selectedColor;
                    StartCoroutine(ActiveFeedUi(i));
                }
                else
                {
                    feedUiTabTitleText[i].color = unSelectedColor;
                }
            }*/
            //  if (feedUiHorizontalSnap.CurrentPage != 0)
            // {
            isChangeMainScrollRect = true;
            feedUiScrollRectFasterEx = allFeedScrollRectFasterEx[feedUiHorizontalSnap.CurrentPage];

            StartCoroutine(WaitChangeScrollRectFasterOnMain());
            // }
        }
    }

    void SetupLineSelectionPosition()
    {
        float xPos;
        if (feedUiHorizontalSnap.CurrentPage == 0)
        {
            xPos = feedUiSelectionTab[feedUiHorizontalSnap.CurrentPage].position.x - 25f;
        }
        else if (feedUiHorizontalSnap.CurrentPage == 1)
        {
            xPos = feedUiSelectionTab[feedUiHorizontalSnap.CurrentPage].position.x - 10f;
        }
        else if (feedUiHorizontalSnap.CurrentPage == 2)
        {
            xPos = feedUiSelectionTab[feedUiHorizontalSnap.CurrentPage].position.x + 11f;
        }
        else
        {
            xPos = feedUiSelectionTab[feedUiHorizontalSnap.CurrentPage].position.x;
        }

        //feedUiSelectionLine.transform.DOMove(new Vector3(xPos, feedUiSelectionLine.transform.position.y, feedUiSelectionLine.transform.position.z), .2f);
        feedUiSelectionLine.transform.DOMoveX(xPos, .2f);

        //SetColor Feed Tab Title Text
        for (int i = 0; i < feedUiTabTitleText.Length; i++)
        {
            if (i == feedUiHorizontalSnap.CurrentPage)
            {
                feedUiTabTitleText[i].color = selectedColor;
            }
            else
            {
                feedUiTabTitleText[i].color = unSelectedColor;
            }
            /*if (i == 1)//new cmnt
            {
                StartCoroutine(SetContentOnFollowingItemScreen());
            }*/
        }
    }

    public IEnumerator SetContentOnFollowingItemScreen()
    {
        yield return new WaitForSeconds(0.01f);
        followingFeedMainContainer.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.05f);
        followingFeedMainContainer.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    public void SetUpFeedTabDefaultTop()
    {
        feedUiScrollRectFasterEx.verticalNormalizedPosition = 1;
    }

    public bool isChangeMainScrollRect = false;
    IEnumerator WaitChangeScrollRectFasterOnMain()
    {
        yield return new WaitForSeconds(2f);
        isChangeMainScrollRect = false;
    }

    public IEnumerator ActiveFeedUi(int index, int callingIndex)
    {
        if (feedUiScreen.activeSelf)
        {
            for (int i = 0; i < allFeedPanel.Length; i++)
            {
                if (i == index)
                {
                    allFeedPanel[i].transform.gameObject.SetActive(true);
                    if (callingIndex == 1)//set default scroll top.......
                    {
                        SetUpFeedTabDefaultTop();
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < allFeedPanel.Length; i++)
            {
                if (i != feedUiHorizontalSnap.CurrentPage)
                {
                    allFeedPanel[i].transform.gameObject.SetActive(false);
                }
            }

            if (index == 1)
            {
                StartCoroutine(SetContentOnFollowingItemScreen());
            }
        }
    }

    public void CloseAllFeed(bool isActive)
    {
        if (feedUiScreen.activeSelf)
        {
            for (int i = 0; i < allFeedPanel.Length; i++)
            {
                allFeedPanel[i].transform.gameObject.SetActive(isActive);
            }
        }
    }

    public void APiPagination()
    {
        //Debug.LogError("y pos:" + feedUiScrollRectFasterEx.verticalEndPos);

        if (isChangeMainScrollRect)
        {
            return;
        }

        //Debug.LogError("verticalNormalizedPosition : " + feedUiScrollRectFasterEx.verticalNormalizedPosition + "    :verticalEndPos:" + feedUiScrollRectFasterEx.verticalEndPos + "    :isDataLoad:" + isDataLoad);
        //if (feedUiScrollRectFasterEx.verticalEndPos <= 1 && isDataLoad)
        if (feedUiScrollRectFasterEx.verticalNormalizedPosition <= 0.01f && isDataLoad)
        {
            // Debug.LogError("scrollRect pos :" + feedUiScrollRectFasterEx.verticalNormalizedPosition);
            if (feedUiHorizontalSnap.CurrentPage == 1)
            {
                //Debug.LogError("Feed Following scrollRect pos :" + feedUiScrollRectFasterEx.verticalNormalizedPosition + " rows count:"+ APIManager.Instance.followingUserRoot.Data.Rows.Count);
                //if (APIManager.Instance.followingUserRoot.Data.Rows.Count > 0 && followingFeedImageLoadedCount >= (followingFeedInitiateTotalCount - 1))
                if (APIManager.Instance.followingUserRoot.Data.Rows.Count > 0 && followingFeedInitiateTotalCount < 2)
                {
                    isDataLoad = false;
                    Debug.LogError("isDataLoad False");
                    APIManager.Instance.RequestGetFeedsByFollowingUser((followingUserCurrentpage + 1), 10);
                }
            }
            else
            {
                //Debug.LogError("Feed scrollRect pos :" + feedUiScrollRectFasterEx.verticalNormalizedPosition + " rows count:"+ APIManager.Instance.root.data.rows.Count + " :current screen:" + feedUiHorizontalSnap.CurrentPage);
                if (APIManager.Instance.root.data.rows.Count > 0)
                {
                    bool isCallAPi = false;
                    if (feedUiHorizontalSnap.CurrentPage == 0 && hotFeedInitiateTotalCount < 2)
                    {
                        isCallAPi = true;
                    }
                    else if (feedUiHorizontalSnap.CurrentPage == 2 && hotForYouFeedInitiateTotalCount < 2)
                    {
                        isCallAPi = true;
                    }
                    //Debug.LogError("isCalling:" + isCallAPi);
                    if (isCallAPi)
                    {
                        Debug.LogError("isDataLoad False APiPagination currentPage :" + allFeedCurrentpage);
                        isDataLoad = false;
                        APIManager.Instance.RequestGetAllUsersWithFeeds((allFeedCurrentpage + 1), 5);
                    }
                }
            }
            //Debug.LogError("isDataLoad False");
            //isDataLoad = false;
        }
    }

    public string GetConvertedTimeString(DateTime dateTime)
    {
        DateTime timeUtc = dateTime;
        DateTime today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, TimeZoneInfo.Local);

        TimeSpan timeDiff = (DateTime.Now - today);

        //Debug.LogError("minuts : " + timeDiff.TotalMinutes + "  :days: " + timeDiff.Days + "    :Date:"+dateTime);
        string timestr = "";

        if (timeDiff.TotalMinutes < 1)
        {
            timestr = TextLocalization.GetLocaliseTextByKey("Just Now");
        }
        else if (timeDiff.TotalMinutes > 1 && timeDiff.TotalMinutes <= 60)
        {
            timestr = Mathf.Round((float)(timeDiff.TotalMinutes)) + " " + TextLocalization.GetLocaliseTextByKey("minutes ago");
        }
        else if (timeDiff.TotalMinutes > 60 && timeDiff.TotalMinutes <= 1440)
        {
            timestr = Mathf.Round((float)(timeDiff.TotalMinutes / 60)) + " " + TextLocalization.GetLocaliseTextByKey("hours ago");
        }
        else if (timeDiff.TotalDays > 1 && timeDiff.TotalDays <= 30)
        {
            timestr = timeDiff.Days.ToString() + " " + TextLocalization.GetLocaliseTextByKey("days ago");
        }
        else if (timeDiff.TotalDays > 30 && timeDiff.TotalDays <= 365)
        {
            timestr = Mathf.Round((float)(timeDiff.Days / 30)) + " " + TextLocalization.GetLocaliseTextByKey("months ago");
        }
        else
        {
            timestr = Mathf.Round((float)(timeDiff.Days / 365)) + " " + TextLocalization.GetLocaliseTextByKey("years ago");
        }
        return timestr;
    }

    public string GetConvertedTimeStringSpecifyKind(DateTime dateTime)
    {
        DateTime timeUtc = dateTime;
        //DateTime today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, TimeZoneInfo.Local);
        DateTime today = DateTime.SpecifyKind(timeUtc, DateTimeKind.Local); ;

        TimeSpan timeDiff = (DateTime.Now - today);

        //Debug.LogError("minuts : " + timeDiff.TotalMinutes + "  :days: " + timeDiff.Days + "    :Date:"+dateTime);
        string timestr = "";

        if (timeDiff.TotalMinutes < 1)
        {
            timestr = TextLocalization.GetLocaliseTextByKey("Just Now");
        }
        else if (timeDiff.TotalMinutes > 1 && timeDiff.TotalMinutes <= 60)
        {
            timestr = Mathf.Round((float)(timeDiff.TotalMinutes)) + " " + TextLocalization.GetLocaliseTextByKey("minutes ago");
        }
        else if (timeDiff.TotalMinutes > 60 && timeDiff.TotalMinutes <= 1440)
        {
            timestr = Mathf.Round((float)(timeDiff.TotalMinutes / 60)) + " " + TextLocalization.GetLocaliseTextByKey("hours ago");
        }
        else if (timeDiff.TotalDays > 1 && timeDiff.TotalDays <= 30)
        {
            timestr = timeDiff.Days.ToString() + " " + TextLocalization.GetLocaliseTextByKey("days ago");
        }
        else if (timeDiff.TotalDays > 30 && timeDiff.TotalDays <= 365)
        {
            timestr = Mathf.Round((float)(timeDiff.Days / 30)) + " " + TextLocalization.GetLocaliseTextByKey("months ago");
        }
        else
        {
            timestr = Mathf.Round((float)(timeDiff.Days / 365)) + " " + TextLocalization.GetLocaliseTextByKey("years ago");
        }
        return timestr;
    }

    public void OnClickVideoItemBackButton()
    {
        feedVideoScreen.SetActive(false);
        switch (feedFullViewScreenCallingFrom)
        {
            case "MyProfile":
                Debug.LogError("Full Video Screen back calling from MyProfile");
                MyProfileDataManager.Instance.ResetMainScrollDefaultTopPos();
                MyProfileDataManager.Instance.SetupEmptyMsgForPhotoTab(false);//check for empty message.......
                break;
            case "OtherProfile":
                OtherPlayerProfileData.Instance.ResetMainScrollDefaultTopPos();
                Debug.LogError("Full Video Screen back calling from OtherProfile");
                break;
            default:
                feedUiScreen.SetActive(true);
                break;
        }
        feedFullViewScreenCallingFrom = "";

        foreach (Transform item in videofeedParent)
        {
            Destroy(item.gameObject);
        }
    }

    public void OnClickGiftScreenBackButton()
    {
        giftItemScreens.SetActive(false);
        otherPlayerProfileScreen.SetActive(true);
    }

    //this method is used to Find Friend Button click.......
    public void OnClickSearchUserButton()
    {
        //findFriendInputField.text = "";
        findFriendInputFieldAdvanced.Text = "";
        findFriendScreen.SetActive(true);
    }

    #region find User references
    //this method is used to On find value inputfield value change.......
    public void ObValueChangeFindFriend()
    {
        //if (!string.IsNullOrEmpty(findFriendInputField.text))
        if (!string.IsNullOrEmpty(findFriendInputFieldAdvanced.Text))
        {
            //APIManager.Instance.RequestGetSearchUser(findFriendInputField.text);
            APIManager.Instance.RequestGetSearchUser(findFriendInputFieldAdvanced.Text);
        }
        else
        {
            //if user typed character clear then clear all search user list.
            foreach (Transform item in findFriendContainer)
            {
                Destroy(item.gameObject);
            }
        }
    }

    //this method is used to back button click find friend screen.......
    public void OnClickBackFindFriendButton()
    {
        RemoveUnFollowedUserFromFollowingTab();

        //findFriendInputField.text = "";
        findFriendInputFieldAdvanced.Text = "";
        findFriendScreen.SetActive(false);
    }
    #endregion

    #region Create Feed
    public void CreateFeedAPICall(string url)
    {
        switch (imageOrVideo)
        {
            case "Image":
                //string s1 = createFeedTitle.text;
                //string s1 = createFeedTitleAdvanced.RichText;
                string s1 = APIManager.Instance.userName;
                //string s2 = createFeedDescription.text;
                string s2 = createFeedDescriptionAdvanced.RichText;

                if (string.IsNullOrEmpty(s1))
                {
                    s1 = "@new";
                }
                else
                {
                    s1 = "@" + s1;
                }

                if (string.IsNullOrEmpty(s2))
                {
                    s2 = "  ";
                }
                APIManager.Instance.RequestCreateFeed(APIManager.EncodedString(s1), APIManager.EncodedString(s2), url, "", "true", "", "MyProfileCreateFeed");
                break;
            case "Video":
                //string s11 = createFeedTitle.text;
                //string s11 = createFeedTitleAdvanced.RichText;
                string s11 = APIManager.Instance.userName;
                //string s22 = createFeedDescription.text;
                string s22 = createFeedDescriptionAdvanced.RichText;

                if (string.IsNullOrEmpty(s11))
                {
                    s11 = "@new";
                }
                else
                {
                    s11 = "@" + s11;
                }

                if (string.IsNullOrEmpty(s22))
                {
                    s22 = "  ";
                }
                APIManager.Instance.RequestCreateFeed(APIManager.EncodedString(s11), APIManager.EncodedString(s22), "", url, "true", "", "MyProfileCreateFeed");
                break;
            default:
                break;
        }
    }

    static long GetFileSize(string FilePath)
    {
        if (File.Exists(FilePath))
        {
            return new FileInfo(FilePath).Length;
        }
        return 0;
    }

    public bool lastPostCreatedImageDownload = false;
    public string imageOrVideo = "";
    public string createFeedLastPickFilePath;
    public string createFeedLastPickFileName;
    public void OnClickCreateFeedPickImageOrVideo()
    {
        ResetAndClearCreateFeedData();

        /*createFeedTitle.text = "";
        createFeedDescription.text = "";
        createFeedImage.sprite = null;
        imageOrVideo = "";

        createFeedLastPickFilePath = "";
        createFeedLastPickFileName = "";*/

        createFeedImage.gameObject.SetActive(false);
        createFeedVideoObj.SetActive(false);
        createFeedMediaPlayer.gameObject.SetActive(false);

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                switch (NativeGallery.GetMediaTypeOfFile(path))
                {
                    case NativeGallery.MediaType.Image:
                        Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
                        if (texture == null)
                        {
                            Debug.Log("Couldn't load texture from " + path);
                            return;
                        }
                        createFeedImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                        imageOrVideo = "Image";
                        createFeedImage.gameObject.SetActive(true);
                        Debug.Log("Picked image");
                        break;
                    case NativeGallery.MediaType.Video:
                        createFeedVideoObj.SetActive(true);
                        createFeedMediaPlayer.gameObject.SetActive(true);
                        createFeedMediaPlayer.OpenMedia(new MediaPath(path, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                        createFeedMediaPlayer.Play();
                        Debug.Log("Picked video");
                        imageOrVideo = "Video";
                        break;
                    default:
                        Debug.Log("Probably picked something else");
                        break;
                }

                createFeedLastPickFilePath = path;

                string[] pathArry = path.Split('/');

                //string fileName = pathArry[pathArry.Length - 1];
                string fileName = Path.GetFileName(path);
                createFeedLastPickFileName = (Time.time + fileName);
                Debug.LogError("createFeedLastPickFileName  :" + createFeedLastPickFileName + " :fileName   :" + fileName);

                createFeedScreen.SetActive(true);
            }
        });
        Debug.Log("Permission result: " + permission);
        return;

        /*if (NativeGallery.CanSelectMultipleMediaTypesFromGallery())
        {
            NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
            {
                Debug.LogError("Media path:" + path);

                string fileExtention = Path.GetExtension(path);
                Debug.LogError("File extention:" + fileExtention);              

                if (path != null)
                {
                    // Determine if user has picked an image, video or neither of these
                    switch (NativeGallery.GetMediaTypeOfFile(path))
                    {
                        case NativeGallery.MediaType.Image:
                            Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
                            if (texture == null)
                            {
                                Debug.Log("Couldn't load texture from " + path);
                                return;
                            }
                            createFeedImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                            imageOrVideo = "Image";
                            createFeedImage.gameObject.SetActive(true);
                            Debug.Log("Picked image");
                            break;
                        case NativeGallery.MediaType.Video:

                            long fileSizeibBytes = GetFileSize(path);
                            long fileSizeibMbs = fileSizeibBytes / (1024 * 1024);
                            Debug.LogError("File size:" + fileSizeibMbs);

                            if(fileSizeibMbs > 15)//check video file size and upload upto 15MG only.......
                            {
                                SNSWarningMessageManager.Instance.ShowWarningMessage("Please upload video upto 15MB");
                                return;
                            }

                            if(fileExtention != ".mp4")//check video format and only allow mp4 file.......
                            {
                                SNSWarningMessageManager.Instance.ShowWarningMessage("video format is not supported. please select MP4 file");
                                return;
                            }

                            createFeedVideoObj.SetActive(true);
                            createFeedMediaPlayer.gameObject.SetActive(true);
                            createFeedMediaPlayer.OpenMedia(new MediaPath(path, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                            createFeedMediaPlayer.Play();
                            Debug.Log("Picked video");
                            imageOrVideo = "Video";
                            break;
                        default:
                            Debug.Log("Probably picked something else");
                            break;
                    }

                    createFeedLastPickFilePath = path;

                    string[] pathArry = path.Split('/');

                    //string fileName = pathArry[pathArry.Length - 1];
                    string fileName = Path.GetFileName(path);
                    createFeedLastPickFileName = (Time.time + fileName);
                    Debug.LogError("createFeedLastPickFileName  :" + createFeedLastPickFileName + " :fileName   :" + fileName);

                    createFeedScreen.SetActive(true);

                    //AWSHandler.Instance.PostObjectFeed(path, (Time.time + fileName), "CreateFeed");
                }
            }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

            Debug.Log("Permission result: " + permission);
        }*/
    }

    //this method is used to post Feed 
    public void OnClickCreateFeedPostBtn()
    {
        print("post btn");
        if (PremiumUsersDetails.Instance != null && !PremiumUsersDetails.Instance.CheckSpecificItem("post button"))
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }

        ShowLoader(true);//active loader

        AWSHandler.Instance.PostObjectFeed(createFeedLastPickFilePath, createFeedLastPickFileName, "CreateFeed");
    }

    public void OnClickCreateFeedBackBtn(bool isDataNotReset)
    {
        createFeedScreen.SetActive(false);

        if (!isDataNotReset)
        {
            ResetAndClearCreateFeedData();
        }

        /*createFeedTitle.text = "";
        createFeedDescription.text = "";
        createFeedImage.sprite = null;
        imageOrVideo = "";

        createFeedLastPickFilePath = "";
        createFeedLastPickFileName = "";*/

        createFeedImage.gameObject.SetActive(false);
        createFeedVideoObj.SetActive(false);
        createFeedMediaPlayer.CloseMedia();
        createFeedMediaPlayer.gameObject.SetActive(false);
    }

    public void ResetAndClearCreateFeedData()
    {
        lastPostCreatedImageDownload = false;

        createFeedTitle.text = "";
        createFeedDescription.text = "";
        createFeedTitleAdvanced.Text = "";
        createFeedDescriptionAdvanced.Text = "";
        /*if (createFeedImage.sprite != null)
        {
            Destroy(createFeedImage.sprite);
        }*/
        createFeedImage.sprite = null;
        imageOrVideo = "";

        createFeedLastPickFilePath = "";
        createFeedLastPickFileName = "";

        Resources.UnloadUnusedAssets();
        Caching.ClearCache();
        GC.Collect();
    }
    #endregion

    #region Following Tab Feed Remove and refresh after unfollow user
    //this method is used to check add and remove from unfollowed list.......
    public void FollowingAddAndRemoveUnFollowedUser(int userID, bool isComesFromUnFollowed)
    {
        if (isComesFromUnFollowed)
        {
            if (!unFollowedUserListForFollowingTab.Contains(userID))
            {
                unFollowedUserListForFollowingTab.Add(userID);
            }
        }
        else
        {
            if (unFollowedUserListForFollowingTab.Contains(userID))
            {
                unFollowedUserListForFollowingTab.Remove(userID);
            }
        }
    }

    //this method is used to Remove feed from following tab.......
    public void RemoveUnFollowedUserFromFollowingTab()
    {
        Debug.LogError("RemoveUnFollowedUserFromFollowingTab.......:" + unFollowedUserListForFollowingTab.Count);
        if (unFollowedUserListForFollowingTab.Count > 0)
        {
            APIManager.Instance.FeedFollowingSaveAndUpdateJson(unFollowedUserListForFollowingTab);
        }
    }
    #endregion

    #region Profile Follower and Following list Screen Methods
    int tempFollowFollowingScreenOpenCount = 0;
    //this method is used to profile follower button click.......
    public void ProfileFollowerFollowingScreenSetup(int Tabindex, string userName)
    {
        profileFFScreenTitleText.text = userName;
        profileFollowerFollowingListScreen.SetActive(true);
        if (tempFollowFollowingScreenOpenCount == 0)
        {
            StartCoroutine(WaitToProfileFollowerFollowingHorizontalScroll(Tabindex));
        }
        else
        {
            profileFollowerFollowingHorizontalScroll.GoToScreen(Tabindex);
        }
        ProfileFFLineSelectionSetup(Tabindex);

        for (int i = 0; i < profileFFScreenScrollrectFasterEXList.Length; i++)
        {
            profileFFScreenScrollrectFasterEXList[i].verticalNormalizedPosition = 1;
        }
        tempFollowFollowingScreenOpenCount ++; 
        //MyProfileDataManager.Instance.MyProfileSceenShow(false);
    }

    IEnumerator WaitToProfileFollowerFollowingHorizontalScroll(int Tabindex)
    {
        yield return new WaitForSeconds(0.02f);
        profileFollowerFollowingHorizontalScroll.GoToScreen(Tabindex);
    }

    public void ProfileFFSelectionOnValueChange()
    {
        //float xPos = profileFFSelectionTab[profileFollowerFollowingHorizontalScroll.CurrentPage].position.x;
        //profileFFLineSelection.transform.DOMoveX(xPos, .01f);
        ProfileFFLineSelectionSetup(profileFollowerFollowingHorizontalScroll.CurrentPage);
    }

    public void ProfileFFLineSelectionSetup(int index)
    {
        float xPos = profileFFSelectionTab[index].position.x;
        profileFFLineSelection.transform.DOMoveX(xPos, .2f);
    }

    public void OnClickProfileFollowerFollowingBackButton()
    {
        RemoveUnFollowedUserFromFollowingTab();

        profileFollowerFollowingListScreen.SetActive(false);

        //MyProfileDataManager.Instance.MyProfileSceenShow(true);
    }

    public void ProfileGetAllFollower(int pageNum)
    {
        //Debug.LogError("ProfileGetAllFollower:" + APIManager.Instance.profileAllFollowerRoot.data.rows.Count + "    :pageNum:" + pageNum);
        for (int i = 0; i < APIManager.Instance.profileAllFollowerRoot.data.rows.Count; i++)
        {
            if (!profileFollowerLoadedItemIDList.Contains(APIManager.Instance.profileAllFollowerRoot.data.rows[i].follower.id))
            {
                GameObject followerObject = Instantiate(followerPrefab, profileFollowerListContainer);
                followerObject.GetComponent<FollowerItemController>().SetupData(APIManager.Instance.profileAllFollowerRoot.data.rows[i]);
                profileFollowerItemControllersList.Add(followerObject.GetComponent<FollowerItemController>());
                profileFollowerLoadedItemIDList.Add(APIManager.Instance.profileAllFollowerRoot.data.rows[i].follower.id);
            }
        }

        if (waitToProfileFollowerDataLoadCo != null)
        {
            StopCoroutine(waitToProfileFollowerDataLoadCo);
        }
        waitToProfileFollowerDataLoadCo = StartCoroutine(WaitToProfileFollowerDataLoad(pageNum));
    }

    Coroutine waitToProfileFollowerDataLoadCo;
    IEnumerator WaitToProfileFollowerDataLoad(int pageNum)
    {
        yield return new WaitForSeconds(0.5f);
        isProfileFollowerDataLoaded = true;
        if (pageNum > 1 && APIManager.Instance.profileAllFollowerRoot.data.rows.Count > 0)
        {
            profileFollowerPaginationPageNo += 1;
        }
    }

    public void ProfileGetAllFollowing(int pageNum)
    {
        //Debug.LogError("ProfileGetAllFollowing:" + APIManager.Instance.profileAllFollowingRoot.data.rows.Count + "    :pageNum:" + pageNum);
        for (int i = 0; i < APIManager.Instance.profileAllFollowingRoot.data.rows.Count; i++)
        {
            if (!profileFollowingLoadedItemIDList.Contains(APIManager.Instance.profileAllFollowingRoot.data.rows[i].following.id))
            {
                GameObject followingObject = Instantiate(followingPrefab, profileFollowingListContainer);
                followingObject.GetComponent<FollowingItemController>().SetupData(APIManager.Instance.profileAllFollowingRoot.data.rows[i]);
                profileFollowingItemControllersList.Add(followingObject.GetComponent<FollowingItemController>());
                profileFollowingLoadedItemIDList.Add(APIManager.Instance.profileAllFollowingRoot.data.rows[i].following.id);
            }
        }

        if (waitToProfileFollowingDataLoadCo != null)
        {
            StopCoroutine(waitToProfileFollowingDataLoadCo);
        }
        waitToProfileFollowingDataLoadCo = StartCoroutine(WaitToProfileFollowingDataLoad(pageNum));
    }

    Coroutine waitToProfileFollowingDataLoadCo;
    IEnumerator WaitToProfileFollowingDataLoad(int pageNum)
    {
        yield return new WaitForSeconds(0.5f);
        isProfileFollowingDataLoaded = true;
        if (pageNum > 1 && APIManager.Instance.profileAllFollowingRoot.data.rows.Count > 0)
        {
            profileFollowingPaginationPageNo += 1;
        }
    }

    public void ProfileFollowerFollowingListClear()
    {
        profileFollowerPaginationPageNo = 1;
        profileFollowingPaginationPageNo = 1;
        isProfileFollowerDataLoaded = false;
        isProfileFollowingDataLoaded = false;

        profileFollowerItemControllersList.Clear();
        profileFollowerLoadedItemIDList.Clear();
        foreach (Transform item in profileFollowerListContainer)
        {
            Destroy(item.gameObject);
        }

        profileFollowingItemControllersList.Clear();
        profileFollowingLoadedItemIDList.Clear();
        foreach (Transform item in profileFollowingListContainer)
        {
            Destroy(item.gameObject);
        }
    }

    public void ProfileFollowerPaginationAPICall()
    {
        //Debug.LogError("ProfileFollowerFollowingPagination : " + profileFFScreenScrollrectFasterEXList[0].verticalNormalizedPosition + " :CurrentPage:" + profileFollowerFollowingHorizontalScroll.CurrentPage);
        if (profileFFScreenScrollrectFasterEXList[0].verticalNormalizedPosition <= 0.01f && isProfileFollowerDataLoaded)
        {
            if (APIManager.Instance.profileAllFollowerRoot.data.rows.Count > 0)
            {
                //Debug.LogError("ProfileFollowerFollowingPagination follower currentPage:" + profileFollowerPaginationPageNo);
                isProfileFollowerDataLoaded = false;
                APIManager.Instance.RequestGetAllFollowersFromProfile(MyProfileDataManager.Instance.myProfileData.id.ToString(), (profileFollowerPaginationPageNo + 1), 50);
            }
        }
    }

    public void ProfileFollowingPaginationAPICall()
    {
        //Debug.LogError("ProfileFollowerFollowingPagination : " + profileFFScreenScrollrectFasterEXList[1].verticalNormalizedPosition + " :CurrentPage:" + profileFollowerFollowingHorizontalScroll.CurrentPage);
        if (profileFFScreenScrollrectFasterEXList[1].verticalNormalizedPosition <= 0.01f && isProfileFollowingDataLoaded)
        {
            if (APIManager.Instance.profileAllFollowingRoot.data.rows.Count > 0)
            {
                //Debug.LogError("ProfileFollowerFollowingPagination following currentPage:" + profileFollowingPaginationPageNo);
                isProfileFollowingDataLoaded = false;
                APIManager.Instance.RequestGetAllFollowingFromProfile(MyProfileDataManager.Instance.myProfileData.id.ToString(), (profileFollowingPaginationPageNo + 1), 50);
            }
        }
    }
    #endregion

    #region Edit Delete Feed Methods.......

    public void SetupEditDeleteFeedScreen()
    {
        if (!string.IsNullOrEmpty(feedEditOrDeleteData.feedImage))
        {
            GetImageFromAWS(feedEditOrDeleteData.feedImage, editDeleteCurrentFeedImage);//Get image from aws and save/load into asset cache.......

            float diff = editDeleteCurrentFeedImage.sprite.rect.width - editDeleteCurrentFeedImage.sprite.rect.height;
            if (diff < -160)
            {
                editDeleteCurrentFeedImage.GetComponent<AspectRatioFitter>().aspectRatio = 0.1f;
            }
            else
            {
                editDeleteCurrentFeedImage.GetComponent<AspectRatioFitter>().aspectRatio = 2.2f;
            }
        }
        else
        {
            editDeleteCurrentFeedImage.gameObject.SetActive(false);
            editDeleteVideoDisplay.SetActive(true);
            //editDeleteMideaPlayer.gameObject.SetActive(true);
            editDeleteCurrentPostFeedVideoItem.feedMediaPlayer.Pause();
            editDeleteVideoDisplay.GetComponent<DisplayUGUI>().CurrentMediaPlayer = editDeleteCurrentPostFeedVideoItem.feedMediaPlayer;
            //editDeleteMideaPlayer = editDeleteCurrentPostFeedVideoItem.feedMediaPlayer;
        }
        if (GameManager.currentLanguage == "ja")
        {
            editDeleteFeedUserNameText.text = feedEditOrDeleteData.userData.Name + " " + TextLocalization.GetLocaliseTextByKey("Post by");
        }
        else
        {
            editDeleteFeedUserNameText.text = TextLocalization.GetLocaliseTextByKey("Post by") + " " + feedEditOrDeleteData.userData.Name;
        }
        if (feedEditOrDeleteData.UpdatedAt != null)
        {
            DateTime timeUtc = feedEditOrDeleteData.UpdatedAt;
            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, TimeZoneInfo.Local);

            TimeSpan timeDiff = (DateTime.Now - today);
            Debug.LogError("timeDiff:" + timeDiff + " :timeUtc:" + timeUtc.ToString("yyyy/MM/dd tt hh:MM"));

            editDeleteFeedDateTimeText.text = timeUtc.ToString("yyyy/MM/dd tt hh:MM");
        }
    }

    //this method is used show edit delete feed screen.......
    public void OnShowEditDeleteFeedScreen(bool isActive)
    {
        if (isActive)
        {
            SetupEditDeleteFeedScreen();
        }
        editDeleteFeedScreen.SetActive(isActive);
    }

    //this method is used to Edit/Delete Feed Close Popup Button Click.......
    public void OnClickEditDeleteClosePopupButton()
    {
        StartCoroutine(WaitToOnClickEditDeleteClose());
    }
    IEnumerator WaitToOnClickEditDeleteClose()
    {
        yield return new WaitForSeconds(0.25f);
        editDeleteCurrentFeedImage.gameObject.SetActive(true);
        editDeleteVideoDisplay.SetActive(false);
        //editDeleteMideaPlayer.gameObject.SetActive(false);

        editDeleteFeedUserNameText.text = TextLocalization.GetLocaliseTextByKey("Post") + " " + TextLocalization.GetLocaliseTextByKey("by");
        editDeleteFeedUserNameText.text = "";
        AssetCache.Instance.RemoveFromMemory(editDeleteCurrentFeedImage.sprite);
        editDeleteCurrentFeedImage.sprite = null;

        if (!string.IsNullOrEmpty(feedEditOrDeleteData.feedVideo))
        {
            editDeleteCurrentPostFeedVideoItem.feedMediaPlayer.Play();
        }

        //editDeleteMideaPlayer = null;
        Resources.UnloadUnusedAssets();
        Caching.ClearCache();
        GC.Collect();

        if (editDeleteCurrentPostFeedVideoItem != null)
        {
            editDeleteCurrentPostFeedVideoItem = null;
        }
    }

    //this method is used to Edit/Delete Feed Screen Delete button click.......
    public void OnClickEditDeleteFeedDeleteButton()
    {
        deleteFeedConfirmationScreen.SetActive(true);
    }    

    //this method is used to Edit/Delete Feed Screen Edit button click.......
    public void OnClickEditDeleteFeedEditButton()
    {
        editFeedCurrentFeedImage.gameObject.SetActive(true);
        editFeedCurrentVideoDisplay.SetActive(false);

        if (!string.IsNullOrEmpty(feedEditOrDeleteData.feedImage))
        {
            editFeedCurrentFeedImage.sprite = editDeleteCurrentFeedImage.sprite;
            editFeedCurrentFeedImage.GetComponent<AspectRatioFitter>().aspectRatio = editDeleteCurrentFeedImage.GetComponent<AspectRatioFitter>().aspectRatio;
        }
        else
        {
            editFeedCurrentFeedImage.gameObject.SetActive(false);
            editFeedCurrentVideoDisplay.SetActive(true);

            editFeedCurrentVideoDisplay.GetComponent<DisplayUGUI>().CurrentMediaPlayer = editDeleteCurrentPostFeedVideoItem.feedMediaPlayer;
        }

        if (!string.IsNullOrEmpty(feedEditOrDeleteData.feedDescriptions))
        {
            editFeedDescriptionInputField.Text = APIManager.DecodedString(feedEditOrDeleteData.feedDescriptions);
        }
        else
        {
            editFeedDescriptionInputField.Text = "";
        }

        editFeedScreen.SetActive(true);
    }

    //this method is used to Edit/Delete Feed Screen Share button click.......
    public void OnClickEditDeleteFeedShareButton()
    {
        string url = editDeleteCurrentPostFeedVideoItem.shareMediaUrl;

        new NativeShare().AddFile("Scoial Image")
       .SetSubject("Subject goes here").SetText("Xana App!").SetUrl(url)
       .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
       .Share();
    }

    //this method is used to Delete Feed Confirmation Screen Ok button click.......
    public void OnClickDeleteFeedConfirmationOkButton()
    {
        //ShowLoader(true);
        APIManager.Instance.RequestDeleteFeed(feedEditOrDeleteData.feedId.ToString(), "DeleteFeed");
        //deleteFeedConfirmationScreen.SetActive(false);
    }

    //this method is used to Delete Feed Confirmation Screen Cancel button click.......
    public void OnClickDeleteFeedConfirmationCancelButton()
    {
        deleteFeedConfirmationScreen.SetActive(false);
    }

    //this method is used to success Delete Feed Response.......
    public void OnSuccessDeleteFeed()
    {
        DeleteFeedAfterRemoveAndRefreshData();
    }

    //this method is used to delete after remove prefab and remove from list and refresh.......
    void DeleteFeedAfterRemoveAndRefreshData()
    {
        MyProfileDataManager.Instance.loadedMyPostAndVideoId.Remove(feedEditOrDeleteData.feedId);

        //for image.......
        if (!string.IsNullOrEmpty(feedEditOrDeleteData.feedImage))
        {
            AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == feedEditOrDeleteData.feedId);
            int index = MyProfileDataManager.Instance.allMyFeedImageRootDataList.IndexOf(allFeedByUserIdRow);
            if (MyProfileDataManager.Instance.allPhotoContainer.childCount > index)
            {
                DestroyImmediate(MyProfileDataManager.Instance.allPhotoContainer.GetChild(index).gameObject);
            }
            MyProfileDataManager.Instance.allMyFeedImageRootDataList.Remove(allFeedByUserIdRow);
        }
        else
        {
            AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == feedEditOrDeleteData.feedId);
            int index = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.IndexOf(allFeedByUserIdRow);
            if (MyProfileDataManager.Instance.allMovieContainer.childCount > index)
            {
                DestroyImmediate(MyProfileDataManager.Instance.allMovieContainer.GetChild(index).gameObject);
            }
            MyProfileDataManager.Instance.allMyFeedVideoRootDataList.Remove(allFeedByUserIdRow);
        }

        if (editDeleteCurrentPostFeedVideoItem != null)
        {
            DestroyImmediate(editDeleteCurrentPostFeedVideoItem.gameObject);
            editDeleteCurrentPostFeedVideoItem = null;
        }

        if(videoFeedRect.GetComponent<ScrollSnapRect>().startingPage > 0)
        {
            videoFeedRect.GetComponent<ScrollSnapRect>().startingPage = videoFeedRect.GetComponent<ScrollSnapRect>().startingPage-1;
        }

        if (videofeedParent.childCount > 0)
        {
            videoFeedRect.GetComponent<ScrollSnapRect>().StartScrollSnap();//refresh and setup upto top Full view feed screen.......
        }

        OnClickDeleteFeedConfirmationCancelButton();
        editDeleteFeedScreen.GetComponent<OnEnableDisable>().ClosePopUp();

        //SNSNotificationManager.Instance.ShowNotificationMsg("Post deleted");//this method is used to show SNS notification.......

        MyProfileDataManager.Instance.RequestGetUserDetails();

        if(videofeedParent.childCount <= 0)
        {
            MyProfileDataManager.Instance.totalPostText.text = "0";
            OnClickVideoItemBackButton();
        }
    }

    /// <summary>
    /// feed edit-----------------------------
    /// </summary>
    //this method is used to Edit/Delete Feed Screen Edit button click.......
    string lastUpdatedFeedDescriptionStr = "";
    public void OnClickEditFeedDoneButton()
    {
        int updatefeedDescription = 0;
        lastUpdatedFeedDescriptionStr = "";
        if (!string.IsNullOrEmpty(feedEditOrDeleteData.feedDescriptions))
        {
            if (feedEditOrDeleteData.feedDescriptions != editFeedDescriptionInputField.Text)
            {
                //lastUpdatedFeedDescriptionStr = editFeedDescriptionInputField.Text;
                lastUpdatedFeedDescriptionStr = editFeedDescriptionInputField.RichText;
                updatefeedDescription = 1;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(editFeedDescriptionInputField.Text))
            {
                //lastUpdatedFeedDescriptionStr = editFeedDescriptionInputField.Text;
                lastUpdatedFeedDescriptionStr = editFeedDescriptionInputField.RichText;
                updatefeedDescription = 1;
            }
        }

        if (updatefeedDescription == 1)
        {
            if (string.IsNullOrEmpty(lastUpdatedFeedDescriptionStr))
            {
                lastUpdatedFeedDescriptionStr = " ";
            }

            ShowLoader(true);
            APIManager.Instance.RequestEditFeed(feedEditOrDeleteData.feedId.ToString(), APIManager.EncodedString(lastUpdatedFeedDescriptionStr), feedEditOrDeleteData.feedImage, feedEditOrDeleteData.feedVideo);
        }
        editFeedScreen.SetActive(true);
    }

    //this method is used to success response of Feed Edit.......
    public void OnSuccessFeedEdit()
    {
        //for image.......
        if (!string.IsNullOrEmpty(feedEditOrDeleteData.feedImage))
        {
            AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == feedEditOrDeleteData.feedId);
            allFeedByUserIdRow.Descriptions = lastUpdatedFeedDescriptionStr;

            int index = MyProfileDataManager.Instance.allMyFeedImageRootDataList.IndexOf(allFeedByUserIdRow);
            if (MyProfileDataManager.Instance.allPhotoContainer.childCount > index)
            {
                MyProfileDataManager.Instance.allPhotoContainer.GetChild(index).GetComponent<UserPostItem>().userData.Descriptions = lastUpdatedFeedDescriptionStr;
            }
            editDeleteCurrentPostFeedVideoItem.userData.Descriptions = lastUpdatedFeedDescriptionStr;
            editDeleteCurrentPostFeedVideoItem.descriptionText.text = lastUpdatedFeedDescriptionStr;
        }
        else
        {
            AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == feedEditOrDeleteData.feedId);
            allFeedByUserIdRow.Descriptions = lastUpdatedFeedDescriptionStr;

            int index = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.IndexOf(allFeedByUserIdRow);
            if (MyProfileDataManager.Instance.allMovieContainer.childCount > index)
            {
                MyProfileDataManager.Instance.allMovieContainer.GetChild(index).GetComponent<UserPostItem>().userData.Descriptions = lastUpdatedFeedDescriptionStr;
            }
            editDeleteCurrentPostFeedVideoItem.userData.Descriptions = lastUpdatedFeedDescriptionStr;
            editDeleteCurrentPostFeedVideoItem.descriptionText.text = lastUpdatedFeedDescriptionStr;
        }

        feedEditOrDeleteData.feedDescriptions = lastUpdatedFeedDescriptionStr;

        editDeleteCurrentPostFeedVideoItem.RefreshDescriptionAfterEdit(lastUpdatedFeedDescriptionStr);

        editFeedScreen.SetActive(false);
    }

    #endregion

    #region Get Image And Video From AWS
    public void GetVideoUrl(string key)
    {
        var request_1 = new GetPreSignedUrlRequest()
        {
            BucketName = AWSHandler.Instance.Bucketname,
            Key = key,
            Expires = DateTime.Now.AddHours(6)
        };
        //Debug.LogError("Feed Video file sending url request:" + AWSHandler.Instance._s3Client);

        AWSHandler.Instance._s3Client.GetPreSignedURLAsync(request_1, (callback) =>
        {
            if (callback.Exception == null)
            {
                string mediaUrl = callback.Response.Url;
                UnityToolbag.Dispatcher.Invoke(() =>
                {
                    //Debug.LogError("Feed Video URL " + mediaUrl);
                    //feedMediaPlayer.OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: false);
                });
            }
            else
                Debug.LogError(callback.Exception);
        });
    }

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
                //Debug.LogError("GetImageFromAWS Response:" + response.ResponseStream);
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
                        response.Dispose();
                        if (currentExtention == ExtentionType.Image)
                        {
                            //Texture2D texture = new Texture2D(2, 2);
                            //texture.LoadImage(data);
                            //Debug.LogError("key " + key + " :texture width:" + texture.width + "  height:" + texture.height);

                            AssetCache.Instance.SaveImageEnqueueOneResAndWait(key, key, data, (success) =>
                            {
                                if (success)
                                {
                                    //Debug.LogError("Save image download from Aws success");
                                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                                }
                            });
                            //mainImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                            data = null;
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

    #region Feed Comment Screeen Methods
    //this method is used to open comment screen.......
    public void OpenCommentPanel()
    {
        if (!commentPanel.activeInHierarchy)
        {
            commentInputFieldAdvanced.Clear();
            commentPanel.SetActive(true);
        }
    }    

    //this method is used to Full Feed View screen comment button click.......
    public void OnClickFeedBottomCommentButton()
    {
        OpenCommentPanel();
        commentInputFieldAdvanced.Select();
    }

    //this method is used to comment on feed after update require feed response.......
    public void CommentSuccessAfterUpdateRequireFeedResponse()
    {
        Debug.LogError("CommentSuccessAfterUpdateRequireFeedResponse:" + feedFullViewScreenCallingFrom);
        switch (feedFullViewScreenCallingFrom)
        {
            case "MyProfile":
                PostFeedVideoItem postFeedVideoItem = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<PostFeedVideoItem>();
                //current full feed selected item data update.......
                postFeedVideoItem.userData.commentCount += 1;

                string commentCountSTR = GetAbreviation(postFeedVideoItem.userData.commentCount);
                if (commentCountSTR != "0")
                {
                    postFeedVideoItem.commentBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = commentCountSTR;
                }

                //for image.......
                if (!string.IsNullOrEmpty(postFeedVideoItem.userData.Image))
                {
                    //for
                    AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == postFeedVideoItem.userData.Id);
                    allFeedByUserIdRow.commentCount = postFeedVideoItem.userData.commentCount;

                    int index = MyProfileDataManager.Instance.allMyFeedImageRootDataList.IndexOf(allFeedByUserIdRow);
                    if (MyProfileDataManager.Instance.allPhotoContainer.childCount > index)
                    {
                        MyProfileDataManager.Instance.allPhotoContainer.GetChild(index).GetComponent<UserPostItem>().userData.commentCount = allFeedByUserIdRow.commentCount;
                    }
                }
                else
                {
                    AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == postFeedVideoItem.userData.Id);
                    allFeedByUserIdRow.commentCount = postFeedVideoItem.userData.commentCount;

                    int index = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.IndexOf(allFeedByUserIdRow);
                    if (MyProfileDataManager.Instance.allMovieContainer.childCount > index)
                    {
                        MyProfileDataManager.Instance.allMovieContainer.GetChild(index).GetComponent<UserPostItem>().userData.commentCount = allFeedByUserIdRow.commentCount;
                    }
                }
                Debug.LogError("Full Feed Screen Comment calling from MyProfile");
                break;
            case "OtherProfile":
                //current full feed selected item data update.......
                PostFeedVideoItem postFeedVideoItem1 = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<PostFeedVideoItem>();
                postFeedVideoItem1.userData.commentCount += 1;

                string commentCountSTR1 = GetAbreviation(postFeedVideoItem1.userData.commentCount);
                if (commentCountSTR1 != "0")
                {
                    postFeedVideoItem1.commentBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = commentCountSTR1;
                }

                //for image.......
                if (!string.IsNullOrEmpty(postFeedVideoItem1.userData.Image))
                {
                    AllFeedByUserIdRow allFeedByUserIdRow = OtherPlayerProfileData.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == postFeedVideoItem1.userData.Id);
                    allFeedByUserIdRow.commentCount = postFeedVideoItem1.userData.commentCount;

                    int index = OtherPlayerProfileData.Instance.allMyFeedImageRootDataList.IndexOf(allFeedByUserIdRow);
                    if (OtherPlayerProfileData.Instance.userPostParent.childCount > index)
                    {
                        OtherPlayerProfileData.Instance.userPostParent.GetChild(index).GetComponent<UserPostItem>().userData.commentCount = allFeedByUserIdRow.commentCount;
                    }
                }
                else
                {
                    AllFeedByUserIdRow allFeedByUserIdRow = OtherPlayerProfileData.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == postFeedVideoItem1.userData.Id);
                    allFeedByUserIdRow.commentCount = postFeedVideoItem1.userData.commentCount;

                    int index = OtherPlayerProfileData.Instance.allMyFeedVideoRootDataList.IndexOf(allFeedByUserIdRow);
                    if (OtherPlayerProfileData.Instance.allMovieContainer.childCount > index)
                    {
                        OtherPlayerProfileData.Instance.allMovieContainer.GetChild(index).GetComponent<UserPostItem>().userData.commentCount = allFeedByUserIdRow.commentCount;
                    }
                }
                Debug.LogError("Full Feed Screen Comment calling from OtherProfile");
                break;
            case "HotTab":
                CommentAfterRefereshDiscoverAndHotFeedItemResponse();
                Debug.LogError("Full Feed Screen Comment calling from HotTab");
                break;
            case "FollowingTab":
                CommentAfterRefereshFollowingFeedItemResponse();
                Debug.LogError("Full Feed Screen Comment calling from FollowingTab");
                break;
            case "DiscoverTab":
                CommentAfterRefereshDiscoverAndHotFeedItemResponse();
                Debug.LogError("Full Feed Screen Comment calling from DiscoverTab");
                break;
            default:
                break;
        }
    }

    //this method is used to refresh comment response Following feed items.......
    void CommentAfterRefereshFollowingFeedItemResponse()
    {
        //current full feed selected item data update.......
        FollowingUserFeedItem feedFollowingItemController = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<FollowingUserFeedItem>();
        feedFollowingItemController.FollowingUserFeedData.commentCount += 1;

        string commentCountSTR = GetAbreviation(feedFollowingItemController.FollowingUserFeedData.commentCount);
        if (commentCountSTR != "0")
        {
            feedFollowingItemController.commentBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = commentCountSTR;
        }

        FeedsByFollowingUserRow feedsByFollowingUserRow = APIManager.Instance.allFollowingUserRootList.Find((x) => x.Id == feedFollowingItemController.FollowingUserFeedData.Id);
        if (feedsByFollowingUserRow != null)
        {
            feedsByFollowingUserRow.commentCount = feedFollowingItemController.FollowingUserFeedData.commentCount;
        }
    }

    //this method is used to refresh comment response Hot and Discover feed items.......
    void CommentAfterRefereshDiscoverAndHotFeedItemResponse()
    {
        //current full feed selected item data update.......
        FeedVideoItem feedVideoItem = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<FeedVideoItem>();
        feedVideoItem.FeedData.commentCount += 1;

        string commentCountSTR = GetAbreviation(feedVideoItem.FeedData.commentCount);
        if (commentCountSTR != "0")
        {
            feedVideoItem.commentBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = commentCountSTR;
        }

        AllUserWithFeedRow allUserWithFeedRow = APIManager.Instance.allUserRootList.Find((x) => x.id == feedVideoItem.FeedRawData.id);

        if (allUserWithFeedRow != null)
        {
            AllUserWithFeed allUserWithFeed = allUserWithFeedRow.feeds.Find((x) => x.id == feedVideoItem.FeedData.id);

            if (allUserWithFeed != null)
            {
                allUserWithFeed.commentCount = feedVideoItem.FeedData.commentCount;
            }
        }
    }
    #endregion

    #region Feed Like or DisLike methods.......
    //this method is used to like or dislike feed after update require feed response.......
    public void LikeDislikeSuccessAfterUpdateRequireFeedResponse(bool isLike, int likeCount)
    {
        Debug.LogError("LikeDislikeSuccessAfterUpdateRequireFeedResponse:" + feedFullViewScreenCallingFrom + "  :isLike:" + isLike + "  :LikeCount:" + likeCount);
        switch (feedFullViewScreenCallingFrom)
        {
            case "MyProfile":
                //current full feed selected item data update.......
                PostFeedVideoItem postFeedVideoItem = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<PostFeedVideoItem>();
                postFeedVideoItem.userData.LikeCount = likeCount;
                postFeedVideoItem.userData.isLike = isLike;
                postFeedVideoItem.LikeCountAndUISetup(postFeedVideoItem.userData.LikeCount);

                //for image.......
                if (!string.IsNullOrEmpty(postFeedVideoItem.userData.Image))
                {
                    //for
                    AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == postFeedVideoItem.userData.Id);
                    allFeedByUserIdRow.LikeCount = postFeedVideoItem.userData.LikeCount;
                    allFeedByUserIdRow.isLike = postFeedVideoItem.userData.isLike;
                }
                else
                {
                    AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == postFeedVideoItem.userData.Id);
                    allFeedByUserIdRow.isLike = postFeedVideoItem.userData.isLike;
                }
                Debug.LogError("Full Feed Screen like or dislike calling from MyProfile");
                break;
            case "OtherProfile":
                //current full feed selected item data update.......
                PostFeedVideoItem postFeedVideoItem1 = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<PostFeedVideoItem>();
                postFeedVideoItem1.userData.LikeCount = likeCount;
                postFeedVideoItem1.userData.isLike = isLike;
                postFeedVideoItem1.LikeCountAndUISetup(postFeedVideoItem1.userData.LikeCount);

                //for image.......
                if (!string.IsNullOrEmpty(postFeedVideoItem1.userData.Image))
                {
                    AllFeedByUserIdRow allFeedByUserIdRow = OtherPlayerProfileData.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == postFeedVideoItem1.userData.Id);
                    allFeedByUserIdRow.LikeCount = postFeedVideoItem1.userData.LikeCount;
                    allFeedByUserIdRow.isLike = postFeedVideoItem1.userData.isLike;
                }
                else
                {
                    AllFeedByUserIdRow allFeedByUserIdRow = OtherPlayerProfileData.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == postFeedVideoItem1.userData.Id);
                    allFeedByUserIdRow.LikeCount = postFeedVideoItem1.userData.LikeCount;
                    allFeedByUserIdRow.isLike = postFeedVideoItem1.userData.isLike;
                }
                Debug.LogError("Full Feed Screen like or dislike calling from OtherProfile");
                break;
            case "HotTab":
                LikeOrDisLikeAfterRefereshDiscoverAndHotFeedItemResponse(isLike, likeCount);
                Debug.LogError("Full Feed Screen like or dislike calling from HotTab");
                break;
            case "FollowingTab":
                LikeOrDisLikeAfterRefereshFollowingFeedItemResponse(isLike, likeCount);
                Debug.LogError("Full Feed Screen like or dislike calling from FollowingTab");
                break;
            case "DiscoverTab":
                LikeOrDisLikeAfterRefereshDiscoverAndHotFeedItemResponse(isLike, likeCount);
                Debug.LogError("Full Feed Screen like or dislike calling from DiscoverTab");
                break;
            default:
                break;
        }
    }

    //this method is used to refresh like or dislike response Following feed items.......
    void LikeOrDisLikeAfterRefereshFollowingFeedItemResponse(bool isLike, int likeCount)
    {
        //current full feed selected item data update.......
        FollowingUserFeedItem feedFollowingItemController = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<FollowingUserFeedItem>();
        Debug.LogError("LikeOrDisLike islike:" + isLike + "     :FeedId:" + feedFollowingItemController.FollowingUserFeedData.Id + "    :likeCount:" + feedFollowingItemController.FollowingUserFeedData.LikeCount);
        feedFollowingItemController.FollowingUserFeedData.LikeCount = likeCount;
        feedFollowingItemController.FollowingUserFeedData.isLike = isLike;

        feedFollowingItemController.LikeCountAndUISetup(feedFollowingItemController.FollowingUserFeedData.LikeCount);

        FeedsByFollowingUserRow feedsByFollowingUserRow = APIManager.Instance.allFollowingUserRootList.Find((x) => x.Id == feedFollowingItemController.FollowingUserFeedData.Id);
        if (feedsByFollowingUserRow != null)
        {
            feedsByFollowingUserRow.LikeCount = feedFollowingItemController.FollowingUserFeedData.LikeCount;
            feedsByFollowingUserRow.isLike = feedFollowingItemController.FollowingUserFeedData.isLike;
        }
    }

    //this method is used to refresh comment response Hot and Discover feed items.......
    void LikeOrDisLikeAfterRefereshDiscoverAndHotFeedItemResponse(bool isLike, int likeCount)
    {
        //current full feed selected item data update.......
        FeedVideoItem feedVideoItem = videofeedParent.GetChild(videoFeedRect.GetComponent<ScrollSnapRect>()._currentPage).GetComponent<FeedVideoItem>();
        Debug.LogError("LikeOrDisLike islike:" + isLike + "     :feedRawData Id:" + feedVideoItem.FeedRawData.id + "    :FeedId:" + feedVideoItem.FeedData.id + "   :likeCount:" + feedVideoItem.FeedData.likeCount);
        feedVideoItem.FeedData.likeCount = likeCount;
        feedVideoItem.FeedData.isLike = isLike;
        feedVideoItem.LikeCountAndUISetup(feedVideoItem.FeedData.likeCount);
        
        AllUserWithFeedRow allUserWithFeedRow = APIManager.Instance.allUserRootList.Find((x) => x.id == feedVideoItem.FeedRawData.id);

        if (allUserWithFeedRow != null)
        {
            AllUserWithFeed allUserWithFeed = allUserWithFeedRow.feeds.Find((x) => x.id == feedVideoItem.FeedData.id);

            if (allUserWithFeed != null)
            {
                allUserWithFeed.likeCount = feedVideoItem.FeedData.likeCount;
                allUserWithFeed.isLike = feedVideoItem.FeedData.isLike;
            }
        }
    }
    #endregion

    #region Get Int Value in 1K 1M 1B.......
    public string GetAbreviation(int value)
    {
        if (value < 0)
        {
            value = 0;
        }
        int NrOfDigits = GetNumberOfDigits(value);
        double FirstDigits;
        string Abreviation = "";
        //Debug.Log (NrOfDigits);
        if (NrOfDigits % 3 == 0)
        {
            FirstDigits = value / (Mathf.Pow(10, NrOfDigits - 3));
            if ((Mathf.Pow(10, NrOfDigits - 3)) / 1000 == 1)
            {
                Abreviation = "K";
            }
            else if ((Mathf.Pow(10, NrOfDigits - 3)) / 1000000 == 1)
            {
                Abreviation = "M";
            }
            else if ((Mathf.Pow(10, NrOfDigits - 3)) / 1000000000 == 1)
            {
                Abreviation = "B";
            }
        }
        else
        {
            FirstDigits = value / (Mathf.Pow(10, NrOfDigits - NrOfDigits % 3));
            if ((Mathf.Pow(10, NrOfDigits - NrOfDigits % 3)) / 1000 == 1)
            {
                Abreviation = "K";
            }
            else if ((Mathf.Pow(10, NrOfDigits - NrOfDigits % 3)) / 1000000 == 1)
            {
                Abreviation = "M";
            }
            else if ((Mathf.Pow(10, NrOfDigits - NrOfDigits % 3)) / 1000000000 == 1)
            {
                Abreviation = "B";
            }
        }
        int valueOfFlaot = 0;
        if (FirstDigits % 1 > 0)
        {
            valueOfFlaot = 2;
        }
        return FirstDigits.ToString("F" + valueOfFlaot.ToString()) + Abreviation;
    }

    private int GetNumberOfDigits(int value)
    {
        int NrOfDigits = 0;
        while (value > 0)
        {
            value = value / 10;
            NrOfDigits++;
        }
        return NrOfDigits;
    }

    public void TestKMBValue(int value)
    {
        Debug.LogError("Test Value:" + GetAbreviation(value));
    }
    #endregion

    #region Feed Comment and Like Socket Event Refresh Data.......
    public void FeedCommentAndLikeSocketEventSuccessAfterUpdateRequireFeedResponse(int feedId, int createdBY, int likeCount, int commentCount, bool isComesFromLike)
    {
        Debug.LogError("LikeDislikeSuccessAfterUpdateRequireFeedResponse:" + feedFullViewScreenCallingFrom);
        switch (feedFullViewScreenCallingFrom)
        {
            case "MyProfile":
                Debug.LogError("Full Feed Screen like or dislike or feed comment socket event calling from MyProfile");
                FeedCommentAndLikeSocketEventAfterRefereshMyProfileResponse(feedId, likeCount, commentCount, isComesFromLike, true);
                break;
            case "OtherProfile":
                Debug.LogError("Full Feed Screen like or dislike or feed comment socket event calling from OtherProfile");
                FeedCommentAndLikeSocketEventAfterRefereshOtherUserProfileResponse(feedId, likeCount, commentCount, isComesFromLike, true);                
                break;
            case "HotTab":
                Debug.LogError("Full Feed Screen like or dislike or feed comment socket event calling from HotTab");
                FeedCommentAndLikeSocketEventAfterRefereshDiscoverAndHotFeedItemResponse(feedId, createdBY, likeCount, commentCount, isComesFromLike, true);
                break;
            case "FollowingTab":
                Debug.LogError("Full Feed Screen like or dislike or feed comment socket event calling from FollowingTab");
                FeedCommentAndLikeSocketEventAfterRefereshFollowingFeedItemResponse(feedId, likeCount, commentCount, isComesFromLike, true);
                break;
            case "DiscoverTab":
                Debug.LogError("Full Feed Screen like or dislike or feed comment socket event calling from DiscoverTab");
                FeedCommentAndLikeSocketEventAfterRefereshDiscoverAndHotFeedItemResponse(feedId, createdBY, likeCount, commentCount, isComesFromLike, true);
                break;
            default:
                Debug.LogError("Default call.......");
                FeedCommentAndLikeSocketEventAfterRefereshDiscoverAndHotFeedItemResponse(feedId, createdBY, likeCount, commentCount, isComesFromLike, false);
                FeedCommentAndLikeSocketEventAfterRefereshFollowingFeedItemResponse(feedId, likeCount, commentCount, isComesFromLike, false);
                FeedCommentAndLikeSocketEventAfterRefereshMyProfileResponse(feedId, likeCount, commentCount, isComesFromLike, false);
                FeedCommentAndLikeSocketEventAfterRefereshOtherUserProfileResponse(feedId, likeCount, commentCount, isComesFromLike, false);
                break;
        }
    }

    //this method is used to refresh like or dislike or comment socket event response Following Tab.......
    void FeedCommentAndLikeSocketEventAfterRefereshFollowingFeedItemResponse(int feedId, int likeCount, int commentCount, bool isComesFromLike, bool isFullFeedScreen)
    {
        Debug.LogError("FeedCommentAndLikeSocketEventAfterRefereshFollowingFeedItemResponse feed ID:" + feedId + "  :LikeCount:" + likeCount + "    :CommentCount:" + commentCount + "  :IsComesFromLike:" + isComesFromLike + "    :IsFullScreen:" + isFullFeedScreen);
        if (APIController.Instance.feedFollowingIdList.Contains(feedId))
        {
            Debug.LogError("Following tab feed data updated");
            FeedsByFollowingUserRow feedsByFollowingUserRow = APIManager.Instance.allFollowingUserRootList.Find((x) => x.Id == feedId);
            if (feedsByFollowingUserRow != null)
            {
                if (isComesFromLike)
                {
                    feedsByFollowingUserRow.LikeCount = likeCount;
                }
                else
                {
                    feedsByFollowingUserRow.commentCount = commentCount;
                }
            }

            if (isFullFeedScreen)
            {
                int index = APIController.Instance.feedFollowingIdList.IndexOf(feedId);
                Debug.LogError("Index:" + index);
                //current full feed selected item data update.......
                FollowingUserFeedItem feedFollowingItemController = videofeedParent.GetChild(index).GetComponent<FollowingUserFeedItem>();
                if (isComesFromLike)
                {
                    feedFollowingItemController.FollowingUserFeedData.LikeCount = likeCount;
                    feedFollowingItemController.LikeCountAndUISetup(feedFollowingItemController.FollowingUserFeedData.LikeCount);
                }
                else
                {
                    feedFollowingItemController.FollowingUserFeedData.commentCount = commentCount;
                    feedFollowingItemController.CommentCountUISetup(feedFollowingItemController.FollowingUserFeedData.commentCount);

                    Debug.LogError("Comment Panel.......:" + commentPanel.activeInHierarchy);
                    if (commentPanel.activeInHierarchy)//if comment screen is open then refresh comment list api.......
                    {
                        feedFollowingItemController.OnClickCommentButton(true);
                    }
                }
            }
        }
    }

    //this method is used to refresh like or dislike or comment socket event response Hot and Discover Tab.......
    void FeedCommentAndLikeSocketEventAfterRefereshDiscoverAndHotFeedItemResponse(int feedId , int createdBY, int likeCount, int commentCount, bool isComesFromLike, bool isFullFeedScreen)
    {
        Debug.LogError("FeedCommentAndLikeSocketEventAfterRefereshDiscoverAndHotFeedItemResponse feed ID:" + feedId  + "    :CreatedBY:" + createdBY + "  :LikeCount:" + likeCount + "    :CommentCount:" + commentCount + "  :IsComesFromLike:" + isComesFromLike + "    :IsFullScreen:" + isFullFeedScreen);
        if (APIController.Instance.feedHotIdList.Contains(createdBY))
        {
            if (APIController.Instance.feedForYouIdList.Contains(feedId))
            {
                Debug.LogError("Hot or Discover tab feed data updated");
                AllUserWithFeedRow allUserWithFeedRow = APIManager.Instance.allUserRootList.Find((x) => x.id == createdBY);

                if (allUserWithFeedRow != null)
                {
                    AllUserWithFeed allUserWithFeed = allUserWithFeedRow.feeds.Find((x) => x.id == feedId);

                    if (allUserWithFeed != null)
                    {
                        if (isComesFromLike)
                        {
                            allUserWithFeed.likeCount = likeCount;
                        }
                        else
                        {
                            allUserWithFeed.commentCount = commentCount;
                        }
                    }
                }

                if (isFullFeedScreen)
                {
                    int index = APIController.Instance.feedForYouIdList.IndexOf(feedId);
                    Debug.LogError("Index:" + index);
                    //current full feed selected item data update.......
                    FeedVideoItem feedVideoItem = videofeedParent.GetChild(index).GetComponent<FeedVideoItem>();
                    if (isComesFromLike)
                    {
                        feedVideoItem.FeedData.likeCount = likeCount;
                        feedVideoItem.LikeCountAndUISetup(feedVideoItem.FeedData.likeCount);
                    }
                    else
                    {
                        feedVideoItem.FeedData.commentCount = commentCount;
                        feedVideoItem.CommentCountUISetup(feedVideoItem.FeedData.commentCount);

                        Debug.LogError("Comment Panel.......:" + commentPanel.activeInHierarchy);
                        if (commentPanel.activeInHierarchy)//if comment screen is open then refresh comment list api.......
                        {
                            feedVideoItem.OnClickCommentButton(true);
                        }
                    }
                }                
            }
        }
    }

    //this method is used to refresh like or dislike or comment socket event response MyProfile screen.......
    void FeedCommentAndLikeSocketEventAfterRefereshMyProfileResponse(int feedId, int likeCount, int commentCount, bool isComesFromLike, bool isFullFeedScreen)
    {
        Debug.LogError("FeedCommentAndLikeSocketEventAfterRefereshMyProfileResponse feed ID:" + feedId + "  :LikeCount:" + likeCount + "    :CommentCount:" + commentCount + "  :IsComesFromLike:" + isComesFromLike + "    :IsFullScreen:" + isFullFeedScreen);
        if (MyProfileDataManager.Instance.loadedMyPostAndVideoId.Contains(feedId))
        {
            AllFeedByUserIdRow allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == feedId);
            if (allFeedByUserIdRow == null)
            {
                allFeedByUserIdRow = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == feedId);
            }

            if (allFeedByUserIdRow != null)
            {
                Debug.LogError("my profile feed data updated");
                if (isComesFromLike)
                {
                    allFeedByUserIdRow.LikeCount = likeCount;
                }
                else
                {
                    allFeedByUserIdRow.commentCount = commentCount;
                }
                if (isFullFeedScreen)
                {
                    int index;
                    if (!string.IsNullOrEmpty(allFeedByUserIdRow.Image))
                    {
                        index = MyProfileDataManager.Instance.allMyFeedImageRootDataList.IndexOf(allFeedByUserIdRow);
                    }
                    else
                    {
                        index = MyProfileDataManager.Instance.allMyFeedVideoRootDataList.IndexOf(allFeedByUserIdRow);
                    }

                    if (index < videofeedParent.childCount)
                    {
                        PostFeedVideoItem postFeedVideoItem = videofeedParent.GetChild(index).GetComponent<PostFeedVideoItem>();
                        Debug.LogError("index:" + index + " :postFeedVideoItem Id:" + postFeedVideoItem.userData.Id);
                        if (postFeedVideoItem.userData.Id == feedId)
                        {
                            if (isComesFromLike)
                            {
                                postFeedVideoItem.userData.LikeCount = likeCount;
                                postFeedVideoItem.LikeCountAndUISetup(postFeedVideoItem.userData.LikeCount);
                            }
                            else
                            {
                                postFeedVideoItem.userData.commentCount = commentCount;
                                postFeedVideoItem.CommentCountUISetup(postFeedVideoItem.userData.commentCount);
                                Debug.LogError("Comment Panel.......:" + commentPanel.activeInHierarchy);
                                if (commentPanel.activeInHierarchy)//if comment screen is open then refresh comment list api.......
                                {
                                    postFeedVideoItem.OnClickCommentButton(true);
                                }
                            }
                        }
                    }
                }
            }            
        }
    }

    //this method is used to refresh like or dislike or comment socket event response Other user Profile screen.......
    void FeedCommentAndLikeSocketEventAfterRefereshOtherUserProfileResponse(int feedId, int likeCount, int commentCount, bool isComesFromLike, bool isFullFeedScreen)
    {
        Debug.LogError("FeedCommentAndLikeSocketEventAfterRefereshOtherUserProfileResponse feed ID:" + feedId + "  :LikeCount:" + likeCount + "    :CommentCount:" + commentCount + "  :IsComesFromLike:" + isComesFromLike + "    :IsFullScreen:" + isFullFeedScreen);
        if (OtherPlayerProfileData.Instance.loadedMyPostAndVideoId.Contains(feedId))
        {
            AllFeedByUserIdRow allFeedByUserIdRow = OtherPlayerProfileData.Instance.allMyFeedImageRootDataList.Find((x) => x.Id == feedId);
            if (allFeedByUserIdRow == null)
            {
                allFeedByUserIdRow = OtherPlayerProfileData.Instance.allMyFeedVideoRootDataList.Find((x) => x.Id == feedId);
            }

            if (allFeedByUserIdRow != null)
            {
                Debug.LogError("my profile feed data updated");
                if (isComesFromLike)
                {
                    allFeedByUserIdRow.LikeCount = likeCount;
                }
                else
                {
                    allFeedByUserIdRow.commentCount = commentCount;
                }
                if (isFullFeedScreen)
                {
                    int index;
                    if (!string.IsNullOrEmpty(allFeedByUserIdRow.Image))
                    {
                        index = OtherPlayerProfileData.Instance.allMyFeedImageRootDataList.IndexOf(allFeedByUserIdRow);
                    }
                    else
                    {
                        index = OtherPlayerProfileData.Instance.allMyFeedVideoRootDataList.IndexOf(allFeedByUserIdRow);
                    }

                    if (index < videofeedParent.childCount)
                    {
                        PostFeedVideoItem postFeedVideoItem = videofeedParent.GetChild(index).GetComponent<PostFeedVideoItem>();
                        Debug.LogError("index:" + index + " :postFeedVideoItem Id:" + postFeedVideoItem.userData.Id);
                        if (postFeedVideoItem.userData.Id == feedId)
                        {
                            if (isComesFromLike)
                            {
                                postFeedVideoItem.userData.LikeCount = likeCount;
                                postFeedVideoItem.LikeCountAndUISetup(postFeedVideoItem.userData.LikeCount);
                            }
                            else
                            {
                                postFeedVideoItem.userData.commentCount = commentCount;
                                postFeedVideoItem.CommentCountUISetup(postFeedVideoItem.userData.commentCount);
                                Debug.LogError("Comment Panel.......:" + commentPanel.activeInHierarchy);
                                if (commentPanel.activeInHierarchy)//if comment screen is open then refresh comment list api.......
                                {
                                    postFeedVideoItem.OnClickCommentButton(true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}