
using System.Collections.Generic;

public class ConstantsGod

{
    public static string AUTH_TOKEN = "AUTH_TOKEN";
    public static string ANIMATIONNAME = "animation name";
    public static string PLAYERNAME = "player name";
    public static string API_BASEURL = "https://app-api.xana.net";
    public static string ANDROIDPATH="path";
    public static readonly string UPLOADVIDEOPATH = "uploadVideo";
    public static readonly string VIDEOPATH = "Video Path";
    public static readonly string DEFAULT_TOKEN = "piyush55";
    public static readonly string TOTAL_AUDIO_VOLUME = "TOTAL AUDIO VOLUME";
    public static readonly string BGM_VOLUME = "BGM VOLUME";
    public static readonly string MIC = "MIC VOLUME";
    public static readonly string VIDEO_VOLUME = "VIDEO VOLUME";
    public static readonly string CAMERA_SENSITIVITY = "CAMERA SENSITIVITY";
    public static readonly string BASE_URL = "https://api-xana.angelium.net/api/";
    public static string ReactionThumb = "reaction thumb";
    public static string SENDMESSAGETEXT = "send message";
    public static string GUSTEUSERNAME = "guste user";
    public static string NFTTYPE = "nft type";

    public static string ANIMATION_DATA = "AnimationData";
    public static string EMOTE_SELECTION_INDEX = "EmoteAnimSelectionIndex";
    public static string SELECTED_ANIMATION_NAME = "selectedAnimName";

    public static string POSTTIMESTAMP = "post time";
    public static string POSTDESCRIPTION = "post description";
    public static string POSTUSERNAME = "post username";
    public static string POSTMEMBERSCOUNT = "member count";


    public static string NFTTHUMB = "nft thumb";
    public static string NFTOWNER = "nft owner";
    public static string NFTCREATOR = "nft creator";
    public static string NFTDES = "nft des";
    public static string NFTLINK = "nft link";


    public static string API = "https://api.xana.net/";
    public static string SERVER = "ws://socket-lb-648131231.us-east-2.elb.amazonaws.com:3000";

    public static string UserPriorityRole = "Guest";
    public static List<string> UserRoles = new List<string>() { "Guest"};

    #region World Manager

    public static string GETENVIRONMENTSAPI = API + "xanaEvent/getEnvironments";
    public static string JWTTOKEN = "JWT eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoxOTc4NiwidXNlcm5hbWUiOiJueWxhIiwiZXhwIjoxNjAzMjYwNjUxLCJlbWFpbCI6Im5heWxhLm5vYm9yZGVyc0BnbWFpbC5jb20iLCJvcmlnX2lhdCI6MTYwMzI1NzA1MX0.zJKxLtvBLK-uf4kdmc5b20r4iSkpFLfv5So2c0oBc0U";


    //All Item Api
    public static string GETALLSTOREITEMCATEGORY = "/item/all-category";
    public static string GETALLSTOREITEMSUBCATEGORY = "/item/subcategories";
    public static string GETALLSTOREITEMS = "/item/all-items";



    public static string GETENVIRONMENTSAPINew = "/item/environment";
    public static string GetAllMuseumsAPI = "/item/museums/2/1/25";
   // public static string GetAllMuseumsAPI = "/item/museums/v2/1/20";
    public static string BACKGROUNDFILES = "/item/background-files";
    public static string ANIMATIONFILES = "/item/animations";
    public static string FILTERPROFILE = "/item/get-filter-assets";
    public static string UPLOADFILE = "/item/upload-file";
    public static string OCCUPIDEASSETS = "/item/get-user-occupied-asset/";
    public static string DELETEOCCUPIDEUSER = "/item/delete-user-occupied-asset/";
    public static string CREATEOCCUPIDEUSER = "/item/create-user-occupied-asset";
    public static string UPDATEOCCUPIDEUSER = "/item/update-user-occupied-asset/";
    public static string SHARELINKS = "/item/shareLinks";
    public static string SHAREDEMOS = "/item/shareDemos";
    public static string YOUTUBEVIDEOBYSCENE = "/item/v2/shareLinks/"; //scene name 

    public static string GetDefaultAPI = "/items/get-items-with-defaults";
    // public static string GetUserDetailsAPI = "users/single-user";
    public static string PurchasedAPI = "/items/purchase-items";
    public static string SendCoinsAPI = "/users/update-coins";
    public static string WALLETSTATUS = "/auth/get-wallet-status";
    public static string GETSETS = "/item/get-sets";
    public static string LogoutFromotherDeviceAPI = "/auth/logout-from-other";
    public static string SendEmailOTP = "/auth/send-otp-for-email";
    public static string VerifyEmailOTP = "/auth/otp-verify-for-email";
    public static string RegisterWithEmail = "/auth/register-with-email";
    public static string LoginAPIURL = "/auth/sign-in";
    public static string NameAPIURL = "/users/set-name";
    public static string ChangePasswordAPI = "/users/change-password";
    public static string UpdateProfileAPI = "/users/update-profile";
    public static string DeleteAPI = "/users/delete-account";
    public static string GetUserDetailsAPI = "/users/single-user";
    public static string SetDeviceTokenAPI = "/users/set-device-token";
    public static string LogOutAPI = "/users/logout";
    public static string UpdateAvatarAPI = "/users/update-avatar";
    //[Space(10)]
    //[Header("Total-API-Phone")]
    public static string SendPhoneOTPAPI = "/auth/send-otp-for-phone";
    public static string VerifyPhoneOTPAPI = "/auth/otp-verify-for-phone";
    public static string RegisterPhoneAPI = "/auth/register-with-phone";
    public static string ResendOTPAPI = "/auth/resend-otp";
    //[Header("Total-API-ForgetPassword")]
    public static string ForgetPasswordAPI = "/auth/forgot-password";
    public static string ForgetPasswordOTPAPI = "/auth/verify-forgot-password-otp";
    public static string ForgetPasswordResetAPI = "/auth/reset-password";
    //[Header("Guest API")]
    public static string guestAPI = "/auth/login-as-guest";
    public static string GetAllAnimatons = "/item/animations";
    public static string GetAllReactions = "/item/get-all-reactions";
    public static string GetVersion = "/item/get-version";
    public static string MaintenanceAPI = "/item/get-version/";

