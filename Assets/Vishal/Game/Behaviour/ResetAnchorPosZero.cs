using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnchorPosZero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
}