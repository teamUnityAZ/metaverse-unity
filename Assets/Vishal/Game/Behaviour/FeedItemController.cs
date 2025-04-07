using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SuperStar.Helpers;
using UnityEngine.UI;
using UnityEngine.Video;
using RenderHeads.Media.AVProVideo;
using System.IO;
using System;
using Amazon.S3.Model;
using UnityEngine.Networking;

public class FeedItemController : MonoBehaviour
{
    public AllUserWithFeed FeedData;
    public AllUserWithFeedRow FeedRawData;
    //public AllUserWithFeedData userData;

    public RawImage imgFeedRaw; 
    public Image imgFeed, cameraIcon;
    public GameObject PhotoImage, VideoImage;
    //public VideoPlayer feedVideoPlayer;
    public MediaPlayer feedMediaPlayer;
    public GameObject videoDisplay;
    /*public TextMeshProUGUI feedTitle;
     public TextMeshProUGUI feedPlayerName;
     public TextMeshProUGUI feedLike;
     public TextMeshProUGUI tottlePostText;*/

    [Space]
    public bool isImageSuccessDownloadAndSave = false;
    public bool isReleaseFromMemoryOrNot = false;
    public bool isOnScreen;//check object is on screen or not
    public bool isPrintDebug = false;//for testing

    public bool isVisible = false;
    float lastUpdateCallTime;

    bool isClearAfterMemory = false;

    private void OnDestroy()
    {
        if (!isClearAfterMemory)
        {
            if (!string.IsNullOrEmpty(FeedData.image))
            {
                AssetCache.Instance.RemoveFromMemory(imgFeed.sprite);
                imgFeed.sprite = null;
                APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
            }
        }
    }

    public void ClearMemoryAfterDestroyObj()
    {
        isClearAfterMemory = true;
        if (!String.IsNullOrEmpty(FeedData.image))
        {
            AssetCache.Instance.RemoveFromMemory(imgFeed.sprite);
            imgFeed.sprite = null;
        }
    }

