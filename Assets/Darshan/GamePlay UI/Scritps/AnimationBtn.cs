using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationBtn : MonoBehaviour
{
    public GameObject highlightButton;

    Button btn;
    public bool isClose;

    public void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void OnEnable()
    {
        btn.onClick.AddListener(OnAnimationClick);
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AllAnimsPanelUpdate += AllAnimsPanelUpdate;

        if (GamePlayButtonEvents.inst != null) EmoteAnimationPlay.AnimationStarted += OnAnimationPlay;
        if (GamePlayButtonEvents.inst != null) EmoteAnimationPlay.AnimationStopped += OnAnimationStoped;

        if (EmoteAnimationPlay.Instance.clearAnimation == null)
        {
            EmoteAnimationPlay.Instance.clearAnimation += ClearAnimations;
        }
    }

    public void OnDisable()
    {
        btn.onClick.RemoveListener(OnAnimationClick);
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AllAnimsPanelUpdate -= AllAnimsPanelUpdate;

        if (GamePlayButtonEvents.inst != null) EmoteAnimationPlay.AnimationStarted -= OnAnimationPlay;
        if (GamePlayButtonEvents.inst != null) EmoteAnimationPlay.AnimationStopped -= OnAnimationStoped;
        EmoteAnimationPlay.Instance.clearAnimation -= ClearAnimations;
    }

    private void AllAnimsPanelUpdate(bool value)
    {
        if (isClose)
        {
            gameObject.SetActive(value);
            if (!EmoteAnimationPlay.Instance.isAnimRunning && !EmoteAnimationPlay.Instance.isFetchingAnim)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnAnimationClick()
    {
        if (!PremiumUsersDetails.Instance.CheckSpecificItem("gesture button"))
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }

        if (!isClose)
        {
            highlightButton.SetActive(true);
            GamePlayButtonEvents.inst.OpenAllAnims();
        }
        else
        {
            highlightButton.SetActive(false);
            GamePlayButtonEvents.inst.CloseEmoteSelectionPanel();
            EmoteAnimationPlay.Instance.StopAnimation(); // stoping animation is any action is performing.
        }
    }

    public void OnAnimationPlay(string s)
    {
        Debug.Log("Animation start hua ");
        highlightButton.SetActive(true);
    }

    public void OnAnimationStoped(string s)
    {
        if (!EmoteAnimationPlay.Instance.isEmoteActive)
        {
            if (highlightButton != null && highlightButton.activeInHierarchy)
            {
                highlightButton.SetActive(false);
            }
        }
           
          
    }

    void ClearAnimations()
    {
        //isClose = true;
        EmoteAnimationPlay.Instance.StopAnimation();
        highlightButton.SetActive(false);
        GamePlayButtonEvents.inst.CloseEmoteSelectionPanel();
    }

}
