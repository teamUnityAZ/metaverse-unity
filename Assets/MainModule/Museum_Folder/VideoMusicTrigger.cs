using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoMusicTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        
        Debug.Log("tagname==="+other.tag.ToString());
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.videoPlayerSource.gameObject.SetActive(true);
            SoundManager.Instance.MusicSource.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
      
        //Debug.Log("tagname 1===" + other.tag.ToString());
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.videoPlayerSource.gameObject.SetActive(false);
            SoundManager.Instance.MusicSource.enabled = true;

        }
        
    }
}
