using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_Behaviour : MonoBehaviour
{
    public AudioClip _NPC_Information_Audio;
    public string _InfoString;
    public TMP_Text text;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Press test to interact ");
        if (other.CompareTag("LeftHand"))
        {
            
          //  text.gameObject.transform.localScale = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 5f, this.gameObject.transform.position.z);
            //  NPC_Information npc_Obj = other.GetComponent<NPC_Information>();
            //CanvusHandler.canvusHandlerInstance.is_Trigger = true;
            //CanvusHandler.canvusHandlerInstance.display_InfoString = npc_Obj._InfoString;
            //if (npc_Obj._NPC_Information_Audio != null)
            //{
            //    SoundManager.Instance.EffectsSource.clip = npc_Obj._NPC_Information_Audio;
            //    SoundManager.Instance.EffectsSource.Play();
            //}
            //Debug.Log("Press E to interact " + other.GetComponent<NPC_Information>()._InfoString);
            text.text = "LeftHand" ;
        }
        else
             if (other.CompareTag("RightHand"))
        {

          //  text.gameObject.transform.localScale = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 5f, this.gameObject.transform.position.z);
            text.text = "RightHand";
            Debug.Log("RightHand ");
        }
        }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftHand") )
        {
            text.text = " OnTriggerExit LeftHand";
            //CanvusHandler.canvusHandlerInstance.is_Trigger = false;
            Debug.Log(" Exit Trigger ");
        }
        if (other.CompareTag("RightHand"))
        {
            text.text = " OnTriggerExit RightHand";
            //CanvusHandler.canvusHandlerInstance.is_Trigger = false;
            Debug.Log(" Exit Trigger ");
        }
    }
}
