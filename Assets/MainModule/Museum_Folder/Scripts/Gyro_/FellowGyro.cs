using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellowGyro : MonoBehaviour
{
    private Quaternion baseRotation = new Quaternion(0, 0, 1, 0);

    private void Start()
    {
        GyroManager.instance.EnableGyro();
    }
    private void Update()
    {
        transform.localRotation = GyroManager.instance.GetGyroQotation() * baseRotation;
    }

}
