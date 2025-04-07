using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static StoreManager;

public class AvatarBtn : MonoBehaviour
{
    public string isBtnString;
    public int _Bodyint;
    public int hairIndex;
    public Image myImage;
    public Sprite myIcon;
    public Text PriceTxt;

    private BodyCustomizationTrigger _BDCTrigger;

    public GameObject LipsCustomizer, EyesCustomizer, NoseCustomizer, EyebrowsCustomizer, FaceCustomizer;

    bool itemAlreadySaved = false;
    bool isExist = false;
    bool isAdded = true;
    SavingCharacterDataClass _CharacterData;

    void Start()
    {
        _BDCTrigger = gameObject.GetComponent<BodyCustomizationTrigger>();
        this.gameObject.GetComponent<Button>().onClick.AddListener(OnAvatarBtnClick);
        PriceTxt.enabled = false;

        _CharacterData = new SavingCharacterDataClass();
    }
    void Update()
    {

    }
    private void OnAvatarBtnClick()
    {
        print("btn name is " + isBtnString);
        
        itemAlreadySaved = false;
        isExist = false;

        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            isExist = true;
        }

        string CurrentString = "";

        switch (isBtnString)
        {
            case "Face":
                {
                    CurrentString = "Face";
                   
                    break;
                }
            case "HeadDefault":
                {
                    CurrentString = "Face";
                   
                    break;
                }
            case "EyeBrow":
                {
                    CurrentString = "Eye Brow";
                    break;
                }
            case "EyeBrowDefault":
                {
                    CurrentString = "Eye Brow";
                    break;
                }
            case "Eyes":
                {
                    CurrentString = "Eyes";
                    break;
                }
            case "EyesDefault":
                {
                    CurrentString = "Eyes";
                    break;
                }

            case "Nose":
                {
                    CurrentString = "Nose";
                    break;
                }
            case "NoseDefault":
                {
                    CurrentString = "Nose";
                    break;
                }
            case "Lips":
                {
                    CurrentString = "Lip";
                    break;
                }     
            
            case "LipsDefault":
                {
                    CurrentString = "Lip";
                    break;
                }
            case "Body":
                {
                    CurrentString = "Body";
                    break;
                }
            case "FaceMorph":
                {
                    CurrentString = "Morphs";
                    break;
                }
            case "EyesMorph":
                {
                    CurrentString = "Morphs";
                    break;
                }
            case "EyeBrowMorph":
                {
                    CurrentString = "Morphs";
                    break;
                }
            case "NoseMorph":
                {
                    CurrentString = "Morphs";
                    break;
                }
            case "LipsMorph":
                {
                    CurrentString = "Morphs";
                    break;
                }
        }

        if (!PremiumUsersDetails.Instance.CheckSpecificItem(CurrentString) && CurrentString != "")
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");

