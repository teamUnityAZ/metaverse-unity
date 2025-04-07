using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class ReactDataClass
{
    public int id;
    public string iconUrl;
}

public class ReactScreen : MonoBehaviour
{
    #region PUBLIC_VAR

    public Transform parent;
    public Transform exPressionparent;
    public Transform othersparent;
    public GameObject reactPrefab;
    public GameObject reactPrefabGestures;
    public GameObject reactPrefabOthers;

    public List<ReactEmote> reactDataClass = new List<ReactEmote>();
    public List<ReactGestures> reactDataClassGestures = new List<ReactGestures>();
    public List<ReactOthers> reactDataClassOthers = new List<ReactOthers>();
    public GameObject reactScreen;

    public bool isOpen = false;
    public Image reactImage;
    public Sprite react_disable;
    public Sprite react_enable;
    #endregion

    #region PRIVATE_VAR
    #endregion

    #region UNITY_METHOD
    #endregion

    #region PUBLIC_METHODS

    private Canvas reactScreenCanvas;//riken
    private GraphicRaycaster graphicRaycaster;//riken
    public void ReactButtonClick(bool isFromFevButton = false)
    {
        if (!PremiumUsersDetails.Instance.CheckSpecificItem("chat_reaction"))
        {
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }

        isOpen = !isOpen;
        if (isOpen)
        {
            reactImage.sprite = react_enable;
            reactScreen.SetActive(true);
            CreatePrefab();
        }
        else
        {
            reactImage.sprite = react_disable;
            reactScreen.SetActive(false);
        }

        if (isFromFevButton)
        {
            if (isOpen && reactScreenCanvas == null)
            {
                reactScreenCanvas = reactScreen.AddComponent<Canvas>();
                reactScreenCanvas.overrideSorting = true;
                reactScreenCanvas.sortingOrder = 6;
                graphicRaycaster = reactScreen.AddComponent<GraphicRaycaster>();
            }
            else
            {
                Destroy(graphicRaycaster);
                Destroy(reactScreenCanvas);
                graphicRaycaster = null;
                reactScreenCanvas = null;
            }
        }
    }

    //this method is used to show emote panel when user click on favorite button.......rik 
    public void OnShowEmotePanelFromFavorite()
    {
        if (isOpen)
        {
            if (reactScreenCanvas == null)
            {
                reactScreenCanvas = reactScreen.AddComponent<Canvas>();
                reactScreenCanvas.overrideSorting = true;
                reactScreenCanvas.sortingOrder = 6;
                graphicRaycaster = reactScreen.AddComponent<GraphicRaycaster>();
            }
        }
        else
        {
            ReactButtonClick(true);
        }
    }

    private void Start()
    {
        reactDataClass.Clear();
        reactDataClassGestures.Clear();
        reactDataClassOthers.Clear();
    }
    #endregion

    #region PRIVATE_METHODS
    private void CreatePrefab()
    {
        
        if (reactDataClass.Count == 0 || reactDataClassGestures.Count==0|| reactDataClassOthers.Count == 0)
        {
            ClearParent();
            StartCoroutine(getAllReactions());
        }
    
        
        
    }

    private void ClearParent()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion

    #region COROUTINE

