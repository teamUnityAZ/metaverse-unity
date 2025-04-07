using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CirclulerRoter : MonoBehaviour, IDragHandler
{
    private Vector3 lastPosition;
    public float rotationSpeed = 10f;

    // Drag the selected item.
    public void OnDrag(PointerEventData data)
    {
        Vector3 rotate = Vector3.zero;
        if (data.dragging)
        {
            Vector3 delta = lastPosition - (Vector3)data.position;
            delta = delta.normalized * rotationSpeed;
            rotate.z = delta.normalized.x + delta.normalized.y;
            lastPosition = data.position;
        }
        if (transform.rotation.z < -.5f || transform.rotation.z > .5f) return;
            transform.Rotate(rotate);
        ClampRotation();
        //Debug.LogFormat("rotate: {0}", transform.rotation.z);
    }

    void ClampRotation()
    {
        Quaternion rotation = transform.rotation;
        rotation.z = Mathf.Clamp(rotation.z, -.49f, .49f);
        transform.rotation = rotation; 

    }
}
