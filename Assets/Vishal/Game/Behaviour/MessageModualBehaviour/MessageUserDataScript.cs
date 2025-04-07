using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperStar.Helpers;
using System.IO;

public class MessageUserDataScript : MonoBehaviour
{
    public AllFollowingRow allFollowingRow;

    public TextMeshProUGUI textUserName;
    public Image profileImage;

    public Toggle selectionToggle;
    // public Toggle selectionToggle;

    private void OnDestroy()
    {
        if (!string.IsNullOrEmpty(allFollowingRow.following.avatar))
        {
            AssetCache.Instance.RemoveFromMemory(profileImage.sprite);
            profileImage.sprite = null;
            APIManager.Instance.ResourcesUnloadAssetFile();//UnloadUnusedAssets file call every 15 items.......
        }
    }

    public void LoadFeed(AllFollowingRow allFollowingRow1)
    {
        allFollowingRow = allFollowingRow1;

        if (!string.IsNullOrEmpty(allFollowingRow.following.avatar))
        {
            bool isAvatarUrlFromDropbox = CheckUrlDropboxOrNot(allFollowingRow.following.avatar);
            //Debug.LogError("isAvatarUrlFromDropbox: " + isAvatarUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);

            if (isAvatarUrlFromDropbox)
            {
                AssetCache.Instance.EnqueueOneResAndWait(allFollowingRow.following.avatar, allFollowingRow.following.avatar, (success) =>
                {
                    if (success)
                        AssetCache.Instance.LoadSpriteIntoImage(profileImage, allFollowingRow.following.avatar, changeAspectRatio: true);
                });
            }
            else
            {
                GetImageFromAWS(allFollowingRow.following.avatar, profileImage);//Get image from aws and save/load into asset cache.......
            }
        }
        if (!string.IsNullOrEmpty(allFollowingRow.following.name))
        {
            textUserName.text = allFollowingRow.following.name;
        }
        else
        {
            textUserName.text = allFollowingRow.following.email;
        }
    }

    public void OnClickSelectFriend()
    {
        if (!selectionToggle.isOn)
        {
            GameObject friendItemObject = Instantiate(APIController.Instance.selectedFriendItemPrefab, MessageController.Instance.selectedFriendItemPrefabParent);
            //friendItemObject.GetComponent<MessageUserDataScript>().allFollowingRow = allFollowingRow;
            friendItemObject.GetComponent<MessageUserDataScript>().LoadFeed(allFollowingRow);
            selectionToggle.isOn = true;
            MessageController.Instance.CreateNewMessageUserList.Add(allFollowingRow.userId.ToString());
            MessageController.Instance.createNewMessageUserAvatarSPList.Add(profileImage.sprite);
            // APIController.Instance.allFollowingUserList.Remove(allFollowingRow.following.name);
            APIController.Instance.allChatMemberList.Add(allFollowingRow.following.name);

            if (MessageController.Instance.searchManagerFindFriends.searchScrollView.activeSelf)
            {
                MessageUserDataScript ddd = MessageController.Instance.searchManagerFindFriends.allMessageUserDataList.Find(x => x.allFollowingRow.id == allFollowingRow.id);
                ddd.selectionToggle.isOn = selectionToggle.isOn;
            }
        }
        else
        {
            for (int i = 0; i < MessageController.Instance.selectedFriendItemPrefabParent.childCount; i++)
            {
                if (MessageController.Instance.selectedFriendItemPrefabParent.GetChild(i).gameObject.GetComponent<MessageUserDataScript>().allFollowingRow.userId == allFollowingRow.userId)
                {
                    // APIController.Instance.allFollowingUserList.Add(allFollowingRow.following.name);
                    APIController.Instance.allChatMemberList.Remove(allFollowingRow.following.name);
                    MessageController.Instance.CreateNewMessageUserList.Remove(allFollowingRow.userId.ToString());
                    MessageController.Instance.createNewMessageUserAvatarSPList.Remove(profileImage.sprite);
                    Destroy(MessageController.Instance.selectedFriendItemPrefabParent.GetChild(i).gameObject);
                    break;
                }
            }
            selectionToggle.isOn = false;
        }
        MessageController.Instance.ActiveSelectionScroll();
    }

    public void OnClickDeleteFriend()
    {
        for (int i = 0; i < MessageController.Instance.followingUserParent.childCount; i++)
        {
            if (MessageController.Instance.followingUserParent.GetChild(i).gameObject.GetComponent<MessageUserDataScript>().allFollowingRow.userId == allFollowingRow.userId)
            {
                //Debug.LogError("aaaaaaaaaaaaaaaaa");
                APIController.Instance.allChatMemberList.Remove(allFollowingRow.following.name);
                MessageController.Instance.CreateNewMessageUserList.Remove(allFollowingRow.userId.ToString());
                MessageController.Instance.createNewMessageUserAvatarSPList.Remove(profileImage.sprite);
                MessageController.Instance.ActiveSelectionScroll();
                MessageController.Instance.followingUserParent.GetChild(i).gameObject.GetComponent<MessageUserDataScript>().selectionToggle.isOn = false;
                Destroy(this.gameObject);
                break;
            }
        }
    }

    public void ResetScrollView()
    {
        MessageController.Instance.selectionScrollView.transform.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        MessageController.Instance.selectionScrollView.transform.parent.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

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
}