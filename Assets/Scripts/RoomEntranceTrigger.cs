using UnityEngine;

public class RoomEntranceTrigger : MonoBehaviour
{
    private RoomController room;

    public TypewriterText resultTypewriter;

    void Start()
    {
        room = GetComponentInParent<RoomController>();

        if (room == null)
            Debug.LogError("No RoomController found on parent!");

        if (resultTypewriter == null)
            resultTypewriter = FindObjectOfType<TypewriterText>(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Patient")) return;

        Debug.Log("ENTERED ROOM");

        if (room != null)
        {
            room.ShowTarget();
            Debug.Log("Room target active: " + room.row + "," + room.col);
        }

        PlayDiagnosisFromPatient();
    }

    void PlayDiagnosisFromPatient()
    {
        Patient patient = FindObjectOfType<Patient>();

        if (patient == null)
        {
            Debug.LogError("No Patient found!");
            return;
        }

        DoctorTool tool = FindObjectOfType<DoctorTool>();
        if (tool != null)
            tool.SelectPatient(patient);

        if (resultTypewriter != null)
        {
            resultTypewriter.gameObject.SetActive(true);

            string ailment = patient.currentCondition.ToString();

            string message = "Diagnosis: " + ailment;

            resultTypewriter.ShowMessage(message);
        }
        else
        {
            Debug.LogError("No TypewriterText found!");
        }
    }
}