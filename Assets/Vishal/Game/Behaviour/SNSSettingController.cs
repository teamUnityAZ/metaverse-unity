﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SNSSettingController : MonoBehaviour
{
    public static SNSSettingController Instance;

    [Header("Setting Screen Reference")]
    public GameObject settingScreen;
    public TextMeshProUGUI versionText;

    //private string privacyPolicyLink = "https://cdn.xana.net/xanaprod/privacy-policy/PRIVACYPOLICY-2.pdf";
    //private string termsAndConditionLink = "https://cdn.xana.net/xanaprod/privacy-policy/termsofuse.pdf";

    [Space]
    [Header("My Account Screen Reference")]
    public GameObject myAccountScreen;

    [Space]
    [Header("My Account Personal Information References")]
    public GameObject myAccountPersonalInfoScreen;
    [SerializeField] private GameObject personalInfoEmailObj;
    [SerializeField] private GameObject personalInfoPhoneNumberObj;
    [SerializeField] private TextMeshProUGUI personalInfoEmailText;
    [SerializeField] private TextMeshProUGUI personalInfoPhoneNumberText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    #region Setting Screen.......
    //this method is used to Open Setting Screen.......
    public void OnClickSettingOpen()
    {
        settingScreen.SetActive(true);
        SettingScreenSetup();
    }

    //this method is used to Setup Setting Screen.......
    public void SettingScreenSetup()
    {
        versionText.text = Application.version;
    }

    //this method is used to Close Setting Screen......
    public void OnClickSettingClose()
    {
        settingScreen.SetActive(false);
    }

    //this method is used to My Account Button click.......
    public void OnClickMyAccountButton()
    {
        MyProfileDataManager.Instance.CreateFirstFeedPlusAnimStop(true);

        OnClickSettingClose();
        myAccountScreen.SetActive(true);
    }

    //this method is used to My Account Screen Back Button Click.......
    public void OnClickMyAccountBackButton()
    {
        MyProfileDataManager.Instance.CreateFirstFeedPlusAnimStop(false);//check profile post empty or not and start bottom create plus icon anim
    }

    //this method is used to terms and policy.......
    public void OpenPrivacyPolicyHyperLink()
    {
        if (XanaConstants.xanaConstants != null)
        {
            Application.OpenURL(ConstantsGod.r_privacyPolicyLink);
        }
    }

    //this method is used to Tearms and condition button click.......
    public void OpenTermsAndConditionHyperLink()
    {
        if (XanaConstants.xanaConstants != null)
        {
            Application.OpenURL(ConstantsGod.r_termsAndConditionLink);
        }
    }
    #endregion

    #region My Account Screen.......
    //this method is used to Personal Information Button Click.......
    public void OnClickPersonalInformationButton()
    {
        Debug.LogError("Personal information button click");
        
        if (MyProfileDataManager.Instance.myProfileData.id == 0)
        {
            FeedUIController.Instance.ShowLoader(true);
            APIManager.Instance.RequestGetUserDetails("MyAccount");//Get My Profile data    
        }
        else
        {
            SetUpDataOfPersonalInfo(MyProfileDataManager.Instance.myProfileData.email, MyProfileDataManager.Instance.myProfileData.phoneNumber);
            myAccountPersonalInfoScreen.SetActive(true);
        }
    }

    void SetUpDataOfPersonalInfo(string email, string phoneNumber) 
    {
        personalInfoPhoneNumberText.text = "";
        personalInfoEmailText.text = "";

        if (!string.IsNullOrEmpty(email))
        {
            personalInfoEmailText.text = email;
            personalInfoEmailObj.SetActive(true);
        }
        else
        {
            personalInfoEmailObj.SetActive(false);
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            personalInfoPhoneNumberText.text = phoneNumber;
            personalInfoPhoneNumberObj.SetActive(true);
        }
        else
        {
            personalInfoPhoneNumberObj.SetActive(false);
        }
    }

    //this method is used to setup data of personal information screen.......
    public void SetUpPersonalInformationScreen()
    {
        FeedUIController.Instance.ShowLoader(false);
        SetUpDataOfPersonalInfo(MyProfileDataManager.Instance.myProfileData.email, MyProfileDataManager.Instance.myProfileData.phoneNumber);
        myAccountPersonalInfoScreen.SetActive(true);        
    }

    //this method is used to logout button click.......
    public void OnClickLogoutButton()
    {
        Debug.Log("logout Button click");
        if (UserRegisterationManager.instance != null)
        {
            UserRegisterationManager.instance.SubmitLogoutAccount();
        }
    }

    //this method is used to logout success.......
    public void LogoutSuccess()
    {
        Debug.Log("logout success calling from SNSSetting");
        if (FeedUIController.Instance != null)
        {
            MyProfileDataManager.Instance.ClearAndResetAfterLogout();

            if (UserRegisterationManager.instance != null)
            {
                UserRegisterationManager.instance.ResetDataAfterLogoutSuccess();
                UserRegisterationManager.instance.nftlist = string.Empty;
            }
            NftDataScript.Instance.ResetNftData();
            if (File.Exists(Application.persistentDataPath + "/NftData.txt"))
            {
                FileInfo file_info = new FileInfo(Application.persistentDataPath + "/NftData.txt");
                file_info.Delete();
            }
            
            myAccountScreen.SetActive(false);
            FeedUIController.Instance.ResetAllFeedScreen(false);
            FeedUIController.Instance.ClearAllFeedDataAfterLogOut();
            FeedUIController.Instance.footerCan.GetComponent<BottomTabManager>().OnClickHomeButton();
            FeedUIController.Instance.footerCan.GetComponent<BottomTabManager>().CheckLoginOrNotForFooterButton();
            PremiumUsersDetails.Instance.combinedUserFeatures.Clear();
            ConstantsGod.UserPriorityRole = "free";
            CommonAPIManager.Instance.SetUpBottomUnReadCount(0);
        }
    }
    #endregion
}