using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    public GameObject cameraObject;
    private PlayerControllerNew player;
    public float x;
    public float y;
    public float z;
    private void OnEnable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnSwitchCamera += SetlookCamReference;
    }

    private void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnSwitchCamera -= SetlookCamReference;
    }
    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerControllerNew>();

        //cameraObject =  ReferrencesForDynamicMuseum.instance.randerCamera.gameObject;
        SetlookCamReference();
    }

    void SetlookCamReference()
    {
        if (player.isFirstPerson)
        {
            cameraObject = player.firstPersonCameraObj;
        }
        else
        {
            cameraObject = ReferrencesForDynamicMuseum.instance.randerCamera.gameObject;
        }
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.LookAt(new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y, cameraObject.transform.position.z));
    }
}
