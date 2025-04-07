using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AWSHandler : MonoBehaviour
{
    public static AWSHandler Instance;
    public string Bucketname = "s3-congnito-file-upload-demo";

    public string IdentityPoolId = "ap-southeast-1:0543f963-dd08-48ff-b3e3-8669eb5defa0";
    public RegionEndpoint CognitoIdentityRegion = RegionEndpoint.GetBySystemName("ap-southeast-1");
    public string fileName;
    public String URL;
    public AmazonS3Client _s3Client;
    public AWSCredentials _credentials;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        //Instance = this;
    }

    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        if (_s3Client == null)//_s3Client initialize.......
        {
            _s3Client = new AmazonS3Client(Credentials, CognitoIdentityRegion);
        }
    }

#if UNITY_ANDROID
    public void UsedOnlyForAOTCodeGeneration()
    {
        //Bug reported on github https://github.com/aws/aws-sdk-net/issues/477
        //IL2CPP restrictions: https://docs.unity3d.com/Manual/ScriptingRestrictions.html
        //Inspired workaround: https://docs.unity3d.com/ScriptReference/AndroidJavaObject.Get.html

        AndroidJavaObject jo = new AndroidJavaObject("android.os.Message");
        int valueString = jo.Get<int>("what");
    }
#endif

    private AWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials(IdentityPoolId, CognitoIdentityRegion);
            return _credentials;
        }
    }

    public IAmazonS3 Client
    {
        get
        {
            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(Credentials, CognitoIdentityRegion);
            }
            //test comment
            return _s3Client;
        }
    }

    public void PostObject(string fileName, string key)
    {
        Debug.LogError("Retrieving the file");

        MessageController.Instance.LoaderShow(true);//rik loader active.......

        currentSNSApiLoaderController = MessageController.Instance.apiLoaderController;
        if (currentSNSApiLoaderController != null)
        {
            currentSNSApiLoaderController.ShowUploadStatusImage(true);
        }

        var stream = new FileStream(fileName,
        FileMode.Open, FileAccess.Read, FileShare.Read);

        if (stream == null)
            Debug.LogError("Null Stream");

        Debug.LogError("\nCreating request object:" + stream.Length);
        var request = new PutObjectRequest()
        {
            BucketName = Bucketname,
            Key = key,
            InputStream = stream,
            CannedACL = S3CannedACL.Private
        };
        Debug.LogError(request.InputStream.Length);

        request.StreamTransferProgress += new EventHandler<StreamTransferProgressArgs>((s, e) => UploadMessagePartProgressEventCallback(s, e));
        Debug.LogError("\nMaking HTTP post call");
        Client.PutObjectAsync(request, (responseObj) =>
        {
            if (currentSNSApiLoaderController != null)
            {
                currentSNSApiLoaderController.ShowUploadStatusImage(false);
            }

            string attechmentstr = "[" + MessageController.Instance.attechmentArraystr + key + MessageController.Instance.attechmentArraystr + "]";
            if (responseObj.Exception == null)
            {
                Debug.LogError("attachment str:" + attechmentstr);
                // getUrl(key);
                if (MessageController.Instance.allChatGetConversationDatum.receivedGroupId != 0)
                {
                    APIManager.Instance.RequestChatCreateMessage(0, MessageController.Instance.allChatGetConversationDatum.receivedGroupId, "", "", attechmentstr);
                }
                else if (MessageController.Instance.allChatGetConversationDatum.receiverId != 0)
                {
                    if (MessageController.Instance.allChatGetConversationDatum.receiverId == APIManager.Instance.userId)
                    {
                        APIManager.Instance.RequestChatCreateMessage(MessageController.Instance.allChatGetConversationDatum.senderId, 0, "", "", attechmentstr);
                    }
                    else
                    {
                        APIManager.Instance.RequestChatCreateMessage(MessageController.Instance.allChatGetConversationDatum.receiverId, 0, "", "", attechmentstr);
                    }
                }
                else
                {
                    if (MessageController.Instance.isDirectMessage)
                    {
                        if (MessageController.Instance.CreateNewMessageUserList.Count > 0)
                        {
                            MessageController.Instance.isDirectMessageFirstTimeRecivedID = MessageController.Instance.CreateNewMessageUserList[0];
                            Debug.LogError("Direct One To One Image Share:" + MessageController.Instance.isDirectMessageFirstTimeRecivedID);
                            APIManager.Instance.RequestChatCreateMessage(int.Parse(MessageController.Instance.CreateNewMessageUserList[0]), 0, "", "", attechmentstr);
                        }
                    }
                }
                Debug.LogError(string.Format("\nobject {0} posted to bucket {1}", responseObj.Request.Key, responseObj.Request.BucketName));
            }
            else
            {                
                MessageController.Instance.LoaderShow(false);//rik loader false.......

                Debug.LogError("\nException while posting the result object" + string.Format("\n receieved error {0}", responseObj.Response.HttpStatusCode.ToString()));
            }
        });
    }

    private void UploadMessagePartProgressEventCallback(object s, StreamTransferProgressArgs e)
    {
        Debug.LogError("Uploading done = " + e.PercentDone);
        if (currentSNSApiLoaderController != null)
        {
            int per = (e.PercentDone * 2);
            Debug.LogError("Uploading done multiple = " + per);
            if (per > 100)
            {
                per = 100;
            }
            currentSNSApiLoaderController.UploadingStatus(per);
        }
    }

    public void PostAvatarObject(string fileName, string key, string CallingFrom)
    {
        Debug.LogError("PostAvatarObject calling.......filename:" + fileName + "    :key:" + key + "    :callingFrom:" + CallingFrom);

        var stream = new FileStream(fileName,
        FileMode.Open, FileAccess.Read, FileShare.Read);

        if (stream == null)
            Debug.LogError("Null Stream");

        Debug.LogError("\nCreating request object:" + stream.Length);
        var request = new PutObjectRequest()
        {
            BucketName = Bucketname,
            Key = key,
            InputStream = stream,
            CannedACL = S3CannedACL.Private
        };
        Debug.LogError(request.InputStream.Length);

        request.StreamTransferProgress += new EventHandler<StreamTransferProgressArgs>((s, e) => UploadPartProgressEventCallback(s, e));
        Debug.LogError("\nMaking HTTP post call");
        Client.PutObjectAsync(request, (responseObj) =>
        {
            //string attechmentstr = "[" + MessageController.Instance.attechmentArraystr + key + MessageController.Instance.attechmentArraystr + "]";
            if (responseObj.Exception == null)
            {
                Debug.LogError("PostAvatarObject success Attachment str:" + key);
                switch (CallingFrom)
                {
                    case "CreateGroupAvatar":
                        MessageController.Instance.CreateNewGroup(key);
                        break;
                    case "UpdateUserAvatar":
                        APIController.Instance.UpdateAvatarOnServer(key, CallingFrom);
                        break;
                    case "EditProfileAvatar":
                        APIController.Instance.UpdateAvatarOnServer(key, CallingFrom);
                        break;
                    default:
                        break;
                }
                Debug.LogError(string.Format("\nobject {0} posted to bucket {1}", responseObj.Request.Key, responseObj.Request.BucketName));
            }
            else
            {
                Debug.LogError("PostAvatarObject Failed  Attachment str:" + key);
                switch (CallingFrom)
                {
                    case "CreateGroupAvatar":
                        MessageController.Instance.CreateNewGroup("");
                        break;
                    case "EditProfileAvatar":
                        APIManager.Instance.RequestGetUserDetails(CallingFrom);
                        break;
                    default:
                        break;
                }
                Debug.LogError("\nException while posting the result object" + string.Format("\n receieved error {0}", responseObj.Response.HttpStatusCode.ToString()));
            }
        });
    }

    public SNSAPILoaderController currentSNSApiLoaderController = new SNSAPILoaderController();

    //this method is used to post object in feed.
    public void PostObjectFeed(string fileName, string key, string callingFrom)
    {
        switch (callingFrom)
        {
            case "CreateFeed":
                currentSNSApiLoaderController = FeedUIController.Instance.apiLoaderController;
                break;
            case "CreateFeedRoom":
                currentSNSApiLoaderController = ARFaceModuleManager.Instance.apiLoaderController;
                break;
            default:
                Debug.LogError("Default case");
                currentSNSApiLoaderController = FeedUIController.Instance.apiLoaderController;
                break;
        }

        if (currentSNSApiLoaderController != null)
        {
            currentSNSApiLoaderController.ShowUploadStatusImage(true);
        }

        Debug.LogError("Retrieving the file fileName:" + fileName + " :Key:" + key + "  :CallingFrom:" + callingFrom);

        var stream = new FileStream(fileName,
        FileMode.Open, FileAccess.Read, FileShare.Read);

        if (stream == null)
            Debug.LogError("Null Stream");

        Debug.LogError("\nCreating request object:" + stream.Length);
        var request = new PutObjectRequest()
        {
            BucketName = Bucketname,
            Key = key,
            InputStream = stream,
            CannedACL = S3CannedACL.Private
        };
        Debug.LogError(request.InputStream.Length);

        request.StreamTransferProgress += new EventHandler<StreamTransferProgressArgs>((s, e) => UploadPartProgressEventCallback(s, e));
        Debug.LogError("\nMaking HTTP post call");
        Client.PutObjectAsync(request, (responseObj) =>
        {
            //string feedUrl = "[" + FeedUIController.Instance.attechmentArraystr + key + FeedUIController.Instance.attechmentArraystr + "]";
            
            string feedUrl = key;
            if (responseObj.Exception == null)
            {
                Debug.LogError("attachment str:" + feedUrl);

                switch (callingFrom)
                {
                    case "CreateFeed":
                        FeedUIController.Instance.CreateFeedAPICall(feedUrl);
                        break;
                    case "CreateFeedRoom":
                        ARFaceModuleManager.Instance.CreateFeedAPICall(feedUrl);
                        break;
                    default:
                        Debug.LogError("Default case");
                        if (currentSNSApiLoaderController != null)
                        {
                            currentSNSApiLoaderController.ShowUploadStatusImage(false);
                        }
                        FeedUIController.Instance.ShowLoader(false);//active Loader false.......
                        break;
                }

                Debug.LogError(string.Format("\nobject {0} posted to bucket {1}", responseObj.Request.Key, responseObj.Request.BucketName));
            }
            else
            {
                if (currentSNSApiLoaderController != null)
                {
                    currentSNSApiLoaderController.ShowUploadStatusImage(false);
                }

                switch (callingFrom)
                {
                    case "CreateFeed":
                        FeedUIController.Instance.ShowLoader(false);//active Loader false.......
                        break;
                    case "CreateFeedRoom":
                        ARFaceModuleManager.Instance.ShowLoader(false);
                        break;
                    default:
                        Debug.LogError("Default case");
                        FeedUIController.Instance.ShowLoader(false);//active Loader false.......
                        break;
                }               
                Debug.LogError("\nException while posting the result object" + string.Format("\n receieved error {0}", responseObj.Response.HttpStatusCode.ToString()));
            }
        });
    }
       
    public void getUrl(string key)
    {
        var request_1 = new GetPreSignedUrlRequest()
        {
            BucketName = Bucketname,
            Key = key,
            Expires = DateTime.Now.AddHours(6)
        };
        Debug.LogError("sending url request");
        _s3Client.GetPreSignedURLAsync(request_1, (callback) =>
        {
            if (callback.Exception == null)
            {
                URL = callback.Response.Url;

                Debug.LogError("URL " + URL);
            }
            else
                Debug.LogError(callback.Exception);
        });
    }

    private void UploadPartProgressEventCallback(object s, StreamTransferProgressArgs e)
    {
        Debug.LogError("Uploading done = " + e.PercentDone);
        if (currentSNSApiLoaderController != null)
        {
            int per = (e.PercentDone * 2);
            Debug.LogError("Uploading done multiple = " + per);
            if (per > 100)
            {
                per = 100;
            }
            currentSNSApiLoaderController.UploadingStatus(per);
        }
    }

    public void GetObject(string key)
    {
        //  ResultText.text = string.Format("fetching {0} from bucket {1}", SampleFileName, S3BucketName);
        Debug.LogError("GetObj AWS key:" + key);
        Client.GetObjectAsync(Bucketname, key, (responseObj) =>
        {
            string data = null;
            var response = responseObj.Response;
            if (response.ResponseStream != null)
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    data = reader.ReadToEnd();
                }
                Debug.LogError("ImageConversion recieved111:"+data);
            }
        });
    }

    public void GetObjects()
    {
        var request = new ListObjectsRequest()
        {
            BucketName = Bucketname
        };

        Client.ListObjectsAsync(request, (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                Debug.LogError("Got Response \nPrinting now \n");
                responseObject.Response.S3Objects.ForEach((o) =>
                {
                    Debug.LogError(string.Format("{0}\n", o.Key));
                });
            }
            else
            {
                Debug.LogError("Got Exception \n");
            }
        });
    }    
}