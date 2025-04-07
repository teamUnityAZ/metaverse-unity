using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dissableinTime : MonoBehaviour
{

    public float timetoDisable;
    // Start is called before the first frame update
    void OnEnable()
    {

        Invoke("Disablepanel", timetoDisable);


    }
    void Disablepanel()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
 
}
