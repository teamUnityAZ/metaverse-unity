using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {

	public float scrollSpeed = 0.5F;
	public int materialNumber = 0;
    public Renderer rend;
    void Start() {
        rend = GetComponent<Renderer>();
    }
    void Update() {
        float offset = Time.time * scrollSpeed;
		rend.materials[materialNumber].SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
