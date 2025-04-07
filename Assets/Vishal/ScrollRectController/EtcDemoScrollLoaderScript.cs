using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class EtcDemoScrollLoaderScript : MonoBehaviour
{
    public ScrollRectFasterEx scrollRectFasterEx;

    public GameObject containerObj;
    public bool isLoaderShow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(containerObj.GetComponent<RectTransform>().anchoredPosition.y < 10)
        {

        }
    }
}