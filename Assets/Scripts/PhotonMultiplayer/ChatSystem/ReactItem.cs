using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ReactItem : MonoBehaviour
{
    #region PUBLIC_VAR
    public Image icon;
    public string iconUrl;
    public string _mainImage;
    public int index;
    public Button button;
    #endregion

    #region PRIVATE_VAR
    #endregion

    #region UNITY_METHOD
    //IEnumerator Start()
    //{
       
    //}

     void OnEnable()
    {
        Debug.Log("onenable call hua"+ iconUrl);
      
        if (iconUrl != "" && icon.sprite.name == "buttonLoading")
        {
            StartCoroutine(GetTexture());           
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ButtonClick);
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetData(string url, string mainImage, int _index)
    {
        index = _index;
        iconUrl = url;
        _mainImage = mainImage;

        if (this.isActiveAndEnabled)
        {
            StartCoroutine(GetTexture());
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void ButtonClick()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        Debug.Log("Reaction URL : " + _mainImage);

        if (GamePlayButtonEvents.inst != null && GamePlayButtonEvents.inst.selectionPanelOpen)
        {
            OnSaveDataOnButton();
            return;
        }

        PlayerPrefs.SetString(ConstantsGod.ReactionThumb, _mainImage);
        ArrowManager.OnInvokeReactionButtonClickEvent(PlayerPrefs.GetString(ConstantsGod.ReactionThumb));
    }

    private void OnSaveDataOnButton()
    {
        AnimationData animData = new AnimationData();
        animData.animationName = _mainImage;
        animData.animationURL = _mainImage;
        animData.thumbURL = _mainImage;
        animData.bgColor = GetComponent<Image>().color;
        animData.isEmote = true;
        GamePlayButtonEvents.inst.OnAnimationSelect(animData);
    }
    #endregion

    #region COROUTINE

    IEnumerator GetTexture()
    {
        Debug.Log("Enter");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(iconUrl);
        www.SendWebRequest();
        while(!www.isDone)
        {
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
            icon.sprite = sprite;
        }
    }

    #endregion

    #region DATA
    #endregion

}
