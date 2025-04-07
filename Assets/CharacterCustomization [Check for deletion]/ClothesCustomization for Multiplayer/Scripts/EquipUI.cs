using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipUI : MonoBehaviour/*, IPointerClickHandler*/
{
    [HideInInspector]
    public GameObject unit;
    private ChangeGear changeGearScript;
    private Equipment equipmentScript;
    private Text textChild;
   
    private void Start()
    {
        unit = GameManager.Instance.mainCharacter;
        changeGearScript = unit.GetComponent<ChangeGear>();
        equipmentScript = unit.GetComponent<Equipment>();
  
        Invoke("DefaultClothes", 0.1f);
    }
    public void DefaultClothes()
    {
            ChangeCostume("MDshirt");
            ChangeCostume("MDpant");
            ChangeCostume("MDshoes");
            changeGearScript.EquipItem("Hair", "MDhairs");       
    }    
    public void BackFromArtbone()
    {
        GameManager.Instance.ChangeCharacterAnimationState(false);
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    public void ChangeCostume(string CostumeName)
    {
        if(CostumeName.Contains("pant"))
        {
            AddOrRemoveClothes("naked_legs", "Legs", CostumeName, 0);
        }
        else
             if (CostumeName.Contains("boots") || CostumeName.Contains("shoes") || CostumeName.Contains("sho"))
        {
            AddOrRemoveClothes("naked_slug", "Feet", CostumeName, 7);
        }
        else
             if (CostumeName.Contains("gambeson") || CostumeName.Contains("shirt"))
        {
            AddOrRemoveClothes("naked_chest", "Chest", CostumeName, 1);
        }
        else
             if (CostumeName.Contains("hair"))
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
        else if (equipmentScript.wornChest !=null && equipmentScript.equippedItems[equippedItemsIndex].Slug == equipmentScript.wornChest.gameObject.name )
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
        else if (equipmentScript.wornBoots != null &&  equipmentScript.equippedItems[equippedItemsIndex].ItemType == "Feet")
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
