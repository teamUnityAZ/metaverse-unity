using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    public int Index;
    public Text BtnTxt;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(BtnClicked);
    }

    void BtnClicked()
    {
        print(gameObject.transform.parent.name);
        if (gameObject.transform.parent.name != "Accesary")
        {
            //if (GameManager.Instance.UserStatus_)
            //    PlayerPrefs.SetInt("IsLoggedIn", 1);  // was loggedin as account 
            //else
            //    PlayerPrefs.SetInt("IsLoggedIn", 0);  // was as a guest

            if (PlayerPrefs.GetInt("presetPanel") == 1)
            {

                
                PlayerPrefs.SetInt("presetPanel", 0);  // was loggedin as account 
                StoreManager.instance.GreyRibbonImage.SetActive(true);
                StoreManager.instance.WhiteRibbonImage.SetActive(false);
                StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;

                print("-----");
                DefaultEnteriesforManican.instance.DefaultReset();
                GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
                SavaCharacterProperties.instance.LoadMorphsfromFile();
            }
        }

        XanaConstants.xanaConstants.currentButtonIndex = Index;

        StoreManager.instance.DisableColorPanels();
        StoreManager.instance.UpdateStoreSelection(Index);

        GetComponentInParent<SubBottons>().ClickBtnFtn(Index);
    }
}
