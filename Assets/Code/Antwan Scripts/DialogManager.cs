using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;

    private Queue<string> dialogQueue = new Queue<string>();
    private bool isTyping = false;
    private Coroutine currentTypingCoroutine;

    public GameObject dialogBox; // Reference to the dialog UI box

    void Start()
    {
            dialogBox.SetActive(false); // Ensure the dialog box is hidden initially
    }

    public void DisplayDialog(string message)
    {
        dialogQueue.Enqueue(message);

        if (!isTyping)
        {
            StartCoroutine(ProcessDialogQueue());
        }
        else
        {
            // Skip to the latest dialog
            SkipToNewDialog(message);
        }
    }

    private IEnumerator ProcessDialogQueue()
    {
        while (dialogQueue.Count > 0)
        {
            string message = dialogQueue.Dequeue();
            currentTypingCoroutine = StartCoroutine(TypeText(message));
            yield return currentTypingCoroutine;
        }

        isTyping = false;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;

        if (dialogBox != null && !dialogBox.activeSelf)
        {
            dialogBox.SetActive(true);
        }

        dialogText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(2f); // Wait before allowing the next message
    }

    public void SkipToNewDialog(string newMessage)
    {
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine); // Stop the ongoing typing coroutine
        }

        dialogText.text = newMessage; // Immediately display the new message
        dialogQueue.Clear();         // Clear the queue to avoid outdated messages
    }

    public void HideDialog()
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
        }

        dialogText.text = "";
        dialogQueue.Clear(); // Clear messages when hiding
        isTyping = false;
    }
}
