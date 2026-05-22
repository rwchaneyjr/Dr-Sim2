using UnityEngine;

public class CameraFollowTurn : MonoBehaviour
{
    public Transform target;

    [Header("Camera Position")]
    public Vector3 farOffset = new Vector3(0f, 80f, -1f);
    public Vector3 closeOffset = new Vector3(0f, 1f, 0f);

    [Header("Zoom")]
    public float zoomSpeed = 1.5f;

    private Vector3 currentOffset;

    void Start()
    {
        // Start far away
        currentOffset = farOffset;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Slowly move offset from far to close
        currentOffset = Vector3.Lerp(
            currentOffset,
            closeOffset,
            zoomSpeed * Time.deltaTime
        );

        // Follow target position, but DO NOT rotate camera
        transform.position = target.position + currentOffset;

        // No camera turning
        // transform.rotation = ...
    }
}