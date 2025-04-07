using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerCustom : MonoBehaviour
{
    [Header("RigidBody")]
    public Rigidbody m_Rigidbody;

    [Header("Animator")]
    public Animator m_Animator;

    [Header("Movement/Rotation Speed")]
    public float m_MovementSpeed;
    public float m_RotationSpeed;
    public float m_LerpSpeed;

    [Header("Main Camera")]
    public GameObject m_MainCamera;
    public Vector3 m_PositionOffSet;

    Vector2 m_StartTouchPosition;
    Vector3 m_MovementDirection;

    float m_Blend;
    bool m_IsRotationApplied;
    public bool m_IsCameraMoving;

    [SerializeField]
    private CinemachineFreeLook cinemachine;

    public CharacterController character;


 public   float horizontal;
    public float vertical;

    float x_Look_Value;
    float y_Look_Value;
    public bool is_Advanture;

    public float lookSpeed;
    public float rotationSpeed = 280.0f;
 

 

    void Start()
    {
        m_IsRotationApplied = true;
    }

    void Update()
    {
        Editor_Input();

    }

    private void FixedUpdate()
    {
        Editor_Movement();

    }


     Quaternion rotationToMoveDirection;

    void Editor_Input()
    {
       
            m_MovementDirection = Vector3.zero;
        
        Debug.Log("Current WE are here in Editor Input");
        //if (!is_Advanture)
        //{
        //    m_MovementDirection = new Vector3(0f, 0f, vertical);//camer
        //}
        //else
      //  if (m_MovementDirection != Vector3.zero)
        {
            m_MovementDirection = new Vector3(horizontal, 0f, vertical);//camer
        }


        Vector3 projectCameraForward = Vector3.ProjectOnPlane(m_MainCamera.transform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);


        m_MovementDirection = rotationToCamera * m_MovementDirection;
        if (m_MovementDirection != Vector3.zero)
        {
            rotationToMoveDirection = Quaternion.LookRotation(m_MovementDirection, Vector3.up);
        }
        if (!is_Advanture)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToCamera, rotationSpeed * Time.deltaTime);
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);


       // cinemachine.m_XAxis.Value += x_Look_Value * 200 * lookSpeed * Time.deltaTime;
       // cinemachine.m_XAxis.Value += y_Look_Value * lookSpeed * Time.deltaTime;
    }

    void Editor_Movement()
    {
        if (m_MovementDirection.magnitude > 0)
        {
        
              m_Rigidbody.velocity = m_MovementDirection * m_MovementSpeed;
             m_Blend += 0.05f;
            m_Animator.SetFloat("Movement", Mathf.Clamp(m_Blend, 0.0f, 0.6f));
        }
        else
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Animator.SetFloat("Movement", 0.0f);
        }
    }

    //void Mobile_Input()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        Touch l_Touch = Input.GetTouch(0);

    //        if (l_Touch.phase == TouchPhase.Began)
    //        {
    //            m_StartTouchPosition = l_Touch.position;
    //        }

    //        if (l_Touch.phase == TouchPhase.Moved)
    //        {
    //            Vector3 l_Direction = (l_Touch.position - m_StartTouchPosition).normalized;
    //            m_MovementDirection = new Vector3(l_Direction.x, 0, l_Direction.y);
    //        }

    //        if (l_Touch.phase == TouchPhase.Ended)
    //        {
    //            m_Animator.SetFloat("Movement", 0);
    //        }
    //    }
    //}

    //void Mobile_Movement()
    //{
    //    if (m_MovementDirection.magnitude > 0)
    //    {
    //        m_Rigidbody.velocity = m_MovementDirection * m_MovementSpeed;
    //        transform.LookAt(m_MovementDirection * 500);
    //        m_Blend += 0.05f;
    //        m_Animator.SetFloat("Movement", Mathf.Clamp(m_Blend, 0.0f, 0.6f));
    //    }
    //    else
    //    {
    //        m_Rigidbody.velocity = Vector3.zero;
    //    }
    //}

    //void CameraMovement()
    //{
    //    if (!m_IsCameraMoving) return;

    //    Vector3 l_NewPosition = transform.position + m_PositionOffSet;
    //    m_MainCamera.transform.position = Vector3.Lerp(m_MainCamera.transform.position, l_NewPosition, 0.2f);
    //    m_MainCamera.transform.LookAt(transform.position);
    //    //m_MainCamera.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X"));
    //}
    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
        Debug.Log("PlayerController : Move Input vertical" + vertical + "\n horizontal " + horizontal);
    }
    public void LookInputEvent(float x, float y)
    {
        this.x_Look_Value = x;
        this.y_Look_Value = y;
    }
}