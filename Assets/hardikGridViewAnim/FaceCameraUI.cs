using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraUI : MonoBehaviour
{
    public static Transform selficamOtherAssign;
    private Transform mainCam;
    private Transform thirdPersonCam;
    private Transform firstPersonCam;
    public GameObject selfieStick;
    public Transform selfieCam;
    public Transform selfieCamOther;
    
    private Transform localTrans;
    public bool oneTimeCall = false;
    // Start is called before the first frame update
    void Start()
    {
        localTrans = GetComponent<Transform>();

        if(GameObject.FindGameObjectWithTag("SceneManager"))
        {
            mainCam = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<LoadFromFile>().PlayerCamera.transform;
            thirdPersonCam = mainCam;
            firstPersonCam= GameObject.FindGameObjectWithTag("SceneManager").GetComponent<LoadFromFile>().firstPersonCamera.transform;
        }

    }


    public void OnSwitchToFirstPersonCamera()
    {

    }


    // Update is called once per frame
    void Update()
    {


        if (firstPersonCam.gameObject.activeInHierarchy)
        {
            mainCam = firstPersonCam;
        }
        else
        {
            mainCam = thirdPersonCam;
        }

        if (mainCam.gameObject.activeInHierarchy)
        {
            //selfieCam = null;
            //selfieCamOther = null;
            localTrans.LookAt(2 * localTrans.position - mainCam.position);
            oneTimeCall = true;
        }
        else
        {

            if (selfieStick.activeInHierarchy)
            {
                //selfieCamOther = selfieStick.gameObject.transform.GetChild(2).GetChild(0).gameObject.transform;
                //selfieCam = selfieStick.gameObject.transform.GetChild(1).gameObject.transform;

                if (oneTimeCall)
                {
                    selficamOtherAssign = selfieCamOther;
                    Debug.Log("call value==="+ selficamOtherAssign);
                    oneTimeCall = false;
                    GameObject[] objects = GameObject.FindGameObjectsWithTag("PhotonLocalPlayer");

                    for (int i = 0; i < objects.Length; i++)
                    {
                        if (!objects[i].GetComponent<PhotonView>().IsMine)
                        {
                            objects[i].gameObject.GetComponent<ArrowManager>().PhotonUserName.gameObject.GetComponent<FaceCameraUI>().selfieCam = selfieCam;
                            objects[i].gameObject.GetComponent<ArrowManager>().PhotonUserName.gameObject.GetComponent<FaceCameraUI>().selfieCamOther = selfieCamOther;
                        }

                    }

                }
                localTrans.LookAt(2 * localTrans.position - selfieCam.position);
                localTrans.LookAt(2 * localTrans.position - selfieCamOther.position);


            }

         
           


            //localTrans.LookAt(2 * localTrans.position - selfieCam.position);


            //if (!selfieStick.activeInHierarchy)
            //{

            //}
            //{
            //    this.gameObject.GetComponent<FaceCameraUI>().selfieCamOther = selfieCamOther;
            //}



        }
            
          

     
    }

    public void callInvoke()
    {
        
    }

}
