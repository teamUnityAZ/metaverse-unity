using Amazon.S3.Model;
using RenderHeads.Media.AVProVideo;
using SuperStar.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveAttechmentScript : MonoBehaviour
{
    public ChatAttachmentsRow attachmentsRow;

    public Image attechmentImage;
    public GameObject mediaPlayer, VideoPlayer;
    public static ExtentionType currentExtention;

    private void OnDisable()
    {
        AssetCache.Instance.RemoveFromMemory(attechmentImage.sprite);
        attechmentImage.sprite = null;
        //Resources.UnloadUnusedAssets();//every clear.......
        //Caching.ClearCache();
        APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
    }

    public void LoadData()
    {      
        //GetObject(attachmentsRow.url);
        if (!string.IsNullOrEmpty(attachmentsRow.url))
        {
            GetAndLoadMediaFile(attachmentsRow.url);
        }
    }

    public void SetVideoUi(bool isVideo)
    {
        if (isVideo)
        {
            attechmentImage.gameObject.SetActive(false);
            mediaPlayer.SetActive(true);
            VideoPlayer.SetActive(true);
        }
        else
        {
            attechmentImage.gameObject.SetActive(true);
            mediaPlayer.SetActive(false);
            VideoPlayer.SetActive(false);
        }
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
                        mediaPlayer.GetComponent<MediaPlayer>().OpenMedia(new MediaPath(mediaUrl, MediaPathType.AbsolutePathOrURL), autoPlay: true);
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
                                    //Debug.LogError("Save on Aws sucess");
                                    AssetCache.Instance.LoadSpriteIntoImage(mainImage, key, changeAspectRatio: true);
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

    /*public void GetObject(string key)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "XanaChat", key);
        //Debug.LogError("path " + filePath);
        // Debug.LogError("GetObject0000:" + key);
        // Debug.LogError("Bucketname:" + AWSHandler.Instance.Bucketname);
        GetExtentionType(filePath);
        //Debug.LogError("currentExtention" + currentExtention);
        if (System.IO.File.Exists(filePath))
        {
            if (currentExtention == ExtentionType.Image)
            {
                SetVideoUi(false);
                //Debug.LogError("already exists ");
                byte[] bytes = File.ReadAllBytes(filePath);

                Texture2D texture = new Texture2D(900, 900, TextureFormat.RGB24, false);
                texture.filterMode = FilterMode.Trilinear;
                texture.LoadImage(bytes);
                attechmentImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
            else if (currentExtention == ExtentionType.Video)
            {
                SetVideoUi(true);
                mediaPlayer.GetComponent<MediaPlayer>().OpenMedia(new MediaPath(filePath, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                mediaPlayer.GetComponent<MediaPlayer>().Play();
            }
            // attechmentImage.sprite = Sprite.Create(texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.0f), 1.0f);
        }
        else
        {
            string saveDir = Path.Combine(Application.persistentDataPath, "XanaChat");
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            // Debug.LogError("path" + path);

            //  ResultText.text = string.Format("fetching {0} from bucket {1}", SampleFileName, S3BucketName);
            //  AWSHandler.Instance._s3Client.GetObjectAsync(AWSHandler.Instance.Bucketname, key, (responseObj) =>
            AWSHandler.Instance.Client.GetObjectAsync(AWSHandler.Instance.Bucketname, key, (responseObj) =>
            {
                byte[] data = null;
                var response = responseObj.Response;
                //Debug.LogError("GetObject:" + response.ResponseStream + " :Extention:"+ currentExtention);
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
                            SetVideoUi(false);
                            Texture2D texture = bytesToTexture2D(data);
                            //Debug.LogError("key " + key);
                            File.WriteAllBytes(filePath, data);
                            // NativeGallery.SaveImageToGallery(data, "XanaChat", key, null);
                            attechmentImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                            //Debug.LogError(attechmentImage.sprite == null ? "Image Failed" : "image loded");
                        }
                        else if (currentExtention == ExtentionType.Video)
                        {
                            SetVideoUi(true);
                            mediaPlayer.GetComponent<MediaPlayer>().OpenMedia(new MediaPath(filePath, MediaPathType.AbsolutePathOrURL), autoPlay: true);
                            mediaPlayer.GetComponent<MediaPlayer>().Play();
                        }
                    }
                }
            });
        }
    }


    public Texture2D bytesToTexture2D(byte[] imageBytes)
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        return tex;
    }*/  
}