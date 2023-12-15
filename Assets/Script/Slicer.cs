using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slicer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform lineStart, lineEnd;
    public bool isDrawing = false;
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
        isDrawing = Input.GetMouseButton(0);
        if(lineStart.position != Vector3.zero && lineEnd.position != Vector3.zero && isDrawing){
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrawing = false;

        // Recalculate UV Coordinates for drawing plane scale 2 times main plane 
        startUV = (startUV - Vector2.one * 0.25f) * 2f;
        endUV = (endUV - Vector2.one * 0.25f) * 2f;
        // Clamp UV Coordinates to the range [0, 1]
        startUV = new Vector2(Mathf.Clamp01(startUV.x), Mathf.Clamp01(startUV.y));
        endUV = new Vector2(Mathf.Clamp01(endUV.x), Mathf.Clamp01(endUV.y));
        Debug.Log("Recalculated UV Coordinates " + startUV + "; " + endUV);

        lineEnd.parent?.GetComponent<ObstacleBehaviour>().Slice(startUV, endUV);

        lineStart.parent = null;
        lineEnd.parent = null;
        lineStart.position = Vector3.zero;
        lineEnd.position = Vector3.zero;
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
                }
            }
        }

    }

    public RaycastHit ShootRayAtPoint(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, slicableLayer)){
            // Debug.Log(hit.collider.gameObject.name);
        }
        return hit;
    }
}
