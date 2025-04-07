﻿using ExitGames.Client.Photon;
using Metaverse;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Networking;
using static EmoteFilterManager;
using UnityEngine.InputSystem.OnScreen;
using System.IO;

public class EmoteAnimationPlay : MonoBehaviour, IInRoomCallbacks, IOnEventCallback
{
    public bool alreadyRuning = true;
    private int counter = 0;
    public GameObject AnimHighlight;
    public static EmoteAnimationPlay Instance;
    public GameObject spawnCharacterObject;
    public GameObject spawnCharacterObjectRemote;
    public GameObject popupPenal;
    public bool IsEmote;
    // public Animator animator1=null;
    public Animator animator = null;
    public Animator animatorremote = null;
    public RuntimeAnimatorController controller;
    // public GameObject CameraLook;
    public AnimationDetails bean;
    public  GameObject AnimObject;

    public List<AnimationList> emoteAnim=new List<AnimationList>();
    private GameObject AnimObjectHigh;


    Dictionary<object, object> AnimationUrl = new Dictionary<object, object>();
    private bool firsttimecall;
    private PhotonView[] photonplayerObjects;
    private int remotePlayerId;
    public static string remoteUrlAnimation;
    public static string remoteUrlAnimationName;
    private bool callOnce;
    private bool iscashed = false;
    Dictionary<object, object> cashed_data = new Dictionary<object, object>();
    private List<EventData> data = new List<EventData>();
    public bool isEmoteActive = false;
    public bool MyAnimLoader = false;
    internal bool isFetchingAnim = false;
    internal bool isAnimRunning = false;
    internal static event Action<string> AnimationStarted;
    internal static event Action<string> AnimationStopped;
    public delegate void ClearAnimation();
    public ClearAnimation clearAnimation;
    bool isInit = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnEnable()
    {
        firsttimecall = false;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.StopEmoteAnimation -= StopAnimation;
    }

    void Init()
    {
        if (!isInit)
        {
            if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.StopEmoteAnimation += StopAnimation;
            isInit = true;
        }
        else if (GamePlayButtonEvents.inst == null) isInit = false;
    }

    void Update()
    {
        if (IsEmote)
        {
            IsEmote = false;
            animator.SetBool("IsEmote", true);
        }
    }

    public void Load(string url, GameObject prefabAnim)
    {
      //  Debug.Log("already run==="+alreadyRuning);
        Init();
       
            if (alreadyRuning)
        {

            if (AnimObject != null && prefabAnim != null && AnimObject.GetInstanceID() == prefabAnim.GetInstanceID() && isAnimRunning)
            {
                return;
            }
            alreadyRuning = false;
          
            //if (AnimObject != null && prefabAnim != null && AnimObject.GetInstanceID() == prefabAnim.GetInstanceID() && isAnimRunning)
            //{
            //    if (AnimObjectHigh != null) AnimObjectHigh.SetActive(false);
            //    StopAnimation();
            //    return;
            //}
            AnimationUrl.Clear();
            isFetchingAnim = true;
            StartCoroutine(GetAssetBundleFromServerUrl(url, prefabAnim));
        }
      
      
        //}
    }

    //Comment Step4 Load Assets Bundle Animator from URL
    public IEnumerator GetAssetBundleFromServerUrl(string BundleURL, GameObject prefabObject)
    {
      
        if (prefabObject != null) AnimObjectHigh = prefabObject.transform.GetChild(2).gameObject;

        if (MyAnimLoader == false)
        {
            sendDataAnimationUrl(BundleURL);
            try
            {
                AnimObject = prefabObject;
            }
            catch(Exception e)
            {
                AnimObject = prefabObject;
            }

            //if(AnimObject != null) AnimObject.transform.GetChild(3).gameObject.SetActive(true);
            if (AnimObject != null) AnimObject.transform.GetChild(3).gameObject.SetActive(true);
            if (AnimObjectHigh != null) AnimObjectHigh.SetActive(true);
            MyAnimLoader = true;
        }
        yield break;

    }
   // int counter = 0;


