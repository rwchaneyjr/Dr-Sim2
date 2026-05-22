using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [Header("Drag your 2 cameras here")]
    public Camera overheadCamera;
    public Camera closeUpCamera;

    private bool usingOverhead = false;

    void Start()
    {
        // Safety check
        if (overheadCamera == null || closeUpCamera == null)
        {
            Debug.LogError("Please assign both cameras in the Inspector.");
            return;
        }

        // Start with overhead camera on
        overheadCamera.enabled = false;
        closeUpCamera.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            usingOverhead = !usingOverhead;

            overheadCamera.enabled = !usingOverhead;
            closeUpCamera.enabled = usingOverhead;
        }
    }
}