    public IEnumerator getAllReactions()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        Debug.Log("Reaction URl========="+ ConstantsGod.API_BASEURL + ConstantsGod.GetAllReactions + "/" + APIBaseUrlChange.instance.apiversion);
        UnityWebRequest uwr = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.GetAllReactions + "/"+APIBaseUrlChange.instance.apiversion);
        try
        {
            if (UserRegisterationManager.instance.LoggedInAsGuest)
            {
                uwr.SetRequestHeader("Authorization", PlayerPrefs.GetString("GuestToken"));
            }
            else
            {
                uwr.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken"));
            }


        }
        catch (Exception e1)
        {
        }

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {

            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            try
            {

                Debug.Log("Response Reaction===" + uwr.downloadHandler.text.ToString().Trim());
               ReactionDetails bean = Gods.DeserializeJSON<ReactionDetails>(uwr.downloadHandler.text.ToString().Trim());
                if (bean.success)
                {
                    reactDataClass.Clear();
                    reactDataClassGestures.Clear();
                    reactDataClassOthers.Clear();

                    for(int i = 0; i < bean.data.reactionList.Count; i++)
                    {
                        if (bean.data.reactionList[i].group.Equals("Emote"))
                        {

                            ReactEmote bean1 = new ReactEmote();
                            bean1.thumb = bean.data.reactionList[i].thumbnail;
                            bean1.mainImage = bean.data.reactionList[i].icon3d;
                            //GameObject newItem = Instantiate(reactPrefab, Vector3.zero, Quaternion.identity, parent);
                            //newItem.GetComponent<ReactItem>().SetData(bean.data.reactionList[i].thumbnail + "?width=50&height=50", bean.data.reactionList[i].icon3d,i);
                            reactDataClass.Add(bean1);
                        }
                        else if(bean.data.reactionList[i].group.Equals("Gestures"))
                        {
                            //GameObject newItem = Instantiate(reactPrefabExpression, Vector3.zero, Quaternion.identity, exPressionparent);
                            //newItem.GetComponent<ReactItem>().SetData(bean.data.reactionList[i].thumbnail + "?width=50&height=50", bean.data.reactionList[i].icon3d, i);
                            ReactGestures bean1 = new ReactGestures();
                            bean1.thumb = bean.data.reactionList[i].thumbnail;
                            bean1.mainImage = bean.data.reactionList[i].icon3d;
                            //GameObject newItem = Instantiate(reactPrefab, Vector3.zero, Quaternion.identity, parent);
                            //newItem.GetComponent<ReactItem>().SetData(bean.data.reactionList[i].thumbnail + "?width=50&height=50", bean.data.reactionList[i].icon3d,i);
                            reactDataClassGestures.Add(bean1);
                        }
                        else if (bean.data.reactionList[i].group.Equals("Others"))
                        {
                            //GameObject newItem = Instantiate(reactPrefabExpression, Vector3.zero, Quaternion.identity, exPressionparent);
                            //newItem.GetComponent<ReactItem>().SetData(bean.data.reactionList[i].thumbnail + "?width=50&height=50", bean.data.reactionList[i].icon3d, i);
                            ReactOthers bean1 = new ReactOthers();
                            bean1.thumb = bean.data.reactionList[i].thumbnail;
                            bean1.mainImage = bean.data.reactionList[i].icon3d;
                            //GameObject newItem = Instantiate(reactPrefab, Vector3.zero, Quaternion.identity, parent);
                            //newItem.GetComponent<ReactItem>().SetData(bean.data.reactionList[i].thumbnail + "?width=50&height=50", bean.data.reactionList[i].icon3d,i);
                            reactDataClassOthers.Add(bean1);
                        }

                    }
                }

              //  int emotCount = reactDataClass.Count;
                //int ExpressionCount = reactDataClassExpression.Count;
                for (int i = 0; i < reactDataClass.Count; i++)
                {
                    //if (bean.data.reactionList[i].group.Equals("Emote"))
                    //{
                    GameObject newItem = Instantiate(reactPrefab, Vector3.zero, Quaternion.identity, parent);
                    newItem.GetComponent<ReactItem>().SetData(reactDataClass[i].thumb + "?width=50&height=50", reactDataClass[i].mainImage, i);
                    //    //  reactDataClass.Add(bean.data.reactionList[i].thumbnail);
                    //}
                    //else if (bean.data.reactionList[i].group.Equals("expression"))
                    //{
                    //    GameObject newItem = Instantiate(reactPrefabExpression, Vector3.zero, Quaternion.identity, exPressionparent);
                    //    newItem.GetComponent<ReactItem>().SetData(bean.data.reactionList[i].thumbnail + "?width=50&height=50", bean.data.reactionList[i].icon3d, i);
                    //    // reactDataClassExpression.Add(bean.data.reactionList[i].thumbnail);
                    //}

                    // GameObject newItem1 = Instantiate(reactPrefab, Vector3.zero, Quaternion.identity, exPressionparent);

                    // newItem.GetComponent<ReactItem>().SetData(reactDataClass[i]+ "?width=50&height=50", i);
                }

                for(int j = 0; j < reactDataClassGestures.Count; j++)
                {
                    Debug.Log("Getures thumb===="+ reactDataClassGestures[j].thumb);
                    GameObject newItem = Instantiate(reactPrefab, Vector3.zero, Quaternion.identity, exPressionparent);
                    newItem.GetComponent<ReactItem>().SetData(reactDataClassGestures[j].thumb + "?width=50&height=50", reactDataClassGestures[j].mainImage, j);
                }
                for (int j = 0; j < reactDataClassOthers.Count; j++)
                {
                    Debug.Log("others thumb====" + reactDataClassOthers[j].thumb);
                    GameObject newItem = Instantiate(reactPrefab, Vector3.zero, Quaternion.identity, othersparent);
                    newItem.GetComponent<ReactItem>().SetData(reactDataClassOthers[j].thumb + "?width=50&height=50", reactDataClassOthers[j].mainImage, j);
                }

            }
            catch
            {

            }
        }
    }


    #endregion

    #region DATA

    public class ReactEmote
    {
        public string thumb { get; set; }
        public string mainImage { get; set; }
    }

    public class ReactGestures
    {
        public string thumb { get; set; }
        public string mainImage { get; set; }
    }
    public class ReactOthers
    {
        public string thumb { get; set; }
        public string mainImage { get; set; }
    }

    public class ReactionList
    {
        public int id { get; set; }
        public string name { get; set; }
        public object android_bundle { get; set; }
        public object ios_bundle { get; set; }
        public string thumbnail { get; set; }
        public int version { get; set; }
        public string group { get; set; }
        public string icon3d { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Data
    {
        public List<ReactionList> reactionList { get; set; }
    }

    public class ReactionDetails
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public string msg { get; set; }
    }
    #endregion

}
