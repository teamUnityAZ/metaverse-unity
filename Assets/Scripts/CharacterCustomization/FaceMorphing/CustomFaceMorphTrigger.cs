using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFaceMorphTrigger : MonoBehaviour
{
    [Header("Hor/Ver Face Morph Slider Indexes")]
    public int m_BlendShapeIndex;
    public bool index = true;
    public string m_BlendshapeName;
    public int m_MovementDirection;
    public bool m_HorOrVer;  // Either the Trigger will move on Horizontal or Vertical


    [Header("Movement Constraints")] // Movement region of the trigger 
    public float XMin;
    public float XMax;
    public float YMin;
    public float YMax;

    [Header("Movement Scaler")]
    public float m_MovementScaler;

    [Header("Mirror Point")]
    public GameObject m_MirrorPoint; // In Zepeto if we move one point on face then, another point on the other side of the face will also move

    [Header("45 Degree Movement")]
    public bool m_45DegreeMovement;

    [Header("Trigger Index")]
    public int m_Index;

    [HideInInspector]
    public float m_XSliderValue;
    [HideInInspector]
    public float m_YSliderValue;


    private void OnMouseDown()
    {
        //if (!index)
        //{
        //    m_BlendShapeIndex = CustomFaceMorph.Instance.m_FaceSkinMeshRenderer.sharedMesh.GetBlendShapeIndex(m_BlendshapeName);
        //}

        //if (m_HorOrVer)
        //    m_XSliderValue = CustomFaceMorph.Instance.m_FaceSkinMeshRenderer.gameObject.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(m_BlendShapeIndex);
        //else
        //    m_YSliderValue = CustomFaceMorph.Instance.m_FaceSkinMeshRenderer.gameObject.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(m_BlendShapeIndex);

        //GetComponent<SpriteRenderer>().color = CustomFaceMorph.Instance.m_ColorTwo;
      //  CustomFaceMorph.Instance.SelectedFaceMorphTrigger(this.gameObject, m_MirrorPoint, XMin, XMax, YMin, YMax, m_BlendShapeIndex, m_MovementDirection, m_MovementScaler, m_HorOrVer, m_45DegreeMovement, m_Index);
    }
}
