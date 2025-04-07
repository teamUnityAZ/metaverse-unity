using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

public class DownloadandRigClothes : MonoBehaviour
{
    [HideInInspector]
    public AssetBundle bundle;
    public AssetBundleRequest newRequest;
    [SerializeField]
    public GameObject load;
    public EquipUI ui;

    Equipment _Myequipment;
    string prev_shirt, prev_pant, prev_shoes, prev_hairs;
    string current_shirt, current_pant, current_shoes;
    bool NotNeedtoDownload = true;
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            ui = GameManager.Instance.EquipUiObj;
        }
        // Invoke("checkIsFalse", 5f);
    }
    public void NeedToDownloadOrNot(ItemDetail _itemDetailObj, string androidpath, string iospath, string type, string clothname, int ItemId)
    {
        NotNeedtoDownload = true;
        switch (type)
        {
            case "Eyes":
                if (File.Exists(Application.persistentDataPath + "/" + clothname))
                {

                    BindExistingClothes(type, clothname, _itemDetailObj);
                }
                else
                {
                    StartCoroutine(DownloadAssetBundle(_itemDetailObj, type, clothname, androidpath, iospath));
                }
                break;
            case "Lip":
                if (File.Exists(Application.persistentDataPath + "/" + clothname))
                {
                    BindExistingClothes(type, clothname);
                }
                else
                {
                    StartCoroutine(DownloadAssetBundle(_itemDetailObj, type, clothname, androidpath, iospath));
                }
                break;
            case "Skin":
                if (File.Exists(Application.persistentDataPath + "/" + clothname))
                {
                    BindExistingClothes(type, clothname);
                }
                else
                {
                    StartCoroutine(DownloadAssetBundle(_itemDetailObj, type, clothname, androidpath, iospath));
                }
                break;
            default:
                for (int j = 0; j < ItemDatabase.instance.itemList.Count; j++)
                {
                    if (clothname == ItemDatabase.instance.itemList[j].Slug.ToLower())
                    {
                        NotNeedtoDownload = false;
                    }
                    //}
                    if (!NotNeedtoDownload)
                    {
                        BindExistingClothes(type, clothname);
                        return;
                    }
                }
                if (!File.Exists(Application.persistentDataPath + "/" + clothname))
                {
                    StartCoroutine(DownloadAssetBundle(_itemDetailObj, type, clothname, androidpath, iospath));
                }
                else
                    LoadFile(_itemDetailObj, type, clothname, androidpath, iospath);

                break;
        }
    }
    public void BindExistingClothes(string type, string clothname, ItemDetail _itemObj = null)
    {
      //  print(clothname);

        if (!load.activeSelf)
        {
            load.SetActive(true);
            Invoke("LoadingBar", 1.0f);
        }
        Texture2D mytexture;
        string filePath = Application.persistentDataPath + "/" + clothname;

        if (!string.IsNullOrEmpty(clothname))
            switch (type)
            {
                case "Legs":
                    ui.AddOrRemoveClothes("naked_legs", "Legs", clothname, 0);
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                      GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
                    break;
                case "Chest":
                    ui.AddOrRemoveClothes("naked_chest", "Chest", clothname, 1);
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                         GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
                    break;
                case "Feet":
                    ui.AddOrRemoveClothes("naked_slug", "Feet", clothname, 7);
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                     GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
                    break;
                case "Hair":
                    ui.AddOrRemoveClothes("bald_head", "Hair", clothname, 2);
                    break;
                case "Eyes":
                    mytexture = ReadTextureFromFile(filePath);
                    GameManager.Instance.EyeballTexture1.material.mainTexture = mytexture;
                    GameManager.Instance.EyeballTexture2.material.mainTexture = mytexture;
                    //------------------------------
                    break;

                case "Lip":
                    mytexture = ReadTextureFromFile(filePath);
                    GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = mytexture;
                    break;

                case "Skin":
                    mytexture = ReadTextureFromFile(filePath);
                    GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = mytexture;
                    for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
                    {
                        if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                            GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = mytexture;
                        if (_itemObj != null)
                        {
                            GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemID = int.Parse(_itemObj.id);
                        }
                    }
                    break;
            }
    }

    public Texture2D ReadTextureFromFile(string path)
    {
        byte[] _bytes;
        Texture2D mytexture;

        _bytes = File.ReadAllBytes(path);
        if (_bytes == null)
            return null;
        mytexture = new Texture2D(1, 1, TextureFormat.DXT5, false);
        mytexture.LoadImage(_bytes);
        return mytexture;
    }
    public void DownloadStoreItems(ItemDetail _itemObj, string AssetType)
    {
        //    if (!LoadFile(_itemObj, AssetType))
        //   {
        //  StartCoroutine(DownloadAssetBundle(_itemObj, AssetType ));
        // }
    }
    public IEnumerator DownloadAssetBundle(ItemDetail _itemObj, string type, string tempname, string androidpath, string iospath)
    {
        switch (type)
        {
            case "Lip":
            case "Eyes":
            case "Skin":
                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_itemObj.assetLinkIos))
                {
                    uwr.SendWebRequest();
                    while(!uwr.isDone)
                    {
                        yield return null;
                    }
                    if (uwr.isNetworkError)
                    {
                        Debug.Log(uwr.error);
                    }
                    else
                    {
                        Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                        byte[] fileData = tex.EncodeToPNG();
                        string filePath = Application.persistentDataPath + "/" + tempname;
                        File.WriteAllBytes(filePath, fileData);
                        uwr.Dispose();
                        yield return new WaitForSeconds(2);
                        if (!LoadFile(_itemObj, type, tempname, androidpath, iospath))
                        {

                        }
                    }
                }
                break;

            default:

                string path;

