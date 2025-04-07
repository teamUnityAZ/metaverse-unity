﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using UnityEngine.Networking;
using System;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using Mopsicus.Plugins;
using Sign_Up_Scripts;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;
using AdvancedInputFieldPlugin;
using System.Linq;

public class UserRegisterationManager : MonoBehaviour
{

    public static UserRegisterationManager instance;

    private string XanaliaAPI = "https://api.xanalia.com/user/my-collection";
    [Header("Total-Panal")]
    public GameObject FirstPanal;
    //public GameObject EmailPanal;
    public GameObject OTPPanal;
    public GameObject PasswordPanal;
    public GameObject usernamePanal;
    public GameObject LoginPanal;
    public GameObject SignUpPanal;
    public GameObject SignUpPanalwithPhone;
    public GameObject ChangePasswordPanal;
    public GameObject UpdateprofilePanal;
    public GameObject TestingAPIsPanal;
    public GameObject RegistrationCompletePanal;
    public GameObject ForgetenterUserNamePanal;
    public GameObject ForgetEnterPasswordPanal;
    public GameObject LogoutfromOtherDevicePanel;

    //hardik changes
    public string nftlist;
    //end
    public Color NormalColor;
    public Color HighlightedColor;
    [Header("SignUp-InputFields")]
    //  public MobileInputField EmailInputTextNew;
    public AdvancedInputField EmailFieldNew;
    //   public MobileInputField UsernameTextNew;
    public AdvancedInputField UsernameFieldAdvance;
    public Text CountryCodeText;
    // public MobileInputField PhoneInputTextNew;      
    public AdvancedInputField PhoneFieldNew;
    [Header("Password-InputFields")]
    public ShiftCode Password1InputTextShiftCode;
    public ShiftCode Password2ConfirmInputTextShiftCode;
    public AdvancedInputField Password1New;
    public AdvancedInputField Password2New;

    [Header("OTP-InputFields")]
    public List<MobileInputField> pinNew;
    public MobileInputField mainfield_for_opt;
    public AdvancedInputField mainfieldOTPNew;
    public Text[] text_to_show;
    [Space(5)]
    [Header("Login-InputFields")]
    public MobileInputField LoginEmailNew;
    public ShiftCode LoginPasswordShiftCode;
    public AdvancedInputField LoginEmailOrPhone;
    public AdvancedInputField LoginPassword;

    [Space(5)]
    [Header("ForgetPassword-InputFields")]
    // public MobileInputField EmailOrPhone_ForgetPasswrod;
    public AdvancedInputField EmailOrPhone_Forget_NewField;
    //  public MobileInputField Password1_ForgetPasswrod;
    // public MobileInputField Standard1_ForgetPasswrod;
    public AdvancedInputField Password1_ForgetNewField;
    public AdvancedInputField Password2_ForgetNewField;
    //  public MobileInputField Password2_ForgetPasswrod;
    //   public MobileInputField Standard2_ForgetPasswrod;
    public ShiftCode InputTextShiftCodeChangePass;
    public ShiftCode InputTextShiftCodeChangePass2;
    [Space(5)]
    [Header("Change Password-InputFields")]
    public MobileInputField OldPasswordField;
    public MobileInputField ChangePassword1;
    public MobileInputField ChangePassword2;
    [Space(5)]
    [Header("Update Profile-InputFields")]
    public MobileInputField GenderField;
    public MobileInputField JobField;
    public MobileInputField CountryField;
    public MobileInputField BioField;
    [Space(10)]
    [Header("Error Texts GameObjects")]
    public GameObject errorTextEmail;
    public GameObject errorTextPassword;
    public GameObject errorTextNumber;
    public GameObject errorTextName;
    public GameObject errorTextPIN;
    public GameObject errorTextLogin;
    public GameObject errorTextForgetAPI;
    public GameObject errorTextResetPasswordAPI;
    public ErrorHandler errorHandler;

    public List<string> myData;
    string Email;
    string ForgetPasswordEmlOrPhnContainer;
    private string LocalPhoneNumber;
    string password;
    string Username;
    private bool isCheckingOTP;
    string CurrentOTP;
    [HideInInspector]
    public bool LoggedIn;
    string ForgetPasswordTokenAfterVerifyling;
    [Header("Flow Bools")]
    [HideInInspector]
    public bool SignUpWithPhoneBool;
    bool SignUpWithEmailBool;
    bool BackBool;
    private bool ResendOTPBool;
    private bool OTPFieldBool;
    bool ForgetPasswordBool;
    private bool mobile_number;


    [Header("SignUp Tab Selector UI")]
    public GameObject phoneTab;
    public GameObject emailTab;
    public GameObject numberScreen;
    public GameObject WalletScreen;
    public Image WalletSelectedImg;
    public Image PhoneSelectedImg;
    public Image EmailSelectedImg;
    public GameObject emailScreen;
    public Text phoneTabText;
    public Text emailTabText;
    public Text WalletTabText;
    GameObject imageObject;
    GameObject imageObject2;
    [Tooltip("Pass Animator Component attached to the slider")]
    public Animator tabSelectorAnimator;

    public bool SavingBool;
    [HideInInspector]
    private Button currentSelectedNxtButton;
    public bool LoggedInAsGuest;
    public GameObject Name_Screen_Preset_Panel;
    public bool checkbool_preser_start;
    public string XanaliaUserTokenId="";



    #region WelcomeScreen
    public GameObject welcomeScreen;
    public bool shownWelcome;
    [Header("Premium user UI")]
    public GameObject PremiumUserUI;

    public void ShowWelcomeScreen()
    {
        if (!PlayerPrefs.HasKey("shownWelcome"))
        {
            if (PlayerPrefs.GetInt("IsProcessComplete") == 0)
            {
                welcomeScreen.SetActive(true);
                shownWelcome = true;
                //PlayerPrefs.SetInt("shownWelcome", 1);
            }
        }
    }

    public void ShowWelcomeClosed()
    {
        if (PlayerPrefs.GetInt("IsProcessComplete") == 1)
        {
            if (PlayerPrefs.GetInt("iSignup") == 1)
            {
                //  print("dklfjghjksdghdjklghdjklfghjdfhjk ");
                PlayerPrefs.SetInt("presetPanel", 1);
                ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
                StoreManager.instance.OnSaveBtnClicked();  // reg complete go home
            }
        }
        else
        {
            welcomeScreen.SetActive(false);
            shownWelcome = false;
            //print("park ---");
            if (!PlayerPrefs.HasKey("shownWelcome"))
            {
                //PlayerPrefs.SetInt("shownWelcome", 1);

                StoreManager.instance.StartPanel_PresetParentPanel.SetActive(true);
            }
        }

        //  usernamePanal.SetActive(true);
    }
    // if comming back from character screen 
    public void ShowWellComeCloseRetrack()
    {
        PlayerPrefs.DeleteKey("shownWelcome");
        //  PlayerPrefs.DeleteKey("iSignup");
        PlayerPrefs.SetInt("iSignup", 0);
        checkbool_preser_start = true;
        // welcomeScreen.SetActive(true);
        ShowWelcomeScreen();

    }

    public void iwanto_signUp()
    {
        PlayerPrefs.SetInt("iSignup", 1);
        StoreManager.instance.StartPanel_PresetParentPanel.SetActive(true);
    }
    public void CoutinueAsAGuest()
    {
        //    welcomeScreen.SetActive(false);
        //   shownWelcome = false;
        //   StoreManager.instance.StartPanel_PresetParentPanel.SetActive(true);

    }
    #endregion
    IEnumerator waitforOneframe()
    {
        yield return new WaitForEndOfFrame();
        //EmailPanal.SetActive(false);
        FirstPanal.SetActive(false);
        OTPPanal.SetActive(false);
        PasswordPanal.SetActive(false);
        usernamePanal.SetActive(false);
        LoginPanal.SetActive(false);
        SignUpPanal.SetActive(false);
        SignUpPanalwithPhone.SetActive(false);
        ForgetenterUserNamePanal.SetActive(false);
        ForgetEnterPasswordPanal.SetActive(false);
    }
    private void Awake()
    {
        int x=ReturnNftRole("Free");
        print("Alraeady Logged In Awake " + PlayerPrefs.GetInt("IsLoggedIn"));
        checkbool_preser_start = true;
        /*
        EmailPanal.SetActive(true);
         FirstPanal.SetActive(true);
        OTPPanal.SetActive(true);
        PasswordPanal.SetActive(true);
        usernamePanal.SetActive(true);
        LoginPanal.SetActive(true);
         SignUpPanal.SetActive(true);
        SignUpPanalwithPhone.SetActive(true);
        ForgetenterUserNamePanal.SetActive(true);
        ForgetEnterPasswordPanal.SetActive(true);
        StartCoroutine(waitforOneframe());
        */
        instance = this;
        if (!File.Exists(GameManager.Instance.GetStringFolderPath()))
        {
            SavaCharacterProperties.instance.CreateFileFortheFirstTime();
        }
        if (!PlayerPrefs.HasKey("iSignup"))
        {
            PlayerPrefs.SetInt("iSignup", 0);
            PlayerPrefs.SetInt("IsProcessComplete", 0); // check if guest or signup process is complete or not 
        }
    }

    //          public Text CongratulationText;
    // Start is called before the first frame update
    void Start()
    {
     //   StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
        //OpenUIPanal(11);
        //Adds a listener to the main input field and invokes a method when the value changes.
        //   mainfield_for_opt.gameObject.GetComponent<InputField>().onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        mainfieldOTPNew.OnValueChanged.AddListener(delegate { ValueChangeCheck(); });
        //pinNew[1].gameObject.GetComponent<InputField>().onValueChanged.AddListener(delegate { ValueChangeCheck(1); });
        //pinNew[2].gameObject.GetComponent<InputField>().onValueChanged.AddListener(delegate { ValueChangeCheck(2); });
        //pinNew[3].gameObject.GetComponent<InputField>().onValueChanged.AddListener(delegate { ValueChangeCheck(3); });
        BackBool = false;
        UIManager.Instance.LoginRegisterScreen = FirstPanal;
        UIManager.Instance.SignUpScreen = SignUpPanal;
        CountryCodeText.text = "+81";
        mobile_number = false;
        print("Alraeady Logged In " + PlayerPrefs.GetInt("IsLoggedIn"));

        if (PlayerPrefs.GetInt("IsLoggedIn") == 1 && PlayerPrefs.GetInt("WalletLogin") != 1)
        {
            print("Alraeady Logged In");
            MyClassOfLoginJson LoginObj = new MyClassOfLoginJson();
            LoginObj = LoginObj.CreateFromJSON(PlayerPrefs.GetString("UserNameAndPassword"));
            StartCoroutine(LoginUserWithNewT(ConstantsGod.API_BASEURL + ConstantsGod.LoginAPIURL, PlayerPrefs.GetString("UserNameAndPassword"), null, true));
            LoggedInAsGuest = false;
        }
        else if (PlayerPrefs.GetInt("WalletLogin") == 1)
        {
            PlayerPrefs.SetInt("IsLoggedIn", 1);
            LoggedInAsGuest = false;
            StartCoroutine(WalletLoggedInAccessGroup());
        }
        else
        {
            LoggedInAsGuest = true;
            DefaultEnteriesforManican.instance.ResetForPresets();
            GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
            SavaCharacterProperties.instance.LoadMorphsfromFile();
        }
        StartCoroutine(LoginGuest(ConstantsGod.API_BASEURL + ConstantsGod.guestAPI));
           StartCoroutine(LoginUserPresetOnly());
       
        
    }

    IEnumerator WalletLoggedInAccessGroup()
    {
        yield return new WaitForSeconds(1);

        if (PlayerPrefs.GetString("PremiumUserType") == "Access Pass" || PlayerPrefs.GetString("PremiumUserType") == "Extra NFT" || PlayerPrefs.GetString("PremiumUserType") == "djevent")
        {
            print("these are premium users");
            PremiumUsersDetails.Instance.GetGroupDetails(PlayerPrefs.GetString("PremiumUserType"));
        }
        else
        {
            PremiumUsersDetails.Instance.GetGroupDetails("freeuser");
        }
    }


    #region Sign Up Animations

    public void OnSignUpEmailTabPressed()
    {
        


            float r = 0.2f, g = 0.3f, b = 0.7f, a = 0.6f;
        if (emailScreen.activeInHierarchy)
            return;

        emailScreen.SetActive(true);
        numberScreen.SetActive(false);
        WalletScreen.SetActive(false);

        Debug.Log("Fix");

        //lol
        EmailFieldNew.Text = "";
        //EmailInputTextNew.Text = "";
        //EmailInputTextNew.gameObject.GetComponent<InputField>().Select();
        //EmailInputTextNew.gameObject.GetComponent<InputField>().ActivateInputField();
        //EmailInputTextNew.enabled = false;
        //StartCoroutine(WaitandActive());
        //EmailInputTextNew.enabled = true;
        //EmailInputTextNew.SelectOtherField();

        Color _temp = new Color();
        _temp = errorTextNumber.GetComponent<Text>().color;
        _temp.a = 0;
        // float r = 0.2f, g = 0.3f, b = 0.7f, a = 0.6f;
        errorTextNumber.GetComponent<Text>().color = _temp;
        StartCoroutine(Animate(EmailSelectedImg.rectTransform));
        emailTabText.gameObject.GetComponent<Text>().color = HighlightedColor;
        phoneTabText.gameObject.GetComponent<Text>().color = NormalColor;
        WalletTabText.gameObject.GetComponent<Text>().color = NormalColor;
        // emailTabText.fontStyle = FontStyle.Bold;
        // phoneTabText.fontStyle = FontStyle.Normal;

        // tabSelectorAnimator.Play("Phone");
    }

