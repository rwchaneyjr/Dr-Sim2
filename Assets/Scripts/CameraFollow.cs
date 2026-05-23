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

    [Header("Patient Visibility")]
    public float patientAppearDelay = 0.5f;
    public float cameraRestDistance = 0.05f;

    public Transform target;
    float timer = 0f;
    Renderer[] targetRenderers;
    bool[] targetRendererStartStates;
    Transform renderersTarget;
    bool hasAppliedPatientVisibility;
    bool currentPatientVisibility;
    float patientAppearTimer = 0f;
    bool patientHasAppeared;

    void LateUpdate()
    {
        if (target == null)
        {
            Patient patient = FindObjectOfType<Patient>();

            if (patient != null)
            {
                target = patient.transform;
                CacheTargetRenderers();
                Debug.Log("CAMERA FOUND PATIENT");
            }
            else
            {
                return;
            }
        }

        timer += Time.deltaTime;
        CacheTargetRenderers();

        Vector3 activeOffset;
        float activeSpeed;
        bool normalCameraActive;

        if (timer < startDuration)
        {
            activeOffset = startOffset;
            activeSpeed = startMoveSpeed;
            normalCameraActive = false;
        }
        else
        {
            activeOffset = normalOffset;
            activeSpeed = normalMoveSpeed;
            normalCameraActive = true;
        }

        Vector3 wantedPosition = target.position + activeOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            wantedPosition,
            activeSpeed * Time.deltaTime
        );

        bool cameraAtRest =
            normalCameraActive &&
            Vector3.Distance(transform.position, wantedPosition) <= cameraRestDistance;

        UpdatePatientVisibility(normalCameraActive, cameraAtRest);

        Quaternion wantedRotation =
            Quaternion.LookRotation(target.position - transform.position);

        if (timer >= startDuration)
        {
            wantedRotation *= Quaternion.Euler(-20f, 0f, 0f);
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            wantedRotation,
            turnSpeed * Time.deltaTime
        );
    }

    void CacheTargetRenderers()
    {
        if (target == null || renderersTarget == target)
            return;

        renderersTarget = target;
        targetRenderers = target.GetComponentsInChildren<Renderer>(true);
        targetRendererStartStates = new bool[targetRenderers.Length];

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            targetRendererStartStates[i] = targetRenderers[i].enabled;
        }

        hasAppliedPatientVisibility = false;
        patientAppearTimer = 0f;
        patientHasAppeared = false;
    }

    void SetPatientVisible(bool visible)
    {
        if (targetRenderers == null || targetRendererStartStates == null)
            return;

        if (hasAppliedPatientVisibility && currentPatientVisibility == visible)
            return;

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] != null)
                targetRenderers[i].enabled = visible && targetRendererStartStates[i];
        }

        currentPatientVisibility = visible;
        hasAppliedPatientVisibility = true;
    }

    void UpdatePatientVisibility(bool normalCameraActive, bool cameraAtRest)
    {
        if (patientHasAppeared)
        {
            SetPatientVisible(true);
            return;
        }

        if (!normalCameraActive || !cameraAtRest)
        {
            patientAppearTimer = 0f;
            SetPatientVisible(false);
            return;
        }

        patientAppearTimer += Time.deltaTime;

        if (patientAppearTimer >= patientAppearDelay)
        {
            patientHasAppeared = true;
            SetPatientVisible(true);
        }
        else
        {
            SetPatientVisible(false);
        }
    }

    void OnDisable()
    {
        SetPatientVisible(true);
    }
}