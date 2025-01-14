using System.Collections;
using UnityEngine;

public class FadeIn2DObject : MonoBehaviour
{
    public float delay = 6f; // Delay before fade starts
    public float fadeDuration = 2f; // Duration of the fade
    private SpriteRenderer spriteRenderer; // For 2D sprites

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found! Attach this script to a GameObject with a SpriteRenderer.");
            return;
        }

        // Set the initial alpha to 0
        Color color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);

        // Start the fade coroutine
        StartCoroutine(FadeInSprite());
    }

    private IEnumerator FadeInSprite()
    {
        // Wait for the delay
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        // Gradually fade in the sprite
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure full opacity at the end
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
    }
}
