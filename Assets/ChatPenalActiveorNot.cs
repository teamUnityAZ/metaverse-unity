using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatPenalActiveorNot : MonoBehaviour
{
    public GameObject chatPanel;
    private bool activeState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
    public void openClose()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (chatPanel.activeInHierarchy)
            {
                activeState = true;
                chatPanel.SetActive(false);
            }

        }
        else
        {
            Debug.Log("close panel call" + activeState);
            if (activeState)
            {
                Debug.Log("close panel call");
                chatPanel.SetActive(true);
                activeState = false;

            }
        }
    }
}
