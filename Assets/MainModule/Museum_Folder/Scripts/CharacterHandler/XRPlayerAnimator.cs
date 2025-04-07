using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class Test : DeviceBasedContinuousMoveProvider
{
    Vector2 m_MovementDirection;
   public Vector2 check()
    {

        m_MovementDirection = base.ReadInput();
        return m_MovementDirection;

    }
}
public class XRPlayerAnimator :  MonoBehaviour
{
  public  Test test;

    public Animator m_Animator;

    Vector2 m_MovementDirection;

    public float m_Blend;

   // public DeviceBasedContinuousMoveProvider deviceBased;
    // Start is called before the first frame update
    void Start()
    {
        test = new Test();
      //  deviceBased = new DeviceBasedContinuousMoveProvider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
      //  m_MovementDirection = test.check();

        if (m_MovementDirection.magnitude > 0)
        {
            m_MovementDirection = test.check();
             m_Blend += 0.05f;
            m_Animator.SetFloat("Movement", Mathf.Clamp(m_Blend, 0.0f, 0.6f));
        }
        else
        {
          
            m_Animator.SetFloat("Movement", 0.0f);
        }
    }
}
