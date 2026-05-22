/*using UnityEngine;

public class PlayerDoctorTrigger : MonoBehaviour
{
    public DoctorHealTile uiManager;
    public DoctorTool doctorTool;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered trigger: " + other.name);

        if (!other.CompareTag("DoctorZone")) return;

        uiManager.ShowUI();

        // 🔥 THIS IS THE FIX
        Patient patient = other.GetComponentInChildren<Patient>();

        if (doctorTool != null && patient != null)
        {
            doctorTool.SelectPatient(patient);
        }
        else
        {
            Debug.LogWarning("DoctorTool or Patient missing!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited trigger: " + other.name);

        if (!other.CompareTag("DoctorZone")) return;

        uiManager.HideUI();

        if (doctorTool != null)
        {
            doctorTool.ClearUI();
        }
    }
}*/