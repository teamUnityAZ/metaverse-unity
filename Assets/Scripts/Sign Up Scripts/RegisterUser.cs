using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RegisterUser : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(HitRegisterUserAPI());
    }

    public IEnumerator HitRegisterUserAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", "adil123452211");
        form.AddField("password", "123456");  
        form.AddField("token", "piyush55");
        form.AddField("username", "adi1234522");
        using (UnityWebRequest www = UnityWebRequest.Post("https://api.xana.net/xanaWeb/register", form))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                Debug.Log(www.downloadProgress);
                yield return null;
            }
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.LogError("Network Error");
            }
            else
            {
                if (operation.isDone)
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }
    }



}
