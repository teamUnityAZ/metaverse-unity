using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TextChaneWithButtonState : MonoBehaviour 
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeToWhite()
    {
        this.GetComponent<Text>().color = Color.white;
    }
    public void ChangeToSelectedColor()
    {
        this.GetComponent<Text>().color = Color.blue;
    }
}

