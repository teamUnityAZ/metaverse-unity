﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using Metaverse;

public class SelfieController : MonoBehaviour
{
    public static SelfieController Instance;

    [HideInInspector]
    public GameObject m_SelfieStick;


    [Header("Selfie Panel")]
    public GameObject m_captureImage2;

    [HideInInspector]
    public GameObject m_IKObject;

    [HideInInspector]
    public GameObject m_IKComponenet;

    //[HideInInspector]
    public GameObject m_CharacterParent;

    [Header("Player Controller")]
    public GameObject m_PlayerController;

    [Header("Movement Speed")]
    public float m_Speed;

    [Header("Object To Close")]
    public GameObject[] m_ObjectsToClose;

    [Header("Captured Image")]
    public RawImage m_CapturedImage, m_CapturedImage2;

    [Header("Disable camera movement")]
    public bool disablecamera;

    public GameObject[] OnFeatures;
    public GameObject[] OffFeatures;
    public GameObject selfiePanel;

    [HideInInspector]
    public bool isReconnecting;

    public void SwitchFromSelfieControl()
    {

        SelfieController.Instance.DisableSelfieFeature();
        for (int i = 0; i < OnFeatures.Length; i++)
        {
            if (OnFeatures[i] != null)
                OnFeatures[i].SetActive(true);
            LoadFromFile.instance.EnableObjects();
            if (GamePlayButtonEvents.inst != null)
            {
                GamePlayButtonEvents.inst.SelfiePanleUpdateObject(true);
            }
        }
        for (int i = 0; i < OffFeatures.Length; i++)
        {
            if (OffFeatures[i] != null)
                OffFeatures[i].SetActive(false);
        }
    }


    [Header("Blink Panel Animation")]  // this is used, to change the panel from face to body customization ui panel
    public GameObject m_BlinkAnimationPanel;
    public AnimationCurve m_AnimCurve;
    public AnimationCurve m_AnimCurve1;
    public float m_AnimTime;

    [Header("Blink Colors")]
    public Color m_ColorOne;
    public Color m_ColorTwo;


    public RenderTexture m_RenderTexture;
    Texture2D m_Texture2D;


    public bool m_IsSelfieFeatureActive, t_nftMuseums;
    int layerMaskArrow = 1 << 17;

    public Text ShowToastMessage;
    public float horizontal, vertical;

    public float m_test;

    public float m_Ymin = 0;
    public float m_YMax = 0;

    public float avatarMin = 0;
    public float avatarMax = 0;


    public int m_SefieIndex;

    bool allowTouch = true;

