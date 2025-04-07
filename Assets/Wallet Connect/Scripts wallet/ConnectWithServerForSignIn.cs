using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using System.Net;
using System.Text;
using WebSocketSharp;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
public class ConnectWithServerForSignIn : MonoBehaviour
{
    public GameObject Page1;
    public GameObject Page2;
    public GameObject Page3;
    public GameObject Page4;  
    public GameObject Page5;  
    WebSocket websocket;
    public Text msgtxt;    
    public Text SignedTxt;   
    public string AppID;
    public GameObject QRGenrate;
    public string AppUrlForAndroid;
    public string bundleIdofLunchingApp;
    bool CheckLunchingFail = false;  
    Sprite mySprite;
    public string JsonIDString;
    public InputField MsgField;
    public string GetUserNounceURL = "";
    public string VerifySignedURL = "";
    private string ServerNounce;
    private GameObject WalletLoginLoader;

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UnityOnStart(int num);
#endif
    string uniqueID()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        int z1 = UnityEngine.Random.Range(0, 1000000);
        int z2 = UnityEngine.Random.Range(0, 1000000);
        string uid = currentEpochTime + ":" + z1 + ":" + z2;
        return uid;
    }
 
    public void OpenMenu(string menuName)
    {
        Page1.SetActive(false);
        Page2.SetActive(false);
        Page3.SetActive(false);
        Page4.SetActive(false);
        Page5.SetActive(false);

        switch (menuName)
        {

            case "GenerateQR":
                {
                    Page2.SetActive(true);
                    print("Implement QR generate");
                    GenerateQRCode();
                    break;
                }

            case "connected":
                {
                    Page3.SetActive(true);
                     break;
                } 
            case "MessageForwardToWallet":
                {
                     Page4.SetActive(true);
                    break;
                }  
            
            case "disconnected":
                {
                    WalletLoginLoader.SetActive(false);
                    Page1.SetActive(true);
                     break;
                }
            case "Rejected":
                {
                    WalletLoginLoader.SetActive(false);
                     print("I am in rejected Case");
                    Page1.SetActive(true);    
                     break;  
                }
                  
            case "VerifySignature":
                {
                    Page4.SetActive(true);
                    break;
                }     
            
            case "OpenJWTPage":
                {
                    WalletLoginLoader.SetActive(false);

                    Page5.SetActive(true);
                    break;
                }  
        }  

    }

 

    // Start is called before the first frame update
    void Start()
    {
          Page1.SetActive(true);
        Page2.SetActive(false);     
           msgtxt.text = "No msg";  
    websocket = new WebSocket("ws://54.255.221.170:9898/");
    websocket.OnOpen += (o, e) => {
      Debug.Log("Open");  
    };      
     websocket.Connect();  
    websocket.OnMessage += (o, e) => {
          ExampleMainThreadCall(e.Data);
     };      
   }

    public void ExampleMainThreadCall(string getText)
    {
         UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread(getText));
    }  
    public IEnumerator ThisWillBeExecutedOnTheMainThread(string txt)
    {
        Debug.Log("This is executed from the main thread");
        GetText(txt);  
          yield return null;
    }  
     void GetText(string txt)  
    {
         print(txt);   
       //  txt = JsonIDString;  
        ConnectServerDataExtraction.SuccessClass objSuccess = new ConnectServerDataExtraction.SuccessClass();
        ConnectServerDataExtraction.ConnectedClass objConnected = new ConnectServerDataExtraction.ConnectedClass();
        ConnectServerDataExtraction.VerifySignatureClass objVerify = new ConnectServerDataExtraction.VerifySignatureClass();
        ConnectServerDataExtraction.ErrorClass objerror= new ConnectServerDataExtraction.ErrorClass();
        objSuccess = null;
         objConnected = null;
        objVerify = null;
        objerror = null;
        ConnectServerDataExtraction.CommonObjectFields commonFields = JsonUtility.FromJson<ConnectServerDataExtraction.CommonObjectFields>(txt);    
         switch (commonFields.type)
            {    
            case "success":           
                print("Success ");       
                objSuccess = JsonUtility.FromJson<ConnectServerDataExtraction.SuccessClass>(txt);        
               break;     
            case "connected":      
                 print("Connected ");    
                objConnected = JsonUtility.FromJson<ConnectServerDataExtraction.ConnectedClass>(txt);     
                //  DoSomeThingWith(JsonUtility.FromJson<House>(json));   
                break;    
            case "disconnect":   
                print("Disconnected");   
                 objSuccess = JsonUtility.FromJson<ConnectServerDataExtraction.SuccessClass>(txt);
                break;
            case "rejected":
                print("rejected here");
                objSuccess = JsonUtility.FromJson<ConnectServerDataExtraction.SuccessClass>(txt);
                break;
             case "verifysig":
                print("type verifysig");
                objVerify = JsonUtility.FromJson<ConnectServerDataExtraction.VerifySignatureClass>(txt);
                break;
            case "error":
                Debug.LogError("type Error");
                objerror = JsonUtility.FromJson<ConnectServerDataExtraction.ErrorClass>(txt);
                break;
        }  

      if(objSuccess != null )
        {
             if (objSuccess.data == "app disconnected")
            {
                print("app disconnected");
                 OpenMenu("disconnected");
             }
            else if(objSuccess.data == "msg forwarded to wallet to get signed")
            {
                print("msg forwarded to wallet");
                 OpenMenu("MessageForwardToWallet");   
            }
             else if (commonFields.type == "rejected")
            {
                print("Connect Request Rejected");  
                  OpenMenu("Rejected");
             }      
             else
            {
                OpenMenu("GenerateQR");
             }      
          }  
      if(objConnected != null)
        {
            OpenMenu("connected");
            print("Implement COnnnected");
             print("Wallet address is " + objConnected.data.address);
              print("Wallet id is " + objConnected.data.walletId);
             print("Wallet msg is " + objConnected.data.msg);
              string walletPublicID = objConnected.data.address;
            ConnectServerDataExtraction.NounceClass NounceObj = new ConnectServerDataExtraction.NounceClass();  
            NounceObj = NounceObj.NounceClassFtn(walletPublicID);
            var jsonObj = JsonUtility.ToJson(NounceObj);
            print("Nouce JSON is  " + jsonObj);
            StartCoroutine(HitGetNounceFromServerAPI(GetUserNounceURL, jsonObj));
         }  
        if (objVerify != null)
        {  
            print("public key is " + objVerify.data.pubKey);
            print("signature key is " + objVerify.data.sig);
            SignedTxt.text = objVerify.data.sig;     
             OpenMenu("VerifySignature");
              string SignedSignature = objVerify.data.sig;
            ConnectServerDataExtraction.VerifySignedMsgClass VerifySignatureObj = new ConnectServerDataExtraction.VerifySignedMsgClass();
            VerifySignatureObj = VerifySignatureObj.VerifySignedClassFtn(ServerNounce, objVerify.data.sig);
            var jsonObj = JsonUtility.ToJson(VerifySignatureObj);
            print("Verify Signed msg Json is  " + jsonObj);
            StartCoroutine(HitVerifySignatureAPI(VerifySignedURL, jsonObj));
         }     
        if (objerror != null)
        {
            WalletLoginLoader.SetActive(false);
             Debug.LogError(objerror.data); 
            Debug.LogError(objerror.type);
        }  
         
         msgtxt.text = txt;
    }    
      public void GenerateMsg()      
    {       
        
         MsgField.text = ServerNounce ;
           ConnectServerDataExtraction.GenerateMsgClass MsgGenObj = new ConnectServerDataExtraction.GenerateMsgClass();
        //  GenerateMsgClass MsgGenObj = new GenerateMsgClass();   
        MsgGenObj = MsgGenObj.msgClassFtn(ServerNounce, AppID);     
         var jsonObj = JsonUtility.ToJson(MsgGenObj);   
        print("Asking class "+jsonObj);      
        websocket.Send(jsonObj);        
     }            
     /*
    [Serializable]
    class GenerateMsgClass
    {
        public string type = "ask";
        public msgClassData data = null ;  
        public GenerateMsgClass  msgClassFtn(String msg ,  string myAppID )
        {
            GenerateMsgClass obj1 = new GenerateMsgClass();
            msgClassData Obj2 = new msgClassData();
            obj1.type = "ask";   
            obj1.data = Obj2.getData2(msg , myAppID);
            return obj1;    
        }        
    }
  [Serializable]
    class msgClassData
    {
        public string msg= "msg here";
        public string appId= "app id here";
       public msgClassData getData2(String msg1, string myAppID)
        {
             msgClassData Obj1 = new msgClassData();
            Obj1.msg = msg1;
            Obj1.appId = myAppID;  
             return Obj1;
        }
    }
    */

     void GenerateQRCode()
    {
        newGenrate();
    }


    public void OpenXanaliaApp()
    {
        print("Open Xanalia");
#if UNITY_IOS && !UNITY_EDITOR
        UnityOnStart(int.Parse(AppID));
#endif
#if UNITY_ANDROID || UNITY_EDITOR
        OpenAppForAndroid();
#endif 
    }

    void OpenAppForAndroid()
    {
        string message = "xanaliaapp://connect/";
        message += AppID.ToString();
         print(message);
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleIdofLunchingApp);
            launchIntent.Call<AndroidJavaObject>("putExtra", "arguments", message);
        }
        catch (System.Exception e)
        {  
            CheckLunchingFail = true;
        }  
        if (CheckLunchingFail)
        {
            Debug.Log("app not found");
            Application.OpenURL(message); 
        }        
        else
        {
             ca.Call("startActivity", launchIntent);
        }  
        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }
    public void OpenXanaliaAppWithURL()
    {
        print("Open Xanalia");
        OpenAppForAndroidURL();
    }  

    void OpenAppForAndroidURL()
    {
        string message = "xanaliaapp://connect/";
        message += AppID.ToString();
        print(message);
        Application.OpenURL(message);
     }






    /*
     [Serializable]
    class CommonObjectFields
    {
        public string type;
        public string data;
     }

    [System.Serializable]
    class SuccessClass
    {
        public string type;
        public string data;
    }
       
    [System.Serializable]
    class ConnectedClass
    {
        public string type;
        public ReadResponse2 data;
     }


    [System.Serializable]
    class ReadResponse
    {
        public string type;
        public ReadResponse2 data;
         public ReadResponse returnResponse(string getText)
        {
            ReadResponse obj = new ReadResponse();
            obj = JsonUtility.FromJson<ReadResponse>(getText);
             return obj; 
        }  
     }

    [System.Serializable]
    class ReadResponse2
    {
        public string msg;
        public string walletId;
        public string address;  
        public ReadResponse returnResponse(string getText)
        {
            ReadResponse obj = new ReadResponse();
            obj = JsonUtility.FromJson<ReadResponse>(getText);
            return obj;
        }
    }
    */
    public void VerifySignature()
    {
        print("Signature verify here ");
    }
     public void DisconnectRequestToServer()
    {
        ConnectServerDataExtraction.Disconnect1 dataObj = new ConnectServerDataExtraction.Disconnect1();
       //  Disconnect1 dataObj = new Disconnect1();
        dataObj = dataObj._Disconnect(AppID);
        var jsonObj = JsonUtility.ToJson(dataObj);
        print(jsonObj);
        // print(new JsonObject(JsonUtility.ToJson(dataObj)).ToString());
        websocket.Send(jsonObj);    
    }     
    /*
    [System.Serializable]
    class Disconnect1
    {
        public string type = "disconnect";
        public Disconnect2 data;
        public Disconnect1 _Disconnect(string _AppID)
        {
            Disconnect1 myObj = new Disconnect1();
            Disconnect2 obj2 = new Disconnect2();
             myObj.type = type;
            myObj.data = obj2._Disconnect2(_AppID);
            return myObj;  
         }  
     }
       
     [System.Serializable]
    class Disconnect2
    {
        public string type = "app_disconnect";
        public Disconnect3 data;
        public Disconnect2 _Disconnect2(string _AppID)
        {
            Disconnect2 myObj = new Disconnect2();
            Disconnect3 myObj2 = new Disconnect3();
             myObj.type = type;
            myObj.data = myObj2._Disconnect3(_AppID);
            return myObj;  
        } 
    }
    [System.Serializable]
    class Disconnect3
    {  
        public string appId = "";
        public Disconnect3 _Disconnect3(string _AppID)   
        {
            Disconnect3 myObj = new Disconnect3();
            myObj.appId = _AppID.ToString();
             return myObj;
        }   
     }  
    */
  public void ConnectingRequestToServer()   
    {
        WalletLoginLoader = EventSystem.current.currentSelectedGameObject;
        WalletLoginLoader = WalletLoginLoader.transform.Find("Loader").gameObject;
         if (WalletLoginLoader == null)
        {
            return;
        }
        if (WalletLoginLoader.activeInHierarchy)
            return;
        WalletLoginLoader.SetActive(true);
 
        AppID = uniqueID();
         websocket.Connect();
    ConnectServerDataExtraction.first11 dataObj = new ConnectServerDataExtraction.first11();   
  //   first11 dataObj = new first11();
    dataObj = dataObj.getData(AppID);
    var jsonObj = JsonUtility.ToJson(dataObj);    
    print(jsonObj);    
    // print(new JsonObject(JsonUtility.ToJson(dataObj)).ToString());
   websocket.Send(jsonObj);   
    } 
    
 
    /*
  [System.Serializable]
  class first11
  {
    public string type = "connect";
    public first22 data;
    public first11 getData(string myAppID)
    {  

      first11 Obj1 = new first11();
       first22 Obj2 = new first22();
        Obj1.data = Obj2.getData(myAppID);
      return Obj1;
    } 
  }
  [System.Serializable]
  class first22
  {
    public string type = "app";
    public data2 data;

    public first22 getData(string myAppID)
    {
      first22 Obj1 = new first22();
     data2 Obj2 = new data2();
      Obj1.type = type;
      Obj1.data = Obj2.GetConnectValue(myAppID);
       return Obj1;
    }
    }

    /*
    "type": "connect",
    "data":{"type":"app", 
    "data":{ "appId":"102", "name":"myApp", "description":"this is my app discription","icon":"http://iconurl.png","url":"http://myappurl.com"
     */
    /*
[System.Serializable]
  class data2
  {   
    public string appId = "11";
    public string name = "XANA";
    public string description = "hello i come from xana";    
    public string icon = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/Icon+and+Logo/logo.png";  
    public string url = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/Icon+and+Logo/XANA+2021.09+logo+1.png";
      public data2 GetConnectValue(string myAppID)
    {
      data2 Obj = new data2();    
      Obj.appId = myAppID;
      Obj.name = name;
      Obj.description = description;
      Obj.icon = icon;
      Obj.url = url;
       return Obj;  
    }     
  }
    */  

    private void newGenrate()
    {
        Texture2D myQR = generateQR(AppID.ToString());
        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
         QRGenrate.GetComponent<Image>().sprite = mySprite;
    } 
      
    private Texture2D generateQR(string text)  
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }


    // API,s calling
    IEnumerator HitGetNounceFromServerAPI(string url, string Jsondata)
    {
        // print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");  
        yield return request.SendWebRequest();    
          print("Json data is  = " + request.downloadHandler.text);  
        //  NounceMsg1
         ConnectServerDataExtraction.NounceMsg1 NounceReadObj = JsonUtility.FromJson<ConnectServerDataExtraction.NounceMsg1>(request.downloadHandler.text);  
      if (!request.isHttpError && !request.isNetworkError)
    {  
         if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            if (NounceReadObj.success)
            {
                    print(" in Success Nounce Is here " + NounceReadObj.data.nonce);
                    ServerNounce = NounceReadObj.data.nonce;
                    GenerateMsg();
             }
        }
    }
    else
    {
        if (request.isNetworkError)
        {
                DisconnectRequestToServer();
            Debug.LogError("Network error in Get Nounce");
        }
        else
        {
            if (request.error != null)
            {
                 if (!NounceReadObj.success)
                {
                        DisconnectRequestToServer();
                        Debug.LogError("Success false in  get Nounce");  
                }
            }
        } 
    }
 }


    IEnumerator HitVerifySignatureAPI(string url, string Jsondata)
    {
        // print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        print("Json data of Signed signature is   = " + request.downloadHandler.text);

      
         //  ConnectServerDataExtraction.VerifyReadSignedMsgFromServer VerifySignatureReadObj = JsonUtility.FromJson<ConnectServerDataExtraction.VerifyReadSignedMsgFromServer>(request.downloadHandler.text);
         ConnectServerDataExtraction.ClassWithToken VerifySignatureReadObj = new ConnectServerDataExtraction.ClassWithToken();
        VerifySignatureReadObj = ConnectServerDataExtraction.ClassWithToken.CreateFromJSON(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
           {
               if (request.error == null)
               {
                   Debug.Log(request.downloadHandler.text);
                   if (VerifySignatureReadObj.success)
                   {
                     OpenMenu("OpenJWTPage");
                       print("JWT token of user is  " + VerifySignatureReadObj.data.token);
                    print("JWT token of user ID  " + VerifySignatureReadObj.data.user.id);
                 }
            }  
           }
           else
           {
               if (request.isNetworkError)
               {
                   DisconnectRequestToServer();
                   Debug.LogError("Network error in Verify signature");
               }
               else
               {
                   if (request.error != null)
                   {
                       if (!VerifySignatureReadObj.success)
                       {
                           DisconnectRequestToServer();
                           Debug.LogError("Success false in  verify sig");
                       }
                   }
               }
           }
     }






}





// 




///************************************   GET Arguments In Android     ***********************************//
///
/*
 public Text argumentTxt;
    private bool focusbool;
    // Start is called before the first frame update
    void Start()
    {
        UpdateArguments();
     }  


    public void UpdateArguments()
    {
        string arguments = "";
          AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
         AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        bool hasExtra = intent.Call<bool>("hasExtra", "arguments");
        if (hasExtra)
        {
            AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
            arguments = extras.Call<string>("getString", "arguments");
            argumentTxt.text = arguments;
        }    
        else
        {
            argumentTxt.text = "No orguments";
        }    
    }     

    private void OnApplicationPause(bool pause)
    {
        
    }
    private void OnApplicationFocus(bool focus)
    {
        focusbool = focus;
        if (focusbool)
        {
            UpdateArguments();
        }  
    }  


 */



