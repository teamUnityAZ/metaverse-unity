using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SuperStar.Helpers;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using UnityEngine.UI.Extensions;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using AdvancedInputFieldPlugin;
using System.Linq;
using System.Text;
using System.Web;

public class MyProfileDataManager : MonoBehaviour
{
    public static MyProfileDataManager Instance;

    public string defaultUrl = "https://";

    public GetUserDetailData myProfileData = new GetUserDetailData();

    public List<AllFeedByUserIdRow> allMyFeedImageRootDataList = new List<AllFeedByUserIdRow>();//image feed list
    public List<AllFeedByUserIdRow> allMyFeedVideoRootDataList = new List<AllFeedByUserIdRow>();//video feed list

    public AllFeedByUserIdRoot currentPageAllFeedWithUserIdRoot = new AllFeedByUserIdRoot();

    public AllUserWithFeedRow FeedRawData;

    [Space]
    [Header("Screen References")]
    public GameObject myProfileScreen;
    public GameObject editProfileScreen;
    public GameObject pickImageOptionScreen;

    [Space]
    [Header("Profile Screen Refresh Object")]
    public GameObject mainFullScreenContainer;
    public GameObject mainProfileDetailPart;
    public GameObject userPostPart;
    public GameObject bioDetailPart;

    [Space]
    [Header("Player info References")]
    public TextMeshProUGUI topHaderUserNameText;
    public Image profileImage;
    public TextMeshProUGUI totalPostText;
    public TextMeshProUGUI totalFollowerText;
    public TextMeshProUGUI totalFollowingText;
    [Space]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI textUserBio;
    public TextMeshProUGUI websiteText;

    public GameObject seeMoreBioButton;
    public GameObject seeMoreButtonTextObj;
    public GameObject seeLessButtonTextObj;

    [Space]
    [Header("Photo, Movie, NFT Button Panel Tab panel Reference")]
    public ScrollRectGiftScreen tabScrollRectGiftScreen;
    public ParentHeightResetScript parentHeightResetScript;
    public SelectionItemScript selectionItemScript1;
    public SelectionItemScript selectionItemScript2;
    
    [Space]
    [Header("Follow Message Button References")]
    public Image followButtonImage;
    public Sprite followSprite, followingSprite;
    public TextMeshProUGUI followFollowingText;
    public Color followTextColor, FollowingTextColor;

    [Space]
    [Header("Player Uploaded Item References")]
    public Transform allPhotoContainer;
    public Transform allTagContainer;
    public Transform allMovieContainer;
    public GameObject photoPrefab;

    [Header("post empty message reference")]
    public GameObject createYourFirstPostMsgObj;
    public GameObject emptyPhotoPostMsgObj;
    public GameObject emptyMoviePostMsgObj;
    public GameObject FooterCreateIcon;

    [Space]
    public GameObject tabPrivateObject;
    public GameObject tabPublicObject;

    [Space]
    public Sprite defultProfileImage;

    [Space]
    [Header("Edit Profile Reference")]
    public Image editProfileImage;
    //public TMP_InputField editProfileNameInputfield;
    public InputField editProfileNameInputfield;
    public AdvancedInputField editProfileNameAdvanceInputfield;
    //public TMP_InputField editProfileJobInputfield;
    public InputField editProfileJobInputfield;
    public AdvancedInputField editProfileJobAdvanceInputfield;
    //public TMP_InputField editProfileWebsiteInputfield;
    public InputField editProfileWebsiteInputfield;
    public AdvancedInputField editProfileWebsiteAdvanceInputfield;
    public TMP_InputField editProfileBioInputfield;
    //public InputField editProfileBioInputfield;
    //public TMP_InputField editProfileGenderInputfield;
    public InputField editProfileGenderInputfield;
    public GameObject editProfilemainInfoPart;
    public GameObject websiteErrorObj;
    public GameObject nameErrorMessageObj;
    public Button editProfileDoneButton;

    [Space]
    public GameObject editProfileBioScreen;
    //public TMP_InputField bioEditInputField;
    public InputField bioEditInputField;

    public AdvancedInputField bioEditAdvanceInputField;

    [Space]
    [Header("For API Pagination")]
    public ScrollRectFasterEx profileMainScrollRectFasterEx;
    public bool isFeedLoaded = false;
    public int profileFeedAPiCurrentPageIndex = 1;

    [Space]
    [Header("Premium UserRole Referense")]
    public UserRolesView userRolesView;
    /*public GameObject AlphaPassUI;
    public GameObject PremiumUserUI;    
    public GameObject DJEventUI;
    public GameObject VIPPassEvent;*/

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    int tempOPCount = 0;
    bool tempLogout = false;
    private void OnEnable()
    {
        if (tempOPCount == 0) 
        { 
            userRolesView.SetUpUserRole(ConstantsGod.UserPriorityRole, ConstantsGod.UserRoles);//this method is used to set user role.......
            tempOPCount++;
        }
        else
        {
            if (tempLogout)
            {
                tempLogout = false;
                StartCoroutine(WaitToRefreshProfileScreen());
            }
        }
    }

    private void Start()
    {
        ClearDummyData();//clear dummy data.......

        string saveDir = Path.Combine(Application.persistentDataPath, "XanaChat");
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        if (GlobalVeriableClass.callingScreen == "Profile")
        {
            //ProfileTabButtonClick();
            myProfileScreen.SetActive(true);
        }

        string countryName = System.Globalization.RegionInfo.CurrentRegion.EnglishName;
        //Debug.LogError("Country Name:" + countryName + "    Name:"+ System.Globalization.RegionInfo.CurrentRegion.Name);
    }

    private void Update()
    {
        if (myProfileScreen.activeSelf)
        {
            ProfileAPiPagination();//profile feed pagination.......
        }
    }

    //this method is used to clear the dummy data.......
    public void ClearDummyData()
    {
        playerNameText.text = "";
        topHaderUserNameText.text = "";
        topHaderUserNameText.GetComponent<LayoutElement>().enabled = false;
        jobText.text = "";
        jobText.gameObject.SetActive(false);
        textUserBio.text = "";
        websiteText.text = "";
        websiteText.gameObject.SetActive(false);
        profileImage.sprite = defultProfileImage;
    }

    //this method is used to clear my profile data after logout.......
    public void ClearAndResetAfterLogout()
    {
        userRolesView.ResetBadges();
        loadedMyPostAndVideoId.Clear();  //amit-19-3-2022 onlogout clear feed id list 
        ClearDummyData();
        tempLogout = true;
        MyProfileSceenShow(false);
    }

    //this method is used to Profile Tab Button Click
    public void ProfileTabButtonClick()
    {
        APIManager.Instance.RequestGetUserDetails("myProfile");//Get My Profile data       
        MyProfileSceenShow(true);//active myprofile screen
    }

    public void MyProfileSceenShow(bool isShow)
    {
        myProfileScreen.SetActive(isShow);
        if (!isShow)
        {
            SetupEmptyMsgForPhotoTab(true);//check for empty message.......
        }
    }