    #endregion

    #region SNS Managers    
    public static string r_privacyPolicyLink = "https://cdn.xana.net/xanaprod/privacy-policy/PRIVACYPOLICY-2.pdf";
    public static string r_termsAndConditionLink = "https://cdn.xana.net/xanaprod/privacy-policy/termsofuse.pdf";

    // public static string r_mainURL = "https://app-api.xana.net";
    public static string r_AWSImageKitBaseUrl = "https://aydvewoyxq.cloudimg.io/_xana_/";

    public static string r_url_GetAllUsersWithFeeds = "/hot/all-users-with-feeds";
    public static string r_url_GetFeedsByUserId = "/hot/feeds";
    public static string r_url_GetFeedsByFollowingUser = "/feeds/following";
    public static string r_url_GetTaggedFeedsByUserId = "/hot/tagged-feeds";

    public static string r_url_FollowAUser = "/follow/user";
    public static string r_url_GetAllFollowing = "/follow/get-all-following";
    public static string r_url_GetAllFollowers = "/follow/get-all-followers";
    public static string r_url_MakeFavouriteFollower = "/follow/make-fav";
    public static string r_url_UnFollowAUser = "/follow/unfollow-user";

    public static string r_url_AllFeed = "/feeds";
    public static string r_url_CommentFeed = "/feeds/comment-feed";
    public static string r_url_FeedCommentList = "/feeds/feed-comment-list";
    public static string r_url_CreateFeed = "/feeds/single-feed";
    public static string r_url_DeleteFeed = "/feeds/delete-feed";
    public static string r_url_EditFeed = "/feeds/edit-feed";
    public static string r_url_DeleteComment = "/feeds/feed-comment-delete";

    public static string r_url_FeedLikeDisLike = "/feeds/like-dislike-post";

    public static string r_url_SearchUser = "/users/search-user";
    public static string r_url_WebsiteValidation = "/auth/check-website-validity";

    public static string r_url_SetName = "/users/set-name";
    public static string r_url_ChangePassword = "/users/change-password";
    public static string r_url_GetUserDetails = "/users/single-user";
    public static string r_url_UpdateUserAvatar = "/users/update-avatar";
    public static string r_url_UpdateUserProfile = "/users/update-profile";
    public static string r_url_GetSingleUserProfile = "/follow/get-single-profile";
    public static string r_url_GetSingleUserRole = "/user/get-user-role?xanaId=";
    public static string r_url_DeleteAccount = "/users/delete-account";

    public static string r_url_ChatCreateGroup = "/chat/create-group";
    public static string r_url_AddGroupMember = "/chat/add-group-member";
    public static string r_url_UpdateGroupInfo = "/chat/update-group";
    public static string r_url_ChatCreateMessage = "/chat/create-msg";
    public static string r_url_ChatGetAttachments = "/chat/get-attachments";
    public static string r_url_ChatGetConversation = "/chat/get-conversation";
    public static string r_url_ChatMuteUnMuteConversation = "/chat/mute-unmute-conversation";
    public static string r_url_ChatGetMessages = "/chat/get-messages";
    public static string r_url_GetAllChatUnReadMessagesCount = "/chat/get-all-message-unreadCount";
    public static string r_url_LeaveTheChat = "/chat/leave-chat-group";
    public static string r_url_DeleteConversation = "/chat/delete-conversation";
    public static string r_url_DeleteChatGroup = "/chat/delete-chat-group";
    public static string r_url_RemoveGroupMember = "/chat/remove-group-member";
    #endregion

    #region Xanalia Api and Wallet Connection
    public static string API_BASEURL_XANALIA = "https://api.xanalia.com";
   
    public static readonly string userMy_Collection_Xanalia = "/user/my-collection";
    public static readonly string getUserProfile_Xanalia = "/user/get-user-profile";

    public static readonly string GetUserNounceURL = "/auth/get-user-nonce";
    public static readonly string VerifySignedURL = "/auth/verify-signature";
    //public static readonly string NameAPIURL = "";
    public static readonly string VerifySignedXanaliaURL = "/auth/verify-signature";
    public static readonly string GetXanaliaNounceURL = "/auth/get-address-nonce";
    public static readonly string GetXanaliaNFTURL = "/xanalia/mydata";
    public static readonly string GetGroupDetailsAPI = "/item/get-sets";


    #endregion

    #region GrammyAward Api

    public static readonly string getAnimationTime = "/item/get-timeCount";
    #endregion
}
