using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PutObjectsOnPath : MonoBehaviour, IDragHandler
{
    [SerializeField] Transform[] path;
    [SerializeField] RectTransform rect;
    List<GameObject> childObjs;
    private Vector3 lastPosition;
    public float movementSpeed = .0010f;
    [Range(0f,.25f)]
    [SerializeField] float percent;

    [SerializeField] float dist = 0.1445f;

    public void Awake()
    {
        childObjs = new List<GameObject>();
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            childObjs.Add(transform.GetChild(i).gameObject);
        }
    }

    public void FixedUpdate()
    {
        UpdateObjects(percent);
    }

    // Drag the selected item.
    public void OnDrag(PointerEventData data)
    {
        float threshold = 0f;
        if (data.dragging)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, data.position, data.pressEventCamera, out pos);
            //Vector3 delta = (Vector3)data.position - lastPosition;
            Vector3 delta = (Vector3)pos - lastPosition;
            //Debug.LogFormat("delta {0}  Normalized {1}  pos {2}", delta, delta.normalized, pos);
            delta = delta.normalized;
            int direction = (pos.x >= 0) ? -1 : 1;
            threshold = delta.normalized.x + delta.normalized.y * direction;
            lastPosition = pos;
            //lastPosition = data.position;
        }
        UpdateValue(threshold);
        //Debug.LogFormat("rotate: {0}", transform.rotation.z);
    }

  

    private void UpdateValue(float threshold)
    {
        //Debug.LogFormat("threshold {0}", threshold * Time.deltaTime * movementSpeed);
        percent -= threshold * Time.deltaTime * movementSpeed;
        percent = Mathf.Clamp(percent, 0f, .25f);
    }

    void UpdateObjects(float val)
    {
        for (int i = 0; i < childObjs.Count; i++)
        {
            float value = val + (i * dist);
            value = Mathf.Clamp(value, 0f, 1f);
            iTween.PutOnPath(childObjs[i], path, value);
        }
    }
}
