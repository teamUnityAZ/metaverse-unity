using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlendShapeDataClass 
{

    public enum FaceMorphFeature { Eyes, Nose, Lips, EyeBrows, Face }  

    public FaceMorphFeature _FaceMorphFeature;

    public int f_BlendShapeOne;  
    public int f_BlendShapeTwo;

    public float m_BlendTime;
    public AnimationCurve m_AnimCurve;


    
}
