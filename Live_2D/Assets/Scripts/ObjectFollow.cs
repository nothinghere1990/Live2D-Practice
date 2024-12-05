using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Raycasting;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public CubismRaycaster raycaster;
    public CubismRaycastHit[] hits;
    public CubismDrawable selectedDrawable;
    public int closestPointIndex;
    public int farestPointIndex;
    public Vector3 closestPoint;
    private Vector3 farestPoint;
    public Vector3 offset;
    public Vector3 startDirection;
    public float startAngle;
    public GameObject sticker;
    public LineRenderer lineRenderer;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnValidate()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Initialize
            selectedDrawable = null;
            closestPointIndex = -1;
            farestPointIndex = -1;
            startDirection = Vector3.zero;
            startAngle = 0;
            
            //Cubism Raycaster detect mouse point on top most img layer.
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * _camera.farClipPlane, Color.green, 1);
            hits = new CubismRaycastHit[1];
            if (raycaster.Raycast(ray.origin, ray.direction * _camera.farClipPlane, hits) > 0)
            {
                print($"{hits[0].Drawable.name} : {hits[0].WorldPosition}");
                selectedDrawable = hits[0].Drawable;
                
                float min = Mathf.Infinity;
                float max = 0;

                //i = 0
                // Distances of "Points around One Polygon" to Mouse Point
                for (int i = 0; i < hits[0].Drawable.VertexPositions.Length; i++)
                {
                    Vector3 point = hits[0].Drawable.VertexPositions[i];
                    float dis = Vector3.Distance(point, hits[0].WorldPosition);
                    
                    //Find the closest point "index" to mouse point.
                    if (dis < min)
                    {
                        closestPointIndex = i;
                        min = dis;
                    }
                    
                    //Find the farest point "index" to mouse point.
                    if (dis > max)
                    {
                        farestPointIndex = i;
                        max = dis;
                    }
                }

                closestPoint = selectedDrawable.VertexPositions[closestPointIndex];
                closestPoint.x *= transform.lossyScale.x;//Closest point move with scale.
                closestPoint.y *= transform.lossyScale.y;
                closestPoint.z = _camera.nearClipPlane;

                farestPoint = selectedDrawable.VertexPositions[farestPointIndex];
                farestPoint.x *= transform.lossyScale.x;
                farestPoint.y *= transform.lossyScale.y;
                farestPoint.z = _camera.nearClipPlane;

                startDirection = farestPoint - closestPoint;
                offset = hits[0].WorldPosition - closestPoint;
                offset.z = 0;
            }
            else print("miss");
        }

        if (selectedDrawable != null)
        {
            closestPoint = selectedDrawable.VertexPositions[closestPointIndex];
            closestPoint.x *= transform.lossyScale.x;
            closestPoint.y *= transform.lossyScale.y;
            closestPoint.z = _camera.nearClipPlane;
            
            farestPoint = selectedDrawable.VertexPositions[farestPointIndex];
            farestPoint.x *= transform.lossyScale.x;
            farestPoint.y *= transform.lossyScale.y;
            farestPoint.z = _camera.nearClipPlane;

            
            //Draw line from closest point to farest point.
            lineRenderer.SetPositions(new Vector3[2] { closestPoint, farestPoint });
            lineRenderer.widthMultiplier = 0.03f;
            Debug.DrawLine(closestPoint, closestPoint + startDirection, Color.blue, Time.deltaTime);
            
            //Draw vertex pos lines.
            int length = selectedDrawable.VertexPositions.Length;
            for (int i = 0; i < length; i++)
            {
                Vector3 point = selectedDrawable.VertexPositions[i];
                Vector3 nextPoint = selectedDrawable.VertexPositions[(i + 1) % length];
                Debug.DrawLine(new Vector3(point.x* transform.lossyScale.x, point.y* transform.lossyScale.y,Camera.main.nearClipPlane),
                    new Vector3(nextPoint.x * transform.lossyScale.x, nextPoint.y * transform.lossyScale.y, Camera.main.nearClipPlane), Color.red, Time.deltaTime);
            }
        }

        if (sticker == null) return;
        sticker.transform.position = closestPoint + offset;
        sticker.transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(startDirection, (farestPoint - closestPoint), startDirection));
    }
}