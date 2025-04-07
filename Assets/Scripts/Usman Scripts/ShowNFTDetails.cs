using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowNFTDetails : MonoBehaviour
{
    public GameObject displayPanel;
    public GameObject imageObject;
    public GameObject videoObject;
    public Text descriptionText, titleText, usernameText, ownerText;
    public string nftlinkText;
    public GameObject creatorimageObject;
    public GameObject ownerimageObject;
    public string ownerlinkText;
    public static ShowNFTDetails instance;
    public Sprite dummyprofileIcone;
    
    MetaDataInPrefab currData;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (displayPanel != null)
        {
            displayPanel.layer = LayerMask.NameToLayer("NFTDisplayPanel");
        }
       
    }

    public void ShowImage(MetaDataInPrefab mdip)
    {
        currData = mdip;
        imageObject.GetComponent<Image>().preserveAspect = true;
        imageObject.GetComponent<Image>().sprite = mdip.thunbNailImage;
        creatorimageObject.GetComponent<Image>().preserveAspect = true;
        creatorimageObject.GetComponent<Image>().sprite = mdip.thunbNailImage1;
        ownerimageObject.GetComponent<Image>().preserveAspect = true;
        ownerimageObject.GetComponent<Image>().sprite = mdip.thunbNailImage1;
        imageObject.SetActive(true);
        creatorimageObject.SetActive(true);
        ownerimageObject.SetActive(true);
        videoObject.SetActive(false);
        displayPanel.SetActive(true);
        usernameText.text = "" + mdip.creatorDetails.username;
        ownerText.text = "" + mdip.creatorDetails.username;
        nftlinkText = mdip.nftLink;
        ownerlinkText = mdip.creatorLink;
        if (GameManager.currentLanguage == "ja")
        {
            titleText.text = "" + mdip.metaData.name;
            descriptionText.text = "" + mdip.tokenDetails.ja_nft_description;
        }
        else {

            titleText.text = "" + mdip.metaData.name;
            descriptionText.text = "" + mdip.tokenDetails.en_nft_description;

        }
    }
    public void ShowVideo(MetaDataInPrefab mdip)
    {
        currData = mdip;
        //if (videoObject.transform.childCount > 0)
        //{
        //    for(int i= videoObject.transform.childCount - 1; i >= 0; i--)
        //    {
        //        Destroy(videoObject.transform.GetChild(i));
        //    }
        //}
        videoObject.transform.GetComponent<RawImage>().texture = mdip.videoPlayer.texture;
        if (mdip.thunbNailImage.texture.height / mdip.thunbNailImage.texture.width > 1.776f)
        {
            videoObject.transform.GetChild(0).GetComponent<RawImage>().rectTransform.localScale =
                new Vector3(1 / ((mdip.thunbNailImage.texture.height * 1f) / (mdip.thunbNailImage.texture.width * 1f)), 1f, 1);
            //print("Hight of Im is:"+mdip.thunbNailImage.texture.height);
        }
        else if (mdip.metaData.name.Contains("磁場の拡散（Diffusion of magnetic field）"))
        {
            videoObject.transform.GetComponent<RawImage>().rectTransform.localScale =
                new Vector3(0.8f, 0.7f / ((mdip.thunbNailImage.texture.height * 1f) / (mdip.thunbNailImage.texture.width * 1f)), 1);
           // print("Hight of else Ima is:" + mdip.thunbNailImage.texture.height);
        }
        else if (mdip.metaData.name.Contains("MODEL-typeC-"))
        {
            videoObject.transform.GetComponent<RawImage>().rectTransform.localScale =
                new Vector3(0.8f, 0.7f / ((mdip.thunbNailImage.texture.height * 1f) / (mdip.thunbNailImage.texture.width * 1f)), 1);
           // print("Hight of else Ima is:" + mdip.thunbNailImage.texture.height);
        }
        else if (mdip.metaData.name.Contains("In And Out"))
        {
            videoObject.transform.GetComponent<RawImage>().rectTransform.localScale =
                new Vector3(0.8f, 0.7f / ((mdip.thunbNailImage.texture.height * 1f) / (mdip.thunbNailImage.texture.width * 1f)), 1);
            // print("Hight of else Ima is:" + mdip.thunbNailImage.texture.height);
        }
        else if (mdip.metaData.name.Contains("skull"))
        {
            videoObject.transform.GetComponent<RawImage>().rectTransform.localScale =
                new Vector3(1f, 1f / ((mdip.thunbNailImage.texture.height * 1f) / (mdip.thunbNailImage.texture.width * 1f)), 1);
            // print("Hight of else Ima is:" + mdip.thunbNailImage.texture.height);
        }
        else if (mdip.thunbNailImage.texture.height / mdip.thunbNailImage.texture.width < 1.5f)
        {
            videoObject.transform.GetComponent<RawImage>().rectTransform.localScale =
                  new Vector3(1, 1.334f / ((mdip.thunbNailImage.texture.height * 1f) / (mdip.thunbNailImage.texture.width * 1f)), 1);
           // print("Hight of else Ima is:" + mdip.thunbNailImage.texture.height);
        }
        creatorimageObject.GetComponent<Image>().preserveAspect = true;
        creatorimageObject.GetComponent<Image>().sprite = mdip.thunbNailImage1;
        ownerimageObject.GetComponent<Image>().preserveAspect = true;
        ownerimageObject.GetComponent<Image>().sprite = mdip.thunbNailImage1;
        videoObject.transform.rotation = Quaternion.identity;
        mdip.videoPlayer.SetDirectAudioMute(0, false);
        usernameText.text = "" + mdip.creatorDetails.username;
        ownerText.text = "" + mdip.creatorDetails.username;
        nftlinkText = mdip.nftLink;
        ownerlinkText = mdip.creatorLink;
        if (GameManager.currentLanguage == "ja")
        {
            titleText.text = "" + mdip.metaData.name;
            descriptionText.text = "" + mdip.tokenDetails.ja_nft_description;
        }
        else
        {

            titleText.text = "" + mdip.metaData.name;
            descriptionText.text = "" + mdip.tokenDetails.en_nft_description;

        }
        //usernameText.text = "" + mdip.metaData.username;
        //videoObject.transform.GetChild(0).GetComponent<RawImage>().mainTexture.width = mdip.thunbNailImage.texture.width;
        //Instantiate(mdip.spriteObject, this.transform.position, this.transform.rotation, videoObject.transform);
        //videoObject.GetComponent<Image>().sprite = mdip.GetComponent<Image>().sprite;
        imageObject.SetActive(false);
        videoObject.SetActive(true);
        displayPanel.SetActive(true);
    }

    public void UpdateGif(Sprite currFrame)
    {
        imageObject.GetComponent<Image>().sprite = currFrame;
       
    }


    public void ClosePanel()
    {
        LoadingHandler.Instance.OnCloasenft();
        ReferrencesForDynamicMuseum.instance.forcetoenable();
        displayPanel.SetActive(false);
        if (currData.videoPlayer != null)
            currData.videoPlayer.SetDirectAudioMute(0, true);
        currData.isVisible = false;
        if(currData.videoPlayer != null)
            Destroy(currData.videoPlayer.gameObject);
        currData = null;
       

    }
    public void directtoUrl()
    {
        Application.OpenURL(nftlinkText);

    }
    public void owneroUrl()
    {
        Application.OpenURL(ownerlinkText);

    }
}
