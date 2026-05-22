using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterText : MonoBehaviour
{
    public TMP_Text textBox;
    public float letterDelay = 0.08f;

    [Header("Auto Hide")]
    public bool hideAfterFinish = true;
    public float hideDelay = 2f;

    private Coroutine typing;

    public void ShowMessage(string message)
    {
        gameObject.SetActive(true);

        if (textBox != null)
            textBox.gameObject.SetActive(true);

        if (typing != null)
            StopCoroutine(typing);

        if (message.StartsWith("Diagnosis:"))
        {
            message = "<color=red>" + message + "</color>";
        }

        typing = StartCoroutine(TypeLetters(message));
    }

    IEnumerator TypeLetters(string message)
    {
        if (textBox == null) yield break;

        textBox.color = Color.black;
        textBox.text = message;
        textBox.ForceMeshUpdate();
        textBox.maxVisibleCharacters = 0;

        int totalVisible = textBox.GetParsedText().Length;

        for (int i = 0; i <= totalVisible; i++)
        {
            textBox.maxVisibleCharacters = i;
            yield return new WaitForSeconds(letterDelay);
        }

        if (hideAfterFinish)
        {
            yield return new WaitForSeconds(hideDelay);

            textBox.gameObject.SetActive(false);
        }
    }
}