using TMPro;
using UnityEngine;
using System.Collections;
public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;

    public void DisplayDialog(string message)
    {
        StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        dialogText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
