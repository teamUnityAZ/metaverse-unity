using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalUV256 : MonoBehaviour
{
    // Start is called before the first frame update
    public float scrollSpeed = 0.5F;
    public int materialNumber = 0;
    float offset = 0f;
    public Renderer rend;
    public float switchTime = 10.0f; 
    void Start()
    {
        rend = GetComponent<Renderer>();
        InvokeRepeating("updateJump", 1f, switchTime);
    }
    void Update()
    {
        ////float offset = Time.time * (scrollSpeed + jugard);
        //offset = ( scrollSpeed + offset);
        ////rend.materials[materialNumber].SetTextureOffset("_MainTex", new Vector2(offset + 256.0f, 0));
        //rend.materials[materialNumber].SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }

    public void updateJump()
    {  //float offset = Time.time * (scrollSpeed + jugard);
        offset = (scrollSpeed + offset);
        //rend.materials[materialNumber].SetTextureOffset("_MainTex", new Vector2(offset + 256.0f, 0));
        rend.materials[materialNumber].SetTextureOffset("_MainTex", new Vector2(0, offset));

    }
}
