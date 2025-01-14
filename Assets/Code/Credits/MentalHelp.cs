using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MentalHelp : MonoBehaviour
{
    public float delay = 4f; // Delay before fade starts
    public float fadeDuration = 2f; // Duration of the fade
    private Text uiText; // For Unity UI Text

    private void Start()
    {
        // Get the Text component
        uiText = GetComponent<Text>();

        if (uiText == null)
        {
            Debug.LogError("No Text component found! Attach this script to a GameObject with a Text component.");
            return;
        }

        // Start the fade coroutine
        StartCoroutine(FadeInText());
    }

    private IEnumerator FadeInText()
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        Color color = uiText.color; // Corrected this line

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            uiText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure full opacity at the end
        uiText.color = new Color(color.r, color.g, color.b, 1f);
    }
}
