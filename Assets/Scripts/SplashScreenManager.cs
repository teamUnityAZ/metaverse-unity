using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    
    public GameObject sceneManager;
   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spalsh());
    }

    IEnumerator spalsh()
    {
        yield return new WaitForSeconds(4.5f);
        sceneManager.GetComponent<SplashScreenManagerTimer>().enabled=true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
