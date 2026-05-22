using UnityEngine;

public class MouseMoveSphere_AnyCamera : MonoBehaviour
{
    [Header("Settings")]
  //  public float heightOffset = -.84f;
    public float moveSpeed = 15f;
    public float stoppingDistance = 0.01f;

    [Header("Layer")]
    public LayerMask groundLayer; // assign in Inspector

    private Vector3 targetPos;

    void Update()
    {
        // ✅ ONLY move while RIGHT mouse is held
        if (!Input.GetMouseButton(1)) return;

        Camera cam = GetActiveCamera();
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ✅ ONLY hit ground layer
        if (Physics.Raycast(ray, out hit, 500f, groundLayer))
        {
            targetPos = new Vector3(
    hit.point.x,
    0-.82f,   // 🔥 FORCE ABOVE FLOOR
    hit.point.z
);
        }

        // ✅ Smooth move
        float dist = Vector3.Distance(transform.position, targetPos);

        if (dist > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }
    }

    Camera GetActiveCamera()
    {
        foreach (Camera c in Camera.allCameras)
        {
            if (c.enabled)
                return c;
        }
        return null;
    }
}