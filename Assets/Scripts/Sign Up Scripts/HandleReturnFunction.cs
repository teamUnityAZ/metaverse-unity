using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mopsicus.Plugins;
using UnityEngine.UI;
public class HandleReturnFunction : MonoBehaviour
{
    // Start is called before the first frame update

    public MobileInputField Field1;
    public ShiftCode ShiftCodeField;
 
    void Start()
    {
        
    }

    public void OnReturnFirst()
    {
         Field1.DeactiveField();
         ShiftCodeField.SelectTextFromReturn();    
    }      
    // Update is called once per frame
    void Update()
    {
        
    }

    public void onReturnName()
    {
        Field1.DeactiveField();
    }
}
