using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPanelTapAndHold : MonoBehaviour
{
    // Start is called before the first frame update
    public bool rectInterpolate;
    //public Image fadeImage;
    bool reverse;
    bool m_scaleToZero;
    Transform parent;
    int factor = 30;
    int closeFactor = 20;

    private void Awake()
    {     
        parent = this.transform.parent;
    }
   
    public void DisablePanel()
    {
        rectInterpolate = false;
        this.transform.SetParent(parent);
        reverse = true;
        //fadeImage.GetComponent<Animator>().SetBool("FadeIn", true);
        Invoke("SetFalse", 0.15f);
    }

    private void OnDisable()
    {
        UIManager.Instance.HomePage.SetActive(true);
        reverse = false;
        m_scaleToZero = false;
        this.transform.localScale = Vector3.one;
        GetComponent<CanvasGroup>().alpha = 1;
    }

    // Update is called once per frame

    private void Update()
    {
        if (rectInterpolate)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, factor * Time.deltaTime);
            //transform.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(transform.GetComponent<RectTransform>().offsetMin,new Vector2(0.5f,0.5f), factor * Time.deltaTime);
            //transform.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(transform.GetComponent<RectTransform>().offsetMax, new Vector2(0.5f, 0.5f), factor * Time.deltaTime);
            transform.GetComponent<RectTransform>().anchorMin = Vector2.Lerp(transform.GetComponent<RectTransform>().anchorMin, new Vector2(0.5f, 0.5f), factor * Time.deltaTime);
            transform.GetComponent<RectTransform>().anchorMax = Vector2.Lerp(transform.GetComponent<RectTransform>().anchorMax, new Vector2(0.5f, 0.5f), factor * Time.deltaTime);
            transform.GetComponent<RectTransform>().pivot = Vector2.Lerp(transform.GetComponent<RectTransform>().pivot, new Vector2(0.5f, 0.5f), factor * Time.deltaTime);
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 800);
            transform.localPosition = Vector3.zero;
        }
        if (reverse)
        {
            SetRect(this.transform.GetComponent<RectTransform>());
        }
     }

    void SetRect(RectTransform rec)
    {
        transform.localScale = Vector3.Lerp(transform.localScale,new Vector3(0.4f,0.4f,0.4f), closeFactor * Time.deltaTime);
        GetComponent<CanvasGroup>().alpha = Mathf.Lerp(GetComponent<CanvasGroup>().alpha, 0, closeFactor * Time.deltaTime);
        transform.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(transform.GetComponent<RectTransform>().offsetMin, Vector2.zero, closeFactor * Time.deltaTime);
        transform.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(transform.GetComponent<RectTransform>().offsetMax, Vector2.zero, closeFactor * Time.deltaTime);
        transform.GetComponent<RectTransform>().anchorMin = Vector2.Lerp(transform.GetComponent<RectTransform>().anchorMin, Vector2.zero, closeFactor * Time.deltaTime);
        transform.GetComponent<RectTransform>().anchorMax = Vector2.Lerp(transform.GetComponent<RectTransform>().anchorMax, Vector2.one, closeFactor * Time.deltaTime);
        transform.GetComponent<RectTransform>().pivot = Vector2.Lerp(transform.GetComponent<RectTransform>().pivot, new Vector2(0.5f, 0.5f), closeFactor * Time.deltaTime);
    }

    void SetFalse()
    {
        reverse = false;
       // m_scaleToZero = true;
        this.gameObject.SetActive(false);
       // Invoke("SetDisable", 0.1f);
    }

  

}
