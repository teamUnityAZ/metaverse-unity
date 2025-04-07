using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferrencesForDynamicMuseum : MonoBehaviour
{
    public GameObject[] overlayPanels;
    public GameObject workingCanvas,PlayerParent,MainPlayerParent;
    public GameObject[] disableObjects;
    public static ReferrencesForDynamicMuseum instance;
    public Camera randerCamera;
    public List<GameObject> disableObjectsInMuseums;
    public TMPro.TextMeshProUGUI totalCounter; // Counter to show total connected peoples.
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        if (XanaConstants.xanaConstants.IsMuseum)
        {
            foreach(GameObject go in disableObjectsInMuseums)
            {
                go.SetActive(false);
            }
        }
    }
    public void forcetodisable() {
        foreach (GameObject go in disableObjects)
        {
            go.SetActive(false);
        }
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.UpdateCanvasForMuseum(false);
    }
    public void forcetoenable() {
        foreach (GameObject go in disableObjects)
        {
            go.SetActive(true);
        }
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.UpdateCanvasForMuseum(true);
    }
}
