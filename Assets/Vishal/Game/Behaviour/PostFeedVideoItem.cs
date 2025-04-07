﻿using Amazon.S3.Model;
using RenderHeads.Media.AVProVideo;
using SuperStar.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostFeedVideoItem : MonoBehaviour
{
    public AllFeedByUserIdRow userData;
    public TaggedFeedsByUserIdRow tagUserData;
    public FeedsByFollowingUser feedUserData;
    //  public FeedsByFollowingUserRow FollowingUserFeedData;

    //  public AllUserWithFeedRow FeedRawData;

    public Image imgFeed, profileImage;
    public GameObject PhotoImage;

    public MediaPlayer feedMediaPlayer;
    public GameObject videoDisplay;

    public TextMeshProUGUI userName;
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI timeText;
    public GameObject commentBtn;
    public GameObject likeCountBtn;
    public Color likeSelectionColor;
    public GameObject optionButton;

    public string avatarUrl;

    [Space]
    public bool isVideoOrImage = false;
    public bool isImageSuccessDownloadAndSave = false;
    public bool isReleaseFromMemoryOrNot = false;
    public bool isOnScreen = false;//check object is on screen or not
    public bool isPrintDebug = false;//for testing

    public bool isVisible = false;
    float lastUpdateCallTime;   

    private void Start()
    {
        float value0 = FeedUIController.Instance.feedVideoButtonPanelImage.rect.width;
        float value1 = FeedUIController.Instance.feedVideoButtonPanelImage.rect.height;
        // Debug.LogError("height : " + Screen.height + "width " + (Screen.height - 150) + ":value:" + value1);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(value0, value1);

        if (FeedUIController.Instance.feedFullViewScreenCallingFrom == "MyProfile")
        {
            optionButton.SetActive(true);
        }
        else
        {
            optionButton.SetActive(false);
        }
    }

    private void OnEnable()
    {        
        RefreshDecTextContentSizeFitter();
    }

    private void OnDisable()
    {
        if (!isVideoOrImage)
        {
            AssetCache.Instance.RemoveFromMemory(imgFeed.sprite);
            imgFeed.sprite = null;
        }
        else
        {
            feedMediaPlayer.CloseMedia();
        }
        if (!string.IsNullOrEmpty(avatarUrl))
        {
            AssetCache.Instance.RemoveFromMemory(profileImage.sprite);
            profileImage.sprite = null;
        }
        //Resources.UnloadUnusedAssets();//every clear.......
        //Caching.ClearCache();
        APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
    }

    private void Update()//delete image after object out of screen
    {
        if (APIManager.Instance.isTestDefaultToken)//for direct SNS Scene Test....... 
        {
            return;
        }

        lastUpdateCallTime += Time.deltaTime;
        if (lastUpdateCallTime > 0.3f)//call every 0.4 sec
        {
            Vector3 mousePosNormal = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Vector3 mousePosNR = Camera.main.ScreenToViewportPoint(mousePosNormal);
            if (isPrintDebug)
            {
                Debug.LogError("NormalPos:" + mousePosNR);
            }
            if (mousePosNR.y >= -0.4f && mousePosNR.y <= 1.4f)
            {
                isOnScreen = true;
            }
            else
            {
                isOnScreen = false;
            }

            lastUpdateCallTime = 0;
        }

        if (isVisible && isOnScreen)//this is check if object is visible on camera then load feed or video one time
        {
            isVisible = false;
            //Debug.LogError("Image download starting one time");
            DownloadAndLoadFeed();
        }
        else if (isImageSuccessDownloadAndSave)
        {
            if (isOnScreen)
            {
                if (isReleaseFromMemoryOrNot)
                {
                    isReleaseFromMemoryOrNot = false;
                    //re load from asset 
                    //Debug.LogError("Re Download Image and play video");
                    if (!isVideoOrImage)//if false then load image otherwise play video
                    {
                        if (AssetCache.Instance.HasFile(userData.Image))
                        {
                            AssetCache.Instance.LoadSpriteIntoImage(imgFeed, userData.Image, changeAspectRatio: true);
                        }
                        if (!string.IsNullOrEmpty(avatarUrl))
                        {
                            if (AssetCache.Instance.HasFile(avatarUrl))
                            {
                                AssetCache.Instance.LoadSpriteIntoImage(profileImage, avatarUrl, changeAspectRatio: true);
                            }
                        }
                    }
                    else
                    {
                        feedMediaPlayer.Play();
                    }
                }
            }
            else
            {
                if (!isReleaseFromMemoryOrNot)
                {
                    //realse from memory 
                    isReleaseFromMemoryOrNot = true;
                    //Debug.LogError("remove from memory and stop video");
                    if (!isVideoOrImage)//if false then Remove image otherwise Stop video
                    {
                        AssetCache.Instance.RemoveFromMemory(imgFeed.sprite);
                        imgFeed.sprite = null;
                        if (!string.IsNullOrEmpty(avatarUrl))
                        {
                            AssetCache.Instance.RemoveFromMemory(profileImage.sprite);
                            profileImage.sprite = null;
                        }
                        Resources.UnloadUnusedAssets();//Every clear
                        Caching.ClearCache();
                        GC.Collect();
                        //APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
                    }
                    else
                    {
                        feedMediaPlayer.Pause();
                    }
                }
            }
        }
    }

    public void LoadFeed()
    {
        userName.text = "";
        timeText.text = "";
        if (userData.Id != 0)
        {
            userName.text = APIManager.DecodedString(userData.Title);

            //for comment count.......
            CommentCountUISetup(userData.commentCount);

            //for like count.......
            LikeCountAndUISetup(userData.LikeCount);

            if (!string.IsNullOrEmpty(userData.Descriptions))//new
            {
                descriptionText.text = APIManager.DecodedString(userData.Descriptions);
                SetupDecPart(descriptionText.text);
            }
            else
            {
                descriptionText.text = "";
                seeMoreDecObj.SetActive(false);
            }

            if (userData.UpdatedAt != null)
            {
                timeText.text = FeedUIController.Instance.GetConvertedTimeString(userData.UpdatedAt);
            }
            //feedTitle.text = FeedData.title;
            //feedLike.text = FeedData.likeCount.ToString();
        }
        else if (tagUserData.id != 0)
        {
            //userName.text = tagUserData.feed.title;
            //commentBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tagUserData.feed.commentCount.ToString();
            if (!string.IsNullOrEmpty(userData.Descriptions))//new
            {
                descriptionText.text = APIManager.DecodedString(tagUserData.feed.descriptions);
                SetupDecPart(descriptionText.text);
            }
            else
            {
                descriptionText.text = "";
                seeMoreDecObj.SetActive(false);
            }
            //feedTitle.text = tagUserData.feed.title;
            //feedLike.text = FeedData.likeCount.ToString();
        }

        Invoke("UserNameSizeSetUp", 0.2f);

        GetShareURL();

        if (!APIManager.Instance.isTestDefaultToken)
        {
            isVisible = true;
        }
        else
        {
            DownloadAndLoadFeed();
        }
    }

    void UserNameSizeSetUp()
    {
        if (userName.GetComponent<RectTransform>().sizeDelta.x > 550f)
        {
            userName.GetComponent<LayoutElement>().preferredWidth = 550f;
        }
    }

    void DownloadAndLoadFeed()
    {
        if (!string.IsNullOrEmpty(avatarUrl))//set avatar image.......
        {
            bool isAvatarUrlFromDropbox = CheckUrlDropboxOrNot(avatarUrl);
            //Debug.LogError("isAvatarUrlFromDropbox: " + isAvatarUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);

            if (isAvatarUrlFromDropbox)
            {
                AssetCache.Instance.EnqueueOneResAndWait(avatarUrl, avatarUrl, (success) =>
                {
                    if (success)
                    {
                        AssetCache.Instance.LoadSpriteIntoImage(profileImage, avatarUrl, changeAspectRatio: true);
                    }
                });
            }
            else
            {
                GetImageFromAWS(avatarUrl, profileImage);//Get image from aws and save/load into asset cache.......
            }
        }

        if (userData.Id != 0)
        {
            /*userName.text = userData.Title;

            if (!string.IsNullOrEmpty(userData.Descriptions))//new
            {
                descriptionText.text = userData.Descriptions;
                SetupDecPart(descriptionText.text);
            }
            else
            {
                descriptionText.text = "";
                seeMoreDecObj.SetActive(false);
            }

            if (userData.UpdatedAt != null)
            {
                timeText.text = FeedUIController.Instance.GetConvertedTimeString(userData.UpdatedAt);
            }*/
            //feedTitle.text = FeedData.title;
            //feedLike.text = FeedData.likeCount.ToString();

            if (!string.IsNullOrEmpty(userData.Image))
            {
                bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(userData.Image);
                //Debug.LogError("isImageUrlFromDropbox:  " + isImageUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);
                if (isImageUrlFromDropbox)
                {
                    AssetCache.Instance.EnqueueOneResAndWait(userData.Image, userData.Image, (success) =>
                    {
                        if (success)
                        {
                            AssetCache.Instance.LoadSpriteIntoImage(imgFeed, userData.Image, changeAspectRatio: true);
                            isImageSuccessDownloadAndSave = true;
                        }
                    });
                }
                else
                {
                    GetImageFromAWS(userData.Image, imgFeed);//Get image from aws and save/load into asset cache.......
                }

                SetVideoUi(false);
            }
            else if (!string.IsNullOrEmpty(userData.Video))
            {
                bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(userData.Video);
                isVideoOrImage = true;

                // Debug.LogError("FeedData.video " + userData.Image);
                SetVideoUi(true);

                if (isVideoUrlFromDropbox)
                {
                    bool videoPlay = feedMediaPlayer.OpenMedia(new MediaPath(userData.Video, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                    //feedMediaPlayer.Play();
                    if (videoPlay)
                    {
                        isImageSuccessDownloadAndSave = true;
                    }
                }
                else
                {
                    GetVideoUrl(userData.Video);//get video url from aws and play.......
                }
            }
        }
        else if (tagUserData.id != 0)
        {
            //userName.text = tagUserData.feed.title;
            /*if (!string.IsNullOrEmpty(userData.Descriptions))//new
            {
                descriptionText.text = tagUserData.feed.descriptions;
                SetupDecPart(descriptionText.text);
            }
            else
            {
                descriptionText.text = "";
                seeMoreDecObj.SetActive(false);
            }*/
            
            //feedTitle.text = tagUserData.feed.title;
            //feedLike.text = FeedData.likeCount.ToString();

            if (!string.IsNullOrEmpty(tagUserData.feed.image))
            {
                bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(tagUserData.feed.image);
                //Debug.LogError("isImageUrlFromDropbox:  " + isImageUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);
                if (isImageUrlFromDropbox)
                {
                    AssetCache.Instance.EnqueueOneResAndWait(tagUserData.feed.image, tagUserData.feed.image, (success) =>
                    {
                        if (success)
                        {
                            AssetCache.Instance.LoadSpriteIntoImage(imgFeed, tagUserData.feed.image, changeAspectRatio: true);
                            isImageSuccessDownloadAndSave = true;
                        }
                    });
                }
                else
                {
                    GetImageFromAWS(tagUserData.feed.image, imgFeed);//Get image from aws and save/load into asset cache.......
                }

                SetVideoUi(false);
            }
            else if (!string.IsNullOrEmpty(tagUserData.feed.video))
            {
                bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(tagUserData.feed.video);
                isVideoOrImage = true;

                SetVideoUi(true);

                if (isVideoUrlFromDropbox)
                {
                    bool videoPlay = feedMediaPlayer.OpenMedia(new MediaPath(tagUserData.feed.video, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                    //feedMediaPlayer.Play();
                    if (videoPlay)
                    {
                        isImageSuccessDownloadAndSave = true;
                    }
                }
                else
                {
                    GetVideoUrl(tagUserData.feed.video);//get video url from aws and play.......
                }
                // Debug.LogError("FeedData.video " + userData.Image);
            }
        }
    }

    public void SetVideoUi(bool isVideo)
    {
        if (isVideo)
        {
            PhotoImage.gameObject.SetActive(false);
            videoDisplay.gameObject.SetActive(true);
            feedMediaPlayer.gameObject.SetActive(true);
        }
        else
        {
            PhotoImage.gameObject.SetActive(true);
            videoDisplay.gameObject.SetActive(false);
            feedMediaPlayer.gameObject.SetActive(false);
        }
    }

    //this method is used to user profile button click.......
    public void OnClickPlayerProfileButton()
    {
        Debug.LogError("Feed Id :" + userData.Id + "    :User Id:" + feedUserData.Id);

        FeedUIController.Instance.OnClickVideoItemBackButton();//this method close feed full screen and back to myprofile or other player profile screen.......

        //this api get any user profile data and feed for other player profile....... 
        //OtherPlayerProfileData.Instance.RequestGetUserDetails(feedUserData.Id);
    }

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
                    if (this.isActiveAndEnabled)
                    {
                        //Debug.LogError("Feed Video URL " + mediaUrl);
                        bool videoPlay = feedMediaPlayer.OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                        //feedMediaPlayer.Play();
                        if (videoPlay)
                        {
                            isImageSuccessDownloadAndSave = true;
                        }
                    }
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
            if (mainImage == imgFeed)
            {
                isImageSuccessDownloadAndSave = true;
            }
            return;
        }
        else
        {
            AssetCache.Instance.EnqueueOneResAndWait(key, (ConstantsGod.r_AWSImageKitBaseUrl + key), (success) =>
            {
                if (success)
                {
                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                    if (mainImage == imgFeed)
                    {
                        isImageSuccessDownloadAndSave = true;
                    }
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
                                    //Debug.LogError("Save on Aws sucess");
                                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                                    if (mainImage == imgFeed)
                                    {
                                        isImageSuccessDownloadAndSave = true;
                                    }
                                }
                            });
                            //mainImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                        }
                        data = null;
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

    #region See more and lee dec method and reference
    [Space]
    [Header("See More Dec Reference")]
    public GameObject seeMoreDecObj;
    public GameObject seeMoreButtonTextObj;
    public GameObject seeLessButtonTextObj;

    public ContentSizeFitter userDetailsContentSizeFitter;

    public void OnClickDecSeeMoreLessButton()
    {
        if (seeMoreButtonTextObj.activeSelf)
        {
            if (userData.Id != 0)
            {
                descriptionText.text = APIManager.DecodedString(userData.Descriptions);
            }
            else if (tagUserData.id != 0)
            {
                descriptionText.text = APIManager.DecodedString(tagUserData.feed.descriptions);
            }
            SeeMoreLessBioTextSetup(false);
        }
        else
        {
            descriptionText.text = tempMinDecLineStr;
            SeeMoreLessBioTextSetup(true);
        }

        RefreshDecTextContentSizeFitter();
    }

    public void RefreshDecTextContentSizeFitter()
    {
        if (waitToRefreshCo != null)
        {
            StopCoroutine(waitToRefreshCo);
        }

        waitToRefreshCo = StartCoroutine(WaitToRefreshUserDetails());
    }

    Coroutine waitToRefreshCo;
    IEnumerator WaitToRefreshUserDetails()
    {
        userDetailsContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.01f);
        userDetailsContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    void SeeMoreLessBioTextSetup(bool isSeeMore)
    {
        seeMoreButtonTextObj.SetActive(isSeeMore);
        seeLessButtonTextObj.SetActive(!isSeeMore);
    }

    string tempMinDecLineStr = "";
    public void SetupDecPart(string strText)
    {
        int numLines = strText.Split('\n').Length;
        //Debug.LogError("Des Line Count:" + numLines);

        if (numLines > 2)
        {
            string[] subLineSTR = strText.Split('\n').Take(2).ToArray();
            //Debug.LogError("Result:" + subLineSTR);

            tempMinDecLineStr = "";
            for (int i = 0; i < subLineSTR.Length; i++)
            {
                tempMinDecLineStr += subLineSTR[i] + "\n";
            }
            descriptionText.text = tempMinDecLineStr;

            SeeMoreLessBioTextSetup(true);
            seeMoreDecObj.SetActive(true);
        }
        else
        {
            //false see more button
            seeMoreDecObj.SetActive(false);
        }
        if (this.gameObject.activeInHierarchy)
        {
            RefreshDecTextContentSizeFitter();
        }
    }

    public void RefreshDescriptionAfterEdit(string updatedStr)
    {
        if (!string.IsNullOrEmpty(updatedStr))//new
        {
            descriptionText.text = updatedStr;
            SetupDecPart(descriptionText.text);
        }
        else
        {
            descriptionText.text = "";
            seeMoreDecObj.SetActive(false);
        }
    }
    #endregion

    #region Share Image or Video Referense or methods.......
    public string shareMediaUrl = "";
    //this method is used to get Share media url from AWS.......
    void GetShareURL()
    {
        if (!string.IsNullOrEmpty(userData.Image))
        {
            bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(userData.Image);
            if (isImageUrlFromDropbox)
            {
                shareMediaUrl = userData.Image;
            }
            else
            {
                GetFeedUrl(userData.Image);
            }
        }
        else if (!string.IsNullOrEmpty(userData.Video))
        {
            bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(userData.Video);
            if (isVideoUrlFromDropbox)
            {
                shareMediaUrl = userData.Video;
            }
            else
            {
                GetFeedUrl(userData.Video);
            }
        }
    }

    public void GetFeedUrl(string key)
    {
        string tempFeedURl = "";
        var request_1 = new GetPreSignedUrlRequest()
        {
            BucketName = AWSHandler.Instance.Bucketname,
            Key = key,
            Expires = DateTime.Now.AddHours(6)
        };
        AWSHandler.Instance._s3Client.GetPreSignedURLAsync(request_1, (callback) =>
        {
            if (callback.Exception == null)
            {
                tempFeedURl = callback.Response.Url;
                string[] feedUrlArr = tempFeedURl.Split(char.Parse("?"));
                shareMediaUrl = feedUrlArr[0];
            }
            else
                Debug.LogError(callback.Exception);
        });
    }

    //this method is used to share button click.......
    public void OnClickShareButton()
    {       
        new NativeShare().AddFile("Scoial Image")
       .SetSubject("Subject goes here").SetText("Xana App!").SetUrl(shareMediaUrl)
       .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
       .Share();
    }
    #endregion

    #region Menu Button method.......
    //this method is used to Menu button click.......
    public void OnClickMenuButton()
    {
        FeedEditOrDeleteData feedEditOrDeleteData = new FeedEditOrDeleteData();
        feedEditOrDeleteData.feedId = userData.Id;
        feedEditOrDeleteData.feedTitle = userData.Title;
        feedEditOrDeleteData.feedDescriptions = userData.Descriptions;
        feedEditOrDeleteData.feedImage = userData.Image;
        feedEditOrDeleteData.feedVideo = userData.Video;
        feedEditOrDeleteData.feedCreatedBy = userData.CreatedBy;
        feedEditOrDeleteData.CreatedAt = userData.CreatedAt;
        feedEditOrDeleteData.UpdatedAt = userData.UpdatedAt;
        feedEditOrDeleteData.userData = feedUserData;

        FeedUIController.Instance.feedEditOrDeleteData = feedEditOrDeleteData;
        FeedUIController.Instance.editDeleteCurrentPostFeedVideoItem = this;
        FeedUIController.Instance.OnShowEditDeleteFeedScreen(true);
    }
    #endregion

    #region Comment Button Methods.......
    //this method is used to Set Comment count on text....... 
    public void CommentCountUISetup(int commentCount)
    {
        string commentCountSTR = FeedUIController.Instance.GetAbreviation(commentCount);
        if (commentCountSTR != "0")
        {
            commentBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = commentCountSTR;
            //commentBtn.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -3);
        }
        else
        {
            commentBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            //commentBtn.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -23);
        }
    }

    public void OnClickCommentButton(bool isRefresh)
    {
        //this method is get comment list and check if current feed this on multiple time not get comment list.......
        APIManager.Instance.CommentListGetAndClickFeedCommentButton(userData.Id, isRefresh, userData.commentCount);
        if (!isRefresh)
        {
            FeedUIController.Instance.OpenCommentPanel();
        }
    }
    #endregion

    #region Like or DisLike Button Methods.......
    //this method is setup likeCount text and image.......
    public void LikeCountAndUISetup(int likeCount)
    {
        string likeCountSTR = FeedUIController.Instance.GetAbreviation(likeCount);
        if (likeCountSTR != "0")
        {
            likeCountBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = likeCountSTR;
            //likeCountBtn.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -3);
        }
        else
        {
            likeCountBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            //likeCountBtn.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
        }
        if (userData.isLike)
        {
            likeCountBtn.transform.GetChild(0).GetComponent<Image>().color = likeSelectionColor;
        }
        else
        {
            likeCountBtn.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
    }


    //this method is used to like or dislike button click.......
    public void OnClickLikeOrDisLikeButton()
    {
        APIManager.Instance.RequestLikeOrDisLikeFeed(userData.Id.ToString(), likeCountBtn.GetComponent<Button>());
    }
    #endregion
}