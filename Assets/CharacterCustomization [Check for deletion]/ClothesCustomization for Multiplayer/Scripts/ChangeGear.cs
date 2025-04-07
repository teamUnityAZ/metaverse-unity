using UnityEngine;
using Photon.Pun;
public class ChangeGear : MonoBehaviour
{
    public Equipment equipmentScript; 

    private void Start()
    {
      //  if(GameObject.FindGameObjectWithTag("PhotonLocalPlayer"))
       // equipmentScript = this.GetComponent<Equipment>();
        //create equipment list

       // if (GameObject.FindGameObjectWithTag("PhotonLocalPlayer"))
            equipmentScript.InitializeEquipptedItemsList();
        //equip stuff
        // EquipItem("Legs", "pants"); 
    }

  

    public void EquipItem(string itemType, string itemSlug)
    {
        //   print("55555 " + gameObject.tag + " " + equipmentScript.equippedItems.Count);

        for (int i = 0; i < equipmentScript.equippedItems.Count; i++)
        {
            //  print(" in for loop");
            //    print(itemType + " 44444 " + equipmentScript.equippedItems[i].ItemType + " " + gameObject.tag + " " + i);
            if (equipmentScript.equippedItems[i].ItemType == itemType && itemSlug != equipmentScript.equippedItems[i].ItemName)
            {
                //  print(itemType + " 22222 " + itemSlug + " " + gameObject.tag);
                //  print(" in if condition");
                // if (GetComponent<PhotonView>())
                // Debug.Log(itemSlug + "---" + gameObject.tag + "---" + GetComponent<PhotonView>().ViewID);

                equipmentScript.equippedItems[i] = ItemDatabase.instance.FetchItemBySlug(itemSlug);
                equipmentScript.AddEquipment(equipmentScript.equippedItems[i]);
                if (itemType != "Hair" && itemType != "Feet")
                    DiableEffectedBodyParts(equipmentScript.equippedItems[i].ItemPrefab.GetComponent<EffectedParts>());
                break;
            }
        }
    }

    public void UnequipItem(string itemType, string itemSlug)
    {
       // print(itemType + " 33333 " + itemSlug + " " + gameObject.tag);
        for (int i = 0; i < equipmentScript.equippedItems.Count; i++)
        {
            if (equipmentScript.equippedItems[i].ItemType == itemType)
            {
                if (itemType != "Hair" && itemType != "Feet")
                    EnableEffectedBodyParts(equipmentScript.equippedItems[i].ItemPrefab.GetComponent<EffectedParts>());
                equipmentScript.RemoveEquipment(equipmentScript.equippedItems[i]);
              
                break;
            }
        }
    }
    //-------------
    public void DiableEffectedBodyParts(EffectedParts _parts)
    {
        for (int i = 0; i < _parts.m_EffectedBodyPartsIndexes.Length; i++)
        {
            gameObject.GetComponent<CharcterBodyParts>().m_BodyParts[_parts.m_EffectedBodyPartsIndexes[i]].SetActive(false);

        }
    }
    public void EnableEffectedBodyParts(EffectedParts _parts)
    {
        for (int i = 0; i < _parts.m_EffectedBodyPartsIndexes.Length; i++)
        {
            gameObject.GetComponent<CharcterBodyParts>().m_BodyParts[_parts.m_EffectedBodyPartsIndexes[i]].SetActive(true);

        }
    }

    //------------
}