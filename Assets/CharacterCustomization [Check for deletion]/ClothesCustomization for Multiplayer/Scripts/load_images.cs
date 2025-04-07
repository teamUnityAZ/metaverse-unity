using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class load_images : MonoBehaviour
{

    [Serializable]
    public class categorydata
    {
        public string categoryname;
        [HideInInspector]
        public int counter = 0;
        public List<GameObject> buttons;
        public categorydata(string category)
        {
            categoryname = category;
            buttons = new List<GameObject>();
        }
    }

    [SerializeField]
    AssetManager manager;
    [SerializeField]
    loadBundle bundle;
    [SerializeField]
    List<categorydata> categories;
    [SerializeField]
    GameObject button;
    [SerializeField]
    GameObject[] obje;
    private void Start()
    {
        categories = new List<categorydata>();

        foreach (GameObject cat in obje)
        {

            categories.Add(new categorydata(cat.name));
        }
    }

    [Obsolete]
    public void addImage()
    {
        foreach (Assets asset in manager.assetdata)
        {

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].categoryname == asset.category)
                {
                    GameObject btn = Instantiate(button);
                    btn.name = asset.assetname;

                    btn.SetActive(true);
                    btn.transform.parent = obje[i].transform.Find("ScrollRect/Viewport/DynamicGridLayoutGroup").transform;
                    btn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    btn.GetComponent<Button>().onClick.AddListener(() => {
#if UNITY_ANDROID
                        StartCoroutine(bundle.GetAssetBundle(asset.assetsPathandroid, asset.category));
#elif UNITY_IOS
                        StartCoroutine(bundle.GetAssetBundle(asset.assetsPathios, asset.category));
#else

#endif
                        StartCoroutine(bundle.timeout());


                    });
                    StartCoroutine(addsprite(btn.GetComponent<Image>(), asset.assetpreviewlink));
                    categories[i].buttons.Add(btn);
                    categories[i].counter++;
                }



                /*  
                  for(int i = 0; i < categories.Count; i++)
                      {
                          if(categories[i].categoryname == asset.category)
                          {
                              categories[i].buttons[categories[i].counter].GetComponent<Button>().onClick.AddListener(() => {

                                  StartCoroutine(bundle.GetAssetBundle(asset.assetsPath, asset.category));

                                  StartCoroutine(bundle.timeout());

                              });
                              StartCoroutine(addsprite(categories[i].buttons[categories[i].counter].GetComponent<Image>(), asset.assetpreviewlink));
                              categories[i].counter++;
                          }

                         */
            }

        }

        /* switch (asset.assetname)
         {
             case "pant":
                 pant[i].GetComponent<Button>().onClick.AddListener(() => {

                     StartCoroutine(bundle.GetAssetBundle(asset.assetsPath, asset.assetname));



                 });
                 StartCoroutine(addsprite(pant[j].GetComponent<Image>(), asset.assetpreviewlink));
                 i++;

                 break;
             case "shirt":
                 shirt[j].GetComponent<Button>().onClick.AddListener(() => {

                     StartCoroutine(bundle.GetAssetBundle(asset.assetsPath, asset.assetname));



                 });
                 StartCoroutine(addsprite(shirt[j].GetComponent<Image>(), asset.assetpreviewlink));
                 j++;

                 break;
             case "shoes":
                 shoes[k].GetComponent<Button>().onClick.AddListener(() => {

                     StartCoroutine(bundle.GetAssetBundle(asset.assetsPath, asset.assetname));



                 });
                 StartCoroutine(addsprite(shoes[j].GetComponent<Image>(), asset.assetpreviewlink));
                 k++;

                 break;
         }*/



    }




    public IEnumerator addsprite(Image data, string rewview)
    {


        // WHEN IMAGES ARE IN PNG FORMAT
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(rewview))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                data.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));


            }
        }

        yield return null;





        //WHEN IMAGES ARE IN ASSET BUNDLE FORMAT
        /*
                using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(rewview))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.isNetworkError)
                    {
                        Debug.Log(uwr.error);
                    }
                    else
                    {
                        AssetBundle bund = DownloadHandlerAssetBundle.GetContent(uwr);

                        if (bund != null)
                        {
                            AssetBundleRequest newRequest = bund.LoadAllAssetsAsync<Texture2D>();
                            while (!newRequest.isDone)
                            {
                                Debug.Log(newRequest.progress);
                                yield return null;
                            }
                            if (newRequest.isDone)
                            {
                                object obj = newRequest.asset;

                                Texture2D tex = (Texture2D)obj;
                                //  if(tex!=null)
                                data.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));

                            }

                        }
                        if (bund != null)
                            bund.Unload(false);

                    }
                }

                yield return null;
        */
    }




}
