﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class AREyeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject leftEyePrefabs;

    [SerializeField]
    private GameObject rightEyePrefabs;

    private GameObject leftEye;
    private GameObject rightEye;

    private ARFace arFace;

    void Awake()
    {
        arFace = GetComponent<ARFace>();
    }

    void OnEnable()
    {
        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null && faceManager.subsystem != null && faceManager.subsystem.SubsystemDescriptor.supportsEyeTracking)
        {
            arFace.updated += OnUpdated;
            Debug.Log("Eye Tracking is supported on this device");
        }
        else
        {
            Debug.LogError("Eye Tracking is not supported on this device");
        }
    }

    void OnDisable()
    {
        arFace.updated -= OnUpdated;
        SetVisibility(false);
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        if (arFace.leftEye != null && leftEye == null)
        {
            leftEye = Instantiate(leftEyePrefabs, arFace.leftEye);
            leftEye.SetActive(false);
        }
        if (arFace.rightEye != null && rightEye == null)
        {
            rightEye = Instantiate(rightEyePrefabs, arFace.rightEye);
            rightEye.SetActive(false);
        }

        // set visibility
        bool shouldBeVisible = (arFace.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisibility(shouldBeVisible);
    }

    void SetVisibility(bool isVisible)
    {
        if (leftEye != null && rightEye != null)
        {
            leftEye.SetActive(isVisible);
            rightEye.SetActive(isVisible);
        }
    }
}
