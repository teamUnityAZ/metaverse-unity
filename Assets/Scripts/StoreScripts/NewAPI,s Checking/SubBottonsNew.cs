using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SubBottonsNew : MonoBehaviour
{
    public GameObject[] TotalBtns;
    public Color NormalColor;
    public Color HighlightedColor;
    public bool ClothBool;
    public bool AvatarBool;

    private CharacterCustomizationUIManager customizationUIManager;

    // Start is called before the first frame update
    void Start()
    {
        customizationUIManager = FindObjectOfType<CharacterCustomizationUIManager>();
    }

    private int currentSelectedCategoryIndex;

   public void ClickBtnFtn(int m_Index)
    {
         for (int i = 0; i < TotalBtns.Length; i++)
        {
            TotalBtns[i].GetComponent<ButtonScript>().BtnTxt.color = NormalColor;
         }  
        TotalBtns[m_Index].GetComponent<ButtonScript>().BtnTxt.color = HighlightedColor;
        if(ClothBool)
        {
            NewAPisStoreIntegtration.instance.OpenClothContainerPanelNewAPI(m_Index);
         }
        else if(AvatarBool)
        {
            NewAPisStoreIntegtration.instance.OpenAvatarContainerPanelNewAPI(m_Index);
        }    
      //   print(m_Index);
     }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
