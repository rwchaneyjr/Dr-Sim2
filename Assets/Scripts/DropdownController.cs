using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DropdownController : MonoBehaviour
{
    public TMP_Dropdown dropdown;     // drag your dropdown here
    public DoctorTool doctorTool;     // drag DoctorTool here

    void Start()
    {
        // STEP 1: Clear existing options
        dropdown.ClearOptions();

        // STEP 2: Create new options
        List<string> options = new List<string>()
        {
            "Dehydration",
            "Infection",
            "Fever",
            "Burn",
            "Sprain",
            "HeartPalpitation",
            "Headache"
        };

        dropdown.AddOptions(options);

        // STEP 3: Listen for selection
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void OnDropdownChanged(int index)
    {
        string selected = dropdown.options[index].text;

        Debug.Log("Selected: " + selected);

        // Convert string → enum
        Patient.Condition condition = StringToCondition(selected);

        // Call your cure function
        doctorTool.Cure(condition);
    }

    Patient.Condition StringToCondition(string name)
    {
        switch (name)
        {
            case "Dehydration": return Patient.Condition.Dehydration;
            case "Infection": return Patient.Condition.Infection;
            case "Fever": return Patient.Condition.Fever;
            case "Burn": return Patient.Condition.Burn;
            case "Sprain": return Patient.Condition.Sprain;
            case "HeartPalpitation": return Patient.Condition.HeartPalpitation;
            case "Headache": return Patient.Condition.Headache;
        }

        return Patient.Condition.Fever; // fallback
    }
}