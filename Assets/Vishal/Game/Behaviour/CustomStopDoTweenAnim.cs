using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomStopDoTweenAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        this.GetComponent<DOTweenAnimation>().DOPlay();
    }

    private void OnDisable()
    {
        this.GetComponent<DOTweenAnimation>().DOPause();
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
