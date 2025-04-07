using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveMentProvider : LocomotionProvider
{

   
    public List<XRController> controllers = null;

    private CharacterController characterController = null;
    private GameObject head = null;

    protected override void Awake()
    {
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
    }

    protected void Start()
    {
        PositionController();
    }

    protected void Update()
    {
        PositionController();
     
    }

    private void PositionController()
    {

        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2);
        characterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        newCenter.x = head.transform.position.x;
        newCenter.z = head.transform.position.z;

        characterController.center = newCenter;

       
    }

    private void CheckforInput()
    {

    }

    private void CheckForMovement(InputDevice device)
    {

    }
    private void ApplyGravity()
    {


    }

}
