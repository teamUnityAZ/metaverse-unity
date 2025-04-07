using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerClient : MonoBehaviour
{
    public static PlayerControllerClient instance;

    [SerializeField]
    private float movementSpeed = 2f;

    private float movementSpeedTemp = 2f;

    private float speedSmoothTime = 0.1f;

    public Animator animator = null;

    public Text clientUsername;

    void Start()
    {
        movementSpeedTemp = movementSpeed;
    }
    

    public void ClientEnd(float animationFloat, Vector3 transformPos,Quaternion transformRot)
    {
        Debug.Log("Data receiving" + transformPos + transformRot);
        this.transform.position = transformPos;
        this.transform.rotation = transformRot;
        animator.SetFloat("Blend", 0.5f * animationFloat, speedSmoothTime, Time.deltaTime);
    }

}
