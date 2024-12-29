using System.Collections;
using TMPro;
using UnityEngine;

public class BattelChatBox : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;
    private char letter = '\0'; // Resettable default value

    public void Input(string text)
    {
        Cleaner();
        Transform parentPanel = dialogText.transform.parent;
        if (parentPanel != null && !parentPanel.gameObject.activeSelf)
            parentPanel.gameObject.SetActive(true);

        StartCoroutine(Animatic(text));
    }

    private IEnumerator Animatic(string input)
    {
        dialogText.text = "";

        foreach (char letter in input.ToCharArray())
        {
            this.letter = letter; // Track the current letter being processed
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }

    void Cleaner() // Clears old text [Overlap Remover]
    {
        StopAllCoroutines(); // Stop any ongoing typing animations
        dialogText.text = ""; // Clear the dialog box text
        letter = '\0'; // Reset the letter
    }
}