    private void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnSelfieButton += EnbaleSelfieFeature;
    }

    public void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnSelfieButton -= EnbaleSelfieFeature;
    }

    void Start()
    {
        disablecamera = true;
        t_nftMuseums = false;
        if (!gameObject.GetComponent<SelfieTouchDetector>())
        {
            gameObject.AddComponent<SelfieTouchDetector>();
        }
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 1)
        {
            allowTouch = false;
        }
        else
        {
            allowTouch = true;
        }
    }
    void Update()
    {
      
        if (m_IsSelfieFeatureActive && allowTouch)
        {
#if UNITY_EDITOR

            SelfieControls();
#endif



#if UNITY_IOS

            if (Input.touchCount <= 1)
                        {
                            Touch l_Touch = Input.GetTouch(0);

                            if (l_Touch.phase == TouchPhase.Moved)
                            {
                    SelfieControls();
                    //            // Main Character Rotation
                    //            m_CharacterParent.transform.rotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Mouse X") * m_Speed);

                    //            // Avatar Clamped Hand Movement In Y-Axis Only
                    //            m_IKObject.transform.localPosition += Vector3.up * Input.GetAxis("Mouse Y") * 0.01f;
                    //            float l_Y = Mathf.Clamp(m_IKObject.transform.localPosition.y, 0.75f, 1.25f);
                    //m_IKObject.transform.localPosition = new Vector3(m_IKObject.transform.localPosition.x, l_Y, m_IKObject.transform.localPosition.z);

                    //            // Avatar's IK Clamped Rotation In Y-Axis Only
                    //            m_IKObject.transform.localRotation *= Quaternion.Euler(Vector3.right * -Input.GetAxis("Mouse Y") * m_Speed);
                    //            float l_XRot = Mathf.Clamp(m_IKObject.transform.localRotation.x, -0.23f, 0.08f);
                    //            m_IKObject.transform.localRotation = Quaternion.Euler(new Vector3(l_XRot * (360 / 3.14f), 0, 0));
                            }
                        }

#endif
#if UNITY_ANDROID
            if (Input.touchCount != 0 && Input.touchCount <= 1) // to check single touch
            {

                Touch l_Touch = Input.GetTouch(0);

                if (l_Touch.phase == TouchPhase.Moved )
                {
                    SelfieControls();

                    //// Main Character Rotation
                    //m_CharacterParent.transform.rotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Mouse X") * m_Speed);

                    //// Avatar Clamped Hand Movement In Y-Axis Only
                    //m_IKObject.transform.localPosition += Vector3.up * Input.GetAxis("Mouse Y") * 0.01f;
                    //float l_Y = Mathf.Clamp(m_IKObject.transform.localPosition.y, 0.75f, 1.25f);
                    //m_IKObject.transform.localPosition = new Vector3(m_IKObject.transform.localPosition.x, l_Y, m_IKObject.transform.localPosition.z);

                    //// Avatar's IK Clamped Rotation In Y-Axis Only
                    //m_IKObject.transform.localRotation *= Quaternion.Euler(Vector3.right * -Input.GetAxis("Mouse Y") * m_Speed);
                    //float l_XRot = Mathf.Clamp(m_IKObject.transform.localRotation.x, -0.23f, 0.08f);
                    //m_IKObject.transform.localRotation = Quaternion.Euler(new Vector3(l_XRot * (360 / 3.14f), 0, 0));
                }
            }


#endif

        }



    }

    public bool allowRotate = false;
    void SelfieControls()
    {
        if (allowRotate)
        {
            float xInput = Input.GetAxis("Mouse X");
            float yInput = Input.GetAxis("Mouse Y");
            // Main Character Rotation
            if (XanaConstants.xanaConstants.SelfiMovement)
                m_CharacterParent.transform.rotation *= Quaternion.Euler(Vector3.up * xInput * m_Speed);

            if (m_IKObject == null)
                return;
            // Avatar Clamped Hand Movement In Y-Axis Only

            if (XanaConstants.xanaConstants.SelfiMovement)
                m_IKObject.transform.localPosition += Vector3.up * yInput * (Time.deltaTime * 2); //0.01f

            float l_Y = Mathf.Clamp(m_IKObject.transform.localPosition.y, m_Ymin, m_YMax);
            m_IKObject.transform.localPosition = new Vector3(m_IKObject.transform.localPosition.x, l_Y, m_IKObject.transform.localPosition.z);

            // Avatar's IK Clamped Rotation In Y-Axis Only
            m_IKObject.transform.localRotation *= Quaternion.Euler(Vector3.right * -yInput * m_Speed);
            float l_XRot = Mathf.Clamp(m_IKObject.transform.localRotation.x, avatarMin, avatarMax);
            m_IKObject.transform.localRotation = Quaternion.Euler(new Vector3(l_XRot * (360 / 3.14f), 0, 0));
        }
    }


    public void InitializeCharacter(GameObject l_SelfieStick, GameObject l_Parent, GameObject l_IkTarget, GameObject l_IKReference)
    {
        m_SelfieStick = l_SelfieStick;
        m_CharacterParent = l_Parent;
        m_IKObject = l_IkTarget;
        m_IKComponenet = l_IKReference;

        //UpdateSelfieFOV();

        int l_PlayerID = 0;

        if (m_SefieIndex == 1)
        {
            m_Ymin = 0.0f;
            m_YMax = 0.27f;
        }
        else
        {
            //m_Ymin = 0.6f;
            //m_YMax = 1.6f;
            m_Ymin = 1.2f;
            m_YMax = 3f;
        }
    }

    public void SetIkValues(int l_Index)
    {
        if (l_Index == 0)
        {
            m_Ymin = 0.0f;
            m_YMax = 0.27f;
        }
        else
        {
            //m_Ymin = 0.6f;
            //m_YMax = 1.6f;
            m_Ymin = 1.2f;
            m_YMax = 3f;
        }
    }


    void UpdateSelfieFOV()
    {
        GameObject cam = m_SelfieStick.transform.GetChild(0).GetChild(0).gameObject;
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 1.5f, 18);
        cam.GetComponent<Camera>().fieldOfView = 50;
        cam.GetComponent<Camera>().cullingMask = ~layerMaskArrow;
        cam.transform.GetChild(0).GetComponent<Camera>().cullingMask = ~layerMaskArrow;
    }




    public void EnbaleSelfieFeature()
    {
        if (EmoteAnimationPlay.Instance)
            EmoteAnimationPlay.Instance.clearAnimation?.Invoke();
      
        StartCoroutine(EnableSelfieWithDelay());
       
    }

    IEnumerator EnableSelfieWithDelay()
    {
     
        yield return new WaitForSeconds(.3f);
        m_PlayerController.GetComponent<PlayerControllerNew>().SwitchToSelfieMode();
#if UNITY_EDITOR
        m_IsSelfieFeatureActive = true;
        m_IKComponenet.GetComponent<IKMuseum>().EnableIK();

        ChangeCloseObjectsState(false);

        m_SelfieStick.SetActive(true);
        m_PlayerController.GetComponent<PlayerControllerNew>().m_IsMovementActive = false;

        for (int i = 0; i < OnFeatures.Length; i++)
        {
            if (OnFeatures[i] != null)
                OnFeatures[i].SetActive(false);

            if (GamePlayButtonEvents.inst != null)
            {
                GamePlayButtonEvents.inst.SelfiePanleUpdateObject(false);
            }
        }
        m_CapturedImage.gameObject.SetActive(false);
        m_CapturedImage.texture = null;
        selfiePanel.SetActive(true);
        t_nftMuseums = true;
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount <= 1)
        {
            m_IsSelfieFeatureActive = true;
            m_IKComponenet.GetComponent<IKMuseum>().EnableIK();

            ChangeCloseObjectsState(false);

            m_SelfieStick.SetActive(true);
            m_PlayerController.GetComponent<PlayerControllerNew>().m_IsMovementActive = false;

            for (int i = 0; i < OnFeatures.Length; i++)
            {
            if (OnFeatures[i] != null)
                OnFeatures[i].SetActive(false);

            if (GamePlayButtonEvents.inst != null)
            {
                GamePlayButtonEvents.inst.SelfiePanleUpdateObject(false);
            }
            }
            m_CapturedImage.gameObject.SetActive(false);
            m_CapturedImage.texture = null;
            selfiePanel.SetActive(true);
            t_nftMuseums = true;
        }

