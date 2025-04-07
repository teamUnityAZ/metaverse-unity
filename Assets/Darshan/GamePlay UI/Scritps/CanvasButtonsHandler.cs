using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonsHandler : MonoBehaviour
{
    static CanvasButtonsHandler _inst;
    public static CanvasButtonsHandler inst
    {
        get
        {
            if (_inst == null) _inst = FindObjectOfType<CanvasButtonsHandler>();
            return _inst;
        }
    }
    public GameObject actionsContainer;
    public Transform actionToggleImg;
    public ActionSelectionPanelHandler ActionSelectionPanel;

    [Header("FPS Button Reference")]
    public GameObject fPSButton;

    public void OnGotoAnotherWorldClick()
    {
        GamePlayButtonEvents.inst.OnGotoAnotherWorldClick();
    }

    public void OnWordrobeClick()
    {
        GamePlayButtonEvents.inst.OnWordrobeClick();
    }

    public void OnHelpButtonClick(bool isOn)
    {
        GamePlayButtonEvents.inst.UpdateHelpObjects(isOn);
    }

    public void OnSettingButtonClick()
    {
        GamePlayButtonEvents.inst.OnSettingButtonClick();
    }

    public void OnExitButtonClick()
    {
        GamePlayButtonEvents.inst.OnExitButtonClick();
    }

    public void OnPeopeClick()
    {
        GamePlayButtonEvents.inst.OnPeopeClick();
    }

    public void OnAnnouncementClick()
    {
        GamePlayButtonEvents.inst.OnAnnouncementClick();
    }

    public void OnInviteClick()
    {
        GamePlayButtonEvents.inst.OnInviteClick();
    }

    public void OnSwitchCameraClick()
    {
        if (!PremiumUsersDetails.Instance.CheckSpecificItem("fp_camera"))
        {
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }

        GamePlayButtonEvents.inst.OnSwitchCameraClick();
    }

    public void OnChangehighlightedFPSbutton(bool isSelected)
    {
        fPSButton.GetComponent<Image>().enabled = isSelected;
    }

    public void OnSelfiBtnClick()
    {
        GamePlayButtonEvents.inst.OnSelfieClick();
    }

    public void OnOpenAnimationPanel()
    {
        ActionSelectionPanel.OpenPanel();
        GamePlayButtonEvents.inst.selectionPanelOpen = true;
        GamePlayButtonEvents.inst.OpenAllAnims();
    }

    public void CloseEmoteSelectionPanel()
    {
        GamePlayButtonEvents.inst.CloseEmoteSelectionPanel();
        ActionSelectionPanel.ClosePanel();
        GamePlayButtonEvents.inst.selectionPanelOpen = false;
    }

    public void OnJumpBtnUp()
    {
        GamePlayButtonEvents.inst.OnJumpBtnUp();
    }

    public void OnJumpBtnDown()
    {
        GamePlayButtonEvents.inst.OnJumpBtnDown();
    }

    bool isActionShowing;
    public void OnActionsToggleClicked()
    {

        if (!PremiumUsersDetails.Instance.CheckSpecificItem("env_actions"))
        {
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }

        isActionShowing = !isActionShowing;
        actionsContainer.SetActive(isActionShowing);
        Vector3 rot = new Vector3(0f, 0f, (isActionShowing) ? 0f : 180f);
        actionToggleImg.rotation = Quaternion.Euler(rot);
    }
}
