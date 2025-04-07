using Metaverse;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using WebSocketSharp;

public class LoadButtonClick : MonoBehaviour
{
    [HideInInspector] public string objectUrl;
    [HideInInspector] public string thumbUrl;
    [HideInInspector] public string animationName;
    [HideInInspector] public EmoteFilterManager controller;
    public GameObject highlighter;
    public GameObject ContentPanel;
    public GameObject prefabObj;

    public void OnEnable()
    {
        EmoteAnimationPlay.AnimationStarted += OnAnimationStarted;
        EmoteAnimationPlay.AnimationStopped += AnimationStopped;
        string name = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.SELECTED_ANIMATION_NAME);
        if (!name.IsNullOrEmpty() && !animationName.IsNullOrEmpty() && animationName.Equals(name))
        {
            highlighter.SetActive(true);
        }
        else
        {
            highlighter.SetActive(false);
        }
    }

    public void OnDisable()
    {
        EmoteAnimationPlay.AnimationStarted -= OnAnimationStarted;
        EmoteAnimationPlay.AnimationStopped -= AnimationStopped;
    }

    public void Initializ(string animUrl,string animname, EmoteFilterManager ctrlr, GameObject Content, string thumbURL = null)
    {
        objectUrl = animUrl;
        animationName = animname;
        controller = ctrlr;
        ContentPanel = Content;
        thumbUrl = thumbURL;
    }

    public void OnButtonClick()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        if (GamePlayButtonEvents.inst != null && GamePlayButtonEvents.inst.selectionPanelOpen)
        {
            OnSaveDataOnButton();
            return;
        }

        //if (EmoteAnimationPlay.Instance.alreadyRuning)
        {
            //LoadFromFile.animClick = true;
            EmoteAnimationPlay.remoteUrlAnimation = objectUrl;
            EmoteAnimationPlay.remoteUrlAnimationName = animationName;
            //  PlayerPrefs.Save();
            //prefabObj.transform.GetChild(3).gameObject.SetActive(true);
            EmoteAnimationPlay.Instance.Load(objectUrl, prefabObj);

            foreach (Transform obj in ContentPanel.transform)
            {
                
                    obj.gameObject.transform.GetChild(2).gameObject.SetActive(false);
               // }

            }
            highlighter.SetActive(true);
            PlayerPrefsUtility.SetEncryptedString(ConstantsGod.SELECTED_ANIMATION_NAME, animationName);
        }
        try
        {
            LoadFromFile.instance.leftJoyStick.transform.GetChild(0).GetComponent<OnScreenStick>().movementRange = 0;

        }
        catch (Exception e)
        {

        }
        // StartCoroutine(ButtonClick());

    }

    public void OnSaveDataOnButton()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();

        AnimationData animData = new AnimationData();
        animData.animationName = animationName;
        animData.animationURL = objectUrl;
        animData.thumbURL = thumbUrl;
        animData.bgColor = GetComponent<Image>().color;
        GamePlayButtonEvents.inst.OnAnimationSelect(animData);
    }


    private void AnimationStopped(string animName)
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        highlighter.SetActive(false);
    }

    private void OnAnimationStarted(string animName)
    {
        if(animationName.Equals(animName)) highlighter.SetActive(true);
        else highlighter.SetActive(false);
    }
}
