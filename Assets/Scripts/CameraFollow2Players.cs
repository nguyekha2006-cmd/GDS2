using UnityEngine;

public class CameraFollow2Players : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float smoothTime = 0.2f;
    private Vector3 velocity;

    public Vector2 offset = new Vector2(0f, 2.5f);

    public float minZoom = 5f;
    public float maxZoom = 10f;
    public float zoomLimiter = 10f;
    public float zoomSmooth = 5f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        MoveCamera();
        ZoomCamera();
    }

    public void SnapToTarget()
    {
        Vector3 midpoint = (player1.position + player2.position) / 2f;

        Vector3 newPos = new Vector3(
            midpoint.x + offset.x,
            midpoint.y + offset.y,
            transform.position.z
        );

        transform.position = newPos;
    }

    void MoveCamera()
    {
        Vector3 midpoint = (player1.position + player2.position) / 2f;

        Vector3 newPos = new Vector3(
            midpoint.x + offset.x,
            midpoint.y + offset.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    void ZoomCamera()
    {
        float distance = Vector2.Distance(player1.position, player2.position);

        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSmooth);
    }
}