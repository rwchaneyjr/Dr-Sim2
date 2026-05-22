using TMPro;
using UnityEngine;

public class ForceResultText : MonoBehaviour
{
    TMP_Text txt;

    void Start()
    {
        // ✅ FORCE PARENT ACTIVE
        if (transform.parent != null)
            transform.parent.gameObject.SetActive(true);

        // ✅ FORCE THIS ACTIVE
        gameObject.SetActive(true);

        txt = GetComponent<TMP_Text>();

        if (txt != null)
        {
            txt.enabled = true;
            txt.alpha = 1f;
            txt.color = Color.black;
            txt.fontSize = 60;
            txt.text = "Correct cure!";
        }

        RectTransform rect = GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.sizeDelta = new Vector2(800, 120);
        }

        Canvas.ForceUpdateCanvases();

        Debug.Log("RESULT FORCED ON");
    }
}