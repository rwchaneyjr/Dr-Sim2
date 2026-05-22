using UnityEngine;

public class CameraFlyToEarth : MonoBehaviour
{
    public Transform earthTarget;   // drag Earth here
    public float speed = 5f;
    public float stopDistance = 12f;

    void Update()
    {
        if (earthTarget == null) return;

        float dist = Vector3.Distance(transform.position, earthTarget.position);

        if (dist > stopDistance)
        {
            // Move forward toward Earth
            transform.position = Vector3.MoveTowards(
                transform.position,
                earthTarget.position,
                speed * Time.deltaTime
            );

            // Always look at Earth
            transform.LookAt(earthTarget);
        }
    }
}