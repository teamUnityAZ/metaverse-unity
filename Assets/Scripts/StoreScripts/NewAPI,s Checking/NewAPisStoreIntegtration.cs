using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class NewAPisStoreIntegtration : MonoBehaviour
{
    public static NewAPisStoreIntegtration instance;
    public string Token;
    public string GetAllCategoriesAPI;
    public string GetAllSubCategoriesAPI;
    public string GetAllItems;
    public GameObject ClothMainPanelObj;
     public GameObject[] ClothsMainPanel;
     public GameObject[] ClothsPanel;
    public GameObject ClothBtnsPanel;
    public GameObject ClothLine;  
     public GameObject AvatarMainPanelObj;
     public GameObject[] AvatarMainPanel;
    public GameObject AvatarBtnsPanel;
     public GameObject[] AvatarPanel;
    public GameObject AvatarLine;
     // Containers
    GetAllInfoMainCategories ObjofMainCategory;
    string[] ArrayofMainCategories;
    List <ItemsofSubCategories> SubCategoriesList ;
     private bool CheckAPILoaded;
      public GameObject Store;
    public GameObject PreStore;
 
    // Start is called before the first frame update
    void Start()
    {
        CheckAPILoaded = false;
         GetAllMainCategories();
    }
    public void GotoStore()
    {
        Store.SetActive(true);
        PreStore.SetActive(false);
        OpenMainPanels(0);
     }
    public void BackFromStore()
    {
        Store.SetActive(false);
        PreStore.SetActive(true);
    } 
    void Awake()
    {
        instance = this;
    }
    public void OpenMainPanels(int _GetIndex)
    {
        if (_GetIndex == 0)
        {
            ClothMainPanelObj.SetActive(true);
            AvatarMainPanelObj.SetActive(false);
            ClothLine.SetActive(true);
            AvatarLine.SetActive(false);
            ClothBtnsPanel.GetComponent<SubBottonsNew>().ClickBtnFtn(0);
         }
        else if (_GetIndex == 1)
        {
            ClothMainPanelObj.SetActive(false);
            AvatarMainPanelObj.SetActive(true);
            ClothLine.SetActive(false);
            AvatarLine.SetActive(true);
            AvatarBtnsPanel.GetComponent<SubBottonsNew>().ClickBtnFtn(0);
         }  
    }  
  
    public void OpenClothContainerPanelNewAPI(int m_GetIndex)
    {
        for (int i = 0; i < ClothsPanel.Length; i++)
        {
            ClothsPanel[i].SetActive(false);
        }
        ClothsPanel[m_GetIndex].SetActive(true);
         if(CheckAPILoaded)
        {
            if (SubCategoriesList.Count > 0)
            {
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex].id);
            }
        }
        else
        {
            StartCoroutine(WaitForAPICallCompleted(m_GetIndex));
         }
      }

     public void OpenAvatarContainerPanelNewAPI(int m_GetIndex)
    {
        for (int i = 0; i < AvatarPanel.Length; i++)
        {
            AvatarPanel[i].SetActive(false);
        }
        AvatarPanel[m_GetIndex].SetActive(true);
        if (CheckAPILoaded)
        {
            if (SubCategoriesList.Count > 0)
            {
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex + 8].id);
            }
        }
        else
        {
            StartCoroutine(WaitForAPICallCompleted(m_GetIndex));
        }
    }  
    IEnumerator WaitForAPICallCompleted(int m_GetIndex)
    {
        print("wait Until");
        yield return new WaitUntil(() => CheckAPILoaded == true);
        if (CheckAPILoaded)
        {
            print("wait Until completed");
            if (SubCategoriesList.Count > 0)
            {
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex].id);
            }
        }

    }
    // **************************** Get Items by Sub categories ******************************//
    private string StringIndexofSubcategories(int _index)
    {
         var result = string.Join(",", _index.ToString());  
        result = "[" + result + "]";     
        return result;   
    }
    [System.Serializable]
    public class ConvertSubCategoriesToJsonObj
    {
        public string subCategories;
        public int pageNumber;
        public int pageSize;
        public ConvertSubCategoriesToJsonObj CreateTOJSON(string jsonString, int _pageNumber, int _PageSize)
        {
            ConvertSubCategoriesToJsonObj myObj = new ConvertSubCategoriesToJsonObj();
            myObj.subCategories = jsonString;
            myObj.pageNumber = _pageNumber;
            myObj.pageSize = _PageSize;
             return myObj;
        }
    }
     private void SubmitAllItemswithSpecificSubCategory(int GetCategoryIndex)
    {
        string result = StringIndexofSubcategories(GetCategoryIndex);
        ConvertSubCategoriesToJsonObj SubCatString = new ConvertSubCategoriesToJsonObj();
           string bodyJson = JsonUtility.ToJson(SubCatString.CreateTOJSON(result, 1, 10));
          StartCoroutine(HitALLItemsAPI(GetAllItems, bodyJson));
    }
    IEnumerator HitALLItemsAPI(string url, string Jsondata)
    {  
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", Token);
        yield return request.SendWebRequest();
         GetItemInfo JsonDataObj = new GetItemInfo();
        JsonDataObj = JsonUtility.FromJson<GetItemInfo>(request.downloadHandler.text);
  
          if (!request.isHttpError && !request.isNetworkError)
          {
              if (request.error == null)
              {
                   if (JsonDataObj.success == true)
                  {
                      print(JsonDataObj.data[0].items.Count);  
                        print("All Categories are here");
                  }
              }  
          }
          else
          {
              if (request.isNetworkError)
              {
                  print("Network Error");
              }
              else
              {
                  if (request.error != null)
                  {
                      if (JsonDataObj.success == false)
                      {
                          print("Hey success false " + JsonDataObj.msg);
                      }
                  }
              }
          }
     }
  
    [System.Serializable]
    public class GetItemInfo
    {
        public bool success;
        public List<DataList> data;
          public string msg;
    }
         [System.Serializable]
        public class DataList
        {
            public int id;
            public int categoryId;
            public string name;
            public string createdAt;
            public string updatedAt;
            public List<TotaItemsClass> items;
        }
 
     [System.Serializable]
    public class TotaItemsClass
    {
        public int id;
        public string assetLinkAndroid;
        public string assetLinkIos;
        public string assetLinkWindows;
        public string iconLink;
        public int categoryId;
        public int subCategoryId;
        public string name;
        public bool isPaid;
        public string price;
        public bool isPurchased;
        public bool isFavourite;
        public bool isOccupied;
        public bool isDeleted;
        public string createdBy;
        public string createdAt;
        public string updatedAt;
        public string[] itemTags;
    }
     // **************************** Get Items by Sub categories ENDSSSSS ******************************//
  


    // **************************************************************************************************//
    ////////////////////////// <MAIN Category Started Here> ///////////////////////////////////////////////
    public void GetAllMainCategories()
    {
       StartCoroutine(HitAllMainCategoriesAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMCATEGORY, ""));
    }    
    IEnumerator HitAllMainCategoriesAPI(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {  
            request.SetRequestHeader("Authorization", Token);
            yield return request.SendWebRequest();
             ObjofMainCategory = GetAllData(request.downloadHandler.text);
           if (!request.isHttpError && !request.isNetworkError)
         {
             if (request.error == null)
             {
                  if (ObjofMainCategory.success == true)
                 {
                     print(ObjofMainCategory.data.Count);
                        SaveAllMainCategoriesToArray();
                        
                    }  
             }  
         }
         else
         {
             if (request.isNetworkError)
             {
                 print("Network Error");
             }
             else
             {
                 if (request.error != null)
                 {
                     if (ObjofMainCategory.success == false)
                     {
                         print("Hey success false " + ObjofMainCategory.msg);
                     }
                 }
             }
         }
         }
    }
    public void SaveAllMainCategoriesToArray()
    {
        List<string> purchaseItemsIDs = new List<string>();
        for (int i = 0; i < ObjofMainCategory.data.Count; i++)
        {
             purchaseItemsIDs.Add(ObjofMainCategory.data[i].id);
        }  
        ArrayofMainCategories = purchaseItemsIDs.ToArray();
        GetAllSubCategories();
    }
 
    public GetAllInfoMainCategories GetAllData(string m_JsonData)
    {
        GetAllInfoMainCategories JsonDataObj = new GetAllInfoMainCategories();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoMainCategories>(m_JsonData);  
         return JsonDataObj;
    }
    [System.Serializable]
    public class GetAllInfoMainCategories
    {
        public bool success;
         public List<ItemsParents> data;  
         public string msg;
    }
     [System.Serializable]
     public class ItemsParents
    {
        public string id;
        public string name;
        public string createdAt;
        public string updatedAt;
    }
    ////////////////////////// <MAIN Category ENDS here> ///////////////////////////////////////////////



    ///////////////////////// ************************** //////////////////////////////////////////////
    ////////////////////////// <SUB Category STARTS here> ///////////////////////////////////////////////
     private string AccessIndexOfSpecificCategory()
    {
         var result = string.Join(",", ArrayofMainCategories);
         result = "[" + result + "]";
        return result;
    }
    [System.Serializable]
    public class ConvertMainCat_Index_ToJson
    {
        public string categories;
        public ConvertMainCat_Index_ToJson CreateTOJSON(string jsonString)
        {
            ConvertMainCat_Index_ToJson myObj = new ConvertMainCat_Index_ToJson();
            myObj.categories = jsonString;
            return myObj;
        }
    }
     public void GetAllSubCategories()
    {
        string result =AccessIndexOfSpecificCategory();
        ConvertMainCat_Index_ToJson MainCatString = new ConvertMainCat_Index_ToJson();
        string bodyJson = JsonUtility.ToJson(MainCatString.CreateTOJSON(result));
        print("Stringify array Json is " + bodyJson);
        StartCoroutine(HitSUBCategoriesAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMSUBCATEGORY, bodyJson));
    }      
    
   IEnumerator HitSUBCategoriesAPI(string url, string Jsondata)
    {
          var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", Token);
         yield return request.SendWebRequest();
        GetAllInfoSUBOFCategories JsonDataObj = new GetAllInfoSUBOFCategories();
       JsonDataObj = GetDataofSUBCategories(request.downloadHandler.text);  
       if (!request.isHttpError && !request.isNetworkError)
       {
           if (request.error == null)
           {
               Debug.Log(request.downloadHandler.text);
               if (JsonDataObj.success == true)
               {
                   print(JsonDataObj.data.Count);
                    SubCategoriesList = JsonDataObj.data;
                     CheckAPILoaded = true;
                     print("All Categories are here");
               }   
           }
       }
       else
       {
           if (request.isNetworkError)
           {
                CheckAPILoaded = true;

                print("Network Error");
           }
           else
           {
               if (request.error != null)
               {
                   if (JsonDataObj.success == false)
                   {
                        CheckAPILoaded = true;
                         print("Hey success false " + JsonDataObj.msg);
                   }
               }
           }
       }
     }
    public GetAllInfoSUBOFCategories GetDataofSUBCategories(string m_JsonData)
    {
        GetAllInfoSUBOFCategories JsonDataObj = new GetAllInfoSUBOFCategories();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoSUBOFCategories>(m_JsonData);
        return JsonDataObj;
    }
 
     [System.Serializable]
    public class GetAllInfoSUBOFCategories
    {
        public bool success;
        public List<ItemsofSubCategories> data;
        public string msg;
    }
    [System.Serializable]
    public class ItemsofSubCategories
    {
        public int id;
        public int categoryId;  
        public string name;
        public string createdAt;
        public string updatedAt;
    }
    //***************************** Get All Sub Categories END here **************************//
 }
