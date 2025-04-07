using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using Cinemachine;
using UnityEditor;
using WebSocketSharp;
using UnityEngine.SceneManagement;


public class LoadFromFile : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public GameObject mainPlayer;
    public GameObject mainController;
    public GameObject LiveStreamPrefab;
    private GameObject YoutubeStreamPlayer;
    //private GameObject YoutubeStreamPlayer1;

    public CinemachineFreeLook PlayerCamera;
    public CinemachineFreeLook playerCameraCharacterRender;
    public Camera environmentCameraRender;
    public Camera firstPersonCamera;
    public Transform currentSpawnPoint;
    private GameObject currentEnvironment;

    private float fallOffset = 10f;
    public Camera NoPostProCam;
    public bool setLightOnce = false;
    public PopulationGenerator populationGenerator;
    public static LoadFromFile instance;
    public GameObject AnimHighlight;
    public GameObject popupPenal;
    public GameObject spawnCharacterObject;
    public GameObject spawnCharacterObjectRemote;
    public GameObject CameraLook, player;
    public static bool animClick=false;

    [HideInInspector]
    public GameObject leftJoyStick;

    [HideInInspector]
    public float joyStickMovementRange;

    public LayerMask layerMask;

    private void Awake()
    {
        instance = this;
        //    LoadFile();
        setLightOnce = false;
    }

    public void OnEnable()
    {
        base.OnEnable();
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OpenAllAnimsPanel += AnimClick;
    }

    public void OnDisable()
    {
        base.OnDisable();
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OpenAllAnimsPanel -= AnimClick;
    }

    private void Start()
    {
        EnableObjects();
        Input.multiTouchEnabled = true;
        for (int i = 0; i < SelfieController.Instance.OnFeatures.Length; i++)
        {
            if(SelfieController.Instance.OnFeatures[i]!=null)
            {
                if (SelfieController.Instance.OnFeatures[i].name == "LeftJoyStick")
                {
                    leftJoyStick = SelfieController.Instance.OnFeatures[i];
                    break;
                }
            }
        }

      
      
        // StartCoroutine(VoidCalculation());
    }

    public void EnableObjects()
    {
       // EmoteAnimationPlay.Instance.CameraLook = CameraLook;
        EmoteAnimationPlay.Instance.AnimHighlight = AnimHighlight;
        EmoteAnimationPlay.Instance.popupPenal = popupPenal;
       EmoteAnimationPlay.Instance.spawnCharacterObject = spawnCharacterObject;
        EmoteAnimationPlay.Instance.spawnCharacterObjectRemote = spawnCharacterObjectRemote;
       
            StartCoroutine(EmoteAnimationPlay.Instance.getAllAnimations());
       
    }
    public  IEnumerator VoidCalculation()
    {
        while (true)
        {
            if (CheckVoid())
            {
                
                    Debug.Log("Resetting Position");
                    ResetPlayerPosition();
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void AnimClick()
    {
        if (EmoteAnimationPlay.Instance.popupPenal.activeInHierarchy || animClick) return;

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


        animClick = true;

       AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
#if UNITY_EDITOR
        EmoteAnimationPlay.Instance.animationClick();
#endif
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            EmoteAnimationPlay.Instance.animationClick();
        }

       

#endif
    }
    public void LoadFile()
    {
        mainPlayer.SetActive(false);
            Debug.Log("Env Name : " + FeedEventPrefab.m_EnvName);
        if (!setLightOnce)
        {
            LoadLightSettings(FeedEventPrefab.m_EnvName);
            setLightOnce = true;
        }
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + "Environment");// Application.persistentDataPath + "/" + "Environment");
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        //
        //    return;
        //setLightOnce = true;
        string prefabName = "";
        if (XanaConstants.xanaConstants.IsMuseum)
        {
            prefabName = "MuseumScene";
        }
        else
        {
            prefabName = "Environment";
        }

        PlayerCamera.gameObject.SetActive(true);
        environmentCameraRender.gameObject.SetActive(true);
        environmentCameraRender.transform.GetChild(0).gameObject.SetActive(true);

        SelfieController.Instance.DisableSelfieFromStart();

        if (currentEnvironment == null)
        {
            print("Instantiate new Enviornment");
            var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(prefabName) ;
            //currentEnvironment = prefab;

            //#if UNITY_ANDROID
            //        prefab.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            //#endif
            //Instantiate(prefab, Vector3.zero, Quaternion.identity);
            currentEnvironment = Instantiate(prefab);
            if (populationGenerator && !XanaConstants.xanaConstants.IsMuseum)
            {
                populationGenerator.GeneratePopulation();
            }

            //riken
            Light[] directionalLightList = FindObjectsOfType<Light>();
            for (int i = 0; i < directionalLightList.Length; i++)
            {
                if (directionalLightList[i].type == LightType.Directional && directionalLightList[i].gameObject.tag!="CharacterLight")
                {
                    directionalLightList[i].cullingMask = layerMask;
                }
            }
            //.......
        }
        if (FeedEventPrefab.m_EnvName.Contains("DJ Event"))
        {
            YoutubeStreamPlayer = Instantiate(Resources.Load("DJEventData/YoutubeVideoPlayer") as GameObject);

#if UNITY_ANDROID || UNITY_EDITOR
            YoutubeStreamPlayer.transform.position = new Vector3(-0.39f, -0.05f, 16.93f);
            YoutubeStreamPlayer.transform.localScale = new Vector3(0.3844884f, 0.375074f, 0.375074f);
#else
YoutubeStreamPlayer.transform.position = new Vector3(-0.39f, -0.05f, 17.11f);
            YoutubeStreamPlayer.transform.localScale = new Vector3(0.3844884f, 0.375074f, 0.375074f);
#endif
            YoutubeStreamPlayer.SetActive(false);
            if (YoutubeStreamPlayer)
            {
                YoutubeStreamPlayer.SetActive(true);
            }
        }
        if (FeedEventPrefab.m_EnvName.Contains("XANA Festival Stage") && !FeedEventPrefab.m_EnvName.Contains("Dubai"))
        {
            YoutubeStreamPlayer = Instantiate(Resources.Load("XANAFestivalStageData/YoutubeVideoPlayer1") as GameObject);

#if UNITY_ANDROID || UNITY_EDITOR
            YoutubeStreamPlayer.transform.position = new Vector3(-0.39f, -0.05f, 17f);
            YoutubeStreamPlayer.transform.localScale = new Vector3(0.3844884f, 0.375074f, 0.375074f);
#else
YoutubeStreamPlayer.transform.position = new Vector3(-0.39f, -0.05f, 17.11f);
            YoutubeStreamPlayer.transform.localScale = new Vector3(0.3844884f, 0.375074f, 0.375074f);
#endif
            YoutubeStreamPlayer.SetActive(false);
            if (YoutubeStreamPlayer)
            {
                YoutubeStreamPlayer.SetActive(true);
            }
        }
        //Invoke("SpawnPlayer", 3f);

        if (currentEnvironment.transform.GetChild(currentEnvironment.transform.childCount - 1).name == "SpawnPoint")
        {
            currentSpawnPoint = currentEnvironment.transform.GetChild(currentEnvironment.transform.childCount - 1);
            Debug.Log("Last: " + currentEnvironment.transform.GetChild(currentEnvironment.transform.childCount - 1).name + "  " + currentEnvironment.transform.GetChild(currentEnvironment.transform.childCount - 1).position);
            StartCoroutine(SpawnPlayer());
        }
        else
        {
            foreach (Transform child in currentEnvironment.transform)
            {
                if (child.name == "SpawnPoint")
                {
                    currentSpawnPoint = child;
                    Debug.Log(child.name + "  " + child.transform.position);
                    StartCoroutine(SpawnPlayer());
                    break;
                }
            }
        }
        
        myLoadedAssetBundle.Unload(false);
    }

    private void LoadLightSettings(string mEnvName)
    {
        string path = "Environment Data/" + mEnvName + " Data/LightingData/LightingData";
        if (!mEnvName.IsNullOrEmpty())
        {
            EnvironmentProperties EnvProp = Resources.Load<EnvironmentProperties>(path);
            if (EnvProp)
            {
                
                    EnvProp.ApplyLightSettings();
            }
            else
            {
                Debug.LogWarning("No Environment Light Properties Found");
            }
        }
        else
        {
            Debug.LogWarning("No Environment Name Found");
        }
    }

    bool CheckVoid()
    {
        if (mainController.transform.position.y < (currentEnvironment.transform.position.y - fallOffset))
            return true;

        return false;
    }

   public IEnumerator SpawnPlayer()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PhotonLocalPlayer"))
        {
            if (go.GetComponent<PhotonView>().IsMine)
                goto End;
        }
        yield return new WaitForSeconds(2.5f);

        if (!(SceneManager.GetActiveScene().name.Contains("Museum")))
        {
            if (FeedEventPrefab.m_EnvName.Contains("AfterParty"))
            {
                if (XanaConstants.xanaConstants.setIdolVillaPosition)
                {
                    currentSpawnPoint.position = new Vector3(currentSpawnPoint.position.x, currentSpawnPoint.position.y + 2, currentSpawnPoint.position.z);
                    XanaConstants.xanaConstants.setIdolVillaPosition = false;
                }
                else
                {
                    for (int i = 0; i < IdolVillaRooms.instance.villaRooms.Length; i++)
                    {
                        if (IdolVillaRooms.instance.villaRooms[i].name == ChracterPosition.currSpwanPos)
                        {
                            currentSpawnPoint.position = IdolVillaRooms.instance.villaRooms[i].gameObject.GetComponent<ChracterPosition>().spawnPos;
                            break;
                        }
                        else
                        {
                            currentSpawnPoint.position = new Vector3(currentSpawnPoint.position.x, currentSpawnPoint.position.y + 2, currentSpawnPoint.position.z);
                        }
                    }
                }
            }
            else
            {
                currentSpawnPoint.position = new Vector3(currentSpawnPoint.position.x, currentSpawnPoint.position.y + 2, currentSpawnPoint.position.z);
            }

            RaycastHit hit;
            CheckAgain:
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(currentSpawnPoint.position, currentSpawnPoint.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                if (hit.collider.gameObject.tag == "PhotonLocalPlayer")
                {
                    currentSpawnPoint.gameObject.transform.position = new Vector3(currentSpawnPoint.position.x + Random.Range(-2, 2), currentSpawnPoint.position.y, currentSpawnPoint.position.z + Random.Range(-2, 2));
                    goto CheckAgain;
                } //else if()

                else if (hit.collider.gameObject.GetComponent<NPCRandomMovement>())
                {
                    currentSpawnPoint.gameObject.transform.position = new Vector3(currentSpawnPoint.position.x + Random.Range(-2, 2), currentSpawnPoint.position.y, currentSpawnPoint.position.z + Random.Range(-2, 2));
                    goto CheckAgain;
                }

                currentSpawnPoint.gameObject.transform.position = new Vector3(currentSpawnPoint.position.x, hit.point.y, currentSpawnPoint.position.z);
            }
            mainPlayer.transform.position = new Vector3(0,0,0);
            mainController.transform.position = currentSpawnPoint.position+ new Vector3(0,0.1f,0); // Adding with Vector3 to spawn avatar on floor.
            if (FeedEventPrefab.m_EnvName.Contains("XANALIA NFTART AWARD 2021"))
            {
                mainController.transform.rotation = Quaternion.Euler(0f, 230f, 0f);
            } 
            if (FeedEventPrefab.m_EnvName.Contains("DJ Event")) 
            {
                mainController.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            if (FeedEventPrefab.m_EnvName.Contains("XANA Festival Stage"))
            {
                mainController.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

        }
        //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
         player = PhotonNetwork.Instantiate("34", currentSpawnPoint.position, Quaternion.identity, 0);
      //  player.tag = "PhotonPlayer";
        Debug.Log("Spawning");


        //ReAdjustCamera();
        SetAxis();
        mainPlayer.SetActive(true);
        Metaverse.AvatarManager.Instance.InitCharacter();
        End:
        yield return new WaitForSeconds(.1f);
        //TurnOnPostCam();
        try
        {
            LoadingHandler.Instance.Loading_WhiteScreen.SetActive(false);
        }
        catch(System.Exception e)
        {
            Debug.LogError("Exception here..............");
        }
        while(!player)
        {
            yield return new WaitForSeconds(1f);
        }
        LoadingHandler.Instance.HideLoading();
    }
   
   // [ContextMenu("Change Mask")]
   // public void TurnOnPostCam()
   // {
   //     if (NoPostProCam)
   //     {
   //         // NoPostProCam.cullingMask = LayerMask.GetMask("NoPostProcessing");
   //         // Camera.main.Render();
   //         // NoPostProCam.Render();
   //         NoPostProCam.gameObject.SetActive(true);
   //
   //     }
   // }


    public void SetAxis()
    {
        CinemachineFreeLook cam = PlayerCamera.GetComponent<CinemachineFreeLook>();
        if (cam)
        {
            if (XanaConstants.xanaConstants.EnviornmentName=="XANALIA NFTART AWARD 2021")
            {
                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 0;
                cam.m_YAxis.Value = 0.5f;
            }
            else
            {

                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 180;
                cam.m_YAxis.Value = 0.5f;
            }

            if  (FeedEventPrefab.m_EnvName.Contains("DJ Event"))
            {
                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 0;
                cam.m_YAxis.Value = 0.5f;
            }
            else
            {

                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 0;
                cam.m_YAxis.Value = 0.5f;
            }

           
            if (FeedEventPrefab.m_EnvName.Contains("AfterParty"))
            {
                if (ChracterPosition.instance.NewPos.name.Contains("OutSide"))
                {
                    cam.Follow = mainController.transform;
                    cam.m_XAxis.Value = 0f;
                    cam.m_YAxis.Value = 0.5f;
                }
                else 
                    {
                        cam.Follow = mainController.transform;
                        cam.m_XAxis.Value = -137f;
                        cam.m_YAxis.Value = 0.5f;
                    }
                
            }
           

            CinemachineFreeLook cam2 = playerCameraCharacterRender.GetComponent<CinemachineFreeLook>();
            if (cam2)
            {

                if (XanaConstants.xanaConstants.EnviornmentName == "XANALIA NFTART AWARD 2021")
                {
                    cam2.Follow = mainController.transform;
                    cam2.m_XAxis.Value = 0;
                    cam2.m_YAxis.Value = 0.5f;
                }
                else
                {

                    cam2.Follow = mainController.transform;
                    cam2.m_XAxis.Value = 180;
                    cam2.m_YAxis.Value = 0.5f;
                }
                if (FeedEventPrefab.m_EnvName.Contains("DJ Event"))
                {
                    cam2.Follow = mainController.transform;
                    cam2.m_XAxis.Value = 0;
                    cam2.m_YAxis.Value = 0.5f;
                }
                else
                {

                    cam2.Follow = mainController.transform;
                    cam2.m_XAxis.Value = 0;
                    cam2.m_YAxis.Value = 0.5f;
                }
               
                if (FeedEventPrefab.m_EnvName.Contains("AfterParty"))
                {
                    if (ChracterPosition.instance.NewPos.name.Contains("OutSide"))
                    {
                        cam.Follow = mainController.transform;
                        cam.m_XAxis.Value = 0f;
                        cam.m_YAxis.Value = 0.5f;
                    }
                    else
                    {
                        cam.Follow = mainController.transform;
                        cam.m_XAxis.Value = -137f;
                        cam.m_YAxis.Value = 0.5f;
                    }
                
                }
               

            }
        }
    }

    void ResetPlayerPosition()
    {
        Debug.Log("Reset Player Position");

        mainController.GetComponent<PlayerControllerNew>().gravityVector.y = 0;
        mainController.transform.localPosition = currentSpawnPoint.localPosition;

        if (IdolVillaRooms.instance != null)
        {
            IdolVillaRooms.instance.ResetVilla();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    public override void OnLeftRoom()
    {

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("Instantiating Photon Complete");

        ResetPlayerPosition();
    }
}

