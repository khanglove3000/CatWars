using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Vector3 draOrigin;

    [SerializeField]
    private SpriteRenderer mapRenderer;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x/2f;   
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x/2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y/2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y/2f;
    }

    private void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
            draOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
     

        if(Input.GetMouseButton(0))
        {
            Vector3 difference = draOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position = ClampCamera(cam.transform.position + difference, mapRenderer.transform);

        }
    }
    private Vector3 ClampCamera(Vector3 targetPosition, Transform posY)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, posY.position.y, targetPosition.z);
    }
}