    public void OnSignUpPhoneTabPressed()
    {
        //Debug.Log("before if");
        PhoneFieldNew.Text = "";
        if (numberScreen.activeInHierarchy)
            return;
        //Debug.Log("after if");
        emailScreen.SetActive(false);
        numberScreen.SetActive(true);
        WalletScreen.SetActive(false);
        //PhoneInputTextNew.Text = "";
        //StartCoroutine(WaitandActive());
        //PhoneInputTextNew.gameObject.GetComponent<InputField>().Select();
        //PhoneInputTextNew.gameObject.GetComponent<InputField>().ActivateInputField();
        //PhoneInputTextNew.SelectOtherField();
        PhoneFieldNew.Text = "";
        Color _temp = new Color();
        _temp = errorTextEmail.GetComponent<Text>().color;
        _temp.a = 0;
        errorTextEmail.GetComponent<Text>().color = _temp;
        StartCoroutine(Animate(PhoneSelectedImg.rectTransform));
        phoneTabText.gameObject.GetComponent<Text>().color = HighlightedColor;
        emailTabText.gameObject.GetComponent<Text>().color = NormalColor;
        WalletTabText.gameObject.GetComponent<Text>().color = NormalColor;


        // tabSelectorAnimator.Play("Email");
    }
    public void OnSignUpWalletTabPressed()
    {
        if (WalletScreen.activeInHierarchy)
            return;
        emailScreen.SetActive(false);
        numberScreen.SetActive(false);
        WalletScreen.SetActive(true);
        StartCoroutine(Animate(WalletSelectedImg.rectTransform));
        WalletTabText.gameObject.GetComponent<Text>().color = HighlightedColor;
        emailTabText.gameObject.GetComponent<Text>().color = NormalColor;
        phoneTabText.gameObject.GetComponent<Text>().color = NormalColor;

        //  tabSelectorAnimator.Play("Wallet");   
    }
    private IEnumerator Animate(RectTransform targetPos)
    {

        print("Animate here");
        //tabSelectorAnimator.GetComponent<Image>().rectTransform.position = targetPos.position;  

        //   yield return null;
       
        float t = 0;
        var target = tabSelectorAnimator.GetComponent<Image>().rectTransform.position;

        while (t < .4f)
        {
            print(" t Animate here");
            tabSelectorAnimator.GetComponent<Image>().rectTransform.position = Vector3.Lerp(tabSelectorAnimator.GetComponent<Image>().rectTransform.position, targetPos.position, t * 3);
            t += Time.deltaTime;
            if (!ConnectingWallet.instance.walletFunctionalitybool)
            {
                imageObject = GameObject.FindGameObjectWithTag("Phoneimage2");
                imageObject2 = GameObject.FindGameObjectWithTag("Emailimage2");
                if (imageObject != null)
                {
                    PhoneSelectedImg = imageObject.GetComponent<Image>();
                }

                if (imageObject2 != null)
                {
                    EmailSelectedImg = imageObject2.GetComponent<Image>();

                }
            }
            yield return null;
        }
    }


    #endregion

    public void BackFtn(int Openbackint)
    {
        print(Openbackint);
        if (ForgetPasswordBool)
        {
            OpenUIPanal(14);
            ForgetPasswordBool = false;
        }
        else
        {
            if (SignUpWithPhoneBool)
            {
                if (Openbackint == 2)
                {
                    // PhoneInputTextNew.Text = "";
                    PhoneFieldNew.Text = "";
                    //Openbackint = 9;
                }
                if (Openbackint == 8)
                {
                    //   PhoneInputTextNew.Text = "";
                    PhoneFieldNew.Text = "";
                }
            }
            else if (SignUpWithEmailBool)
            {

            }
            OpenUIPanal(Openbackint);
        }
    }

    public void GoToRegistrationScreen(int R_Integer)
    {
        if (R_Integer == 9)
        {
            SignUpWithPhoneBool = true;
        }
        else if (R_Integer == 2)
        {
            SignUpWithPhoneBool = false;
        }
        OpenUIPanal(R_Integer);
    }

    IEnumerator WaitandActive()
    {
        yield return new WaitForSeconds(.01f);
    }
    TouchScreenKeyboard Currentkeyboard;


    // Invoked when the value of the text field changes.
    public void ValueChangeCheck()
    {
        string[] myOtpTxt = new string[text_to_show.Length];
        //  char[] charArr = new char[mainfield_for_opt.Text.Length];
        char[] charArr = new char[mainfieldOTPNew.Text.Length];
        //  charArr = mainfield_for_opt.Text.ToCharArray();
        charArr = mainfieldOTPNew.Text.ToCharArray();
        for (int i = 0; i < myOtpTxt.Length; i++)
        {
            if (i < charArr.Length)
            {
                myOtpTxt[i] = charArr[i].ToString();
                text_to_show[i].text = myOtpTxt[i].ToString();

            }
            else
            {
                myOtpTxt[i] = "";
                text_to_show[i].text = myOtpTxt[i].ToString();

            }
        }
    }

    public void BackButtonPressedhere()
    {

    }

    public void NewBack()
    {
        LoginEmailOrPhone.gameObject.GetComponent<InputFieldKeyboardClient>().enabled = false;
        LoginPassword.gameObject.GetComponent<InputFieldKeyboardClient>().enabled = false;
        LoginPanal.SetActive(false);
        if (chk_forAccountALreadyLogedin)
        {
            chk_forAccountALreadyLogedin = false;
            ShowWelcomeScreen();

        }
        else

            OpenUIPanal(1);
    }
    bool chk_forAccountALreadyLogedin;
    public void OpenUIPanal(int ActivePanalCounter)
    {
        if (!checkbool_preser_start)  // guest
        {
            PresetData_Jsons.lastSelectedPreset.gameObject.SetActive(false);
            PresetData_Jsons.lastSelectedPreset = null;
            PresetData_Jsons.clickname = null;

            StoreManager.instance.StartPanel_PresetParentPanel.SetActive(true);
            UserRegisterationManager.instance.usernamePanal.SetActive(false);
            return;
        }

        if (isSetXanaliyaUserName)
        {
            isSetXanaliyaUserName = false;
        }

        FirstPanal.SetActive(false);
        //EmailPanal.SetActive(false);
        OTPPanal.SetActive(false);
        PasswordPanal.SetActive(false);
        usernamePanal.SetActive(false);
        LoginPanal.SetActive(false);
        SignUpPanal.SetActive(false);
        //SignUpPanalwithPhone.SetActive(false);
        //ChangePasswordPanal.SetActive(false);
        //UpdateprofilePanal.SetActive(false);
        //TestingAPIsPanal.SetActive(false);
        RegistrationCompletePanal.SetActive(false);
        ForgetenterUserNamePanal.SetActive(false);
        ForgetEnterPasswordPanal.SetActive(false);

        if (errorTextEmail.GetComponent<Animator>().GetBool("playAnim"))
            errorTextEmail.GetComponent<Animator>().SetBool("playAnim", false);
        if (errorTextPassword.GetComponent<Animator>().GetBool("playAnim"))
            errorTextPassword.GetComponent<Animator>().SetBool("playAnim", false);
        if (errorTextNumber.GetComponent<Animator>().GetBool("playAnim"))
            errorTextNumber.GetComponent<Animator>().SetBool("playAnim", false);
        if (errorTextName.GetComponent<Animator>().GetBool("playAnim"))
            errorTextName.GetComponent<Animator>().SetBool("playAnim", false);
        if (errorTextPIN.GetComponent<Animator>().GetBool("playAnim"))
            errorTextPIN.GetComponent<Animator>().SetBool("playAnim", false);
        if (errorTextLogin.GetComponent<Animator>().GetBool("playAnim"))
            errorTextLogin.GetComponent<Animator>().SetBool("playAnim", false);
        if (errorTextForgetAPI.GetComponent<Animator>().GetBool("playAnim"))
            errorTextForgetAPI.GetComponent<Animator>().SetBool("playAnim", false);
        if (errorTextResetPasswordAPI.GetComponent<Animator>().GetBool("playAnim"))
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", false);

        switch (ActivePanalCounter)
        {
            case 1:
                {
                    if (shownWelcome)
                    {

                        if (PlayerPrefs.GetInt("iSignup") == 1)
                        {
                            PlayerPrefs.SetInt("iSignup", 0);

                            welcomeScreen.SetActive(true);
                            shownWelcome = true;
                        }
                        else
                            ShowWelcomeClosed();

                        //welcomeScreen.SetActive(true);
                    }
                    else
                    {
                        FirstPanal.SetActive(true);
                    }
                    break;
                }
            case 2:
                {
                    PlayerPrefs.SetInt("iSignup", 1);
                    //  EmailPanal.SetActive(true);
                    SignUpPanal.SetActive(true);
                    EmailFieldNew.Text = "";
                    //EmailInputTextNew.Text = "";
                    //EmailInputTextNew.gameObject.GetComponent<InputField>().Select();
                    //EmailInputTextNew.gameObject.GetComponent<InputField>().ActivateInputField();
                    //EmailInputTextNew.enabled = false;
                    //StartCoroutine(WaitandActive());
                    //EmailInputTextNew.enabled = true;
                    //EmailInputTextNew.SelectOtherField();
                    break;
                }
            case 3:
                {
                    OTPPanal.SetActive(true);
                    //  mainfield_for_opt.Text = "";
                    //  mainfield_for_opt.SelectOtherField();
                    mainfieldOTPNew.Text = "";
                    mainfieldOTPNew.Select();
                    for (int i = 0; i < text_to_show.Length; i++)
                    {
                        text_to_show[i].text = "";
                    }
                    // for (int i = 0; i < pinNew.Count; i++)
                    //{
                    //    //   pinNew[i].gameObject.GetComponent<InputField>().text = "";
                    //    pinNew[i].Text = "";
                    //}        
                    //OTPFieldBool = false;
                    // pinNew[0].SelectOtherField();
                    // pinNew[0].gameObject.GetComponent<InputField>().Select();
                    //  pinNew[0].gameObject.GetComponent<InputField>().ActivateInputField();
                    break;
                }
            case 4:
                {
                    PasswordPanal.SetActive(true);
                    Password1New.Text = "";
                    Password2New.Text = "";
                    //  Password1InputTextShiftCode.EmptyPassword();
                    //  Password2ConfirmInputTextShiftCode.EmptyPassword();
                    break;
                }
            case 5:
                {
                    usernamePanal.SetActive(true);
                    UsernameFieldAdvance.Text = "";
                    //StartCoroutine(WaitandActive());
                    //UsernameTextNew.Text = "";
                    //UsernameTextNew.enabled = false;
                    //UsernameTextNew.enabled = true;
                    //UsernameTextNew.gameObject.GetComponent<InputField>().Select();
                    //UsernameTextNew.gameObject.GetComponent<InputField>().ActivateInputField();
                    //UsernameTextNew.SelectOtherField();
                    break;
                }
            case 6:
                {

                    LoginPanal.SetActive(true);
                    /*
                    LoginEmailNew.Text = "";
                    LoginEmailNew.gameObject.GetComponent<InputField>().Select();
                    LoginEmailNew.gameObject.GetComponent<InputField>().ActivateInputField();
                    LoginEmailNew.enabled = false;
                    StartCoroutine(WaitandActive());
                    LoginEmailNew.enabled = true;
                    LoginEmailNew.SelectOtherField();
                    */
                   
                    LoginEmailOrPhone.Text = "";
                    LoginEmailOrPhone.Select();
                    savePasswordList.instance.DeleteONStart();
                    //  LoginPasswordShiftCode.EmptyPassword();
                    LoginPassword.Text = "";
                    chk_forAccountALreadyLogedin = true;
                    break;
                }
            case 7:
                {
                    if (shownWelcome)
                    {
                        Debug.LogError("show welcome");
                        PlayerPrefs.SetInt("shownWelcome", 1);

                        ShowWelcomeClosed();
                        LoggedIn = true;
                    }
                    else
                    {
                        Debug.LogError("show welcome else");
                        LoggedIn = true;
                        GameManager.Instance.SignInSignUpCompleted();
                    }
                    break;
                }
            case 8:
                {

                    PlayerPrefs.SetInt("iSignup", 1);// going for register user
                    SignUpPanal.SetActive(true);
                    OnSignUpEmailTabPressed();

                    OnSignUpPhoneTabPressed();
                   

                    break;
                }
            case 9:
                {
                    SignUpPanalwithPhone.SetActive(true);
                    PhoneFieldNew.Text = "";
                    //StartCoroutine(WaitandActive());
                    //PhoneInputTextNew.gameObject.GetComponent<InputField>().Select();
                    //PhoneInputTextNew.gameObject.GetComponent<InputField>().ActivateInputField();
                    //PhoneInputTextNew.SelectOtherField();
                    break;
                }
            case 10:
                {
                    OldPasswordField.Text = "";
                    ChangePassword1.Text = "";
                    ChangePassword2.Text = "";
                    ChangePasswordPanal.SetActive(true);
                    OldPasswordField.InputField.Select();
                    OldPasswordField.InputField.ActivateInputField();
                    break;
                }
            case 11:
                {
                    GenderField.Text = "";
                    JobField.Text = "";
                    CountryField.Text = "";
                    BioField.Text = "";
                    UpdateprofilePanal.SetActive(true);
                    break;
                }
            case 12:
                {
                    TestingAPIsPanal.SetActive(true);
                    break;
                }
            case 13:
                {
                    RegistrationCompletePanal.SetActive(true);
                    StoreManager.instance.StartPanel_PresetParentPanel.SetActive(true);
                    if (shownWelcome)
                        ShowWelcomeClosed();
                    break;
                }
            case 14:
                {
                    ForgetPasswordTokenAfterVerifyling = "";
                    ForgetenterUserNamePanal.SetActive(true);
                    EmailOrPhone_Forget_NewField.Text = "";
                    //StartCoroutine(WaitandActive());
                    //EmailOrPhone_ForgetPasswrod.Text = "";
                    //EmailOrPhone_ForgetPasswrod.SelectOtherField();

                    break;
                }
            case 15:
                {
                    ForgetEnterPasswordPanal.SetActive(true);
                    Password1_ForgetNewField.Text = "";
                    Password2_ForgetNewField.Text = "";

                    //StartCoroutine(WaitandActive());
                    //InputTextShiftCodeChangePass.EmptyPassword();
                    //InputTextShiftCodeChangePass2.EmptyPassword();

                    //    Password1_ForgetPasswrod.Text="";
                    //    Password2_ForgetPasswrod.Text="";
                    // Password1_ForgetPasswrod.SelectOtherField();
                    break;
                }
        }
    }
    /// <SignUpWithPhoneNumber>
    // DifferentAPI,s Call

    // Started Device Token 

