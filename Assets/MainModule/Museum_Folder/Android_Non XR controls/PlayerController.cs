using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float lookSpeed = 1;

    [SerializeField]
    private CinemachineFreeLook cinemachine;

    public float moveSpeed = 5f;
    public float rotationSpeed = 280.0f;
    public bool is_Advanture;

    float horizontal;
    float vertical;

    float x_Look_Value;
    float y_Look_Value;

    //private void Awake()
    //{
    //    //cinemachine = gamGetComponent<CinemachineFreeLook>();
    //}



    public Transform cameraMain;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    public static bool _JumpBool;

    public Controls controls;
    private void Awake()
    {

        controls = new Controls();
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void Start()
    {
          // cameraMain.transform.cameraMain = Camera.main.transform;
          
    }

//    private void Update()
//    {
//        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;
//#if UNITY_EDITOR
//        Vector3 projectCameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
//        Quaternion rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);


//        //moveDirection = rotationToCamera * moveDirection;
//        //Quaternion rotationToMoveDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
//        //if (!is_Advanture)
//        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToCamera, rotationSpeed * Time.deltaTime);
//        //else
//        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);

////#elif UNITY_ANDROID
////            cinemachine.m_XAxis.Value += x_Look_Value * 200 *lookSpeed * Time.deltaTime;
////            cinemachine.m_XAxis.Value += y_Look_Value *  lookSpeed * Time.deltaTime;


////#endif
//       // transform.position += moveDirection * moveSpeed * Time.deltaTime;
//    }

    void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        Vector2 moveInput = controls.Gameplay.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);//cameraMain.forward * vertical + cameraMain.forward * horizontal); // Vector3 move = (cameraMain.forward * vertical + cameraMain.forward * horizontal);//new Vector3(horizontal, 0, vertical);
        move.y = 0;

        Vector3 moveDirection = Vector3.forward * moveInput.y + Vector3.right * moveInput.x;


        controller.Move(move * Time.deltaTime * playerSpeed);

        Vector3 projectCameraForward = Vector3.ProjectOnPlane(cameraMain.transform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);


        moveDirection = rotationToCamera * moveDirection;
        Quaternion rotationToMoveDirection = Quaternion.LookRotation(moveDirection, Vector3.up);

        if (!is_Advanture)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToCamera, rotationSpeed * Time.deltaTime);
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);



        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (controls.Gameplay.Jump.triggered && groundedPlayer)
        {
         //   Debug.Log("PlayerController : JumpEvent Input vertical" + controls.Gameplay.Jump.triggered);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

       
    }





    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
      //  Debug.Log("PlayerController : Move Input vertical" + vertical + "\n horizontal "+ horizontal);
    }
    public void LookInputEvent(float x,float y)
    {
        this.x_Look_Value = x;
        this.y_Look_Value = y;
    }

   



}