#if UNITY_ANDROID
                path = androidpath;
#else
  path = iospath;
#endif


                if (path == prev_pant || path == prev_shirt || path == prev_shoes || path == prev_hairs)
                    yield break;
                load.SetActive(true);

                using (UnityWebRequest uwr = UnityWebRequest.Get(path))
                {
                    var operation = uwr.SendWebRequest();
                    while (!uwr.isDone)
                    {
                        yield return null;
                    }

                    if (uwr.isHttpError || uwr.isNetworkError)
                    {
                        load.SetActive(false);
                        Debug.LogError("Network Error");
                    }
                    else
                    {
                        byte[] fileData = uwr.downloadHandler.data;
                        string filePath = Application.persistentDataPath + "/" + tempname;
                        File.WriteAllBytes(filePath, fileData);
                        uwr.Dispose();
                        if (operation.isDone)
                        {
                            LoadFile(_itemObj, type, tempname, androidpath, iospath);
                        }
                    }
                }
                break;
        }

        yield return null;
    }
    bool LoadFile(ItemDetail _itemObj, string _AssetType, string tempname, string androidpath, string iospath)
    {
        Texture2D mytexture;
        string filePath = Application.persistentDataPath + "/" + tempname;
        if (!File.Exists(filePath))
        {
            return false;
        }
        switch (_AssetType)
        {
            case "Eyes":
                mytexture = ReadTextureFromFile(filePath);
                GameManager.Instance.EyeballTexture1.material.mainTexture = mytexture;
                GameManager.Instance.EyeballTexture2.material.mainTexture = mytexture;

                return true;

            case "Lip":
                mytexture = ReadTextureFromFile(filePath);
                GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = mytexture;
                return true;

            case "Skin":
                mytexture = ReadTextureFromFile(filePath);
                GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = mytexture;
                for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
                {
                    if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = mytexture;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemID = int.Parse(_itemObj.id);
                }
                return true;
            default:
                var myLoadedAssetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + tempname);
                if (myLoadedAssetBundle == null)
                {
                    Debug.Log("Failed to load AssetBundle!");
                    load.SetActive(false);
                    return false;
                }
                var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(myLoadedAssetBundle.name) as GameObject;
                BindDownloadedCloths(prefab, _AssetType, _itemObj, tempname, myLoadedAssetBundle, androidpath, iospath);
                myLoadedAssetBundle.Unload(false);
                return true;
        }
    }
    bool chek;

    void LoadingBar()
    {
        load.SetActive(false);
    }
    void BindDownloadedCloths(GameObject DownloadedBundle, string type, ItemDetail _itemDetailObj, string tempname, AssetBundle myBundle, string androidpath, string iospath)
    {
        load.SetActive(true);
      //  print("+++++" + androidpath);
        Invoke("LoadingBar",1f);
        if (DownloadedBundle != null)
        {

            string path;

#if UNITY_ANDROID
            path = androidpath;
#else
  path = iospath;
#endif


            ItemDatabase.instance.itemList.Add(new Item(int.Parse(_itemDetailObj.id), tempname, "", tempname, type, DownloadedBundle, androidpath, iospath, type));
            ui.ChangeCostume(tempname);
            switch (type)
            {
                case "Legs":
                    prev_pant = path;
                    current_pant = tempname;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                      GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
                    break;
                case "Chest":
                    prev_shirt = path;
                    current_shirt = tempname;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                         GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
                    break;
                case "Feet":
                    current_shoes = tempname;
                    prev_shoes = path;

                    break;
                case "Hair":
                    current_shoes = tempname;
                    prev_hairs = path;
                    break;
            }



          //  yield return new WaitForSeconds(1.5f);
           // load.SetActive(false);
            if (StoreManager.instance.checkforSavebutton)
            {
                StoreManager.instance.checkforSavebutton = false;
                StoreManager.instance.SaveStoreBtn.SetActive(true);
                StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                StoreManager.instance.GreyRibbonImage.SetActive(false);
                StoreManager.instance.WhiteRibbonImage.SetActive(true);
            }
        }

    }

//    void BindDownloadedCloths(GameObject DownloadedBundle, string type, ItemDetail _itemDetailObj, string tempname, AssetBundle myBundle, string androidpath,string iospath)
//    {
//        if (DownloadedBundle != null)
//        {

//            string path;

//#if UNITY_ANDROID
//            path = androidpath;
//#else
//  path = iospath;
//#endif


//            ItemDatabase.instance.itemList.Add(new Item(int.Parse(_itemDetailObj.id), tempname, "", tempname, type, DownloadedBundle, androidpath,iospath, type));
//            ui.ChangeCostume(tempname);
//            switch (type)
//            {
//                case "Legs":                   
//                    prev_pant = path;
//                    current_pant = tempname;
//                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
//                      GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
//                    break;
//                case "Chest":
//                    prev_shirt = path;
//                    current_shirt = tempname;
//                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
//                         GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
//                    break;
//                case "Feet":
//                    current_shoes = tempname;
//                    prev_shoes = path;

//                    break;
//                case "Hair":
//                    current_shoes = tempname;
//                    prev_hairs = path;
//                    break;
//            }
//            load.SetActive(false);
//            if (StoreManager.instance.checkforSavebutton)
//            {
//                StoreManager.instance.checkforSavebutton = false;
//                StoreManager.instance.SaveStoreBtn.SetActive(true);
//                StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
//                StoreManager.instance.GreyRibbonImage.SetActive(false);
//                StoreManager.instance.WhiteRibbonImage.SetActive(true);
//            }
//        }

//    }
}      


