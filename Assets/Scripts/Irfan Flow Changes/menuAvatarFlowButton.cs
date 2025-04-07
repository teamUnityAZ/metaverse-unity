using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class menuAvatarFlowButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(OnClickMenuAvatarBtn);
    }
 
    void OnClickMenuAvatarBtn()
    {
        GameManager.Instance.AvatarMenuBtnPressed();

    }
}
