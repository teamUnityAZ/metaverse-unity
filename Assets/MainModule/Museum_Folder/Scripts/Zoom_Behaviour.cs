using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Zoom_Behaviour : MonoBehaviour
{
    public GameObject zoomCam;
    public int FOV;
    public bool zoomBool;
    CinemachineVirtualCamera VCam;
    // Start is called before the first frame update
    void Start()
    {
        VCam = zoomCam.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(zoomBool)
        {
            VCam.m_Lens.FieldOfView = Mathf.Lerp(VCam.m_Lens.FieldOfView, 35, Time.deltaTime);
            if (VCam.m_Lens.FieldOfView<37)
            {
                zoomBool = false;
            }
        }
    }

    public void Zoom()
    {

    }
}
