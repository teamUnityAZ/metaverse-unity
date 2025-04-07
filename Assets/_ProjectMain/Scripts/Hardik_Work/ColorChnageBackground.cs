using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChnageBackground : MonoBehaviour
{
    public Button green;
    public Button Red;
    public Button Pink;
    public Button White;
    public Button Yellow;
    public Button Black;
    public Button Orange;
    public static ColorChnageBackground instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
