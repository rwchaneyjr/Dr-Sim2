using UnityEngine;
using UnityEngine.SceneManagement;
public class ClickToLoadScene : MonoBehaviour
{
    public GameObject cannonPrefab;
    public Camera mainCamera;

    void Update()
    {
        // Left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}