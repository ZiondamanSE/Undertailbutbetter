using TMPro;
using UnityEngine;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public string text;
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;

    public void DisplayDialog(string message)
    {
        message = text;
        message = null;
        Transform parentPanel = dialogText.transform.parent;

        if (parentPanel != null && !parentPanel.gameObject.activeSelf)
        {
            parentPanel.gameObject.SetActive(true);
        }

        StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        //yield return new WaitForSeconds(10f);
    }
}
