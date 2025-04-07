using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectedParts : MonoBehaviour
{
    public int[] m_EffectedBodyPartsIndexes;
    void Start()
    {
      
    }

   
   
    public void DiableEffectedBodyParts()
    {
        for (int i = 0; i < m_EffectedBodyPartsIndexes.Length; i++)
        {
            CharcterBodyParts.instance.m_BodyParts[m_EffectedBodyPartsIndexes[i]].SetActive(false);

        }
    }
    public void EnableEffectedBodyParts()
    {
        for (int i = 0; i < m_EffectedBodyPartsIndexes.Length; i++)
        {
            CharcterBodyParts.instance.m_BodyParts[m_EffectedBodyPartsIndexes[i]].SetActive(true);

        }
    }
}
