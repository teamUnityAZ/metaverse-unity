using System;
using System.Collections;
using System.Collections.Generic;
using AIFLogger;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class EmoteActionBtn : MonoBehaviour { 
    Button actionBtn;
    GameObject highlightObj;
    ButtonClickAndHold btnClickNHold;
    AnimationData animData;

    bool isAnimRunning;
    bool dragging = false;
    private DragDectector dragDectector;
    public void Awake()
    {
        actionBtn = GetComponent<Button>();
        highlightObj = transform.Find("BtnHighlight").gameObject;
        btnClickNHold = GetComponent<ButtonClickAndHold>();
        dragDectector = transform.gameObject.AddComponent<DragDectector>();
    }

    public void OnEnable()
    {
        //if (actionBtn != null) actionBtn.onClick.AddListener(OnActionBtnClick);
        if (btnClickNHold != null)
        {
            btnClickNHold.Clicked += OnActionBtnClick;
            btnClickNHold.LongClicked += OnLongBtnClick;
        }
        if (GamePlayButtonEvents.inst != null)
        {
            GamePlayButtonEvents.inst.AnimationDataUpdated += LoadAnimData;
        }

        EmoteAnimationPlay.AnimationStarted += AnimationStarted;
        EmoteAnimationPlay.AnimationStopped += AnimationStopped;
        LoadAnimData(transform.parent.GetSiblingIndex());
        string animName = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.SELECTED_ANIMATION_NAME);
        if (!animName.IsNullOrEmpty() && animData != null && animData.animationName.Equals(animName))
        {
            isAnimRunning = EmoteAnimationPlay.Instance.isAnimRunning;
            highlightObj.SetActive(true);
        }else
        {
            isAnimRunning = false;
            highlightObj.SetActive(false);
        }
    }

    public void OnDisable()
    {
        //if (actionBtn != null) actionBtn.onClick.RemoveListener(OnActionBtnClick);
        if (btnClickNHold != null)
        {
            btnClickNHold.LongClicked -= OnLongBtnClick;
            btnClickNHold.Clicked -= OnActionBtnClick;
        }
        if (GamePlayButtonEvents.inst != null)
        {
            GamePlayButtonEvents.inst.AnimationDataUpdated -= LoadAnimData;
        }
        EmoteAnimationPlay.AnimationStarted -= AnimationStarted;
        EmoteAnimationPlay.AnimationStopped -= AnimationStopped;
    }


    public void LateUpdate()
    {
        UpdateActionBtn();
    }

    void UpdateActionBtn()
    {
        if (EmoteAnimationPlay.Instance != null)
        {
            if(animData != null) btnClickNHold.interactable = !EmoteAnimationPlay.Instance.isFetchingAnim;
        }
        if (isAnimRunning)
        {
            actionBtn.Select();
        }
        if (highlightObj != null) highlightObj.SetActive(isAnimRunning);
    }

    public void LoadAnimData(int index)
    {
        if (index != transform.parent.GetSiblingIndex()) return;

        string data = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.ANIMATION_DATA + index);
        if (!data.IsNullOrEmpty())
        {
            AnimationData d = JsonUtility.FromJson<AnimationData>(data);
            if (animData != null && animData.animationName.Equals(d.animationName)) return;
            animData = d;
            btnClickNHold.interactable = true;
        }
    }

    Coroutine startAnim = null;
    public void OnActionBtnClick()
    {
        if (dragDectector.isDragging)
        {
            return;
        }

        if (animData == null || animData.animationName.IsNullOrEmpty())
        {
            print("No Animation Assigned");
            //when empty emote button is clicked open selection panel  ----- amit-21-06-2022
            btnClickNHold.OnLongClick();
            return;
        }

        if (animData.isEmote)//rik
        {
            PlayerPrefs.SetString(ConstantsGod.ReactionThumb, animData.animationURL);
            ArrowManager.OnInvokeReactionButtonClickEvent(PlayerPrefs.GetString(ConstantsGod.ReactionThumb));
            return;
        }
        if (isAnimRunning && EmoteAnimationPlay.Instance.isAnimRunning)
        {
            //print("Stoping Animation");
            StopEmoteAnimation();
            return;
        }

        StopEmoteAnimation();
        //print("Start Animation");
        if(startAnim!= null) StopCoroutine(startAnim);
        startAnim = StartCoroutine(StartEmoteAnim());
    }

    IEnumerator StartEmoteAnim()
    {
        while(EmoteAnimationPlay.Instance.isAnimRunning)
        {
            yield return null;
        }
        if (animData != null)
        {
            EmoteAnimationPlay.remoteUrlAnimation = animData.animationURL;
            EmoteAnimationPlay.remoteUrlAnimationName = animData.animationName;
            EmoteAnimationPlay.Instance.Load(animData.animationURL, null);
            isAnimRunning = true;
            PlayerPrefsUtility.SetEncryptedString(ConstantsGod.SELECTED_ANIMATION_NAME, animData.animationName);
            actionBtn.Select();
            try
            {
                LoadFromFile.instance.leftJoyStick.transform.GetChild(0).GetComponent<OnScreenStick>().movementRange = 0;

            }
            catch (Exception e)
            {

            }
        }
    }

    void StopEmoteAnimation()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnEmoteAnimationStop();
    }

    private void OnLongBtnClick()
    {
        if (dragDectector.isDragging)
            return;
        PlayerPrefsUtility.SetEncryptedInt(ConstantsGod.EMOTE_SELECTION_INDEX, transform.parent.GetSiblingIndex());
        CanvasButtonsHandler.inst.OnOpenAnimationPanel();
    }

    private void AnimationStopped(string animName)
    {
        isAnimRunning = false;
        highlightObj.SetActive(false);
        PlayerPrefsUtility.SetEncryptedString(ConstantsGod.SELECTED_ANIMATION_NAME, "");
    }

    private void AnimationStarted(string animName)
    {
        Debug.Log("animation call hua new");
        if (animData != null && animData.animationName.Equals(animName))
        {
            isAnimRunning = EmoteAnimationPlay.Instance.isAnimRunning;
            highlightObj.SetActive(true);
        }
        else
        {
            isAnimRunning = false;
            highlightObj.SetActive(false);
        }
    }

    public void UnloadAnim()
    {
        animData = null;
    }

  

}
