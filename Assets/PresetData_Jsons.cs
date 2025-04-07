using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PresetData_Jsons : MonoBehaviour
{

    public string JsonDataPreset;
    private string PresetNameinServer = "Presets";
    // Start is called before the first frame update
    public static string clickname;
    public bool IsStartUp_Canvas;   // if preset panel is appeared on start thn allow it to change 

    public static GameObject lastSelectedPreset=null;
    void Start()  
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ChangecharacterOnCLickFromserver);
    }
    public void callit()
    {
        clickname = "";
    }
    void ChangecharacterOnCLickFromserver()
    {

        try
        {
            if (lastSelectedPreset != null)
            {
                lastSelectedPreset.gameObject.SetActive(false);
                lastSelectedPreset =this.transform.GetChild(0).gameObject;
                lastSelectedPreset.SetActive(true);
            }
            else
            {
                lastSelectedPreset = this.transform.GetChild(0).gameObject;
                lastSelectedPreset.SetActive(true);
            }
        }
        catch(Exception e)
        {
            lastSelectedPreset = null;
        }

        if (!IsStartUp_Canvas)   //for presets in avatar panel 
         {
            if (clickname != gameObject.name)
                clickname = gameObject.name;
            else
                return;
        }
    //    print("Calling cloths");
        if (!PremiumUsersDetails.Instance.CheckSpecificItem(PresetNameinServer))
        {
            //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
             print("Please Upgrade to Premium account");
            return;  
        }  
        else
        {
            print("Horayyy you have Access");
        }
          


        PlayerPrefs.SetInt("presetPanel", 1);

        // Hack for latest update // keep all preset body fat to 0
        //change lipsto default
        SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
        _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(JsonDataPreset);  //(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
        print(_CharacterData.BodyFat);
        //Lips Default
        _CharacterData.myItemObj[5].ItemLinkAndroid = "";
        _CharacterData.myItemObj[5].ItemName="";
        _CharacterData.myItemObj[5].ItemID=0;  
        //Lips
        _CharacterData.BodyFat = 0;
        File.WriteAllText((Application.persistentDataPath + "/SavingReoPreset.json"), JsonUtility.ToJson(_CharacterData));
 
        //   File.WriteAllText((Application.persistentDataPath + "/SavingReoPreset.json"), JsonDataPreset);

        // DefaultEnteriesforManican.instance.DefaultReset();
        DefaultEnteriesforManican.instance.ResetForPresets();
        GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
        SavaCharacterProperties.instance.LoadMorphsfromFile();
        StoreManager.instance.UndoSelection();
        //Enable save button

        if (StoreManager.instance.StartPanel_PresetParentPanel.activeSelf)
        {

            if (PlayerPrefs.GetInt("iSignup")==1)
            {
                //for register
                //   PlayerPrefs.SetInt("iSignup", 1);
                //UserRegisterationManager.instance.OpenUIPanal(8);
                //    ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
                 Invoke("abcd", 2.0f);

                //  PlayerPrefs.SetInt("presetPanel", 0);
              //  if (PlayerPrefs.GetInt("presetPanel") == 1)   // preset panel is enable so saving preset to account 
              //      PlayerPrefs.SetInt("presetPanel", 0);

              ///  ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
                StoreManager.instance.StartPanel_PresetParentPanel.SetActive(false);
               
                //   

            }
            else                // as a guest
            {

                if (!lastSelectedPreset.activeInHierarchy)
                    lastSelectedPreset.SetActive(true);
                StoreManager.instance.StartPanel_PresetParentPanel.SetActive(false);
                UserRegisterationManager.instance.usernamePanal.SetActive(true);
                // enable check so that it will know that index is comming from start of the game
                UserRegisterationManager.instance.checkbool_preser_start = false;
            }
        }
        else
        {
            StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
            StoreManager.instance.GreyRibbonImage.SetActive(false);
            StoreManager.instance.WhiteRibbonImage.SetActive(true);
        }

        //  PlayerPrefs.SetInt("IsLoggedIn", 0);
        if (!lastSelectedPreset.activeInHierarchy)
            lastSelectedPreset.SetActive(true);
    }
    void abcd()
    {
        print("Coroutin Called " + PlayerPrefs.GetInt("presetPanel"));
        if (PlayerPrefs.GetInt("presetPanel") == 1)   // preset panel is enable so saving preset to account 
            PlayerPrefs.SetInt("presetPanel", 0);
        ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
    }
}
