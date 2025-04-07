using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageDrag : MonoBehaviour , IDragHandler
{
    // in this script we are mapping the 2d movement coordinates into 3d coordinated and reading the
    // color of the texture at relative position in the 3d view

    [Header("Color Pallet")] // make cirlce cursor as cild of the color pallet image
    public GameObject m_ColorPallet;

    [Header("Color Detector")]
    public GameObject m_ColorDetector;
    
    

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position += (Vector3)eventData.delta;

        Rect l_Rect=m_ColorPallet.GetComponent<RectTransform>().rect;
        Vector3 l_Position=transform.GetComponent<RectTransform>().localPosition;

        float l_Width=l_Rect.width;
        float l_Height=l_Rect.height;

        float l_XScaler=9.0f/l_Width;
        float l_YScaler=4.0f/l_Height;

        m_ColorDetector.transform.position=new Vector3(l_Position.x*l_XScaler,0.5f,l_Position.y*l_YScaler);

        float l_X=Mathf.Clamp(l_Position.x,20-(l_Width/2),(l_Width/2)-20);
        float l_Y=Mathf.Clamp(l_Position.y,20-(l_Height/2),(l_Height/2)-20);

        transform.GetComponent<RectTransform>().localPosition=new Vector3(l_X,l_Y,0);
    }

    #endregion
}
