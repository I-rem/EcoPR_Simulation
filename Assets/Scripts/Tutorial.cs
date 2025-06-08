using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject mascot;
    public Image mascotImage;
    public float shakeDuration = 0.4f;
    public float shakeMagnitude = 10f;
    public float colorLerpDuration = 0.5f;
    public float jumpHeight = 30f;
    public float jumpDuration = 0.3f;

    RectTransform rectTransform;
    Vector2 originalAnchoredPos;
    Color originalColor;

    public void TriggerHappyReaction()
    {
        
        StopAllCoroutines();
        rectTransform = mascot.GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;
        originalColor = mascotImage.color;
        StartCoroutine(HappyJump());
    }

    public void TriggerAngryReaction()
    {
        StopAllCoroutines();
        rectTransform = mascot.GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;
        originalColor = mascotImage.color;
        StartCoroutine(AngryShakeAndColor());
    }

    IEnumerator AngryShakeAndColor()
    {
        StartCoroutine(ChangeColorGradually(originalColor, Color.red, colorLerpDuration));

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xOffset = Mathf.Sin(elapsed * 50f) * shakeMagnitude;
            rectTransform.anchoredPosition = originalAnchoredPos + new Vector2(xOffset, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalAnchoredPos;

        StartCoroutine(ChangeColorGradually(Color.red, originalColor, colorLerpDuration));
    }

    IEnumerator HappyJump()
    {
        Vector2 start = originalAnchoredPos;
        Vector2 peak = originalAnchoredPos + new Vector2(0, jumpHeight);

        StartCoroutine(ChangeColorGradually(originalColor, Color.green, colorLerpDuration));
        float halfDuration = jumpDuration / 2f;
        float t = 0f;

        // Jump up
        while (t < halfDuration)
        {
            float progress = t / halfDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(start, peak, Mathf.SmoothStep(0, 1, progress));
            t += Time.deltaTime;
            yield return null;
        }

        // Jump down
        t = 0f;
        while (t < halfDuration)
        {
            float progress = t / halfDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(peak, start, Mathf.SmoothStep(0, 1, progress));
            t += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalAnchoredPos;
        StartCoroutine(ChangeColorGradually(Color.green, originalColor, colorLerpDuration));
    }

    IEnumerator ChangeColorGradually(Color fromColor, Color toColor, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            mascotImage.color = Color.Lerp(fromColor, toColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        mascotImage.color = toColor;
    }

}
