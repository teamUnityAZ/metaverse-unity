using Amazon.S3.Model;
using RenderHeads.Media.AVProVideo;
using SuperStar.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AttechmentData : MonoBehaviour
{
    public ChatAttachmentsRow attachmentsRow;

    public Image attechmentImage;
    public GameObject mediaPlayer, VideoPlayer;
    public GameObject videoIcon;
    public static ExtentionType currentExtention;
    public bool isChooseAttechmentScreen;

    private void OnDestroy()
    {
        if (!isChooseAttechmentScreen)
        {
            AssetCache.Instance.RemoveFromMemory(attechmentImage.sprite);
            attechmentImage.sprite = null;
            APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
        }
    }

    private void OnDisable()
    {
        if (isChooseAttechmentScreen)
        {
            AssetCache.Instance.RemoveFromMemory(attechmentImage.sprite);
            attechmentImage.sprite = null;
            //Resources.UnloadUnusedAssets();//every clear.......
            //Caching.ClearCache();
            APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
        }
    }

    public void LoadData(bool isChooseAttechmentScreen1)
    {
        isChooseAttechmentScreen = isChooseAttechmentScreen1;
        GetAndLoadMediaFile(attachmentsRow.url);
    }

    public void GetAndLoadMediaFile(string key)
    {
        //Debug.LogError("GetAndLoadMediaFile: " + key);
        GetExtentionType(key);
        //Debug.LogError("currentExtention:   " + currentExtention);
        if (currentExtention == ExtentionType.Image)
        {
            GetImageFromAWS(key, attechmentImage);
        }
        else if (currentExtention == ExtentionType.Video)
        {
            GetVideoUrl(key);
        }
    }   

    public void OnClickSeeFullImage()
    {        
        foreach (Transform item in MessageController.Instance.saveAttechmentParent)
        {
            Destroy(item.gameObject);
        }
        int index = 0;
        int pageIndex = 0;
        bool isMatch = false;
        for (int i = 0; i < APIManager.Instance.AllChatAttachmentsRoot.data.rows.Count; i++)
        {
            GameObject attechment = Instantiate(APIController.Instance.saveAttechmentPrefab, MessageController.Instance.saveAttechmentParent);
            attechment.GetComponent<SaveAttechmentScript>().attachmentsRow = APIManager.Instance.AllChatAttachmentsRoot.data.rows[i];
            attechment.GetComponent<SaveAttechmentScript>().LoadData();

            //Debug.LogError("id:" + APIManager.Instance.AllChatAttachmentsRoot.data.rows[i].id);
            //Debug.LogError("ids: " + attachmentsRow.id);
            if (APIManager.Instance.AllChatAttachmentsRoot.data.rows[i].id == attachmentsRow.id && !isMatch)
            {
                pageIndex = index;
                MessageController.Instance.saveAttechmentParent.transform.parent.GetComponent<ScrollSnapRect>().startingPage = pageIndex;
                MessageController.Instance.SaveAttachmentDetailsSetup(pageIndex);
                isMatch = true;
            }
            else
            {
                //Debug.LogError("index: " + index);
                index += 1;
            }
        }
        MessageController.Instance.AttechmentDownloadScreen.SetActive(true);
        MessageController.Instance.saveAttechmentParent.transform.parent.GetComponent<ScrollSnapRect>().StartScrollSnap();
    }

    public void SetVideoUi(bool isVideo)
    {
        if (isVideo)
        {
            if(attechmentImage!=null)
                attechmentImage.gameObject.SetActive(false);
            mediaPlayer.SetActive(true);
            VideoPlayer.SetActive(true);
            videoIcon.SetActive(true);
        }
        else
        {
            if (attechmentImage != null)
                attechmentImage.gameObject.SetActive(true);
            mediaPlayer.SetActive(false);
            VideoPlayer.SetActive(false);
            videoIcon.SetActive(false);
        }
    }

    #region Get Image and Video From AWS
    public void GetVideoUrl(string key)
    {
        var request_1 = new GetPreSignedUrlRequest()
        {
            BucketName = AWSHandler.Instance.Bucketname,
            Key = key,
            Expires = DateTime.Now.AddHours(6)
        };
        //Debug.LogError("Chat Video file sending url request:" + AWSHandler.Instance._s3Client);
        //AWSHandler.Instance.GetObject(key);
        AWSHandler.Instance._s3Client.GetPreSignedURLAsync(request_1, (callback) =>
        {
            if (callback.Exception == null)
            {
                string mediaUrl = callback.Response.Url;
                UnityToolbag.Dispatcher.Invoke(() =>
                {
                    if (this.isActiveAndEnabled)
                    {
                        //Debug.LogError("Chat Video URL " + mediaUrl);
                        SetVideoUi(true);
                        mediaPlayer.GetComponent<MediaPlayer>().OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: false);
                        //mediaPlayer.GetComponent<MediaPlayer>().OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                        //mediaPlayer.GetComponent<MediaPlayer>().Play();
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
                    //Debug.LogError("Save and Image download success");
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
                                    CheckAndSetResolutionOfImage(mainImage.sprite);
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
    #endregion

    #region Check And Set Image Orientation 
    public AspectRatioFitter aspectRatioFitter;
    public void CheckAndSetResolutionOfImage(Sprite feedImage)
    {
        float diff = feedImage.rect.width - feedImage.rect.height;

        //Debug.LogError("CheckAndSetResolutionOfImage:" + diff);
        if (diff < -150f)
        {
            aspectRatioFitter.aspectRatio = 0.1f;
        }
        else
        {
            aspectRatioFitter.aspectRatio = 2f;
        }
    }
    #endregion
}