﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperStar.Helpers;
using System.IO;
using UnityEngine.Networking;

public class ChatMemberData : MonoBehaviour
{
    // public ChatGetConversationDatum allChatGetConversationDatum;
    public ChatGetConversationGroupUser chatGetConversationUser;
    public ChatGetConversationDatum allChatGetConversationDatum;

    public int createdGroupId;

    public Image profileImage;
    public TextMeshProUGUI userNameText;

    public Image followButtonImage;
    public Sprite followingSprite;
    public TextMeshProUGUI followText;
    public Color FollowingTextColor;

    public GameObject menuButton;

    public bool isFollowFollowing = false;

    int callingIndex;

    public void LoadData(int index)
    {
        if(createdGroupId == APIManager.Instance.userId)//if user is admin then show member menu button otherwise disable.......
        {
            menuButton.SetActive(true);
        }
        else
        {
            menuButton.SetActive(false);
        }

        callingIndex = index;

        if (index == 0)
        {
            isFollowFollowing = chatGetConversationUser.isFollowing;
            if (!string.IsNullOrEmpty(chatGetConversationUser.user.name))
            {
                userNameText.text = chatGetConversationUser.user.name;
            }
            else
            {
                userNameText.text = chatGetConversationUser.user.email;
            }

            if (!string.IsNullOrEmpty(chatGetConversationUser.user.avatar))//rik for avatar user
            {
                bool isAvatarUrlFromDropbox = CheckUrlDropboxOrNot(chatGetConversationUser.user.avatar);
                //Debug.LogError("isAvatarUrlFromDropbox: " + isAvatarUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);

                if (isAvatarUrlFromDropbox)
                {
                    AssetCache.Instance.EnqueueOneResAndWait(chatGetConversationUser.user.avatar, chatGetConversationUser.user.avatar, (success) =>
                    {
                        if (success)
                            AssetCache.Instance.LoadSpriteIntoImage(profileImage, chatGetConversationUser.user.avatar, changeAspectRatio: true);
                    });
                }
                else
                {
                    GetImageFromAWS(chatGetConversationUser.user.avatar, profileImage);//Get image from aws and save/load into asset cache.......
                }
                //Debug.LogError("chatmemberdata 1111:" + chatGetConversationUser.user.avatar);
            }

            SetFolloButton(isFollowFollowing);
        }
        else if (index == 1)
        {
            if (allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
            {
                userNameText.text = allChatGetConversationDatum.ConSender.name;
                if (!string.IsNullOrEmpty(allChatGetConversationDatum.ConSender.avatar))
                {
                    //Debug.LogError("AllConversation ConSender11:" + allChatGetConversationDatum.ConSender.name);
                    bool isAvatarUrlFromDropbox = CheckUrlDropboxOrNot(allChatGetConversationDatum.ConSender.avatar);
                    //Debug.LogError("isAvatarUrlFromDropbox: " + isAvatarUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);

                    if (isAvatarUrlFromDropbox)
                    {
                        AssetCache.Instance.EnqueueOneResAndWait(allChatGetConversationDatum.ConSender.avatar, allChatGetConversationDatum.ConSender.avatar, (success) =>
                        {
                            if (success)
                                AssetCache.Instance.LoadSpriteIntoImage(profileImage, allChatGetConversationDatum.ConSender.avatar, changeAspectRatio: true);
                        });
                    }
                    else
                    {
                        GetImageFromAWS(allChatGetConversationDatum.ConSender.avatar, profileImage);//Get image from aws and save/load into asset cache.......
                    }
                }
                SetFolloButton(false);
            }
            else
            {
                userNameText.text = allChatGetConversationDatum.ConReceiver.name;
                if (!string.IsNullOrEmpty(allChatGetConversationDatum.ConReceiver.avatar))
                {
                    //Debug.LogError("AllConversation ConReceiver:" + allChatGetConversationDatum.ConReceiver.name);

                    bool isAvatarUrlFromDropbox = CheckUrlDropboxOrNot(allChatGetConversationDatum.ConReceiver.avatar);
                    //Debug.LogError("isAvatarUrlFromDropbox: " + isAvatarUrlFromDropbox + " :name:" + FeedsByFollowingUserRowData.User.Name);

                    if (isAvatarUrlFromDropbox)
                    {
                        AssetCache.Instance.EnqueueOneResAndWait(allChatGetConversationDatum.ConReceiver.avatar, allChatGetConversationDatum.ConReceiver.avatar, (success) =>
                        {
                            if (success)
                                AssetCache.Instance.LoadSpriteIntoImage(profileImage, allChatGetConversationDatum.ConReceiver.avatar, changeAspectRatio: true);
                        });
                    }
                    else
                    {
                        GetImageFromAWS(allChatGetConversationDatum.ConReceiver.avatar, profileImage);//Get image from aws and save/load into asset cache.......
                    }
                }
                SetFolloButton(false);
            }
        }
    }

    public void SetFolloButton(bool isFollow)
    {
        isFollowFollowing = isFollow;//setUP follow following....... 
        if (isFollow)
        {
            followButtonImage.color = FollowingTextColor;
            followText.text = TextLocalization.GetLocaliseTextByKey("Following");
            followText.color = Color.black;
        }
        else
        {
            followButtonImage.color = Color.black;
            followText.text = TextLocalization.GetLocaliseTextByKey("Follow");
            followText.color = Color.white;
        }
        //followText.GetComponent<TextLocalization>().LocalizeTextText();
    }

    //this method is user to Update main messagecontroller conversation data groupuser follow, following.......
    void GroupUserResponceUpdateAfterFollowOrUnFollow(bool isFollow)
    {
        int index = MessageController.Instance.allChatGetConversationDatum.group.groupUsers.IndexOf(chatGetConversationUser);
        //Debug.LogError("GroupUserResponceUpdateAfterFollowOrUnFollow Index:" + index);
        if (index < MessageController.Instance.allChatGetConversationDatum.group.groupUsers.Count)
        {
            MessageController.Instance.allChatGetConversationDatum.group.groupUsers[index].isFollowing = isFollow;
        }
    }

    //this method is used to Menu button click.......
    public void OnClickMenuButton()
    {
        Debug.LogError("Menu button click.......");
        MessageController.Instance.currentSelectedGroupMemberDataScript = this;
        MessageController.Instance.removeGroupmemberConfirmationScreen.SetActive(true);
    }

    //this method is used to follow following button click
    public void OnClickFollowFollowingBtn()
    {
        MessageController.Instance.LoaderShow(true);//active loader.......
        if (isFollowFollowing)
        {
            Debug.LogError("UnFollow User Id:"+ chatGetConversationUser.user.id);
            //unfollow.......
            RequestUnFollowAUser(chatGetConversationUser.user.id.ToString());
        }
        else
        {
            Debug.LogError("Follow User Id:" + chatGetConversationUser.user.id);
            //follow.......
            RequestFollowAUser(chatGetConversationUser.user.id.ToString());
        }
    }

    public void RequestFollowAUser(string user_Id)
    {
        StartCoroutine(IERequestFollowAUser(user_Id));
    }
    public IEnumerator IERequestFollowAUser(string user_Id)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", user_Id);

        using (UnityWebRequest www = UnityWebRequest.Post((ConstantsGod.API_BASEURL + ConstantsGod.r_url_FollowAUser), form))
        {
            www.SetRequestHeader("Authorization", APIManager.Instance.userAuthorizeToken);

            yield return www.SendWebRequest();

            MessageController.Instance.LoaderShow(false);//false loader.......

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                Debug.Log("follow user success data:" + data);
                SetFolloButton(true);
                GroupUserResponceUpdateAfterFollowOrUnFollow(true);

                //Refresh Get following api.......
                MessageController.Instance.GetAllFollowingForSelectFriends();//request Get All Following api call.......
                //MessageController.Instance.SelectFriendFollowinPaginationResetData();//Reset select friends following api pagination.......
                //APIManager.Instance.RequestGetAllFollowing(1, 100, "message");
            }
        }
    }

    public void RequestUnFollowAUser(string user_Id)
    {
        StartCoroutine(IERequestUnFollowAUser(user_Id));
    }
    public IEnumerator IERequestUnFollowAUser(string user_Id)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", user_Id);

        using (UnityWebRequest www = UnityWebRequest.Post((ConstantsGod.API_BASEURL + ConstantsGod.r_url_UnFollowAUser), form))
        {
            www.SetRequestHeader("Authorization", APIManager.Instance.userAuthorizeToken);

            yield return www.SendWebRequest();

            MessageController.Instance.LoaderShow(false);//false loader.......

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                Debug.Log("Unfollow user success data:" + data);
                SetFolloButton(false);
                GroupUserResponceUpdateAfterFollowOrUnFollow(false);
                //Refresh Get following api.......
                MessageController.Instance.GetAllFollowingForSelectFriends();//request Get All Following api call.......
                //MessageController.Instance.SelectFriendFollowinPaginationResetData();//Reset select friends following api pagination.......
                //APIManager.Instance.RequestGetAllFollowing(1, 100, "message");
            }
        }
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
            AssetCache.Instance.EnqueueOneResAndWait(key, (ConstantsGod.r_AWSImageKitBaseUrl + key), (success) =>
            {
                if (success)
                {
                    AssetCache.Instance.LoadSpriteIntoImage(profileImage, key, changeAspectRatio: true);
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
                                    AssetCache.Instance.LoadSpriteIntoImage(profileImage, allChatGetConversationDatum.ConReceiver.avatar, changeAspectRatio: true);
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

    #region User Profile Button Click.......
    //this method is used to user profile button click.......
    public void OnClickPlayerProfileButton()
    {
        Debug.LogError("OnClickPlayerProfileButton User Id:" + chatGetConversationUser.userId + "   :Calling Index:" + callingIndex);
        if (callingIndex == 0)
        {
            MessageController.Instance.footerCan.GetComponent<BottomTabManager>().OnClickFeedButton();
            if (!PremiumUsersDetails.Instance.PremiumUserUI.activeSelf)
            {
                if (OtherPlayerProfileData.Instance != null)
                {
                    Debug.LogError("OnClickPlayerProfileButton other profile calling.......");
                    OtherPlayerProfileData.Instance.backKeyManageList.Add("GroupDetailsScreen");//For back mamages.......

                    //this api get any user profile data and feed for other player profile....... 
                    SingleUserProfileData singleUserProfileData = new SingleUserProfileData();
                    if (chatGetConversationUser.user != null)
                    {
                        singleUserProfileData.id = chatGetConversationUser.user.id;
                        singleUserProfileData.name = chatGetConversationUser.user.name;
                        singleUserProfileData.email = chatGetConversationUser.user.email;
                        singleUserProfileData.avatar = chatGetConversationUser.user.avatar;
                    }

                    SingleUserProfile singleUserProfile = new SingleUserProfile();
                    singleUserProfileData.userProfile = singleUserProfile;

                    OtherPlayerProfileData.Instance.RequestGetUserDetails(singleUserProfileData);
                }
            }
        }
    }
    #endregion
}