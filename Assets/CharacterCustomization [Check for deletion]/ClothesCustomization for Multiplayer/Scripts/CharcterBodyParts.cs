using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterBodyParts : MonoBehaviour
{
    public static CharcterBodyParts instance;
    public Equipment equipment;
    public List<GameObject> m_BodyParts;
    private void Awake()
    {
        instance = this;
        foreach (GameObject b in m_BodyParts)
        {
            if (b.GetComponent<SkinnedMeshRenderer>())
                b.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
        }
    }

    private void Update()
    {
        if (equipment.wornBoots != null)
        {
            if (equipment.wornBoots.name == "shoesc51" || equipment.wornBoots.name == "shosec13" || equipment.wornBoots.name == "shoesc54" || equipment.wornBoots.name == "shoesc52" || equipment.wornBoots.name == "shoesc50" && m_BodyParts[2].activeInHierarchy)
            {
                m_BodyParts[2].SetActive(false);
            }
        }
    }

    public void DisableBodyPartsCustom()
    {
        if (equipment.wornBoots != null)
        {
            if (equipment.wornBoots.name == "shoesc51" || equipment.wornBoots.name == "shosec13" || equipment.wornBoots.name == "shoesc54" || equipment.wornBoots.name == "shoesc52" || equipment.wornBoots.name == "shoesc50" && m_BodyParts[2].activeInHierarchy)
            {
                m_BodyParts[2].SetActive(false);
            }
            else if (equipment.wornBoots.name != "shoesc51" || equipment.wornBoots.name != "shosec13" || equipment.wornBoots.name != "shoesc54" || equipment.wornBoots.name != "shoesc52" || equipment.wornBoots.name != "shoesc50" && !m_BodyParts[2].activeInHierarchy)
            {
                m_BodyParts[2].SetActive(true);
            }
        }
    }
}
