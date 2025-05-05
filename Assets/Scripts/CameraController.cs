using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float height = 8.0f;
    public float rotationAngle = 45.0f;
    public float smoothness = 5.0f;

    public float minZoom = 5.0f;
    public float maxZoom = 15.0f;
    public float zoomSpeed = 1.0f;
    private Vector3 targetPosition;

    void Start()
    {
        if (target == null)
        {
            NPCController player = FindFirstObjectByType<NPCController>();
            if (player != null)
                target = player.transform;
        }

        Camera.main.orthographic = true;
        Camera.main.orthographicSize = distance / 2.0f;
        UpdateCameraPosition();
    }

    void LateUpdate()
    {
        if (target == null) return;
        HandleZoom();
        UpdateCameraPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
    }

    private void UpdateCameraPosition()
    {
        if (target == null) return;
        float radians = rotationAngle * Mathf.Deg2Rad;
        float x = target.position.x - distance * Mathf.Sin(radians);
        float z = target.position.z - distance * Mathf.Cos(radians);

        targetPosition = new Vector3(x, target.position.y + height, z);
        transform.LookAt(target.position);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minZoom, maxZoom);
            Camera.main.orthographicSize = distance / 2.0f;
        }
    }
}