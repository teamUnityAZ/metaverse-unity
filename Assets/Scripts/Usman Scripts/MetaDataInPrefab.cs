using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Video;
using XanaNFT;

public class MetaDataInPrefab : MonoBehaviour
{
    public MetaData metaData;
    public TokenDetails tokenDetails;
    public CreatorDetails creatorDetails;
    public string nftLink;
    public string creatorLink;
    public Sprite thunbNailImage;
    public GameObject spriteObject;
    public Sprite thunbNailImage1;
    //public GameObject spriteObject1;
    //------------------------------------
    //For videos
    public VideoClip videoClip;
    public SpriteRenderer spriteRenderer;
    public VideoPlayer videoPlayer;
    private bool videoPlaying = false;
    public bool isVideo = false;

    // Start is called before the first frame update
    public RaycastHit hit;
    public Camera playerCamera;

    public float rayDistance = 4f;

    public GameObject frame, localPlayer;
    Material mat;



    // private int layerMask;

    //private bool canOpenPicture = true;


    /// <summary>
    /// variables for gif 
    /// </summary>
    private List<Texture2D> mFrames = new List<Texture2D>();
    private List<float> mFrameDelay = new List<float>();

    private int mCurFrame = 0;
    private float mTime = 0.0f;
    private Sprite gifTexture;
    private bool isGif;
    [HideInInspector()]
    public bool isVisible;
    private void Start()
    {
        mat = (Material)Resources.Load("FramMaterial");
        if (playerCamera == null)
        {
            playerCamera = ReferrencesForDynamicMuseum.instance.randerCamera;
        }
        GameObject[] go = GameObject.FindGameObjectsWithTag("PhotonLocalPlayer");
        foreach (GameObject go1 in go)
        {
            if (go1.GetComponent<PhotonView>().IsMine)
            {
                localPlayer = go1;
                break;
            }
        }


    }
    public void StartNow()
    {
        frame = this.transform.GetChild(0).gameObject;
        frame.SetActive(false);
        //print("StartNow");
        if (metaData.thumbnft != null)
            StartCoroutine(DownloadImageAndShow());
    }

    private void OnEnable()
    {
        PlayerControllerNew.CameraChangeDelegateEvent += OnChangeCamera;
    }

    private void OnDisable()
    {
        PlayerControllerNew.CameraChangeDelegateEvent -= OnChangeCamera;
    }


    private void OnChangeCamera(Camera cam)
    {
        playerCamera = cam;
    }

