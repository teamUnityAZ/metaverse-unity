using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public static GyroManager instance;
    public static GyroManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = FindObjectOfType<GyroManager>();
                if(instance==null)
                {
                    instance = new GameObject("Spwaned GyroManager", typeof(GyroManager)).GetComponent<GyroManager>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    [Header ("logic")]
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;

    public void EnableGyro()
    {
        if (gyroActive)
            return;

        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroActive = gyro.enabled;
        }
       
    }

    private void Update()
    {
        if(gyroActive)
        {
            rotation = gyro.attitude;
            Debug.Log(rotation);
            Debug.Log("gyro is working");


        }
    }
    public Quaternion GetGyroQotation()
    {
        return rotation;
    }


}
