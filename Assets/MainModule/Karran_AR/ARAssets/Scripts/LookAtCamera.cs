using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Transform target;
    bool isInAr;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.Equals("ARMode"))
        {
            isInAr = true;
            target = GameObject.FindGameObjectWithTag("AR-Camera").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInAr)
        {
            var lookPos = transform.position - target.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
        //ARManager.instance.text.text = target.position.ToString();
    }
}
