using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyCustomizationTrigger : MonoBehaviour
{
    public enum FaceMorphFeature { Eyes, Nose, Lips, EyeBrows, Face }  // To Set The "BodyCustomizerTrigger" To Modify Either The Face Or Body

    public FaceMorphFeature m_FaceMorphFeature;
    public int f_BlendShapeOne;   // Blend Shape Index : if you are using only one blendshape then give -1 in the inspector in any one.
    public int f_BlendShapeTwo;
    public float m_BlendTime;
    public AnimationCurve m_AnimCurve;

    public void CustomizationTriggerTwo()
    {
        //-----------------------------------
        //StoreManager.instance.BuyStoreBtn.SetActive(false);
        StoreManager.instance.SaveStoreBtn.SetActive(true);
        StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
        StoreManager.instance.GreyRibbonImage.SetActive(false);
        StoreManager.instance.WhiteRibbonImage.SetActive(true);
        StoreManager.instance.ClearBuyItems();
        //-------------------------------------

        if (m_FaceMorphFeature == FaceMorphFeature.Eyes)
        {
            if (GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(f_BlendShapeOne) != 100f)
            {
                BodyCustomizer.Instance.ApplyEyesBlendShapes(f_BlendShapeOne, f_BlendShapeTwo);

                SavaCharacterProperties.instance.SaveItemList.EyePresets._FaceMorphFeature = BlendShapeDataClass.FaceMorphFeature.Eyes;
                SavaCharacterProperties.instance.SaveItemList.EyePresets.f_BlendShapeOne = f_BlendShapeOne;
                SavaCharacterProperties.instance.SaveItemList.EyePresets.f_BlendShapeTwo = f_BlendShapeTwo;
                SavaCharacterProperties.instance.SaveItemList.EyePresets.m_AnimCurve = m_AnimCurve;
                SavaCharacterProperties.instance.SaveItemList.EyePresets.m_BlendTime = m_BlendTime;
            }
        }
        if (m_FaceMorphFeature == FaceMorphFeature.Nose)
        {
            if (GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(f_BlendShapeOne) != 100f)
            {
                BodyCustomizer.Instance.ApplyNoseBlendShapes(f_BlendShapeOne, f_BlendShapeTwo);

                SavaCharacterProperties.instance.SaveItemList.NosePresets._FaceMorphFeature = BlendShapeDataClass.FaceMorphFeature.Nose;
                SavaCharacterProperties.instance.SaveItemList.NosePresets.f_BlendShapeOne = f_BlendShapeOne;
                SavaCharacterProperties.instance.SaveItemList.NosePresets.f_BlendShapeTwo = f_BlendShapeTwo;
                SavaCharacterProperties.instance.SaveItemList.NosePresets.m_AnimCurve = m_AnimCurve;
                SavaCharacterProperties.instance.SaveItemList.NosePresets.m_BlendTime = m_BlendTime;
            }
        }
        if (m_FaceMorphFeature == FaceMorphFeature.Lips)
        {
            if (GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(f_BlendShapeOne) != 100f)
            {
                BodyCustomizer.Instance.ApplyLipsBlendShapes(f_BlendShapeOne, f_BlendShapeTwo);

                SavaCharacterProperties.instance.SaveItemList.LipsPresets._FaceMorphFeature = BlendShapeDataClass.FaceMorphFeature.Lips;
                SavaCharacterProperties.instance.SaveItemList.LipsPresets.f_BlendShapeOne = f_BlendShapeOne;
                SavaCharacterProperties.instance.SaveItemList.LipsPresets.f_BlendShapeTwo = f_BlendShapeTwo;
                SavaCharacterProperties.instance.SaveItemList.LipsPresets.m_AnimCurve = m_AnimCurve;
                SavaCharacterProperties.instance.SaveItemList.LipsPresets.m_BlendTime = m_BlendTime;
            }
        }
        if (m_FaceMorphFeature == FaceMorphFeature.EyeBrows)
        {
            if (GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(f_BlendShapeOne) != 100f)
            {
                BodyCustomizer.Instance.ApplyEyeBrowsBlendShapes(f_BlendShapeOne, f_BlendShapeTwo);

                SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets._FaceMorphFeature = BlendShapeDataClass.FaceMorphFeature.EyeBrows;
                SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets.f_BlendShapeOne = f_BlendShapeOne;
                SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets.f_BlendShapeTwo = f_BlendShapeTwo;
                SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets.m_AnimCurve = m_AnimCurve;
                SavaCharacterProperties.instance.SaveItemList.EyeBrowPresets.m_BlendTime = m_BlendTime;
            }
        }
        if (m_FaceMorphFeature == FaceMorphFeature.Face)
        {
            if (GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(f_BlendShapeOne) != 100f)
            {
                BodyCustomizer.Instance.ApplyFaceBlendShapes(f_BlendShapeOne, f_BlendShapeTwo);

                SavaCharacterProperties.instance.SaveItemList.FacePresets._FaceMorphFeature = BlendShapeDataClass.FaceMorphFeature.Face;
                SavaCharacterProperties.instance.SaveItemList.FacePresets.f_BlendShapeOne = f_BlendShapeOne;
                SavaCharacterProperties.instance.SaveItemList.FacePresets.f_BlendShapeTwo = f_BlendShapeTwo;
                SavaCharacterProperties.instance.SaveItemList.FacePresets.m_AnimCurve = m_AnimCurve;
                SavaCharacterProperties.instance.SaveItemList.FacePresets.m_BlendTime = m_BlendTime;
            }
        }
    }
}
