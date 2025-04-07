using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class LoadingHandler : MonoBehaviour
{
    public static LoadingHandler Instance;

    [Header("Loading UI Elements")]
    public GameObject loadingPanel;

    public Image loadingSlider;
    public Text loadingText;
    public TextMeshProUGUI loadingPercentageText;

    [Header("Loading BG Elements")]
    public Image loadingBgImage;
    public Image loadingBgImageAlter;
    public Sprite[] loadingSprites;

    public float fadeTimer;

    

    /// <summary>
    /// Help Screen Arrays for 2 scenarios.
    /// If loading percentage is less than 50 only display helpScreenOne items
    /// else loading percentage is greater or equal to 50 display helpScreenTwo items
    /// </summary>
    [Header("Loading Help Screens UI")]
    public GameObject[] helpScreensOne;
    public GameObject[] helpScreensTwo;

    public GameObject Loading_WhiteScreen;
    public GameObject nftvideoloader;
    private int currentBgIndex = 0;
    private int aheadBgIndex = 1;

    [Header("Character Loading")]
    public GameObject characterLoading;

    [Header("fader For Villa")]
    public Image fader;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);

        loadingText.text = "";
    }
    
    private void Start()
    {
        StartCoroutine(StartBGChange());

        #if UNITY_EDITOR
                Debug.unityLogger.logEnabled = true;
        #else
                Debug.unityLogger.logEnabled = false;
        #endif
    }

    IEnumerator StartBGChange()
    {
        loadingBgImage.sprite = loadingSprites[currentBgIndex];

        loadingBgImage.DOFade(1, 0);
        loadingBgImageAlter.DOFade(0, 0);

        yield return new WaitForSeconds(2.0f + fadeTimer);

        loadingBgImageAlter.sprite = loadingSprites[aheadBgIndex];

        loadingBgImage.DOFade(0, fadeTimer);
        loadingBgImageAlter.DOFade(1, fadeTimer);

        yield return new WaitForSeconds(fadeTimer * 2);

        currentBgIndex += 1;
        aheadBgIndex += 1;

        if (currentBgIndex >= loadingSprites.Length)
        {
            currentBgIndex = 0;
        }

        if (aheadBgIndex >= loadingSprites.Length)
        {
            aheadBgIndex = 0;
        }

        StartCoroutine(StartBGChange());
    }

    public void UpdateLoadingStatusText(string message)
    {
        loadingText.text = message;
        loadingText.GetComponent<TextLocalization>().LocalizeTextText(message);
        loadingText.GetComponent<TextLocalization>().LocalizeTextText();
    }

    public void UpdateLoadingSlider(float value, bool doLerp = false)
    {
        if (doLerp)
        {
            loadingSlider.DOFillAmount(value, 0.15f);
        }
        else
        { 
            loadingSlider.fillAmount = value;
        }
        loadingPercentageText.text = ((int)(value * 100f)).ToString() + "%";

        if (loadingSlider.fillAmount < 0.5f)
        {
            ChangeHelpScreenUI(true);
        }
        else
        {
            ChangeHelpScreenUI(false);
        }
    }

    public void ShowLoading()
    {
        loadingPanel.SetActive(true);
    }

    public void HideLoading()
    { 
        loadingPanel.SetActive(false);
    }

    public bool GetLoadingStatus()
    {
        return loadingPanel.activeInHierarchy;
    }
   
    /// <summary>
    /// Switch between HelpDialogs
    /// </summary>
    /// <param name="isFirst"> 
    /// if isFirst is TRUE then helpScreenOne items will be displayed
    /// and helpScreenTwo items will be hidden </param>
    public void ChangeHelpScreenUI(bool isFirst)
    {
        if (isFirst)
        {
            foreach (GameObject _helpDialog in helpScreensOne)
            {
                _helpDialog.SetActive(true);
            }
            foreach (GameObject _helpDialog in helpScreensTwo)
            {
                _helpDialog.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject _helpDialog in helpScreensOne)
            {
                _helpDialog.SetActive(false);
            }
            foreach (GameObject _helpDialog in helpScreensTwo)
            {
                _helpDialog.SetActive(true);
            }
        }

    }
    public void OnCloasenft()
    {
        nftvideoloader.SetActive(false);
    }
    public void OnLoadnft()
    {
        nftvideoloader.SetActive(true);
    }


    public IEnumerator ShowLoadingForCharacterUpdation(float delay)
    {
        characterLoading.SetActive(true);
        yield return new WaitForSeconds(delay);
        characterLoading.SetActive(false);
    }


    public IEnumerator FadeIn()
    {
        fader.gameObject.SetActive(true);
        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            fader.color = new Color(0, 0, 0, i);
            yield return null;
        }
       
    }

    public IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            fader.color = new Color(0, 0, 0, i);
            yield return null;
        }
        fader.gameObject.SetActive(false);
    }


}
