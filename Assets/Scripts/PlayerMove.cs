using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [Header("Interaction")]
    public bool isOnTarget = false;
    public float interactDistance = 1.5f;
    public GameObject doctorPrefab;
    private GameObject spawnedDoctor;
    public GameObject nursePrefab;
    private GameObject spawnedNurse;
    public GameObject cubePrefab;
    [Header("UI")]
    public GameObject healthMenuPanel;
    public GameObject diagnosisText;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float turnSpeed = 8f;
    public float stoppingDistance = 1f;
    public Camera playerCamera;
    [Header("UI")]
    public GameObject healthMenu;

    [Header("Target Sphere")]
    public Transform targetSphere;
    public DoctorTool doctorTool;

    [Header("Animation")]
    public Animator animatorK;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (animatorK == null)
            animatorK = GetComponent<Animator>();

        rb.useGravity = false;
        rb.isKinematic = false;

        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        CheckIfOnTarget();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animatorK.SetBool("Walk", false);
            animatorK.SetBool("Idle", false);
            animatorK.SetTrigger("Press");

            ActivateInteraction();
            StartCoroutine(ShowUIAfterAnim());
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StopMovingWithoutIdle();
            return;
        }

        if (targetSphere == null)
        {
            StopMoving();
            return;
        }

        Vector3 targetPos = new Vector3(targetSphere.position.x, rb.position.y, targetSphere.position.z);
        float distance = Vector3.Distance(rb.position, targetPos);

        if (distance <= stoppingDistance)
        {
            StopMoving();
            return;
        }

        Vector3 moveDir = (targetPos - rb.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        SetAnimation(true);
    }

    void CheckIfOnTarget()
    {
        if (targetSphere == null) return;

        float dist = Vector3.Distance(transform.position, targetSphere.position);
        isOnTarget = dist <= interactDistance;
    }

    void StopMoving()
    {
        rb.velocity = Vector3.zero;
        SetAnimation(false);
    }

    void StopMovingWithoutIdle()
    {
        rb.velocity = Vector3.zero;
    }

    void SetAnimation(bool walking)
    {
        if (animatorK != null)
        {
            animatorK.SetBool("Walk", walking);
            animatorK.SetBool("Idle", !walking);
        }
    }

    public void ActivateInteraction()
    {
        if (doctorPrefab == null) return;

        if (spawnedDoctor != null)
        {
            Destroy(spawnedDoctor);
            spawnedDoctor = null;
        }

        if (spawnedNurse != null)
        {
            Destroy(spawnedNurse);
            spawnedNurse = null;
        }

        Vector3 doctorPos = transform.position + new Vector3(1f, 0f, .6f);
        doctorPos.y = transform.position.y;

        spawnedDoctor = Instantiate(
            doctorPrefab,
            doctorPos,
            Quaternion.LookRotation(playerCamera.transform.position - doctorPos)
        );
        spawnedDoctor.transform.position=new Vector3(spawnedDoctor.transform.position.x, transform.position.y-.9f, spawnedDoctor.transform.position.z);
        if (nursePrefab != null)
        {
            Vector3 nursePos = doctorPos + new Vector3(1.5f, 0f, 0f);
            nursePos.y = transform.position.y;

            spawnedNurse = Instantiate(
                nursePrefab,
                nursePos,
                Quaternion.LookRotation(transform.position - nursePos)
            );
        }
    }

    IEnumerator ShowUIAfterAnim()
    {
        yield return new WaitForSeconds(1.0f);

        DoctorTool tool = FindObjectOfType<DoctorTool>();

        if (tool == null) yield break;

        tool.ShowCureUI();
    }

    public void DestroyDoctorAfterDelay(float delay)
    {
        StartCoroutine(DestroyDoctorRoutine(delay));
    }

    IEnumerator DestroyDoctorRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (spawnedDoctor != null)
        {
            Destroy(spawnedDoctor);
            spawnedDoctor = null;
            Debug.Log("Spawned doctor removed.");
        }

        if (spawnedNurse != null)
        {
            Destroy(spawnedNurse);
            spawnedNurse = null;
            Debug.Log("Spawned nurse removed.");
        }
    }

    public void HideSpawnedDoctor()
    {
        if (spawnedDoctor != null)
        {
            Destroy(spawnedDoctor);
            spawnedDoctor = null;
            Debug.Log("Doctor removed instantly.");
        }
    }

    void ResetPress()
    {
        animatorK.SetBool("Press", false);
    }
}