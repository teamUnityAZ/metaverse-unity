﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCharacterController : MonoBehaviour
{

    public XRNode inputSource;
    public Vector2 inputAxis;

    public CharacterController character;
     public float speed = 2f;
    public float gravity = -9.81f;

    public float additionalHeight= 0.2f;
    float fallingSpeed;

    public LayerMask groundLayer;
    public bool isGrounded;

  
    private float rotationSpeed = 0.1f;
    XRRig rig;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }
    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);

        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);


        character.Move(direction * Time.fixedDeltaTime * speed);

         isGrounded = CheckIfGround();
        if (isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);

    }
    bool CheckIfGround()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLenght = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLenght, groundLayer);
        return hasHit;
    }
}
