using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Android;

public class GetJoin : MonoBehaviour
{
    public static GetJoin instance;

    public GetJoinResponse response;

    private string canJoin;
    public GameObject Message;
    public GameObject worldPlay;
  //  public Button museumBtn;


    private void OnEnable()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
       
    }





    private void Start()
    {       
     //   museumBtn.enabled = false;
        StartCoroutine(CallGetJoin());
    }  
         
    public IEnumerator CallGetJoin()
    {
       
        WWWForm form = new WWWForm();

        form.AddField("token", "piyush55");

        using (UnityWebRequest request = UnityWebRequest.Post(ConstantsGod.API+"xanaServer/getJoin", form))
        {
            request.timeout = 10;
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }

            if (request.isNetworkError || request.isHttpError)
            {
                ShowErrorMsg();
            }           
            else
            {
               response = Gods.DeserializeJSON<GetJoinResponse>(request.downloadHandler.text.Trim());
                if (request.isDone)
                {
                   // museumBtn.enabled = true;
                    canJoin = response.data[0].canJoin;

                    request.Dispose();
                }
            }
        }

    }

    public void ShowMsg()
    {
        if (canJoin == "false")
        {
            worldPlay.SetActive(false);
            Message.SetActive(true);
        }
    }


    public void ShowErrorMsg()
    {
        Message.GetComponentInChildren<Text>().text = "エラーが発生しました、暫くしてからお試しください。";
        Message.SetActive(true);
        
    }


}


public partial class GetJoinResponse
{    
    public bool success { get; set; }
   
    public string msg { get; set; }
   
    public List<DatumGetJoin> data { get; set; }
}

public partial class DatumGetJoin
{   
    public string _id { get; set; }
   
    public string canJoin { get; set; }
}