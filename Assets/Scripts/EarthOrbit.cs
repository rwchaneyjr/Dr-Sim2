using UnityEngine;

public class EarthOrbit : MonoBehaviour
{
    public Transform target; // drag Earth here

    [Header("Orbit Settings")]
    public float distance = 50f;
    public float zoomSpeed = 10f;
    public float rotateSpeed = 100f;

    private float currentX = 0f;
    private float currentY = 20f;

    void Update()
    {
        if (target == null) return;

        // 🔄 ROTATE AROUND EARTH (Right Mouse)
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        }

        // 🔍 ZOOM IN / OUT (Scroll Wheel)
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, 1f, 10f);

        // 🎯 POSITION CALCULATION
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, 0, -distance);

        transform.position = target.position + rotation * direction;
        transform.LookAt(target);
    }
}