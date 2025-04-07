﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicControllerButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject otherButton;
    public GameObject micOnButtonGameplay;
    public GameObject micOffButtonGameplay;

    private void OnEnable()
    {
        if (XanaConstants.xanaConstants.mic == 1)
        {
            if(this.gameObject.name== "OffButton")
            {
                otherButton.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else if(this.gameObject.name == "OnButton")
            {
                this.gameObject.SetActive(true);
                otherButton.SetActive(false);
            }
        }
        else
        {
            if (this.gameObject.name == "OffButton")
            {
                this.gameObject.SetActive(true);
                otherButton.SetActive(false);
            }
            else if (this.gameObject.name == "OnButton")
            {
                otherButton.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
        //XanaVoiceChat.instance.UpdateMicButton();
    }

    public void ClickMicMain()
    {
        if(XanaConstants.xanaConstants.mic == 1)
        {
            XanaConstants.xanaConstants.StopMic();
            XanaVoiceChat.instance.TurnOffMic();
            micOnButtonGameplay.SetActive(false);
            micOffButtonGameplay.SetActive(true);
        }
        else
        {
            XanaConstants.xanaConstants.PlayMic();
            XanaVoiceChat.instance.TurnOnMic();
            micOnButtonGameplay.SetActive(true);
            micOffButtonGameplay.SetActive(false);
        }
        OnEnable();
    }
}
