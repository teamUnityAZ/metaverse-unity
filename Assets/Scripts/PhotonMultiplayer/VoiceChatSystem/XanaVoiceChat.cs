using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class XanaVoiceChat : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject micOnBtn;
    public GameObject micOffBtn;
    public Sprite micOnSprite;
    public Sprite micOffSprite;

    private VoiceConnection voiceConnection;
    private Recorder recorder;
    private Speaker speaker;

    private Button micBtn;

    private bool canTalk;
    private bool useMic;

    public UnityAction MicToggleOff, MicToggleOn;
    public static XanaVoiceChat instance;

    [Header("Mic Toast to instatiate")]
    public GameObject mictoast;
    public Transform placetoload;



    public void Awake()
    {
        if (instance)
        {
            instance.Start();
            DestroyImmediate(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        recorder = GameObject.FindObjectOfType<Recorder>();

        voiceConnection = GetComponent<VoiceConnection>();

        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        MicToggleOff = TurnOnMic;
        MicToggleOn = TurnOffMic;

        //micOffBtn = GameObject.Find("MicOffToggle");
        //micOnBtn = GameObject.Find("MicToggle");
        micOffBtn.GetComponent<Button>().onClick.AddListener(MicToggleOff);
        micOnBtn.GetComponent<Button>().onClick.AddListener(MicToggleOn);
        if (XanaConstants.xanaConstants.EnviornmentName == "DJ Event")
        {
            micOffBtn.SetActive(false);
            micOnBtn.SetActive(false);
            XanaConstants.xanaConstants.mic = 0;
        }
        StartCoroutine(CheckVoiceConnect());
    }

    public void TurnOnMic()
    {

        if (XanaConstants.xanaConstants.mic == 0)
        {
            GameObject go = Instantiate(mictoast, placetoload);
            Destroy(go, 1.5f);
            return;
        }

        print("Turn on mic");
        micOffBtn.SetActive(false);
        micOnBtn.SetActive(true);
        recorder.TransmitEnabled = true;


    }

    public void TurnOffMic()
    {
        print("Turn off mic");
        micOffBtn.SetActive(true);
        micOnBtn.SetActive(false);
        recorder.TransmitEnabled = false;
    }

    private void OnEnable()
    {
        //if (XanaConstants.xanaConstants.mic == 1)
        //{
        //    TurnOnMic();
        //}
        //else
        //{
        //    TurnOffMic();
        //}
        //voiceConnection.SpeakerLinked += OnSpeakerCreated;
        //voiceConnection.Client.AddCallbackTarget(this);
    }

    public void UpdateMicButton()
    {
        StartCoroutine(CheckVoiceConnect());
    }

    private void OnDisable()
    {
        //voiceConnection.SpeakerLinked -= OnSpeakerCreated;
        //voiceConnection.Client.RemoveCallbackTarget(this);
    }

    protected virtual void OnSpeakerCreated(Speaker _speaker)
    {
        Debug.Log("Speaker is Created");
        speaker = _speaker;
    }

    IEnumerator CheckVoiceConnect()
    {
        while (!PhotonVoiceNetwork.Instance.Client.IsConnected)
        {
            //Debug.Log("Still Connecting");
            yield return null;
        }
        recorder.TransmitEnabled = true;
        recorder.DebugEchoMode = false;
        if (XanaConstants.xanaConstants.mic == 1)
        {
            TurnOnMic();
        }
        else
        {
            TurnOffMic();
        }
        //GetLocalSpeaker();
        //ToggleVoiceChat(false);
    }

    void GetLocalSpeaker()
    {
        Debug.Log("Checking if local speaker ready");
        StartCoroutine(WaitAndSearchForLocalSpeaker());
    }

    IEnumerator WaitAndSearchForLocalSpeaker()
    {
        Speaker[] _speaker = FindObjectsOfType<Speaker>(); ;

        while (speaker == null)
        {
            //  Debug.Log("Get Local Speaker: " + _speaker.Length);

            yield return null;

            //if (voiceConnection.speak.Count != 0)
            //    speaker = voiceConnection.linkedSpeakers[0];
        }

        ToggleVoiceChat(false);
    }

    public void ToggleMic(bool _useMic)
    {
        //if (_useMic && !canTalk)
        //{
        //    ShowVoiceChatDialogBox();
        //    return;
        //}

        //useMic = _useMic;

        //if (useMic)
        //{


        //    micOnBtn.SetActive(true);
        //    micOffBtn.SetActive(false);

        //    recorder.TransmitEnabled = true;
        //}
        //else
        //{
        //    micOnBtn.SetActive(false);
        //    micOffBtn.SetActive(true);

        //    recorder.TransmitEnabled = false;
        //}
    }

    void ShowVoiceChatDialogBox()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, "Please turn on \"Voice\" from the settings", 0);
                toastObject.Call("show");
            }));
        }
#endif
    }

    public void ToggleVoiceChat(bool _canTalk)
    {
        canTalk = _canTalk;

        if (!canTalk)
        {
            Debug.Log("Speaker: " + speaker);

            if (speaker != null)
            {
                speaker.GetComponent<AudioSource>().mute = true;

            }
            if (recorder != null)
            {
                ToggleMic(false);
            }
        }
        else
        {
            if (speaker != null)
            {
                speaker.GetComponent<AudioSource>().mute = false;
            }
            if (recorder != null)
            {
                //ToggleMic(true);
            }
        }
    }


}
