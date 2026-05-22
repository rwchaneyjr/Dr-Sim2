using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PatientCondition : MonoBehaviour
{
    public TMP_Dropdown conditionDropdown;
    public TMP_Text resultText;
    public TMP_Text instructionText;
    public GameObject patientObject;

    private Patient.Condition currentCondition = Patient.Condition.Dehydration;

    void Start()
    {
        SetupDropdown();
    }

    void SetupDropdown()
    {
        if (conditionDropdown == null) return;

        conditionDropdown.ClearOptions();

        List<string> options = new List<string>()
        {
            "Dehydration",
            "Infection",
            "Fever",
            "Burn",
            "Sprain",
            "Heart Palpitation",
            "Headache",
            "Food Poisoning",
            "Cold",
            "Broken Arm",
            "Flu",
            "Tooth Ache",
            "Stomach Pain"
        };

        conditionDropdown.AddOptions(options);
        conditionDropdown.value = 0;
        conditionDropdown.RefreshShownValue();
    }

    public void OnConditionSelected(int index)
    {
        if (conditionDropdown == null) return;

        string choice = conditionDropdown.options[index].text;
        currentCondition = ConditionFromText(choice);

        Patient patient = GetPatient();

        if (patient != null)
            patient.SetCondition(currentCondition);

        if (resultText != null)
            resultText.text = "Patient chose: " + choice;
    }

    public void ConfirmCondition()
    {
        if (conditionDropdown == null) return;

        string choice = conditionDropdown.options[conditionDropdown.value].text;
        currentCondition = ConditionFromText(choice);

        Patient patient = GetPatient();

        if (patient != null)
            patient.SetCondition(currentCondition);

        if (resultText != null)
            resultText.text = "Patient confirmed: " + choice;
    }

    Patient GetPatient()
    {
        if (patientObject != null)
            return patientObject.GetComponent<Patient>();

        return FindObjectOfType<Patient>();
    }

    Patient.Condition ConditionFromText(string text)
    {
        switch (text)
        {
            case "Dehydration": return Patient.Condition.Dehydration;
            case "Infection": return Patient.Condition.Infection;
            case "Fever": return Patient.Condition.Fever;
            case "Burn": return Patient.Condition.Burn;
            case "Sprain": return Patient.Condition.Sprain;
            case "Heart Palpitation": return Patient.Condition.HeartPalpitation;
            case "Headache": return Patient.Condition.Headache;
            case "Food Poisoning": return Patient.Condition.FoodPoisoning;
            case "Cold": return Patient.Condition.Cold;
            case "Broken Arm": return Patient.Condition.BrokenArm;
            case "Flu": return Patient.Condition.Flu;
            case "Tooth Ache": return Patient.Condition.ToothAche;
            case "Stomach Pain": return Patient.Condition.StomachPain;
            default: return Patient.Condition.Dehydration;
        }
    }

    public Patient.Condition GetCurrentCondition()
    {
        return currentCondition;
    }
}