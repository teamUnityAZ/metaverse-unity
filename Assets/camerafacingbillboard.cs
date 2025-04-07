using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class camerafacingbillboard : MonoBehaviour
{
    public GameObject cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("EnvironmentRenderCamera");
    }

    // Update is called once per frame
    void Update()
    {
      
        // Transform cam = Camera.main.transform;
        transform.LookAt(transform.position + cameraTransform.transform.rotation * Vector3.forward , cameraTransform.transform.rotation*Vector3.up);
    }
}
