using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static StoreManager;
using UnityEngine.EventSystems;

public class ItemDetail : MonoBehaviour
{
    public Color NormalColor;
    public Color HighlightedColor;
    public string id;
    public string assetLinkAndroid;
    public string assetLinkIos;
    public string assetLinkWindows;
    public string iconLink;
    public string categoryId;
    public string subCategory;
    public string name;
    public string isPaid;
    public string price;
    public string isPurchased;
    public string isFavourite;
    public string isOccupied;
    public bool isDeleted;
    public string createdBy;
    public string createdAt;
    public string updatedAt;
    public string[] itemTags;
    public Image SelectImg;
    public Text PriceTxt;
    public int MyIndex;
    public bool SelectedBool;
    public Image _iconImg;
    private string _clothetype;
    private string DefaultTempString;
    [HideInInspector]
    public string _downloadableURL;
    public EnumClass.CategoryEnum CategoriesEnumVar;
    public Coroutine runningCoroutine;
    public bool enableUpdate = false;
    public bool completedCoroutine = false, runOnce = false;
    public GameObject loadingSpriteImage;
    public bool firstTimeEnable = false;

    bool itemAlreadySaved = false;
    int saveIndex = -1;
    bool isAdded = true;

    public Equipment equipment;

    public void StartRun()
    {

        if (!runOnce)
        {
            runOnce = true;
            //if (runningCoroutine != null)
            //{
            //    StopCoroutine(runningCoroutine);
            //}

            if (CategoriesEnumVar == EnumClass.CategoryEnum.LipsAvatar || CategoriesEnumVar == EnumClass.CategoryEnum.EyesAvatar || CategoriesEnumVar == EnumClass.CategoryEnum.SkinToneAvatar)

            {
                PriceTxt.gameObject.SetActive(false);
                this.gameObject.GetComponent<Button>().onClick.AddListener(ColorBtnClicked);

                _iconImg.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                _iconImg.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

            }
            else
            {
                this.gameObject.GetComponent<Button>().onClick.AddListener(ItemBtnClicked);
            }
            decimal PriceInDecimal = decimal.Parse(price);
            int priceint = (int)PriceInDecimal;
            PriceTxt.text = priceint.ToString();
            switch (CategoriesEnumVar)
            {
                case EnumClass.CategoryEnum.HairAvatar:
                    _clothetype = "Hair";
                    DefaultTempString = "/MDhairs.";
                    break;
                case EnumClass.CategoryEnum.Shoes:
                    _clothetype = "Feet";
                    DefaultTempString = "/MDshoes.";
                    break;
                case EnumClass.CategoryEnum.Outer:
                    _clothetype = "Chest";
                    DefaultTempString = "/MDshirt.";
                    break;
                case EnumClass.CategoryEnum.Bottom:
                    _clothetype = "Legs";
                    DefaultTempString = "/MDpant.";
                    break;
                case EnumClass.CategoryEnum.LipsAvatar:
                    _clothetype = "Lip";
                    break;
                case EnumClass.CategoryEnum.EyesAvatar:
                    _clothetype = "Eyes";
                    break;
                case EnumClass.CategoryEnum.SkinToneAvatar:
                    _clothetype = "Skin";
                    break;
            }
            isPurchased = "true";

            if (isPaid == "true")
            {
                PriceTxt.text = "Coming Soon";
                this.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        if (iconLink != null)
        {
            if (!completedCoroutine)
            {
                runningCoroutine = StartCoroutine(addsprite(_iconImg, iconLink));
            }
            else
            {
                _iconImg.gameObject.SetActive(true);
            }
        }
    }
    private void OnEnable()
    {
        //_iconImg.gameObject.SetActive(false);
        //if (iconLink != null && !completedCoroutine)
        //{
        //_iconImg.gameObject.SetActive(false);
        if (enableUpdate)
            StartRun();
        //}
        //if(!completedCoroutine)
        //runningCoroutine = StartCoroutine(addsprite(_iconImg, iconLink));
    }

    private void OnDisable()
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        //this.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void UpdateValues()
    {
        decimal PriceInDecimal = decimal.Parse(price);
        int priceint = (int)PriceInDecimal;
        PriceTxt.text = priceint.ToString();
    }

    public IEnumerator addsprite(Image data, string rewview)
    {
        // WHEN IMAGES ARE IN PNG FORMAT
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(rewview))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                tex.Compress(true);
                if (data)   // Check if data is null or not
                    data.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                _iconImg.gameObject.SetActive(true);
                loadingSpriteImage.SetActive(false);
                completedCoroutine = true;
                enableUpdate = false;
            }
            uwr.Dispose();
        }
        yield return null;

    }
    public void DeSelectBtns()
    {
        //  SelectImg.enabled = false;
        this.gameObject.GetComponent<Image>().color = NormalColor;
    }

    public void DeSelectAfterBuying()
    {
        SelectedBool = !SelectedBool;
        if (SelectedBool)
        {
            // SelectImg.enabled = true;
            this.gameObject.GetComponent<Image>().color = HighlightedColor;
        }
        else
        {
            // SelectImg.enabled = false;
            this.gameObject.GetComponent<Image>().color = NormalColor;
        }
        print(SelectedBool);
        if (!SelectedBool)
        {
            StoreManager.instance.GetSelectedBtn(-1, CategoriesEnumVar);
        }
        else
        {
            StoreManager.instance.GetSelectedBtn(MyIndex, CategoriesEnumVar);
        }
    }

    public void ItemBtnClicked()
    {
        print("item clicked " + gameObject.name);
        XanaConstants.xanaConstants.isItemChanged = true;
        print("CategoriesEnumVar : " + CategoriesEnumVar.ToString());

        string CurrentString = "";

        CurrentString = CategoriesEnumVar.ToString();

        switch (CurrentString)
        {
            case "Shoes":
                {
                    CurrentString = "Shoes";
                    break;
                }
            case "Outer":
                {
                    CurrentString = "Shirts/Outer";
                    break;
                }

            case "HairAvatar":
                {
                    CurrentString = "Hairs";
                    break;
                }

            case "Bottom":
                {
                    CurrentString = "Pents /Bottom";
                    break;
                }
            case "EyesAvatar":
                {
                    CurrentString = "eyes";
                    break;
                }
        }

        if (!PremiumUsersDetails.Instance.CheckSpecificItem(CurrentString))
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);

            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");

            itemAlreadySaved = false;

            switch (CurrentString)
            {
                case "Shoes":
                    {
                        XanaConstants.xanaConstants.shoes = id;
                        XanaConstants.xanaConstants.wearableStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 2;

                        break;
                    }
                case "Shirts/Outer":
                    {
                        XanaConstants.xanaConstants.shirt = id;
                        XanaConstants.xanaConstants.wearableStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 1;

                        break;
                    }

                case "Hairs":
                    {
                        XanaConstants.xanaConstants.hair = id;
                        XanaConstants.xanaConstants.avatarStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 3;

                        break;
                    }

                case "Pents /Bottom":
                    {
                        XanaConstants.xanaConstants.pants = id;
                        XanaConstants.xanaConstants.wearableStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 0;

                        break;
                    }
            }

            XanaConstants.xanaConstants._curretClickedBtn = this.gameObject;

            if (XanaConstants.xanaConstants._lastClickedBtn && XanaConstants.xanaConstants._curretClickedBtn == XanaConstants.xanaConstants._lastClickedBtn)
                return;

            XanaConstants.xanaConstants._curretClickedBtn.GetComponent<Image>().enabled = true;

            if (XanaConstants.xanaConstants._lastClickedBtn)
            {
                if (XanaConstants.xanaConstants._lastClickedBtn.GetComponent<ItemDetail>())
                    XanaConstants.xanaConstants._lastClickedBtn.GetComponent<Image>().enabled = false;
            }

            XanaConstants.xanaConstants._lastClickedBtn = this.gameObject;

            if (!completedCoroutine)
                return;
            print("button cliecked");

            for (int i = StoreManager.instance.UndoRedoList.Count - 1; i > StoreManager.instance.CurrentIndex; i--)
            {
                StoreManager.instance.UndoRedoList.RemoveAt(i);
            }

            if (isPurchased == "true")
            {

                print("Purchased");
                StoreManager.instance.checkforSavebutton = true;
                StoreManager.instance.ClearBuyItems();
                StoreManager.instance._DownloadRigClothes.NeedToDownloadOrNot(this, assetLinkAndroid, assetLinkIos, _clothetype, name.ToLower(), int.Parse(id));
                if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                {
                    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                    _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                    if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != subCategory)
                    {
                        for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
                        {
                            if (_CharacterData.myItemObj[i].ItemType == _clothetype && _CharacterData.myItemObj[i].ItemLinkAndroid != "" && _CharacterData.myItemObj[i].ItemLinkIOS != "")
                            {
                                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                MyPushData01.ClothTex_Item.ItemID = int.Parse(id);
                                MyPushData01.ClothTex_Item.ItemName = name;
                                MyPushData01.ClothTex_Item.ItemLinkIOS = _CharacterData.myItemObj[i].ItemLinkIOS;
                                MyPushData01.ClothTex_Item.ItemLinkAndroid = _CharacterData.myItemObj[i].ItemLinkAndroid;
                                MyPushData01.ClothTex_Item.ItemType = _clothetype;
                                MyPushData01.ClothTex_Item.SubCategoryname = subCategory;
                                StoreManager.instance.CurrentIndex++;
                                StoreManager.instance.UndoRedoList.Add(MyPushData01);
                            }

                            else
                            {
                                if (i == _CharacterData.myItemObj.Count - 1)
                                {
                                    isAdded = false;
                                }
                            }
                        }
                    }

                    else
                    {
                        isAdded = false;
                    }
                    if (CharcterBodyParts.instance != null)
                        CharcterBodyParts.instance.DisableBodyPartsCustom();
                }
                else
                {
                    if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != subCategory)
                    {
                        UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                        MyPushData01.ClothTex_Item.ItemID = int.Parse(id);
                        MyPushData01.ClothTex_Item.ItemName = name;
                        MyPushData01.ClothTex_Item.ItemLinkAndroid = DefaultTempString;
                        MyPushData01.ClothTex_Item.ItemLinkIOS = DefaultTempString;
                        MyPushData01.ClothTex_Item.ItemType = _clothetype;
                        MyPushData01.ClothTex_Item.SubCategoryname = subCategory;
                        StoreManager.instance.CurrentIndex++;
                        StoreManager.instance.UndoRedoList.Add(MyPushData01);
                    }
                    else
                    {
                        isAdded = false;
                    }
                }

                if (!isAdded)
                {
                    isAdded = true;
                    UndoRedoDataClass MyPushData4 = new UndoRedoDataClass();
                    MyPushData4.ClothTex_Item.ItemID = int.Parse(id);
                    MyPushData4.ClothTex_Item.ItemName = name;
                    MyPushData4.ClothTex_Item.SubCategoryname = subCategory;
                    MyPushData4.ClothTex_Item.ItemLinkAndroid = assetLinkAndroid;
                    MyPushData4.ClothTex_Item.ItemLinkIOS = assetLinkIos;
                    MyPushData4.ClothTex_Item.ItemType = _clothetype;
                    StoreManager.instance.UndoRedoList.Add(MyPushData4);

                    StoreManager.instance.CurrentIndex++;
                }

                if (StoreManager.instance.UndoBtn)
                    StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;

                if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                {
                    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                    _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                    if (_CharacterData.myItemObj.Count != 0)
                        if (id == _CharacterData.myItemObj[saveIndex].ItemID.ToString())
                            itemAlreadySaved = true;
                }

                if (!itemAlreadySaved)
                {
                    StoreManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = true;
                    SavedButtonClickedBlue();
                }

                else
                {
                    StoreManager.instance.SaveStoreBtn.SetActive(true);
                    StoreManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
                    StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                    StoreManager.instance.GreyRibbonImage.SetActive(true);
                    StoreManager.instance.WhiteRibbonImage.SetActive(false);
                }

            }
            else
            {
                StoreManager.instance.SaveStoreBtn.SetActive(false);
                StoreManager.instance.GreyRibbonImage.SetActive(true);
                StoreManager.instance.WhiteRibbonImage.SetActive(false);
                SelectedBool = !SelectedBool;
                if (SelectedBool)
                {
                    this.gameObject.GetComponent<Image>().color = HighlightedColor;
                    // SelectImg.enabled = true;
                }
                else
                {
                    this.gameObject.GetComponent<Image>().color = NormalColor;
                    // SelectImg.enabled = false;
                }
                print(SelectedBool);
                if (!SelectedBool)
                {
                    StoreManager.instance.GetSelectedBtn(-1, CategoriesEnumVar);
                }
                else
                {
                    StoreManager.instance.GetSelectedBtn(MyIndex, CategoriesEnumVar);
                }
            }
        }
    }

    public void ColorBtnClicked()
    {
        turnOffHighlighter();

        print("CategoriesEnumVar : " + CategoriesEnumVar.ToString());

        string CurrentString = "";

        if (CategoriesEnumVar.ToString() == "SkinToneAvatar")
        {
            CurrentString = "Skin";
        }

        else if (CategoriesEnumVar.ToString() == "EyesAvatar")
        {
            CurrentString = "eyes";
        }

        else if (CategoriesEnumVar.ToString() == "LipsAvatar")
        {
            CurrentString = "Lip";
        }

        if (!PremiumUsersDetails.Instance.CheckSpecificItem(CurrentString))
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);

            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");

            itemAlreadySaved = false;

            if (CategoriesEnumVar.ToString() == "SkinToneAvatar")
            {
                XanaConstants.xanaConstants.skinColor = id;
                XanaConstants.xanaConstants.avatarStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = gameObject;

                saveIndex = 6;
            }

            else if (CategoriesEnumVar.ToString() == "EyesAvatar")
            {
                XanaConstants.xanaConstants.eyeColor = id;
                XanaConstants.xanaConstants.colorSelection[0] = gameObject;

                saveIndex = 4;
            }

            else if (CategoriesEnumVar.ToString() == "LipsAvatar")
            {
                XanaConstants.xanaConstants.lipColor = id;
                XanaConstants.xanaConstants.colorSelection[1] = gameObject;

                saveIndex = 5;
            }

            XanaConstants.xanaConstants._curretClickedBtn = this.gameObject;

            if (XanaConstants.xanaConstants._lastClickedBtn && XanaConstants.xanaConstants._curretClickedBtn == XanaConstants.xanaConstants._lastClickedBtn)
                return;

            XanaConstants.xanaConstants._curretClickedBtn.GetComponent<Image>().enabled = true;

            if (XanaConstants.xanaConstants._lastClickedBtn)
            {
                if (XanaConstants.xanaConstants._lastClickedBtn.GetComponent<ItemDetail>())
                    XanaConstants.xanaConstants._lastClickedBtn.GetComponent<Image>().enabled = false;
            }

            print("in Color Button Clicked Function");
            StoreManager.instance.ClearBuyItems();

            if (!File.Exists(Application.persistentDataPath + "/" + name))
            {
                StartCoroutine(DownloadTextureFile(assetLinkIos));
            }
            else
            {

                applytexture(ReadTextureFromFile(Application.persistentDataPath + "/" + name), assetLinkIos);

            }

            XanaConstants.xanaConstants._lastClickedBtn = this.gameObject;
        }
    }

    IEnumerator DownloadTextureFile(string TextureURL)
    {
        // WHEN IMAGES ARE IN PNG FORMAT
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(TextureURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                // tex.Compress(true);
                byte[] fileData = tex.EncodeToPNG();
                string filePath = Application.persistentDataPath + "/" + name;
                File.WriteAllBytes(filePath, fileData);


            }
            applytexture(ReadTextureFromFile(Application.persistentDataPath + "/" + name), assetLinkIos);

            uwr.Dispose();
        }
        yield return null;
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
        mytexture.Compress(true);
        return mytexture;
    }


    public void applytexture(Texture2D tex, string TextureURL)
    {

        switch (_clothetype)
        {
            case "Lip":
                GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = tex;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemLinkAndroid = TextureURL;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemLinkIOS = TextureURL;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemName = name;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemID = int.Parse(id);
                break;
            case "Eyes":
                GameManager.Instance.EyeballTexture1.material.mainTexture = tex;
                GameManager.Instance.EyeballTexture2.material.mainTexture = tex;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemLinkAndroid = TextureURL;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemLinkIOS = TextureURL;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemName = name;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemID = int.Parse(id);

                break;
            case "Skin":
                GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = tex;
                GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = tex;
                for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
                {
                    if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = tex;
                }
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemLinkIOS = TextureURL;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemLinkAndroid = TextureURL;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemName = name;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemID = int.Parse(id);
                break;
        }

        for (int i = StoreManager.instance.UndoRedoList.Count - 1; i > StoreManager.instance.CurrentIndex; i--)
        {
            StoreManager.instance.UndoRedoList.RemoveAt(i);
        }

        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != subCategory)
            {
                for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
                {
                    if (_CharacterData.myItemObj[i].SubCategoryname == _clothetype && _CharacterData.myItemObj[i].ItemLinkAndroid != "" && _CharacterData.myItemObj[i].ItemLinkIOS != "")
                    {
                        UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                        MyPushData01.ClothTex_Item.ItemID = int.Parse(id);
                        MyPushData01.ClothTex_Item.ItemName = name;
                        MyPushData01.ClothTex_Item.ItemLinkAndroid = _CharacterData.myItemObj[i].ItemLinkIOS;
                        MyPushData01.ClothTex_Item.ItemLinkIOS = _CharacterData.myItemObj[i].ItemLinkIOS;
                        MyPushData01.ClothTex_Item.ItemType = _clothetype;
                        MyPushData01.ClothTex_Item.SubCategoryname = subCategory;
                        StoreManager.instance.CurrentIndex++;
                        StoreManager.instance.UndoRedoList.Add(MyPushData01);

                    }

                    else
                    {
                        if (i == _CharacterData.myItemObj.Count - 1)
                        {
                            isAdded = false;
                        }
                    }
                }
            }

            else
            {
                isAdded = false;
            }
        }
        else
        {
            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != subCategory)
            {
                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                MyPushData01.ClothTex_Item.ItemID = int.Parse(id);
                MyPushData01.ClothTex_Item.ItemName = name;
                MyPushData01.ClothTex_Item.ItemLinkIOS = Application.persistentDataPath + "/" + "D" + _clothetype + "texture.jpg";
                MyPushData01.ClothTex_Item.ItemLinkAndroid = Application.persistentDataPath + "/" + "D" + _clothetype + "texture.jpg";
                MyPushData01.ClothTex_Item.ItemType = _clothetype;
                MyPushData01.ClothTex_Item.SubCategoryname = subCategory;
                StoreManager.instance.CurrentIndex++;
                StoreManager.instance.UndoRedoList.Add(MyPushData01);
            }

            else
            {
                isAdded = false;
            }
        }

        if (!isAdded)
        {
            isAdded = true;
            UndoRedoDataClass MyPushData7 = new UndoRedoDataClass();
            MyPushData7.ClothTex_Item.ItemID = int.Parse(id);
            MyPushData7.ClothTex_Item.ItemName = name;
            MyPushData7.ClothTex_Item.SubCategoryname = subCategory;
            MyPushData7.ClothTex_Item.ItemLinkIOS = assetLinkIos;
            MyPushData7.ClothTex_Item.ItemLinkAndroid = assetLinkIos;
            StoreManager.instance.UndoRedoList.Add(MyPushData7);
            MyPushData7.ClothTex_Item.ItemType = _clothetype;
            StoreManager.instance.CurrentIndex++;
        }

        if (StoreManager.instance.UndoBtn)
            StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;


        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            if (_CharacterData.myItemObj.Count != 0)
                if (id == _CharacterData.myItemObj[saveIndex].ItemID.ToString())
                    itemAlreadySaved = true;
        }

        if (!itemAlreadySaved)
        {
            StoreManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = true;
            SavedButtonClickedBlue();
        }

        else
        {
            StoreManager.instance.SaveStoreBtn.SetActive(true);
            StoreManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
            StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            StoreManager.instance.GreyRibbonImage.SetActive(true);
            StoreManager.instance.WhiteRibbonImage.SetActive(false);
        }

    }
    void SavedButtonClickedBlue()
    {
        StoreManager.instance.SaveStoreBtn.SetActive(true);
        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
        StoreManager.instance.GreyRibbonImage.SetActive(false);
        StoreManager.instance.WhiteRibbonImage.SetActive(true);
    }

    void turnOffHighlighter()
    {
        Transform temp = EventSystem.current.currentSelectedGameObject.transform.parent;

        foreach (Transform item in temp.transform)
        {
            item.GetComponent<Image>().enabled = false;
        }
    }
}
