using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_LookAt : MonoBehaviour
{
    public Transform targetPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public char test;
    public string check;

    public InputField lol;
    // Update is called once per frame
    void Update()
    {

      


    }

    public void Test()
    {

        if (test==lol.text.LastIndexOf('#'))
        {
            Debug.Log("Testing Value" + lol.text);

        }
        


    }
}
