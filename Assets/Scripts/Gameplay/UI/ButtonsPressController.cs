using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Metaverse;

[System.Serializable]
public class Btn
{
    public Sprite normal;
    public Sprite pressed;
    public Image image;
    public GameObject[] screens;


}

public class ButtonsPressController : MonoBehaviour
{
    public static ButtonsPressController Instance;

    [SerializeField] public List<Btn> btns;

    [Header("Btns to hide")]
    [SerializeField] public List<GameObject> objectsToHide;

    public GameObject[] HelpSprites;
    public bool Settings_pressed = false;



    private bool setHeader = true;
    bool IsHelpPanelOpen = false;




    private void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnHelpButton += HelpBtnPressed;
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnExitButton += OnExitClick;
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnSettingButton += OnSettingClick;
    }

    public void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnHelpButton -= HelpBtnPressed;
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnExitButton -= OnExitClick;
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.OnSettingButton -= OnSettingClick;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null)
            return;
#else
#if UNITY_ANDROID || UNITY_IOS
 if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && EventSystem.current.currentSelectedGameObject != null)
            return;
#endif
#endif

        if (Input.GetKeyDown(KeyCode.Mouse0) && IsHelpPanelOpen)
        {
            for (int i = 0; i < HelpSprites.Length; i++)
            {
                HelpSprites[i].SetActive(false);
            }
            //TurnCameras(true);
            IsHelpPanelOpen = false;
        }
    }

    public void SetPress(int index)
    {
        for (int i = 0; i < HelpSprites.Length; i++)
        {
            HelpSprites[i].SetActive(false);
        }

        
        bool showScreen = !btns[index].screens[0].activeInHierarchy;
        setHeader = showScreen;

        CloseAllScreens();

        //ResetAllSprites();

        Settings_pressed = true;
        if (showScreen)
        {
            HideObjects();
            TurnCameras(false);
            btns[index].image.sprite = btns[index].pressed;
            foreach (var screen in btns[index].screens)
            {
                if (!screen.activeInHierarchy)
                {

                    screen.SetActive(true);
                }
            }
        }

        else

        {
            ShowObjects();
            TurnCameras(true);
            Settings_pressed = false;
        }

    }

    void OnExitClick()
    {
        SetPress(5);
    }

    void OnSettingClick()
    {
        SetPress(0);
    }
   
    public void CloseAllScreens()
    {
        foreach (var btn in btns)
        {
            foreach (var screen in btn.screens)
            {
                screen.SetActive(false);
            }
        }





    }

    private void TurnCameras(bool active)
    {
        if (active)
        {
            CameraLook.instance.AllowControl();
        }
        else
        {
            CameraLook.instance.DisAllowControl();
        }
    }

    private void ResetAllSprites()
    {
        foreach (var item in btns)
        {
            item.image.sprite = item.normal;
        }
    }

    public void HelpBtnPressed()
    {
        if (!IsHelpPanelOpen)
        {
            CloseAllScreens();
            //ResetAllSprites();
            IsHelpPanelOpen = true;
            for (int i = 0; i < HelpSprites.Length; i++)
            {
                HelpSprites[i].SetActive(true);
            }
            TurnCameras(false);
        }
        else if (IsHelpPanelOpen)
        {
            IsHelpPanelOpen = false;
            for (int i = 0; i < HelpSprites.Length; i++)
            {
                HelpSprites[i].SetActive(false);
            }
            TurnCameras(true);
        }
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.UpdateHelpObjects(IsHelpPanelOpen);
    }


    private void HideObjects()
    {
        if (objectsToHide.Count > 0)
        {
            foreach (var item in objectsToHide)
            {
                item.SetActive(false);
            }
        }
    }

    private void ShowObjects()
    {
        if (objectsToHide.Count > 0)
        {
            foreach (var item in objectsToHide)
            {
                item.SetActive(true);
            }
        }
    }

    public void ResetAllToClose()
    {
        CloseAllScreens();
        ResetAllSprites();
        ShowObjects();


    }

    private bool IsIpad()
    {
        if (Camera.main.aspect >= 1.3 && Camera.main.aspect <= 1.35)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
