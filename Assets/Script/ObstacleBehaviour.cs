using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public float scrollSpeed = 0.8f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(-Vector3.forward * scrollSpeed * Time.deltaTime);    
    }
}
