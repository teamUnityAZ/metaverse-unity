
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScreenShot : MonoBehaviour
{
    bool takePicture;
    [SerializeField]
    RawImage preview;
    string path;
    public static ScreenShot instance;

    public GameObject arObjects;
    public Button captureButton;
    public GameObject cameraViewPage;
    //public GameObject recordButton;
    private void Start()
    {
       path = Application.persistentDataPath + "/" + "ScreenShot.jpg";
    }

  
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (takePicture)
        {
            takePicture = false;

            var temRend = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, temRend);

            Texture2D tempText = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            Rect rect = new Rect(0, 0, source.width, source.height);
            tempText.ReadPixels(rect, 0, 0, false);
            tempText.Apply();
            preview.texture = tempText;
            preview.transform.localScale = Vector3.one;
            arObjects.SetActive(false);
            Save(path, tempText);
        }
        Graphics.Blit(source, destination);
    }

    public void TakeScreenShot()
    {
        takePicture = true;
        //if (ARHeadWebCamTextureExample.mode == Mode.Action)
        //{
        //    cameraViewPage.SetActive(false);
        //}
        //captureButton.gameObject.GetComponent<RecordButton>().enabled = false;
        //cameraViewPage.SetActive(false);
        //recordButton.SetActive(false);
    }

    public void BackButton()
    {
        preview.transform.localScale = Vector3.zero;
        arObjects.SetActive(true);
        cameraViewPage.SetActive(true);
        //captureButton.gameObject.GetComponent<RecordButton>().enabled = true;
        //cameraViewPage.SetActive(true);
        //recordButton.SetActive(true);
    }

    void Save(string path, Texture2D tex)
    {
        byte[] bytes = tex.EncodeToJPG();
        File.WriteAllBytes(path, bytes);
        Debug.Log("File saved");
    }
    public void GoToHome()
    {
        //UserLogin.m_IsLoggedIn = true;
        SceneManager.LoadScene(0);
    }


}
