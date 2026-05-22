using UnityEngine;

public class OrbitAroundEarthPause : MonoBehaviour
{
    [Header("Target")]
    public Transform earthTarget;

    [Header("Orbit Settings")]
    public float orbitSpeed = 8f;
    public float heightOffset = 3f;
    public float distance = 20f;

    [Header("Smoothing")]
    public float moveSmoothness = 2f;
    public float lookSmoothness = 2f;

    [Header("Pause Toggle")]
    public KeyCode pauseKey = KeyCode.Space;

    private float angle = 0f;
    private bool isPaused = false;

    void Start()
    {
        if (earthTarget == null)
        {
            Debug.LogError("No Earth target assigned.");
            return;
        }

        // Start angle from current camera position
        Vector3 offset = transform.position - earthTarget.position;
        angle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if (earthTarget == null) return;

        // Toggle pause on/off
        if (Input.GetKeyDown(pauseKey))
        {
            isPaused = !isPaused;
            Debug.Log("Orbit paused: " + isPaused);
        }

        // Only move angle when not paused
        if (!isPaused)
        {
            angle += orbitSpeed * Time.deltaTime;
        }

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        Vector3 targetPosition = earthTarget.position + new Vector3(x, heightOffset, z);

        // Camera moves toward target position
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSmoothness * Time.deltaTime
        );

        // Camera keeps looking at Earth
        Quaternion targetRotation = Quaternion.LookRotation(earthTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            lookSmoothness * Time.deltaTime
        );
    }
}