using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class SavaCharacterProperties : MonoBehaviour
{
    public static SavaCharacterProperties instance;
    public SavingCharacterDataClass SaveItemList = new SavingCharacterDataClass();
    public FilterBlendShapeSettings _sliderindexes;

    [HideInInspector]
    public static int NeedToShowSplash;
    private void Awake()
    {
        instance = this;
        NeedToShowSplash = 1;

    }
    public void Start()
    {
        //--=-=-=-=-=-=
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            if (_CharacterData.myItemObj.Count != 0)
            {
                if (_CharacterData.myItemObj[5].ItemLinkIOS != null)
                {
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemLinkIOS = _CharacterData.myItemObj[5].ItemLinkIOS;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemName = _CharacterData.myItemObj[5].ItemName;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemID = _CharacterData.myItemObj[5].ItemID;
                }
                if (_CharacterData.myItemObj[4].ItemLinkIOS != null)
                {
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemLinkIOS = _CharacterData.myItemObj[4].ItemLinkIOS;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemName = _CharacterData.myItemObj[4].ItemName;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemID = _CharacterData.myItemObj[4].ItemID;
                }
                if (_CharacterData.myItemObj[6].ItemLinkIOS != null)
                {
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemLinkIOS = _CharacterData.myItemObj[6].ItemLinkIOS;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemName = _CharacterData.myItemObj[6].ItemName;
                    GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemID = _CharacterData.myItemObj[6].ItemID;
                }
            }
        }


        //-=-=-=-=

        SaveItemList.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];

        //-=-=-=-=

        SaveItemList.faceMorphed = false;
        SaveItemList.eyeBrowMorphed = false;
        SaveItemList.eyeMorphed = false;
        SaveItemList.noseMorphed = false;
        SaveItemList.lipMorphed = false;
        //------

        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            print(_CharacterData.myItemObj.Count);
            SaveItemList.myItemObj = _CharacterData.myItemObj;
            SaveItemList.BodyFat = _CharacterData.BodyFat;
            SaveItemList.EyePresets = _CharacterData.EyePresets;
            SaveItemList.NosePresets = _CharacterData.NosePresets;
            SaveItemList.LipsPresets = _CharacterData.LipsPresets;
            SaveItemList.EyeBrowPresets = _CharacterData.EyeBrowPresets;
            SaveItemList.FacePresets = _CharacterData.FacePresets;

            SaveItemList.FaceBlendsShapes = _CharacterData.FaceBlendsShapes;

            SaveItemList.faceMorphed = _CharacterData.faceMorphed;
            SaveItemList.eyeBrowMorphed = _CharacterData.eyeBrowMorphed;
            SaveItemList.eyeMorphed = _CharacterData.eyeMorphed;
            SaveItemList.noseMorphed = _CharacterData.noseMorphed;
            SaveItemList.lipMorphed = _CharacterData.lipMorphed;

        }
        Invoke("AssignSavedPresets", 0.3f);

        AssignCustomSlidersData();
    }
    public void AssignSavedPresets()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (i < _CharacterData.FaceBlendsShapes.Length)  //added condition to handle AR face blendshape.s
                    GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
            }

            CharacterCustomizationManager.Instance.UpdateChBodyShape(_CharacterData.BodyFat);
        }
    }


    public void AssignSavedPresets_Hack()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (i < _CharacterData.FaceBlendsShapes.Length) //added condition to handle AR face blendshape.s
                    GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
            }
            CharacterCustomizationManager.Instance.UpdateChBodyShape(_CharacterData.BodyFat);
        }
    }

    public void AssignCustomSlidersData()
    {

        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            if (_CharacterData.FaceBlendsShapes != null)
                for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
                {
                    if (_sliderindexes.ContainsIndex(i) && i < _CharacterData.FaceBlendsShapes.Length)    //added condition to handle AR face blendshape.s
                    {
                        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
                    }
                }
        }
    }

    public void AssignCustomsliderNewData()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

            SkinnedMeshRenderer smr = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>();

            if (_CharacterData.FaceBlendsShapes != null)
            {
                for (int i = 0; i < smr.sharedMesh.blendShapeCount; i++)
                {
                    if (i < _CharacterData.FaceBlendsShapes.Length)     //added condition to handle AR face blendshape.s
                        _CharacterData.FaceBlendsShapes[i] = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i);
                }
                File.WriteAllText(GameManager.Instance.GetStringFolderPath(), JsonUtility.ToJson(_CharacterData));
                StoreManager.instance.OnSaveBtnClicked();
            }

        }
    }



    public void SavePlayerPropertiesInClassObj()
    {
        SaveItemList.myItemObj.Clear();
        SaveItemList.id = LoadPlayerAvatar.avatarId;
        SaveItemList.name = LoadPlayerAvatar.avatarName;
        SaveItemList.thumbnail = LoadPlayerAvatar.avatarThumbnailUrl;
        SaveItemList.myItemObj.Add(GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[0]);
        SaveItemList.myItemObj.Add(GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[1]);
        SaveItemList.myItemObj.Add(GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[7]);
        SaveItemList.myItemObj.Add(GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[2]);
        SaveItemList.myItemObj.Add(GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture);
        SaveItemList.myItemObj.Add(GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture);
        SaveItemList.myItemObj.Add(GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture);
        StoreManager.instance.GreyRibbonImage.SetActive(true);
        StoreManager.instance.WhiteRibbonImage.SetActive(false);
        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
        for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems.Count; i++)
        {
            switch (GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[i].ItemType)
            {
                case "Legs":
                    SaveItemList.myItemObj[0] = GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[i];
                    break;
                case "Chest":
                    SaveItemList.myItemObj[1] = GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[i];
                    break;
                case "Feet":
                    SaveItemList.myItemObj[2] = GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[i];
                    break;
                case "Hair":
                    SaveItemList.myItemObj[3] = GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems[i];
                    break;
            }
        }
        SaveItemList.myItemObj[4] = GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture;
        SaveItemList.myItemObj[5] = GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture;
        SaveItemList.myItemObj[6] = GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture;


        for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
        {
            if (i < SaveItemList.FaceBlendsShapes.Length) //added condition to handle AR face blendshape.s
                SaveItemList.FaceBlendsShapes[i] = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i);

        }

        SaveItemList.faceMorphed = XanaConstants.xanaConstants.isFaceMorphed;
        SaveItemList.eyeBrowMorphed = XanaConstants.xanaConstants.isEyebrowMorphed;
        SaveItemList.eyeMorphed = XanaConstants.xanaConstants.isEyeMorphed;
        SaveItemList.noseMorphed = XanaConstants.xanaConstants.isNoseMorphed;
        SaveItemList.lipMorphed = XanaConstants.xanaConstants.isLipMorphed;

        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            _CharacterData.id = SaveItemList.id;
            _CharacterData.name = SaveItemList.name;
            _CharacterData.thumbnail = SaveItemList.thumbnail;

            _CharacterData.myItemObj = SaveItemList.myItemObj;
            _CharacterData.BodyFat = SaveItemList.BodyFat;
            //   Debug.LogError()
            _CharacterData.EyePresets = SaveItemList.EyePresets;
            _CharacterData.NosePresets = SaveItemList.NosePresets;
            _CharacterData.LipsPresets = SaveItemList.LipsPresets;
            _CharacterData.EyeBrowPresets = SaveItemList.EyeBrowPresets;
            _CharacterData.FacePresets = SaveItemList.FacePresets;
            _CharacterData.FaceBlendsShapes = SaveItemList.FaceBlendsShapes;
            _CharacterData.faceMorphed = SaveItemList.faceMorphed;
            _CharacterData.eyeBrowMorphed = SaveItemList.eyeBrowMorphed;
            _CharacterData.eyeMorphed = SaveItemList.eyeMorphed;
            _CharacterData.noseMorphed = SaveItemList.noseMorphed;
            _CharacterData.lipMorphed = SaveItemList.lipMorphed;

            string bodyJson = JsonUtility.ToJson(_CharacterData);
            File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
        }
        // IF NOT EXISTS THEN WRITE THE NEW FILE
        else
        {
            SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
            string bodyJson = JsonUtility.ToJson(SaveItemList);
            File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
        }

    }




    public void SavePlayerProperties()
    {
        SavePlayerPropertiesInClassObj();
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
            ServerSIdeCharacterHandling.Instance.CreateUserOccupiedAsset();
        //OVERRIDE THE OLD FILE IF EXISTS
        //if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        //{
        //    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
        //    _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

        //    _CharacterData.name = SaveItemList.name;
        //    _CharacterData.thumbnail = SaveItemList.thumbnail;

        //    _CharacterData.myItemObj = SaveItemList.myItemObj;
        //    _CharacterData.BodyFat = SaveItemList.BodyFat;
        // //   Debug.LogError()
        //    _CharacterData.EyePresets = SaveItemList.EyePresets;
        //    _CharacterData.NosePresets = SaveItemList.NosePresets;
        //    _CharacterData.LipsPresets = SaveItemList.LipsPresets;
        //    _CharacterData.EyeBrowPresets = SaveItemList.EyeBrowPresets;
        //    _CharacterData.FacePresets = SaveItemList.FacePresets;
        //    _CharacterData.FaceBlendsShapes = SaveItemList.FaceBlendsShapes;
        //    _CharacterData.faceMorphed = SaveItemList.faceMorphed;
        //    _CharacterData.eyeBrowMorphed = SaveItemList.eyeBrowMorphed;
        //    _CharacterData.eyeMorphed = SaveItemList.eyeMorphed;
        //    _CharacterData.noseMorphed = SaveItemList.noseMorphed;
        //    _CharacterData.lipMorphed = SaveItemList.lipMorphed;

        //    string bodyJson = JsonUtility.ToJson(_CharacterData);
        //    File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
        //    // if(!UserRegisterationManager.instance.LoggedInAsGuest)
        //    if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        //        ServerSIdeCharacterHandling.Instance.CreateUserOccupiedAsset();
        //    print("Calling data");
        //}
        //// IF NOT EXISTS THEN WRITE THE NEW FILE
        //else
        //{
        //    SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
        //    string bodyJson = JsonUtility.ToJson(SaveItemList);
        //    File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
        //    //  if (!UserRegisterationManager.instance.LoggedInAsGuest)
        //    if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        //        ServerSIdeCharacterHandling.Instance.CreateUserOccupiedAsset();
        //}
    }

    public void CreateFileFortheFirstTime()
    {

        SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
        SubCatString.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
        string bodyJson = JsonUtility.ToJson(SubCatString);

        File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);

    }

    public void SetDefaultData()
    {
        SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
        string bodyJson = JsonUtility.ToJson(SaveItemList);
        File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
    }

    public void LoadMorphsfromFile()
    {
        Start();
    }
    //local file loading
    #region Local
    public void StartLocal()
    {
        SaveItemList.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
        //------


        SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
        //  _CharacterData.FaceBlendsShapes =    new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
        _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));


        SaveItemList.myItemObj = _CharacterData.myItemObj;
        SaveItemList.BodyFat = _CharacterData.BodyFat;
        SaveItemList.EyePresets = _CharacterData.EyePresets;
        SaveItemList.NosePresets = _CharacterData.NosePresets;
        SaveItemList.LipsPresets = _CharacterData.LipsPresets;
        SaveItemList.EyeBrowPresets = _CharacterData.EyeBrowPresets;
        SaveItemList.FacePresets = _CharacterData.FacePresets;
        //   print(_CharacterData.FaceBlendsShapes.Length);
        SaveItemList.FaceBlendsShapes = _CharacterData.FaceBlendsShapes;
        //  Invoke("AssignSavedPresets", 0.3f);

        _CharacterData = new SavingCharacterDataClass();
        _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

        for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
        {
            if (!_sliderindexes.ContainsIndex(i))
                GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, 0);
        }
        if (_CharacterData.EyePresets.f_BlendShapeOne != 0)
            GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(_CharacterData.EyePresets.f_BlendShapeOne, 100f);
        if (_CharacterData.NosePresets.f_BlendShapeOne != 0)
            GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(_CharacterData.NosePresets.f_BlendShapeOne, 100f);
        if (_CharacterData.LipsPresets.f_BlendShapeOne != 0)
            GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(_CharacterData.LipsPresets.f_BlendShapeOne, 100f);
        if (_CharacterData.EyeBrowPresets.f_BlendShapeOne != 0)
            GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(_CharacterData.EyeBrowPresets.f_BlendShapeOne, 100f);
        if (_CharacterData.FacePresets.f_BlendShapeOne != 0)
            GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(_CharacterData.FacePresets.f_BlendShapeOne, 100f);

        CharacterCustomizationManager.Instance.UpdateChBodyShape(_CharacterData.BodyFat);

        // AssignCustomSlidersData();

        //  _CharacterData = new SavingCharacterDataClass();
        //_CharacterData.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
        _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));



        for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
        {
            if (_sliderindexes.ContainsIndex(i))
            {
                //  print(i + " my custom slider index");
                if (i < _CharacterData.FaceBlendsShapes.Length)     //added condition to handle AR face blendshape.s
                    GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
            }
        }



    }




    #endregion





}

[System.Serializable]
public class SavingCharacterDataClass
{
    public string id;
    public string name;
    public string thumbnail;
    public List<Item> myItemObj;
    public BlendShapeDataClass EyePresets = new BlendShapeDataClass();
    public BlendShapeDataClass NosePresets = new BlendShapeDataClass();
    public BlendShapeDataClass LipsPresets = new BlendShapeDataClass();
    public BlendShapeDataClass EyeBrowPresets = new BlendShapeDataClass();
    public BlendShapeDataClass FacePresets = new BlendShapeDataClass();

    public int BodyFat;
    public bool faceMorphed;
    public bool eyeBrowMorphed;
    public bool eyeMorphed;
    public bool noseMorphed;
    public bool lipMorphed;

    public float[] FaceBlendsShapes;

    public SavingCharacterDataClass CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SavingCharacterDataClass>(jsonString);
    }
}


