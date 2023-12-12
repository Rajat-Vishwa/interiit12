using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slicer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform lineStart, lineEnd;
    private bool isDrawing = false;
    public LineRenderer lineRenderer;
    public LayerMask slicableLayer;
    public Vector3 offset;

    public Vector2 startUV, endUV;

    void Start()
    {
        lineStart.position = Vector3.zero;
        lineEnd.position = Vector3.zero;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(Input.GetMouseButton(0) &&   lineStart.position != Vector3.zero && lineEnd.position != Vector3.zero && isDrawing){
            lineRenderer.SetPosition(0, lineStart.position);
            lineRenderer.SetPosition(1, lineEnd.position);
            lineRenderer.enabled = true;
        }
        else{
            lineRenderer.enabled = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineStart.position = Vector3.zero;
        lineEnd.position = Vector3.zero;
        isDrawing = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lineStart.position = Vector3.zero;
        lineEnd.position = Vector3.zero;
        isDrawing = false;
        lineStart.parent = transform;
        lineEnd.parent = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDrawing){
            if(lineStart.position == Vector3.zero){
                RaycastHit hit = ShootRayAtPoint(Input.mousePosition);
                if(hit.collider != null){
                    startUV = hit.textureCoord; // Get the UV of the hit point
                    lineStart.position = hit.point + offset;
                    lineStart.parent = hit.transform.parent;
                }
            }else{
                RaycastHit hit = ShootRayAtPoint(Input.mousePosition);
                if(hit.collider != null){
                    endUV = hit.textureCoord; // Get the UV of the hit point
                    lineEnd.position = hit.point + offset;
                    lineEnd.parent = hit.transform.parent;

                    hit.transform.parent.GetComponent<ObstacleBehaviour>().Slice(startUV, endUV);
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