    //private void OnRenderObject()
    //{
    //    if (videoPlayer != null)
    //    {
    //        if (videoPlayer.isPrepared)
    //        {
    //            videoPlayer.Play();
    //        }
    //    }
    //}
    private IEnumerator DownloadImageAndShow()
    {
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();
        while (Application.internetReachability == NetworkReachability.NotReachable)
        {
            yield return new WaitForEndOfFrame();
            print("Internet Not Reachable");
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (creatorDetails.profile_image != null)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(creatorDetails.profile_image + "?tr=w-16,tr=h-16"))
            {
                request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }
                if (!request.isHttpError && !request.isNetworkError)
                {
                    if (request.error == null)
                    {
                        Texture2D loadedTexture = DownloadHandlerTexture.GetContent(request);
                        thunbNailImage1 = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), new Vector2(.2f, 0f));
                        // spriteObject1.AddComponent<SpriteRenderer>().sprite = thunbNailImage1;
                    }
                }
                else

                {
                    if (request.isNetworkError)
                    {
                        yield return StartCoroutine(DownloadImageAndShow());
                        print("Network Error");
                    }
                    else
                    {
                        if (request.error != null)
                        {
                            yield return StartCoroutine(DownloadImageAndShow());
                        }
                    }
                }
                request.Dispose();

            }
            yield return null;
        }
        else
        {

            thunbNailImage1 = ShowNFTDetails.instance.dummyprofileIcone;

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (metaData.thumbnft.EndsWith(".gif"))
        {
            //goto ENDIT;
            using (UnityWebRequest request = UnityWebRequest.Get(metaData.thumbnft + "?tr=w-400,tr=h-400"))
            {
                request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }
                //yield return new WaitForEndOfFrame();
                if (!request.isHttpError && !request.isNetworkError)
                {
                    if (request.error == null)
                    {
                        Texture2D loadedTexture = null; //= ((DownloadHandlerTexture)request.downloadHandler).texture;
                        byte[] imageData = request.downloadHandler.data;


                        using (var decoder = new MG.GIF.Decoder(imageData))
                        {
                            var img = decoder.NextImage();
                            loadedTexture = img.CreateTexture();
                            //while (img != null)
                            //{
                            //    yield return null;
                            //    mFrames.Add(img.CreateTexture());
                            //    mFrameDelay.Add(img.Delay / 1000.0f);
                            //    img = decoder.NextImage();
                            //}
                        }


                        thunbNailImage = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), new Vector2(.5f, 0f));
                        spriteObject = new GameObject();
                        spriteObject.name = "SpriteObject";
                        //Instantiate(new GameObject("SpriteObject"), this.transform.position, this.transform.rotation, this.transform);
                        spriteObject.transform.parent = this.transform;
                        spriteObject.transform.position = this.transform.position;
                        spriteObject.transform.rotation = this.transform.rotation;
                        spriteObject.transform.localPosition = new Vector3(0, 0, .01f);

                        spriteObject.AddComponent<SpriteRenderer>().sprite = thunbNailImage;//Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), new Vector2(.5f, 0f));

                        spriteObject.GetComponent<SpriteRenderer>().flipX = true;
                        if ((thunbNailImage.texture.width * 1f) > (thunbNailImage.texture.height * 1f))
                        {
                            spriteObject.transform.localScale = new Vector3(2.6f / (thunbNailImage.texture.width / 100f), 2.6f / (thunbNailImage.texture.width / 100f), 1);
                        }
                        else if ((thunbNailImage.texture.width * 1f) < (thunbNailImage.texture.height * 1f))
                        {
                            spriteObject.transform.localScale = new Vector3(2f / (thunbNailImage.texture.height / 100f), 2f / (thunbNailImage.texture.height / 100f), 1);
                        }
                        else
                        {
                            spriteObject.transform.localScale = new Vector3(2f / (thunbNailImage.texture.height / 100f), 2f / (thunbNailImage.texture.height / 100f), 1);
                        }
                        NFTFromServer.RemoveOne();
                        this.gameObject.AddComponent<UnityEngine.BoxCollider>().center = new Vector3(0, .75f, 0);
                        this.gameObject.GetComponent<UnityEngine.BoxCollider>().size = new Vector3(2f, 1.65f, 0.5f);
                        isVideo = false;
                        Vector3[] abc = GetSpriteCorners(spriteObject.GetComponent<SpriteRenderer>());

                        if ((this.transform.localEulerAngles.y > 45 && this.transform.localEulerAngles.y < 105) || (this.transform.localEulerAngles.y > 235 && this.transform.localEulerAngles.y < 315))
                        {
                            //print(abc[0] + " " + abc[1] + " " + abc[2] + " " + abc[3]);
                            abc[0].x = abc[1].x = abc[2].x = abc[3].x;
                        }
                        else
                        {
                            abc[1].x = abc[0].x;
                            abc[1].z = abc[0].z;

                            abc[2].y = abc[1].y;
                            abc[2].z = abc[1].z;

                            abc[3].x = abc[2].x;
                            abc[3].z = abc[2].z;
                        }
                        for (int i = 0; i < abc.Length; i++)
                        {
                            GameObject temp1 = new GameObject();
                            temp1.name = "upSide";
                            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            temp.GetComponent<MeshRenderer>().material = mat; //Resources.Load("FrameMaterial");
                            temp.transform.parent = temp1.transform;
                            temp.transform.position = new Vector3(temp1.transform.position.x, temp1.transform.position.y, temp1.transform.position.z + .5f);
                            temp1.transform.position = abc[i];
                            temp1.transform.parent = this.transform;
                            temp1.transform.localScale = new Vector3(.1f, .1f, 0f);// this.transform.localScale;
                            float distance;
                            if (i == abc.Length - 1)
                            {
                                temp1.transform.LookAt(abc[0]);
                                distance = Vector3.Distance(abc[i], abc[0]);
                            }
                            else
                            {
                                temp1.transform.LookAt(abc[i + 1]);
                                distance = Vector3.Distance(abc[i], abc[i + 1]);
                            }
                            temp1.transform.localScale = new Vector3(.1f, .1f, (distance * 2) + .1f);
                            temp1.transform.localPosition = new Vector3(temp1.transform.localPosition.x, temp1.transform.localPosition.y, 0);
                        }
                        using (var decoder = new MG.GIF.Decoder(imageData))
                        {
                            var img = decoder.NextImage();
                            while (img != null)
                            {
                                yield return null;
                                mFrames.Add(img.CreateTexture());
                                mFrameDelay.Add(img.Delay / 1000.0f);
                                img = decoder.NextImage();
                            }
                        }
                    }
                }
                else
                {
                    if (request.isNetworkError)
                    {
                        yield return StartCoroutine(DownloadImageAndShow());
                        print("Network Error");
                    }
                    else
                    {
                        if (request.error != null)
                        {
                            yield return StartCoroutine(DownloadImageAndShow());
                        }
                    }
                }
                request.Dispose();
            }
            isGif = true;
        ENDIT:
            yield return null;
        }
        else
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(metaData.thumbnft + "?tr=h-512"))
            {
                request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }
                if (!request.isHttpError && !request.isNetworkError)
                {
                    if (request.error == null)
                    {
                        Texture2D loadedTexture = DownloadHandlerTexture.GetContent(request);
                        thunbNailImage = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), new Vector2(.5f, 0f));
                        spriteObject = new GameObject();
                        spriteObject.name = "SpriteObject";
                        //Instantiate(new GameObject("SpriteObject"), this.transform.position, this.transform.rotation, this.transform);
                        spriteObject.transform.parent = this.transform;
                        spriteObject.transform.position = this.transform.position;
                        spriteObject.transform.rotation = this.transform.rotation;
                        spriteObject.transform.localPosition = new Vector3(0, 0, .01f);

                        spriteObject.AddComponent<SpriteRenderer>().sprite = thunbNailImage;

                        spriteObject.GetComponent<SpriteRenderer>().flipX = true;
                        if ((thunbNailImage.texture.width * 1f) > (thunbNailImage.texture.height * 1f))
                        {
                            spriteObject.transform.localScale = new Vector3(2.6f / (thunbNailImage.texture.width / 100f), 2.6f / (thunbNailImage.texture.width / 100f), 1);
                        }
                        else if ((thunbNailImage.texture.width * 1f) < (thunbNailImage.texture.height * 1f))
                        {
                            spriteObject.transform.localScale = new Vector3(2f / (thunbNailImage.texture.height / 100f), 2f / (thunbNailImage.texture.height / 100f), 1);
                        }
                        else
                        {
                            spriteObject.transform.localScale = new Vector3(2f / (thunbNailImage.texture.height / 100f), 2f / (thunbNailImage.texture.height / 100f), 1);
                        }
                        NFTFromServer.RemoveOne();
                        this.gameObject.AddComponent<UnityEngine.BoxCollider>().center = new Vector3(0, .75f, 0);
                        this.gameObject.GetComponent<UnityEngine.BoxCollider>().size = new Vector3(2f, 1.65f, 0.5f);
                        if (metaData.image.EndsWith(".mp4") || metaData.image.EndsWith(".MOV"))
                        {
                            isVideo = true;
                        }
                        else
                        {
                            isVideo = false;
                        }
                        Vector3[] abc = GetSpriteCorners(spriteObject.GetComponent<SpriteRenderer>());

                        if ((this.transform.localEulerAngles.y > 45 && this.transform.localEulerAngles.y < 105) || (this.transform.localEulerAngles.y > 235 && this.transform.localEulerAngles.y < 315))
                        {
                            //print(abc[0] + " " + abc[1] + " " + abc[2] + " " + abc[3]);
                            abc[0].x = abc[1].x = abc[2].x = abc[3].x;
                        }
                        else
                        {
                            abc[1].x = abc[0].x;
                            abc[1].z = abc[0].z;

                            abc[2].y = abc[1].y;
                            abc[2].z = abc[1].z;

                            abc[3].x = abc[2].x;
                            abc[3].z = abc[2].z;
                        }
                        for (int i = 0; i < abc.Length; i++)
                        {
                            GameObject temp1 = new GameObject();
                            temp1.name = "upSide";
                            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            temp.GetComponent<MeshRenderer>().material = mat; //Resources.Load("FrameMaterial");
                            temp.transform.parent = temp1.transform;
                            temp.transform.position = new Vector3(temp1.transform.position.x, temp1.transform.position.y, temp1.transform.position.z + .5f);
                            temp1.transform.position = abc[i];
                            temp1.transform.parent = this.transform;
                            temp1.transform.localScale = new Vector3(.1f, .1f, 0f);// this.transform.localScale;
                            float distance;
                            if (i == abc.Length - 1)
                            {
                                temp1.transform.LookAt(abc[0]);
                                distance = Vector3.Distance(abc[i], abc[0]);
                            }
                            else
                            {
                                temp1.transform.LookAt(abc[i + 1]);
                                distance = Vector3.Distance(abc[i], abc[i + 1]);
                            }
                            temp1.transform.localScale = new Vector3(.1f, .1f, (distance * 2) + .1f);
                            temp1.transform.localPosition = new Vector3(temp1.transform.localPosition.x, temp1.transform.localPosition.y, 0);
                        }
                    }
                }
                else
                {
                    if (request.isNetworkError)
                    {
                        yield return StartCoroutine(DownloadImageAndShow());
                        print("Network Error");
                    }
                    else
                    {
                        if (request.error != null)
                        {
                            yield return StartCoroutine(DownloadImageAndShow());
                        }
                    }
                }
                request.Dispose();
            }
        }

        yield return null;

        if (NFTFromServer.OnImageDownload != null)
        {
            NFTFromServer.OnImageDownload(NFTFromServer.currDownloadedNo++);
        }
    }


    private void SetFrameNow(VideoPlayer source, long frameIdx)
    {
        print(source.width + " " + source.height);
    }

    public static Vector3[] GetSpriteCorners(SpriteRenderer renderer)
    {
        Vector3 topRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max);
        Vector3 topLeft = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, renderer.sprite.bounds.max.z));
        Vector3 botLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min);
        Vector3 botRight = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, renderer.sprite.bounds.max.z));
        return new Vector3[] { topRight, topLeft, botLeft, botRight };
    }


    private void OnMouseUp()
    {
#if !UNITY_EDITOR
            if (Input.touchCount == 1)
            {
#endif
        if (EmoteAnimationPlay.Instance.isEmoteActive || CameraLook.IsPointerOverUIObject()) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.gameObject.name == this.gameObject.name)
            {
                if (isVideo && !SelfieController.Instance.t_nftMuseums && !ShowNFTDetails.instance.displayPanel.activeInHierarchy && !ButtonsPressController.Instance.Settings_pressed)
                {
                    print("showing video");
                    StartCoroutine(ShowVideoNow());

                }
                else if (!isVideo && !SelfieController.Instance.t_nftMuseums && !ShowNFTDetails.instance.displayPanel.activeInHierarchy && !ButtonsPressController.Instance.Settings_pressed)
                {
                    print(SelfieController.Instance.m_IsSelfieFeatureActive);
                    ShowNFTDetails.instance.ShowImage(this);
                    isVisible = true;
                }
            }
        }

