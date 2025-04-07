
 using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DefaultEnteriesforManican : MonoBehaviour
{
    public static DefaultEnteriesforManican instance;
    ChangeGear changeGearScript;
    Equipment equipmentScript;

    public GameObject MyHead;
    public Renderer myeyeball1;
    public Renderer myeyeball2;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

      //  PlayerPrefs.SetInt("Loaded", 1);
    }
    void Start()
    {
        //if (PlayerPrefs.GetInt("Loaded") == 1)
        //{
        //    print("DONOTLOAD");
        //    Invoke("DefaultReset", 3.0f);
        //    PlayerPrefs.SetInt("Loaded", 0);
        //}

      
            changeGearScript = gameObject.GetComponent<ChangeGear>();
        equipmentScript = gameObject.GetComponent<Equipment>();
    }

    public void DefaultReset()
    {
        AddOrRemoveClothes("naked_legs", "Legs", "MDpant", 0);
        AddOrRemoveClothes("naked_chest", "Chest", "MDshirt", 1);
        AddOrRemoveClothes("naked_slug", "Feet", "MDshoes", 7);
        AddOrRemoveClothes("bald_head", "Hair", "MDhairs", 2);

        //body fats
        SavaCharacterProperties.instance.SaveItemList.BodyFat = 0;
        //body blends
        CharacterCustomizationManager.Instance.UpdateChBodyShape(0);
        //lips
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultLips;
        //eyesdefault    
        GameManager.Instance.EyeballTexture1.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        GameManager.Instance.EyeballTexture2.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        //"Skin"
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
        {
            if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        }
        //Check if data exist
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            //blend shapes
            for (int i = 0; i < MyHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (_CharacterData.FaceBlendsShapes != null && i<_CharacterData.FaceBlendsShapes.Length)
                    MyHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
                else
                    MyHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[0]);
            }
        }
        LastSaved_Reset();
    }




    public void DefaultReset_HAck()
    {
        AddOrRemoveClothes("naked_legs", "Legs", "MDpant", 0);
        AddOrRemoveClothes("naked_chest", "Chest", "MDshirt", 1);
        AddOrRemoveClothes("naked_slug", "Feet", "MDshoes", 7);
        AddOrRemoveClothes("bald_head", "Hair", "MDhairs", 2);

        //body fats
        SavaCharacterProperties.instance.SaveItemList.BodyFat = 0;
        //body blends
        CharacterCustomizationManager.Instance.UpdateChBodyShape(0);
        //lips
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultLips;
        //eyesdefault    
        GameManager.Instance.EyeballTexture1.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        GameManager.Instance.EyeballTexture2.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        //"Skin"
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
        {
            if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        }
        //Check if data exist
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            //blend shapes
            for (int i = 0; i < MyHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                //  if (_CharacterData.FaceBlendsShapes != null)
                //    MyHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
                //   else
                MyHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, 0);
            }


            if (_CharacterData.myItemObj[5].ItemLinkIOS != null)
            {
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemLinkIOS = ""; // _CharacterData.myItemObj[5].ItemLink;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemName = "";// _CharacterData.myItemObj[5].ItemName;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemID = -1; // _CharacterData.myItemObj[5].ItemID;
            }
            if (_CharacterData.myItemObj[4].ItemLinkIOS != null)
            {
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemLinkIOS = "";// _CharacterData.myItemObj[4].ItemLink;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemName = "";// _CharacterData.myItemObj[4].ItemName;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemID = -1;// _CharacterData.myItemObj[4].ItemID;
            }
            if (_CharacterData.myItemObj[6].ItemLinkIOS != null)
            {
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemLinkIOS = ""; //_CharacterData.myItemObj[6].ItemLink;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemName = "";// _CharacterData.myItemObj[6].ItemName;
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemID = -1;// _CharacterData.myItemObj[6].ItemID;
            }
        }



        LastSaved_Reset();
    }

    public void ResetForLastSaved()
    {

        //body fats
        SavaCharacterProperties.instance.SaveItemList.BodyFat = 0;
        //body blends
        CharacterCustomizationManager.Instance.UpdateChBodyShape(0);
        //lips
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultLips;
        //eyesdefault    
        GameManager.Instance.EyeballTexture1.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        GameManager.Instance.EyeballTexture2.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        //"Skin"
        //GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        //for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
        //{
        //    if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
        //        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        //}
        ItemDatabase.instance.RevertSavedCloths();
    }

    public void ResetForPresets()
    {

        //body fats
        SavaCharacterProperties.instance.SaveItemList.BodyFat = 0;
        //body blends
        CharacterCustomizationManager.Instance.UpdateChBodyShape(0);
        //lips
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultLips;
        //eyesdefault    
        GameManager.Instance.EyeballTexture1.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        GameManager.Instance.EyeballTexture2.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
        //"Skin"
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
        {
            if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
        }
        ItemDatabase.instance.RevertSavedCloths();
    }




    public void LastSaved_Reset()
    {
        ItemDatabase.instance.RevertSavedCloths();
    }
    public void AddOrRemoveClothes(string nakedSlug, string clothesType, string clothesSlug, int equippedItemsIndex)
    {

        if (equipmentScript.equippedItems[equippedItemsIndex].Slug == nakedSlug)
        {
            changeGearScript.EquipItem(clothesType, clothesSlug);
        }
        else if (equipmentScript.wornChest != null && equipmentScript.equippedItems[equippedItemsIndex].Slug == equipmentScript.wornChest.gameObject.name)
        {

            changeGearScript.UnequipItem(clothesType, equipmentScript.wornChest.gameObject.name);
            changeGearScript.EquipItem(clothesType, clothesSlug);

        }
        else
                if (equipmentScript.wornLegs != null && equipmentScript.equippedItems[equippedItemsIndex].ItemType == "Legs")
        {

            changeGearScript.UnequipItem(clothesType, equipmentScript.wornLegs.gameObject.name);
            changeGearScript.EquipItem(clothesType, clothesSlug);
        }
        else if (equipmentScript.wornBoots != null && equipmentScript.equippedItems[equippedItemsIndex].ItemType == "Feet")
        {

            changeGearScript.UnequipItem(clothesType, equipmentScript.wornBoots.gameObject.name);
            changeGearScript.EquipItem(clothesType, clothesSlug);
        }
        else if (equipmentScript.wornHair != null && equipmentScript.equippedItems[equippedItemsIndex].ItemType == "Hair")
        {

            changeGearScript.UnequipItem(clothesType, equipmentScript.wornHair.gameObject.name);
            changeGearScript.EquipItem(clothesType, clothesSlug);
        }

    }

}
