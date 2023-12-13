using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer MainMixer;
    public void SetFullScreen(bool isFullScreen){
        Screen.fullScreen=isFullScreen;

    }

    public void SetVolume(float volume){
        float setTo = 20 * Mathf.Log10(volume);
       MainMixer.SetFloat("Volume",setTo);
    }
}
