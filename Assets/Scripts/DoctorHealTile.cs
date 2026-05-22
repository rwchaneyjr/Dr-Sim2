using UnityEngine;

public class DoctorHealTile : MonoBehaviour
{
    [Header("UI objects to hide/show")]
    public GameObject[] uiObjects;

    private bool show = false;

    private void Start()
    {
        Debug.Log("DoctorHealTile Start called");
        SetUI(show);
    }

    public void ShowUI()
    {
        Debug.Log("ShowUI called");
        show = true;
        SetUI(show);
    }

    public void HideUI()
    {
        Debug.Log("HideUI called");
        show = false;
        SetUI(show);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("DoctorHealTile OnTriggerEnter called with " + other.name);
        if (!other.CompareTag("Player")) return;

        show = true;
        SetUI(show);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 🔴 REMOVE THIS LINE
        // show = false;
        // SetUI(show);
    }

    private void SetUI(bool state)
    {
        Debug.Log("DoctorHealTile SetUI called with state: " + state);

        for (int i = 0; i < uiObjects.Length; i++)
        {
            if (uiObjects[i] != null)
            {
                uiObjects[i].SetActive(state);
            }
        }
    }
}