    public void SetupData(GetUserDetailData myData, string callingFrom)
    {
        myProfileData = myData;
        Debug.LogError(callingFrom);
        if (callingFrom == "EditProfileAvatar")
        {
            FeedUIController.Instance.ShowLoader(false);
            EditProfileDoneButtonSetUp(true);//setup edit profile done button.......
            editProfileScreen.SetActive(false);
                       
            Debug.LogError("Profile Update Success and delete file");
            if (File.Exists(setImageAvatarTempPath))
            {
                File.Delete(setImageAvatarTempPath);
            }
            /*if (setGroupFromCamera)
            {
                if (File.Exists(setImageAvatarTempPath))
                {
                    File.Delete(setImageAvatarTempPath);
                }
            }*/
            if (AssetCache.Instance.HasFile(setImageAvatarTempFilename))
            {
                Debug.LogError("IOS update Profile Pic Delete");
                AssetCache.Instance.DeleteAsset(setImageAvatarTempFilename);
            }
            //setGroupFromCamera = false;
            setImageAvatarTempPath = "";
            setImageAvatarTempFilename = "";

            LoadDataMyProfile();//set data
        }
        else
        {
            LoadDataMyProfile();//set data
            APIManager.Instance.RequestGetFeedsByUserId(APIManager.Instance.userId, 1, 30, "MyProfile");
            //APIManager.Instance.RequestGetFeedsByUserId(APIManager.Instance.userId, 1, myProfileData.feedCount, "MyProfile");
        }
    }

    bool isSetTempSpriteAfterUpdateAvatar = false;
    //this method is used to set temp profile image after update profile image.......
    public void AfterUpdateAvatarSetTempSprite()
    {
        if (profileImage.sprite != editProfileImage.sprite)
        {
            isSetTempSpriteAfterUpdateAvatar = true;
            profileImage.sprite = editProfileImage.sprite;
        }
    }
        
    string lastTopUserText;
    public void LoadDataMyProfile()
    {
        userRolesView.SetUpUserRole(ConstantsGod.UserPriorityRole, ConstantsGod.UserRoles);//this method is used to set user role.......

        /*AlphaPassUI.SetActive(false);
        PremiumUserUI.SetActive(false);
        DJEventUI.SetActive(false);
        VIPPassEvent.SetActive(false);*/

        /*switch (ConstantsGod.UserPriorityRole)
        {
            case "alpha-pass":
                {
                    AlphaPassUI.SetActive(true);
                    break;
                }
            case "premium":
                {
                    PremiumUserUI.SetActive(true);
                    break;
                } 
            case "dj-event":
                {
                    DJEventUI.SetActive(true);   
                    break;
                }        
            case "free":
                {
                    break;
                }
            case "vip-pass":
                {
                    VIPPassEvent.SetActive(true);
                    break;
                }
        }*/

        topHaderUserNameText.GetComponent<LayoutElement>().enabled = false;

        playerNameText.text = myProfileData.name;
        topHaderUserNameText.text = myProfileData.name;

        if (lastTopUserText != myProfileData.name)
        {
            topHaderUserNameText.GetComponent<ResetPrefferedWidthScript>().SetupObjectWidth();
        }
        lastTopUserText = myProfileData.name;

        totalFollowerText.text = myProfileData.followerCount.ToString();
        totalFollowingText.text = myProfileData.followingCount.ToString();
        totalPostText.text = myProfileData.feedCount.ToString();

        if (string.IsNullOrEmpty(myProfileData.userProfile.website))
        {
            websiteText.gameObject.SetActive(false);
        }
        else
        {
            websiteText.text = myProfileData.userProfile.website;
            websiteText.gameObject.SetActive(true);
        }

        if (myProfileData.userProfile != null)
        {
            if (!string.IsNullOrEmpty(myProfileData.userProfile.job))
            {
                //jobText.text = Regex.Replace(myProfileData.userProfile.job, @"[^\u0000-\u007F]", String.Empty); 
                //jobText.text = myProfileData.userProfile.job;
                jobText.text = APIManager.DecodedString(myProfileData.userProfile.job);
                jobText.gameObject.SetActive(true);
            }
            else
            {
                jobText.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(myProfileData.userProfile.bio))
            {
                textUserBio.text = APIManager.DecodedString(myProfileData.userProfile.bio);
                SetupBioPart(textUserBio.text);//check and show only 10 line.......
            }
            else
            {
                //textUserBio.text = "You have no bio yet.";
                seeMoreBioButton.SetActive(false);
                textUserBio.text = TextLocalization.GetLocaliseTextByKey("You have no bio yet.");
            }
        }
        else
        {
            //textUserBio.text = "You have no bio yet.";
            seeMoreBioButton.SetActive(false);
            textUserBio.text = TextLocalization.GetLocaliseTextByKey("You have no bio yet.");
        }

        Debug.LogError("isSetTempSpriteAfterUpdateAvatar:" + isSetTempSpriteAfterUpdateAvatar);
        if (!isSetTempSpriteAfterUpdateAvatar)//if temp avatar set is true then do not add default profile image.......
        {
            profileImage.sprite = defultProfileImage;
        }
        isSetTempSpriteAfterUpdateAvatar = false;

        if (!string.IsNullOrEmpty(myProfileData.avatar))
        {
            //Debug.LogError("My profile Avatar :-" + myProfileData.avatar);
            GetImageFromAWS(myProfileData.avatar, profileImage);
        }
        else
        {
            profileImage.sprite = defultProfileImage;
        }

        StartCoroutine(WaitToRefreshProfileScreen());
    }

    public string ReplaceNonCharacters(string aString, char replacement)
    {
        var sb = new StringBuilder(aString.Length);
        for (var i = 0; i < aString.Length; i++)
        {
            if (char.IsSurrogatePair(aString, i))
            {
                int c = char.ConvertToUtf32(aString, i);
                i++;
                if (IsCharacter(c))
                    sb.Append(char.ConvertFromUtf32(c));
                else
                    sb.Append(replacement);
            }
            else
            {
                char c = aString[i];
                if (IsCharacter(c))
                    sb.Append(c);
                else
                    sb.Append(replacement);
            }
        }
        return sb.ToString();
    }

    public bool IsCharacter(int point)
    {
        return point < 0xFDD0 || // everything below here is fine
            point > 0xFDEF &&    // exclude the 0xFFD0...0xFDEF non-characters
            (point & 0xfffE) != 0xFFFE; // exclude all other non-characters
    }

    IEnumerator WaitToRefreshProfileScreen()
    {
        bioDetailPart.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.01f);