            switch (isBtnString)
            {
                case "Face":
                    {
                        XanaConstants.xanaConstants.faceIndex = GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                        XanaConstants.xanaConstants.isFaceMorphed = false;

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.FacePresets.f_BlendShapeOne == XanaConstants.xanaConstants.faceIndex && !_CharacterData.faceMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;
                    }
                case "EyeBrow":
                    {
                        XanaConstants.xanaConstants.eyeBrowIndex = GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                        XanaConstants.xanaConstants.isEyebrowMorphed = false;

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.EyeBrowPresets.f_BlendShapeOne == XanaConstants.xanaConstants.eyeBrowIndex && !_CharacterData.eyeBrowMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;
                    }

                case "Eyes":
                    {
                        XanaConstants.xanaConstants.eyeIndex = GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                        XanaConstants.xanaConstants.isEyeMorphed = false;

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.EyePresets.f_BlendShapeOne == XanaConstants.xanaConstants.eyeIndex && !_CharacterData.eyeMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;
                    }

                case "Nose":
                    {
                        XanaConstants.xanaConstants.noseIndex = GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                        XanaConstants.xanaConstants.isNoseMorphed = false;

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.NosePresets.f_BlendShapeOne == XanaConstants.xanaConstants.noseIndex && !_CharacterData.noseMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;
                    }
                case "Lips":
                    {
                        XanaConstants.xanaConstants.lipIndex = GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
                        XanaConstants.xanaConstants.isLipMorphed = false;

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.LipsPresets.f_BlendShapeOne == XanaConstants.xanaConstants.lipIndex && !_CharacterData.lipMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;
                    }
                case "Body":
                    {
                        XanaConstants.xanaConstants.bodyNumber = _Bodyint;

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.BodyFat == XanaConstants.xanaConstants.bodyNumber)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;
                    }
                case "FaceMorph":
                    {
                        XanaConstants.xanaConstants.isFaceMorphed = true;
                        break;
                    }
                case "EyesMorph":
                    {
                        XanaConstants.xanaConstants.isEyeMorphed = true;
                        break;
                    }
                case "EyeBrowMorph":
                    {
                        XanaConstants.xanaConstants.isEyebrowMorphed = true;
                        break;
                    }
                case "NoseMorph":
                    {
                        XanaConstants.xanaConstants.isNoseMorphed = true;
                        break;
                    }
                case "LipsMorph":
                    {
                        XanaConstants.xanaConstants.isLipMorphed = true;
                        break;
                    }
            }

            XanaConstants.xanaConstants._curretClickedBtn = this.gameObject;
            if (XanaConstants.xanaConstants._lastClickedBtn.GetComponent<AvatarBtn>() && XanaConstants.xanaConstants._curretClickedBtn.name != XanaConstants.xanaConstants._lastClickedBtn.name)
                XanaConstants.xanaConstants._lastClickedBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            XanaConstants.xanaConstants.avatarStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = gameObject;

            if (XanaConstants.xanaConstants._lastClickedBtn && XanaConstants.xanaConstants._curretClickedBtn == XanaConstants.xanaConstants._lastClickedBtn)
                return;

            XanaConstants.xanaConstants._curretClickedBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            if (XanaConstants.xanaConstants._lastClickedBtn)
            {
                if (XanaConstants.xanaConstants._lastClickedBtn.GetComponent<AvatarBtn>())
                    XanaConstants.xanaConstants._lastClickedBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            }

            XanaConstants.xanaConstants._lastClickedBtn = this.gameObject;

            for (int i = StoreManager.instance.UndoRedoList.Count - 1; i > StoreManager.instance.CurrentIndex; i--)
            {
                StoreManager.instance.UndoRedoList.RemoveAt(i);
            }

            switch (isBtnString)
            {
                case "EyesDefault":
                    {
                        SavaCharacterProperties.instance.SaveItemList.EyePresets.f_BlendShapeOne = 0;

                        XanaConstants.xanaConstants.eyeIndex = 0;
                        XanaConstants.xanaConstants.isEyeMorphed = false;

                        SkinnedMeshRenderer characterHead = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>();

                        for (int i = 0; i < characterHead.sharedMesh.blendShapeCount; i++)
                        {
                            if (characterHead.sharedMesh.GetBlendShapeName(i).Contains("eye"))
                                characterHead.SetBlendShapeWeight(i, 0);
                        }
                        StoreManager.instance.SaveStoreBtn.SetActive(true);
                        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                        StoreManager.instance.GreyRibbonImage.SetActive(false);
                        StoreManager.instance.WhiteRibbonImage.SetActive(true);
                       
                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.EyePresets.f_BlendShapeOne == 0 && !_CharacterData.eyeMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;
                    }
                case "EyeBrowDefault":
                    {
                        SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets.f_BlendShapeOne = 0;

                        XanaConstants.xanaConstants.eyeBrowIndex = 0;
                        XanaConstants.xanaConstants.isEyebrowMorphed = false;

                        SkinnedMeshRenderer characterHead = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>();

                        for (int i = 0; i < characterHead.sharedMesh.blendShapeCount; i++)
                        {
                            if (characterHead.sharedMesh.GetBlendShapeName(i).Contains("EyeBrow"))
                                characterHead.SetBlendShapeWeight(i, 0);
                        }
                        StoreManager.instance.SaveStoreBtn.SetActive(true);
                        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                        StoreManager.instance.GreyRibbonImage.SetActive(false);
                        StoreManager.instance.WhiteRibbonImage.SetActive(true);

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.EyeBrowPresets.f_BlendShapeOne == 0 && !_CharacterData.eyeBrowMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;

                    }
                case "HeadDefault":
                    {
                        SavaCharacterProperties.instance.SaveItemList.FacePresets.f_BlendShapeOne = 0;

                        XanaConstants.xanaConstants.faceIndex = 0;
                        XanaConstants.xanaConstants.isFaceMorphed = false;

                        SkinnedMeshRenderer characterHead = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>();

                        for (int i = 0; i < characterHead.sharedMesh.blendShapeCount; i++)
                        {
                            if (characterHead.sharedMesh.GetBlendShapeName(i).Contains("Head"))
                                characterHead.SetBlendShapeWeight(i, 0);
                        }
                        StoreManager.instance.SaveStoreBtn.SetActive(true);
                        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                        StoreManager.instance.GreyRibbonImage.SetActive(false);
                        StoreManager.instance.WhiteRibbonImage.SetActive(true);

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.FacePresets.f_BlendShapeOne == 0 && !_CharacterData.faceMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;

                    }
                case "LipsDefault":

                    {
                        SavaCharacterProperties.instance.SaveItemList.LipsPresets.f_BlendShapeOne = 0;

                        XanaConstants.xanaConstants.lipIndex = 0;
                        XanaConstants.xanaConstants.isLipMorphed = false;

                        SkinnedMeshRenderer characterHead = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>();

                        for (int i = 0; i < characterHead.sharedMesh.blendShapeCount; i++)
                        {
                            if (characterHead.sharedMesh.GetBlendShapeName(i).Contains("Lips"))
                                characterHead.SetBlendShapeWeight(i, 0);
                        }
                        StoreManager.instance.SaveStoreBtn.SetActive(true);
                        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                        StoreManager.instance.GreyRibbonImage.SetActive(false);
                        StoreManager.instance.WhiteRibbonImage.SetActive(true);

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.LipsPresets.f_BlendShapeOne == 0 && !_CharacterData.lipMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;

                    }
                case "NoseDefault":
                    {
                        SavaCharacterProperties.instance.SaveItemList.NosePresets.f_BlendShapeOne = 0;

                        XanaConstants.xanaConstants.noseIndex = 0;
                        XanaConstants.xanaConstants.isNoseMorphed = false;

                        SkinnedMeshRenderer characterHead = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>();

                        for (int i = 0; i < characterHead.sharedMesh.blendShapeCount; i++)
                        {
                            if (characterHead.sharedMesh.GetBlendShapeName(i).Contains("Nose"))
                                characterHead.SetBlendShapeWeight(i, 0);
                        }
                        StoreManager.instance.SaveStoreBtn.SetActive(true);
                        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                        StoreManager.instance.GreyRibbonImage.SetActive(false);
                        StoreManager.instance.WhiteRibbonImage.SetActive(true);

                        if (isExist)
                        {
                            if (XanaConstants.xanaConstants.isItemChanged)
                                itemAlreadySaved = false;
                            else if (_CharacterData.NosePresets.f_BlendShapeOne == 0 && !_CharacterData.noseMorphed)
                            {
                                itemAlreadySaved = true;
                            }
                        }

                        break;

                    }

                case "Face":
                    {
                        _BDCTrigger.CustomizationTriggerTwo();

                        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                        {
                            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {
                                if (_CharacterData.FacePresets.f_BlendShapeOne != 0)
                                {
                                    UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                    MyPushData01.ClothTex_Item.ItemID = _CharacterData.FacePresets.f_BlendShapeOne;
                                    MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                    MyPushData01.ClothTex_Item.ItemName = "One";
                                    MyPushData01.ClothTex_Item.ItemType = "Preset";
                                    MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
                                    StoreManager.instance.CurrentIndex++;
                                    StoreManager.instance.UndoRedoList.Add(MyPushData01);
                                }

                                else
                                {
                                    isAdded = false;
                                }
                            }

                            else
                            {
                                isAdded = false;
                            }
                        }
                        else
                        {
                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {

                                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                MyPushData01.ClothTex_Item.ItemName = "Zero";
                                MyPushData01.ClothTex_Item.ItemType = "Preset";
                                MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
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
                            UndoRedoDataClass MyPushData9 = new UndoRedoDataClass();
                            MyPushData9.ClothTex_Item.ItemPrefab = this.gameObject;
                            MyPushData9.ClothTex_Item.ItemType = "Preset";
                            MyPushData9.ClothTex_Item.SubCategoryname = isBtnString;
                            StoreManager.instance.UndoRedoList.Add(MyPushData9);
                            StoreManager.instance.CurrentIndex++;
                        }

                        if (StoreManager.instance.UndoBtn)
                            StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;

                        break;
                    }
                case "EyeBrow":
                    {
                        _BDCTrigger.CustomizationTriggerTwo();

                        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                        {
                            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {
                                if (_CharacterData.EyeBrowPresets.f_BlendShapeOne != 0)
                                {
                                    UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                    MyPushData01.ClothTex_Item.ItemID = _CharacterData.EyeBrowPresets.f_BlendShapeOne;
                                    MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                    MyPushData01.ClothTex_Item.ItemName = "One";
                                    MyPushData01.ClothTex_Item.ItemType = "Preset";
                                    MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
                                    StoreManager.instance.CurrentIndex++;
                                    StoreManager.instance.UndoRedoList.Add(MyPushData01);
                                }

                                else
                                {
                                    isAdded = false;
                                }
                            }

                            else
                            {
                                isAdded = false;
                            }
                        }
                        else
                        {
                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {

                                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                MyPushData01.ClothTex_Item.ItemName = "Zero";
                                MyPushData01.ClothTex_Item.ItemType = "Preset";
                                MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
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
                            UndoRedoDataClass MyPushData10 = new UndoRedoDataClass();
                            MyPushData10.ClothTex_Item.ItemPrefab = this.gameObject;
                            MyPushData10.ClothTex_Item.ItemType = "Preset";
                            MyPushData10.ClothTex_Item.SubCategoryname = isBtnString;
                            StoreManager.instance.UndoRedoList.Add(MyPushData10);
                            StoreManager.instance.CurrentIndex++;
                        }

                        if (StoreManager.instance.UndoBtn)
                            StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;

                        break;
                    }
                case "Eyes":
                    {
                        _BDCTrigger.CustomizationTriggerTwo();

                        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                        {
                            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {
                                if (_CharacterData.EyePresets.f_BlendShapeOne != 0)
                                {
                                    UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                    MyPushData01.ClothTex_Item.ItemID = _CharacterData.EyePresets.f_BlendShapeOne;
                                    MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                    MyPushData01.ClothTex_Item.ItemName = "One";
                                    MyPushData01.ClothTex_Item.ItemType = "Preset";
                                    MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
                                    StoreManager.instance.CurrentIndex++;
                                    StoreManager.instance.UndoRedoList.Add(MyPushData01);
                                }

                                else
                                {
                                    isAdded = false;
                                }
                            }

                            else
                            {
                                isAdded = false;
                            }
                        }
                        else
                        {
                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {

                                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                MyPushData01.ClothTex_Item.ItemName = "Zero";
                                MyPushData01.ClothTex_Item.ItemType = "Preset";
                                MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
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
                            UndoRedoDataClass MyPushData11 = new UndoRedoDataClass();
                            MyPushData11.ClothTex_Item.ItemPrefab = this.gameObject;
                            MyPushData11.ClothTex_Item.ItemType = "Preset";
                            MyPushData11.ClothTex_Item.SubCategoryname = isBtnString;
                            StoreManager.instance.UndoRedoList.Add(MyPushData11);
                            StoreManager.instance.CurrentIndex++;
                        }

                        if (StoreManager.instance.UndoBtn)
                            StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;
                        break;
                    }
                case "Nose":
                    {
                        _BDCTrigger.CustomizationTriggerTwo();

                        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                        {
                            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {
                                if (_CharacterData.NosePresets.f_BlendShapeOne != 0)
                                {
                                    UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                    MyPushData01.ClothTex_Item.ItemID = _CharacterData.NosePresets.f_BlendShapeOne;
                                    MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                    MyPushData01.ClothTex_Item.ItemName = "One";
                                    MyPushData01.ClothTex_Item.ItemType = "Preset";
                                    MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
                                    StoreManager.instance.CurrentIndex++;
                                    StoreManager.instance.UndoRedoList.Add(MyPushData01);
                                }
                                else
                                {
                                    isAdded = false;
                                }
                            }
                            else
                            {
                                isAdded = false;
                            }
                        }
                        else
                        {
                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {

                                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                MyPushData01.ClothTex_Item.ItemName = "Zero";
                                MyPushData01.ClothTex_Item.ItemType = "Preset";
                                MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
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
                            UndoRedoDataClass MyPushData12 = new UndoRedoDataClass();
                            MyPushData12.ClothTex_Item.ItemPrefab = this.gameObject;
                            MyPushData12.ClothTex_Item.ItemType = "Preset";
                            MyPushData12.ClothTex_Item.SubCategoryname = isBtnString;
                            StoreManager.instance.UndoRedoList.Add(MyPushData12);
                            StoreManager.instance.CurrentIndex++;
                        }

                        if (StoreManager.instance.UndoBtn)
                            StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;
                        break;
                    }
                case "Lips":
                    {
                        _BDCTrigger.CustomizationTriggerTwo();

                        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                        {
                            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {
                                if (_CharacterData.LipsPresets.f_BlendShapeOne != 0)
                                {
                                    UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                    MyPushData01.ClothTex_Item.ItemID = _CharacterData.LipsPresets.f_BlendShapeOne;
                                    MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                    MyPushData01.ClothTex_Item.ItemType = "Preset";
                                    MyPushData01.ClothTex_Item.ItemName = "One";
                                    MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
                                    StoreManager.instance.CurrentIndex++;
                                    StoreManager.instance.UndoRedoList.Add(MyPushData01);
                                }
                                else
                                {
                                    isAdded = false;
                                }
                            }
                            else
                            {
                                isAdded = false;
                            }
                        }
                        else
                        {
                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {

                                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                MyPushData01.ClothTex_Item.ItemName = "Zero";
                                MyPushData01.ClothTex_Item.ItemType = "Preset";
                                StoreManager.instance.CurrentIndex++;
                                MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
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
                            UndoRedoDataClass MyPushData13 = new UndoRedoDataClass();
                            MyPushData13.ClothTex_Item.ItemPrefab = this.gameObject;
                            MyPushData13.ClothTex_Item.ItemType = "Preset";
                            MyPushData13.ClothTex_Item.SubCategoryname = isBtnString;
                            StoreManager.instance.UndoRedoList.Add(MyPushData13);
                            StoreManager.instance.CurrentIndex++;
                        }

                        if (StoreManager.instance.UndoBtn)
                            StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;
                        break;
                    }

                case "Body":
                    {
                        //StoreManager.instance.BuyStoreBtn.SetActive(false);
                        StoreManager.instance.SaveStoreBtn.SetActive(true);
                        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                        StoreManager.instance.GreyRibbonImage.SetActive(false);
                        StoreManager.instance.WhiteRibbonImage.SetActive(true);
                        StoreManager.instance.ClearBuyItems();
                        CharacterCustomizationManager.Instance.UpdateChBodyShape(_Bodyint);

                        SavaCharacterProperties.instance.SaveItemList.BodyFat = _Bodyint;

                        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                        {
                            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {
                                if (_CharacterData.BodyFat != 0)
                                {
                                    UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                    MyPushData01.ClothTex_Item.ItemID = _CharacterData.BodyFat;
                                    MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                    MyPushData01.ClothTex_Item.ItemType = "BodyFat";
                                    MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
                                    StoreManager.instance.CurrentIndex++;
                                    StoreManager.instance.UndoRedoList.Add(MyPushData01);
                                }

                                else
                                {
                                    isAdded = false;
                                }
                            }

                            else
                            {
                                isAdded = false;
                            }
                        }
                        else
                        {
                            if (StoreManager.instance.CurrentIndex == -1 || StoreManager.instance.CurrentIndex != 0 && StoreManager.instance.UndoRedoList[instance.CurrentIndex].ClothTex_Item.SubCategoryname != isBtnString)
                            {

                                UndoRedoDataClass MyPushData01 = new UndoRedoDataClass();
                                MyPushData01.ClothTex_Item.ItemID = 0;
                                MyPushData01.ClothTex_Item.ItemPrefab = this.gameObject;
                                MyPushData01.ClothTex_Item.ItemType = "BodyFat";
                                MyPushData01.ClothTex_Item.SubCategoryname = isBtnString;
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
                            UndoRedoDataClass MyPushData8 = new UndoRedoDataClass();
                            MyPushData8.ClothTex_Item.ItemID = _Bodyint;
                            MyPushData8.ClothTex_Item.ItemPrefab = this.gameObject;
                            MyPushData8.ClothTex_Item.ItemType = "BodyFat";
                            MyPushData8.ClothTex_Item.SubCategoryname = isBtnString;
                            StoreManager.instance.UndoRedoList.Add(MyPushData8);

                            StoreManager.instance.CurrentIndex++;
                        }

                        if (StoreManager.instance.UndoBtn)
                            StoreManager.instance.UndoBtn.GetComponent<Button>().interactable = true;

                        break;
                    }
                case "Skin":
                    {
                        break;
                    }
                case "FaceMorph":
                    {
                        CharacterCustomizationManager.Instance.OnFrontSide();
                        UIManager.Instance._footerCan.SetActive(false);
                        GameManager.Instance.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                        ChangeCameraForZoomFace.instance.ChangeCameraToIsometric();
                        BlendShapeImporter.Instance.MorphTypeSelected("Head");
                        CharacterCustomizationUIManager.Instance.LoadCustomBlendShapePanel("Face");
                        BlendShapeImporter.Instance.TurnOnPoints();
                        break;
                    }

                case "EyeBrowMorph":
                    {
                        CharacterCustomizationManager.Instance.OnFrontSide();
                        UIManager.Instance._footerCan.SetActive(false);
                        GameManager.Instance.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                        ChangeCameraForZoomFace.instance.ChangeCameraToIsometric();
                        BlendShapeImporter.Instance.MorphTypeSelected("EyeBrow");
                        CharacterCustomizationUIManager.Instance.LoadCustomBlendShapePanel("Eyebrow");
                        BlendShapeImporter.Instance.TurnOnPoints();
                        break;
                    }
                case "EyesMorph":
                    {
                        CharacterCustomizationManager.Instance.OnFrontSide();
                        UIManager.Instance._footerCan.SetActive(false);
                        GameManager.Instance.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                        ChangeCameraForZoomFace.instance.ChangeCameraToIsometric();
                        BlendShapeImporter.Instance.MorphTypeSelected("eye");
                        CharacterCustomizationUIManager.Instance.LoadCustomBlendShapePanel("Eyes");
                        BlendShapeImporter.Instance.TurnOnPoints();
                        break;
                    }
                case "NoseMorph":
                    {
                        CharacterCustomizationManager.Instance.OnFrontSide();
                        UIManager.Instance._footerCan.SetActive(false);
                        GameManager.Instance.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                        ChangeCameraForZoomFace.instance.ChangeCameraToIsometric();
                        BlendShapeImporter.Instance.MorphTypeSelected("Nose");
                        CharacterCustomizationUIManager.Instance.LoadCustomBlendShapePanel("Nose");
                        BlendShapeImporter.Instance.TurnOnPoints();
                        break;
                    }
                case "LipsMorph":
                    {
                        CharacterCustomizationManager.Instance.OnFrontSide();
                        UIManager.Instance._footerCan.SetActive(false);
                        GameManager.Instance.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                        ChangeCameraForZoomFace.instance.ChangeCameraToIsometric();
                        BlendShapeImporter.Instance.MorphTypeSelected("Lips");
                        CharacterCustomizationUIManager.Instance.LoadCustomBlendShapePanel("Lips");
                        BlendShapeImporter.Instance.TurnOnPoints();
                        break;
                    }
            }

            //XanaConstants.xanaConstants._lastClickedBtn = this.gameObject;

            if (!itemAlreadySaved)
            {
                StoreManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = true;
                StoreManager.instance.SaveStoreBtn.SetActive(true);
                StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                StoreManager.instance.GreyRibbonImage.SetActive(false);
                StoreManager.instance.WhiteRibbonImage.SetActive(true);
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
    }
}