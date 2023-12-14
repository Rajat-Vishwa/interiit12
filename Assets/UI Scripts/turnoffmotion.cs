using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnoffmotion : MonoBehaviour
{
    public MonoBehaviour scriptToToggle;
    private bool PlayerController = true;

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the state of the script
            PlayerController = !PlayerController;

            // Enable or disable the script based on the state
            scriptToToggle.enabled = PlayerController;
        }
    }
}
