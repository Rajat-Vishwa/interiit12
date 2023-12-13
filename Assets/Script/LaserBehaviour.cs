using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform laserHit;
    public Transform laserOrigin;

    private bool laserActive = false;
    public float laserDistance = 100f;

    [Space]
    public float timeSlowScale = 0.1f;
    public float timeNormalScale = 1f;
    public float timeDamp = 0.1f;

    public GameObject followCam, shoulderCam;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;    
        laserOrigin = transform;
    }

    void Update()
    {
        laserActive = Input.GetButton("Fire1");

        // Camera Switch
        if (Input.GetMouseButton(1)){
            followCam.SetActive(false);
            shoulderCam.SetActive(true);

            Time.timeScale = Mathf.Lerp(Time.timeScale, timeSlowScale, timeDamp * Time.deltaTime);
        }
        else{
            followCam.SetActive(true);
            shoulderCam.SetActive(false);

            Time.timeScale = Mathf.Lerp(Time.timeScale, timeNormalScale, timeDamp * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        
    }

}
