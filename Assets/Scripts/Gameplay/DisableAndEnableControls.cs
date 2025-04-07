using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAndEnableControls : MonoBehaviour
{
    public List<GameObject> disableObjectOnEnableOfthis;
    public GameObject disablenewCanvas;
    bool isSetToValue = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void Awake()
    {
        StartCoroutine(Fade());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fade()
    {
        if (XanaConstants.xanaConstants.IsMuseum)
        {
            disablenewCanvas = GameObject.FindGameObjectWithTag("NewCanvas");
            yield return new WaitForSeconds(.01f);
        }
    }

    private void OnEnable()
    {
        if (!isSetToValue)
        {
            if (XanaConstants.xanaConstants.IsMuseum)
            {
                if (ReferrencesForDynamicMuseum.instance.disableObjects.Length > 0)
                {
                    disableObjectOnEnableOfthis.Clear();
                    foreach (GameObject go in ReferrencesForDynamicMuseum.instance.disableObjects)
                    {
                      
                        disableObjectOnEnableOfthis.Add(go);
                    }
                }
            }
            isSetToValue = true;
        }
        foreach(GameObject go in disableObjectOnEnableOfthis)
        {
            go.SetActive(false);
           
        }
        disablenewCanvas.SetActive(false);
    }
    private void OnDisable()
    {
        foreach (GameObject go in disableObjectOnEnableOfthis)
        {
            go.SetActive(true);
           
        }
        disablenewCanvas.SetActive(true);
    }
}
