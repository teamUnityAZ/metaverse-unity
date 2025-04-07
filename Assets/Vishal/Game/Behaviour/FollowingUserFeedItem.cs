﻿using RenderHeads.Media.AVProVideo;
using SuperStar.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using Amazon.S3.Model;
using System.Linq;

public class FollowingUserFeedItem : MonoBehaviour
{
    public FeedsByFollowingUserRow FollowingUserFeedData;

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
        if (FollowingUserFeedData.User != null)
        {
            if (!string.IsNullOrEmpty(FollowingUserFeedData.User.Avatar))
            {
                AssetCache.Instance.RemoveFromMemory(profileImage.sprite);
                profileImage.sprite = null;
            }
        }
        APIManager.Instance.ResourcesUnloadAssetFile();
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
                        if (AssetCache.Instance.HasFile(FollowingUserFeedData.Image))
                        {
                            AssetCache.Instance.LoadSpriteIntoImage(imgFeed, FollowingUserFeedData.Image, changeAspectRatio: true);
                        }
                        if (!string.IsNullOrEmpty(FollowingUserFeedData.User.Avatar))
                        {
                            if (AssetCache.Instance.HasFile(FollowingUserFeedData.User.Avatar))
                            {
                                AssetCache.Instance.LoadSpriteIntoImage(profileImage, FollowingUserFeedData.User.Avatar, changeAspectRatio: true);
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
                        if (FollowingUserFeedData.User != null)
                        {
                            if (!string.IsNullOrEmpty(FollowingUserFeedData.User.Avatar))
                            {
                                AssetCache.Instance.RemoveFromMemory(profileImage.sprite);
                                profileImage.sprite = null;
                            }
                        }
                        Resources.UnloadUnusedAssets();//Every clear
                        Caching.ClearCache();
                        GC.Collect();
                        //APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
                    }
                    else
                    {
                        feedMediaPlayer.Stop();
                    }
                }
            }
        }
    }   

    public void LoadFeed()
    {
        descriptionText.text = "";

        if (FollowingUserFeedData.User != null)
        {
            feedUserData.Id = FollowingUserFeedData.User.Id;
            feedUserData.Name = FollowingUserFeedData.User.Name;
            feedUserData.Email = FollowingUserFeedData.User.Email;
            feedUserData.Avatar = FollowingUserFeedData.User.Avatar;

            userName.text = APIManager.DecodedString(FollowingUserFeedData.User.Name);
            Invoke("UserNameSizeSetUp", 0.2f);
        }

        //for comment count.......
        CommentCountUISetup(FollowingUserFeedData.commentCount);

        //for like count.......
        LikeCountAndUISetup(FollowingUserFeedData.LikeCount);

        if (!string.IsNullOrEmpty(FollowingUserFeedData.Descriptions))//new
        {
            descriptionText.text = APIManager.DecodedString(FollowingUserFeedData.Descriptions);
            SetupDecPart(descriptionText.text);
        }
        else
        {
            descriptionText.text = "";
            seeMoreDecObj.SetActive(false);
        }

        if (FollowingUserFeedData.UpdatedAt != null)
        {
            timeText.text = FeedUIController.Instance.GetConvertedTimeString(FollowingUserFeedData.UpdatedAt);
        }
        //  feedTitle.text = FeedData.title;
        // feedLike.text = FeedData.likeCount.ToString();

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
        if (FollowingUserFeedData.User != null)
        {
            if (!string.IsNullOrEmpty(FollowingUserFeedData.User.Avatar))//set avatar image.......
            {
                bool isAvatarUrlFromDropbox = CheckUrlDropboxOrNot(FollowingUserFeedData.User.Avatar);
                //Debug.LogError("isAvatarUrlFromDropbox: " + isAvatarUrlFromDropbox + " :name:" + FeedRawData.name);

                if (isAvatarUrlFromDropbox)
                {
                    AssetCache.Instance.EnqueueOneResAndWait(FollowingUserFeedData.User.Avatar, FollowingUserFeedData.User.Avatar, (success) =>
                    {
                        if (success)
                            AssetCache.Instance.LoadSpriteIntoImage(profileImage, FollowingUserFeedData.User.Avatar, changeAspectRatio: true);
                    });
                }
                else
                {
                    GetImageFromAWS(FollowingUserFeedData.User.Avatar, profileImage);//Get image from aws and save/load into asset cache.......
                }
            }
        }

        if (!string.IsNullOrEmpty(FollowingUserFeedData.Image))
        {
            bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(FollowingUserFeedData.Image);
            //Debug.LogError("isImageUrlFromDropbox:  " + isImageUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);
            if (isImageUrlFromDropbox)
            {
                AssetCache.Instance.EnqueueOneResAndWait(FollowingUserFeedData.Image, FollowingUserFeedData.Image, (success) =>
                {
                    if (success)
                    {
                        AssetCache.Instance.LoadSpriteIntoImage(imgFeed, FollowingUserFeedData.Image, changeAspectRatio: true);
                        isImageSuccessDownloadAndSave = true;
                    }
                });
            }
            else
            {
                GetImageFromAWS(FollowingUserFeedData.Image, imgFeed);//Get image from aws and save/load into asset cache.......
            }

            PhotoImage.SetActive(true);
            feedMediaPlayer.gameObject.SetActive(false);
            videoDisplay.gameObject.SetActive(false);
        }
        else if (!string.IsNullOrEmpty(FollowingUserFeedData.Video))
        {
            bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(FollowingUserFeedData.Video);
            isVideoOrImage = true;

            //Debug.LogError("FeedData.video " + FollowingUserFeedData.Video);
            PhotoImage.SetActive(false);
            videoDisplay.gameObject.SetActive(true);
            feedMediaPlayer.gameObject.SetActive(true);

            if (isVideoUrlFromDropbox)
            {
                bool videoPlay = feedMediaPlayer.OpenMedia(new MediaPath(FollowingUserFeedData.Video, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                //feedMediaPlayer.Play();
                if (videoPlay)
                {
                    isImageSuccessDownloadAndSave = true;
                }
            }
            else
            {
                GetVideoUrl(FollowingUserFeedData.Video);//get video url from aws and play.......
            }
        }
    }

    //this method is used to user profile button click.......
    public void OnClickPlayerProfileButton()
    {
        Debug.LogError("Feed Id :" + FollowingUserFeedData.Id + "   :User Id :" + FollowingUserFeedData.User.Id);

        OtherPlayerProfileData.Instance.backKeyManageList.Add("FollowingTabScreen");//For back mamages.......

        //this api get any user profile data and feed for other player profile....... 
        SingleUserProfileData singleUserProfileData = new SingleUserProfileData();
        if (FollowingUserFeedData.User != null)
        {
            singleUserProfileData.id = FollowingUserFeedData.User.Id;
            singleUserProfileData.name = FollowingUserFeedData.User.Name;
            singleUserProfileData.email = FollowingUserFeedData.User.Email;
            singleUserProfileData.avatar = FollowingUserFeedData.User.Avatar;
        }

        SingleUserProfile singleUserProfile = new SingleUserProfile();
        singleUserProfileData.userProfile = singleUserProfile;

        OtherPlayerProfileData.Instance.RequestGetUserDetails(singleUserProfileData);
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
            descriptionText.text = APIManager.DecodedString(FollowingUserFeedData.Descriptions);
            SeeMoreLessBioTextSetup(false);
        }
        else
        {
            descriptionText.text = tempMinDecLineStr;
            SeeMoreLessBioTextSetup(true);
        }

        RefreshDecTextContentSizeFitter();
    }

    void RefreshDecTextContentSizeFitter()
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
    #endregion

    #region Share Image or Video Referense or methods.......
    public string shareMediaUrl = "";
    //this method is used to get Share media url from AWS.......
    void GetShareURL()
    {
        if (!string.IsNullOrEmpty(FollowingUserFeedData.Image))
        {
            bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(FollowingUserFeedData.Image);
            if (isImageUrlFromDropbox)
            {
                shareMediaUrl = FollowingUserFeedData.Image;
            }
            else
            {
                GetFeedUrl(FollowingUserFeedData.Image);
            }
        }
        else if (!string.IsNullOrEmpty(FollowingUserFeedData.Video))
        {
            bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(FollowingUserFeedData.Video);
            if (isVideoUrlFromDropbox)
            {
                shareMediaUrl = FollowingUserFeedData.Video;
            }
            else
            {
                GetFeedUrl(FollowingUserFeedData.Video);
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
        APIManager.Instance.CommentListGetAndClickFeedCommentButton(FollowingUserFeedData.Id, isRefresh, FollowingUserFeedData.commentCount);
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
        if (FollowingUserFeedData.isLike)
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
        APIManager.Instance.RequestLikeOrDisLikeFeed(FollowingUserFeedData.Id.ToString(), likeCountBtn.GetComponent<Button>());
    }
    #endregion
}