using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class loadBundle : MonoBehaviour
{
    // Start is called before the first frame update

    public AssetBundle bundle;
    [SerializeField]
    GameObject load;
    public EquipUI ui;
    // public List<Item> assestdata;
    public AssetBundleRequest newRequest;
    Equipment _Myequipment;
    string prev_shirt, prev_pant, prev_shoes;
    string current_shirt, current_pant, current_shoes;
    private void Awake()
    {

    }
    public void set_previous()
    {
        if (PlayerPrefs.GetString("player pant") != "" && PlayerPrefs.GetString("player shirt") != "MDpants")
        {
            FindObjectOfType<ChangeGear>().UnequipItem("Legs", FindObjectOfType<Equipment>().wornLegs.gameObject.name);
            FindObjectOfType<ChangeGear>().EquipItem("Legs", PlayerPrefs.GetString("player pant"));
        }
        else
        {
            FindObjectOfType<ChangeGear>().UnequipItem("Legs", FindObjectOfType<Equipment>().wornLegs.gameObject.name);
            FindObjectOfType<ChangeGear>().EquipItem("Legs", "MDpants");
        }
        if (PlayerPrefs.GetString("player shirt") != "" && PlayerPrefs.GetString("player shirt") != "MDgambeson")
        {
            FindObjectOfType<ChangeGear>().UnequipItem("Chest", FindObjectOfType<Equipment>().wornChest.gameObject.name);
            FindObjectOfType<ChangeGear>().EquipItem("Chest", PlayerPrefs.GetString("player shirt"));
        }
        else
        {
            FindObjectOfType<ChangeGear>().UnequipItem("Chest", FindObjectOfType<Equipment>().wornChest.gameObject.name);
            FindObjectOfType<ChangeGear>().EquipItem("Chest", "MDgambeson");

        }
    }

    public void setstart()
    {
        if (PlayerPrefs.GetString("player pant") != "" && PlayerPrefs.GetString("player shirt") != "MDpants")
        {
            FindObjectOfType<ChangeGear>().EquipItem("Legs", PlayerPrefs.GetString("player pant"));
        }
        else
        {
            FindObjectOfType<ChangeGear>().EquipItem("Legs", "MDpants");
        }
        if (PlayerPrefs.GetString("player shirt") != "" && PlayerPrefs.GetString("player shirt") != "MDgambeson")
        {
            FindObjectOfType<ChangeGear>().EquipItem("Chest", PlayerPrefs.GetString("player shirt"));
        }
        else
        {
            FindObjectOfType<ChangeGear>().EquipItem("Chest", "MDgambeson");

        }
    }
    public void save()
    {
        PlayerPrefs.SetString("player pant", current_pant);
        PlayerPrefs.SetString("player shirt", current_shirt);
        PlayerPrefs.SetString("player shoes", current_shoes);
    }
    //  Text txt;
    void Start()
    {
        _Myequipment = GameManager.Instance.mainCharacter.GetComponent<Equipment>();
        // set_previous();
        Invoke("setstart", 1.5f);
    }
    public IEnumerator timeout()
    {
        yield return new WaitForSeconds(15);

        load.SetActive(false);
        StopAllCoroutines();
    }

    [Obsolete]
    public IEnumerator GetAssetBundle(string path, string type)
    {
        if (path == prev_pant || path == prev_shirt || path == prev_shoes)
            yield break;

        load.SetActive(true);
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle

                bundle = DownloadHandlerAssetBundle.GetContent(uwr);

                Debug.Log("Sucess");

                if (bundle != null)
                {
                    newRequest = bundle.LoadAllAssetsAsync<GameObject>();
                    while (!newRequest.isDone)
                    {
                        Debug.Log(newRequest.progress);
                        yield return null;
                    }
                    if (newRequest.isDone)
                    {
                        Debug.Log("Sucess" + newRequest.allAssets.Length);
                        UnityEngine.Object[] obj = newRequest.allAssets;

                        for (int j = 0; j < obj.Length; j++)
                        {
                            switch (type)
                            {
                                case "SkirtsSelection":
                                  ItemDatabase.instance.itemList.Add(new Item(UnityEngine.Random.Range(0, 1000), newRequest.allAssets[j].name, "", newRequest.allAssets[j].name, "Legs", obj[j] as GameObject ,"","",""));
                                    ui.AddOrRemoveClothes("naked_legs", "Legs", newRequest.allAssets[j].name, 0);
                                    prev_pant = path;
                                    current_pant = newRequest.allAssets[j].name;
                                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                                      GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));


                                    StartCoroutine(StoreinLocal(path, newRequest.allAssets[j].name));

                                    break;
                                case "CostumeSelection":
                                    ItemDatabase.instance.itemList.Add(new Item(UnityEngine.Random.Range(0, 1000), newRequest.allAssets[j].name, "", newRequest.allAssets[j].name, "Chest", obj[j] as GameObject, "","", ""));
                                    ui.AddOrRemoveClothes("naked_chest", "Chest", newRequest.allAssets[j].name, 1);
                                    prev_shirt = path;
                                    current_shirt = newRequest.allAssets[j].name;
                                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                                        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));

                                    StartCoroutine(StoreinLocal(path, newRequest.allAssets[j].name));

                                    break;
                                case "ShoesSelection":
                                    ItemDatabase.instance.itemList.Add(new Item(UnityEngine.Random.Range(0, 1000), newRequest.allAssets[j].name, "", newRequest.allAssets[j].name, "Feet", obj[j] as GameObject,"", "", ""));
                                    ui.AddOrRemoveClothes("naked_slug", "Feet", newRequest.allAssets[j].name, 7);
                                    current_shoes = newRequest.allAssets[j].name;
                                    prev_shoes = path;
                                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                                     GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));

                                    StartCoroutine(StoreinLocal(path, newRequest.allAssets[j].name));
                                    break;
                            }

                        }
                    }
                    if (bundle != null)
                        bundle.Unload(false);
                    load.SetActive(false);
                    //   Invoke("RemoveExtra",2f);
                }
                else { Debug.Log("null"); }
            }
        }
        yield return null;

    }
    public IEnumerator StoreinLocal(string _path, string _itemName)
    {
        WWW _www = new WWW(_path);
        yield return _www;
        var bytes = _www.bytes;
        string _locationtostore = Application.persistentDataPath + "/" + _itemName;
        File.WriteAllBytes(_locationtostore, bytes);
    }
    public void RemoveExtra()
    {
        for (int i = 0; i < ItemDatabase.instance.itemList.Count; i++)
        {
            if (ItemDatabase.instance.itemList[i].ItemType == "Legs" && ItemDatabase.instance.itemList[i].ItemName != _Myequipment.wornLegs.name)
            {
                ItemDatabase.instance.itemList.RemoveAt(i);
            }
        }
    }
    public Item FetchItemBySlug(string slugName)
    {
        for (int i = 0; i < ItemDatabase.instance.itemList.Count; i++)
        {

            if (ItemDatabase.instance.itemList[i].Slug == slugName)
            {
                return ItemDatabase.instance.itemList[i];
            }
        }
        return null;
    }
    public void clearData()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo asset in info)
        {
            if (asset.Name == current_pant || asset.Name == current_shirt || asset.Name == current_shoes)
            {
                continue;
            }
            Debug.LogError("Deleted " + asset.Name);
            asset.Delete();

        }
    }
    public GameObject TextureandMeshwithDownloaded(string _assetname)
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, _assetname));

        GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>(_assetname);

        return prefab;
    }
}