        bioDetailPart.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        mainProfileDetailPart.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.01f);
        mainProfileDetailPart.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        mainFullScreenContainer.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.01f);
        mainFullScreenContainer.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    string tempBioOnly10LineStr = "";
    public void SetupBioPart(string bioText)
    {
        int numLines = bioText.Split('\n').Length;
        //Debug.LogError("Bio Line Count:" + numLines);

        if (numLines > 10)
        {
            string[] bioLineSTR = bioText.Split('\n').Take(10).ToArray();
            //Debug.LogError("Result:" + bioLineSTR);

            tempBioOnly10LineStr = "";
            for (int i = 0; i < bioLineSTR.Length; i++)
            {
                tempBioOnly10LineStr += bioLineSTR[i] + "\n";
            }
            textUserBio.text = tempBioOnly10LineStr;

            SeeMoreLessBioTextSetup(true);
            seeMoreBioButton.SetActive(true);
        }
        else
        {
            //false see more button
            seeMoreBioButton.SetActive(false);
        }
    }

    //this method is used to Bio SeeMore or Less button click.......
    public void OnClickBioSeeMoreLessButton()
    {
        if (seeMoreButtonTextObj.activeSelf)
        {
            textUserBio.text = APIManager.DecodedString(myProfileData.userProfile.bio);
            SeeMoreLessBioTextSetup(false);
        }
        else
        {
            textUserBio.text = tempBioOnly10LineStr;
            SeeMoreLessBioTextSetup(true);
        }
        ResetMainScrollDefaultTopPos();
        StartCoroutine(WaitToRefreshProfileScreen());
    }

    void SeeMoreLessBioTextSetup(bool isSeeMore)
    {
        seeMoreButtonTextObj.SetActive(isSeeMore);
        seeLessButtonTextObj.SetActive(!isSeeMore);
    }

    //this method is used to reset to main scroll default position to top.......
    public void ResetMainScrollDefaultTopPos()
    {
        profileMainScrollRectFasterEx.verticalNormalizedPosition = 1;
    }

    //this method is used to MyProfile APi Pagination.......
    //public float lastVerticalNormalizedPosition = -1;
    public void ProfileAPiPagination()
    {
        //Debug.LogError("Profile y pos:" + profileMainScrollRectFasterEx.verticalEndPos + "  :verticalnormalize pos:"+ profileMainScrollRectFasterEx.verticalNormalizedPosition + "  :normalize:"+profileMainScrollRectFasterEx.normalizedPosition + "   :isLoaded:"+ isFeedLoaded);
        //if (profileMainScrollRectFasterEx.verticalEndPos <= 1 && isFeedLoaded)
        //if (profileMainScrollRectFasterEx.verticalNormalizedPosition <= 0 && lastVerticalNormalizedPosition != profileMainScrollRectFasterEx.verticalNormalizedPosition && isFeedLoaded)
        if (profileMainScrollRectFasterEx.verticalNormalizedPosition < 0.01f && isFeedLoaded)
        {
            //Debug.LogError("scrollRect pos :" + profileMainScrollRectFasterEx.verticalNormalizedPosition + " rows count:" + allFeedWithUserIdRoot.Data.Rows.Count + "   :pageIndex:" + (profileFeedAPiCurrentPageIndex+1));
            //lastVerticalNormalizedPosition = profileMainScrollRectFasterEx.verticalNormalizedPosition;
            if (currentPageAllFeedWithUserIdRoot.Data.Rows.Count > 0)
            {
                isFeedLoaded = false;
                //Debug.LogError("isDataLoad False");
                APIManager.Instance.RequestGetFeedsByUserId(APIManager.Instance.userId, (profileFeedAPiCurrentPageIndex + 1), 10, "MyProfile");
            }
        }
    }

    public List<int> loadedMyPostAndVideoId = new List<int>();
    public void AllFeedWithUserId(int pageNumb)
    {
        currentPageAllFeedWithUserIdRoot = APIManager.Instance.allFeedWithUserIdRoot;

        /*foreach (Transform item in allPhotoContainer)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in allMovieContainer)
        {
            Destroy(item.gameObject);
        }*/

        FeedUIController.Instance.ShowLoader(false);
        //FeedUIController.Instance.OnClickCheckOtherPlayerProfile();

        for (int i = 0; i < currentPageAllFeedWithUserIdRoot.Data.Rows.Count; i++)
        {
            if (!loadedMyPostAndVideoId.Contains(currentPageAllFeedWithUserIdRoot.Data.Rows[i].Id))
            {
                bool isVideo = false;

                Transform parent = allPhotoContainer;
                if (!string.IsNullOrEmpty(currentPageAllFeedWithUserIdRoot.Data.Rows[i].Image))
                {
                    parent = allPhotoContainer;
                }
                else if (!string.IsNullOrEmpty(currentPageAllFeedWithUserIdRoot.Data.Rows[i].Video))
                {
                    isVideo = true;
                    parent = allMovieContainer;
                }

                GameObject userTagPostObject = Instantiate(photoPrefab, parent);
                UserPostItem userPostItem = userTagPostObject.GetComponent<UserPostItem>();
                userPostItem.userData = currentPageAllFeedWithUserIdRoot.Data.Rows[i];
                FeedsByFollowingUser feedUserData = new FeedsByFollowingUser();
                feedUserData.Id = myProfileData.id;
                feedUserData.Name = myProfileData.name;
                feedUserData.Email = myProfileData.email;
                feedUserData.Avatar = myProfileData.avatar;
                userPostItem.feedUserData = feedUserData;
                userPostItem.avtarUrl = myProfileData.avatar;
                userPostItem.LoadFeed();

                loadedMyPostAndVideoId.Add(currentPageAllFeedWithUserIdRoot.Data.Rows[i].Id);

                if (pageNumb == 1 && i == 0)
                {
                    //Debug.LogError("Latest Profile pic set as top");
                    userTagPostObject.transform.SetAsFirstSibling();
                    if (!isVideo)//image
                    {
                        allMyFeedImageRootDataList.Insert(0, currentPageAllFeedWithUserIdRoot.Data.Rows[i]);
                    }
                    else
                    {
                        allMyFeedVideoRootDataList.Insert(0, currentPageAllFeedWithUserIdRoot.Data.Rows[i]);
                    }
                }
                else
                {
                    if (!isVideo)//image
                    {
                        allMyFeedImageRootDataList.Add(currentPageAllFeedWithUserIdRoot.Data.Rows[i]);
                    }
                    else
                    {
                        allMyFeedVideoRootDataList.Add(currentPageAllFeedWithUserIdRoot.Data.Rows[i]);
                    }
                }
            }
        }

        StartCoroutine(WaitToFeedLoadedUpdate(pageNumb));
    }

    IEnumerator WaitToFeedLoadedUpdate(int pageNum)
    {
        yield return new WaitForSeconds(0.1f);
        userPostPart.GetComponent<ParentHeightResetScript>().GetAndCheckMaxHeightInAllTab();

        SetupEmptyMsgForPhotoTab(false);//check for empty message.......

        yield return new WaitForSeconds(1f);
        isFeedLoaded = true;
        if (pageNum > 1 && currentPageAllFeedWithUserIdRoot.Data.Rows.Count > 0)
        {
            profileFeedAPiCurrentPageIndex += 1;
        }
        Debug.LogError("my profile AllFeedWithUserId:" + isFeedLoaded);
    }

    public IEnumerator AllTagFeed()
    {
        foreach (Transform item in allTagContainer)
        {
            Destroy(item.gameObject);
        }
        yield return new WaitForSeconds(0.5f);
        //FeedUIController.Instance.ApiLoaderScreen.SetActive(false);
        //FeedUIController.Instance.OnClickCheckOtherPlayerProfile();
        for (int i = 0; i < APIManager.Instance.taggedFeedsByUserIdRoot.data.rows.Count; i++)
        {
            GameObject userPostObject = Instantiate(photoPrefab, allTagContainer);
            //Debug.LogError("tagdata" + APIManager.Instance.taggedFeedsByUserIdRoot.data.rows[i]);
            UserPostItem userPostItem = userPostObject.GetComponent<UserPostItem>();
            userPostItem.tagUserData = APIManager.Instance.taggedFeedsByUserIdRoot.data.rows[i];

            FeedsByFollowingUser feedUserData = new FeedsByFollowingUser();
            feedUserData.Id = FeedRawData.id;
            feedUserData.Name = FeedRawData.name;
            feedUserData.Email = FeedRawData.email;
            feedUserData.Avatar = FeedRawData.avatar;
            userPostItem.feedUserData = feedUserData;

            userPostItem.avtarUrl = FeedRawData.avatar;
            userPostItem.LoadFeed();
        }
    }

    public void OnSetUserUi(bool isFollow)
    {
        if (isFollow)
        {
            followButtonImage.sprite = followingSprite;
            //followFollowingText.text = "Following";
            followFollowingText.text = TextLocalization.GetLocaliseTextByKey("Following");
            followFollowingText.color = FollowingTextColor;
        }
        else
        {
            followButtonImage.sprite = followSprite;
            //followFollowingText.text = "Follow";
            followFollowingText.text = TextLocalization.GetLocaliseTextByKey("Follow");
            followFollowingText.color = followTextColor;
        }
    }

    public void PrivatePublicTabSetup(bool isFollow)
    {
        if (isFollow)
        {
            tabPrivateObject.SetActive(false);
            tabPublicObject.SetActive(true);
        }
        else
        {
            tabPrivateObject.SetActive(true);
            tabPublicObject.SetActive(false);
        }
    }

    //this method is used to check and setup ui for Empty photo tab message.......
    public void SetupEmptyMsgForPhotoTab(bool isReset)
    {
        //check for photo.......
        if (allPhotoContainer.childCount > 0 || isReset)
        {
            allPhotoContainer.gameObject.SetActive(true);
            emptyPhotoPostMsgObj.SetActive(false);            
        }
        else
        {
            allPhotoContainer.gameObject.SetActive(false);
            emptyPhotoPostMsgObj.SetActive(true);            
        }

        //check for movie.......
        if(allMovieContainer.childCount > 0 || isReset)
        {
            allMovieContainer.gameObject.SetActive(true);
            emptyMoviePostMsgObj.SetActive(false);
        }
        else
        {
            allMovieContainer.gameObject.SetActive(false);
            emptyMoviePostMsgObj.SetActive(true);
        }

        //check for create first message.......
        if(allPhotoContainer.childCount > 0 || allMovieContainer.childCount > 0 || isReset)
        {
            createYourFirstPostMsgObj.SetActive(false);
            FooterCreateIcon.GetComponent<Animator>().enabled = false;
            FooterCreateIcon.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f,0f,0f);
        }
        else
        {
            createYourFirstPostMsgObj.SetActive(true);
            FooterCreateIcon.GetComponent<Animator>().enabled = true;
        }
    }

    //this method is used to check if my nft list is available the auto hide create first feed popup.......
    public void CheckAndDisableFirstFeedPopupForMyNFT(bool isMyNFTScreen)
    {
        //check for create first message.......
        if (isMyNFTScreen)
        {
            if (NftDataScript.Instance.ContentPanel.transform.childCount > 0)
            {
                createYourFirstPostMsgObj.SetActive(false);
                FooterCreateIcon.GetComponent<Animator>().enabled = false;
                FooterCreateIcon.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            if (allPhotoContainer.childCount > 0 || allMovieContainer.childCount > 0)
            {
                createYourFirstPostMsgObj.SetActive(false);
                FooterCreateIcon.GetComponent<Animator>().enabled = false;
                FooterCreateIcon.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                createYourFirstPostMsgObj.SetActive(true);
                FooterCreateIcon.GetComponent<Animator>().enabled = true;
            }
        }
    }

    //this method is used to Stop or play bottom create plus icon.......
    public void CreateFirstFeedPlusAnimStop(bool isDisableAnim)
    {
        if (myProfileScreen.activeSelf)
        {
            if (allPhotoContainer.childCount > 0 || allMovieContainer.childCount > 0 || isDisableAnim)
            {
                FooterCreateIcon.GetComponent<Animator>().enabled = false;
                FooterCreateIcon.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                FooterCreateIcon.GetComponent<Animator>().enabled = true;
            }
        }
    }

    public void OnClickOtherPalyerProfileBackButton()
    {
        FeedUIController.Instance.feedUiScreen.SetActive(true);
        FeedUIController.Instance.otherPlayerProfileScreen.SetActive(false);
    }

    public void OnClickFollowUserButton()
    {
        APIManager.Instance.RequestFollowAUser(FeedRawData.id.ToString(), "MyProfile");
    }

    //this method is used to Create post Button Click.......
    public void OnClickCreatePostButton()
    {
        FeedUIController.Instance.OnClickCreateFeedPickImageOrVideo();
    }

    //this method is used to website button click.......
    public void OnClickWebsiteButtonClick()
    {
        string websiteUrl = "";
        if (CheckUrlDropboxOrNot(websiteText.text))
        {
            websiteUrl = websiteText.text;
        }
        else
        {
            websiteUrl = String.Concat(defaultUrl + websiteText.text);//https://www.xana.net/
        }
        Debug.LogError("WebsiteURL:" + websiteUrl);
        Application.OpenURL(websiteUrl);
    }

    #region Photo, Movie, NFT Tab Methods.......
    //this method is used to Photo Tab button click.......
    public void OnClickPhotoTabButtonMain(int index)
    {
        selectionItemScript1.OnSelectedClick(index);
        TabCommonChange(index);

        NftDataScript.Instance.nftloading.SetActive(false);
        NftDataScript.Instance.ResetNftData();
        NftDataScript.Instance.NftLoadingPenal.SetActive(false);
    }
    public void OnClickPhotoTabButtonSub(int index)
    {
        selectionItemScript2.OnSelectedClick(index);
        TabCommonChange(index);

        NftDataScript.Instance.nftloading.SetActive(false);
        NftDataScript.Instance.ResetNftData();
        NftDataScript.Instance.NftLoadingPenal.SetActive(false);
    }

    //this method is used to Movie Tab button click.......
    public void OnClickMovieTabButtonMain(int index)
    {
        selectionItemScript1.OnSelectedClick(index);
        TabCommonChange(index);

        NftDataScript.Instance.nftloading.SetActive(false);
        NftDataScript.Instance.ResetNftData();
        NftDataScript.Instance.NftLoadingPenal.SetActive(false);
    }
    public void OnClickMovieTabButtonSub(int index)
    {
        selectionItemScript2.OnSelectedClick(index);
        TabCommonChange(index);

        NftDataScript.Instance.nftloading.SetActive(false);
        NftDataScript.Instance.ResetNftData();
        NftDataScript.Instance.NftLoadingPenal.SetActive(false);
    }
  

    //this method is used to NFT Tab button click.......
    public void OnClickNFTTabButtonMain(int index)
    {
        if (!PremiumUsersDetails.Instance.CheckSpecificItem("mynftbutton"))
        {
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }
        NftDataScript.Instance.NftLoadingPenal.SetActive(true);
        NftDataScript.Instance.currentSelection();
        selectionItemScript1.OnSelectedClick(index);
        TabCommonChange(index);
    }
    public void OnClickNFTTabButtonSub(int index)
    {
        if (!PremiumUsersDetails.Instance.CheckSpecificItem("mynftbutton"))
        {
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }

        NftDataScript.Instance.NftLoadingPenal.SetActive(true);
        NftDataScript.Instance.currentSelection();
        selectionItemScript2.OnSelectedClick(index);
        TabCommonChange(index);
    }

    void TabCommonChange(int index)
    {
        tabScrollRectGiftScreen.LerpToPage(index);
        parentHeightResetScript.OnHeightReset(index);

        if (index == 2)
        {
            CheckAndDisableFirstFeedPopupForMyNFT(true);//for Create First Feed Popup Auto hide if nft list available.......
        }
        else
        {
            CheckAndDisableFirstFeedPopupForMyNFT(false);//for Create First Feed Popup Auto hide if nft list available.......
        }
    }
    #endregion

    #region Edit Profile Methods.......

    //this method is used to edit profile button click
    public void OnClickEditProfileButton()
    {
        EditProfileDoneButtonSetUp(true);//setup edit profile done button.......
        editProfileScreen.SetActive(true);
        SetupEditProfileScreen();
    }

    void SetupEditProfileScreen()
    {
        editProfileImage.sprite = profileImage.sprite;
        editProfileNameInputfield.text = playerNameText.text;
        editProfileNameAdvanceInputfield.Text = playerNameText.text;
        if (myProfileData.userProfile != null)
        {
            //editProfileJobInputfield.text = myProfileData.userProfile.job;
            //editProfileJobAdvanceInputfield.Text = myProfileData.userProfile.job;
            editProfileJobAdvanceInputfield.Text = APIManager.DecodedString(myProfileData.userProfile.job); 
            //editProfileWebsiteInputfield.text = myProfileData.userProfile.website;
            editProfileWebsiteAdvanceInputfield.Text = myProfileData.userProfile.website;
            editProfileBioInputfield.text = APIManager.DecodedString(myProfileData.userProfile.bio);
            editProfileGenderInputfield.text = myProfileData.userProfile.gender;

            editProfileBioInputfield.transform.parent.GetComponent<InputFieldHightResetScript>().OnValueChangeAfterResetHeight();
        }
    }

    public string UTF32Str(string originalSTR)
    {
        // Create a UTF-32 encoding that supports a BOM.
        var enc = new UTF32Encoding();

        Console.WriteLine("Original string:");
        Console.WriteLine(originalSTR);

        // Encode the string.
        Byte[] encodedBytes = enc.GetBytes(originalSTR);
        Console.WriteLine("The encoded string has {0} bytes.\n",
                          encodedBytes.Length);

        // Write the bytes to a file with a BOM.
        var fs = new FileStream(@".\UTF32Encoding.txt", FileMode.Create);
        Byte[] bom = enc.GetPreamble();
        fs.Write(bom, 0, bom.Length);
        fs.Write(encodedBytes, 0, encodedBytes.Length);
        Console.WriteLine("Wrote {0} bytes to the file.\n", fs.Length);
        fs.Close();

        // Open the file using StreamReader.
        var sr = new StreamReader(@".\UTF32Encoding.txt");
        String newString = sr.ReadToEnd();
        sr.Close();
        Console.WriteLine("String read using StreamReader:");
        Console.WriteLine(newString);
        Console.WriteLine();

        // Open the file as a binary file and decode the bytes back to a string.
        fs = new FileStream(@".\Utf32Encoding.txt", FileMode.Open);
        Byte[] bytes = new Byte[fs.Length];
        fs.Read(bytes, 0, (int)fs.Length);
        fs.Close();

        String decodedString = enc.GetString(bytes);
        Console.WriteLine("Decoded bytes from binary file:");
        Console.WriteLine(decodedString);
        return decodedString;
    }    

    //this method used to Edit profile Back Button click.......
    public void OnClickEditProfileBackButton()
    {
        if (File.Exists(setImageAvatarTempPath))
        {
            File.Delete(setImageAvatarTempPath);
        }
        setImageAvatarTempPath = "";
        setImageAvatarTempFilename = "";
    }

    //this method is used to set edit profile done button interactable active or disable.......
    public void EditProfileDoneButtonSetUp(bool isActive)
    {
        editProfileDoneButton.interactable = isActive;
    }

    public int checkEditNameUpdated = 0;
    public int checkEditInfoUpdated = 0;
    string website = "";
    string job = "";
    string bio = "";
    string gender = "";
    string username = "";

    public void OnClickEditProfileDoneButton()
    {
        checkEditNameUpdated = 0;
        checkEditInfoUpdated = 0;

        username = playerNameText.text;
        job = "";
        gender = "";
        website = "";
        bio = "";

        if (myProfileData.userProfile != null)
        {
            //job = myProfileData.userProfile.job;
            job = APIManager.DecodedString(myProfileData.userProfile.job);
            website = myProfileData.userProfile.website;
            bio = APIManager.DecodedString(myProfileData.userProfile.bio);
            gender = myProfileData.userProfile.gender;
        }

        EditProfileDoneButtonSetUp(false);//setup edit profile done button.......

        if (!CheckForWebSite())
        {
            EditProfileInfoCheckAndAPICalling();
        }
    }

    void EditProfileInfoCheckAndAPICalling()
    {
        /*checkEditNameUpdated = 0;
        checkEditInfoUpdated = 0;

        string username = playerNameText.text;
        job = "";
        gender = "";
        website = "";
        bio = "";

        if (myProfileData.userProfile != null)
        {
            job = myProfileData.userProfile.job;
            website = myProfileData.userProfile.website;
            bio = myProfileData.userProfile.bio;
            gender = myProfileData.userProfile.gender;
        }*/

        //if (!string.IsNullOrEmpty(editProfileNameInputfield.text))
        if (!string.IsNullOrEmpty(editProfileNameAdvanceInputfield.Text))
        {
            //if (editProfileNameInputfield.text != playerNameText.text)
            if (editProfileNameAdvanceInputfield.Text != playerNameText.text)
            {
                //string tempStr = editProfileNameInputfield.text;
                string tempStr = editProfileNameAdvanceInputfield.Text;
                if (tempStr.StartsWith(" "))
                {
                    tempStr = tempStr.TrimStart(' ');
                }
                Debug.LogError("temp Name Str:" + tempStr);
                username = tempStr;
                checkEditNameUpdated = 1;
            }
        }
        else
        {
            Debug.LogError("Please enter username");
            EditProfileErrorMessageShow(nameErrorMessageObj);
            return;
        }

        //if (editProfileJobInputfield.text != job)
        if (editProfileJobAdvanceInputfield.Text != job)
        {
            //string tempStr = editProfileJobInputfield.text;
            string tempStr = editProfileJobAdvanceInputfield.RichText;
            if (tempStr.StartsWith(" "))
            {
                tempStr = tempStr.TrimStart(' ');
            }
            Debug.LogError("temp Job Str:" + tempStr);
            job = tempStr;
            checkEditInfoUpdated = 1;
        }
        else
        {
            //if (string.IsNullOrEmpty(editProfileJobInputfield.text))
            if (string.IsNullOrEmpty(editProfileJobAdvanceInputfield.Text))
            {
                job = "";
            }
        }

        //if (editProfileWebsiteInputfield.text != website)
        /*if (editProfileWebsiteAdvanceInputfield.Text != website)
        {
            //string tempStr = editProfileWebsiteInputfield.text;
            string tempStr = editProfileWebsiteAdvanceInputfield.Text;
            if (tempStr.StartsWith(" "))
            {
                tempStr = tempStr.TrimStart(' ');
            }
            Debug.LogError("temp Web Str:" + tempStr);
            website = tempStr;
            checkEditInfoUpdated = 1;

            if (!string.IsNullOrEmpty(tempStr))
            {
                //FeedUIController.Instance.ShowLoader(true);
                //editProfileDoneButton.interactable = false;

                string webUrl = tempStr;
                bool isUrl = false;
                if (!CheckUrlDropboxOrNot(webUrl))
                {
                    webUrl = String.Concat(defaultUrl + tempStr);
                }
                else
                {
                    isUrl = true;
                }

                Debug.LogError("WebUrl:" + webUrl + "  :isUrl:" + isUrl);

                if (!IsReachableUri(webUrl) || webUrl.Contains("@"))
                {
                    Debug.LogError("Please enter valid web");
                    //FeedUIController.Instance.ShowLoader(false);
                    //websiteErrorObj.GetComponent<Animator>().SetBool("playAnim", true);
                    if (webErrorCo != null)
                    {
                        StopCoroutine(webErrorCo);
                        websiteErrorObj.SetActive(false);
                    }
                    websiteErrorObj.SetActive(true);
                    webErrorCo = StartCoroutine(WaitUntilErrorAnimationFinished(websiteErrorObj.GetComponent<Animator>()));                    
                    return;
                }
                else
                {                    
                    editProfileDoneButton.interactable = true;
                    if (isUrl)
                    {
                        Uri myUri = new Uri(tempStr);
                        website = myUri.Host;
                        Debug.LogError("temp Web Str111:" + website);
                    }
                }
            }

            //website = editProfileWebsiteInputfield.text;
            //checkEditInfoUpdated = 1;
        }
        else
        {
            //if (string.IsNullOrEmpty(editProfileWebsiteInputfield.text))
            if (string.IsNullOrEmpty(editProfileWebsiteAdvanceInputfield.Text))
            {
                website = "";
            }
        }*/

        if (editProfileBioInputfield.text != bio)
        {
            string tempStr = editProfileBioInputfield.text;
            if (tempStr.StartsWith(" "))
            {
                tempStr = tempStr.TrimStart(' ');
            }
            Debug.LogError("temp Bio Str:" + tempStr);
            bio = tempStr;
            checkEditInfoUpdated = 1;
        }
        else
        {
            if (string.IsNullOrEmpty(editProfileBioInputfield.text))
            {
                bio = "";
            }
        }

        if (!string.IsNullOrEmpty(editProfileGenderInputfield.text))
        {
            if (editProfileGenderInputfield.text != gender)
            {
                gender = editProfileGenderInputfield.text;
                checkEditInfoUpdated = 1;
            }
        }

        //editProfileScreen.SetActive(false);//Flase edit profile screen

        if (checkEditNameUpdated == 1)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = "";
            }
            APIManager.Instance.RequestSetName(username);
        }

        if (checkEditInfoUpdated == 1)
        {
            string countryName = System.Globalization.RegionInfo.CurrentRegion.EnglishName;
            Debug.LogError("User Ingo Name:" + username + "   :job:" + job + "    :website:" + website + "    :bio:" + bio + "  :Gender:" + gender + "  :Country:" + countryName);

            if (string.IsNullOrEmpty(job))
            {
                job = "";
            }
            if (string.IsNullOrEmpty(website))
            {
                website = "";
            }
            if (string.IsNullOrEmpty(bio))
            {
                bio = "";
            }
            if (string.IsNullOrEmpty(gender))
            {
                gender = "Male";
                Debug.LogError("Default Gender:" + gender);
            }
            if (string.IsNullOrEmpty(countryName))
            {
                countryName = "";
            }

            APIManager.Instance.RequestUpdateUserProfile(gender, APIManager.EncodedString(job), countryName, website, APIManager.EncodedString(bio));
        }

        if (string.IsNullOrEmpty(setImageAvatarTempPath))
        {
            if (checkEditNameUpdated == 1 || checkEditInfoUpdated == 1)
            {
                Debug.LogError("EditProfileInfoCheckAndAPICalling Get User Details API Call");
                StartCoroutine(WaitEditProfileGetUserDetails(false));
            }
            else
            {
                editProfileScreen.SetActive(false);
                EditProfileDoneButtonSetUp(true);//setup edit profile done button.......
            }
        }
        else
        {
            StartCoroutine(WaitEditProfileGetUserDetails(true));
        }
    }

    //this method is return value with url is valid or not 
    public bool IsReachableUri(string url)
    {
        HttpWebRequest request;
        try
        {
            request = (HttpWebRequest)WebRequest.Create(url);
        }
        catch (Exception e)
        {
            Debug.LogError("isreachableUri ecception:" + e);
            return false;
        }
        request.Timeout = 2500;
        request.Method = "HEAD"; // As per Lasse's comment
        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
        catch (WebException)
        {
            return false;
        }
    }

    public bool IsValidMail(string emailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailaddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    bool isUrl = false;
    public bool CheckForWebSite()
    {
        if (editProfileWebsiteAdvanceInputfield.Text != website)
        {
            //string tempStr = editProfileWebsiteInputfield.text;
            string tempStr = editProfileWebsiteAdvanceInputfield.Text;
            if (tempStr.StartsWith(" "))
            {
                tempStr = tempStr.TrimStart(' ');
            }
            Debug.LogError("temp Web Str:" + tempStr);
            website = tempStr;
            checkEditInfoUpdated = 1;

            if (!string.IsNullOrEmpty(website))
            {
                isUrl = false;
                string webUrl = website;
                if (!CheckUrlDropboxOrNot(webUrl))
                {
                    // webUrl = String.Concat(defaultUrl + tempStr);
                }
                else
                {
                    isUrl = true;
                }

                FeedUIController.Instance.ShowLoader(true);
                RequestForWebSiteValidation(webUrl);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            //if (string.IsNullOrEmpty(editProfileWebsiteInputfield.text))
            if (string.IsNullOrEmpty(editProfileWebsiteAdvanceInputfield.Text))
            {
                website = "";
            }
            return false;
        }
    }

    Coroutine webValidCo;
    public void RequestForWebSiteValidation(string url)
    {
        if (webValidCo != null)
        {
            StopCoroutine(webValidCo);
        }
        webValidCo = StartCoroutine(IERequestForWebSiteValidation(url));
    }
    public IEnumerator IERequestForWebSiteValidation(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("url", url);
        Debug.LogError("Web URL:" + url);
        using (UnityWebRequest www = UnityWebRequest.Post((ConstantsGod.API_BASEURL + ConstantsGod.r_url_WebsiteValidation), form))
        {
            yield return www.SendWebRequest();

            FeedUIController.Instance.ShowLoader(false);

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                EditProfileErrorMessageShow(websiteErrorObj);
                Debug.LogError("Invalid WebSite");
            }
            else
            {
                string data = www.downloadHandler.text;
                Debug.Log("Website Validation success data:" + data);
                WebSiteValidRoot webSiteValidRoot = JsonConvert.DeserializeObject<WebSiteValidRoot>(data);
                if (webSiteValidRoot.success)
                {
                    if (isUrl)
                    {
                        Uri myUri = new Uri(website);
                        website = myUri.Host;
                    }
                    Debug.LogError("final result Web Str:" + website);

                    EditProfileInfoCheckAndAPICalling();
                    Debug.Log("Valid WebSite:");
                }
                else
                {
                    EditProfileErrorMessageShow(websiteErrorObj);
                    Debug.LogError("Invalid WebSite");
                }
            }
        }
    }

    //this method is used to show web site error message.......
    void EditProfileErrorMessageShow(GameObject currentOBJ)
    {
        if (editProfileErrorCo != null)
        {
            StopCoroutine(editProfileErrorCo);
            currentEditProfileErrorMessgaeObj.SetActive(false);
        }
        //websiteErrorObj.SetActive(true);
        currentOBJ.SetActive(true);
        currentEditProfileErrorMessgaeObj = currentOBJ;
        editProfileErrorCo = StartCoroutine(WaitUntilErrorAnimationFinished());
    }

    Coroutine editProfileErrorCo;
    GameObject currentEditProfileErrorMessgaeObj;
    //this coroutine is used to show and wait until finish error message animation.......
    IEnumerator WaitUntilErrorAnimationFinished()
    {
        yield return new WaitForSeconds(0.5f);
        EditProfileDoneButtonSetUp(true);//setup edit profile done button.......
        yield return new WaitForSeconds(1f);
        //MyAnim.SetBool("playAnim", false);
        //FeedUIController.Instance.ShowLoader(false);
        currentEditProfileErrorMessgaeObj.SetActive(false);
    }

    //this coroutine is used check and GetUserDetails or UploadAvatar api call.......
    IEnumerator WaitEditProfileGetUserDetails(bool isProfileUpdate)
    {
        FeedUIController.Instance.ShowLoader(true);
        yield return new WaitForSeconds(1f);
        if (!isProfileUpdate)
        {
            APIManager.Instance.RequestGetUserDetails("EditProfileAvatar");//Get My Profile data 
        }
        else
        {
            Debug.LogError("Uploading profile pic :" + setImageAvatarTempPath);
            AWSHandler.Instance.PostAvatarObject(setImageAvatarTempPath, setImageAvatarTempFilename, "EditProfileAvatar");//upload avatar image on AWS.
        }
    }

    //this method is used to Bio Button click.......
    public void OnEditBioButtonClick()
    {
        editProfileBioScreen.SetActive(true);
        //bioEditInputField.text = editProfileBioInputfield.text;

        bioEditAdvanceInputField.Text = editProfileBioInputfield.text;

        bioEditInputField.transform.parent.GetComponent<InputFieldHightResetScript>().OnValueChangeAndResetNormalInputField();
        //bioEditInputField.transform.parent.GetComponent<InputFieldHightResetScript>().OnValueChangeAfterResetHeight();
    }

    //this method is used to edit bio Back Button click.......
    public void OnEditBioBackButtonClick()
    {
        if (editProfileErrorCo != null)
        {
            StopCoroutine(editProfileErrorCo);
            currentEditProfileErrorMessgaeObj.SetActive(false);
        }
        editProfileBioScreen.SetActive(false);
    }

    //this method is used to edit bio Done Button click.......
    public void OnEditBioDoneButtonClick()
    {
        if (editProfileErrorCo != null)
        {
            StopCoroutine(editProfileErrorCo);
            currentEditProfileErrorMessgaeObj.SetActive(false);
        }
        editProfileBioScreen.SetActive(false);

        //string resultString = Regex.Replace(bioEditInputField.text.ToString(), @"^\s*$[\r\n]*", string.Empty, RegexOptions.Multiline);
        string resultString = Regex.Replace(bioEditAdvanceInputField.RichText.ToString(), @"^\s*$[\r\n]*", string.Empty, RegexOptions.Multiline);

        editProfileBioInputfield.text = resultString;
        //editProfileBioInputfield.text = bioEditInputField.text;
        editProfileBioInputfield.transform.parent.GetComponent<InputFieldHightResetScript>().OnValueChangeAfterResetHeight();
    }   

    //this method is used to change Profile Button click.......
    public void OnClickChangeProfilePicButton()
    {
        pickImageOptionScreen.SetActive(true);
    }

    public string setImageAvatarTempPath = "";
    public string setImageAvatarTempFilename = "";
   
    //public bool setGroupFromCamera = false;
    //[Space]
    //public Texture2D setGroupTempAvatarTexture;

    //this method is used to pick group avatar from gellery for group avatar.
    public void OnPickImageFromGellery(int maxSize)
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
          setImageAvatarTempPath = "";
        setImageAvatarTempFilename = "";
        //setGroupFromCamera = false;

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                if (pickImageOptionScreen.activeSelf)//false meadia option screen.
                {
                    pickImageOptionScreen.SetActive(false);
                }

                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                //setGroupTempAvatarTexture = texture;

                Debug.LogError("OnPickGroupAvatarFromGellery path: " + path);

                string[] pathArry = path.Split('/');

                //string fileName = pathArry[pathArry.Length - 1];
                string fileName = Path.GetFileName(path);
                Debug.LogError("OnPickGroupAvatarFromGellery FileName: " + fileName);

                string[] fileNameArray = fileName.Split('.');
                string str = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".";
                fileName = fileNameArray[0] + str + fileNameArray[1];

                setImageAvatarTempPath = Path.Combine(Application.persistentDataPath, "XanaChat", fileName); ;
                setImageAvatarTempFilename = fileName;

                Crop(texture, setImageAvatarTempPath);

                //editProfileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        });
        Debug.Log("Permission result: " + permission);
       
#elif UNITY_ANDROID
        setImageAvatarTempPath = "";
        setImageAvatarTempFilename = "";
        //setGroupFromCamera = false;

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                if (pickImageOptionScreen.activeSelf)//false meadia option screen.
                {
                    pickImageOptionScreen.SetActive(false);
                }

                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                //setGroupTempAvatarTexture = texture;

                Debug.LogError("OnPickGroupAvatarFromGellery path: " + path);

                string[] pathArry = path.Split('/');

                //string fileName = pathArry[pathArry.Length - 1];
                string fileName = Path.GetFileName(path);
                Debug.LogError("OnPickGroupAvatarFromGellery FileName: " + fileName);

                string[] fileNameArray = fileName.Split('.');
                string str = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".";
                fileName = fileNameArray[0] + str + fileNameArray[1];

                setImageAvatarTempPath = Path.Combine(Application.persistentDataPath, "XanaChat", fileName); ;
                setImageAvatarTempFilename = fileName;

                Crop(texture, setImageAvatarTempPath);

                //editProfileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        });

        if (permission != NativeGallery.Permission.Granted)
        {
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
        Debug.Log("Permission result: " + permission);
#endif
        //OnPickProfileImageFromGellery(maxSize);
    }

    //this method is used to take picture from camera for group avatar.
    public void OnPickImageFromCamera(int maxSize)
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
         setImageAvatarTempPath = "";
        setImageAvatarTempFilename = "";
        //setGroupFromCamera = false;
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                if (pickImageOptionScreen.activeSelf)//false meadia option screen.
                {
                    pickImageOptionScreen.SetActive(false);
                }
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                //setGroupTempAvatarTexture = texture;

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

                setImageAvatarTempPath = filePath;
                setImageAvatarTempFilename = fileName;
                //setGroupFromCamera = true;

                Crop(texture, setImageAvatarTempPath);
                //editProfileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        }, maxSize);
         
#elif UNITY_ANDROID
        setImageAvatarTempPath = "";
        setImageAvatarTempFilename = "";
        //setGroupFromCamera = false;
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                if (pickImageOptionScreen.activeSelf)//false meadia option screen.
                {
                    pickImageOptionScreen.SetActive(false);
                }
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                //setGroupTempAvatarTexture = texture;

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

                setImageAvatarTempPath = filePath;
                setImageAvatarTempFilename = fileName;
                //setGroupFromCamera = true;

                Crop(texture, setImageAvatarTempPath);
                //editProfileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        }, maxSize);

        if (permission != NativeCamera.Permission.Granted)
        {
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
        Debug.Log("Permission result: " + permission);
#endif
    }
#endregion
    
#region Get Feed By User Id
    /*public void RequestGetFeedsByUserId(int userId, int pageNum, int pageSize)
    {
        FeedUIController.Instance.ShowLoader(true);
        StartCoroutine(IERequestGetFeedsByUserId(userId, pageNum, pageSize));
    }
    public IEnumerator IERequestGetFeedsByUserId(int userId, int pageNum, int pageSize)
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get((APIManager.Instance.mainURL + APIManager.Instance.url_GetFeedsByUserId + "/" + userId + "/" + pageNum + "/" + pageSize)))
        {
            www.SetRequestHeader("Authorization", APIManager.Instance.userAuthorizeToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                Debug.LogError("my profile feed data success:" + data);
                allFeedWithUserIdRoot = JsonConvert.DeserializeObject<AllFeedByUserIdRoot>(data);
                StartCoroutine(AllFeedWithUserId());
                // Debug.LogError("data" + allFeedWithUserIdRoot.Success);
            }
        }
    }*/
#endregion

#region Get Image From AWS
    public void GetImageFromAWS(string key, Image mainImage)
    {
        //Debug.LogError("My Profile GetImageFromAWS key:" + key);
        if (AssetCache.Instance.HasFile(key))
        {
            //Debug.LogError("Image Available on Disk");
            AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
            return;
        }
        else
        {
            AssetCache.Instance.EnqueueOneResAndWait(key, (ConstantsGod.r_AWSImageKitBaseUrl + key), (success) =>
            {
                if (success)
                {
                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                }
            });
            return;

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
                        if (currentExtention == ExtentionType.Image)
                        {
                            Texture2D texture = new Texture2D(2, 2);
                            texture.LoadImage(data);
                            //Debug.LogError("key " + key + " :texture width:" + texture.width + "  height:" + texture.height);

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
        Debug.LogError("ExtentionType: " + extension);
        if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "bmp" || extension == "tiff" || extension == "heic")
        {
            currentExtention = ExtentionType.Image;
            return ExtentionType.Image;
        }
        else if (extension == "mp4" || extension == "mov" || extension == "wav" || extension == "avi")
        {
            currentExtention = ExtentionType.Video;
            // Debug.LogError("vvvvvvvvvvvvvvvvvvvvvvvvvvvv");
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

    public string GetWebDomainFromUrl(string url)
    {
        string[] splitArray = url.Split(':');
        if (splitArray.Length > 0)
        {
            if (splitArray[0] == "https" || splitArray[0] == "http")
            {
                return splitArray[1];
            }
        }
        return url;
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
                editProfileImage.sprite = s;

                try
                {
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
            Destroy(screenshot);
        },
        settings: new ImageCropper.Settings()
        {
            ovalSelection = ovalSelection,
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

#region Profile Follower and Following list Screen Methods
    //this method is used to profile follower button click.......
    public void OnClickFollowerButton()
    {
        FeedUIController.Instance.ProfileFollowerFollowingScreenSetup(0, topHaderUserNameText.text);

        if (APIManager.Instance.profileAllFollowerRoot.data.rows.Count != myProfileData.followerCount)
        {
            FeedUIController.Instance.ProfileFollowerFollowingListClear();

            //FeedUIController.Instance.ShowLoader(true);
            FeedUIController.Instance.isProfileFollowerDataLoaded = false;
            APIManager.Instance.RequestGetAllFollowersFromProfile(myProfileData.id.ToString(), 1, 50);

            if (followingCo != null)
            {
                StopCoroutine(followingCo);
            }
            followingCo = StartCoroutine(WaitToCallFollowing());
        }
    }

    Coroutine followingCo;
    IEnumerator WaitToCallFollowing()
    {
        yield return new WaitForSeconds(1f);
        FeedUIController.Instance.isProfileFollowingDataLoaded = false;
        APIManager.Instance.RequestGetAllFollowingFromProfile(myProfileData.id.ToString(), 1, 50);
    }

    //this method is used to profile Following button click.......
    public void OnClickFollowingButtton()
    {
        FeedUIController.Instance.ProfileFollowerFollowingScreenSetup(1, topHaderUserNameText.text);

        if (APIManager.Instance.profileAllFollowingRoot.data.rows.Count != myProfileData.followingCount)
        {
            FeedUIController.Instance.ProfileFollowerFollowingListClear();

            //FeedUIController.Instance.ShowLoader(true);
            FeedUIController.Instance.isProfileFollowingDataLoaded = false;
            APIManager.Instance.RequestGetAllFollowingFromProfile(myProfileData.id.ToString(), 1, 50);

            if (followeCo != null)
            {
                StopCoroutine(followeCo);
            }
            followeCo = StartCoroutine(WaitToFollower());
        }
    }

    Coroutine followeCo;
    IEnumerator WaitToFollower()
    {
        yield return new WaitForSeconds(1f);
        FeedUIController.Instance.isProfileFollowerDataLoaded = false;
        APIManager.Instance.RequestGetAllFollowersFromProfile(myProfileData.id.ToString(), 1, 50);
    }
#endregion

#region my profile Data API
    public GetUserDetailRoot tempMyProfileDataRoot = new GetUserDetailRoot();
    public void RequestGetUserDetails()
    {
        StartCoroutine(IERequestGetUserDetails());
    }
    public IEnumerator IERequestGetUserDetails()
    {
        WWWForm form = new WWWForm();

        //   form.AddField("name", setName_name);

        using (UnityWebRequest www = UnityWebRequest.Get((ConstantsGod.API_BASEURL + ConstantsGod.r_url_GetUserDetails)))
        {
            www.SetRequestHeader("Authorization", APIManager.Instance.userAuthorizeToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("IERequestGetUserDetails error:" + www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                Debug.LogError("IERequestGetUserDetails Loaded Completed data:" + data);
                tempMyProfileDataRoot = JsonUtility.FromJson<GetUserDetailRoot>(data);

                myProfileData = tempMyProfileDataRoot.data;
                OnlyLoadDataMyProfile();//set data                
            }
        }
    }

    public void OnlyLoadDataMyProfile()
    {
        totalFollowerText.text = myProfileData.followerCount.ToString();
        totalFollowingText.text = myProfileData.followingCount.ToString();
        totalPostText.text = myProfileData.feedCount.ToString();
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
        Debug.Log ("CAMERA is NOT permitted and checked never ask again option");

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