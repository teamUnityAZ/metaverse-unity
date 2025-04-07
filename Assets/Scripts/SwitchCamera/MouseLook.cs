using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    #region PUBLIC_VAR
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    public PlayerControllerNew playerController;

    [Header("Gyro")]
    private float x;
    private float y;
    public GameObject onToggal;
    public GameObject offToggal;
 //   public Toggle gyroToggle;
    [SerializeField] private bool isGyroOn = false;
    [SerializeField] private bool gyroEnabled;
    readonly float sensitivity = 50.0f;
    private Gyroscope gyro;
    #endregion

    #region PRIVATE_VAR
    // Mobile Device
    private Vector2 delta;
    private bool _allowSyncedControl;
    private bool _allowRotation = true;
    #endregion

    #region UNITY_METHOD
    #endregion

    #region PUBLIC_METHODS
    // Start is called before the first frame update
    void Start()
    {
        //  gyroToggle.isOn = false;
        onToggal.SetActive(false);
          gyroEnabled = EnableGyro();
        playerBody = transform.GetComponentInParent<PlayerControllerNew>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.isFirstPerson)
            return;

        if (CameraLook.IsPointerOverUIObject())
        {
            _allowRotation = false;
            return;
        }
        _allowRotation = true;


#if UNITY_EDITOR
        if (!isGyroOn)
        {
            float mouseX;
            float mouseY;
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 55f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * (mouseX));
        }

#elif UNITY_IOS || UNITY_ANDROID
        if (_allowRotation)
        {
            Longtouch();
        }
 
#endif

        // Gyro Rotation
        if (isGyroOn)
        {
            if (gyroEnabled)
            {
                GyroRotation();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isGyroOn)
            return;

        if (_allowSyncedControl && _allowRotation)
        {
            MoveCamera(delta);
        }
    }


    // Mobile Touch 
    void Longtouch()
    {
        delta = Vector2.zero;

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

    private void MoveCamera(Vector2 delta)
    {
        xRotation -= delta.y * 10* CameraLook.instance.lookSpeedd * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 55f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * (delta.x * 10 * CameraLook.instance.lookSpeedd * Time.deltaTime));
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region Gyro
    public void OnToggle()
    {
        offToggal.SetActive(true);
        onToggal.SetActive(false);
        isGyroOn = false;
       
    }

    public void offToggel()
    {
        offToggal.SetActive(false);
        onToggal.SetActive(true);
        isGyroOn = true;
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    void GyroRotation()
    {
        x = Input.gyro.rotationRate.x;
        y = Input.gyro.rotationRate.y;

        float xFiltered = FilterGyroValues(x);
        RotateUpDown(xFiltered * sensitivity);

        float yFiltered = FilterGyroValues(y);
        RotateRightLeft(yFiltered * sensitivity);
    }

    float FilterGyroValues(float axis)
    {
        if (axis < -0.1 || axis > 0.1)
        {
            return axis;
        }
        else
        {
            return 0;
        }
    }

    public void RotateUpDown(float axis)
    {
        transform.RotateAround(transform.position, transform.right, -axis * Time.deltaTime);
    }

    //rotate the camera rigt and left (y rotation)
    public void RotateRightLeft(float axis)
    {
        playerBody.RotateAround(transform.position, Vector3.up, -axis * Time.deltaTime);
    }
    #endregion

    #region COROUTINE
    #endregion

    #region DATA
    #endregion
}