    string uniqueID2()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        int z1 = UnityEngine.Random.Range(0, 1000000);
        int z2 = UnityEngine.Random.Range(0, 1000000);
        string uid = currentEpochTime + ":" + z1 + ":" + z2;
        return uid;
    }

    public void SubmitSetDeviceToken()
    {
        //  print("submit Set device ID here");
        string l_DeivceID = uniqueID();
        // string l_DeivceID = PlayerPrefs.GetString("AppID2");
        MyClassForSettingDeviceToken myObject = new MyClassForSettingDeviceToken();
        string bodyJson = JsonUtility.ToJson(myObject.GetUpdatedDeviceToken(l_DeivceID)); ;
        StartCoroutine(HitSetDeviceTokenAPI(ConstantsGod.API_BASEURL + ConstantsGod.SetDeviceTokenAPI, bodyJson, l_DeivceID));

        // TokenAfterRegister
    }
    IEnumerator HitSetDeviceTokenAPI(string url, string Jsondata, string LocalGetDeviceID)
    {
        // print("Body " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }
        //  print(request.GetRequestHeader("Authorization"));
        //  print(request.isDone);
        Debug.Log(request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                if (myObject1.success)
                {
                    PlayerPrefs.SetString("DeviceToken", LocalGetDeviceID);
                    //    print("device ID here is " + LocalGetDeviceID);
                    //  print("Set Device Token succesfully");  
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                Debug.LogError("Network error in set device token");
            }
            else
            {
                if (request.error != null)
                {
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        Debug.LogError("Success false in  in set device token");
                    }
                }
            }
        }
    }




    [Serializable]
    public class MyClassForSettingDeviceToken : JsonObjectBase
    {
        public string deviceToken;
        public MyClassForSettingDeviceToken GetUpdatedDeviceToken(string L_deviceTkn)
        {
            MyClassForSettingDeviceToken myObj = new MyClassForSettingDeviceToken();
            myObj.deviceToken = L_deviceTkn;
            return myObj;
        }
    }
    IEnumerator WaitAndLogout()
    {
        if (SNSSettingController.Instance != null)
        {
            SNSSettingController.Instance.LogoutSuccess();
        }
        yield return null;
    }
    IEnumerator HitLogOutAPI(string url, string Jsondata)
    {
        LoadingHandler.Instance.characterLoading.gameObject.SetActive(true);
        print("Body of Logout " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        yield return request.SendWebRequest();
        //  print(request.GetRequestHeader("Authorization"));
        //  print(request.isDone);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    PlayerPrefs.SetInt("IsLoggedIn", 0);
                    PlayerPrefs.SetInt("WalletLogin", 0);
                    //PlayerPrefs.GetString("LoginToken");
                    //  print("hey Logout here");  
                    PlayerPrefs.SetString("LoginToken", "");

                    LoggedInAsGuest = false;
                    DefaultEnteriesforManican.instance.DefaultReset();
                    yield return new WaitForSeconds(0.1f);
                    resetClothstoGuest();

                    PlayerPrefs.SetString("UserName", "");
                    LoggedIn = false;

                    PremiumUsersDetails.Instance.testing = false;
                    PlayerPrefs.DeleteAll();//Delete All PlayerPrefs After Logout Success.......
                    PlayerPrefs.SetString("TermsConditionAgreement", "Agree");
                    PlayerPrefs.Save();
                    yield return StartCoroutine(WaitAndLogout());
                    yield return StartCoroutine(LoginGuest(ConstantsGod.API_BASEURL + ConstantsGod.guestAPI, true));
                    ConstantsGod.UserRoles = new List<string>() { "Guest" };
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().AfterLogout();
                    LoadPlayerAvatar.instance_loadplayer.avatarButton.gameObject.SetActive(false);
                    //  PremiumUsersDetails.Instance.GetGroupDetails("guest");

                    //   print("Set Device Token succesfully");
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                //errorTextPassword.GetComponent<Text>().text = request.error.ToUpper();
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextPassword.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        //   print("Hey success false " + myObject1.msg);
                        errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                        //errorTextPassword.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        errorHandler.ShowErrorMessage(ErrorType.Default_Message.ToString(), errorTextPassword.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                    }
                }
            }
        }
        LoadingHandler.Instance.characterLoading.gameObject.SetActive(false);

        StoreManager.instance.CheckWhenUserLogin();
    }

    public void ResetDataAfterLogoutSuccess()//rik
    {
        shownWelcome = false;
        //for Reset Avatar Selection Screen Previous Account Data.......
        PlayerPrefs.SetInt("PresetValue", -1);
        if (PresetData_Jsons.lastSelectedPreset != null)
        {
            PresetData_Jsons.lastSelectedPreset.gameObject.SetActive(false);
            PresetData_Jsons.lastSelectedPreset = null;
        }
        PresetData_Jsons.clickname = "";
        for (int i = 0; i < StoreManager.instance.PresetArrayContent.transform.childCount; i++)
        {
            StoreManager.instance.PresetArrayContent.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }
        StoreManager.instance.PresetArrayContent.transform.parent.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        //end reset

    }

    void resetClothstoGuest()
    {

        SavaCharacterProperties.instance.LoadMorphsfromFile(); // loading morohs 
                                                               //  DefaultEnteriesforManican.instance.LastSaved_Reset();
                                                               //     ServerSIdeCharacterHandling.Instance.loadprevious();  //Load last saved values
    }                                                         // Ending Device Token 

    // Submit Logout
    public void SubmitLogoutAccount()
    {
        //   print("SubmitLogoutDevice here");

        if (PlayerPrefs.GetString("AppID2") == "")
        {
            SubmitSetDeviceToken();
        }
        else
        {
            MyClassForSettingDeviceToken myObject = new MyClassForSettingDeviceToken();
            string bodyJson = JsonUtility.ToJson(myObject.GetUpdatedDeviceToken(uniqueID())); ;
            StartCoroutine(HitLogOutAPI(ConstantsGod.API_BASEURL + ConstantsGod.LogOutAPI, bodyJson));
        }
        //   print("Submit Logout");
    }

    // Submit GetUser Details        
    public void SubmitGetUserDetails()
    {
        //  print("Submit GetUser Details");
        StartCoroutine(HitGetUserDetails(ConstantsGod.API_BASEURL + ConstantsGod.GetUserDetailsAPI, ""));
    }

    [System.Serializable]
    public class ClassforUserDetails : JsonObjectBase
    {
        public string success;
        public JsondataOfUserDetails data;
        public string msg;
        public ClassforUserDetails CreateFromJSON(string jsonString)
        {
            //  print("Person " + jsonString);
            return JsonUtility.FromJson<ClassforUserDetails>(jsonString);
        }
    }

    [System.Serializable]
    public class JsondataOfUserDetails : JsonObjectBase
    {
        public string id;
        public string name;
        public string dob;
        public string phoneNumber;
        public string email;
        public string avatar;
        public string isVerified;
        public string isRegister;
        public string isDeleted;
        public string createdAt;
        public string updatedAt;
        public UserProfileForUserDetails userProfile;

        public static JsondataOfUserDetails CreateFromJSON(string jsonString)
        {
            //  print("Person " + jsonString);
            return JsonUtility.FromJson<JsondataOfUserDetails>(jsonString);
        }
    }

    [System.Serializable]
    public class UserProfileForUserDetails : JsonObjectBase
    {
        public string id;
        public string userId;
        public string gender;
        public string job;
        public string country;
        public string bio;
        public string isDeleted;
        public string createdAt;
        public string updatedAt;
        public static JsondataOfUserDetails CreateFromJSON(string jsonString)
        {
            //  print("Person " + jsonString);
            return JsonUtility.FromJson<JsondataOfUserDetails>(jsonString);
        }
    }

    IEnumerator HitGetUserDetails(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
            yield return request.SendWebRequest();
            // if (request.isNetworkError)
            //{
            //    Debug.Log("Error: " + request.error);
            //}
            //else
            //{
            //    Debug.Log("Received: " + request.downloadHandler.text);
            //}

            ClassforUserDetails myObject1 = new ClassforUserDetails();
            if (!request.isHttpError && !request.isNetworkError)
            {
                myObject1 = myObject1.CreateFromJSON(request.downloadHandler.text);
                if (request.error == null)
                {
                    Debug.Log(request.downloadHandler.text);
                    //    print(myObject1.data.userProfile.country);
                    //   print(myObject1.data.email);

                    if (myObject1.success == "true")
                    {
                        //   print("Success of user details");
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                    //errorTextPassword.GetComponent<Text>().text = request.error.ToUpper();
                    errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextPassword.GetComponent<Text>());
                    StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                }
                else
                {
                    if (request.error != null)
                    {
                        if (myObject1.success == "false")
                        {
                            //      print("Hey success false " + myObject1.msg);
                            errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                            //errorTextPassword.GetComponent<Text>().text = myObject1.msg.ToUpper();
                            errorHandler.ShowErrorMessage(myObject1.msg, errorTextPassword.GetComponent<Text>());
                            StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                        }
                    }
                }
            }
        }
    }

    //Start Submit UpdateUserAvatar
    public void SubmitUpdateUserAvatar()
    {
        //  print("Submit UpdateUserAvatar");
        MyClassForUpdatingUserAvatar myObject = new MyClassForUpdatingUserAvatar();
        string bodyJson = JsonUtility.ToJson(myObject.GetUpdatedUserAvatar("updated avatar")); ;
        StartCoroutine(HitUpdateAvatarAPI(ConstantsGod.API_BASEURL + ConstantsGod.UpdateAvatarAPI, bodyJson));
    }

    [Serializable]
    public class MyClassForUpdatingUserAvatar : JsonObjectBase
    {
        public string avatar;
        public MyClassForUpdatingUserAvatar GetUpdatedUserAvatar(string L_userAvt)
        {
            MyClassForUpdatingUserAvatar myObj = new MyClassForUpdatingUserAvatar();
            myObj.avatar = L_userAvt;
            return myObj;
        }
    }

    IEnumerator HitUpdateAvatarAPI(string url, string Jsondata)
    {
        // print("Body " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        yield return request.SendWebRequest();
        //  print(request.GetRequestHeader("Authorization"));
        ///   print(request.isDone);
        //  Debug.Log(request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                //  Debug.Log(request.downloadHandler.text);
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    //    print("Avatar Updated Success");
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                //errorTextPassword.GetComponent<Text>().text = request.error.ToUpper();
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextPassword.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        //     print("Hey success false " + myObject1.msg);
                        errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                        //errorTextPassword.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        errorHandler.ShowErrorMessage(ErrorType.Default_Message.ToString(), errorTextPassword.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                    }
                }
            }
        }
    }

    //END Submit UpdateUserAvatar

    // Submit ForgetPassword Section

    public void SubmitForgetPassword()
    {
        //  string ForgetPassword_EmlOrPhone = EmailOrPhone_ForgetPasswrod.Text;
        string ForgetPassword_EmlOrPhone = EmailOrPhone_Forget_NewField.Text;
        if (ForgetPassword_EmlOrPhone == "")
        {
            //  print("Email Or Password should not be empty");
            errorTextForgetAPI.GetComponent<Animator>().SetBool("playAnim", true);
            //  if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            // errorTextForgetAPI.GetComponent<Text>().text = "全ての欄に入力してください";
            // }
            // else
            // {
            // errorTextForgetAPI.GetComponent<Text>().text = "Fields Should not be empty";
            // }
            errorHandler.ShowErrorMessage(ErrorType.Fields__empty.ToString(), errorTextForgetAPI.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextForgetAPI.GetComponent<Animator>()));
            return;
        }
        GameObject _ForgetPasswordBtnObject = EventSystem.current.currentSelectedGameObject;
        _ForgetPasswordBtnObject = _ForgetPasswordBtnObject.transform.Find("Loader").gameObject;

        if (_ForgetPasswordBtnObject.activeInHierarchy)
            return;
        _ForgetPasswordBtnObject.SetActive(true);

        string url = ConstantsGod.API_BASEURL + ConstantsGod.ForgetPasswordAPI;
        ForgetPassword_EmlOrPhone = ForgetPassword_EmlOrPhone.Trim();
        ForgetPassword_EmlOrPhone = ForgetPassword_EmlOrPhone.ToLower();
        MyClassOfPostingForgetPassword myObject = new MyClassOfPostingForgetPassword();
        string bodyJson;
        bodyJson = JsonUtility.ToJson(myObject.GetForgetPassworddata(ForgetPassword_EmlOrPhone));
        StartCoroutine(HitForgetPasswordAPI(url, bodyJson, ForgetPassword_EmlOrPhone, _ForgetPasswordBtnObject));
    }
    public IEnumerator HitForgetPasswordAPI(string url, string Jsondata, string localEmail_oR_PhoneNumber, GameObject _loader)
    {
        //  print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        MyClassNewApi myObject1 = new MyClassNewApi();
        myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                //   Debug.Log(request.downloadHandler.text);
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    if (_loader != null)
                        _loader.SetActive(false);
                    ForgetPasswordBool = true;
                    SignUpWithPhoneBool = false;
                    OpenUIPanal(3);
                    ForgetPasswordEmlOrPhnContainer = localEmail_oR_PhoneNumber;
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                if (_loader != null)
                    _loader.SetActive(false);
                errorTextForgetAPI.GetComponent<Animator>().SetBool("playAnim", true);

                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     errorTextForgetAPI.GetComponent<Text>().text = "接続状態が悪く繋がりません";
                // }
                // else
                // {
                //     errorTextForgetAPI.GetComponent<Text>().text = request.error.ToUpper();
                // }
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextForgetAPI.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextForgetAPI.GetComponent<Animator>()));
            }
            else
            {
                if (_loader != null)
                    _loader.SetActive(false);
                if (request.error != null)
                {

                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        errorTextForgetAPI.GetComponent<Animator>().SetBool("playAnim", true);
                        // if (Application.systemLanguage == SystemLanguage.Japanese  )
                        // {
                        //     //3: Email address is already exists
                        //     errorTextForgetAPI.GetComponent<Text>().text = "同じメールアドレスが登録されています";
                        // }
                        // else
                        // {
                        //     errorTextForgetAPI.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        // }
                        //errorHandler.ShowErrorMessage(ErrorType.User_Does_Not_Exist_with_Email.ToString(), errorTextForgetAPI.GetComponent<Text>());
                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextForgetAPI.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextForgetAPI.GetComponent<Animator>()));
                    }
                }
            }
        }
    }
    private string NewPasswordForgetApi;

    public void SubmitResetPassword()
    {
        print("Submit Reset Password");

        // print("Submit Password");

        // InputTextShiftCodeChangePass2
        //     string NewPassword = Password1_ForgetPasswrod.Text.Trim();
        //  string ReNewPassword = Password2_ForgetPasswrod.Text.Trim();

        // string NewPassword = InputTextShiftCodeChangePass.GetText();
        string NewPassword = Password1_ForgetNewField.Text;
        //   string ReNewPassword = InputTextShiftCodeChangePass2.GetText();
        string ReNewPassword = Password2_ForgetNewField.Text;

        if (NewPassword == "" || ReNewPassword == "")
        {
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", true);
            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextResetPasswordAPI.GetComponent<Text>().text = "全ての欄に入力してください";
            // }
            // else
            // {
            //     errorTextResetPasswordAPI.GetComponent<Text>().text = "Fields should not be empty";
            // }
            errorHandler.ShowErrorMessage(ErrorType.Fields__empty.ToString(), errorTextResetPasswordAPI.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextResetPasswordAPI.GetComponent<Animator>()));
            return;
        }
        else if (NewPassword != ReNewPassword)
        {
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", true);

            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextResetPasswordAPI.GetComponent<Text>().text = "パスワードが一致していません";
            // }
            // else
            // {
            //     errorTextResetPasswordAPI.GetComponent<Text>().text = "Password not matched";
            // }
            errorHandler.ShowErrorMessage(ErrorType.Passwords_do_not_match.ToString(), errorTextResetPasswordAPI.GetComponent<Text>());

            StartCoroutine(WaitUntilAnimationFinished(errorTextResetPasswordAPI.GetComponent<Animator>()));
            //   print("Password not matched");
            return;
        }
        else
        {
            if (ValidatePassword(NewPassword, ReNewPassword, errorTextResetPasswordAPI.GetComponent<Text>()))
            {
                MyClassOfPostingReset myObject = new MyClassOfPostingReset();
                string bodyJson = JsonUtility.ToJson(myObject.GetdataFromClass(NewPassword));
                NewPasswordForgetApi = NewPassword;
                StartCoroutine(HitResetAPI(ConstantsGod.API_BASEURL + ConstantsGod.ForgetPasswordResetAPI, bodyJson));
            }
        }
    }

    // hardik password validation 6/6/2022
    private bool ValidatePassword(string password, string repass,Text errorTextResetPasswordAPI)
    {
        var input = password;
        // ErrorMessage = string.Empty;


        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,15}");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(input))
        {
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", true);
            errorHandler.ShowErrorMessage(ErrorType.Password_should_contain_at_least_one_lower_case_letter.ToString(), errorTextResetPasswordAPI.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextResetPasswordAPI.GetComponent<Animator>()));
            return false;
        }
        else if (!hasUpperChar.IsMatch(input))
        {
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", true);
            errorHandler.ShowErrorMessage(ErrorType.Password_should_contain_at_least_one_upper_case_letter.ToString(), errorTextResetPasswordAPI.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextResetPasswordAPI.GetComponent<Animator>()));

            return false;
        }
        else if (!hasMiniMaxChars.IsMatch(input) || !hasMiniMaxChars.IsMatch(repass))
        {
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", true);
            errorHandler.ShowErrorMessage(ErrorType.Password_should_not_be_lesser_than_8_or_greater_than_15_characters.ToString(), errorTextResetPasswordAPI.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextResetPasswordAPI.GetComponent<Animator>()));
            return false;

        }
        else if (!hasNumber.IsMatch(input))
        {
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", true);
            errorHandler.ShowErrorMessage(ErrorType.Password_should_contain_at_least_one_numeric_value.ToString(), errorTextResetPasswordAPI.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextResetPasswordAPI.GetComponent<Animator>()));
            return false;
        }

        else if (!hasSymbols.IsMatch(input))
        {
            errorTextResetPasswordAPI.GetComponent<Animator>().SetBool("playAnim", true);
            errorHandler.ShowErrorMessage(ErrorType.Password_should_contain_at_least_one_special_case_character.ToString(), errorTextResetPasswordAPI.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextResetPasswordAPI.GetComponent<Animator>()));
            return false;
        }
        else
        {
            return true;
        }
    }
    // end


    IEnumerator HitResetAPI(string url, string Jsondata)
    {
        //   print("Body " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", ForgetPasswordTokenAfterVerifyling);
        yield return request.SendWebRequest();
        // print(request.GetRequestHeader("Authorization"));
        //  Debug.Log(request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    if (ForgetPasswordBool)
                    {
                        //   ForgetPasswordEmlOrPhnContainer
                        //  NewPasswordForgetApi
                        savePasswordList.instance.saveDataFromForgetPassword(ForgetPasswordEmlOrPhnContainer, NewPasswordForgetApi);
                        OpenUIPanal(6);
                        ForgetPasswordTokenAfterVerifyling = "";
                        ForgetPasswordBool = false;
                    }
                    else
                    {
                        //   print("Registration With Name Completed ");
                        OpenUIPanal(13);
                        GameManager.Instance.SignInSignUpCompleted();
                        usernamePanal.SetActive(false);
                        LoggedIn = true;
                    }

                    //OpenUIPanal(6);
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     errorTextName.GetComponent<Text>().text = "接続状態が悪く繋がりません";
                // }
                // else
                // {
                //     errorTextName.GetComponent<Text>().text = request.error.ToUpper();
                // }
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextName.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        //  print("Hey success false " + myObject1.msg);
                        errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
                        // if (Application.systemLanguage == SystemLanguage.Japanese  )
                        // {
                        //     errorTextName.GetComponent<Text>().text = "名前が無効です";
                        // }
                        // else
                        // {
                        //     errorTextName.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        // }
                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextName.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
                    }
                }
            }
        }
    }



    [Serializable]
    public class MyClassOfPostingForgetPassword : JsonObjectBase
    {
        public string userName;
        public MyClassOfPostingForgetPassword GetForgetPassworddata(string eml)
        {
            MyClassOfPostingForgetPassword myObj = new MyClassOfPostingForgetPassword();
            myObj.userName = eml;
            return myObj;
        }
    }
    [Serializable]
    public class MyClassOfPostingForgetPasswordOTP : JsonObjectBase
    {
        public string userName;
        public string otp;
        public MyClassOfPostingForgetPasswordOTP GetdataFromClass(string usrnme, string otp)
        {
            MyClassOfPostingForgetPasswordOTP myObj = new MyClassOfPostingForgetPasswordOTP();
            myObj.userName = usrnme;
            myObj.otp = otp;
            return myObj;
        }
    }
    [Serializable]
    public class MyClassOfPostingReset : JsonObjectBase
    {
        public string password;
        public MyClassOfPostingReset GetdataFromClass(string Pass)
        {
            MyClassOfPostingReset myObj = new MyClassOfPostingReset();
            myObj.password = Pass;
            return myObj;
        }
    }

    [System.Serializable]
    public class ClassWithTokenofResetPassword
    {
        public string success;
        public JustTokenResetPassword data;
        public string msg;

        public static ClassWithTokenofResetPassword CreateFromJSON(string jsonString)
        {
            //  print("Person " + jsonString);
            return JsonUtility.FromJson<ClassWithTokenofResetPassword>(jsonString);
        }
    }

    [System.Serializable]
    public class JustTokenResetPassword
    {
        public string otpMatched;
        public string tempToken;

        public static JustTokenResetPassword CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<JustTokenResetPassword>(jsonString);
        }
    }


    // END ForgetPassword Section





    // Submit Delete Account

    public void SubmitDeleteAccount()
    {
        //  print("Submit Delete Account");
    }

    // DifferentAPI,s END



    //// UpdateUserProfile
    ///
    public void SubmitUpdateProfile()
    {
        string genderlocal = GenderField.Text.Trim();
        string joblocal = JobField.Text.Trim();
        string Countrylocal = CountryField.Text.Trim();
        string Biolocal = BioField.Text.Trim();
        if (genderlocal == "" || joblocal == "" || Countrylocal == "" || Biolocal == "")
        {
            //  print("None fields should be empty");
            return;
        }
        else
        {
            MyClassForUpdatingProfile myObject = new MyClassForUpdatingProfile();
            string bodyJson = JsonUtility.ToJson(myObject.GetUpdateProfiledata(genderlocal, joblocal, Countrylocal, Biolocal)); ;
            StartCoroutine(HitUpdateProfileAPI(ConstantsGod.API_BASEURL + ConstantsGod.UpdateProfileAPI, bodyJson));
        }
    }

    [Serializable]
    public class MyClassForUpdatingProfile : JsonObjectBase
    {
        public string gender;
        public string job;
        public string country;
        public string bio;
        public MyClassForUpdatingProfile GetUpdateProfiledata(string gnder, string jobb, string cntry, string bioo)
        {
            MyClassForUpdatingProfile myObj = new MyClassForUpdatingProfile();
            myObj.gender = gnder;
            myObj.job = jobb;
            myObj.country = cntry;
            myObj.bio = bioo;
            return myObj;
        }
    }
    IEnumerator HitUpdateProfileAPI(string url, string Jsondata)
    {
        //  print("Body " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        yield return request.SendWebRequest();
        //   print(request.GetRequestHeader("Authorization"));
        //   print(request.isDone);
        Debug.Log(request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                //  Debug.Log(request.downloadHandler.text);
                if (myObject1.success)
                {
                    //    print("Update Profile Successfully");
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                //errorTextPassword.GetComponent<Text>().text = request.error.ToUpper();
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextPassword.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        //   print("Hey success false " + myObject1.msg);
                        errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                        //errorTextPassword.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextPassword.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                    }
                }
            }
        }
    }

    // End UpdateUserProfile

    /// <SignUpWithPhoneNumber>
    public void SubmitPhoneNumber()
    {
        print(PhoneFieldNew.Text);
        // if (PhoneInputTextNew.Text == "")
        if (PhoneFieldNew.Text == "")
        {
            errorTextNumber.GetComponent<Animator>().SetBool("playAnim", true);

            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     // 5: Phone Number should not be empty
            //     errorTextNumber.GetComponent<Text>().text = "携帯番号を入力してください";
            // }   
            // else
            // {
            //     errorTextNumber.GetComponent<Text>().text = "Phone number should not be empty";
            // } 
            //
            errorHandler.ShowErrorMessage(ErrorType.Phone_number__empty.ToString(), errorTextNumber.GetComponent<Text>());

            StartCoroutine(WaitUntilAnimationFinished(errorTextNumber.GetComponent<Animator>()));
            return;
        }
        // if (PhoneInputTextNew.Text.Length > 10)
        if (PhoneFieldNew.Text.Length > 10)
        {
            errorTextNumber.GetComponent<Animator>().SetBool("playAnim", true);

            errorHandler.ShowErrorMessage(ErrorType.Enter_Valid_Number.ToString(), errorTextNumber.GetComponent<Text>());
            if (GameManager.currentLanguage == "ja" && CountryCodeText.text == "+81")
            {
                // 5: Phone Number should not be empty
                errorTextNumber.GetComponent<Text>().text = "市外局番を抜いて入力ください（例 080→80、090→90）";
            }

            StartCoroutine(WaitUntilAnimationFinished(errorTextNumber.GetComponent<Animator>()));
            return;
        }

        else
        {

            //   PhoneInputTextNew.Text = PhoneInputTextNew.Text.Trim();
            PhoneFieldNew.Text = PhoneFieldNew.Text.Trim();
            //    string phonenumberText = CountryCodeText.text + PhoneInputTextNew.Text;
            string phonenumberText = CountryCodeText.text + PhoneFieldNew.Text;
            string url = ConstantsGod.API_BASEURL + ConstantsGod.SendPhoneOTPAPI;
            if (ResendOTPBool)
            {
                url = ConstantsGod.API_BASEURL + ConstantsGod.ResendOTPAPI;
                ResendOTPBool = false;
            }

            GameObject NxtButtonObj = EventSystem.current.currentSelectedGameObject;
            if (NxtButtonObj)
            {
                currentSelectedNxtButton = NxtButtonObj.GetComponent<Button>();
            }

            if (NxtButtonObj.transform.Find("Loader") != null)
                NxtButtonObj = NxtButtonObj.transform.Find("Loader").gameObject;
            else
                NxtButtonObj = null;

            if (NxtButtonObj == null)
            {
                MyClassOfPhoneNumber myObject = new MyClassOfPhoneNumber();
                string bodyJson = JsonUtility.ToJson(myObject.GetdataFromClass(phonenumberText));
                StartCoroutine(HitPhoneAPI(url, bodyJson, phonenumberText));
            }
            else
            {
                if (NxtButtonObj.activeInHierarchy)
                    return;
                NxtButtonObj.SetActive(true);
                if (currentSelectedNxtButton)
                {
                    currentSelectedNxtButton.interactable = false;
                }
                //    print(url);
                MyClassOfPhoneNumber myObject = new MyClassOfPhoneNumber();
                string bodyJson = JsonUtility.ToJson(myObject.GetdataFromClass(phonenumberText));
                StartCoroutine(HitPhoneAPI(url, bodyJson, phonenumberText, NxtButtonObj));
            }
        }
    }

    public void NotNowHuman()
    {

        GameManager.Instance.NotNowOfSignManager();
    }

    /// <ChangePassword>
    public void SubmitChangePassword()
    {
        //  print("Submit Password");
        string oldPassword = OldPasswordField.Text.Trim();
        string NewPassword = ChangePassword1.Text.Trim();
        string ReNewPassword = ChangePassword2.Text.Trim();

        if (oldPassword == "")
        {
            //  print("Old password should not be empty");
            return;
        }
        else if (NewPassword == "" || ReNewPassword == "")
        {
            //    print("New Password should not be empty");
            return;
        }

        if (NewPassword != ReNewPassword)
        {
            //  print("Password not matched");
            return;
        }
        MyClassForChangePassword myObject = new MyClassForChangePassword();
        string bodyJson = JsonUtility.ToJson(myObject.GetChangePassworddata(oldPassword, NewPassword));
        StartCoroutine(HitChangePasswordAPI(ConstantsGod.API_BASEURL + ConstantsGod.ChangePasswordAPI, bodyJson));
        print(bodyJson);
    }

    IEnumerator HitChangePasswordAPI(string url, string Jsondata)
    {
        //  print("Body " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        yield return request.SendWebRequest();
        //   print(request.GetRequestHeader("Authorization"));
        //  print(request.isDone);
        Debug.Log(request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                //     Debug.Log(request.downloadHandler.text);
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    print("Change Password Successfully");
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                //errorTextPassword.GetComponent<Text>().text = request.error.ToUpper();
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextPassword.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        //         print("Hey success false " + myObject1.msg);
                        errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                        //errorTextPassword.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        //errorHandler.ShowErrorMessage(ErrorType.Wrong_Password, errorTextPassword.GetComponent<Text>());
                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextPassword.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                    }
                }
            }
        }
    }

    //  EndChangePassword
    // Send OTP to Phone Number
    IEnumerator HitPhoneAPI(string url, string Jsondata, string LPhoneNumber, GameObject _loader = null)
    {
        //print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        print(request.downloadHandler.text);
        if (request.downloadHandler.text.Contains("Enter Valid Number"))
        {
            Debug.Log("working");
            mobile_number = true;
        }
        else if (request.downloadHandler.text.Contains("User Already Exists With This Number"))
        {
            mobile_number = false;
        }
        MyClassNewApi myObject1 = new MyClassNewApi();
        myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);


        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (_loader != null)
                    _loader.SetActive(false);
                Debug.Log(request.downloadHandler.text);
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    if (currentSelectedNxtButton)
                    {
                        currentSelectedNxtButton.interactable = true;
                    }
                    SignUpWithPhoneBool = true;
                    SignUpWithEmailBool = false;
                    LocalPhoneNumber = LPhoneNumber;
                    OpenUIPanal(3);
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                if (_loader != null)
                {
                    if (currentSelectedNxtButton)
                    {
                        currentSelectedNxtButton.interactable = true;
                    }

                    _loader.SetActive(false);
                }

                errorTextNumber.GetComponent<Animator>().SetBool("playAnim", true);

                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     //6: Cannot Connect to Destination Host
                //     errorTextNumber.GetComponent<Text>().text = "接続状態が悪く繋がりません";
                // }
                // else
                // {
                //     errorTextNumber.GetComponent<Text>().text = request.error.ToUpper();
                // }   
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextNumber.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextNumber.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    //  print("Message Return: " + myObject1.data);
                    //myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        //   print("Hey success false " + myObject1.msg);

                        errorTextNumber.GetComponent<Animator>().SetBool("playAnim", true);

                        // if (Application.systemLanguage == SystemLanguage.Japanese  )
                        // {
                        //     //4: Phone number is already exists
                        //     errorTextNumber.GetComponent<Text>().text = "同じ携帯番号が登録されています";
                        // }     
                        // else
                        // {
                        //     errorTextNumber.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        // }
                        //if (!mobile_number)
                        //{
                        //    errorHandler.ShowErrorMessage(ErrorType.User_Already_Exist, errorTextNumber.GetComponent<Text>());
                        //}

                        //else if (mobile_number)
                        //{
                        //    errorHandler.ShowErrorMessage(ErrorType.Enter_Valid_Number, errorTextNumber.GetComponent<Text>());
                        //}

                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextNumber.GetComponent<Text>());


                        if (_loader != null)
                        {
                            if (currentSelectedNxtButton)
                            {
                                currentSelectedNxtButton.interactable = true;
                            }
                            _loader.SetActive(false);
                        }


                        StartCoroutine(WaitUntilAnimationFinished(errorTextNumber.GetComponent<Animator>()));
                    }
                }
            }
        }
    }

    public void ResendOTP()
    {
        if (ForgetPasswordBool)
        {
            SubmitForgetPassword();
        }
        else
        {
            ResendOTPBool = true;
            if (SignUpWithPhoneBool)
            {
                SubmitPhoneNumber();
            }
            else
            {
                SubmitEmail();
            }
        }


    }
    // END
    /// </SignUpWithPhoneNumber>

    // SignUpwithEmail
    public void SubmitEmail()
    {
        //  print(EmailInputTextNew.Text);
        //  if (EmailInputTextNew.Text == "")
        if (EmailFieldNew.Text == "")
        {
            errorTextEmail.GetComponent<Animator>().SetBool("playAnim", true);

            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //    
            //     errorTextEmail.GetComponent<Text>().text = "メールアドレスを入力してください";
            //  }
            // else
            // {
            //     errorTextEmail.GetComponent<Text>().text = "Email field should not be empty";
            //  }

            errorHandler.ShowErrorMessage(ErrorType.Email_field__empty.ToString(), errorTextEmail.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextEmail.GetComponent<Animator>()));
            return;
        }
        else
        {

            //  print("Email validity is " + IsValidEmail(EmailInputTextNew.Text));
            //  EmailInputTextNew.Text = EmailInputTextNew.Text.Trim();
            EmailFieldNew.Text = EmailFieldNew.Text.Trim();
            // if (IsValidEmail(EmailInputTextNew.Text))
            if (IsValidEmail(EmailFieldNew.Text))
            {
                //  string email = EmailInputTextNew.Text;
                string email = EmailFieldNew.Text;
                string url = ConstantsGod.API_BASEURL + ConstantsGod.SendEmailOTP;
                email = email.ToLower();
                if (ResendOTPBool)
                {
                    url = ConstantsGod.API_BASEURL + ConstantsGod.ResendOTPAPI;
                    ResendOTPBool = false;
                }

                GameObject NxtButtonObj = EventSystem.current.currentSelectedGameObject;
                if (NxtButtonObj)
                {
                    currentSelectedNxtButton = NxtButtonObj.GetComponent<Button>();
                }

                NxtButtonObj = NxtButtonObj.transform.Find("Loader").gameObject;

                if (NxtButtonObj.activeInHierarchy)
                    return;
                NxtButtonObj.SetActive(true);
                if (currentSelectedNxtButton)
                {
                    currentSelectedNxtButton.interactable = false;
                }

                MyClassOfPostingEmail myobjectOfEmail = new MyClassOfPostingEmail();
                string bodyJson = JsonUtility.ToJson(myobjectOfEmail.GetEmaildata(email));
                StartCoroutine(HitEmailAPIWithNewTechnique(url, bodyJson, email, NxtButtonObj));
            }
            else
            {
                errorTextEmail.GetComponent<Animator>().SetBool("playAnim", true);

                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     errorTextEmail.GetComponent<Text>().text = "有効なメールアドレスを入力してください";
                // }
                // else
                // {
                //     errorTextEmail.GetComponent<Text>().text = "Please enter valid email";
                // }
                errorHandler.ShowErrorMessage(ErrorType.Please_enter_valid_email.ToString(), errorTextEmail.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextEmail.GetComponent<Animator>()));
            }
        }
    }

    [Serializable]
    public class MyClassOfPostingEmail : JsonObjectBase
    {
        public string email;
        public MyClassOfPostingEmail GetEmaildata(string eml)
        {
            MyClassOfPostingEmail myObj = new MyClassOfPostingEmail();
            myObj.email = eml;
            return myObj;
        }
    }


    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public IEnumerator HitEmailAPIWithNewTechnique(string url, string Jsondata, string localEmail, GameObject _loader = null)
    {
        //    print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (_loader != null)
            {
                _loader.SetActive(false);
            }

            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    if (currentSelectedNxtButton)
                    {
                        currentSelectedNxtButton.interactable = true;
                    }
                    OpenUIPanal(3);
                    Email = localEmail;
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                if (_loader != null)
                {
                    if (currentSelectedNxtButton)
                    {
                        currentSelectedNxtButton.interactable = true;
                    }
                    _loader.SetActive(false);
                }
                errorTextEmail.GetComponent<Animator>().SetBool("playAnim", true);

                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     errorTextEmail.GetComponent<Text>().text = "接続状態が悪く繋がりません";
                // }
                // else
                // {
                //     errorTextEmail.GetComponent<Text>().text = request.error.ToUpper();
                // }
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextEmail.GetComponent<Text>());
                print("getting text from here");
                StartCoroutine(WaitUntilAnimationFinished(errorTextEmail.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);

                    print(request.downloadHandler.text);
                    if (!myObject1.success)
                    {
                        //if (request.downloadHandler.text.Contains(" must be a valid email"))
                        //{
                        //    errorHandler.ShowErrorMessage(ErrorType.Please_enter_valid_email, errorTextEmail.GetComponent<Text>());
                        //}
                        //else
                        //{

                        //    errorHandler.ShowErrorMessage(ErrorType.Same_Email_is_already_registered, errorTextEmail.GetComponent<Text>());

                        //}
                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextEmail.GetComponent<Text>());
                        errorTextEmail.GetComponent<Animator>().SetBool("playAnim", true);
                        if (_loader != null)
                        {
                            if (currentSelectedNxtButton)
                            {
                                currentSelectedNxtButton.interactable = true;
                            }
                            _loader.SetActive(false);
                        }
                        StartCoroutine(WaitUntilAnimationFinished(errorTextEmail.GetComponent<Animator>()));


                        // if (Application.systemLanguage == SystemLanguage.Japanese  )
                        // {
                        //     //3: Email address is already exists
                        //      errorTextEmail.GetComponent<Text>().text = "同じメールアドレスが登録されています";
                        // }
                        // else
                        // {
                        //     errorTextEmail.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        // }


                    }
                }
            }
        }
    }
    ///////  ENDEmailSection


    // Submitting OTP from Phone Button
    public void SubmitOTP()
    {
        string OTP = "";

        //  OTP = mainfield_for_opt.Text;
        OTP = mainfieldOTPNew.Text;
        // for (int i = 0; i < pinNew.Count; i++)
        // {
        //     OTP += pinNew[i].Text;  
        // }  
        //  print("OTP entered by user is " + OTP);
        if (OTP == "" || OTP.Length < 4)
        {
            errorTextPIN.GetComponent<Animator>().SetBool("playAnim", true);

            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextPIN.GetComponent<Text>().text = "認証コードを入力してください";
            // }
            // else
            // {
            //     errorTextPIN.GetComponent<Text>().text = "OTP fields should not be empty";
            // }
            errorHandler.ShowErrorMessage(ErrorType.OTP_fields__empty.ToString(), errorTextPIN.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextPIN.GetComponent<Animator>()));
            return;
        }
        if (ForgetPasswordBool)
        {
            string url = ConstantsGod.API_BASEURL + ConstantsGod.ForgetPasswordOTPAPI;
            MyClassOfPostingForgetPasswordOTP myobjectOfPhone = new MyClassOfPostingForgetPasswordOTP();
            //   print("Forget Password OTP Section " + OTP);
            string bodyJson = JsonUtility.ToJson(myobjectOfPhone.GetdataFromClass(ForgetPasswordEmlOrPhnContainer, OTP));
            //  print("Json is  " + bodyJson);
            StartCoroutine(HitOTPAPI(url, bodyJson));
        }
        else
        {
            // Phone OTP sending Section
            if (SignUpWithPhoneBool)
            {
                string url = ConstantsGod.API_BASEURL + ConstantsGod.VerifyPhoneOTPAPI;
                // int numberOTP = int.Parse(OTP);
                MyClassOfPostingPhoneOTP myobjectOfPhone = new MyClassOfPostingPhoneOTP();
                //   print("Phone OTP sending Section an Phone number is  " + LocalPhoneNumber);
                //   print("Phone OTP  " + OTP);
                string bodyJson = JsonUtility.ToJson(myobjectOfPhone.GetdataFromClass(LocalPhoneNumber, OTP));
                //   print("Json is  " + bodyJson);
                StartCoroutine(HitOTPAPI(url, bodyJson));
            }
            // Email OTP sending Section
            else
            {
                string url = ConstantsGod.API_BASEURL + ConstantsGod.VerifyEmailOTP;
                //  int numberOTP = int.Parse(OTP);
                MyClassOfPostingOTP myObject = new MyClassOfPostingOTP();
                string bodyJson = JsonUtility.ToJson(myObject.GetdataFromClass(Email, OTP));
                StartCoroutine(HitOTPAPI(url, bodyJson));
            }
        }

    }


    // Start Register User with password
    [Serializable]
    public class MyClassOfRegisterWithNumber : JsonObjectBase
    {
        public string phoneNumber;
        public string password;
        public MyClassOfRegisterWithNumber GetdataFromClass(string L_phonenbr, string passwrd)
        {
            MyClassOfRegisterWithNumber myObj = new MyClassOfRegisterWithNumber();
            myObj.phoneNumber = L_phonenbr;
            myObj.password = passwrd;
            return myObj;
        }
    }


    [Serializable]
    public class MyClassOfRegisterWithEmail : JsonObjectBase
    {
        public string email;
        public string password;
        public MyClassOfRegisterWithEmail GetdataFromClass(string L_eml, string passwrd)
        {
            MyClassOfRegisterWithEmail myObj = new MyClassOfRegisterWithEmail();
            myObj.email = L_eml;
            myObj.password = passwrd;
            return myObj;
        }

        public static MyClassOfRegisterWithEmail CreateFromJSON(string jsonString)
        {
            //  print("Person " + jsonString);
            return JsonUtility.FromJson<MyClassOfRegisterWithEmail>(jsonString);
        }
    }


    public void SubmitPassword()
    {
        print("submit password here");
        //  string pass1 = Password1InputTextShiftCode.GetText();
        string pass1 = Password1New.Text;
        //  string pass2 = Password2ConfirmInputTextShiftCode.GetText();
        string pass2 = Password2New.Text;
        print(pass1);
        print(pass2);

        if (pass1 == "" || pass2 == "")
        {
            print("Password Field should not be empty");
            errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextPassword.GetComponent<Text>().text = "パスワードを入力してください";
            // }
            // else
            // {
            //     errorTextPassword.GetComponent<Text>().text = "Password field should not be empty";
            // }
            errorHandler.ShowErrorMessage(ErrorType.Password_field__empty.ToString(), errorTextPassword.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
            return;
        }

        else
        {
            if (ValidatePassword(pass1, pass2, errorTextPassword.GetComponent<Text>()))
            {
                if (pass1 == pass2)
                {
                    password = pass1;

                    OpenUIPanal(5);

                }
                else
                {
                    errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);

                    // if (Application.systemLanguage == SystemLanguage.Japanese  )
                    // {
                    //     errorTextPassword.GetComponent<Text>().text = "パスワードが一致していません";
                    // }
                    // else
                    // {
                    //     errorTextPassword.GetComponent<Text>().text = "Password not matched";
                    // }
                    errorHandler.ShowErrorMessage(ErrorType.Passwords_do_not_match.ToString(), errorTextPassword.GetComponent<Text>());

                    StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                    //   print("Password not matched");
                }
            }
        }
    }
        

    IEnumerator RegisterUserWithNewTechnique(string url, string Jsondata, string JsonOfName, String NameofUser)
    {
        print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        ClassWithToken myObject = new ClassWithToken();
        myObject = ClassWithToken.CreateFromJSON(request.downloadHandler.text);
        print(myObject.data.token);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                //    Debug.Log(request.downloadHandler.text);
                //if (myObject.success == "true")
                if (myObject.success)
                {

                    MyClassOfRegisterWithEmail myobjectOfEmail = new MyClassOfRegisterWithEmail();

                    myobjectOfEmail = MyClassOfRegisterWithEmail.CreateFromJSON(Jsondata);

                    MyClassOfLoginJson myObject1 = new MyClassOfLoginJson();


                    string bodyJson = JsonUtility.ToJson(myObject1.GetdataFromClass(myobjectOfEmail.email, "", myobjectOfEmail.password, uniqueID()));


                    PlayerPrefs.SetString("UserNameAndPassword", bodyJson);


                    //    print("Token before " + myObject.data.token);
                    PlayerPrefs.SetString("LoginToken", myObject.data.token);
                    var parts = myObject.data.token.Split('.');
                    if (parts.Length > 2)
                    {
                        var decode = parts[1];
                        var padLength = 4 - decode.Length % 4;
                        if (padLength < 4)
                        {
                            decode += new string('=', padLength);
                        }
                        var bytes = System.Convert.FromBase64String(decode);
                        var userInfo = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                        print(userInfo);
                        LoginClass L_LoginObject = new LoginClass();
                        L_LoginObject = CheckResponceJsonOfLogin(userInfo);
                        PlayerPrefs.SetString("UserName", L_LoginObject.id);
                        PlayerPrefs.SetInt("IsLoggedIn", 1);
                        print("Alraeady Logged In " + PlayerPrefs.GetInt("IsLoggedIn"));
                        print("Welcome " + PlayerPrefs.GetString("UserName"));
                    }
                    PlayerPrefs.Save();
                    //PlayerPrefs.SetInt("IsLoggedIn", 1);
                    LoggedIn = true;
                    StartCoroutine(HitNameAPIWithNewTechnique(ConstantsGod.API_BASEURL + ConstantsGod.NameAPIURL, JsonOfName, NameofUser));
                    UIManager.Instance._footerCan.transform.GetChild(0).GetComponent<BottomTabManager>().HomeSceneFooterSNSButtonIntrectableTrueFalse();
                    //   print("token is " + PlayerPrefs.GetString("LoginToken"));
                    //     print("User Registered succesfully from password");
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);

                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     // 6: Cannot Connect to Destination Host
                //     errorTextPassword.GetComponent<Text>().text = "接続状態が悪く繋がりません";
                // }
                // else
                // {
                //     errorTextPassword.GetComponent<Text>().text = request.error.ToUpper();
                // }
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextPassword.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    //if (myObject.success == "false")
                    if (!myObject.success)
                    {
                        //    print("Hey success false " + myObject.msg);
                        errorTextPassword.GetComponent<Animator>().SetBool("playAnim", true);
                        // if (Application.systemLanguage == SystemLanguage.Japanese  )
                        // {
                        //     errorTextPassword.GetComponent<Text>().text = "ユーザーは既に存在します";
                        // }
                        // else
                        // {
                        //     errorTextPassword.GetComponent<Text>().text = myObject.msg.ToUpper();
                        // }
                        //errorHandler.ShowErrorMessage(ErrorType.User_Already_Exist , errorTextPassword.GetComponent<Text>());
                        errorHandler.ShowErrorMessage(myObject.msg, errorTextPassword.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextPassword.GetComponent<Animator>()));
                    }
                }
            }
        }
    }

    // End Register User with password



    public void EnterUserName()
    {

        //    print(PlayerPrefs.GetInt("shownWelcome"));
        //  print(PlayerPrefs.GetInt("iSignup"));
        //   print(PlayerPrefs.GetInt("IsProcessComplete"));
        //   string Localusername = UsernameTextNew.Text;
        string Localusername = UsernameFieldAdvance.Text;

        PlayerPrefs.SetString(ConstantsGod.GUSTEUSERNAME, Localusername);

        if (Localusername == "")// || Localusername.Contains(" "))
        {
            //  print("Username Field should not be empty");
            errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextName.GetComponent<Text>().text = "名前を入力してください";
            // }
            // else
            // {
            //     errorTextName.GetComponent<Text>().text = "Name Field should not be empty";
            //  }
            errorHandler.ShowErrorMessage(ErrorType.Name_Field__empty.ToString(), errorTextName.GetComponent<Text>());

            StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
            return;
        }
        else if (Localusername.StartsWith(" "))
        {
            errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextName.GetComponent<Text>().text = "名前を入力してください";
            // }
            // else
            // {
            //     errorTextName.GetComponent<Text>().text = "Name Field should not be empty";
            //  }
            errorHandler.ShowErrorMessage(ErrorType.UserName_Has_Space.ToString(), errorTextName.GetComponent<Text>());

            StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
            return;
        }

        if (isSetXanaliyaUserName)//rik
        {
            MyClassOfPostingName tempMyObject = new MyClassOfPostingName();
            string bodyJsonOfName1 = JsonUtility.ToJson(tempMyObject.GetNamedata(Localusername));
            StartCoroutine(HitNameAPIWithXanaliyaUser(ConstantsGod.API_BASEURL + ConstantsGod.NameAPIURL, bodyJsonOfName1, Localusername));
            return;
        }

        // if(PlayerPrefs.GetInt("IsProcessComplete")==0)
        if (PlayerPrefs.GetInt("shownWelcome") == 0 && PlayerPrefs.GetInt("IsProcessComplete") == 0 && PlayerPrefs.GetInt("iSignup") == 0)
        {
            print("--- Return using namepanel" + Localusername);
            PlayerPrefs.SetString("GuestName", Localusername);
            usernamePanal.SetActive(false);
            checkbool_preser_start = true;
            //  StoreManager.instance.OnSaveBtnClicked();
            PlayerPrefs.SetInt("shownWelcome", 1);
            if (PlayerPrefs.GetInt("shownWelcome") == 1)
                StoreManager.instance.OnSaveBtnClicked();
            PlayerPrefs.SetInt("IsProcessComplete", 1);// user is registered as guest/register.
            return;
        }
        //   print(PlayerPrefs.GetInt("shownWelcome"));
        //  print(PlayerPrefs.GetInt("iSignup"));
        //  print(PlayerPrefs.GetInt("IsProcessComplete"));
        PlayerPrefs.SetInt("IsProcessComplete", 1);  // 
                                                     //   print("Test passed");



        MyClassOfPostingName myObject = new MyClassOfPostingName();
        string bodyJsonOfName = JsonUtility.ToJson(myObject.GetNamedata(Localusername));
        //  print(bodyJson);
        // StartCoroutine(HitNameAPIWithNewTechnique(NameAPIURL, bodyJsonOfName, Localusername));
        if (SignUpWithPhoneBool)
        {
            string url = ConstantsGod.API_BASEURL + ConstantsGod.RegisterPhoneAPI;
            MyClassOfRegisterWithNumber myobjectOfPhone = new MyClassOfRegisterWithNumber();
            string _bodyJson = JsonUtility.ToJson(myobjectOfPhone.GetdataFromClass(LocalPhoneNumber, password));
            StartCoroutine(RegisterUserWithNewTechnique(url, _bodyJson, bodyJsonOfName, Localusername));
        }
        else
        {
            string url = ConstantsGod.API_BASEURL + ConstantsGod.RegisterWithEmail;
            MyClassOfRegisterWithEmail myobjectOfEmail = new MyClassOfRegisterWithEmail();
            string _bodyJson = JsonUtility.ToJson(myobjectOfEmail.GetdataFromClass(Email, password));
            StartCoroutine(RegisterUserWithNewTechnique(url, _bodyJson, bodyJsonOfName, Localusername));

        }
    }


    public void SubmitLoginCredentials()
    {
        savePasswordList.instance.DisableOnLoginButton();
        // print("Someone is calling me");
        //   string L_LoginEmail = LoginEmailNew.Text;
        string L_LoginEmail = LoginEmailOrPhone.Text;
        print("L_LoginEmail " + L_LoginEmail);
        //   string L_loginPassword = LoginPasswordShiftCode.GetText();
        string L_loginPassword = LoginPassword.Text;
        if (L_LoginEmail == "" || L_loginPassword == "")
        {
            //  print("Email Or Password should not be empty");
            errorTextLogin.GetComponent<Animator>().SetBool("playAnim", true);
            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextLogin.GetComponent<Text>().text = "全ての欄に入力してください";
            // }
            // else
            // {
            //     errorTextLogin.GetComponent<Text>().text = "Fields should not be empty";
            //  }
            errorHandler.ShowErrorMessage(ErrorType.Fields__empty.ToString(), errorTextLogin.GetComponent<Text>());
            StartCoroutine(WaitUntilAnimationFinished(errorTextLogin.GetComponent<Animator>()));
            return;
        }
        string url = ConstantsGod.API_BASEURL + ConstantsGod.LoginAPIURL;

        L_LoginEmail = L_LoginEmail.Trim();

        MyClassOfLoginJson myObject = new MyClassOfLoginJson();
        L_LoginEmail = L_LoginEmail.Trim();
        L_loginPassword = L_loginPassword.Trim();
        string bodyJson;

        GameObject _loginBtnObject = EventSystem.current.currentSelectedGameObject;
        _loginBtnObject = _loginBtnObject.transform.Find("Loader").gameObject;

        if (_loginBtnObject.activeInHierarchy)
            return;

        _loginBtnObject.SetActive(true);

        if (IsValidEmail(L_LoginEmail))
        {

            L_LoginEmail = L_LoginEmail.ToLower();
            bodyJson = JsonUtility.ToJson(myObject.GetdataFromClass(L_LoginEmail, "", L_loginPassword, uniqueID()));
        }
        else
        {
            bodyJson = JsonUtility.ToJson(myObject.GetdataFromClass("", L_LoginEmail, L_loginPassword));
        }
        //print("Start Json " + bodyJson);
        StartCoroutine(LoginUserWithNewT(url, bodyJson, _loginBtnObject));
    }



    //   AppID = uniqueID();
    string uniqueID()
    {

        if (PlayerPrefs.GetString("AppID2") == "")
        {
            print("Give unique key");
            //  DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            int z1 = UnityEngine.Random.Range(0, 1000);
            int z2 = UnityEngine.Random.Range(0, 1000);
            // string uid = currentEpochTime + ":" + z1 + ":" + z2;
            string uid = z1.ToString() + z2.ToString();
            PlayerPrefs.SetString("AppID2", uid);
            PlayerPrefs.Save();
            return PlayerPrefs.GetString("AppID2");
        }
        else
        {
            return PlayerPrefs.GetString("AppID2");
        }


    }

    public void LogOutFromOtherDevice()
    {
        print("LogoutFromOtherDevice");
        StartCoroutine(HitLogOutFromOtherDevice(ConstantsGod.API_BASEURL + ConstantsGod.LogoutFromotherDeviceAPI, PlayerPrefs.GetString("LogoutFromDeviceJSON")));
    }

    public IEnumerator HitLogOutFromOtherDevice(string URL, string _json)
    {
        print("URL of login is " + URL);
        print(_json);
        var request = new UnityWebRequest(URL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(_json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        print("json data is " + request.downloadHandler.text);
        MyClassNewApi obj_LogOut = new MyClassNewApi();
        obj_LogOut = obj_LogOut.Load(request.downloadHandler.text);
        print(obj_LogOut.msg + " | success: of Logout from other device " + obj_LogOut.success);


        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (obj_LogOut.success)
                {
                    Debug.Log("Success true in logout from other device  " + obj_LogOut.msg);
                    LogoutfromOtherDevicePanel.SetActive(false);
                    StartCoroutine(LoginUserWithNewT(ConstantsGod.API_BASEURL + ConstantsGod.LoginAPIURL, PlayerPrefs.GetString("JSONdataforlogin"), null, false));
                    //  PlayerPrefs.SetString("JSONdataforlogin", Jsondata);

                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                Debug.LogError("Network error in logout from other device");
            }
            else
            {
                if (request.error != null)
                {
                    if (!obj_LogOut.success)
                    {
                        Debug.LogError("Success false in logout from other device  " + obj_LogOut.msg);
                    }
                }
            }
        }





    }

    [Serializable]
    public class MyClassOfLogoutDevice : JsonObjectBase
    {
        public string email;
        public string phoneNumber;
        public string password;
        public MyClassOfLogoutDevice GetdataFromClass(string L_eml = "", string L_phonenbr = "", string passwrd = "")
        {
            MyClassOfLogoutDevice myObj = new MyClassOfLogoutDevice();
            myObj.email = L_eml;
            myObj.phoneNumber = L_phonenbr;
            myObj.password = passwrd;
            return myObj;
        }

        public MyClassOfLoginJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<MyClassOfLoginJson>(jsonString);
        }

    }






    public IEnumerator HitEmailAPI(string URL, string localEmail, WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                //  Debug.Log(www.downloadProgress);
                yield return null;
            }
            print(www.downloadHandler.text);
            if (www.isHttpError || www.isNetworkError)
            {

                //  Debug.LogError("Network Error");
                errorTextEmail.GetComponent<Animator>().SetBool("playAnim", true);
                StartCoroutine(WaitUntilAnimationFinished(errorTextEmail.GetComponent<Animator>()));
                errorTextEmail.GetComponent<Text>().text = www.error.ToUpper();
                //  Debug.Log("WWW Error: " + www.error);  
            }
            else
            {
                if (operation.isDone)
                {
                    Debug.Log(www.downloadHandler.text);
                    MyClassNewApi myObject = new MyClassNewApi();
                    myObject = CheckResponceJsonNewApi(www.downloadHandler.text);

                    //if (myObject.success == "true")
                    if (myObject.success)
                    {
                        OpenUIPanal(3);
                        Email = localEmail;
                    }
                    else
                    {
                        errorTextEmail.GetComponent<Animator>().SetBool("playAnim", true);
                        StartCoroutine(WaitUntilAnimationFinished(errorTextEmail.GetComponent<Animator>()));
                        errorTextEmail.GetComponent<Text>().text = myObject.msg.ToUpper();
                        //    print("Error Occured " + myObject.msg);
                    }
                }
            }
        }
    }
    ClassWithTokenofResetPassword myObjectofOTPForResetPassword;
    MyClassNewApi myObjectForOPT;
   

    IEnumerator HitOTPAPI(string url, string Jsondata)
    {
        // print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        // print("Json returned = " + request.downloadHandler.text);
        if (ForgetPasswordBool)
        {
            myObjectofOTPForResetPassword = new ClassWithTokenofResetPassword();
            myObjectofOTPForResetPassword = ClassWithTokenofResetPassword.CreateFromJSON(request.downloadHandler.text);
            print(myObjectofOTPForResetPassword.data.tempToken);

        }
        else
        {
            myObjectForOPT = new MyClassNewApi();
            myObjectForOPT = CheckResponceJsonNewApi(request.downloadHandler.text);
        }

        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (ForgetPasswordBool)
                {
                    if (myObjectofOTPForResetPassword.success == "true")
                    {
                        ForgetPasswordTokenAfterVerifyling = myObjectofOTPForResetPassword.data.tempToken;
                        OpenUIPanal(15);
                    }
                }
                else
                {
                    //if (myObjectForOPT.success == "true")
                    if (myObjectForOPT.success)
                    {
                        OpenUIPanal(4);
                    }
                }

            }
        }
        else
        {
            if (request.isNetworkError)
            {

                errorTextPIN.GetComponent<Animator>().SetBool("playAnim", true);
                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     errorTextPIN.GetComponent<Text>().text = "接続状態が悪く繋がりません";
                // }
                // else
                // {
                //     errorTextPIN.GetComponent<Text>().text = request.error.ToUpper();
                // }
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextPIN.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextPIN.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    if (ForgetPasswordBool)
                    {
                        if (myObjectofOTPForResetPassword.success == "false")
                        {
                            errorTextPIN.GetComponent<Animator>().SetBool("playAnim", true);
                            // if (Application.systemLanguage == SystemLanguage.Japanese  )
                            // {
                            //     errorTextPIN.GetComponent<Text>().text = "認証コードが正しくありません";
                            // }
                            // else
                            // {
                            //          errorTextPIN.GetComponent<Text>().text = myObjectofOTPForResetPassword.msg.ToUpper();
                            //   }
                            errorHandler.ShowErrorMessage(ErrorType.Authentication_Code_is_Incorrect.ToString(), errorTextPIN.GetComponent<Text>());
                            StartCoroutine(WaitUntilAnimationFinished(errorTextPIN.GetComponent<Animator>()));
                        }
                    }
                    else
                    {
                        //if (myObjectForOPT.success == "false"  )
                        if (!myObjectForOPT.success)
                        {
                            errorTextPIN.GetComponent<Animator>().SetBool("playAnim", true);
                            // if (Application.systemLanguage == SystemLanguage.Japanese  )
                            // {
                            //     errorTextPIN.GetComponent<Text>().text = "認証コードが正しくありません";
                            // }
                            // else
                            // {
                            //          errorTextPIN.GetComponent<Text>().text = myObjectForOPT.msg.ToUpper();
                            //  }
                            errorHandler.ShowErrorMessage(ErrorType.Authentication_Code_is_Incorrect.ToString(), errorTextPIN.GetComponent<Text>());
                            StartCoroutine(WaitUntilAnimationFinished(errorTextPIN.GetComponent<Animator>()));
                        }

                    }
                }
                if (myObjectofOTPForResetPassword.data.tempToken == null)
                {
                    errorTextPIN.GetComponent<Animator>().SetBool("playAnim", true);
                    errorHandler.ShowErrorMessage(ErrorType.Poor_connection_please_try_again.ToString(), errorTextPIN.GetComponent<Text>());
                    StartCoroutine(WaitUntilAnimationFinished(errorTextPIN.GetComponent<Animator>()));
                }

            }


        }
    }

    IEnumerator HitNameAPIWithNewTechnique(string url, string Jsondata, string localUsername)
    {
        print("Body " + Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
        yield return request.SendWebRequest();
        //  print(request.GetRequestHeader("Authorization"));
        //  print(request.isDone);
        Debug.Log(request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                //if (myObject1.success == "true")
                if (myObject1.success)
                {
                    PlayerPrefs.SetInt("IsLoggedIn", 1);
                    SubmitSetDeviceToken();
                    //  print("Registration With Name Completed ");
                    LoggedInAsGuest = false;

                    ServerSIdeCharacterHandling.Instance.GetDataFromServer();
                    PlayerPrefs.SetString("PlayerName", localUsername);

                    OpenUIPanal(13);
                    usernamePanal.SetActive(false);
                    LoggedIn = true;
                    //OpenUIPanal(6);  
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     errorTextName.GetComponent<Text>().text = "接続状態が悪く繋がりません"; 
                // }  
                // else
                // {
                //     errorTextName.GetComponent<Text>().text = request.error.ToUpper();
                // }  
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextName.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        //   print("Hey success false " + myObject1.msg);
                        errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
                        // if (Application.systemLanguage == SystemLanguage.Japanese  )
                        // {
                        //     errorTextName.GetComponent<Text>().text = "名前が無効です";
                        // }
                        // else
                        // {
                        //     errorTextName.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        // }
                        //errorHandler.ShowErrorMessage(ErrorType.Invalid_Username , errorTextName.GetComponent<Text>());
                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextName.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
                    }
                }
            }
        }
    }

    public bool isSetXanaliyaUserName = false;
   

    IEnumerator HitNameAPIWithXanaliyaUser(string url, string Jsondata, string localUsername)//rik
    {
        Debug.Log("HitNameAPIWithXanaliyaUser Url:" + url + "   :BodyJson:" + Jsondata + "  :userName:" + localUsername);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));

        yield return request.SendWebRequest();

        Debug.Log("Data:" + request.downloadHandler.text);
        MyClassNewApi myObject1 = new MyClassNewApi();
        if (!request.isHttpError && !request.isNetworkError)
        {
            myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
            if (request.error == null)
            {
                Debug.Log("Success Xanaliya Username set:" + request.downloadHandler.text);
                if (myObject1.success)
                {
                    isSetXanaliyaUserName = false;
                    PlayerPrefs.SetString("PlayerName", localUsername);
                    usernamePanal.transform.Find("Back-Btn (1)").gameObject.SetActive(true);
                    usernamePanal.SetActive(false);
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextName.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
            }
            else
            {
                if (request.error != null)
                {
                    myObject1 = CheckResponceJsonNewApi(request.downloadHandler.text);
                    if (!myObject1.success)
                    {
                        errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextName.GetComponent<Text>());
                        StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
                    }
                }
            }
        }
    }


    [System.Obsolete]
    IEnumerator LoginGuest(string url, bool ComesFromLogOut = false)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, "POST"))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                //  Debug.Log(www.downloadProgress);
                yield return null;
            }
            //  print("json data is " + www.downloadHandler.text);
            ClassWithToken myObject1 = new ClassWithToken();
            myObject1 = ClassWithToken.CreateFromJSON(www.downloadHandler.text);
            if (!www.isHttpError && !www.isNetworkError)
            {
                if (www.error == null)
                {
                    if (myObject1.success)
                    {
                        PlayerPrefs.SetString("GuestToken", myObject1.data.token);
                        if (PlayerPrefs.GetString("PremiumUserType") == "Access Pass" || PlayerPrefs.GetString("PremiumUserType") == "Extra NFT" || PlayerPrefs.GetString("PremiumUserType") == "djevent")
                        {
                            print("these are premium users");
                            PremiumUsersDetails.Instance.GetGroupDetails(PlayerPrefs.GetString("PremiumUserType"));
                        }
                        else
                        {
                            if (PlayerPrefs.GetInt("WalletLogin") != 1)
                            {
                                PremiumUsersDetails.Instance.GetGroupDetails("guest");
                            }
                        }
                        //PlayerPrefs.SetString("TermsConditionAgreement", "Agree");
                        PlayerPrefs.SetInt("firstTime", 1);
                        PlayerPrefs.Save();
                        //Talha Changes 
                        if (!ComesFromLogOut)
                        {
                            EventList.instance.GetWorldAPISNew();
                            StoreManager.instance.GetAllMainCategories();
                        }

                    }
                }
            }
        }
    }

    [System.Obsolete]
    IEnumerator LoginUserWithNewT(string url, string Jsondata, GameObject _loader = null, bool AutoLoginBool = false)
    {
        print("URL of login is " + url);
        print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }
        print("json data is " + request.downloadHandler.text);
        ClassWithToken myObject1 = new ClassWithToken();
        myObject1 = ClassWithToken.CreateFromJSON(request.downloadHandler.text);
        print(myObject1.msg + " | success: " + myObject1.success);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                if (myObject1.success)
                {
                    PlayerPrefs.SetString("UserNameAndPassword", Jsondata);
                    if (_loader != null)
                        _loader.SetActive(false);
                    print("Token is " + myObject1.data.token);
                    print("Xanalia token is " + myObject1.data.xanaliaToken);
                    XanaliaUserTokenId = myObject1.data.xanaliaToken;
                    PlayerPrefs.SetString("TermsConditionAgreement", "Agree");
                    PlayerPrefs.SetInt("shownWelcome", 1);
                    PlayerPrefs.SetInt("firstTime", 1);


                    if (!AutoLoginBool)
                    {
                        // savePasswordList.instance.saveData(LoginEmailNew.Text.Trim(), LoginPasswordShiftCode.GetText().Trim());
                        savePasswordList.instance.saveData(LoginEmailOrPhone.Text.Trim(), LoginPassword.Text.Trim());
                    }

                    PlayerPrefs.SetInt("WalletLogin", 0);
                    PlayerPrefs.SetString("LoginToken", myObject1.data.token);
                    PlayerPrefs.SetString("LoginTokenxanalia", myObject1.data.xanaliaToken);


                    if (myObject1.data.isAdmin)
                    {
                        PremiumUsersDetails.Instance.testing = true;
                    }
                    else
                    {
                        PremiumUsersDetails.Instance.testing = false;
                    }


                    //   { networkType: "mainnet", nftType: "mycollection", status: "my_collection", page: 1, limit: 40,…}
                    /*
                     
                        limit: 40
                        loggedIn: "60f0287d436b32d50cece467"
                        networkType: "mainnet"
                        nftType: "mycollection"
                        page: 1
                        status: "my_collection"
                     */

                    //  public NFTListMainNet AssignNFTList(int _limit, string _loggedIn, string _networkType, string _nftType, int _page, string _status)

                    if (PlayerPrefs.GetString("LoginTokenxanalia") != "")
                    {
                        ConnectServerDataExtraction.NFTListMainNet NFTCreateJsonMain = new ConnectServerDataExtraction.NFTListMainNet();
                        //NFTCreateJsonMain = NFTCreateJsonMain.AssignNFTList(40, "", "testnet", "mycollection", 1, "my_collection","");

                        string xanaliaNetworkType = "mainnet";
                        if(APIBaseUrlChange.instance!=null)
                        {
                            if(APIBaseUrlChange.instance.IsXanaLive)
                            {
                                xanaliaNetworkType = "mainnet";
                            }
                            else
                            {
                                xanaliaNetworkType = "testnet";
                            }
                        }


                        NFTCreateJsonMain = NFTCreateJsonMain.AssignNFTList(100, xanaliaNetworkType, "mycollection", 1);

                        var jsonObj = JsonUtility.ToJson(NFTCreateJsonMain);
                        print("Json is  : " + jsonObj);

                        StartCoroutine(XanaliaUserToken(ConstantsGod.API_BASEURL_XANALIA+ConstantsGod.userMy_Collection_Xanalia, jsonObj));
                        StartCoroutine(XanaliaNonCryptoNFTRole(ConstantsGod.API_BASEURL_XANALIA+ConstantsGod.getUserProfile_Xanalia));
                        //StartCoroutine(XanaliaUserToken("https://api.xanalia.com/user/my-collection", jsonObj));
                        //StartCoroutine(XanaliaNonCryptoNFTRole("https://api.xanalia.com/user/get-user-profile"));
                    }
                    else
                    {
                        print("ID of user is " + myObject1.data.user.id);
                        if (PlayerPrefs.GetString("PremiumUserType") == "Access Pass" || PlayerPrefs.GetString("PremiumUserType") == "Extra NFT")
                        {
                            print("these are premium users");
                            PremiumUsersDetails.Instance.GetGroupDetails(PlayerPrefs.GetString("PremiumUserType"));
                        }
                        else
                        {
                            PremiumUsersDetails.Instance.GetGroupDetails("freeuser");
                        }
                    }


                    SubmitSetDeviceToken();
                    LoggedInAsGuest = false;
                    getdatafromserver();

                    var parts = myObject1.data.token.Split('.');
                    if (parts.Length > 2)
                    {
                        var decode = parts[1];
                        var padLength = 4 - decode.Length % 4;
                        if (padLength < 4)
                        {
                            decode += new string('=', padLength);
                        }
                        var bytes = System.Convert.FromBase64String(decode);
                        var userInfo = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                        print(userInfo);
                        LoginClass L_LoginObject = new LoginClass();
                        L_LoginObject = CheckResponceJsonOfLogin(userInfo);
                        PlayerPrefs.SetString("UserName", L_LoginObject.id);
                        PlayerPrefs.SetInt("IsLoggedIn", 1);
                        print("Alraeady Logged In " + PlayerPrefs.GetInt("IsLoggedIn"));
                        PlayerPrefs.SetString("PlayerName", myObject1.data.user.name);
                        print("Welcome " + PlayerPrefs.GetString("UserName"));
                        usernamePanal.SetActive(false);
                        usernamePanal.SetActive(false);
                        //    m_EquipUI.BackFromArtbone();
                        PlayerPrefs.Save();

                        StoreManager.instance.CheckWhenUserLogin();

                        if (!AutoLoginBool)
                        {
                            OpenUIPanal(7);

                            if (PlayerPrefs.GetString("LoginTokenxanalia") != "" && string.IsNullOrEmpty(myObject1.data.user.name))//rik
                            {
                                OpenUIPanal(5);
                                isSetXanaliyaUserName = true;
                                usernamePanal.transform.Find("Back-Btn (1)").gameObject.SetActive(false);
                            }
                        }
                        if (UIManager.Instance != null)//rik
                        {
                            UIManager.Instance._footerCan.transform.GetChild(0).GetComponent<BottomTabManager>().HomeSceneFooterSNSButtonIntrectableTrueFalse();
                        }
                    }
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                errorTextLogin.GetComponent<Animator>().SetBool("playAnim", true);

                // if (Application.systemLanguage == SystemLanguage.Japanese  )
                // {
                //     errorTextLogin.GetComponent<Text>().text = "接続状態が悪く繋がりません";
                // }
                // else
                // {
                //     errorTextLogin.GetComponent<Text>().text = request.error.ToUpper();
                // }
                errorHandler.ShowErrorMessage(ErrorType.Poor_Connection.ToString(), errorTextLogin.GetComponent<Text>());
                StartCoroutine(WaitUntilAnimationFinished(errorTextLogin.GetComponent<Animator>()));
                if (_loader != null)
                    _loader.SetActive(false);
            }
            else
            {
                if (request.error != null)
                {
                    //if (myObject1.success == "false")
                    if (!myObject1.success)
                    {
                        errorTextLogin.GetComponent<Animator>().SetBool("playAnim", true);
                        print("Hey success false " + myObject1.msg);
                        if (myObject1.msg.Contains("You are already logged in another device"))
                        {
                            if (AutoLoginBool)
                            {
                                PlayerPrefs.DeleteAll();
                                yield return null;
                            }
                            else
                            {
                                PlayerPrefs.SetString("JSONdataforlogin", Jsondata);
                                LogoutfromOtherDevicePanel.SetActive(true);
                                MyClassOfLoginJson myObject = new MyClassOfLoginJson();
                                myObject = myObject.CreateFromJSON(Jsondata);
                                MyClassOfLogoutDevice logoutObj = new MyClassOfLogoutDevice();
                                string bodyJson2 = JsonUtility.ToJson(logoutObj.GetdataFromClass(myObject.email, myObject.phoneNumber, myObject.password));
                                print("bodyJson2 " + bodyJson2);
                                //  string bodyJson1 = JsonUtility.ToJson(myObject.GetdataFromClass("", "", "", uniqueID()));
                                PlayerPrefs.SetString("LogoutFromDeviceJSON", bodyJson2);
                            }
                        }

                        //if(myObject1.msg.Contains("Password is incorrect"))
                        //{
                        //    errorHandler.ShowErrorMessage(ErrorType.Wrong_Password, errorTextLogin.GetComponent<Text>());
                        //}

                        //else if(myObject1.msg.Contains("must be a valid email"))
                        //{
                        //    errorHandler.ShowErrorMessage(ErrorType.Please_enter_valid_email, errorTextLogin.GetComponent<Text>());
                        //}

                        //else
                        //{
                        //    errorHandler.ShowErrorMessage(ErrorType.User_Not_Valid, errorTextLogin.GetComponent<Text>());

                        //}

                        errorHandler.ShowErrorMessage(myObject1.msg, errorTextLogin.GetComponent<Text>());

                        if (_loader != null)
                            _loader.SetActive(false);

                        StartCoroutine(WaitUntilAnimationFinished(errorTextLogin.GetComponent<Animator>()));
                        //  if (Application.systemLanguage == SystemLanguage.Japanese  )
                        // {  
                        //     // 15: User is Not Valid or Registered
                        //     errorTextLogin.GetComponent<Text>().text = "無効または同じユーザー名が登録されています";
                        // }     
                        // else
                        // {  
                        //     errorTextLogin.GetComponent<Text>().text = myObject1.msg.ToUpper();
                        // }

                    }
                }
            }
        }

    }
   
    int ReturnNftRole(string role)
    {
        role=role.Replace('-', '_');
        switch (role)
        {
            case "dj_event":
                {
                    return ((int)NftRolePriority.dj_event);
                }
            case "alpha_pass":
                {
                    return ((int)NftRolePriority.alpha_pass);
                }
            case "premium":
                {
                    return ((int)NftRolePriority.premium);
                }
            case "free":
                {
                    return ((int)NftRolePriority.free);
                }
            case "guest":
                {
                    return ((int)NftRolePriority.guest);
                }
            case "vip-pass":
                {
                    return ((int)NftRolePriority.vip_pass);
                }
        }
        return ((int)NftRolePriority.free);
    }


    [System.Obsolete]
    IEnumerator XanaliaUserToken(string url, string Jsondata)
    {
        print(Jsondata);
        print(url);
        print("Token " + PlayerPrefs.GetString("LoginTokenxanalia"));
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        string _tokenis = "Bearer " + PlayerPrefs.GetString("LoginTokenxanalia");
        request.SetRequestHeader("Authorization", _tokenis);
        request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }
        print("List of MainNet NFT's is   = " + request.downloadHandler.text);

       
            nftlist = request.downloadHandler.text;
            File.WriteAllText((Application.persistentDataPath + "/NftData.txt"), request.downloadHandler.text);
       
     

        //StartCoroutine(NftGet(10f));
      
    }
    [System.Obsolete]
    IEnumerator XanaliaNonCryptoNFTRole(string url)
    {
        //  print(Jsondata);
        print(url);
        print("Token " + PlayerPrefs.GetString("LoginTokenxanalia"));
        var request = new UnityWebRequest(url, "GET");
        //   byte[] bodyRaw = Encoding.UTF8.GetBytes("");
        //    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        string _tokenis = "Bearer " + PlayerPrefs.GetString("LoginTokenxanalia");
        request.SetRequestHeader("Authorization", _tokenis);
        request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }

        Debug.LogError("nft response :- "+ url + request.downloadHandler.text);

        ConnectServerDataExtraction.RootNonCryptoNFTRole myObject = new ConnectServerDataExtraction.RootNonCryptoNFTRole();
        myObject = ConnectServerDataExtraction.RootNonCryptoNFTRole.CreateFromJSON(request.downloadHandler.text);


        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                if (myObject.success)
                {
                   // print("NFTrole For non Crypto user is " + myObject.data.userNftRole[1]);

                    int x=(int)NftRolePriority.guest;
                    string userNftRole= "free";
                    ConstantsGod.UserRoles = myObject.data.userNftRoleArr.ToList();
                    foreach (string s in myObject.data.userNftRoleArr)
                    {
                        Debug.LogError("---- "+s+ "----"+ReturnNftRole(s));
                        int rolePriority = ReturnNftRole(s);
                        if (rolePriority <= x)
                        {
                            x = rolePriority;
                            ConstantsGod.UserPriorityRole = s;
                        }
                        
                        userNftRole = s.ToLower();
                        //myObject.data.userNftRole[0] = myObject.data.userNftRole[0].ToLower();

                        switch (userNftRole)
                        {
                            case "alpha-pass":
                                {
                                    PremiumUsersDetails.Instance.GetGroupDetails("Access Pass");
                                    break;
                                }
                            case "premium":
                                {
                                    PremiumUsersDetails.Instance.GetGroupDetails("Extra NFT");
                                    break;
                                }
                            case "dj-event":
                                {
                                    PremiumUsersDetails.Instance.GetGroupDetails("djevent");
                                    break;
                                }
                            case "free":
                                {
                                    PremiumUsersDetails.Instance.GetGroupDetails("freeuser");
                                    break;
                                }
                            case "vip-pass":
                                {
                                    PremiumUsersDetails.Instance.GetGroupDetails("vip-pass");
                                    break;
                                }
                        }
                    }

                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                Debug.LogError("Network error in set device token");
            }
            else
            {
                if (request.error != null)
                {
                    //if (myObject1.success == "false")
                    if (!myObject.success)
                    {
                        Debug.LogError("Success false in  in set device token");
                    }
                }
            }
        }
    }


    #region Riken....... For Get UserPriorityRole For Other User
    public string GetOtherUserPriorityRole(List<string> userRoleList)
    {
        int x = (int)NftRolePriority.guest;
        string otherUserNftRole = "free";
        foreach (string s in userRoleList)
        {
            int rolePriority = ReturnNftRole(s);
            if (rolePriority <= x)
            {
                x = rolePriority;
                otherUserNftRole = s;
            }
        }
        return otherUserNftRole;
    }
    #endregion

  

    // Get Data for presets Only
    IEnumerator LoginUserPresetOnly()
    {


        // MyClassOfLoginJson LoginObj = new MyClassOfLoginJson();
        // LoginObj.email = "presetallData1@yopmail.com";        //"allpresetdata@yopmail.com";   // get Preset jsons from this account to show 
        // LoginObj.password = "123";

        // string jsondata = JsonUtility.ToJson(LoginObj);

        // var request = new UnityWebRequest(LoginAPIURL, "POST");
        // byte[] bodyRaw = Encoding.UTF8.GetBytes(jsondata);
        // request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        // request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        // yield return request.SendWebRequest();
        //// print("json data is " + request.downloadHandler.text);
        // ClassWithToken myObject1 = new ClassWithToken();
        // myObject1 = ClassWithToken.CreateFromJSON(request.downloadHandler.text);
        // if (!request.isHttpError && !request.isNetworkError)
        // {
        //     if (request.error == null)
        //     {
        //         Debug.Log(request.downloadHandler.text);
        //         if (myObject1.success)
        //         {

        //             print("Token is --- " + myObject1.data.token);

        //            PlayerPrefs.SetString("LoginToken_Preset", myObject1.data.token);
        //            ServerSIdeCharacterHandling.Instance.getPresetDataFromServer();
        //         }
        //     }
        // }
        // else
        // {
        //     Debug.LogError("NetWOrkerror DO Somethin");
        ////     print(myObject1.msg + " | success: " + myObject1.success);
        // }
        yield return null;


    }
    //End



    void getdatafromserver()
    {
        ServerSIdeCharacterHandling.Instance.GetDataFromServer();
    }
    public void LoginWithWallet()
    {
        PlayerPrefs.SetInt("IsLoggedIn", 1);
        OpenUIPanal(7);
        SubmitSetDeviceToken();
        LoggedInAsGuest = false;
        getdatafromserver();
        usernamePanal.SetActive(false);
        PlayerPrefs.Save();
        if (UIManager.Instance != null)//rik
        {
            UIManager.Instance._footerCan.transform.GetChild(0).GetComponent<BottomTabManager>().HomeSceneFooterSNSButtonIntrectableTrueFalse();
        }
    }

    [Serializable]
    public class MyClass
    {
        public MyClass myObject;
        public string success;
        public string data;
        public MyClass Load(string savedData)
        {
            myObject = new MyClass();

            myObject = JsonUtility.FromJson<MyClass>(savedData);
            return myObject;
        }
    }

    [Serializable]
    public class LoginClass : JsonObjectBase
    {
        //public LoginClass LoginObject;
        public string id;
        public string iat;
        public int exp;
        public LoginClass Load(string savedData)
        {
            LoginClass LoginObject = new LoginClass();
            LoginObject = JsonUtility.FromJson<LoginClass>(savedData);
            return LoginObject;
        }
    }

    [Serializable]
    public class MyClassNewApi
    {
        public MyClassNewApi myObject;
        public bool success;
        public string msg;
        public string data;
        public MyClassNewApi Load(string savedData)
        {
            myObject = new MyClassNewApi();
            print("savedData " + savedData);
            myObject = JsonUtility.FromJson<MyClassNewApi>(savedData);
            return myObject;
        }
    }

    //[Serializable]
    //public class OtpPhoneData
    //{
    //    public OtpPhoneData myObject;
    //    public bool success;
    //    public string msg;
    //    public OtpData data;
    //    public OtpPhoneData Load(string savedData)
    //    {
    //        myObject = new OtpPhoneData();
    //        print("savedData " + savedData);
    //        myObject.data = new OtpData();
    //        myObject = JsonUtility.FromJson<OtpPhoneData>(savedData);
    //        return myObject;
    //    }
    //}

    //[Serializable]
    //public class OtpData
    //{
    //    public int status = 0;
    //    public int code = 0;
    //    public string moreInfo = "";
    //}

    [Serializable]
    public class MyClassNewApiForStatusCode
    {
        public string statusCode;
        public string error;
        public string message;
        public MyClassNewApiForStatusCode Load(string jsonString)
        {
            print("savedData " + jsonString);
            return JsonUtility.FromJson<MyClassNewApiForStatusCode>(jsonString);
            //myObject = JsonUtility.FromJson<MyClassNewApi>(savedData);
            //return myObject;
        }
    }

    [System.Serializable]
    public class UserLoginData
    {
        public bool success = false;


    }


    [System.Serializable]
    public class ClassWithToken
    {
        public bool success;
        public JustToken data;
        public string msg;
        public static ClassWithToken CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassWithToken>(jsonString);
        }
    }

    [System.Serializable]
    public class JustToken
    {
        public string token;
        public string encryptedId;
        public string xanaliaToken;
        public UserData user;
        public bool isAdmin;
        public static JustToken CreateFromJSON(string jsonString)
        {
            print("Person " + jsonString);
            return JsonUtility.FromJson<JustToken>(jsonString);
        }
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public string email;
        public string phoneNumber;
        public string coins;
    }

    [System.Serializable]
    public class JsonObjectBase
    {
    }

    [Serializable]
    public class MyClassOfPostingOTP : JsonObjectBase
    {
        public string email;
        public string otp;
        public MyClassOfPostingOTP GetdataFromClass(string email, string otp)
        {
            MyClassOfPostingOTP myObj = new MyClassOfPostingOTP();
            myObj.email = email;
            myObj.otp = otp;
            return myObj;
        }
    }

    [Serializable]
    public class MyClassOfLoginJson : JsonObjectBase
    {
        public string email;
        public string phoneNumber;
        public string password;
        public string deviceId;
        public MyClassOfLoginJson GetdataFromClass(string L_eml, string L_phonenbr, string passwrd, string _Deviceid = "")
        {
            MyClassOfLoginJson myObj = new MyClassOfLoginJson();
            myObj.email = L_eml;
            myObj.phoneNumber = L_phonenbr;
            myObj.password = passwrd;
            myObj.deviceId = _Deviceid;
            return myObj;
        }

        public MyClassOfLoginJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<MyClassOfLoginJson>(jsonString);
        }

    }



    [Serializable]
    public class MyClassOfPostingPhoneOTP : JsonObjectBase
    {
        public string phoneNumber;
        public string otp;
        public MyClassOfPostingPhoneOTP GetdataFromClass(string phonenumber, string otp)
        {
            MyClassOfPostingPhoneOTP myObj = new MyClassOfPostingPhoneOTP();
            myObj.phoneNumber = phonenumber;
            myObj.otp = otp;
            return myObj;
        }
    }

    [Serializable]
    public class MyClassOfPhoneNumber : JsonObjectBase
    {
        public string phoneNumber;
        public MyClassOfPhoneNumber GetdataFromClass(string Cellnumber)
        {
            MyClassOfPhoneNumber myObj = new MyClassOfPhoneNumber();
            myObj.phoneNumber = Cellnumber;
            return myObj;
        }
    }

    [Serializable]
    public class MyClassOfPostingName : JsonObjectBase
    {
        public string name;
        public MyClassOfPostingName GetNamedata(string name)
        {
            MyClassOfPostingName myObj = new MyClassOfPostingName();
            myObj.name = name;
            return myObj;
        }
    }

    [Serializable]
    public class MyClassForChangePassword : JsonObjectBase
    {
        public string oldPassword;
        public string newPassword;

        public MyClassForChangePassword GetChangePassworddata(string oldPass, string NewPass)
        {
            MyClassForChangePassword myObj = new MyClassForChangePassword();
            myObj.oldPassword = oldPass;
            myObj.newPassword = NewPass;
            return myObj;
        }
    }

    LoginClass CheckResponceJsonOfLogin(string Localdata)
    {
        LoginClass myObject = new LoginClass();
        myObject = myObject.Load(Localdata);
        print("user name in class" + (myObject.id));
        return myObject;
    }

    MyClass CheckResponceJson(string Localdata)
    {
        MyClass myObject = new MyClass();
        myObject = myObject.Load(Localdata);
        return myObject;
    }

    MyClassNewApi CheckResponceJsonNewApi(string Localdata)
    {
        MyClassNewApi myObject = new MyClassNewApi();
        myObject = myObject.Load(Localdata);
        print("myObject " + myObject.data);
        return myObject;
    }

    //OtpPhoneData CheckResponceJsonOtpPhoneApi(string Localdata)
    //{
    //    OtpPhoneData myObject = new OtpPhoneData();
    //    myObject = myObject.Load(Localdata);
    //    print("myObject " + myObject.data.status);
    //    return myObject;
    //}

    IEnumerator WaitUntilAnimationFinished(Animator MyAnim)
    {

        yield return new WaitForSeconds(3f);
        MyAnim.SetBool("playAnim", false);
    }
}

enum NftRolePriority
{
    alpha_pass,
    dj_event,
    vip_pass,
    premium,
    free,
    guest
}

