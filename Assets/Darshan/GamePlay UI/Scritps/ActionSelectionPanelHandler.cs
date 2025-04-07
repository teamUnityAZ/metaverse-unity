﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ActionSelectionPanelHandler : MonoBehaviour
{
    public GameObject panel;
    public GameObject errorObj;

    [Header("Emote React Screen Reference")]
    public ReactScreen reactScreen;

    List<AnimationData> animations;

    public void OnEnable()
    {
        if (GamePlayButtonEvents.inst != null)
        {
            GamePlayButtonEvents.inst.OnAnimationSelected += LoadAnimData;
            GamePlayButtonEvents.inst.AnimationDataUpdated += UpdateAnimList;
        }
        animations = new List<AnimationData>();
        LoadAnimList();
    }


    public void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null)
        {
            GamePlayButtonEvents.inst.OnAnimationSelected -= LoadAnimData;
            GamePlayButtonEvents.inst.AnimationDataUpdated -= UpdateAnimList;
            GamePlayButtonEvents.inst.selectedActionIndex = -1;
        }
    }

    void LoadAnimList()
    {
        for (int i = 0; i < 10; i++)
        {
            AnimationData d = new AnimationData();
            animations.Add(d);
            string data = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.ANIMATION_DATA + i);
            if (!data.IsNullOrEmpty())
            {
                d = JsonUtility.FromJson<AnimationData>(data);
                animations[i] = d;
            }
        }
    }

    public void LoadAnimData(AnimationData animData)
    {
        if (CheckAnimationList(animData)) return;
        int selectedIndex = GamePlayButtonEvents.inst.selectedActionIndex;
        PlayerPrefsUtility.SetEncryptedString(ConstantsGod.ANIMATION_DATA + selectedIndex, JsonUtility.ToJson(animData));
        GamePlayButtonEvents.inst.AnimationDataUdpated(selectedIndex);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        if (reactScreen != null)
        {
            reactScreen.OnShowEmotePanelFromFavorite();
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        if (reactScreen != null)
        {
            reactScreen.ReactButtonClick(true);
        }
    }

    public void OnResetAnimationClick()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefsUtility.SetEncryptedString(ConstantsGod.ANIMATION_DATA + i, "");
            GamePlayButtonEvents.inst.AnimationDataUdpated(i);
        }
        GamePlayButtonEvents.inst.OnEmoteAnimationStop();
        animations = new List<AnimationData>();
        LoadAnimList();
    }

    private void UpdateAnimList(int index)
    {
        string data = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.ANIMATION_DATA + index);
        if (!data.IsNullOrEmpty())
        {
            AnimationData d = JsonUtility.FromJson<AnimationData>(data);
            animations[index] = d;
        }else
        {
            animations[index] = new AnimationData();
        }
    }


    private bool CheckAnimationList(AnimationData data)
    {
        for (int i = 0; i < animations.Count; i++)
        {
            if(data != null && !animations[i].animationName.IsNullOrEmpty() && animations[i].animationName.Equals(data.animationName))
            {
                Debug.LogFormat("aniamtion name matched");
                errorObj.SetActive(true);
                return true;
            }
        }
        Debug.LogFormat("aniamtion name not here return false");
        return false;
    }
}
