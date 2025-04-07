﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.IO;

public class FeedAnimationController : MonoBehaviour
{
    public AnimationDetails bean;
    [Space]
    public GameObject playerAvatar;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public GameObject contentCategoryPanel;
    public GameObject categoryPrefab;
    public List<Button> buttonList = new List<Button>();

    public List<string> animationGroup = new List<string>();
    // Start is called before the first frame update

    IEnumerator Start()
    {
        StartCoroutine(FindPlayerAvatar());
        yield return new WaitForSeconds(.5f);
        StartCoroutine(GetAllAnimations());
    }

    IEnumerator FindPlayerAvatar()
    {
        while (playerAvatar == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
                playerAvatar = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
            yield return null;
        }
    }

    // Update is called once per frame
    public void SetCategoryButtonHighLight(int index)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (index == i)
            {
                // if (buttonList[i].transform.GetChild(0).GetChild(1))
                buttonList[i].transform.GetChild(0).GetChild(buttonList[i].transform.GetChild(0).childCount - 1).gameObject.SetActive(true);
                buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
                string groupName = buttonList[i].name;
                for (int j = 0; j < ContentPanel.transform.childCount; j++)
                {
                    if (groupName == "All")
                    {
                        ContentPanel.transform.GetChild(j).gameObject.SetActive(true);
                    }
                    else
                    {
                        if (ContentPanel.transform.GetChild(j).name == groupName)
                        {
                            ContentPanel.transform.GetChild(j).gameObject.SetActive(true);
                        }
                        else
                        {
                            ContentPanel.transform.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                //if (buttonList[i].transform.GetChild(0).GetChild(1))
                buttonList[i].transform.GetChild(0).GetChild(buttonList[i].transform.GetChild(0).childCount - 1).gameObject.SetActive(false);
                buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(115, 115, 115, 255);
            }
        }
    }

    public void SetAnimationHighlight(GameObject currentAnimButton)
    {
        for (int i = 0; i < ContentPanel.transform.childCount; i++)
        {
            ContentPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        currentAnimButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator GetAllAnimations()
    {
        Debug.Log("All Anim");
        UnityWebRequest uwr = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.ANIMATIONFILES+"/"+APIBaseUrlChange.instance.apiversion);
        try
        {
            if (UserRegisterationManager.instance.LoggedInAsGuest)
            {
                uwr.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            }
            else
            {
                uwr.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
            }
        }
        catch (Exception e1)
        {
            Debug.Log(e1);
        }

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            try
            {
                Debug.Log("Response call===" + uwr.downloadHandler.text.ToString().Trim());
                bean = Gods.DeserializeJSON<AnimationDetails>(uwr.downloadHandler.text.ToString().Trim());
                if (!bean.Equals("") || !bean.Equals(null) || bean.msg.Equals("Animations get successfully"))
                {
                    for (int i = 0; i < bean.data.animationList.Count; i++)
                    {
                        GameObject animObject;
                        animObject = Instantiate(ListItemPrefab);
                        animObject.transform.SetParent(ContentPanel.transform);
                        animObject.transform.localPosition = Vector3.zero;
                        animObject.transform.localScale = Vector3.one;
                        animObject.transform.localRotation = Quaternion.identity;
                        animObject.transform.GetChild(1).gameObject.SetActive(false);
                        animObject.name = bean.data.animationList[i].group;

                        if (!animationGroup.Contains(bean.data.animationList[i].group))
                            animationGroup.Add(bean.data.animationList[i].group);

                        StartCoroutine(LoadSpriteEnv(bean.data.animationList[i].thumbnail, animObject.transform.GetChild(0).gameObject, i));
                        //animObject.GetComponent<Button>().onClick.AddListener(() => SetAnimationHighlight(animObject));
#if UNITY_ANDROID
                        string url = bean.data.animationList[i].android_file;
#endif
#if UNITY_IOS
                        string url = bean.data.animationList[i].ios_file;
#endif
                        string bundleName = bean.data.animationList[i].name;
                        animObject.GetComponent<Button>().onClick.AddListener(() => Load(url, bundleName, animObject));
                    }

                    for (int i = 0; i < buttonList.Count; i++)//riken.......
                    {
                        buttonList[i].transform.GetChild(0).gameObject.SetActive(true);
                    }
                    for (int i = 0; i < animationGroup.Count; i++)
                    {
                        GameObject categoryObject;
                        categoryObject = Instantiate(categoryPrefab);
                        categoryObject.transform.SetParent(contentCategoryPanel.transform);
                        categoryObject.transform.localPosition = Vector3.zero;
                        categoryObject.transform.localScale = Vector3.one;
                        categoryObject.transform.localRotation = Quaternion.identity;
                        categoryObject.name = animationGroup[i];
                        categoryObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TextLocalization.GetLocaliseTextByKey(animationGroup[i]);
                        categoryObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(115, 115, 115, 255);
                        categoryObject.transform.GetChild(0).gameObject.SetActive(true);
                        int x = i + 1;
                        categoryObject.transform.GetComponent<Button>().onClick.AddListener(() => SetCategoryButtonHighLight(x));
                        buttonList.Add(categoryObject.GetComponent<Button>());
                    }
                }
                else
                {
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    IEnumerator LoadSpriteEnv(string ImageUrl, GameObject thumbnail, int i)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
        }
        else
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
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                }
                else
                {
                    Sprite sprite = Sprite.Create(thumbnailTexture, new Rect(0, 0, thumbnailTexture.width, thumbnailTexture.height), new Vector2(0, 0));
                    if (thumbnail != null)
                    {
                        thumbnail.GetComponent<Image>().sprite = sprite;
                        //Loader.SetActive(false);
                    }
                    else
                    {
                        // Loader.SetActive(false);
                    }
                }
                www.Dispose();
            }            
        }
    }

