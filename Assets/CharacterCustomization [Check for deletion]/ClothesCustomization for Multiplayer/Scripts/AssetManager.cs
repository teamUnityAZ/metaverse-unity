using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [SerializeField]
    private string Assetlink;
   
    [SerializeField]
    string json;
    [SerializeField]
    loadBundle bundle;
    [SerializeField]
    load_images images;
    public List<Assets> assetdata;
    // Start is called before the first frame update
    void Start()
    {
        assetdata = new List<Assets>();
        StartCoroutine(getAssetsdata());
        


    }

    private IEnumerator getAssetsdata()
    {
        WWW AssetRequest = new WWW(Assetlink);
        while (!AssetRequest.isDone)
        {
            Debug.Log("downloading....");
            // onDLProgress?.Invoke(modelName, bundleRequest.progress);
            yield return null;
        }
        if (AssetRequest.isDone)
        {
          json =   AssetRequest.text;
          
            assetdata = JsonHelper.FromJson<Assets>(json);
            images.addImage();
        }
    }
    

    public void generate_assetdata()
    {
      //  assetdata.Add(new Assets("link1", "data", "source"));

        Debug.LogError(assetdata.Count);
       json = JsonHelper.ToJson(assetdata,true);


    }
}
