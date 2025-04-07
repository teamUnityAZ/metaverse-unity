using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class EmoteBtnItemView : MonoBehaviour
{
    [SerializeField] Image actionImg;
    AnimationData animData;

    private Image bgImage;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
    }

    public void OnEnable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AnimationDataUpdated += LoadAnimData;

        LoadAnimData(transform.GetSiblingIndex());
    }
    public void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.AnimationDataUpdated -= LoadAnimData;
    }

    public void LoadAnimData(int index)
    {
        if (index != transform.GetSiblingIndex()) return;

        actionImg.gameObject.SetActive(false);
        string data = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.ANIMATION_DATA + index);
        if (!data.IsNullOrEmpty())
        {
            AnimationData d = JsonUtility.FromJson<AnimationData>(data);
            if (animData != null && animData.animationName.Equals(d.animationName))
            {
                actionImg.gameObject.SetActive(true);
                return;
            }
            bgImage.color = d.bgColor;
            animData = d;
            StartCoroutine(LoadSpriteEnv(animData.thumbURL));
        }
        else
        {
            if (gameObject.transform.GetChild(0).GetComponent<EmoteActionBtn>())
            {
                gameObject.transform.GetChild(0).GetComponent<EmoteActionBtn>().UnloadAnim();
            }
            StopAllCoroutines();
        }
    }

    IEnumerator LoadSpriteEnv(string ImageUrl)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (ImageUrl.Equals(""))
            {
                // Loader.SetActive(false);
            }
            else
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(ImageUrl);
                www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }
                Texture2D thumbnailTexture = DownloadHandlerTexture.GetContent(www);
                thumbnailTexture.Compress(true);
                Sprite sprite = Sprite.Create(thumbnailTexture, new Rect(0, 0, thumbnailTexture.width, thumbnailTexture.height), new Vector2(0, 0));
                if (actionImg != null)
                {
                    actionImg.sprite = sprite;
                    actionImg.gameObject.SetActive(true);
                }
                www.Dispose();
            }
        }
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
    }
}

[Serializable]
public class AnimationData
{
    public string animationName;
    public string animationURL;
    public string thumbURL;
    public Color32 bgColor;
    public bool isEmote;
}
