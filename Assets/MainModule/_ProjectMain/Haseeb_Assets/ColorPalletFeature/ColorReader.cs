using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorReader : MonoBehaviour
{

    public GameObject m_Object;


    void Start()
    {
        
    }

    void Update()
    {
        ColorPicker();
    }

    void ColorPicker()
    {



        RaycastHit l_Hit;
        if (!Physics.Raycast(transform.position,-transform.up, out l_Hit))
            return;

        Renderer rend = l_Hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = l_Hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = l_Hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        Color col=tex.GetPixel((int)pixelUV.x,(int)pixelUV.y);

        m_Object.GetComponent<SkinnedMeshRenderer>().sharedMaterial.color=col;
    }
}
