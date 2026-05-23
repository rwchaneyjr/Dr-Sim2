using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DoctorTool : MonoBehaviour
{
    public TMP_Text diagnosisText;
    public TMP_Text healthText;
    public TMP_Text resultText;

    public GameObject healthMenuPanel;
    public GameObject secondOpinionMenuPanel;
    public GameObject diagnosisPanel;

    public TMP_Dropdown treatmentDropdown;

    public GameObject doctorCanvas;
    public TMP_Text instructionText;
    public float instructionDuration = 4f;

    private Patient selectedPatient;
    private Coroutine diagnosisCoroutine;
    private Coroutine correctCureCoroutine;
    private Coroutine wrongCureCoroutine;

    void Start()
    {
        StartCoroutine(AutoSelectPatientAfterStart());

        if (doctorCanvas != null)
            doctorCanvas.SetActive(true);

        if (instructionText != null)
            instructionText.text = "Use W A S D  - C Key to Toggle Camera";

        HideCureUI();

        if (diagnosisPanel != null)
            diagnosisPanel.SetActive(false);

        if (diagnosisText != null)
        {
            diagnosisText.text = "";
            diagnosisText.gameObject.SetActive(false);
        }

        if (healthText != null)
        {
            healthText.text = "";
            healthText.gameObject.SetActive(false);
        }

        if (resultText != null)
        {
            resultText.text = "";
            resultText.color = Color.white;
            resultText.gameObject.SetActive(false);
        }

        SetupDropdownOptions();
        StartCoroutine(HideInstruction());
    }

    IEnumerator AutoSelectPatientAfterStart()
    {
        yield return new WaitForSeconds(2f);

        Patient patient = FindObjectOfType<Patient>();

        if (patient != null)
            SelectPatient(patient);
    }

    IEnumerator HideInstruction()
    {
        yield return new WaitForSeconds(instructionDuration);

        if (instructionText != null)
            instructionText.text = "";
    }

    void SetupDropdownOptions()
    {
        if (treatmentDropdown == null) return;

        treatmentDropdown.ClearOptions();

        List<string> options = new List<string>()
        {
            "Select Treatment",
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

        treatmentDropdown.AddOptions(options);
        treatmentDropdown.SetValueWithoutNotify(0);
        treatmentDropdown.RefreshShownValue();

        treatmentDropdown.onValueChanged.RemoveAllListeners();
        treatmentDropdown.onValueChanged.AddListener(delegate { CureFromDropdown(); });
    }

    public void SelectPatient(Patient patient)
    {
        if (patient == null) return;

        selectedPatient = patient;

        Patient.Condition newCondition = GetRandomTreatableCondition();

        if (selectedPatient.currentCondition == newCondition)
            newCondition = GetRandomTreatableCondition();

        selectedPatient.SetCondition(newCondition);

        if (diagnosisCoroutine != null)
            StopCoroutine(diagnosisCoroutine);

        if (diagnosisPanel != null)
            diagnosisPanel.SetActive(true);

        if (diagnosisText != null)
        {
            diagnosisText.gameObject.SetActive(true);
            diagnosisText.text = "Symptoms: " + selectedPatient.GetSymptoms();
        }

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        diagnosisCoroutine = StartCoroutine(ShowDiagnosisAfterDelay(2f));
    }

    IEnumerator ShowDiagnosisAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (diagnosisPanel != null)
            diagnosisPanel.SetActive(true);

        if (selectedPatient != null && diagnosisText != null)
        {
            diagnosisText.gameObject.SetActive(true);
            diagnosisText.text = "Diagnosis:\n" + selectedPatient.currentCondition + "\nPress Space key for\ncure options";
        }

        yield return new WaitForSeconds(3f);

        HideDiagnosisUI();
        diagnosisCoroutine = null;
    }
    void HideDiagnosisUI()
    {
        if (diagnosisPanel != null)
            diagnosisPanel.SetActive(false);

        if (diagnosisText != null)
            diagnosisText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (selectedPatient != null && healthText != null)
            healthText.text = "Health: " + selectedPatient.health.ToString("F0");
    }

    public void ShowCureUI()
    {
        if (healthMenuPanel != null)
            healthMenuPanel.SetActive(true);

        if (secondOpinionMenuPanel != null)
            secondOpinionMenuPanel.SetActive(true);

        if (healthText != null)
            healthText.gameObject.SetActive(true);

        if (treatmentDropdown != null)
            treatmentDropdown.gameObject.SetActive(true);
    }

    void HideCureUI()
    {
        if (healthMenuPanel != null)
            healthMenuPanel.SetActive(false);

        if (secondOpinionMenuPanel != null)
            secondOpinionMenuPanel.SetActive(false);

        if (healthText != null)
            healthText.gameObject.SetActive(false);

        if (treatmentDropdown != null)
            treatmentDropdown.gameObject.SetActive(false);
    }

    Patient.Condition GetRandomTreatableCondition()
    {
        Patient.Condition[] conditions =
        {
            Patient.Condition.Dehydration,
            Patient.Condition.Infection,
            Patient.Condition.Fever,
            Patient.Condition.Burn,
            Patient.Condition.Sprain,
            Patient.Condition.HeartPalpitation,
            Patient.Condition.Headache,
            Patient.Condition.FoodPoisoning,
            Patient.Condition.Cold,
            Patient.Condition.BrokenArm,
            Patient.Condition.Flu,
            Patient.Condition.ToothAche,
            Patient.Condition.StomachPain
        };

        return conditions[Random.Range(0, conditions.Length)];
    }

    public void Cure(Patient.Condition cureType)
    {
        if (selectedPatient == null) return;

        if (selectedPatient.currentCondition == cureType)
        {
            selectedPatient.Heal(30f);
            selectedPatient.Recover();

            if (resultText != null)
            {
                resultText.gameObject.SetActive(true);
                resultText.text = "Correct cure!";
                resultText.color = Color.green;
            }

            if (correctCureCoroutine != null)
                StopCoroutine(correctCureCoroutine);

            correctCureCoroutine = StartCoroutine(HideAfterCorrectCure());
        }
        else
        {
            selectedPatient.AdverseReaction();

            if (resultText != null)
            {
                resultText.gameObject.SetActive(true);
                resultText.text = "Wrong cure!";
                resultText.color = Color.red;
            }

            if (wrongCureCoroutine != null)
                StopCoroutine(wrongCureCoroutine);

            wrongCureCoroutine = StartCoroutine(HideAfterWrongCure());
        }
    }
    IEnumerator HideAfterWrongCure()
    {
        yield return new WaitForSeconds(1.0f);

        HideCureUI();

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        PlayerMove player = FindObjectOfType<PlayerMove>();

        if (player != null)
            player.DestroyDoctorAfterDelay(0f);

        if (CubeGridSpawner.Instance != null)
            CubeGridSpawner.Instance.MovePlayerLeft();

        wrongCureCoroutine = null;
    }
    IEnumerator HideAfterCorrectCure()
    {
        yield return new WaitForSeconds(1.5f);

        HideCureUI();

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        PlayerMove player = FindObjectOfType<PlayerMove>();

        if (player != null)
            player.DestroyDoctorAfterDelay(0f);

        if (CubeGridSpawner.Instance != null)
            CubeGridSpawner.Instance.ShowNextForwardTarget();

        correctCureCoroutine = null;
    }

    public void CureFromDropdown()
    {
        if (treatmentDropdown == null) return;

        string selected = treatmentDropdown.options[treatmentDropdown.value].text;

        if (selected == "Select Treatment") return;

        switch (selected)
        {
            case "Dehydration": Cure(Patient.Condition.Dehydration); break;
            case "Infection": Cure(Patient.Condition.Infection); break;
            case "Fever": Cure(Patient.Condition.Fever); break;
            case "Burn": Cure(Patient.Condition.Burn); break;
            case "Sprain": Cure(Patient.Condition.Sprain); break;
            case "Heart Palpitation": Cure(Patient.Condition.HeartPalpitation); break;
            case "Headache": Cure(Patient.Condition.Headache); break;
            case "Food Poisoning": Cure(Patient.Condition.FoodPoisoning); break;
            case "Cold": Cure(Patient.Condition.Cold); break;
            case "Broken Arm": Cure(Patient.Condition.BrokenArm); break;
            case "Flu": Cure(Patient.Condition.Flu); break;
            case "Tooth Ache": Cure(Patient.Condition.ToothAche); break;
            case "Stomach Pain": Cure(Patient.Condition.StomachPain); break;
        }
    }
}