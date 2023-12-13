using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowmotionsteps : MonoBehaviour
{
    public AudioClip soundClip; // Assign your audio clip in the Unity Editor
    private AudioSource audioSource;
    private AudioSource audioSourceoff;
    public GameObject otherGameObject;
    private bool isRightClicking = false;

    private void Start()
    {
        audioSourceoff=otherGameObject.GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 1 corresponds to the right mouse button
        {
            isRightClicking = true;
            PlaySoundLoop();
            audioSourceoff.Stop();

        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightClicking = false;
            StopSoundLoop();
            audioSourceoff.Play();
        }
    }

    private void PlaySoundLoop()
    {
        if (audioSource != null && soundClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = soundClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopSoundLoop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
