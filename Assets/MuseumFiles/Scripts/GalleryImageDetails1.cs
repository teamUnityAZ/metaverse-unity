using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GalleryImageDetails1 : MonoBehaviour
{
    [Header("Picture Index")]
    public int m_PictureIndex;

    [Header("Special Painting")]
    public bool m_IsSpecialPainting;
    public bool m_IsVideo;

    [Header("Banner Settings")]
    public bool m_IsPortrait;
    public bool m_isBanner;

    [Header("Mayor And Judge Frames")]
    public bool m_IsMayorAndJudgeFrame;
    public int m_MayorAndJudgeFrameIndex;

    //public bool m_isCertificate;
    //public Sprite m_ZoomImages;
    //public _w dialogue;

    //public bool _isSpecialPainting;
    //public Texture2D specialPainting_texture2D;


    private void OnMouseDown()
    {

        if (SelfieController.Instance.m_IsSelfieFeatureActive) return;

        if (!GalleryImageManager.Instance.m_IsDescriptionPanelActive && GalleryImageManager.Instance.CheckCanClickOnImage())
        {
            if (m_IsMayorAndJudgeFrame)
            {
                GalleryImageManager.Instance.LoadMayorAndJudgeFrame(m_MayorAndJudgeFrameIndex);
            }
            else if (m_isBanner)
            {
                GalleryImageManager.Instance.LoadBannerImagePanel(m_IsPortrait, m_PictureIndex);
            }
            else if (m_IsVideo)
            {
                GalleryImageManager.Instance.m_IsVideo = m_IsVideo;
                GalleryImageManager.Instance.LoadVideo(m_PictureIndex);
            }
            else
            {
                //string l_InfoText = m_NPCTrigger.GetComponent<NPC_DialogueTrigger>().info_textMesh.text;
                //dialogue = m_NPCTrigger.GetComponent<NPC_DialogueTrigger>().dialogue;
                GalleryImageManager.Instance.LoadPictureDescriptionPanel(m_PictureIndex, m_IsSpecialPainting);
            }
        }
    }

    //public void OnPictureClicked()
    //{
    //    if (SelfieController.Instance.m_IsSelfieFeatureActive) return;

    //    if (!GalleryImageManager.Instance.m_IsDescriptionPanelActive && GalleryImageManager.Instance.CheckCanClickOnImage())
    //    {
    //        if (m_IsMayorAndJudgeFrame)
    //        {
    //            GalleryImageManager.Instance.LoadMayorAndJudgeFrame(m_MayorAndJudgeFrameIndex);
    //        }
    //        else if (m_isBanner)
    //        {
    //            GalleryImageManager.Instance.LoadBannerImagePanel(m_IsPortrait, m_PictureIndex);
    //        }
    //        else if (m_IsVideo)
    //        {
    //            GalleryImageManager.Instance.m_IsVideo = m_IsVideo;
    //            GalleryImageManager.Instance.LoadVideo(m_PictureIndex);
    //        }
    //        else
    //        {
    //            //string l_InfoText = m_NPCTrigger.GetComponent<NPC_DialogueTrigger>().info_textMesh.text;
    //            //dialogue = m_NPCTrigger.GetComponent<NPC_DialogueTrigger>().dialogue;
    //            GalleryImageManager.Instance.LoadPictureDescriptionPanel(m_PictureIndex, m_IsSpecialPainting);
    //        }
    //    }
    //}
}