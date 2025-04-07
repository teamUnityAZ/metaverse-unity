using Metaverse;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class DefaultClothes : MonoBehaviour
{
    private GameObject unit;
    private ChangeGear changeGearScript;
    private Equipment equipmentScript;

    public GameObject MyHead;
    public Renderer myeyeball1;
    public Renderer myeyeball2;
    byte[] _bytes;
    Texture2D mytexture;
    private void Start()
    {
        _DefaultInitializer();
    }
    public void _DefaultInitializer()
    {
        unit = gameObject;
        changeGearScript = gameObject.GetComponent<ChangeGear>();
        equipmentScript = gameObject.GetComponent<Equipment>();
        if (!GetComponent<PhotonView>().IsMine)
        {
            if (!equipmentScript.wornHair)
                changeGearScript.EquipItem("Hair", "MDhairs");
            if (!equipmentScript.wornBoots)
                changeGearScript.EquipItem("Feet", "MDshoes");
            if (!equipmentScript.wornChest)
                changeGearScript.EquipItem("Chest", "MDshirt");
            if (!equipmentScript.wornLegs)
                changeGearScript.EquipItem("Legs", "MDpant");
        }
        if (GetComponent<PhotonView>().IsMine)
            StartClothes();

    }


    public void StartClothes()
    {
        if (!equipmentScript.wornChest && !equipmentScript.wornBoots && !equipmentScript.wornLegs && gameObject.tag == "PhotonLocalPlayer")
        {
            AddOrRemoveClothes("naked_legs", "Legs", "MDpant", 0);
            AddOrRemoveClothes("naked_chest", "Chest", "MDshirt", 1);
            AddOrRemoveClothes("naked_slug", "Feet", "MDshoes", 7);
            AddOrRemoveClothes("bald_head", "Hair", "MDhairs", 2);


            if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
            {
                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                for (int j = 0; j < _CharacterData.myItemObj.Count; j++)
                {
                    //if (_CharacterData.myItemObj[j].ItemLinkAndroid != "" && _CharacterData.myItemObj[j].ItemLinkIOS != "")
                    //{
                    switch (_CharacterData.myItemObj[j].ItemType)
                    {
                        case "Legs":
                            AddOrRemoveClothes("naked_legs", "Legs", _CharacterData.myItemObj[j].Slug, 0);
                            break;
                        case "Chest":
                            AddOrRemoveClothes("naked_chest", "Chest", _CharacterData.myItemObj[j].Slug, 1);
                            break;
                        case "Feet":
                            AddOrRemoveClothes("naked_slug", "Feet", _CharacterData.myItemObj[j].Slug, 7);
                            break;
                        case "Hair":
                            AddOrRemoveClothes("bald_head", "Hair", _CharacterData.myItemObj[j].Slug, 2);
                            break;
                        case "Eyes":
                            if(File.Exists(Application.persistentDataPath + "/" + _CharacterData.myItemObj[j].ItemName))
                            {
                                _bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + _CharacterData.myItemObj[j].ItemName);
                                if (_bytes == null)
                                    return;
                                mytexture = new Texture2D(2, 2);
                                mytexture.LoadImage(_bytes);

                                myeyeball1.material.mainTexture = mytexture;
                                myeyeball2.material.mainTexture = mytexture;
                            }
                            break;
                        case "Lip":
                            if (File.Exists(Application.persistentDataPath + "/" + _CharacterData.myItemObj[j].ItemName))
                            {
                                _bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + _CharacterData.myItemObj[j].ItemName);
                                if (_bytes == null)
                                    return;
                                mytexture = new Texture2D(2, 2);
                                mytexture.LoadImage(_bytes);
                                MyHead.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = mytexture;
                            }
                            break;
                        case "Skin":
                            if (File.Exists(Application.persistentDataPath + "/" + _CharacterData.myItemObj[j].ItemName))
                            {
                                _bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + _CharacterData.myItemObj[j].ItemName);
                                if (_bytes == null)
                                    return;
                                mytexture = new Texture2D(2, 2);
                                mytexture.LoadImage(_bytes);
                                MyHead.GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = mytexture;
                                for (int i = 0; i < gameObject.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
                                {
                                    if (gameObject.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<SkinnedMeshRenderer>())
                                        gameObject.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<SkinnedMeshRenderer>().material.mainTexture = mytexture;
                                }
                            }
                            break;
                    }
                    //}
                }
            }
            //else
            //{
            //    AddOrRemoveClothes("naked_legs", "Legs", "MDpant", 0);
            //    AddOrRemoveClothes("naked_chest", "Chest", "MDshirt", 1);
            //    AddOrRemoveClothes("naked_slug", "Feet", "MDshoes", 7);
            //    AddOrRemoveClothes("bald_head", "Hair", "MDhairs", 2);
            //}
            ApplySavedProperties();
        }
    }

    public void ApplySavedProperties()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));


            //APPLY BLEND SHAPE
            for (int i = 0; i < MyHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (_CharacterData.FaceBlendsShapes != null && i<_CharacterData.FaceBlendsShapes.Length) //added condition to handle AR face blendshape.s
                    MyHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
                else
                    MyHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[0]);
            }

            for (int i = 0; i < gameObject.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
            {
                if (gameObject.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<SkinnedMeshRenderer>() && gameObject.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount != 0)
                    gameObject.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, _CharacterData.BodyFat);
            }

            gameObject.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, _CharacterData.BodyFat);
            gameObject.GetComponent<Equipment>().wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, _CharacterData.BodyFat);
        }
    }


    //for first person camera changed layer for self player
    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }



    public void ChangeCostume(string CostumeName)
    {
        if (CostumeName.Contains("pants") || CostumeName.Contains("pant"))
        {
            AddOrRemoveClothes("naked_legs", "Legs", CostumeName, 0);
        }
        else
             if (CostumeName.Contains("boots") || CostumeName.Contains("shosec"))
        {
            AddOrRemoveClothes("naked_slug", "Feet", CostumeName, 7);
        }
        else
             if (CostumeName.Contains("gambeson") || CostumeName.Contains("shirt"))
        {
            AddOrRemoveClothes("naked_chest", "Chest", CostumeName, 1);
        }
        else
             if (CostumeName.Contains("Hair") || CostumeName.Contains("hair"))
        {
            AddOrRemoveClothes("bald_head", "Hair", CostumeName, 2);
        }

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
