using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWorldRotationPerFrame : MonoBehaviour
{
    
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
