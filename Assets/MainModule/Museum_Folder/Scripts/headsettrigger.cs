using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headsettrigger : MonoBehaviour
{
    public GameObject HeadObj, HandObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("other.CompareTag" + other.tag);
        //if (other.CompareTag("Hands"))
        //{
        //    Debug.Log("other.CompareTag" + other.tag);
        //    this.transform.parent = other.transform;
        //    //  Gamemanager._InstanceGM.animationController.headSetInReseptionistHand.
        //}
        //if (other.CompareTag("Head"))
        //{
        //    Debug.Log("other.CompareTag" + other.tag);
        //    this.transform.parent = other.transform;
        //    //  Gamemanager._InstanceGM.animationController.headSetInReseptionistHand.
        //}
    }
}
