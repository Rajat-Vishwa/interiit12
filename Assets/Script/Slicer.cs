using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    private Vector3 startPos, endPos;
    private bool isDrawing = false;
    public LayerMask slicableLayer;

    void Start()
    {
        startPos = Vector3.zero;
        endPos = Vector3.zero;
    }

    void Update()
    {
        isDrawing = Input.GetMouseButton(0);
        if(Input.GetMouseButtonUp(0)){
            isDrawing = false;
            startPos = Vector3.zero;
            endPos = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if(isDrawing){
            if(startPos == Vector3.zero){
                RaycastHit hit = ShootRayAtPoint(Input.mousePosition);
                if(hit.collider != null){
                    startPos = hit.point;
                }
            }else{
                RaycastHit hit = ShootRayAtPoint(Input.mousePosition);
                if(hit.collider != null){
                    endPos = hit.point;
                    Debug.DrawLine(startPos, endPos, Color.red, 0.1f);
                }
            }
        }

    }

    public RaycastHit ShootRayAtPoint(Vector3 point){
        Ray ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, slicableLayer)){
            Debug.Log(hit.collider.gameObject.name);
        }
        return hit;
    }
}