    IEnumerator GetAssetBundleFromServerRemotePlayerUrl(string BundleURL, int id)
    {
        if (counter > 4)
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            Caching.ClearCache();
            GC.Collect();
            counter = 0;
        }
      
        string bundlePath = Path.Combine(XanaConstants.xanaConstants.r_EmoteStoragePersistentPath, BundleURL + ".unity3d");

       
            //  StartCoroutine(GetAssetBundleFromServerUrl(url, bundlePath, _gameObject));


            Debug.Log("List===" + bean.data.animationList.Count);
#if UNITY_ANDROID

            BundleURL = bean.data.animationList.Find(x => x.name == BundleURL).android_file;

#elif UNITY_IOS
        BundleURL = bean.data.animationList.Find(x => x.name == BundleURL).ios_file;
#elif UNITY_EDITOR
        BundleURL = bean.data.animationList.Find(x => x.name == BundleURL).android_file;
#endif
            photonplayerObjects = null;
            photonplayerObjects = FindObjectsOfType<PhotonView>();

            for (int i = 0; i < photonplayerObjects.Length; i++)
            {
                if (photonplayerObjects[i] != null)
                {
                    if (photonplayerObjects[i].ViewID == id)
                    {
                    if (CheckForIsAssetBundleAvailable(bundlePath))
                    {
                        StartCoroutine(LoadAssetBundleFromStorage(bundlePath, photonplayerObjects[i].gameObject));
                    }
                    else
                    {
                        animatorremote = photonplayerObjects[i].gameObject.GetComponent<Animator>();

                        Debug.Log("photon objects====" + photonplayerObjects[i].ViewID + id);
                        using (WWW www = new WWW(BundleURL))
                        {
                            
                            while (!www.isDone)
                            {
                                yield return null;
                            }
                            //if (AnimObject != null) AnimObject.transform.GetChild(3).gameObject.SetActive(false);
                            //if (AnimObjectHigh != null) AnimObjectHigh.SetActive(false);
                            yield return www;
                            if (www.error != null)
                            {
                                // Loading.Instance.HideLoading();
                                throw new Exception("WWW download had an error:" + www.error);
                            }
                            else
                            {
                                AssetBundle assetBundle = www.assetBundle;

                                if (assetBundle != null)
                                {

                                    GameObject[] animation = assetBundle.LoadAllAssets<GameObject>();
                                    var remotego = animation[0];
                                    /* foreach (var remotego in animation)*/
                                    {

                                        if (remotego.name.Equals("Animation"))
                                        {

                                            if (!SelfieController.Instance.selfiePanel.activeInHierarchy)
                                            {


                                                Debug.Log("animation call=====");

                                                spawnCharacterObjectRemote = remotego.transform.gameObject;
                                                //if (photonplayerObjects[i].IsMine && AnimObject != null && isAnimRunning)
                                                //{
                                                //    StopAnimation();
                                                //    yield return new WaitUntil(() => !isAnimRunning);
                                                //}
                                                //    if (photonplayerObjects[i].IsMine)
                                                //    {
                                                //        MyAnimLoader = false;
                                                //        if (AnimObject != null)
                                                //        {
                                                //            AnimObject.transform.GetChild(3).gameObject.SetActive(false);
                                                //        }


                                                //        if (!AnimHighlight.activeInHierarchy)
                                                //        {
                                                //            AvatarManager.Instance.currentDummyPlayer.transform.localPosition = new Vector3(0f, -0.081f, 0);

                                                //            //  animatorremote = null;

                                                //            object[] viewMine = { GameObject.FindGameObjectWithTag("Player").transform.GetChild(19).GetComponent<PhotonView>().ViewID };
                                                //            RaiseEventOptions options = new RaiseEventOptions();
                                                //            options.CachingOption = EventCaching.DoNotCache;
                                                //            options.Receivers = ReceiverGroup.All;
                                                //            PhotonNetwork.RaiseEvent(1, viewMine as object, options,
                                                //                SendOptions.SendReliable);

                                                //            //PhotonNetwork.OpRemoveCompleteCacheOfPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
                                                //            iscashed = false;

                                                //            PlayerPrefs.SetString(remoteUrlAnimation, "");
                                                //            // controller.SetStateEffectiveMotion(state, null);
                                                //            // animator.SetBool("IsEmote", false);

                                                //            AvatarManager.Instance.spawnPoint.GetComponent<PlayerControllerNew>().enabled = true;
                                                //            AnimHighlight.SetActive(false);
                                                //            LoadFromFile.animClick = false;
                                                //            break;
                                                //        }
                                                //    }

                                                //}

                                                var overrideController = new AnimatorOverrideController();
                                                overrideController.runtimeAnimatorController = controller;

                                                List<KeyValuePair<AnimationClip, AnimationClip>> keyValuePairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                                                foreach (var clip in overrideController.animationClips)
                                                {

                                                    if (clip.name == "emaotedefault")
                                                        keyValuePairs.Add(new KeyValuePair<AnimationClip, AnimationClip>(clip, spawnCharacterObjectRemote.transform.GetComponent<Animation>().clip));
                                                    else
                                                        keyValuePairs.Add(new KeyValuePair<AnimationClip, AnimationClip>(clip, clip));
                                                }
                                                overrideController.ApplyOverrides(keyValuePairs);

                                                animatorremote.runtimeAnimatorController = overrideController;

                                                animatorremote.SetBool("IsEmote", true);

                                                CheckSelfieOn();
                                                // AvatarManager.Instance.currentDummyPlayer.transform.localPosition = new Vector3(0f, 0f, 0);


                                            }
                                            else
                                            {
                                              //  AvatarManager.Instance.currentDummyPlayer.GetComponent<Animator>().runtimeAnimatorController = controller;
                                            }
                                           
                                        }
                                    }
                                    SaveAssetBundle(www.bytes, bundlePath, photonplayerObjects[i].gameObject);

                                    assetBundle.Unload(false);
                                   

                                }
                            }
                        }

                    }
                    if (photonplayerObjects[i].IsMine)
                    {
                        MyAnimLoader = false;
                        isFetchingAnim = false;
                        isAnimRunning = true;
                      
                        if (AnimObject != null)
                        {
                            AnimObject.transform.GetChild(3).gameObject.SetActive(false);
                        }
                        AnimationStarted?.Invoke(remoteUrlAnimationName);
                      
                    }
                }
            }
            alreadyRuning = true;
            counter++;

        }
    }

    public void CheckSelfieOn()
    {
        if (SelfieController.Instance.selfiePanel.activeInHierarchy)
        {
            clearAnimation?.Invoke();

        }
    }

    public void StopAnimation()
    {
        GameObject player;
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        //if (photonplayerObjects[i].IsMine)
        //{
        //    MyAnimLoader = false;
        //    if (AnimObject != null)
        //    {
        //        AnimObject.transform.GetChild(3).gameObject.SetActive(false);
        //    }
        //}
        //if (!AnimHighlight.activeInHierarchy)
        {
            if (AvatarManager.Instance.currentDummyPlayer != null)
            {
                AvatarManager.Instance.currentDummyPlayer.transform.localPosition = new Vector3(0f, -0.081f, 0);
            }

            //  animatorremote = null;
             player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                print("child count "+ player.transform.childCount);
                if (player.transform.childCount > 19)
                {
                    print("Emote player is " + player.name);
                    print("player name is " + player.transform.GetChild(19).name);
                    object[] viewMine = { player.transform.GetChild(19).GetComponent<PhotonView>().ViewID };
                    RaiseEventOptions options = new RaiseEventOptions();
                    options.CachingOption = EventCaching.DoNotCache;
                    options.Receivers = ReceiverGroup.All;
                    PhotonNetwork.RaiseEvent(1, viewMine as object, options,
                        SendOptions.SendReliable);
                }
            }

            //PhotonNetwork.OpRemoveCompleteCacheOfPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
            iscashed = false;

            PlayerPrefs.SetString(remoteUrlAnimation, "");
            // controller.SetStateEffectiveMotion(state, null);
            // animator.SetBool("IsEmote", false);

            AvatarManager.Instance.spawnPoint.GetComponent<PlayerControllerNew>().enabled = true;
            if(AnimHighlight != null) AnimHighlight.SetActive(false);
            PlayerPrefsUtility.SetEncryptedString(ConstantsGod.SELECTED_ANIMATION_NAME, "");
            LoadFromFile.animClick = false;
        }
        isAnimRunning = false;
        MyAnimLoader = false;
        alreadyRuning = true;
        AnimationStopped?.Invoke(remoteUrlAnimationName);
        if (player.transform.GetChild(19).GetComponent<PhotonView>().IsMine)
        {
            if (AnimObject != null)
            {
                AnimObject.transform.GetChild(3).gameObject.SetActive(false);
            }
            StopAllCoroutines();
            isFetchingAnim = false;
        }

        Debug.Log("Stopppped");
    }

    public void sendDataAnimationUrl(string url)
    {
        AvatarManager.Instance.spawnPoint.GetComponent<PlayerControllerNew>().enabled = false;
        AnimHighlight.SetActive(true);
        // LoadFromFile.animClick = true;


        Debug.Log("mine Player===" + GameObject.FindGameObjectWithTag("Player").transform.GetChild(19).GetComponent<PhotonView>().ViewID);
        Dictionary<object, object> clothsDic = new Dictionary<object, object>();
        clothsDic.Add(GameObject.FindGameObjectWithTag("Player").transform.GetChild(19).GetComponent<PhotonView>().ViewID.ToString(), remoteUrlAnimationName);


        RaiseEventOptions options = new RaiseEventOptions();
        options.CachingOption = EventCaching.DoNotCache;
        options.Receivers = ReceiverGroup.All;
        PhotonNetwork.RaiseEvent(12, clothsDic, options,
            SendOptions.SendReliable);
        cashed_data = clothsDic;
        iscashed = true;
        Debug.Log("data send sucessfully==" + clothsDic.Count);
    }

    

    private void NetworkingClient_EventReceived(EventData obj)
    {
        //Debug.Log("call hua obj==" + obj.Code);

        if (obj.Code == 0)
        {
            if (firsttimecall == false)
            {

                firsttimecall = true;
                Debug.Log("get data===" + obj.CustomData);
                remotePlayerId = (int)obj.CustomData;
            }
        }

        else if (obj.Code == 1)
        {

            Debug.Log("call one time====" + obj.CustomData);
            object[] minePlayer = (object[])obj.CustomData;

            DisableAnim((int)minePlayer[0]);
            //   StartCoroutine(GetAssetBundleFromServerRemotePlayerUrl(obj.CustomData.ToString()));


        }
        else if (obj.Code == 12)
        {

            Debug.Log("call hua obj"+obj);
            StartCoroutine(waittostart(obj));

        }


    }


    public IEnumerator LoadAssetBundleFromStorage(string bundlePath,GameObject PlayerAvatar)
    {
        if (counter > 4)
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            Caching.ClearCache();
            GC.Collect();
            counter = 0;
        }
       

        Debug.LogError("LoadAssetBundleFromStorageRemote:" + bundlePath);
        //  currentButton.transform.GetChild(2).gameObject.SetActive(true);
        animatorremote = PlayerAvatar.gameObject.GetComponent<Animator>();

        Debug.Log("photon objects====" + PlayerAvatar);
        AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundle;

        AssetBundle assetBundle = bundle.assetBundle;
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        if (assetBundle != null)
        {
            AssetBundleRequest newRequest = assetBundle.LoadAllAssetsAsync<GameObject>();
            while (!newRequest.isDone)
            {
                yield return null;
            }
            if (newRequest.isDone)
            {
                Debug.LogError("Success load bundle from storage");
                var animation = newRequest.allAssets;
                foreach (var anim in animation)
                {
                    GameObject go = (GameObject)anim;
                    if (go.name.Equals("Animation"))
                    {
                        //PlayerAvatar.GetComponent<Animator>().runtimeAnimatorController = go.GetComponent<Animator>().runtimeAnimatorController;
                        //PlayerAvatar.GetComponent<Animator>().Play("Animation");

                        var overrideController = new AnimatorOverrideController();
                        overrideController.runtimeAnimatorController = controller;

                        List<KeyValuePair<AnimationClip, AnimationClip>> keyValuePairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                        foreach (var clip in overrideController.animationClips)
                        {

                            if (clip.name == "emaotedefault")
                                keyValuePairs.Add(new KeyValuePair<AnimationClip, AnimationClip>(clip, go.transform.GetComponent<Animation>().clip));
                            else
                                keyValuePairs.Add(new KeyValuePair<AnimationClip, AnimationClip>(clip, clip));
                        }
                        overrideController.ApplyOverrides(keyValuePairs);

                        animatorremote.runtimeAnimatorController = overrideController;

                        animatorremote.SetBool("IsEmote", true);
                    }
                }
            }
            if (assetBundle != null)
            {
                assetBundle.Unload(false);
            }
            counter++;
            alreadyRuning = true;
          
        }

      //  currentButton.transform.GetChild(2).gameObject.SetActive(false);

       
    }

    public void SaveAssetBundle(byte[] data, string path,GameObject id)
    {
        //Create the Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, data);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
            if (id.GetComponent<PhotonView>().IsMine)
            {
                MyAnimLoader = false;
                isFetchingAnim = false;
                isAnimRunning = true;
                
                if (AnimObject != null)
                {
                    AnimObject.transform.GetChild(3).gameObject.SetActive(false);
                }
                AnimationStarted?.Invoke(remoteUrlAnimationName);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }
    public bool CheckForIsAssetBundleAvailable(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator waittostart(EventData obj)
    {
        ///Debug.Log("data count==="+data.Count);
        data.Clear();
        if (data.Count > 0)
        {
            data.Add(obj);
            yield break;
        }
        data.Add(obj);


        while (data.Count > 0)
        {
            Debug.Log("get data 12===" + data[0].CustomData);

            Dictionary<object, object> clothsDic2 = new Dictionary<object, object>();

            clothsDic2 = (Dictionary<object, object>)data[0].CustomData;
            Debug.Log("get 12===" + clothsDic2.Count);

            foreach (KeyValuePair<object, object> keyValue in clothsDic2)
            {
                string s = keyValue.Key.ToString();
                remotePlayerId = int.Parse(s);
                //object[] ob = (object[])clothsDic2[s];
                //string vlaue = .ToString();
                Debug.Log("get send data====" + clothsDic2[s]);

                 yield return StartCoroutine(GetAssetBundleFromServerRemotePlayerUrl(clothsDic2[s].ToString(), int.Parse(s)));

               // yield return null;
            }
            data.RemoveAt(0);

        }
        yield return null;
    }

    public void DisableAnim(int viewId)
    {
        photonplayerObjects = null;
        photonplayerObjects = FindObjectsOfType<PhotonView>();

        for (int i = 0; i < photonplayerObjects.Length; i++)
        {
            if (photonplayerObjects[i] != null)
            {
                if (photonplayerObjects[i].ViewID == viewId)
                {
                    animatorremote = photonplayerObjects[i].gameObject.GetComponent<Animator>();
                    animatorremote.runtimeAnimatorController = controller;
                    animatorremote.SetBool("IsEmote", false);

                    Debug.Log("photon objects====" + photonplayerObjects[i].ViewID + remotePlayerId);
                }
            }
        }
    }

    public void animationClick()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        //if (!PremiumUsersDetails.Instance.CheckSpecificItem("gesture button"))
        //{
        //    //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
        //    print("Please Upgrade to Premium account");
        //    return;
        //}
        //else
        //{
        //    print("Horayyy you have Access");
        //}


        if (AnimHighlight.activeInHierarchy)
        {
            //  CameraLook.GetComponent<CameraLook>().enabled = true;
            //EmoteFilterManager.TouchDisable = false;
            popupPenal.SetActive(false);

           AvatarManager.Instance.currentDummyPlayer.transform.localPosition = new Vector3(0f, -0.081f, 0);

            //  animatorremote = null;

            object[] viewMine = { GameObject.FindGameObjectWithTag("Player").transform.GetChild(19).GetComponent<PhotonView>().ViewID };
            RaiseEventOptions options = new RaiseEventOptions();
            options.CachingOption = EventCaching.DoNotCache;
            options.Receivers = ReceiverGroup.All;
            PhotonNetwork.RaiseEvent(1, viewMine as object, options,
                SendOptions.SendReliable);

            //PhotonNetwork.OpRemoveCompleteCacheOfPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
            iscashed = false;
            PlayerPrefs.SetString(remoteUrlAnimation, "");
            // controller.SetStateEffectiveMotion(state, null);
            // animator.SetBool("IsEmote", false);

            AvatarManager.Instance.spawnPoint.GetComponent<PlayerControllerNew>().enabled = true;
            AnimHighlight.SetActive(false);
            LoadFromFile.animClick = false;


            try
            {
                LoadFromFile.instance.leftJoyStick.transform.GetChild(0).GetComponent<OnScreenStick>().movementRange = LoadFromFile.instance.joyStickMovementRange;

            }
            catch (Exception e)
            {

            }
        }
        else
        {
            // CameraLook.GetComponent<CameraLook>().enabled = false;
            // CameraLook.GetComponent<CameraLook>().enabled = true;
            //   EmoteFilterManager.TouchDisable = true;
            popupPenal.SetActive(true);
            isEmoteActive = true;
            try
            {
                LoadFromFile.instance.joyStickMovementRange = LoadFromFile.instance.leftJoyStick.transform.GetChild(0).GetComponent<OnScreenStick>().movementRange;

            }
            catch (Exception e)
            {

            }
        }



    }


    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player enter hua===" + newPlayer.ActorNumber);

        if (iscashed)
        {

            RaiseEventOptions options = new RaiseEventOptions();
            options.CachingOption = EventCaching.DoNotCache;
            options.TargetActors = new int[] { newPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent(12, cashed_data, options,
                SendOptions.SendReliable);
        }
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        //throw new NotImplementedException();
    }

    public IEnumerator getAllAnimations()
    {

        UnityWebRequest uwr = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.GetAllAnimatons +"/"+ APIBaseUrlChange.instance.apiversion);
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


                Debug.Log("Response===" + uwr.downloadHandler.text.ToString().Trim());
                bean = Gods.DeserializeJSON<AnimationDetails>(uwr.downloadHandler.text.ToString().Trim());
                if (!string.IsNullOrEmpty(bean.data.ToString()))
                {
                    emoteAnim.Clear();
                    emoteAnim.AddRange(bean.data.animationList);

                }
            }
            catch
            {

            }
        }
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //throw new NotImplementedException();
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        // throw new NotImplementedException();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (!string.IsNullOrEmpty(remoteUrlAnimation))
        {
            if (callOnce == false)
            {
                // callOnce = true;
                //Debug.Log("mine Player==="+ GameObject.FindGameObjectWithTag("PhotonLocalPlayer").GetComponent<PhotonView>().ViewID);
                //Dictionary<object, object> clothsDic = new Dictionary<object, object>();
                //clothsDic.Add(GameObject.FindGameObjectWithTag("PhotonLocalPlayer").GetComponent<PhotonView>().ViewID.ToString(), (object)remoteUrlAnimation);
                //PhotonNetwork.RaiseEvent(12, clothsDic, RaiseEventOptions.Default,
                //    SendOptions.SendUnreliable);
                //Debug.Log("data send sucessfully==" + clothsDic.Count);
                //  sendDataAnimationUrl(remoteUrlAnimation);
            }

        }

    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        // throw new NotImplementedException();
    }
}
