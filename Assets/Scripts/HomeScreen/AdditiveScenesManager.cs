using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveScenesManager : MonoBehaviour
{
    public float sceneDelay;

    public string sceneTest;
    public string sceneTest2;
    public string sceneTest3;
    public string sceneTest4;

    public GameObject SNSmodule;
    public GameObject SNSMessage;
    

    private void Awake()
    {
        sceneDelay = .5f;
         StartCoroutine(AddDelayStore(sceneDelay/3));

     }
    private void Start()
    {
        StartCoroutine(AddDelay(sceneDelay));
        StartCoroutine(AddDelaySNSFeedModule(sceneDelay));
        StartCoroutine(AddDelaySNSMessageModule(sceneDelay));
    }
    IEnumerator AddDelayStore(float delay)
    {
        
        yield return new WaitForSeconds(delay);
         AsyncOperation async=SceneManager.LoadSceneAsync(sceneTest2, LoadSceneMode.Additive);
        while(!async.isDone)
        {
            yield return null;
        }
         //yield return new WaitForSeconds(1);
        BodyCustomizer.Instance.BodyCustomCallFromStore();
    }


    IEnumerator AddDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneTest, LoadSceneMode.Additive);
    }

    IEnumerator AddDelaySNSFeedModule(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneTest3, LoadSceneMode.Additive);
    }
    IEnumerator AddDelaySNSMessageModule(float delay)
    {
       
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneTest4, LoadSceneMode.Additive);

        //if (LoadingHandler.Instance != null)
        //{
        //    if (Screen.orientation == ScreenOrientation.Landscape)
        //    {
        //        LoadingHandler.Instance.Loading_WhiteScreen.SetActive(true);
        //        yield return new WaitForSeconds(4f);
        //        LoadingHandler.Instance.Loading_WhiteScreen.SetActive(false);

        //    }
        //    LoadingHandler.Instance.HideLoading();
        //}
    }
   
}
