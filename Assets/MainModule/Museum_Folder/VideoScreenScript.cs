using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VideoScreenScript : MonoBehaviour
{
    private void OnDisable()
    {
        Gamemanager._InstanceGM.mediaPlayer.AudioVolume = 0;
        SoundManager.Instance.MusicSource.volume = 0.2f;
        // Gamemanager._InstanceGM.UnMute2(true);
        Gamemanager._InstanceGM.m_youtubeAudio.mute = false;
        Gamemanager._InstanceGM.mediaPlayer.Stop();
    }
    private void OnEnable()
    {
        Gamemanager._InstanceGM.mediaPlayer.AudioVolume = 0;
        Gamemanager._InstanceGM.m_youtubeAudio.mute = true;
        // Gamemanager._InstanceGM.UnMute2(false);
    }
}