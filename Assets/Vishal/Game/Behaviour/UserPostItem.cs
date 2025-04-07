using Amazon.S3.Model;
using RenderHeads.Media.AVProVideo;
using SuperStar.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserPostItem : MonoBehaviour
{
    public AllFeedByUserIdRow userData;
    public TaggedFeedsByUserIdRow tagUserData;
    public FeedsByFollowingUser feedUserData;
    //public AllFeedByUserIdData userPostData;

    public Image imgFeed, cameraIcon;
    public GameObject PhotoImage, VideoImage;
    public MediaPlayer feedMediaPlayer;
    public GameObject videoDisplay;
    public string avtarUrl;

    public bool isVideoFeed = false;
    [Space]
    public bool isImageSuccessDownloadAndSave = false;
    public bool isReleaseFromMemoryOrNot = false;
    public bool isOnScreen;//check object is on screen or not
    public bool isPrintDebug = false;//for testing

    public bool isVisible = false;
    float lastUpdateCallTime;

    private void OnDisable()
    {
        if (!isVideoFeed)
        {
            AssetCache.Instance.RemoveFromMemory(imgFeed.sprite);
            imgFeed.sprite = null;
            isReleaseFromMemoryOrNot = true;
            //Resources.UnloadUnusedAssets();//every clear.......
            //Caching.ClearCache();
            APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
        }
    }

    /*public int cnt = 0;
    private void OnEnable()
    {
        if (cnt > 0)
        {
            LoadFeed();
        }
        cnt += 1;
    }*/

    private void Update()//delete image after object out of screen
    {
        if (APIManager.Instance.isTestDefaultToken)//for direct SNS Scene Test....... 
        {
            return;
        }
        else if (isVideoFeed)
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
            if (mousePosNR.y >= -0.1f && mousePosNR.y <= 1.1f)
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
                    //Debug.LogError("Re Download Image");
                    if (AssetCache.Instance.HasFile(userData.Image))
                    {
                        AssetCache.Instance.LoadSpriteIntoImage(imgFeed, userData.Image, changeAspectRatio: true);
                        CheckAndSetResolutionOfImage(imgFeed.sprite);
                    }
                }
            }
            else
            {
                if (!isReleaseFromMemoryOrNot)
                {
                    //realse from memory 
                    isReleaseFromMemoryOrNot = true;
                    //Debug.LogError("remove from memory");
                    AssetCache.Instance.RemoveFromMemory(imgFeed.sprite);
                    imgFeed.sprite = null;
                    //Resources.UnloadUnusedAssets();//every clear.......
                    //Caching.ClearCache();
                    APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
                }
            }
        }
    }

    public void LoadFeed()
    {
        if (!APIManager.Instance.isTestDefaultToken)
        {
            isVisible = true;
        }
        else//only for test else part
        {
            DownloadAndLoadFeed();
        }
    }

    void DownloadAndLoadFeed()
    {
        if (userData.Id != 0)
        {
            if (!string.IsNullOrEmpty(userData.Image))
            {
                bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(userData.Image);
                //Debug.LogError("isImageUrlFromDropbox:  " + isImageUrlFromDropbox);
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

                cameraIcon.gameObject.SetActive(false);
                PhotoImage.SetActive(true);
                feedMediaPlayer.gameObject.SetActive(false);
                videoDisplay.gameObject.SetActive(false);
            }
            else if (!string.IsNullOrEmpty(userData.Video))
            {
                bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(userData.Video);
                isVideoFeed = true;

                //Debug.LogError("FeedData.video " + userData.Video);
                cameraIcon.gameObject.SetActive(true);
                PhotoImage.SetActive(false);

                videoDisplay.gameObject.SetActive(true);
                feedMediaPlayer.gameObject.SetActive(true);

                if (isVideoUrlFromDropbox)
                {
                    //feedMediaPlayer.OpenMedia(new MediaPath(userData.Video, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                    feedMediaPlayer.OpenMedia(new MediaPath(userData.Video, MediaPathType.AbsolutePathOrURL), autoPlay: false);
                    //feedMediaPlayer.Play();
                }
                else
                {
                    GetVideoUrl(userData.Video);//get video url from aws and play.......
                }
            }
        }
        else if (tagUserData.id != 0)
        {
            if (!string.IsNullOrEmpty(tagUserData.feed.image))
            {
                bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(tagUserData.feed.image);
                //Debug.LogError("isImageUrlFromDropbox:  " + isImageUrlFromDropbox);
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

                cameraIcon.gameObject.SetActive(false);
                PhotoImage.SetActive(true);
                feedMediaPlayer.gameObject.SetActive(false);
                videoDisplay.gameObject.SetActive(false);
            }
            else if (!string.IsNullOrEmpty(tagUserData.feed.video))
            {
                bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(tagUserData.feed.video);
                isVideoFeed = true;

                //Debug.LogError("FeedData.video " + tagUserData.feed.video);
                cameraIcon.gameObject.SetActive(true);
                PhotoImage.SetActive(false);

                videoDisplay.gameObject.SetActive(true);
                feedMediaPlayer.gameObject.SetActive(true);

                if (isVideoUrlFromDropbox)
                {
                    //feedMediaPlayer.OpenMedia(new MediaPath(tagUserData.feed.video, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                    feedMediaPlayer.OpenMedia(new MediaPath(tagUserData.feed.video, MediaPathType.AbsolutePathOrURL), autoPlay: false);
                    //feedMediaPlayer.Play();
                }
                else
                {
                    GetVideoUrl(tagUserData.feed.video);//get video url from aws and play.......
                }
            }
        }
    }


    public void OnClickPostItem()
    {
        /*APIManager.scrollToTop = false;
        APIManager.feedIdTemp = feedUserData.Id;     
        APIManager.Instance.RequestFeedCommentList(feedUserData.Id,1,1,50);*/
        StartCoroutine(loadVideoFeed());
    }
    IEnumerator loadVideoFeed()
    {
        foreach (Transform item in FeedUIController.Instance.videofeedParent)
        {
            Destroy(item.gameObject);
        }
        int index = 0;
        int pageIndex = 0;
        //FeedUIController.Instance.ShowLoader(true);
        if (userData.Id != 0)
        {
            List<AllFeedByUserIdRow> feedRowsDataList = new List<AllFeedByUserIdRow>();
            if (MyProfileDataManager.Instance.myProfileScreen.activeSelf) 
            {
                FeedUIController.Instance.feedFullViewScreenCallingFrom = "MyProfile";
                if (isVideoFeed)
                {
                    feedRowsDataList = MyProfileDataManager.Instance.allMyFeedVideoRootDataList;
                }
                else
                {
                    feedRowsDataList = MyProfileDataManager.Instance.allMyFeedImageRootDataList;
                }
            }
            else if (FeedUIController.Instance.otherPlayerProfileScreen.activeSelf)
            {
                FeedUIController.Instance.feedFullViewScreenCallingFrom = "OtherProfile";
                if (isVideoFeed)
                {
                    feedRowsDataList = OtherPlayerProfileData.Instance.allMyFeedVideoRootDataList;
                }
                else
                {
                    feedRowsDataList = OtherPlayerProfileData.Instance.allMyFeedImageRootDataList;
                }
            }
            else
            {
                FeedUIController.Instance.feedFullViewScreenCallingFrom = "";
                feedRowsDataList = APIManager.Instance.allFeedWithUserIdRoot.Data.Rows;
            }

            for (int i = 0; i < feedRowsDataList.Count; i++)
            {
                GameObject videofeedObject = Instantiate(APIController.Instance.PostVideoFeedPrefab, FeedUIController.Instance.videofeedParent);

                PostFeedVideoItem postFeedVideoItem = videofeedObject.GetComponent<PostFeedVideoItem>();

                postFeedVideoItem.userData = feedRowsDataList[i];
                postFeedVideoItem.feedUserData = feedUserData;
                postFeedVideoItem.avatarUrl = avtarUrl;
                postFeedVideoItem.LoadFeed();

                if (feedRowsDataList[i].Id == userData.Id)
                {
                    pageIndex = index;
                    FeedUIController.Instance.videoFeedRect.GetComponent<ScrollSnapRect>().startingPage = pageIndex;
                }
                index += 1;
            }
        }
        else if (tagUserData.id != 0)
        {
            for (int i = 0; i < APIManager.Instance.taggedFeedsByUserIdRoot.data.rows.Count; i++)
            {
                GameObject videofeedObject = Instantiate(APIController.Instance.PostVideoFeedPrefab, FeedUIController.Instance.videofeedParent);
                videofeedObject.GetComponent<PostFeedVideoItem>().tagUserData = APIManager.Instance.taggedFeedsByUserIdRoot.data.rows[i];
                //videofeedObject.GetComponent<PostFeedVideoItem>().avatarUrl = avtarUrl;
                videofeedObject.GetComponent<PostFeedVideoItem>().LoadFeed();

                if (APIManager.Instance.taggedFeedsByUserIdRoot.data.rows[i].id == tagUserData.id)
                {
                    pageIndex = index;
                    FeedUIController.Instance.videoFeedRect.GetComponent<ScrollSnapRect>().startingPage = pageIndex;
                }
                index += 1;
            }
        }
        yield return new WaitForSeconds(0.1f);
        //FeedUIController.Instance.ShowLoader(false);
        FeedUIController.Instance.feedVideoScreen.SetActive(true);
        FeedUIController.Instance.videoFeedRect.GetComponent<ScrollSnapRect>().StartScrollSnap();
        FeedUIController.Instance.videofeedParent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.1f);
        FeedUIController.Instance.videofeedParent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;       
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
                        //feedMediaPlayer.OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: true); ;
                        feedMediaPlayer.OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: false);
                        //feedMediaPlayer.Play();
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
            CheckAndSetResolutionOfImage(mainImage.sprite);
            isImageSuccessDownloadAndSave = true;
            return;
        }
        else
        {
            if(!string.IsNullOrEmpty(FeedUIController.Instance.createFeedLastPickFilePath))//for set after upload image imidiate for first time.......
            {
                if(FeedUIController.Instance.createFeedLastPickFileName == key)
                {
                    mainImage.sprite = FeedUIController.Instance.createFeedImage.sprite;
                    CheckAndSetResolutionOfImage(mainImage.sprite);
                    FeedUIController.Instance.lastPostCreatedImageDownload = true;                    
                }
            }

            AssetCache.Instance.EnqueueOneResAndWait(key, (ConstantsGod.r_AWSImageKitBaseUrl + key), (success) =>
            {
                if (success)
                {

                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                    //tt AssetCache.Instance.LoadTexture2DIntoRawImage(imgFeedRaw, FeedData.image, changeAspectRatio: true);
                    //Debug.LogError("Save and Image download success profile post Item");
                    CheckAndSetResolutionOfImage(mainImage.sprite);
                    isImageSuccessDownloadAndSave = true;

                    if (FeedUIController.Instance.createFeedLastPickFileName == key && FeedUIController.Instance.lastPostCreatedImageDownload)//after download and load last created post then clear created post data.......
                    {
                        FeedUIController.Instance.ResetAndClearCreateFeedData();
                    }
                }
            });

            //StartCoroutine(DownloadImageFromAWSUsingImageKit((APIManager.Instance.AWSImageKitBaseUrl + key), key, mainImage));
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
                                    if (FeedUIController.Instance.createFeedLastPickFileName == key && FeedUIController.Instance.lastPostCreatedImageDownload)//after download and load last created post then clear created post data.......
                                    {
                                        FeedUIController.Instance.ResetAndClearCreateFeedData();
                                    }

                                    //Debug.LogError("Save on Aws sucess");
                                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                                    CheckAndSetResolutionOfImage(mainImage.sprite);
                                    isImageSuccessDownloadAndSave = true;
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

    IEnumerator DownloadImageFromAWSUsingImageKit(string ImageUrl, string key, Image mainImage)
    {
        using (var www = UnityWebRequestTexture.GetTexture(ImageUrl))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    byte[] data = ((DownloadHandlerTexture)www.downloadHandler).data;
                    //handle the result
                    AssetCache.Instance.SaveImageEnqueueOneResAndWait(key, key, data, (success) =>
                    {
                        if (success)
                        {
                            if (FeedUIController.Instance.createFeedLastPickFileName == key && FeedUIController.Instance.lastPostCreatedImageDownload)//after download and load last created post then clear created post data.......
                            {
                                FeedUIController.Instance.ResetAndClearCreateFeedData();
                            }

                            //Debug.LogError("Save on Aws sucess");
                            AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                            CheckAndSetResolutionOfImage(mainImage.sprite);
                            isImageSuccessDownloadAndSave = true;

                            data = null;
                        }
                    });
                }
            }
            www.Dispose();
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

    #region Check And Set Image Orientation 
    public AspectRatioFitter aspectRatioFitter;
    public void CheckAndSetResolutionOfImage(Sprite feedImage)
    {
        float diff = feedImage.rect.width - feedImage.rect.height;

        //Debug.LogError("CheckAndSetResolutionOfImage:" + diff);
        if (diff < -160)
        {
            aspectRatioFitter.aspectRatio = 0.1f;
        }
        else
        {
            aspectRatioFitter.aspectRatio = 2.25f;
        }
    }
    #endregion
}