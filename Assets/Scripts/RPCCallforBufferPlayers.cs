
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Voice.Unity;
using System.Linq;
using System.IO;
using Photon.Pun.Demo.PunBasics;
public class RPCCallforBufferPlayers : MonoBehaviour, IPunInstantiateMagicCallback
{
    [HideInInspector]
    public AssetBundle bundle;
    public AssetBundleRequest newRequest;
    private string OtherPlayerId;
    public static List<string> bundle_Name = new List<string>();
    private bool ItemAlreadyExists = false;
    bool NeedtoDownload = true;
    bool NotNeedtoDownload = true;
    string ClothSlugName = "";
    [SerializeField]
    public static Dictionary<object, object> allPlayerIdData = new Dictionary<object, object>();
    object[] _mydatatosend= new object[2];
    public string GetJsonFolderData()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)  // loged from account)
        {
            return File.ReadAllText(Application.persistentDataPath + "/SavingCharacterDataClass.json");
        }
        else
        {
            return File.ReadAllText(Application.persistentDataPath + "/loginAsGuestClass.json");
        }
    }
    private void Start()
    {
        if (this.GetComponent<PhotonView>().IsMine )
        {
            _mydatatosend[0] = GetComponent<PhotonView>().ViewID as object;
            _mydatatosend[1] = GetJsonFolderData() as object;
            GetComponent<PhotonView>().RPC("CheckRpc", RpcTarget.OthersBuffered, _mydatatosend as object);
        }
        if (!this.GetComponent<PhotonView>().IsMine && !this.gameObject.GetComponent<Speaker>())
        {
            this.gameObject.AddComponent<Speaker>();
        }
    }

   public  void OnPhotonInstantiate(PhotonMessageInfo info)
    {
       Launcher.instance.playerobjects.Add(info.photonView.gameObject);
    }
    [PunRPC]
    void CheckRpc(object[] Datasend)
    {

        string SendingPlayerID = Datasend[0].ToString();
        OtherPlayerId = Datasend[0].ToString();
        SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
        _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(Datasend[1].ToString());

        for (int j = 0; j < Launcher.instance.playerobjects.Count; j++)
        {
            if (Launcher.instance.playerobjects[j].GetComponent<PhotonView>().ViewID.ToString() == OtherPlayerId)
                for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
                {
                    if (!Launcher.instance.playerobjects[j].GetComponent<PhotonView>().IsMine)
                   {
                    ClothSlugName = "";
                    StartCoroutine(processIenum(_CharacterData.myItemObj[i].ItemLinkAndroid, _CharacterData.myItemObj[i].ItemLinkIOS, _CharacterData.myItemObj[i].ItemType, SendingPlayerID, _CharacterData.myItemObj[i].ItemName, j));
                   }
                }
        }
    }
    public IEnumerator GetAssetBundle(string androidURL,string iosurl,string itemtype, string i, string clothname, int CharacterIndex)
    {
        string tempname = clothname;
        NeedtoDownload = true;
        NotNeedtoDownload = true;
       
        switch (itemtype)
        {
            case "Lip":
            case "Skin":
            case "Eyes":
                if (!string.IsNullOrEmpty(iosurl))
                {
                    Debug.Log("Texture URL Print " + iosurl);
                    if (File.Exists(Application.persistentDataPath + "/" + tempname))
                    {
                        JustBindCall(itemtype, tempname, i, CharacterIndex);
                    }
                    else
                    {
                        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(iosurl))
                        {
                            yield return uwr.SendWebRequest();
                            if (uwr.isNetworkError)
                            {
                                Debug.Log(uwr.error);
                            }
                            else
                            {
                                Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                                byte[] fileData = tex.EncodeToPNG();
                                string filePath = Application.persistentDataPath + "/" + tempname;
                                File.WriteAllBytes(filePath, fileData);
                                uwr.Dispose();
                                JustBindCall(itemtype, tempname, i, CharacterIndex);
                            }

                            uwr.Dispose();
                        }
                    }
                }
                break;  
           default:
                string tempurl;

#if UNITY_ANDROID
                if (!string.IsNullOrEmpty(androidURL))
                    tempurl = androidURL;
                else 
                    tempurl = iosurl;
#else
  tempurl = iosurl;
#endif
                if (!string.IsNullOrEmpty(tempurl))
                {

                    print("Cloth URL is " + tempurl);

                    if (ItemDatabase.instance.itemList.Any(x => x.ItemName == tempname))
                    {
                        NeedtoDownload = false;
                        NotNeedtoDownload = false;
                        ClothSlugName = tempname;
                    }
                    if (!NotNeedtoDownload)
                    {
                        yield return new WaitForSeconds(2.0f);

                        JustBindCall(itemtype, ClothSlugName, i, CharacterIndex);
                    }
                    if (NeedtoDownload)
                    {
                        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(tempurl))
                        {
                            bundle = null;

                            yield return uwr.SendWebRequest();
                            if (uwr.isNetworkError)
                            {
                                Debug.Log(uwr.error);
                            }
                            else
                            {
                                if (bundle != null)
                                {
                                    bundle.Unload(false); //scene is unload from here
                                }
                                while (!Caching.ready)
                                    yield return null;

                                if (!string.IsNullOrEmpty(uwr.error))
                                {
                                    Debug.Log(uwr.error);
                                    yield return null;
                                }
                                try
                                {
                                    if (ItemDatabase.instance.itemList.Any(x => x.ItemName != tempname))
                                    {
                                        bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                                        uwr.Dispose();
                                        switchCaseCall(itemtype, tempname, i, CharacterIndex);
                                    }
                                    else
                                    {
                                        JustBindCall(itemtype, ClothSlugName, i, CharacterIndex);
                                    }
                                }
                                catch (Exception e)
                                {
                                    print(e.Message);
                                }
                            }
                            uwr.Dispose();
                        }

                        yield return null;
                    }
                }
                break;
        }
    }
    private void switchCaseCall(string name, string clothName, string otherid, int characterIndex)
    {    
        ItemAlreadyExists = false;
        print(name + " My Category name is that");
        print(clothName + " My cloth name is that");
        switch (name)
        {
            case "Legs":
            case "Chest":
            case "Feet":
            case "Hair":
                for (int x = 0; x < ItemDatabase.instance.itemList.Count; x++)
                {           
                    if (ItemDatabase.instance.itemList.Any(m => m.ItemName ==clothName))  
                    {
                        ItemAlreadyExists = true;
                    }
                }
                if (ItemAlreadyExists == false)
                    ItemDatabase.instance.itemList.Add(new Item(UnityEngine.Random.Range(0, 1000), clothName, "", clothName, name, bundle.LoadAsset(bundle.name) as GameObject, "","", ""));           
                    try
                    {
                    Launcher.instance.playerobjects[characterIndex].GetComponent<DefaultClothes>().ChangeCostume(clothName);
                    }
                    catch (Exception e)
                    {
                        print(e.Message);
                    }
                break;
        }
    }
    private void JustBindCall(string Itemtype, string clothName , string playerid, int CharacterIndex)
    {      
       print("name = " + name + "  clothName + " + clothName+ " Character index" + CharacterIndex );
        switch (Itemtype)
        {
            case "Legs":
            case "Chest":
            case "Feet":
            case "Hair":
                    try
                    {
                    Launcher.instance.playerobjects[CharacterIndex].GetComponent<DefaultClothes>().ChangeCostume(clothName);
                    }
                    catch (Exception e)
                    {
                        print(e.Message);
                    }
                break;

            case "Eyes":
                Launcher.instance.playerobjects[CharacterIndex].GetComponent<DefaultClothes>().myeyeball1.material.mainTexture = LoadTextuerfromFIle(clothName);
                Launcher.instance.playerobjects[CharacterIndex].GetComponent<DefaultClothes>().myeyeball2.material.mainTexture = LoadTextuerfromFIle(clothName);
                break;
            case "Skin":
                Launcher.instance.playerobjects[CharacterIndex].GetComponent<DefaultClothes>().MyHead.GetComponent<Renderer>().materials[0].mainTexture = LoadTextuerfromFIle(clothName); 
                        for (int i = 0; i < Launcher.instance.playerobjects[CharacterIndex].GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
                        {
                            if (Launcher.instance.playerobjects[CharacterIndex].GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                        Launcher.instance.playerobjects[CharacterIndex].GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = LoadTextuerfromFIle(clothName); 
                        }
                break;
            case "Lip":
                Launcher.instance.playerobjects[CharacterIndex].GetComponent<DefaultClothes>().MyHead.GetComponent<Renderer>().materials[1].mainTexture = LoadTextuerfromFIle(clothName);
                break;
        }
    }
    public Texture2D LoadTextuerfromFIle(string name)
    {
        byte[] _bytes;
        Texture2D mytexture;

        _bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + name);
        if (_bytes == null)
            return null;
        mytexture = new Texture2D(1, 1,TextureFormat.DXT5,false);
        mytexture.LoadImage(_bytes);

        return mytexture;
    }
    public IEnumerator processIenum(string androidUrl,string IOSurl,string itemtype, string j, string clothname, int characterIndex)
    {
        yield return StartCoroutine(GetAssetBundle(androidUrl,IOSurl,itemtype, j,clothname,  characterIndex));
    }

}