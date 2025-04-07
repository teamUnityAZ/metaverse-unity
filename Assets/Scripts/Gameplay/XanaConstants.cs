using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class XanaConstants : MonoBehaviour
{
    public static XanaConstants xanaConstants;
    public int mic;
    public string CurrentSceneName;
    public string EnviornmentName;
    public AssetBundle museumAssetLoaded;
    public string museumDownloadLink;// = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Aurora_Art_Museum/auroramuseum.android";
    public GameObject buttonClicked;
    public GameObject _lastClickedBtn;
    public GameObject _secondLastClickedBtn;
    public GameObject _curretClickedBtn;
    public bool isItemChanged = false;
    public bool IsMuseum = false;
    public string hair = "";
    public int faceIndex = 0;
    public bool isFaceMorphed = false;
    public int eyeBrowIndex = 0;
    public bool isEyebrowMorphed = false;
    public int eyeIndex = 0;
    public string eyeColor = "";
    public bool isEyeMorphed = false;
    public int noseIndex = 0;
    public bool isNoseMorphed = false;
    public int lipIndex = 0;
    public string lipColor = "";
    public bool isLipMorphed = false;
    public int bodyNumber = -1;
    public string skinColor = "";
    public string shirt = "";
    public string shoes = "";
    public string pants = "";
    public int currentButtonIndex;
    public bool _connectionLost=false;
    public GameObject ConnectionPopUpPanel;
    public GameObject[] avatarStoreSelection;
    public GameObject[] wearableStoreSelection;
    public GameObject[] colorSelection;
    public bool setIdolVillaPosition = true;
    public bool orientationchanged = false;
    public bool SelfiMovement = true;
    //for Create Room Scene Avatar
    [Header("SNS Variables")]
    public bool r_isSNSComingSoonActive = true;
    public GameObject r_MainSceneAvatar;

    
    //Emote Animation.......
    public string r_EmoteStoragePersistentPath
    {
        get
        {
            return Application.persistentDataPath + "/EmoteAnimationBundle";
        }
    }
    public string r_EmoteReactionPersistentPath
    {
        get
        {
            return Application.persistentDataPath + "/EmoteReaction";
        }
    }

    public void Awake()
    {
        if (xanaConstants)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            xanaConstants = this;
            if (PlayerPrefs.HasKey("micSound"))
            {
                mic = PlayerPrefs.GetInt("micSound");
            }
            else
            {
                PlayerPrefs.SetInt("micSound", 1);
                mic = PlayerPrefs.GetInt("micSound");
            }
            DontDestroyOnLoad(this.gameObject);
        }

        avatarStoreSelection = new GameObject[8];
        wearableStoreSelection = new GameObject[8];
        colorSelection = new GameObject[2];

        if (!Directory.Exists(r_EmoteStoragePersistentPath))
        {
            Directory.CreateDirectory(r_EmoteStoragePersistentPath);
        }
    }

    public void StopMic()
    {
        PlayerPrefs.SetInt("micSound", 0);
        mic = PlayerPrefs.GetInt("micSound");
    }

    public void PlayMic()
    {
        PlayerPrefs.SetInt("micSound", 1);
        mic = PlayerPrefs.GetInt("micSound");
    }
}