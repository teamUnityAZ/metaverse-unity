using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeGridLayoutSetting : MonoBehaviour
{
    public GridLayoutGroup m_GridLayoutGrp;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    public void ChangeGridSetting(int columnCount)
    {
        m_GridLayoutGrp.constraintCount = columnCount;
    }
}
