using UnityEngine;
using UnityEngine.UI;

public class Backgrounds : MonoBehaviour
{
    public Sprite[] backgrounds;
    public Image bgImage;

    private Color color;

    private void Start()
    {
        bgImage = GetComponentInChildren<Image>();
        InvokeRepeating("ChangeBackground", 5f, 5f);
    }

    void ChangeBackground()
    {
        int value = Random.Range(0, 3);
        gameObject.GetComponentInChildren<Image>().sprite = backgrounds[value];
    }
}
