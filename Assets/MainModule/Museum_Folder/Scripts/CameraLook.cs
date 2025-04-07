using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;
using Metaverse;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CameraLook : MonoBehaviour
{
    public float lookSpeed;
    public float lookSpeedd;
    [SerializeField]
    private CinemachineFreeLook cinemachine;
    public Camera freeLookCharacter;
    private Controls controls;
    private CharacterController character;
    public RectTransform freelookup;
    public bool gyroCheck = false;
    public static CameraLook instance;
    [SerializeField]
    private float MinZoom = 30f;
    [SerializeField]
    private float MaxZoom = 90f;
    [SerializeField]
    private float ZoomSpeed = 0.05f;
    bool ZoomStarted = false;
    bool firstTimeDone = false;
    private Vector2[] lastZoomPositions;
    [HideInInspector]
    public int m_PressCounter;
    //** Temp Variables
    float m_TempDistance;
    float m_Offset;
    private Vector2 delta;
    public PlayerControllerNew playerController;
    private bool _allowSyncedControl;
    private bool _allowRotation = true;
    public Slider sensitivitySlider;
    public PointerEventData m_pointereventdata;
    public GraphicRaycaster m_Raycaster_MsgUI;
    public GraphicRaycaster m_Raycaster_HelpUI;
    public GraphicRaycaster m_Raycaster_GameplayUI;
    public GraphicRaycaster m_Raycaster_PremiumUI, m_Raycaster_ConectionFailedUI;

    public EventSystem m_EventSystem;

    public Transform cameraPos;

    static float pinchDistanceDelta;
    static float pinchDistance;
    const float pinchRatio = 1;
    const float minPinchDistance = 1.5f;

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        controls = new Controls();
        cinemachine = this.GetComponent<CinemachineFreeLook>();
        cinemachine.m_Orbits[2].m_Height = 0.08f;
        if (gyroCheck)
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
    }
    private void Start()
    {
#if UNITY_EDITOR
        lookSpeed = 0.01f;


#endif
        lookSpeedd = 0.72f;
        playerController = AvatarManager.Instance.spawnPoint.GetComponent<PlayerControllerNew>();
        controls.Gameplay.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Gameplay.SecondaryTouchContact.canceled += _ => ZoomEnd();
        cinemachine.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetOnAssign;
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if (GameObject.FindGameObjectWithTag("Msgbox"))
        {
            m_Raycaster_MsgUI = GameObject.FindGameObjectWithTag("Msgbox").GetComponent<GraphicRaycaster>();
        }
        if(GameObject.Find("Museum_ Canvas working New"))
            m_Raycaster_HelpUI = GameObject.Find("Museum_ Canvas working New").GetComponent<GraphicRaycaster>();
        m_Raycaster_PremiumUI = GameObject.Find("premium user canvas").GetComponent<GraphicRaycaster>();
        m_Raycaster_GameplayUI = GameObject.FindGameObjectWithTag("NewCanvas").GetComponent<GraphicRaycaster>();

    }


    IEnumerator Fade()
    {
        if (XanaConstants.xanaConstants.ConnectionPopUpPanel)
        {
            m_Raycaster_ConectionFailedUI = XanaConstants.xanaConstants.ConnectionPopUpPanel.GetComponent<GraphicRaycaster>();
            yield return new WaitForSeconds(.01f);
        }
    }
    public void CameraSensitivity(float camSensitivity)
    {
        lookSpeedd = camSensitivity;
    }
    public void ApplySensitivity()
    {
        CameraSensitivity(sensitivitySlider.value);
    }

    public void AllowControl()
    {
        _allowRotation = true;
        controls.Enable();
    }
    public void DisAllowControl()
    {
        controls.Disable();
        _allowRotation = false;
        _allowSyncedControl = false;
    }
    private void Update()
    {
        StartCoroutine(Fade());
        _allowRotation = true;

        //StartCoroutine(Fade());
        XanaMsgUi();
        XanaUiPanel();
        XanaGameplayPanel();
        XanaPremiumPanel();
        ConnectionFailedPanel();

        if (IsPointerOverUIObject())
        {
            _allowRotation = false;
        }


        if (playerController == null)
        {
            return;
        }
#if UNITY_EDITOR
        if (_allowRotation)
        {
            CameraControls_Editor();
        }
#endif
#if UNITY_IOS || UNITY_ANDROID
        if (_allowRotation)
        {
            CameraControls_Mobile();
            ZoomDetection();
        }
#endif
    }
    void CameraControls_Editor()
    {
        gyroCheck = CanvusHandler.canvusHandlerInstance.isGyro;
        if (!gyroCheck)
        {
            delta = controls.Gameplay.Look.ReadValue<Vector2>();
            cinemachine.m_XAxis.Value += delta.x * 400 * lookSpeed * Time.deltaTime;
            cinemachine.m_YAxis.Value += delta.y * 5 * lookSpeed * Time.deltaTime;
        }
    }
    void Longtouch()
    {
        //if (!EmoteFilterManager.TouchDisable)
        //{
        gyroCheck = CanvusHandler.canvusHandlerInstance.isGyro;
        //if(gyroCheck)
        delta = Vector2.zero;
        if (!gyroCheck && SelfieController.Instance.disablecamera && Input.touchCount > 0 && !playerController.sprint)
        {
            //
            if (playerController.horizontal != 0 && playerController.vertical != 0)//&& Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(1);
                if (t.phase == TouchPhase.Moved)// && (playerController.horizontal == 0 && playerController.vertical == 0))
                {
                    delta = Input.GetTouch(1).deltaPosition;
                    _allowSyncedControl = true;
                }
                else
                {
                    _allowSyncedControl = false;
                }
            }
            else if (playerController.horizontal == 0 && playerController.vertical == 0) // && Input.touchCount > 0)
            {
                //  float lookspedd_log = 0.2f;
                if (Input.touchCount > 0)
                {
                    Touch t = Input.GetTouch(0);
                    if (t.phase == TouchPhase.Moved) // && (playerController.horizontal == 0 && playerController.vertical == 0))
                    {
                        delta = Input.GetTouch(0).deltaPosition;
                        _allowSyncedControl = true;
                    }
                    else
                    {
                        _allowSyncedControl = false;
                    }
                }
            }
        }
        // }

    }
    private void FixedUpdate()
    {
        if (_allowSyncedControl && _allowRotation)
        {
            MoveCamera(delta);
        }
    }
    private void MoveCamera(Vector2 delta)
    {
        cinemachine.m_XAxis.Value += delta.x * 10 * lookSpeedd * Time.deltaTime;
        cinemachine.m_YAxis.Value += -delta.y * 0.08f * lookSpeedd * Time.deltaTime;
    }
    void CameraControls_Mobile()
    {
        Longtouch();
    }
    private void ZoomStart()
    {
        ZoomStarted = true;
    }
    private void ZoomEnd()
    {
        ZoomStarted = false;
    }
    void ZoomDetection()
    {
        if (!CheckCanZoom())
            return;
        if (m_PressCounter != 0) return;
        if (Input.touchCount == 2)
        {
            pinchDistance = pinchDistanceDelta = 0;

            Touch l_TouchOne = Input.GetTouch(0);
            Touch l_TouchTwo = Input.GetTouch(1);
            if (l_TouchOne.phase == TouchPhase.Moved && l_TouchTwo.phase == TouchPhase.Moved)
            {
                float l_Distance = Vector3.Distance(l_TouchOne.position, l_TouchTwo.position);
                float l_SpeedOne = (l_TouchOne.deltaPosition / l_TouchOne.deltaTime).magnitude;
                float l_SpeedTwo = (l_TouchTwo.deltaPosition / l_TouchTwo.deltaTime).magnitude;
                pinchDistance = Vector2.Distance(l_TouchOne.position, l_TouchTwo.position); // getting distance between two fingures. 
                float prevDistance = Vector2.Distance(l_TouchOne.position - l_TouchOne.deltaPosition, l_TouchTwo.position - l_TouchTwo.deltaPosition); // previuos distance between fingures.
                pinchDistanceDelta = pinchDistance - prevDistance;

                if (Mathf.Abs(pinchDistanceDelta) > minPinchDistance)//if it's greater than a minimum threshold, it's a pinch!

                {
                    pinchDistanceDelta *= pinchRatio;
                    if (l_SpeedOne > 100 && l_SpeedTwo > 100) // old value is 90 
                    {
                        if (l_Distance > m_TempDistance)
                        {
                            float l_FieldOfView = cinemachine.m_Lens.FieldOfView;
                            l_FieldOfView -= pinchRatio;
                            cinemachine.m_Lens.FieldOfView = Mathf.Clamp(l_FieldOfView, MinZoom, MaxZoom);
                            freeLookCharacter.fieldOfView = Mathf.Clamp(l_FieldOfView, MinZoom, MaxZoom);
                        }
                        else if (l_Distance < m_TempDistance)
                        {
                            float l_FieldOfView = cinemachine.m_Lens.FieldOfView;
                            l_FieldOfView += pinchRatio;
                            cinemachine.m_Lens.FieldOfView = Mathf.Clamp(l_FieldOfView, MinZoom, MaxZoom);
                            freeLookCharacter.fieldOfView = Mathf.Clamp(l_FieldOfView, MinZoom, MaxZoom);
                        }
                    }
                }
                else
                {
                    pinchDistance = pinchDistanceDelta = 0;
                }


                m_TempDistance = l_Distance;
            }
        }
    }
    bool CheckCanZoom()
    {
        if (playerController.horizontal != 0 && playerController.vertical != 0)
            return false;
        if (playerController.jumpNow)
            return false;
        return true;
    }
    void ZoomCamera(float offset)
    {
        cinemachine.m_Lens.FieldOfView = Mathf.Clamp(cinemachine.m_Lens.FieldOfView - offset, MinZoom, MaxZoom);
    }
    private bool CamLookAround()
    {
        //#if UNITY_EDITOR
        //        Vector2 localMousePos = freelookup.InverseTransformPoint(Input.mousePosition);
        //#else
        //        Vector2 localMousePos = freelookup.InverseTransformPoint(Input.GetTouch(0).position);
        //#endif
        //        //if (freelookup.rect.Contains(localMousePos) && freelookup.GetComponent<UnityEngine.InputSystem.OnScreen.OnScreenStick>().canControl)
        //        {
        //            return true;
        //        }
        return false;
    }


    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("NFTDisplayPanel") || results[i].gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                return true;
            }
        }

