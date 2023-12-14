using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lasersound : MonoBehaviour
{

    public AudioSource audioSource;
    public bool value = false;
    public GameObject otherGameObject;

    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        value = otherGameObject.GetComponent<Slicer>().isDrawing;
        if (value && !audioSource.isPlaying)
        {
            // Play the audio clip
            audioSource.Play();
        }

        // Check if playAudio is false and the audio is playing
        if (!value && audioSource.isPlaying)
        {
            // Stop the audio clip
            audioSource.Stop();
        }
    }
}