using UnityEngine;

public class CameraMouseMovement : MonoBehaviour
{
    [SerializeField] private float edgeActivationDistance;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraResizeSpeed;
    [SerializeField] private Vector2 cameraSizeBounds;

    private Bounds worldBounds;
    private Camera selfCamera;

    private void Awake()
    {
        selfCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        GridExtension gridExtension = FindObjectOfType<GridExtension>();
        
        Vector2 center = new Vector2(gridExtension.gridSize.x / 2.0f * gridExtension.cellSize.x,
            gridExtension.gridSize.y / 2.0f * gridExtension.cellSize.y);

        worldBounds = new Bounds
        {
            min = Vector2.zero,
            max = new Vector2(gridExtension.gridSize.x * gridExtension.cellSize.x,
                              gridExtension.gridSize.y * gridExtension.cellSize.y)
        };

        transform.position = new Vector3(center.x, center.y, transform.position.z);
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 newPosition = transform.position;

        if (mousePosition.x > Screen.width - edgeActivationDistance)
            newPosition.x += cameraMoveSpeed * Time.deltaTime;

        if (mousePosition.x < edgeActivationDistance)
            newPosition.x -= cameraMoveSpeed * Time.deltaTime;

        if (mousePosition.y > Screen.height - edgeActivationDistance)
            newPosition.y += cameraMoveSpeed * Time.deltaTime;

        if (mousePosition.y < edgeActivationDistance)
            newPosition.y -= cameraMoveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, worldBounds.min.x, worldBounds.max.x);
        newPosition.y = Mathf.Clamp(newPosition.y, worldBounds.min.y, worldBounds.max.y);

        transform.position = newPosition;

        float cameraSize = Mathf.Clamp(
            selfCamera.orthographicSize - Input.mouseScrollDelta.y * cameraResizeSpeed * Time.deltaTime,
            cameraSizeBounds.x,
            cameraSizeBounds.y);

        selfCamera.orthographicSize = cameraSize;
    }
}