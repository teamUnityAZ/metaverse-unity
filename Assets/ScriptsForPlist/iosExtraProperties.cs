using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class iosExtraProperties : MonoBehaviour
{
    public bool m_ipad_EnableLandscape;
    public static bool ipadLandscapeEnable;

    private void OnValidate()
    {
        ipadLandscapeEnable = m_ipad_EnableLandscape;
     }
#if !UNITY_EDITOR
    private void Awake()
    {
        ipadLandscapeEnable = m_ipad_EnableLandscape;
        Debug.Log("is Ipag " + isIpad);
        if(isIpad && ipadLandscapeEnable)
        {
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToLandscapeLeft = true;
        }
    }
#endif
    static bool _isIpad = false;
    static bool _ipadTest = false;
    public static bool isIpad
    {
        get
            {
            if (!_ipadTest)
            {
#if UNITY_IOS
                string iosDevice = UnityEngine.iOS.Device.generation.ToString();
                Debug.Log("device: " + iosDevice);
                _isIpad = iosDevice.ToLower().Contains("ipad");
         
                _ipadTest = true;
#endif
            }
            return _isIpad;
            }  
     }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