#endif
    }

    IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.05f);
        m_PlayerController.GetComponent<PlayerControllerNew>().vertical = 0;

    }

    public void DisableSelfieFeature()
    {
       
        m_PlayerController.GetComponent<PlayerControllerNew>().gyroButton.SetActive(false);
       
#if UNITY_EDITOR

        m_IsSelfieFeatureActive = false;
        if(m_IKComponenet!= null && m_IKComponenet.GetComponent<IKMuseum>())
            m_IKComponenet.GetComponent<IKMuseum>().DisableIK();
        ChangeCloseObjectsState(true);
        if(m_SelfieStick !=null)
            m_SelfieStick.SetActive(false);
        t_nftMuseums = false;

        m_CapturedImage.gameObject.SetActive(false);
        m_CapturedImage.texture = null;
        m_PlayerController.GetComponent<PlayerControllerNew>().m_IsMovementActive = true;
        m_PlayerController.GetComponent<PlayerControllerNew>().vertical = -1;
        StartCoroutine(InputDelay());
        IsinAG = false;
        inSPRoom = false;
        disablecamera = true;

        for (int i = 0; i < OnFeatures.Length; i++)
        {
            if (OnFeatures[i] != null)
                OnFeatures[i].SetActive(true);

            if (GamePlayButtonEvents.inst != null)
            {
                GamePlayButtonEvents.inst.SelfiePanleUpdateObject(true);
            }
        }

        selfiePanel.SetActive(false);



        StartCoroutine(SetMuseumRaycasterBoolean());
        //if (MuseumRaycaster.instance != null)
        //    MuseumRaycaster.instance.SelfieClose();
        //else
        //    FindObjectOfType<MuseumRaycaster>().SelfieClose();



#elif UNITY_ANDROID || UNITY_IOS

       
        if (Input.touchCount <= 1 || isReconnecting)
        {
            isReconnecting=false;
            m_IsSelfieFeatureActive = false;
            m_IKComponenet.GetComponent<IKMuseum>().DisableIK();
            ChangeCloseObjectsState(true);
            m_SelfieStick.SetActive(false);
             selfiePanel.SetActive(false);
              t_nftMuseums = false;

            m_CapturedImage.gameObject.SetActive(false);
            m_CapturedImage.texture = null;
            m_PlayerController.GetComponent<PlayerControllerNew>().m_IsMovementActive = true;
            m_PlayerController.GetComponent<PlayerControllerNew>().vertical = -1;
            StartCoroutine(InputDelay());
            IsinAG = false;
            inSPRoom = false;
            disablecamera = true;

            for (int i = 0; i < OnFeatures.Length; i++)
            {
                if (OnFeatures[i] != null)
                    OnFeatures[i].SetActive(true);

                if (GamePlayButtonEvents.inst != null)
                {
                    GamePlayButtonEvents.inst.SelfiePanleUpdateObject(true);
                }
            }

           
            m_RenderTexture.Release();


            StartCoroutine(SetMuseumRaycasterBoolean());

        //    if (MuseumRaycaster.instance != null)
        //    MuseumRaycaster.instance.SelfieClose();
        //else
        //    FindObjectOfType<MuseumRaycaster>().SelfieClose();
        }
#endif
        if (m_PlayerController.GetComponent<PlayerControllerNew>().isFirstPerson)
        {
            m_PlayerController.GetComponent<PlayerControllerNew>().DisablePlayerOnFPS();
            m_PlayerController.GetComponent<PlayerControllerNew>().firstPersonCameraObj.SetActive(true);
        }
    }


    IEnumerator SetMuseumRaycasterBoolean()
    {
        yield return new WaitForSeconds(.5f);

        MuseumRaycaster.canOpenPicture = true;
    }


    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
    }

    public void ChangeCloseObjectsState(bool l_State)
    {
        for (int i = 0; i < m_ObjectsToClose.Length; i++)
        {
            m_ObjectsToClose[i].SetActive(l_State);
        }
    }

    public bool IsinAG, inSPRoom;
    public void TakeScreenShoot()
    {
        if (!PremiumUsersDetails.Instance.CheckSpecificItem("Selfie Button"))
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
            XanaConstants.xanaConstants.SelfiMovement = false;
            m_CapturedImage2.gameObject.SetActive(false);
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }
        m_captureImage2.SetActive(true);
        m_CapturedImage.gameObject.SetActive(false);
        m_CapturedImage.texture = null;
        m_IsSelfieFeatureActive = false;
        StartPanelBlinkAnimation();
        disablecamera = false;
    }


    public void TakeScreenShootAndSaveToGallary()
    {
        Texture2D l_Texture2d = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = m_RenderTexture;
        l_Texture2d.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0);
        l_Texture2d.Apply();

        m_CapturedImage.texture = l_Texture2d;

        m_CapturedImage.gameObject.SetActive(true);

        m_Texture2D = l_Texture2d;
        m_CapturedImage.texture = m_Texture2D;
    }

    public int picCount;

    public void SaveImageLocally()
    {
        byte[] l_Bytes = m_Texture2D.EncodeToPNG();

#if UNITY_EDITOR

        string path = Application.streamingAssetsPath + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png";
        File.WriteAllBytes(path, l_Bytes);

        if (l_Bytes != null)
        {
            if (GameManager.currentLanguage == "ja")
            {
                showToast(ShowToastMessage, "写真フォルダへ保存しました！", 2);
            }
            else
            {
                showToast(ShowToastMessage, "Image save successfully!", 2);
            }
        }

#endif



#if UNITY_ANDROID



        //if (!Directory.Exists(PlayerPrefs.GetString(ConstantsGod.ANDROIDPATH) + "/DCIM/XanaB"))
        //{
        //    Directory.CreateDirectory(PlayerPrefs.GetString(ConstantsGod.ANDROIDPATH) + "/DCIM/XanaB");
        //}


        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(l_Bytes, "/XanaB2", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png");
        //    string path1 = PlayerPrefs.GetString(ConstantsGod.ANDROIDPATH) + "/DCIM/Xana/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png";
        //    File.WriteAllBytes(path1, l_Bytes);

        if (l_Bytes != null)
        {
            if (GameManager.currentLanguage == "ja")
            {
                showToast(ShowToastMessage, "写真フォルダへ保存しました！", 2);
            }
            else
            {
                showToast(ShowToastMessage, "Image save successfully!", 2);
            }
        }

#endif

#if UNITY_IOS

        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(l_Bytes, "GalleryTest", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png");

        if (l_Bytes != null)
        {
            if (GameManager.currentLanguage == "ja")
            {
                showToast(ShowToastMessage, "写真フォルダへ保存しました！", 2);
            }
            else
            {
                showToast(ShowToastMessage, "Image save successfully!", 2);
            }
        }
#endif
    }

    public void StartPanelBlinkAnimation()
    {
        StartCoroutine(PanelBlinkAnimation(m_AnimTime));
    }

    IEnumerator PanelBlinkAnimation(float l_TimeLimit)
    {
        float l_t = 0;

        Color c_from = m_ColorOne;
        Color c_to = m_ColorTwo;

        float t1 = 0, t2 = 0;

        while (l_t <= l_TimeLimit)
        {
            l_t += Time.fixedDeltaTime;

            if (l_t <= l_TimeLimit / 2)
            {
                t1 += Time.fixedDeltaTime;
                m_BlinkAnimationPanel.GetComponent<Image>().color = Color.Lerp(c_from, c_to, m_AnimCurve.Evaluate(t1 / (l_TimeLimit / 2)));
            }
            else
            {
                t2 += Time.fixedDeltaTime;
                m_BlinkAnimationPanel.GetComponent<Image>().color = Color.Lerp(c_to, c_from, m_AnimCurve1.Evaluate(t2 / (l_TimeLimit / 2)));
            }

            yield return null;
        }

        TakeScreenShootAndSaveToGallary();
    }



    public void ActiveSelfieFeature()
    {
        if (IsinAG)
        {
            m_ObjectsToClose[1].SetActive(false);

        }
        if (inSPRoom)
        {
            m_ObjectsToClose[1].SetActive(false);

        }
        m_CapturedImage.gameObject.SetActive(false);
        m_IsSelfieFeatureActive = true;
    }
    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }
        Color currentColor = Color.white;
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);
            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }


    }
    public void showToast(Text txt, string text,
 int duration)
    {
        StartCoroutine(showToastCOR(txt, text, duration));
    }
    private IEnumerator showToastCOR(Text txt, string text,
   int duration)
    {
        Color orginalColor = txt.color;
        txt.text = text;
        txt.enabled = true;
        //Fade in
        yield return fadeInAndOut(txt, true, 0.5f);
        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        //Fade out
        yield return fadeInAndOut(txt, false, 0.5f);
        txt.enabled = false;
        txt.color = orginalColor;
    }

    public void DisableSelfieFromStart()
    {
        t_nftMuseums = false;

        m_PlayerController.GetComponent<PlayerControllerNew>().m_IsMovementActive = true;
        m_CapturedImage.gameObject.SetActive(false);
        m_captureImage2.gameObject.SetActive(false);
        m_CapturedImage.texture = null;
        IsinAG = false;
        inSPRoom = false;
        disablecamera = true;

        for (int i = 0; i < OnFeatures.Length; i++)
        {
            if (OnFeatures[i] != null)
                OnFeatures[i].SetActive(true);
        }

        selfiePanel.SetActive(false);



        if (MuseumRaycaster.instance != null)
        {
            MuseumRaycaster.instance.SelfieClose();
        }

        else
        {
            if (FindObjectOfType<MuseumRaycaster>())
                FindObjectOfType<MuseumRaycaster>().SelfieClose();
        }
        //Debug.Log("==>>>>>>>>>> " + MuseumRaycaster.instance.rayDistance);
    }
}
