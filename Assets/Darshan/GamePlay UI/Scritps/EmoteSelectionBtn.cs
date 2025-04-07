﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class EmoteSelectionBtn : MonoBehaviour
{

    
    public Button resetBtn;
    public GameObject highlight;

    Button selectionBtn;
    Image bgImg;

    AnimationData animData;
    public void Awake()
    {
        selectionBtn = GetComponent<Button>();
        bgImg = GetComponent<Image>();
        //highlight = transform.Find("SelectionHighLight").gameObject;
        //resetBtn = highlight.transform.Find("CloseBtn").GetComponent<Button>();
    }

    public void OnEnable()
    {
        if (selectionBtn != null) selectionBtn.onClick.AddListener(OnSelectionBtnClick);
        if (resetBtn != null) resetBtn.onClick.AddListener(OnResetBtnClick);

        //selects first btn on enable
        int selectedIndex = PlayerPrefsUtility.GetEncryptedInt(ConstantsGod.EMOTE_SELECTION_INDEX, 0);
        if (selectionBtn != null && transform.GetSiblingIndex() == selectedIndex) OnSelectionBtnClick();

        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AnimationDataUpdated += LoadAnimData;

        //LoadAnimData(transform.GetSiblingIndex());
    }
    public void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AnimationDataUpdated -= LoadAnimData;
        if (selectionBtn != null) selectionBtn.onClick.RemoveListener(OnSelectionBtnClick);
        if (resetBtn != null) resetBtn.onClick.RemoveListener(OnResetBtnClick);
    }

    public void LoadAnimData(int index)
    {
        if (index != transform.GetSiblingIndex()) return;

        string data = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.ANIMATION_DATA + index);
        if (!data.IsNullOrEmpty())
        {
            AnimationData d = JsonUtility.FromJson<AnimationData>(data);
            Debug.LogError("top top :--- "+d.bgColor);
            bgImg.color = d.bgColor;
            if (animData != null && animData.animationName.Equals(d.animationName)) return;
            animData = d;
        }
        else
        {
            bgImg.color = new Color32(255,255,255,180);
        }
    }

    public void LateUpdate()
    {
        if(GamePlayButtonEvents.inst != null)
        {
            highlight.SetActive(GamePlayButtonEvents.inst.selectedActionIndex == transform.GetSiblingIndex());
        }
    }

    private void OnSelectionBtnClick()
    {
        selectionBtn.Select();
        if (GamePlayButtonEvents.inst != null)
        {
            GamePlayButtonEvents.inst.OnActionSelectionChanged(transform.GetSiblingIndex());
            GamePlayButtonEvents.inst.OpenAllAnims();
        }
    }

    void OnResetBtnClick()
    {
        PlayerPrefsUtility.SetEncryptedString(ConstantsGod.ANIMATION_DATA + transform.GetSiblingIndex(), "");
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AnimationDataUdpated(transform.GetSiblingIndex());
    }

}
