using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogPopupScript : MonoBehaviour
{
    // 1. Singleton implementation
    public static LogPopupScript Instance;

    [Header("UI References")]
    public CanvasGroup dialogueCanvasGroup; // Use a CanvasGroup for easy fading
    public TMP_Text dialogueText;           // Reference to a TextMeshPro text component

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;       // Duration of fade in/out in seconds

    private void Awake()
    {
        // Ensure only one instance of this manager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Optionally ensure the dialogue is hidden at the start
        dialogueCanvasGroup.alpha = 0f;
    }

    // 2. Public function to show and fade in text, wait, then fade out
    public void ShowPopup(string text, float displayTime)
    {
        // Stop any ongoing fade routines before starting a new one
        StopAllCoroutines();

        // Start the coroutine that handles the full cycle
        StartCoroutine(DialogueRoutine(text, displayTime));
    }

    // 3. Coroutine that performs the fade in, wait, and fade out sequence
    private IEnumerator DialogueRoutine(string text, float displayTime)
    {
        // Update the text to display
        dialogueText.text = text;

        // Fade In
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alphaValue = Mathf.Clamp01(timer / fadeDuration);
            dialogueCanvasGroup.alpha = alphaValue;
            yield return null;
        }

        // Wait (display text) for the specified duration
        yield return new WaitForSeconds(displayTime);

        // Fade Out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alphaValue = 1f - Mathf.Clamp01(timer / fadeDuration);
            dialogueCanvasGroup.alpha = alphaValue;
            yield return null;
        }

        // Make sure alpha is set to 0 after fading out
        dialogueCanvasGroup.alpha = 0f;
    }
}
