using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class GetAnimationTimeGrammy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAnimationTime());
    }

   IEnumerator GetAnimationTime()
    {
        Debug.LogError(ConstantsGod.API_BASEURL + ConstantsGod.getAnimationTime);
        using (UnityWebRequest www=UnityWebRequest.Get(ConstantsGod.API_BASEURL+ConstantsGod.getAnimationTime))
        {
            www.SetRequestHeader("Authorization" , PlayerPrefs.GetString("GuestToken"));
            www.SendWebRequest();
            while (!www.isDone)
            {
                yield return null;
            }
            Debug.LogError(www.downloadHandler.text);
            string apiResponse = www.downloadHandler.text;

            TimeDataClass timeDataClass = new TimeDataClass();
            timeDataClass = JsonUtility.FromJson<TimeDataClass>(apiResponse);


            

        }
    }

   [System.Serializable]
   public class TimeDataClass
    {
        public string success;
        public PlayTimeClass data;
        public string msg;

    }
    [System.Serializable]
    public class PlayTimeClass
    {
        public string playTime;
    }
}
