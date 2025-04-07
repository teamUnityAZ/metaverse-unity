using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class YoutubeStreamController : MonoBehaviour
{
    [SerializeField]
    private GameObject LiveStreamPlayer;
    [SerializeField]
    private GameObject NormalPlayer;
    [SerializeField]
    private YoutubeAPIHandler APIHandler;

    private string PrevURL;
    private bool IsOldURL = true;
    Material bigScreenMat;
    Material verticalScreenMat;
    // Start is called before the first frame update
    private void Start()
    {
        bigScreenMat = GameObject.Find("big screen").GetComponent<Renderer>().sharedMaterial;
        changeMaterial(bigScreenMat, false);
    }
    private void OnEnable()
    {
        PrevURL = null;
        StartCoroutine(SetStreamContinous());
    }
    public void Update()
    {
        if (XanaConstants.xanaConstants._connectionLost)
        {
            if (this.gameObject != null)
            {
                Destroy(this.gameObject);
            }
        }
     }
   
    public IEnumerator SetStreamContinous()
    {
        
        if (bigScreenMat != null)
        {
            
        }
        while (true)
        {
            StartCoroutine(SetStream());
            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SetStream()
    {
        StartCoroutine(APIHandler.GetStream());

        while (APIHandler.Data == null )
        {
            yield return null;
        }
        if (PrevURL.IsNullOrEmpty() || !PrevURL.Equals(APIHandler.Data.URL) ||! APIHandler.Data.isPlaying)
        {
            Debug.Log("New Link Found");
            PrevURL = APIHandler.Data.URL;
            SetUpStream();
        }
    }

    private void SetUpStream()
    {
        changeMaterial(bigScreenMat, true);

        if (APIHandler.Data.IsLive && APIHandler.Data.isPlaying)
        {
            LiveStreamPlayer.SetActive(true);
            NormalPlayer.SetActive(false);

            YoutubePlayerLivestream player = LiveStreamPlayer.GetComponent<YoutubePlayerLivestream>();
            if (player)
            {
                Debug.LogError("YoutubeStreamController SetUpStream live stream.......:" + APIHandler.Data.URL);
                player.GetLivestreamUrl(APIHandler.Data.URL);
            }
        }
        else
        {
            Debug.LogError("YoutubeStreamController SetUpStream Normal.......:" + APIHandler.Data.URL + "   :isPlaying:" + APIHandler.Data.isPlaying);
            LiveStreamPlayer.SetActive(false);
            NormalPlayer.SetActive(true);

            YoutubeSimplified player = NormalPlayer.GetComponent<YoutubeSimplified>();

            if (player && APIHandler.Data.isPlaying)
            {
                player.url = APIHandler.Data.URL;
                player.Play();
            }

        }
    }

    void changeMaterial(Material mat, bool isVideoLoad) {
        if (isVideoLoad)
        {
            mat.color = Color.white;
            //print("turn Material to white");
        }
        else
        {
            
            mat.color = Color.black;
            //print("turn Material to black");

        }
    }
}
