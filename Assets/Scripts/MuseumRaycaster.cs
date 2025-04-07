using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumRaycaster : MonoBehaviour
{
    public static MuseumRaycaster instance;

    public RaycastHit hit;
    public Camera playerCamera;

    public float rayDistance;

    private int layerMask;

    [HideInInspector]
    public static bool canOpenPicture = true;

    void Awake()
    {
        Debug.Log("=>>>>>>>>>>>>>>>>>>>" + gameObject.name);
        instance = this;

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

    private void Start()
    {
        rayDistance = 20;
        layerMask = 1 << LayerMask.NameToLayer("PictureInteractable");
        canOpenPicture = true;
        if (playerCamera == null)
        {
            playerCamera = ReferrencesForDynamicMuseum.instance.randerCamera;
        }
    }

    public void SelfieClose()
    {
        StartCoroutine(WaitForCooldown());

    }

    public void ChangePictureState(bool state)
    {
        canOpenPicture = state;
    }

//    public void Update()
//    {
//        if (EmoteAnimationPlay.Instance.isEmoteActive) return;

//        if (Input.GetMouseButtonUp(0) && canOpenPicture)
//        {
//#if !UNITY_EDITOR
//            if (Input.touchCount == 1)
//            {
//#endif

//            bool headBlock = false;
//            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

//            if (Physics.Raycast(ray, out hit))
//            {
//                if (hit.collider.gameObject.name == "Head Button" || CameraLook.IsPointerOverUIObject())
//                {
//                    headBlock = true;
//                }
//            }


//            if (!headBlock && GalleryImageManager.Instance.CheckIfPicturePanelIsOpen())
//            {
//                //RaycastHit hits = Physics.RaycastAll(ray, rayDistance);

//                if (Physics.Raycast(ray, out hit, rayDistance))
//                {
//                    if (hit.collider.gameObject.name == "PictureInteractable")
//                    {
//                        canOpenPicture = false;
//                        hit.collider.GetComponent<GalleryImageDetails>().OnPictureClicked();
//                    }
//                }

//                //for (int i = 0; i < hits.Length; i++)
//                //{
//                //    RaycastHit hit = hits[i];

//                //    Debug.Log(hit.collider.name);

//                //    if (hit.collider.gameObject.name == "PictureInteractable")
//                //    {
//                //        canOpenPicture = false;
//                //        hit.collider.GetComponent<GalleryImageDetails>().OnPictureClicked();
//                //    }
//                //}
//#if !UNITY_EDITOR

//                }
//#endif
//            }

//        }
//    }

    public IEnumerator WaitForCooldown()
    {
        yield return new WaitForSeconds(0.5f);

        canOpenPicture = true;
    }
}
