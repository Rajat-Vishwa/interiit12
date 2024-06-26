using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public float speed = 5f;
    public GameObject planePrefab; 
    private Transform lastPlaneEnd;

    void Start()
    {
        GameObject initialPlane = Instantiate(planePrefab, transform.position, Quaternion.identity);
        lastPlaneEnd = initialPlane.transform.Find("EndMarker");
    }

    void Update()
    {
        float verticalInput = 3f; // Constant forward movement
        Vector3 movement = new Vector3(0f, 0f, verticalInput) * speed * Time.deltaTime;
        transform.Translate(movement);
        if (lastPlaneEnd != null && transform.position.z > lastPlaneEnd.position.z - 50f)
        {
            SpawnNewPlane();
            DestroyOldPlanes();
        }
    }

    void SpawnNewPlane()
{
    GameObject newPlane = Instantiate(planePrefab, lastPlaneEnd.position, Quaternion.identity);
    Transform endMarker = newPlane.transform.Find("EndMarker");
    if (endMarker != null)
    {
        lastPlaneEnd = endMarker;
    }
    Vector3 offset = lastPlaneEnd.position - newPlane.transform.position;
    newPlane.transform.position += offset;
}


    void DestroyOldPlanes()
    {
        GameObject[] oldPlanes = GameObject.FindGameObjectsWithTag("PlaneSegment"); 
        foreach (GameObject oldPlane in oldPlanes)
        {
            if (oldPlane.transform.position.z < transform.position.z - GetPlaneLength())
            {
                Debug.Log("Destroying plane at position: " + oldPlane.transform.position);
                Destroy(oldPlane);
            }
        }
    }


    float GetPlaneLength()
    {
        return 10f;
    }
}
