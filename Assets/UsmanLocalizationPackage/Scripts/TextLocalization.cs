﻿
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalization : MonoBehaviour
{
    public Text LocalizeText;
    public TextMeshProUGUI LocalizeTextTMP;

    private Coroutine _myCoroutine;

    string currentText = "";

    private void OnEnable()
    {
        if (LocalizeText)
        {
            currentText = LocalizeText.text;
        }

        else if (LocalizeTextTMP)
        {
            currentText = LocalizeTextTMP.text;
        }

        // GameManager.currentLanguage = "ja";

        LocalizeTextText();
    }

    //private void Start()
    //{
    //    Debug.Log(LocalizeText.text + " start");

    //    if (LocalizeText)
    //    {
    //        currentText = LocalizeText.text;
    //    }

    //    else if (LocalizeTextTMP)
    //    {
    //        currentText = LocalizeTextTMP.text;
    //    }

    //    LocalizeTextText();
    //}

    public void LocalizeTextText()
    {
        if (_myCoroutine != null)
        {
            StopCoroutine(_myCoroutine);
        }

        if (gameObject.activeInHierarchy)
        {
            _myCoroutine = StartCoroutine(StartTranslation());
        }
    }

    private IEnumerator StartTranslation()
    {
        while (!CustomLocalization.IsReady)
        {
            yield return null;
        }
       
        if (LocalizeText != null)
        {
            StaticLocalizeTextText();

        }
        else if (LocalizeTextTMP != null)
        {
            StaticLocalizeTextPro();
        }
    }

    private void StaticLocalizeTextPro()
    {
        
        if (CustomLocalization.localisationDict == null || CustomLocalization.localisationDict.Count <= 0) return;

        #region Old Method

        // foreach (RecordsLanguage rl in CustomLocalization.LocalisationSheet)
        // {
        //     if (rl.Keys == LocalizeTextTMP.text && !LocalizeTextTMP.text.IsNullOrEmpty())
        //     {
        //         if (Application.systemLanguage == SystemLanguage.Japanese &&
        //             !string.IsNullOrEmpty(rl.Japanese))
        //             LocalizeTextTMP.text = rl.Japanese;
        //         else if (Application.systemLanguage == SystemLanguage.English &&
        //                  !string.IsNullOrEmpty(rl.English))
        //             LocalizeTextTMP.text = rl.English;
        //         _hasTranslated = true;
        //         break;
        //     }
        // }

        #endregion
        
        if (CustomLocalization.localisationDict.TryGetValue(currentText, out RecordsLanguage find))
        {
            if (!CustomLocalization.forceJapanese)
            {
                switch (GameManager.currentLanguage)
                {
                    case "en":
                        LocalizeTextTMP.text = find.English;
                        break;
                    case "ja":
                        LocalizeTextTMP.text = find.Japanese;
                        break;
                }
            }
            else
            {
                LocalizeTextTMP.text = find.Japanese;
            }
        }
    }

    private void StaticLocalizeTextText()
    {
        if (CustomLocalization.localisationDict == null || CustomLocalization.localisationDict.Count <= 0) return;

        #region Old Method

        // foreach (RecordsLanguage rl in CustomLocalization.LocalisationSheet)
        // {
        //     if (rl.Keys == LocalizeText.text && !LocalizeText.text.IsNullOrEmpty())
        //     {
        //        if (Application.systemLanguage == SystemLanguage.Japanese && !string.IsNullOrEmpty(rl.Japanese))
        //             LocalizeText.text = rl.Japanese;
        //         else if (Application.systemLanguage == SystemLanguage.English &&
        //                  !string.IsNullOrEmpty(rl.English))
        //             LocalizeText.text = rl.English;
        //         _hasTranslated = true;
        //         break;
        //     }
        // }

        #endregion

        if (CustomLocalization.localisationDict.TryGetValue(currentText, out RecordsLanguage find))
        {
            if (!CustomLocalization.forceJapanese)
            {
                switch (GameManager.currentLanguage)
                {
                    case "en":
                        LocalizeText.text = find.English;
                        break;
                    case "ja":
                        LocalizeText.text = find.Japanese;
                        break;
                }
            }
            else
            {
                LocalizeText.text = find.Japanese;
            }
        }

    }

    /// <summary>
    /// Sets Unity's text component as text found in localisation sheet
    /// </summary>
    /// <param name="key">Key in localisation sheet</param>
    public void LocalizeTextText(string key) //Utility for external use
    {

        if (LocalizeText != null)
        {
            currentText = key;
            LocalizeText.text = key;
        }
        else if (LocalizeTextTMP != null)
        {
            currentText = key;
            LocalizeTextTMP.text = key;
        }
        LocalizeTextText();
    }
    
    /// <summary>
    /// Returns translation as string found by key (This function does not check if sheet is filled
    /// use LocalizeText() instead
    /// </summary>
    /// <param name="key">Key to find translation against</param>
    /// <returns>Found Key as string or key itself if not found in sheet</returns>
    public static string GetLocaliseTextByKey(string key)
    {
        if (CustomLocalization.localisationDict == null || CustomLocalization.localisationDict.Count <= 0) return key;
        if (CustomLocalization.localisationDict.TryGetValue(key, out RecordsLanguage find))
        {
            if (!CustomLocalization.forceJapanese)
            {
                switch (GameManager.currentLanguage)
                {
                    case "en":
                        return find.English;
                    case "ja":
                        return find.Japanese;
                }
            }
            else
            {
                return find.Japanese;
            }
            
        }
        Debug.LogWarning("Key not found. Please add it to sheet");
        return key; //Normally return key as it is if not found
    }
}