#if !UNITY_EDITOR
        for (int i = 0; i < Input.touchCount; i++)
        {
            eventDataCurrentPosition.position = Input.GetTouch(i).position;

            results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results[i].gameObject.layer == LayerMask.NameToLayer("NFTDisplayPanel"))
            {
                return true;
            }
        }
#endif

        return false;
    }
    public void XanaMsgUi()
    {
        if (m_Raycaster_MsgUI != null)
        {
            m_pointereventdata = new PointerEventData(m_EventSystem);
            m_pointereventdata.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster_MsgUI.Raycast(m_pointereventdata, results);
            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    // print(results.Count);
                    _allowRotation = false;
                }
            }
        }
    }
    public void XanaUiPanel()
    {
        if (m_Raycaster_HelpUI != null)
        {
            m_pointereventdata = new PointerEventData(m_EventSystem);
            m_pointereventdata.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            m_Raycaster_HelpUI.Raycast(m_pointereventdata, result);
            if (result.Count > 0)
            {
                if (result[0].gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    // print(results.Count);
                    _allowRotation = false;
                }
            }
        }
    }
    public void XanaGameplayPanel()
    {
        if (m_Raycaster_GameplayUI != null)
        {
            m_pointereventdata = new PointerEventData(m_EventSystem);
            m_pointereventdata.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster_GameplayUI.Raycast(m_pointereventdata, results);
            if (results.Count > 2)
            {
                if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    // print(results.Count);
                    _allowRotation = false;
                }
            }
        }
    }
    public void XanaPremiumPanel()
    {
        if (m_Raycaster_PremiumUI != null)
        {
            m_pointereventdata = new PointerEventData(m_EventSystem);
            m_pointereventdata.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster_PremiumUI.Raycast(m_pointereventdata, results);
            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    // print(results.Count);
                    _allowRotation = false;
                }
            }
        }
    }
    public void ConnectionFailedPanel()
    {
        if (m_Raycaster_ConectionFailedUI != null)
        {
            m_pointereventdata = new PointerEventData(m_EventSystem);
            m_pointereventdata.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster_ConectionFailedUI.Raycast(m_pointereventdata, results);
            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    //  print("Clicks are:"+results.Count);
                    _allowRotation = false;
                }
            }
        }
    }

    #region Waste Code
    //if(Gamemanager._InstanceGM._IsStandalone)
    //{
    //  Vector2 delta = controls.Gameplay.Look.ReadValue<Vector2>();
    //  cinemachine.m_YAxis.Value += delta.y * lookSpeed * Time.deltaTime;
    //}
    //else
    //if (Gamemanager._InstanceGM._IsAndroid)
    //{
    //  if (gyroCheck && !UnityEngine.InputSystem.Gyroscope.current.enabled)
    //  {
    //    InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
    //  }
    //  if (gyroCheck)
    //  {
    //    Vector3 gyro = controls.Gameplay.gyro.ReadValue<Vector3>();
    //    cinemachine.m_XAxis.Value += gyro.x * 200 * lookSpeed * Time.deltaTime;
    //    cinemachine.m_YAxis.Value += gyro.y * lookSpeed * Time.deltaTime;
    //  }
    //if (ZoomStarted)
    //{
    //  ZoomDetection();
    //}
    //if (Input.touchCount != 2)
    //{
    //  firstTimeDone = false;
    //}
    //}
    #endregion
}