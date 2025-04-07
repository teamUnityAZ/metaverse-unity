using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedEventTextClipper : MonoBehaviour
{

    public int PreferredLength;
   

    private Text _myText;
    private TextMeshProUGUI _myTextMesh;
    // Start is called before the first frame update
    void Start()
    {
        _myText = GetComponent<Text>();
        //Length = _myText.text.Length;
        if (!_myText)
        {
            _myTextMesh = GetComponent<TextMeshProUGUI>();
            //Length = _myText.text.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentLanguage == "ja")
        {
            if (_myText)
            {
                if (_myText.text.Length > PreferredLength)
                {
                    _myText.text = _myText.text.Remove(PreferredLength - 1) + "...";
                }
            }

            if (_myTextMesh)
            {
                if (_myTextMesh.text.Length > PreferredLength)
                {
                    _myTextMesh.text = _myText.text.Remove(PreferredLength) + "...";
                }
            }
        }

    }
}
