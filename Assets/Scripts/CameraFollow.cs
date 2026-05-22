using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Start Camera")]
    public Vector3 startOffset = new Vector3(0f, 7f, -8f);
    public float startMoveSpeed = 1.5f;
    public float startDuration = 3f;

    [Header("Normal Camera")]
    public Vector3 normalOffset = new Vector3(0f, 2.75f, -1f);
    public float normalMoveSpeed = 2.5f;
    public float turnSpeed = 5f;
   // public Transform Patient;
    public Transform target;
    float timer = 0f;

    void LateUpdate()
    {
        if (target == null)
        {
            Patient patient = FindObjectOfType<Patient>();

            if (patient != null)
            {
                target = patient.transform;
                Debug.Log("CAMERA FOUND PATIENT");
            }
            else
            {
                return;
            }
        }

        timer += Time.deltaTime;

        Vector3 activeOffset;
        float activeSpeed;

        if (timer < startDuration)
        {
            activeOffset = startOffset;
            activeSpeed = startMoveSpeed;
        }
        else
        {
            activeOffset = normalOffset;
            activeSpeed = normalMoveSpeed;
        }

        Vector3 wantedPosition =
            target.position + activeOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            wantedPosition,
            activeSpeed * Time.deltaTime
        );

      /*  Quaternion wantedRotation =
            Quaternion.LookRotation(target.position - transform.position);
        if (timer >= startDuration)
        {
            wantedRotation *= Quaternion.Euler(-20f, 0f, 0f);
        }
      //  transform.rotation = Quaternion.Slerp(
        //    transform.rotation,
        //    wantedRotation,
         //   turnSpeed * Time.deltaTime
      //  );*/
    }
}