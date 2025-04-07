using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class JoyStickManager : MonoBehaviour
{
    [SerializeField]
    private GameObject JoystickObject;
    GraphicRaycaster raycaster;

    void Awake()
    {
       /// JoystickObject.SetActive(false);
        // Get both of the components we need to do this
        this.raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
          
            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                //Debug.Log("Hit " + result.gameObject.name);
                if (result.gameObject.name == "LeftJoyStick")
                {
                    JoystickObject.SetActive(true);

                    JoystickObject.transform.position = pointerData.position;
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (JoystickObject != null)
                JoystickObject.SetActive(false);
        }
    }

}

