using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class Equipment : MonoBehaviour
{
    #region Fields
    //public AvatarBorderSelectionManager _avatarBorderSelectionManager;//rik

    //gameObjects

    public GameObject avatar;
    public GameObject wornLegs;
    public GameObject wornChest;
    public GameObject wornHair;
    public GameObject wornBeard;
    public GameObject wornMustache;
    public GameObject wornHandRight;
    public GameObject wornChestArmor;
    public GameObject wornBoots;
    //lists
    public List<Item> equippedItems = new List<Item>();
    //scripts
    private Stitcher stitcher;
    //ints
    private int totalEquipmentSlots;


    public Item LipsTexture, EyesTexture, SkinToneTexture;
    #endregion

    #region Monobehaviour
    public void Awake()
    {
        //   stitcher = new Stitcher();
    }
    // private void Start()
    public void Start()

    {
        stitcher = new Stitcher();
        // avatar = this.gameObject;

        /*if (_avatarBorderSelectionManager != null)//rik
        {
            Invoke("SetAllClothsOnARAvatar", 0.1f);  
        }*/
        LipsTexture.ItemType = "Lip";
        // LipsTexture.ItemID = 0;
        LipsTexture.SubCategoryname = "Lip";

        EyesTexture.ItemType = "Eyes";
        // EyesTexture.ItemID = 0;
        EyesTexture.SubCategoryname = "Eyes";

        SkinToneTexture.ItemType = "Skin";
        //SkinToneTexture.ItemID = 0;
        SkinToneTexture.SubCategoryname = "Skin";


        //  SkinnedMeshRenderer.sha .ShadowCastingMode.Off; //  = false; // .  // = false;

        //    wornChest.GetComponent<SkinnedMeshRenderer>().receiveShadows = UnityEngine.Rendering.ShadowCastingMode.Off;
        //     wornBoots.GetComponent<SkinnedMeshRenderer>().receiveShadows = UnityEngine.Rendering.ShadowCastingMode.Off;
       

    }
    public void AfterLogout()
    {
        stitcher = new Stitcher();
        // avatar = this.gameObject;

        /*if (_avatarBorderSelectionManager != null)//rik
        {
            Invoke("SetAllClothsOnARAvatar", 0.1f);  
        }*/
        LipsTexture.ItemType = "Lip";
        LipsTexture.ItemType = "Lip";
        //  LipsTexture.ItemID = 0;
        LipsTexture.SubCategoryname = "Lip";
        EyesTexture.ItemType = "Eyes";
        //  EyesTexture.ItemID = 0;
        EyesTexture.SubCategoryname = "Eyes";

        SkinToneTexture.ItemType = "Skin";
        //   SkinToneTexture.ItemID = 0;
        SkinToneTexture.SubCategoryname = "Skin";
        //  SkinToneTexture.ItemID = 0;
    }

    public void InitializeEquipptedItemsList()
    {
        totalEquipmentSlots = 8;

        for (int i = 0; i < totalEquipmentSlots; i++)
        {
            equippedItems.Add(new Item());
        }

        AddEquipmentToList(0); //Legs
        AddEquipmentToList(1); //Chest
        AddEquipmentToList(2); //Hair 
        AddEquipmentToList(3); //Beard 
        AddEquipmentToList(4); //Mustache
        AddEquipmentToList(5); //HandRight
        AddEquipmentToList(6); //ChestArmor
        AddEquipmentToList(7); //Feet
    }

    public void AddEquipmentToList(int id)
    {
        for (int i = 0; i < equippedItems.Count; i++)
        {

            if (equippedItems[id].ItemID == -1)
            {
                equippedItems[id] = ItemDatabase.instance.FetchItemByID(id);
                break;
            }
        }
    }

    public void AddEquipment(Item equipmentToAdd)
    {


        if (equipmentToAdd.ItemType == "Legs")
        {
            wornLegs = AddEquipmentHelper(wornLegs, equipmentToAdd);
            wornLegs.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        }
        else if (equipmentToAdd.ItemType == "Chest")
        {
            wornChest = AddEquipmentHelper(wornChest, equipmentToAdd);
            wornChest.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        }
        else if (equipmentToAdd.ItemType == "Hair")
        {
            wornHair = AddEquipmentHelper(wornHair, equipmentToAdd);
            wornHair.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        }
        else if (equipmentToAdd.ItemType == "Beard")
            wornBeard = AddEquipmentHelper(wornBeard, equipmentToAdd);
        else if (equipmentToAdd.ItemType == "Mustache")
            wornMustache = AddEquipmentHelper(wornMustache, equipmentToAdd);
        else if (equipmentToAdd.ItemType == "HandRight")
            wornHandRight = AddEquipmentHelper(wornHandRight, equipmentToAdd);
        else if (equipmentToAdd.ItemType == "ChestArmor")
            wornChestArmor = AddEquipmentHelper(wornChestArmor, equipmentToAdd);
        else if (equipmentToAdd.ItemType == "Feet")
        {
            wornBoots = AddEquipmentHelper(wornBoots, equipmentToAdd);
            wornBoots.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
            wornBoots.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        }


        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (wornLegs)
            {
                wornLegs.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
                wornLegs.GetComponent<SkinnedMeshRenderer>().receiveShadows = false;

            }
            if (wornChest)
            {
                wornChest.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
                wornChest.GetComponent<SkinnedMeshRenderer>().receiveShadows = false;
            }
            if (wornHair)
            {
                wornHair.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
                wornHair.GetComponent<SkinnedMeshRenderer>().receiveShadows = false;
            }
            //if (wornChestArmor.GetComponent<SkinnedMeshRenderer>())
            //{
            //    wornChestArmor.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            //    wornChestArmor. GetComponent<SkinnedMeshRenderer>().receiveShadows = false;
            //}
            if (wornBoots)
            {
                wornBoots.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
                wornBoots.GetComponent<SkinnedMeshRenderer>().receiveShadows = false;
            }
        }


    }

    public GameObject AddEquipmentHelper(GameObject wornItem, Item itemToAddToWornItem)
    {
        wornItem = Wear(itemToAddToWornItem.ItemPrefab, wornItem);
        wornItem.name = itemToAddToWornItem.Slug;

        //for border Selection.......//rik
        /*if (_avatarBorderSelectionManager != null)
        {
            wornItem.AddComponent<Outline1>();
            wornItem.GetComponent<Outline1>().enabled = false;
            _avatarBorderSelectionManager.AllAvatarBorderList.Add(wornItem.GetComponent<Outline1>());
        }*/

        UpdateStoreList();

        return wornItem;
    }

    public void RemoveEquipment(Item equipmentToAdd)
    {
        if (equipmentToAdd.ItemType == "Legs")
            wornLegs = RemoveEquipmentHelper(wornLegs, 0);
        else if (equipmentToAdd.ItemType == "Chest")
            wornChest = RemoveEquipmentHelper(wornChest, 1);
        else if (equipmentToAdd.ItemType == "Hair")
            wornHair = RemoveEquipmentHelper(wornHair, 2);
        else if (equipmentToAdd.ItemType == "Beard")
            wornBeard = RemoveEquipmentHelper(wornBeard, 3);
        else if (equipmentToAdd.ItemType == "Mustache")
            wornMustache = RemoveEquipmentHelper(wornMustache, 4);
        else if (equipmentToAdd.ItemType == "HandRight")
            wornHandRight = RemoveEquipmentHelper(wornHandRight, 5);
        else if (equipmentToAdd.ItemType == "ChestArmor")
            wornChestArmor = RemoveEquipmentHelper(wornChestArmor, 6);
        else if (equipmentToAdd.ItemType == "Feet")
            wornBoots = RemoveEquipmentHelper(wornBoots, 7);
    }

    public GameObject RemoveEquipmentHelper(GameObject wornItem, int nakedItemIndex)
    {
        wornItem = RemoveWorn(wornItem);
        equippedItems[nakedItemIndex] = ItemDatabase.instance.FetchItemByID(nakedItemIndex);
        return wornItem;
    }

    #endregion

    private GameObject RemoveWorn(GameObject wornClothing)
    {
        if (wornClothing == null)
            return null;

        if (wornClothing.GetComponent<Outline1>())
        {
            //_avatarBorderSelectionManager.AllAvatarBorderList.Remove(wornClothing.GetComponent<Outline1>());//rik
        }

        GameObject.Destroy(wornClothing);
        return null;
    }

    private GameObject Wear(GameObject clothing, GameObject wornClothing)
    {
        if (clothing == null)
            return null;
        clothing = (GameObject)GameObject.Instantiate(clothing);
        wornClothing = stitcher.Stitch(clothing, avatar);

        if (SceneManager.GetActiveScene().name != "Main")
            wornClothing.layer = 22;
        else
            wornClothing.layer = 11;


        GameObject.Destroy(clothing);
        return wornClothing;
    }

    public void SetAllClothsOnARAvatar()
    {
        Debug.LogError("SetAllClothsOnARAvatar.......");//rik
        /*if (_avatarBorderSelectionManager != null && AvatarStaticDataStoreHandler.currentMainAvatarEquippedItems.Count > 0)
        {
             for (int i = 0; i < AvatarStaticDataStoreHandler.currentMainAvatarEquippedItems.Count; i++)
            {
                if (AvatarStaticDataStoreHandler.currentMainAvatarEquippedItems[i].ItemPrefab != null)
                {
                     AddEquipment(AvatarStaticDataStoreHandler.currentMainAvatarEquippedItems[i]);
                    AvatarStaticDataStoreHandler.currentMainAvatarEquippedItems[i].ItemPrefab.GetComponent<EffectedParts>().DiableEffectedBodyParts();
                }
            }
        }*/
    }

    public void UpdateStoreList()
    {
        XanaConstants.xanaConstants.hair = equippedItems[2].ItemID.ToString();
        XanaConstants.xanaConstants.shirt = equippedItems[1].ItemID.ToString();
        XanaConstants.xanaConstants.shoes = equippedItems[7].ItemID.ToString();
        XanaConstants.xanaConstants.pants = equippedItems[0].ItemID.ToString();
        XanaConstants.xanaConstants.lipColor = LipsTexture.ItemID.ToString();
        XanaConstants.xanaConstants.eyeColor = EyesTexture.ItemID.ToString();
        XanaConstants.xanaConstants.skinColor = SkinToneTexture.ItemID.ToString();

        XanaConstants.xanaConstants.faceIndex = SavaCharacterProperties.instance.SaveItemList.FacePresets.f_BlendShapeOne;
        XanaConstants.xanaConstants.eyeBrowIndex = SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets.f_BlendShapeOne;
        XanaConstants.xanaConstants.eyeIndex = SavaCharacterProperties.instance.SaveItemList.EyePresets.f_BlendShapeOne;
        XanaConstants.xanaConstants.noseIndex = SavaCharacterProperties.instance.SaveItemList.NosePresets.f_BlendShapeOne;
        XanaConstants.xanaConstants.lipIndex = SavaCharacterProperties.instance.SaveItemList.LipsPresets.f_BlendShapeOne;
        XanaConstants.xanaConstants.bodyNumber = int.Parse(transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0).ToString());

        XanaConstants.xanaConstants.isFaceMorphed = SavaCharacterProperties.instance.SaveItemList.faceMorphed;
        XanaConstants.xanaConstants.isEyebrowMorphed = SavaCharacterProperties.instance.SaveItemList.eyeBrowMorphed;
        XanaConstants.xanaConstants.isEyeMorphed = SavaCharacterProperties.instance.SaveItemList.eyeMorphed;
        XanaConstants.xanaConstants.isNoseMorphed = SavaCharacterProperties.instance.SaveItemList.noseMorphed;
        XanaConstants.xanaConstants.isLipMorphed = SavaCharacterProperties.instance.SaveItemList.lipMorphed;
    }
    public void SaveDefaultValues()
    {
        SavaCharacterProperties.instance.SaveItemList.FacePresets.f_BlendShapeOne = 0;
        SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets.f_BlendShapeOne = 0;
        SavaCharacterProperties.instance.SaveItemList.EyePresets.f_BlendShapeOne = 0;
        SavaCharacterProperties.instance.SaveItemList.NosePresets.f_BlendShapeOne = 0;
        SavaCharacterProperties.instance.SaveItemList.LipsPresets.f_BlendShapeOne = 0;

        SavaCharacterProperties.instance.SaveItemList.faceMorphed = false;
        SavaCharacterProperties.instance.SaveItemList.eyeBrowMorphed = false;
        SavaCharacterProperties.instance.SaveItemList.eyeMorphed = false;
        SavaCharacterProperties.instance.SaveItemList.noseMorphed = false;
        SavaCharacterProperties.instance.SaveItemList.lipMorphed = false;
    }
}