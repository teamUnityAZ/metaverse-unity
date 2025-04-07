using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enable_DisableObjects : MonoBehaviour
{
    public Button SwitchCameraObject;
    public Button CameraSnapObject;
    public Button ChatObject;
    public Button ReactionObject;
    public Button EmoteObject;
    public Button ActionsObject;
    public GameObject ReactionPanel;
    public GameObject ActionPanel;
    public GameObject EmotePanel;
    public static Enable_DisableObjects Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        PlayerControllerNew.PlayerIsWalking += OnPlayerWalking;
        PlayerControllerNew.PlayerIsIdle += OnPlayerIdle;

    }
    private void OnDisable()
    {
        PlayerControllerNew.PlayerIsWalking -= OnPlayerWalking;
        PlayerControllerNew.PlayerIsIdle -= OnPlayerIdle;
    }

    private void OnPlayerWalking()
    {
        SwitchCameraObject.interactable = false;
        ChatObject.interactable = false;
        ActionsObject.interactable = false;
        EmoteObject.interactable = false;
        ReactionObject.interactable = false;
    }

    private void OnPlayerIdle()
    {
        if (ReactionPanel.activeInHierarchy ||
                    ActionPanel.activeInHierarchy ||
                    EmotePanel.activeInHierarchy)
        {
            SwitchCameraObject.interactable = false;
            CameraSnapObject.interactable = false;
        }
        else
        {
            SwitchCameraObject.interactable = true;
            CameraSnapObject.interactable = true;
        }
        ChatObject.interactable = true;
        ActionsObject.interactable = true;
        EmoteObject.interactable = true;
        ReactionObject.interactable = true;
    }
}