    int cnt = 0;
    private void OnEnable()
    {
        if (cnt > 0 && !isImageSuccessDownloadAndSave)
        {
            isVisible = true;
        }
        cnt += 1;
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
                    if (AssetCache.Instance.HasFile(FeedData.image))
                    {
                        AssetCache.Instance.LoadSpriteIntoImage(imgFeed, FeedData.image, changeAspectRatio: true);
                        CheckAndSetResolutionOfImage(imgFeed.sprite);
                        //tt AssetCache.Instance.LoadTexture2DIntoRawImage(imgFeedRaw, FeedData.image, changeAspectRatio: true);
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
                    //tt AssetCache.Instance.RemoveFromMemory(FeedData.image, true);
                    imgFeed.sprite = null;
                    //tt imgFeedRaw.texture = null;
                    //Resources.UnloadUnusedAssets();//every clear.......
                    //Caching.ClearCache();
                    APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
                }
            }
        }
    }

    public void LoadFeed()
    {
        //feedTitle.text = FeedData.title;
        //feedLike.text = FeedData.likeCount.ToString();

        if (!string.IsNullOrEmpty(FeedData.image))//Feed Hot Collection Items Initiate Total Count Set.......
        {
            FeedUIController.Instance.hotFeedInitiateTotalCount += 1;
        }

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
        if (!string.IsNullOrEmpty(FeedData.image))
        {
            bool isImageUrlFromDropbox = CheckUrlDropboxOrNot(FeedData.image);
            //Debug.LogError("isImageUrlFromDropbox:  " + isImageUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);
            if (isImageUrlFromDropbox)
            {
                AssetCache.Instance.EnqueueOneResAndWait(FeedData.image, FeedData.image, (success) =>
                {
                    if (success)
                    {
                        AssetCache.Instance.LoadSpriteIntoImage(imgFeed, FeedData.image, changeAspectRatio: true);
                        //tt AssetCache.Instance.LoadTexture2DIntoRawImage(imgFeedRaw, FeedData.image, changeAspectRatio: true);
                        isImageSuccessDownloadAndSave = true;
                        if (FeedUIController.Instance.hotFeedInitiateTotalCount > 0)
                        {
                            FeedUIController.Instance.hotFeedInitiateTotalCount -= 1;//Feed Hot items image loaded count Decrease
                            //FeedUIController.Instance.HotFeedImageLoadedCount += 1;//Feed Hot items image loaded count increase
                        }
                    }
                });
            }
            else
            {
                GetImageFromAWS(FeedData.image, imgFeed);//Get image from aws and save/load into asset cache.......
            }

            cameraIcon.gameObject.SetActive(false);
            PhotoImage.SetActive(true);
            //VideoImage.SetActive(false);
            //feedVideoPlayer.gameObject.SetActive(false);
            feedMediaPlayer.gameObject.SetActive(false);
            videoDisplay.gameObject.SetActive(false);
            //Debug.LogError("imagefeed");
        }
        else if (!string.IsNullOrEmpty(FeedData.video))
        {
            bool isVideoUrlFromDropbox = CheckUrlDropboxOrNot(FeedData.video);

            cameraIcon.gameObject.SetActive(true);
            //Debug.LogError("FeedData.video " + FeedData.video);
            //VideoImage.SetActive(true);
            PhotoImage.SetActive(false);
            videoDisplay.gameObject.SetActive(true);
            feedMediaPlayer.gameObject.SetActive(true);

            if (isVideoUrlFromDropbox)
            {
                //feedMediaPlayer.OpenMedia(new MediaPath(FeedData.video, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                feedMediaPlayer.OpenMedia(new MediaPath(FeedData.video, MediaPathType.AbsolutePathOrURL), autoPlay: false);
                //feedMediaPlayer.Play();
            }
            else
            {
                GetVideoUrl(FeedData.video);//get video url from aws and play.......
            }
        }
    }

    public void OnClickCheckOtherPlayerProfile()
    {
        //FeedUIController.Instance.OnClickCheckOtherPlayerProfile();
    }

    public void OnClickFeedItem()
    {
        /*APIManager.scrollToTop = false;
        APIManager.feedIdTemp = FeedData.id;
        APIManager.Instance.RequestFeedCommentList(FeedData.id,1,1,50);*/

        FeedUIController.Instance.feedFullViewScreenCallingFrom = "HotTab";
        StartCoroutine(loadVideoFeed());
    }
    IEnumerator loadVideoFeed()
    {
        foreach (Transform item in FeedUIController.Instance.videofeedParent)
        {
            Destroy(item.gameObject);
        }
        int index = 0;
        //int pageIndex = 0;
        bool isMatch = false;
        //FeedUIController.Instance.ShowLoader(true);
        for (int i = 0; i < APIManager.Instance.allUserRootList.Count; i++)
        {
            for (int j = 0; j < APIManager.Instance.allUserRootList[i].feeds.Count; j++)
            {
                GameObject videofeedObject = Instantiate(APIController.Instance.videofeedPrefab, FeedUIController.Instance.videofeedParent);
                videofeedObject.GetComponent<FeedVideoItem>().FeedRawData = APIManager.Instance.allUserRootList[i];
                videofeedObject.GetComponent<FeedVideoItem>().FeedData = APIManager.Instance.allUserRootList[i].feeds[j];
                videofeedObject.GetComponent<FeedVideoItem>().LoadFeed();

                if (APIManager.Instance.allUserRootList[i].id == FeedRawData.id && !isMatch)
                {
                    if (APIManager.Instance.allUserRootList[i].feeds[j].id == FeedData.id)
                    {
                       // pageIndex = index;
                        FeedUIController.Instance.videoFeedRect.GetComponent<ScrollSnapRect>().startingPage = index;
                        isMatch = true;
                    }
                }
                index += 1;
            }
        }

        yield return new WaitForSeconds(0.1f);
        //FeedUIController.Instance.ShowLoader(false);
        FeedUIController.Instance.feedVideoScreen.SetActive(true);
        FeedUIController.Instance.videoFeedRect.GetComponent<ScrollSnapRect>().StartScrollSnap();
        // FeedUIController.Instance.videoFeedRect.GetComponent<ScrollSnapRect>().LerpToPage(pageIndex);
        //Debug.LogError("name : " + FeedUIController.Instance.videofeedParent.name);
        FeedUIController.Instance.videofeedParent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return new WaitForSeconds(0.1f);
        FeedUIController.Instance.videofeedParent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        FeedUIController.Instance.feedUiScreen.SetActive(false);
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
                    if (this !=null && this.isActiveAndEnabled)
                    {
                        //Debug.LogError("Feed Video URL " + mediaUrl);
                        //feedMediaPlayer.OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: true);
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
            //Debug.LogError("Image Available on Disk hot item");
            AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
            CheckAndSetResolutionOfImage(mainImage.sprite);
            //tt AssetCache.Instance.LoadTexture2DIntoRawImage(imgFeedRaw, key, changeAspectRatio: true);
            isImageSuccessDownloadAndSave = true;
            if (FeedUIController.Instance.hotFeedInitiateTotalCount > 0)
            {
                FeedUIController.Instance.hotFeedInitiateTotalCount -= 1;//Feed Hot items image loaded count Decrease
                //FeedUIController.Instance.HotFeedImageLoadedCount += 1;//Feed Hot items image loaded count increase
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
                    CheckAndSetResolutionOfImage(mainImage.sprite);
                    //tt AssetCache.Instance.LoadTexture2DIntoRawImage(imgFeedRaw, FeedData.image, changeAspectRatio: true);
                    //Debug.LogError("Save and Image download success hot Item");
                    isImageSuccessDownloadAndSave = true;
                    if (FeedUIController.Instance.hotFeedInitiateTotalCount > 0)
                    {
                        FeedUIController.Instance.hotFeedInitiateTotalCount -= 1;//Feed Hot items image loaded count Decrease
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
                                    //Debug.LogError("Save image download from Aws success");
                                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                                    CheckAndSetResolutionOfImage(mainImage.sprite);
                                    //tt AssetCache.Instance.LoadTexture2DIntoRawImage(imgFeedRaw, key, changeAspectRatio: true);
                                    isImageSuccessDownloadAndSave = true;
                                }
                            });
                            //mainImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

                            if (FeedUIController.Instance.hotFeedInitiateTotalCount > 0)
                            {
                                FeedUIController.Instance.hotFeedInitiateTotalCount -= 1;//Feed Hot items image loaded count Decrease
                                //FeedUIController.Instance.HotFeedImageLoadedCount += 1;//Feed Hot items image loaded count increase
                            }
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
                            //Debug.LogError("Save image download from Aws success");
                            AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
                            CheckAndSetResolutionOfImage(mainImage.sprite);
                            //tt AssetCache.Instance.LoadTexture2DIntoRawImage(imgFeedRaw, key, changeAspectRatio: true);
                            isImageSuccessDownloadAndSave = true;
                            data = null;
                        }
                    });
                    //mainImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

                    if (FeedUIController.Instance.hotFeedInitiateTotalCount > 0)
                    {
                        FeedUIController.Instance.hotFeedInitiateTotalCount -= 1;//Feed Hot items image loaded count Decrease
                    }                    
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
            aspectRatioFitter.aspectRatio = 2.2f;
        }
    }
    #endregion
}