using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCodeGenerator : MonoBehaviour {


    public GameObject QRGenrate;
    Sprite mySprite;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //void OnGUI() {

    //    int ranNum = Random.Range(0, 10);

    //    if (ranNum == 1)
    //    {
    //        Texture2D myQR = generateQR("test 1");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 2)
    //    {
    //        Texture2D myQR = generateQR("test 2");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 3)
    //    {
    //        Texture2D myQR = generateQR("test 3");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 4)
    //    {
    //        Texture2D myQR = generateQR("test 4");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 5)
    //    {
    //        Texture2D myQR = generateQR("test 4");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 6)
    //    {
    //        Texture2D myQR = generateQR("test 6");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 7)
    //    {
    //        Texture2D myQR = generateQR("test 7");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 8)
    //    {
    //        Texture2D myQR = generateQR("test 8");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 9)
    //    {
    //        Texture2D myQR = generateQR("test 9");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }
    //    else if (ranNum == 10)
    //    {
    //        Texture2D myQR = generateQR("test 10");
    //        mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    }

    //    QRGenrate.sprite = mySprite;
    // //if (GUI.Button(new Rect(300, 300, 256, 256), myQR, GUIStyle.none))
    //    //{
    //    //    Debug.Log("create");
    //    //}
    //}
    public void newGenrate()
    {
        if (!QRGenrate.activeInHierarchy)
        {
            QRGenrate.SetActive(true);
        }
        int ranNum = Random.Range(0, 10);

        if (ranNum == 1)
        {
            Texture2D myQR = generateQR("test 1");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 2)
        {
            Texture2D myQR = generateQR("test 2");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 3)
        {
            Texture2D myQR = generateQR("test 3");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 4)
        {
            Texture2D myQR = generateQR("test 4");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 5)
        {
            Texture2D myQR = generateQR("test 4");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 6)
        {
            Texture2D myQR = generateQR("test 6");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 7)
        {
            Texture2D myQR = generateQR("test 7");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 8)
        {
            Texture2D myQR = generateQR("test 8");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 9)
        {
            Texture2D myQR = generateQR("test 9");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (ranNum == 10)
        {
            Texture2D myQR = generateQR("test 10");
            mySprite = Sprite.Create(myQR, new Rect(0.0f, 0.0f, myQR.width, myQR.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        QRGenrate.GetComponent<Image>().sprite = mySprite;
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
     
        encoded.Apply();
        return encoded;
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}