    bool alreadyRuning = true;
    public void Load(string url, string bundleName, GameObject _gameObject)
    {
        if (alreadyRuning)
        {
            alreadyRuning = false;

            string bundlePath = Path.Combine(XanaConstants.xanaConstants.r_EmoteStoragePersistentPath, bundleName + ".unity3d");

            if (CheckForIsAssetBundleAvailable(bundlePath))
            {
                StartCoroutine(LoadAssetBundleFromStorage(bundlePath, _gameObject));
            }
            else
            {
                StartCoroutine(GetAssetBundleFromServerUrl(url, bundlePath, _gameObject));
            }
            //StartCoroutine(GetAssetBundleFromServerUrl(url, _gameObject));
            SetAnimationHighlight(_gameObject);
        }
    }

    int counter = 0;
    //Comment Step4 Load Assets Bundle Animator from URL
    IEnumerator GetAssetBundleFromServerUrl(string BundleURL, string bundlePath, GameObject currentButton)
    {
        if (counter > 4)
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            Caching.ClearCache();
            GC.Collect();
            counter = 0;
        }
        Debug.LogError("GetAssetBundleFromServerUrl:" + bundlePath);
        using (WWW www = new WWW(BundleURL))
        {
            currentButton.transform.GetChild(2).gameObject.SetActive(true);
            while (!www.isDone)
            {
                yield return null;
            }
            currentButton.transform.GetChild(2).gameObject.SetActive(false);
            yield return www;
            if (www.error != null)
            {
                throw new Exception("WWW download had an error:" + www.error);
            }
            else
            {
                AssetBundle assetBundle = www.assetBundle;
                if (assetBundle != null)
                {
                    GameObject[] animation = assetBundle.LoadAllAssets<GameObject>();
                    foreach (var go in animation)
                    {
                        if (go.name.Equals("Animation"))
                        {
                            playerAvatar.GetComponent<Animator>().runtimeAnimatorController = go.GetComponent<Animator>().runtimeAnimatorController;
                            playerAvatar.GetComponent<Animator>().Play("Animation");
                        }
                    }
                    Debug.LogError("bundle success download save to storage");
                    SaveAssetBundle(www.bytes, bundlePath);
                    assetBundle.Unload(false);
                }
            }
        }
        counter++;
        alreadyRuning = true;
    }

    //load asset bundle from storage.......#Riken
    public IEnumerator LoadAssetBundleFromStorage(string bundlePath, GameObject currentButton)
    {
        if (counter > 4)
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            Caching.ClearCache();
            GC.Collect();
            counter = 0;
        }

        Debug.LogError("LoadAssetBundleFromStorage:" + bundlePath);
        currentButton.transform.GetChild(2).gameObject.SetActive(true);

        AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundle;

        AssetBundle assetBundle = bundle.assetBundle;
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        if (assetBundle != null)
        {
            AssetBundleRequest newRequest = assetBundle.LoadAllAssetsAsync<GameObject>();
            while (!newRequest.isDone)
            {
                yield return null;
            }
            if (newRequest.isDone)
            {
                Debug.LogError("Success load bundle from storage");
                var animation = newRequest.allAssets;
                foreach (var anim in animation)
                {
                    GameObject go = (GameObject)anim;
                    if (go.name.Equals("Animation"))
                    {
                        playerAvatar.GetComponent<Animator>().runtimeAnimatorController = go.GetComponent<Animator>().runtimeAnimatorController;
                        playerAvatar.GetComponent<Animator>().Play("Animation");
                    }
                }
            }
            assetBundle.Unload(false);
        }

        currentButton.transform.GetChild(2).gameObject.SetActive(false);

        counter++;
        alreadyRuning = true;
    }

    public void SaveAssetBundle(byte[] data, string path)
    {
        //Create the Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, data);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    public bool CheckForIsAssetBundleAvailable(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //End #Riken

    //GetAllAnimations
    [System.Serializable]
    public class AnimationList
    {
        public int id;
        public string name;
        public string group;
        public string thumbnail;
        public string android_file;
        public string ios_file;
        public string description;
        public DateTime createdAt;
        public DateTime updatedAt;
    }

    [System.Serializable]
    public class Data
    {
        public List<AnimationList> animationList;
    }

    [System.Serializable]
    public class AnimationDetails
    {
        public bool success;
        public Data data;
        public string msg;
    }
}