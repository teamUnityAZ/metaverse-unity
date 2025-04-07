using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class DownloadAndPlayVideo : MonoBehaviour
{
    public string video_download_link;
    public VideoPlayer player;
    private void Start()
    {
#if UNITY_ANDROID
        StartCoroutine(VideoDownload());
#endif

    }
    IEnumerator VideoDownload()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(video_download_link))
        {
            yield return request.SendWebRequest();
            yield return new WaitForEndOfFrame();

            if (!request.isHttpError || !request.isNetworkError)
            {
                if (request.error == null)
                {
                    player.url = video_download_link;
                    player.Play();
                }
            }
        }
    }
}