#if !UNITY_EDITOR

                }
#endif
    }


    public IEnumerator ShowVideoNow()
    {
        ReferrencesForDynamicMuseum.instance.forcetodisable();
        LoadingHandler.Instance.UpdateLoadingStatusText("Opening Video");
        LoadingHandler.Instance.OnLoadnft();
        yield return null;
        spriteObject = new GameObject();
        spriteObject.name = "SpriteObject";
        videoPlayer = spriteObject.AddComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        string videoURL = metaData.image;
        if (metaData.image == "https://ik.imagekit.io/xanalia/award/1631447513792.mp4")
        {
            videoURL = "https://cdn.xana.net/xanaprod/Defaults/1631447513792.mp4";
        }
        else if (metaData.image == "https://ik.imagekit.io/xanalia/award/1640776522185.mp4")
        {
            videoURL = "https://cdn.xana.net/xanaprod/Defaults/1640776522185.mp4";
        }
        else if (metaData.image == "https://ik.imagekit.io/xanalia/award/1634037496490.mp4")
        {
            videoURL = "https://cdn.xana.net/xanaprod/Defaults/1634037496490.mp4";
        }
        else if (metaData.image == "https://ik.imagekit.io/xanalia/award/1640866737197.mp4")
        {
            videoURL = "https://cdn.xana.net/xanaprod/Defaults/1640866737197.mp4";
        }
        else if (metaData.image == "https://ik.imagekit.io/xanalia/award/1639885977430.MOV")
        {
            videoURL = "https://cdn.xana.net/xanaprod/Defaults/1639885977430.mp4";
        }
        else if (metaData.image == "https://ik.imagekit.io/xanalia/award/1640323809898.mp4")
        {
            videoURL = "https://cdn.xana.net/xanaprod/Defaults/1640323809898.mp4";
        }
        //else if (metaData.image == "https://ik.imagekit.io/xanalia/award/1640961826804.mp4")
        //{
        //    videoURL = "https://cdn.xana.net/xanaprod/Defaults/1647964549464_1640961826804.mp4";
        //}
        else if (metaData.image == "https://ik.imagekit.io/xanalia/award/1636461565419.mp4")
        {
            videoURL = "https://ik.imagekit.io/xanalia/award/1636461565419.mp4";
        }
        videoPlayer.url = videoURL;
        videoPlayer.SetDirectAudioMute(0, true);// = VideoAudioOutputMode.None;
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        print(videoPlayer.width + " " + videoPlayer.height);
        spriteObject.AddComponent<SpriteRenderer>();
        thunbNailImage = Sprite.Create(new Texture2D((int)videoPlayer.height, (int)videoPlayer.width), new Rect(0f, 0f, (int)videoPlayer.height, (int)videoPlayer.width), new Vector2(0.5f, 0f));
        spriteObject.transform.position = this.transform.position;
        spriteObject.transform.rotation = this.transform.rotation;
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.isLooping = true;
        videoPlayer.targetMaterialRenderer = spriteObject.GetComponent<SpriteRenderer>();
        spriteObject.GetComponent<SpriteRenderer>().sprite = thunbNailImage;
        spriteObject.GetComponent<SpriteRenderer>().flipX = true;

        spriteObject.transform.parent = this.transform;
        spriteObject.transform.localPosition = new Vector3(0, 0, .07f);
        if ((videoPlayer.width * 1f) / (videoPlayer.height * 1f) > 1.778f)
        {
            spriteObject.transform.localScale = new Vector3(1.47f / (videoPlayer.width / 100f), 1.47f / (videoPlayer.height / 100f), 1);
        }
        else
        {
            spriteObject.transform.localScale = new Vector3(1.47f / (videoPlayer.height / 100f), 1.47f / (videoPlayer.width / 100f), 1);
        }
        print(videoPlayer.width + " " + videoPlayer.height);
        spriteObject.transform.Rotate(new Vector3(0, 0, 0));
        NFTFromServer.RemoveOne();
        //this.gameObject.AddComponent<UnityEngine.BoxCollider>().center = new Vector3(0, .75f, 0);
        //this.gameObject.GetComponent<UnityEngine.BoxCollider>().size = new Vector3(2f, 1.65f, 0.5f);
        isVideo = true;
        //Vector3[] abc = GetSpriteCorners(spriteObject.GetComponent<SpriteRenderer>());

        //if ((this.transform.localEulerAngles.y > 45 && this.transform.localEulerAngles.y < 105) || (this.transform.localEulerAngles.y > 235 && this.transform.localEulerAngles.y < 315))
        //{
        //    //print(abc[0] + " " + abc[1] + " " + abc[2] + " " + abc[3]);
        //    abc[0].x = abc[1].x = abc[2].x = abc[3].x;
        //}
        //else
        //{
        //    abc[1].x = abc[0].x;
        //    abc[1].z = abc[0].z;

        //    abc[2].y = abc[1].y;
        //    abc[2].z = abc[1].z;

        //    abc[3].x = abc[2].x;
        //    abc[3].z = abc[2].z;
        //}
        //videoPlayer.Pause();
        //for (int i = 0; i < abc.Length; i++)
        //{
        //    GameObject temp1 = new GameObject();
        //    temp1.name = "upSide";
        //    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    temp.GetComponent<MeshRenderer>().material = mat; //Resources.Load("FrameMaterial");
        //    temp.transform.parent = temp1.transform;
        //    temp.transform.position = new Vector3(temp1.transform.position.x, temp1.transform.position.y, temp1.transform.position.z + .5f);
        //    temp1.transform.position = abc[i];
        //    temp1.transform.parent = this.transform;
        //    temp1.transform.localScale = new Vector3(.1f, .1f, 0f);// this.transform.localScale;
        //    float distance;
        //    if (i == abc.Length - 1)
        //    {
        //        temp1.transform.LookAt(abc[0]);
        //        distance = Vector3.Distance(abc[i], abc[0]);
        //    }
        //    else
        //    {
        //        temp1.transform.LookAt(abc[i + 1]);
        //        distance = Vector3.Distance(abc[i], abc[i + 1]);
        //    }
        //    temp1.transform.localScale = new Vector3(.1f, .1f, (distance * 2) + .1f);
        //    temp1.transform.localPosition = new Vector3(temp1.transform.localPosition.x, temp1.transform.localPosition.y, 0);
        //}
        ShowNFTDetails.instance.ShowVideo(this);
        LoadingHandler.Instance.HideLoading();
    }

    Sprite[] currFrame;

    public void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Debug.LogError("open image again");
        //#if !UNITY_EDITOR
        //            if (Input.touchCount == 1)
        //            {
        //#endif
        //            if (EmoteAnimationPlay.Instance.isEmoteActive || CameraLook.IsPointerOverUIObject()) return;

        //            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        //            if (Physics.Raycast(ray, out hit, rayDistance))
        //            {
        //                if (hit.collider.gameObject.name == this.gameObject.name)
        //                {
        //                    if (isVideo && !SelfieController.Instance.t_nftMuseums && !ShowNFTDetails.instance.displayPanel.activeInHierarchy && !ButtonsPressController.Instance.Settings_pressed)
        //                    {
        //                        print("showing video");
        //                        ShowNFTDetails.instance.ShowVideo(this);
        //                    }
        //                    else if (!isVideo && !SelfieController.Instance.t_nftMuseums && !ShowNFTDetails.instance.displayPanel.activeInHierarchy && !ButtonsPressController.Instance.Settings_pressed)
        //                    {
        //                        print(SelfieController.Instance.m_IsSelfieFeatureActive);
        //                        ShowNFTDetails.instance.ShowImage(this);
        //                        isVisible = true;
        //                    }
        //                }
        //            }

        //#if !UNITY_EDITOR

        //                }
        //#endif
        // }

        if (isGif)
        {
            //if (mFrames == null)
            //{
            //    return;
            //}
            if (currFrame == null)
            {
                if (mFrames == null)
                {
                    return;
                }
                currFrame = new Sprite[mFrames.Count / 2];
                print(mFrames.Count);
                for (int i = 0; i < mFrames.Count / 2; i++)
                {
                    currFrame[i] = Sprite.Create(mFrames[i * 2], new Rect(0f, 0f, mFrames[i * 2].width, mFrames[i * 2].height), new Vector2(.5f, 0f));
                }
                mFrames.Clear();
            }
            mTime += Time.deltaTime;

            if (mTime >= mFrameDelay[mCurFrame])
            {
                mCurFrame = (mCurFrame + 1) % (currFrame.Length);
                mTime = 0.0f;

                spriteObject.GetComponent<SpriteRenderer>().sprite = currFrame[mCurFrame];
                if (isVisible)
                    ShowNFTDetails.instance.UpdateGif(currFrame[mCurFrame]);
            }
        }

        if (videoPlayer != null && Vector3.Distance(this.transform.position, localPlayer.transform.position) < 2f && !videoPlaying)
        {
            if (videoPlayer.isPrepared)
            {
                if (!videoPlaying)
                {
                    videoPlaying = true;
                    videoPlayer.Play();
                }
            }
        }
        else if (videoPlayer != null && Vector3.Distance(this.transform.position, localPlayer.transform.position) > 2f && videoPlaying)
        {
            videoPlaying = false;
            videoPlayer.Pause();
        }
    }
}

