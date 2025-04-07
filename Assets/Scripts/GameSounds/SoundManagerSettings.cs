using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Voice.Unity;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class SoundManagerSettings : MonoBehaviour
{
    public static SoundManagerSettings soundManagerSettings;

    [Header("Audio Sources")]

    public AudioSource bgmSource;
    public AudioSource effectsSource;
    public AudioSource videoSource;

    [Header("Audio Slider")]
    public Slider totalVolumeSlider;
   // public Slider mySlider;
    public Slider bgmSlider;
    public Slider videoSlider;
    public Slider cameraSensitivitySlider;

    [Space]
    public Button MuteBtnMain;
    public Button unMuteBtnMain; 
    
    [Header("Speakers")]
    private Speaker speaker;
    private Recorder recorder;

    void Awake()
    {
        if (soundManagerSettings == null)
        {
            soundManagerSettings = this;
        }

        if (SoundManager.Instance)
        {
            bgmSource = SoundManager.Instance.MusicSource;
            effectsSource = SoundManager.Instance.EffectsSource;
            videoSource = SoundManager.Instance.videoPlayerSource;
        }
        
    }

    private void Start()
    {      
        totalVolumeSlider.value = PlayerPrefs.GetFloat(ConstantsGod.TOTAL_AUDIO_VOLUME, 0.5f);        
        bgmSlider.value = PlayerPrefs.GetFloat(ConstantsGod.BGM_VOLUME, 0.5f);
        videoSlider.value = PlayerPrefs.GetFloat(ConstantsGod.VIDEO_VOLUME, 0.5f);
        cameraSensitivitySlider.value = PlayerPrefs.GetFloat(ConstantsGod.CAMERA_SENSITIVITY, 0.2f);                       
        SetAllVolumes(PlayerPrefs.GetFloat(ConstantsGod.TOTAL_AUDIO_VOLUME, 0.5f));        
        SetBgmVolume(PlayerPrefs.GetFloat(ConstantsGod.BGM_VOLUME, 0.5f));
        SetVideoVolume(PlayerPrefs.GetFloat(ConstantsGod.VIDEO_VOLUME, 0.5f));
        SetCameraSensitivity(PlayerPrefs.GetFloat(ConstantsGod.CAMERA_SENSITIVITY, 0.2f));
        SetMicVolume(PlayerPrefs.GetFloat(ConstantsGod.MIC, 0.5f));


        totalVolumeSlider.onValueChanged.AddListener((float vol) =>
        {
            SetAllVolumes(vol);
        });

        videoSlider.onValueChanged.AddListener((float vol) =>
        {
            SetVideoVolume(vol);
        });

        bgmSlider.onValueChanged.AddListener((float vol) =>
        {
            SetBgmVolume(vol);
        });

        cameraSensitivitySlider.onValueChanged.AddListener((float sensitivity) =>
        {
            SetCameraSensitivity(sensitivity);
        });

        if (!PlayerPrefs.HasKey("musicvolume"))
        {

            PlayerPrefs.SetFloat("musicvolume", 0.5f);
            Load();
        }
        else {
            Load();
        }


    }

    public void SetAllVolumes(float volume)
    {
        PlayerPrefs.SetFloat(ConstantsGod.TOTAL_AUDIO_VOLUME, volume);
        SetMicVolume(totalVolumeSlider.value);
        SetEffectsVolume(totalVolumeSlider.value);
        SetBgmVolume(totalVolumeSlider.value);
        SetVideoVolume(totalVolumeSlider.value);       
    }

    public void SetBgmVolume(float Vol)
    {
        PlayerPrefs.SetFloat(ConstantsGod.BGM_VOLUME, Vol);
        if (bgmSource)
        {
            if (totalVolumeSlider.value >= Vol)
            {
                bgmSource.volume = Vol;
            }
            else
            {
                bgmSource.volume = totalVolumeSlider.value;
            }
        }
        
    }

    public void SetVideoVolume(float Vol)
    {
        PlayerPrefs.SetFloat(ConstantsGod.VIDEO_VOLUME, Vol);

        if (videoSource)
        {
            if (totalVolumeSlider.value >= Vol)
            {
                videoSource.volume = Vol;
            }
            else
            {
                videoSource.volume = totalVolumeSlider.value;
            }
        }
        
    }

    public void SetCameraSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(ConstantsGod.CAMERA_SENSITIVITY, sensitivity);

        if (cameraSensitivitySlider.value >= sensitivity)
        {
            CameraLook.instance.lookSpeed = sensitivity;
        }
        else
        {
            CameraLook.instance.lookSpeed = cameraSensitivitySlider.value;
        }
           
    }

    public void SetEffectsVolume(float Vol)
    {
        PlayerPrefs.SetFloat(ConstantsGod.TOTAL_AUDIO_VOLUME, Vol);

        if (effectsSource)
        {
            if (totalVolumeSlider.value >= Vol)
            {
                effectsSource.volume = Vol;
            }
            else
            {
                effectsSource.volume = totalVolumeSlider.value;
            }
        }
      
    }

    public void SetMicVolume(float vol)
    {
        PlayerPrefs.SetFloat(ConstantsGod.TOTAL_AUDIO_VOLUME, vol);

        
        if(totalVolumeSlider.value >= vol)
        {
            foreach (var gameobject in Launcher.instance.playerobjects)
            {
                if (!gameobject.GetComponent<PhotonView>().IsMine)
                {
                    gameobject.GetComponent<AudioSource>().volume = vol;
                }
            }
        }
        else
        {
                foreach (var gameobject in Launcher.instance.playerobjects)
                {
                    if (!gameobject.GetComponent<PhotonView>().IsMine)
                    gameobject.GetComponent<AudioSource>().volume = totalVolumeSlider.value;
                }
            
            
        }
       
    }
    public void mychangeVolume()
    {
        AudioListener.volume = totalVolumeSlider.value;
        save();
    }

    private void save() {
        PlayerPrefs.SetFloat("musicvolume",totalVolumeSlider.value);
    }
    private void Load() {
        totalVolumeSlider.value = PlayerPrefs.GetFloat("musicvolume");
    
    }


}
