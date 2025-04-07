using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNSComingSoonManager : MonoBehaviour
{
    public static SNSComingSoonManager Instance;
    //public bool isSNSComingSoonActive = false;

    public GameObject snsComingSoonScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (XanaConstants.xanaConstants != null)
        {
            if (!XanaConstants.xanaConstants.r_isSNSComingSoonActive)
            {
                Debug.Log("Delete SNSComingSoonManager and screen");
                Destroy(this.gameObject);
            }
        }
    }
}