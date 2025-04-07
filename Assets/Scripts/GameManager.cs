﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using TMPro;

using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Character")]
 
    public GameObject mainCharacter;
    public GameObject m_ChHead;
    [Header("Character Animator")]
    public Animator m_CharacterAnimator;

    RuntimeAnimatorController m_AnimControlller;

   

    [Header("Camera's")]
    public Camera m_MainCamera;
//    public Camera m_UICamera;
    public Camera m_RenderTextureCamera;
 //   public Camera m_ScreenShotCamera;

    [Header("Worlds Manager")]
    public List<Texture> worldsTexture;

    //[Header("Character Customizations")]
    //public CharacterCustomizationUIManager characterCustomizationUIManager;

    

    [Header("Objects During Flow")]
   //  public GameObject UIManager;  
    public GameObject BGPlane;
    public bool WorldBool;
    public bool OnceGuestBool;
    public bool OnceLoginBool;

    [Header("Camera Work")]
    public GameObject faceMorphCam;
    public GameObject headCam;
    public GameObject bodyCam;

    public Renderer EyeballTexture1;
    public Renderer EyeballTexture2;
    public GameObject ShadowPlane;
    public SavaCharacterProperties SaveCharacterProperties;

    public EquipUI EquipUiObj;
    public BlendShapeImporter BlendShapeObj;
    public bool UserStatus_;   //if its true user is logged in else its as a guest
    public static string currentLanguage = "";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        PlayerPrefs.SetInt("presetPanel", 0);  // was loggedin as account 

/*#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled=false;
#endif*/
    }
    public string GetStringFolderPath()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)  // loged from account)
        {

            if (PlayerPrefs.GetInt("presetPanel") == 1)  // presetpanel enabled account)
            {
                return (Application.persistentDataPath + "/SavingReoPreset.json");
            }
            else
            {
                UserStatus_ = true;
                return (Application.persistentDataPath + "/SavingCharacterDataClass.json");
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("presetPanel") == 1)  // presetpanel enabled account)
            {
                return (Application.persistentDataPath + "/SavingReoPreset.json");
            }
            else
            {
                UserStatus_ = false;
                return (Application.persistentDataPath + "/loginAsGuestClass.json");
            }
        }
    }
    public void ComeFromWorld()
    {
       StartCoroutine( WaitForInstancefromWorld());
       
    }
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        m_AnimControlller = mainCharacter.GetComponent<Animator>().runtimeAnimatorController;
        OnceGuestBool = false;
        OnceLoginBool = false;
        worldsTexture = new List<Texture>();
       // StartCoroutine(WaitForInstance());
        //ComeFromWorld();
       
    }
    //IEnumerator WaitForInstance()
    //{
    //    yield return new WaitForSeconds(.05f);
    //    SaveCharacterProperties = ItemDatabase.instance.GetComponent<SavaCharacterProperties>(); 
    //}
    IEnumerator WaitForInstancefromWorld()
    {
        yield return new WaitForSeconds(.05f);
        SaveCharacterProperties = ItemDatabase.instance.GetComponent<SavaCharacterProperties>();
         if (ItemDatabase.instance != null)
        ItemDatabase.instance.DownloadFromOtherWorld();
        
    }


    public void NotNowOfSignManager()
    {
      UIManager.Instance.LoginRegisterScreen.GetComponent<OnEnableDisable>().ClosePopUp();
        UIManager.Instance.IsWorldClicked();
        if (UIManager.Instance.HomePage.activeInHierarchy )
            UIManager.Instance.HomePage.SetActive(false);

        BGPlane.SetActive(true);
        if(!WorldBool)
        StoreManager.instance.SignUpAndLoginPanel(2);
    }
    public void AvatarMenuBtnPressed()
    {
       UIManager.Instance.AvaterButtonCustomPushed();
        CharacterCustomizationUIManager.Instance.LoadMyClothCustomizationPanel();
 
        if (UserRegisterationManager.instance.LoggedIn||  (PlayerPrefs.GetInt("IsLoggedIn") ==  1)) 
        {
            UIManager.Instance.HomePage.SetActive(false);
            StoreManager.instance.SignUpAndLoginPanel(3);
            BGPlane.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("IsChanged", 0);
            UserRegisterationManager.instance.OpenUIPanal(1);
        }
     }

    public void DestroyAllLoadedTexture()
    {

        for (int i = 0; i < worldsTexture.Count; i++)
        {
            Destroy(worldsTexture[i]);
        }

        worldsTexture.Clear();
    }

    public void SignInSignUpCompleted()
    {
        if (WorldBool)
        {
            UIManager.Instance.HomePage.SetActive(true);
            BGPlane.SetActive(false);
        }
        else
        {
            UIManager.Instance.HomePage.SetActive(false);
            BGPlane.SetActive(true);
            StoreManager.instance.SignUpAndLoginPanel(3);

        }
 
    }
    public void BackFromStoreofCharacterCustom()
    {
        UIManager.Instance.HomePage.SetActive(true);
     
        BGPlane.SetActive(false);
    }

    public void ChangeCharacterAnimationState(bool l_State)
    {    
        m_CharacterAnimator.SetBool("Idle", l_State);
    }

    public void ResetCharacterAnimationController()
    {
        m_CharacterAnimator.runtimeAnimatorController = m_AnimControlller;
        mainCharacter.GetComponent<Animator>().runtimeAnimatorController = m_AnimControlller;
    }

    //public bool onceforreading=false;
    //string jsonlocalization = "";
    //RecordsLanguage[] avc;
    //public string LocalizeTextText( string LocalizeText)
    //{
    //    if (!onceforreading)
    //    {
    //        if (File.Exists(Application.persistentDataPath + "/Localization.dat"))
    //        {
    //            StreamReader reader = new StreamReader(Application.persistentDataPath + "/Localization.dat");
    //            jsonlocalization = reader.ReadToEnd();
    //            reader.Close();
    //            avc = CSVSerializer.Deserialize<RecordsLanguage>(jsonlocalization);

    //            onceforreading = true;
    //        }
    //    }

    //    if (avc != null )//avc.Length > 0)
    //    {
    //        foreach (RecordsLanguage rl in avc)
    //        {
    //            if (rl.Keys == LocalizeText.ToString())
    //            {
    //                if (Application.systemLanguage == SystemLanguage.Japanese && !string.IsNullOrEmpty(rl.Japanese))
    //                    return LocalizeText = rl.Japanese;
    //                else if (Application.systemLanguage == SystemLanguage.English && !string.IsNullOrEmpty(rl.English))
    //                    return LocalizeText = rl.English;
    //            }
    //        }
    //    }
    //    return LocalizeText;
    //}
}
