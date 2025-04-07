using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    /*
         
    IEnumerator HitSetDeviceANDLogOutAPI(string url, string Jsondata)
    {
        print("Body " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");  
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("TokenAfterRegister"));
        yield return request.SendWebRequest();
        print(request.GetRequestHeader("Authorization"));
        print(request.isDone);
        Debug.Log(request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                if (myObject1.success == "true")
                {
                    print("Set Device Token succesfully");
                }
            }
        }
        else   
        {
            if (request.isNetworkError)
            {
                errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                errorTextPassword.GetComponent<Text>().text = request.error.ToUpper();
                StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    if (myObject1.success == "false")
                    {
                        print("Hey success false " + myObject1.msg);
                        errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                        errorTextPassword.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                    }
                }
            }
        }
    }



        */






    }
}
