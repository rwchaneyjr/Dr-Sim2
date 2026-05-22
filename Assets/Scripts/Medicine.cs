using UnityEngine;

public class Medicine : MonoBehaviour
{
    public Patient.Condition treatsCondition;

    public void Apply(Patient patient)
    {
        // Correct medicine
        if (patient.currentCondition == treatsCondition)
        {
            patient.Heal(30f);
        }
        else
        {
            // WRONG → BLACK
            patient.AdverseReaction();
        }
    }
}