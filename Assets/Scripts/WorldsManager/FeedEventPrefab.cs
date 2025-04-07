using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

using UnityEngine.EventSystems;
using System.IO;

public class FeedEventPrefab : MonoBehaviour
{
    [Header("WorldNameAndLinks")]
    public string idOfObject;
    public string m_ThumbnailDownloadURL;
    public string m_FileLink;
    public string uploadTimeStamp;
    public string m_EnvironmentName;
    public static string m_EnvDownloadLink;
    public static string m_EnvName;

    [Header("WorldNameAndDescription")]
    public Text m_WorldName;
    public Text m_WorldNameTH;
    public Text m_WorldDescriptionTxt;
    public Text eviroment_Name;
    public Text creator_Name;

    public string m_BannerLink;

    public Image[] m_BannerSprite;

    [Header("WorldBannerAndDescription")]

    public static string m_BannerLinkParser;
    public string m_WorldDescription;
    public static string m_WorldDescriptionParser;
    public static string m_timestamp;

    [Header("Images")]
    //public Image m_FirstImage;
    public Image m_FadeImage;
    public Image worldIcon;

    public Button m_JoinEventBtn;

    public int m_PressedIndex;
    public static int m_SetPressedIndex;
    public bool isMuseum = false;
    [HideInInspector]
    public bool thumbnailsLoaded;
    

    private void Start()
    {
        m_JoinEventBtn.onClick.AddListener(() => FindObjectOfType<EventList>().JoinEvent());
        Invoke("DownloadPrefabSprite", 0.1f);
        this.GetComponent<Button>().interactable = false;
    }

    void DownloadPrefabSprite()
    {
        if (gameObject.activeInHierarchy && !string.IsNullOrEmpty(m_ThumbnailDownloadURL))
            StartCoroutine(DownloadImage(m_ThumbnailDownloadURL));

    }


    public IEnumerator DownloadImage(string l_imgUrl)
    {
        string folderName;
        if (isMuseum)
        {
            folderName = "museums";
        }
        else
        {
            folderName = "ObjofEnv";
        }
        while (!File.Exists(Application.persistentDataPath + "/MainMenuData/"+folderName+"/" + idOfObject + "/thumbnail.jpg"))
        {
            using (UnityWebRequest www = UnityWebRequest.Get(l_imgUrl))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    yield return null;
                }
                if (www.isHttpError || www.isNetworkError)
                {
                    Debug.LogError("Network Error");
                }
                else
                {
                    byte[] Data = www.downloadHandler.data;
                    File.WriteAllBytes(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + idOfObject + "/" + "thumbnail.jpg", Data);
                    //System.IO.File.WriteAllText(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + "thumbnail.jpg", JsonUtility.ToJson(localObjOfEnv));
                }
                www.Dispose();
            }
            yield return new WaitForEndOfFrame();
        }
        byte[] fileData = File.ReadAllBytes(Application.persistentDataPath + "/MainMenuData/" + folderName + "/" + idOfObject + "/thumbnail.jpg");

        Texture2D texture = new Texture2D(1, 1);

        texture.LoadImage(fileData);
        texture.Compress(true);
        Sprite l_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(1f, 1f));
        
       // gameObject.GetComponent<Image>().sprite = l_sprite;
        //gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = l_sprite;
        worldIcon.sprite= l_sprite;
        m_FadeImage.sprite = l_sprite;
        //eviroment_Name.text = GameManager.Instance.LocalizeTextText(m_EnvironmentName).ToUpper();

        eviroment_Name.GetComponent<TextLocalization>().LocalizeTextText(m_EnvironmentName);
        eviroment_Name.text = eviroment_Name.text.ToUpper();
        gameObject.GetComponent<Button>().interactable = true;
        UpdateWorldPanel();
        GameManager.Instance.worldsTexture.Add(texture);
        yield return null;
    }

    void UpdateWorldPanel()
    {
        m_BannerSprite[0].sprite = m_FadeImage.sprite;
        m_BannerSprite[1].sprite = m_FadeImage.sprite;
        m_BannerSprite[2].sprite = m_FadeImage.sprite;
    }

    public void OnClickPrefab()
    {
        m_EnvDownloadLink = m_FileLink;
        m_EnvName = m_EnvironmentName;
        Launcher.sceneName = m_EnvName;

        m_WorldDescriptionParser = m_WorldDescription;

        m_timestamp = uploadTimeStamp;
        print(m_EnvironmentName);
        print(m_WorldDescription);
        XanaConstants.xanaConstants.EnviornmentName = m_EnvironmentName;
        XanaConstants.xanaConstants.museumDownloadLink = m_EnvDownloadLink;
        XanaConstants.xanaConstants.buttonClicked = this.gameObject;
        if(isMuseum)
        LoginPageManager.m_MuseumIsClicked = true;
        //if(m_EnvName.Contains)
        // m_WorldName.text = GameManager.Instance.LocalizeTextText(m_EnvironmentName);
        //
        //
        // m_WorldNameTH.text = GameManager.Instance.LocalizeTextText(m_EnvName);
        //
        // m_WorldDescriptionTxt.text = GameManager.Instance.LocalizeTextText(m_WorldDescription);

        m_WorldName.GetComponent<TextLocalization>().LocalizeTextText(m_EnvironmentName);
        m_WorldNameTH.GetComponent<TextLocalization>().LocalizeTextText(m_EnvName);
        m_WorldDescriptionTxt.GetComponent<TextLocalization>().LocalizeTextText(m_WorldDescription);

        m_SetPressedIndex = m_PressedIndex;
        if (!isMuseum)
        {
            if (m_EnvName.Contains("Crypto Ninja village"))
            {
                creator_Name.text = "Metaverse Ninja";
            }
            
        }
        else if (isMuseum)
        {
            if (m_EnvName.Contains("THE RHETORIC STAR"))
            {
                creator_Name.text = "World Name";
                creator_Name.GetComponent<TextLocalization>().LocalizeTextText(creator_Name.text);
            }
        }
        else
        {
            if (m_EnvironmentName.Contains("ROCK’N")) {
                PlayerPrefs.SetString("ScenetoLoad", "GekkoSan");
            } else if (m_EnvironmentName.Contains("Gouzu Gallarey"))
            {
                PlayerPrefs.SetString("ScenetoLoad", "GOZMuseum");
            }
            else if (m_EnvironmentName.Contains("Aurora Art"))
            {
                PlayerPrefs.SetString("ScenetoLoad", "Aurora");
            }
            else if (m_EnvironmentName.Contains("Hokusai"))
            {
                PlayerPrefs.SetString("ScenetoLoad", "Hokusai");
            }
            else if (m_EnvironmentName.Contains("Yukinori"))
            {
                PlayerPrefs.SetString("ScenetoLoad", "Yukinori");
            }
            else if (m_EnvironmentName.Contains("NFT Museum"))
            {
                PlayerPrefs.SetString("ScenetoLoad", "THE RHETORIC STAR");
            }
        }
    }
    
}
