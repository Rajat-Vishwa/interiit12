using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMarkers : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");    
    }

    void Update()
    {
        Vector3 pos = player.transform.position;
        pos.z = transform.position.z;

        transform.position = pos;
    }
}
