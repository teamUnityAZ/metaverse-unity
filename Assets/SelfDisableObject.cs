using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisableObject : MonoBehaviour
{
    public static SelfDisableObject Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        //StartCoroutine(DisabelObjects(5f));
    }

    public IEnumerator DisabelObjects(float value)
    {
        yield return new WaitForSeconds(value);
        gameObject.SetActive(false);
    }
}
