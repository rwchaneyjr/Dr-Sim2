using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject target;

    public int row;
    public int col;

    // 👑 KING POSITION
    public int targetRow = 0;
    public int targetCol = 4;

    void Start()
    {
        if (target == null) return;

        // ✅ ONLY ACTIVATE THE CORRECT ROOM
        if (row == targetRow && col == targetCol)
        {
            ShowTarget();
        }
        else
        {
            HideTarget();
        }
    }

    public void ShowTarget()
    {
        target.SetActive(true);
        Debug.Log("👑 Target ACTIVE at: " + row + "," + col);
    }

    public void HideTarget()
    {
        target.SetActive(false);
    }
}
