using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManagerWallet : MonoBehaviour
{
    public GameObject Page1;
    public GameObject Page2;  
    string LoginAPIURL = ConstantsGod.API_BASEURL + ConstantsGod.LoginAPIURL;


    public InputField UserNameField;
    public InputField PasswordField;

    // Start is called before the first frame update
    void Start()
    {
        Page1.SetActive(true);
        Page2.SetActive(false);

    }
    [Serializable]
    public class MyClassOfLoginJson
    {
        public string email;
        public string phoneNumber;
        public string password;
        public MyClassOfLoginJson GetdataFromClass(string L_eml, string L_phonenbr, string passwrd)
        {
            MyClassOfLoginJson myObj = new MyClassOfLoginJson();
            myObj.email = L_eml;
            myObj.phoneNumber = L_phonenbr;
            myObj.password = passwrd;
            return myObj;
        }
    }
  

    public void SubmitLogin()
    {
        print("Submit Login");
       string  L_LoginEmail = UserNameField.text.Trim();
       string   L_loginPassword = PasswordField.text.Trim();

         MyClassOfLoginJson myObject = new MyClassOfLoginJson();
         string bodyJson = JsonUtility.ToJson(myObject.GetdataFromClass(L_LoginEmail, "", L_loginPassword));
        StartCoroutine(LoginUserWithNewT(LoginAPIURL, bodyJson));
     }
    [System.Obsolete]
    IEnumerator LoginUserWithNewT(string url, string Jsondata)
    {  
        print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        print("json data is " + request.downloadHandler.text);
        ClassWithToken myObject1 = new ClassWithToken();
        myObject1 = ClassWithToken.CreateFromJSON(request.downloadHandler.text);
        print(myObject1.msg + " | success: " + myObject1.success);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                //    Debug.Log(request.downloadHandler.text);
                //if (myObject.success == "true")
                if (myObject1.success)
                {   
                    print("Token before " + myObject1.data.token);
                    PlayerPrefs.SetString("LoginToken", myObject1.data.token);
                    Page1.SetActive(false);
                    Page2.SetActive(true);
                    print("App ID is " + myObject1.data.user.id);
                    PlayerPrefs.SetInt("AppID", myObject1.data.user.id);
                    SceneManager.LoadScene(1);   
                 }        
            }      
        }  
    }  
    [System.Serializable]
    public class ClassWithToken
    {
        public bool success;
        public JustToken data;
        public string msg;
        public ClassWithToken()
        {
        }
        public static ClassWithToken CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassWithToken>(jsonString);
        }
    }
    [System.Serializable]
    public class JustToken
    {
        public string token;
        public string encryptedId;
        public UserData user;

        public static JustToken CreateFromJSON(string jsonString)
        {
            print("Person " + jsonString);
            return JsonUtility.FromJson<JustToken>(jsonString);
        }
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public string email;
        public string phoneNumber;
        public string coins;
    }




    public void ConnectWithWallet()
    {
        print("Connect with wallet");

    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
