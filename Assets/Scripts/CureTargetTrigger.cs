using UnityEngine;

public class CureTargetTrigger : MonoBehaviour
{
    public bool leftPadAfterCure = false;

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Patient")) return;

        leftPadAfterCure = true;

        Patient patient = other.GetComponent<Patient>();
        DoctorTool tool = FindObjectOfType<DoctorTool>();

        if (patient == null || tool == null) return;

        Patient.Condition newCondition =
            (Patient.Condition)Random.Range(
                1,
                System.Enum.GetValues(typeof(Patient.Condition)).Length
            );

        patient.SetCondition(newCondition);

        Debug.Log("LEFT PAD → NEW CONDITION SET: " + newCondition);

        // Do NOT call SelectPatient here if you want it to wait
        // tool.SelectPatient(patient);
    }
}