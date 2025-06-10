using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject mascot;
    public Image mascotImage;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 12f;
    public float colorLerpDuration = 0.5f;
    public float jumpHeight = 32f;
    public float jumpDuration = 0.4f;
    public RectTransform clipBoard;
    public RectTransform scoresPanel;
    RectTransform rectTransform;
    Vector2 originalAnchoredPos;
    Color originalColor;

    public Text dialogueText;
    public Text typingDotsText;
    public CanvasGroup dialogueBox;

    public float typeDelay = 0.03f;
    
    private IEnumerator AnimateTypingDots()
    {
        string[] dotStates = { "", ".", "..", "..." };
        string[] doneStates = {"", "..."};
        int index = 0;

        while (typingDotsText && isTyping) 
        {
            typingDotsText.text = dotStates[index];
            index = (index + 1) % dotStates.Length;
            yield return new WaitForSeconds(0.4f);
        }
        while (typingDotsText && !isTyping)
        {
            typingDotsText.text = doneStates[index];
            index = (index + 1) % doneStates.Length;
            yield return new WaitForSeconds(0.4f);
        }
        //typingDotsText.text = ""; // clear after done typing
    }

     private string[] dashBoardLines = new string[]
    {
        "Here is your first *Brand Risk Evaluation & Mitigation Scenario!* ✨",
        "You'll be presented with sitations that require corporate finesse",
        "Each choice will influence the forces tugging at our little green empire.",
        "Here’s what you’ll be juggling",
        "🧑🌾 Farmers & Producers – Our upstream partners. Without them, there’s no “organic supply chain” to overstate.",
        "🏛️ Government & Regulations – audits and laws are annoying... unless they are by our side.",
        "🗣️ Public Perception — the internet loves a villain. Don’t be it.",
        "✊ Activist Pressure They chant. They dig. They tweet.",
        "💰 Money – Every campaign, every pivot, every apology… costs.",
        "Let any of them drop too low, and you’ll be dragged to the Boardroom of Accountability™ — and nobody comes back from that.",
        "Alright, rookie.",
        "Make us proud"
    };

    IEnumerator PlayDialogueLines(string[] Lines)
    {
        foreach (string line in Lines)
        {
            dialogueText.text = "";
            isTyping = true;
            skipRequested = false;

            // Start the typewriter effect
            AudioManager.instance.Play("Typing");
            StartCoroutine(AnimateTypingDots());
            yield return StartCoroutine(TypeLine(line));

            isTyping = false;
            AudioManager.instance.Stop("Typing");
            // Wait for spacebar or click to move to the next line
            yield return new WaitUntil(() => dialogueText.text == line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        }
        typingDotsText.gameObject.SetActive(false);

    }
    bool isTyping = false;
    bool skipRequested = false;

    IEnumerator TypeLine(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            //if (skipRequested)
            //{
            //    dialogueText.text = line;
            //    skipRequested = false;
            //    yield break;
            //}

            dialogueText.text += line[i];
            // If the user tries to skip mid-animation
            yield return new WaitForSeconds(typeDelay);
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
               // skipRequested = true;
                dialogueText.text = line;
                yield break;
            }
                     
        }
    }

    public void TriggerHappyReaction()
    {
        
        StopAllCoroutines();
        rectTransform = mascot.GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;
        originalColor = mascotImage.color;
        StartCoroutine(HappyJump());
        StartCoroutine(FlyInImage(clipBoard, new Vector3(0, -538, 0), 1.0f));
        StartCoroutine(FlyInImage(scoresPanel, new Vector3(0, 0, 0), 1.0f));
       // yield return new WaitForSeconds(0.5f);
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

    public IEnumerator FlyInImage(RectTransform image, Vector3 targetPosition, float duration)
    {
    image.gameObject.SetActive(true);

    // Start offscreen or from above
    //Vector3 startPosition = targetPosition + new Vector3(0f, -700f, 0f); 
    Vector3 startPosition = image.anchoredPosition;
    //image.anchoredPosition = startPosition;



    float elapsed = 0f;

    while (elapsed < duration)
    {
        float t = elapsed / duration;

        float smoothT = Mathf.SmoothStep(0, 1, t);

        image.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);
        float easedT = EaseOutBack(t, 1.0f); // same settle style

        image.anchoredPosition = Vector3.LerpUnclamped(startPosition, targetPosition, easedT);

        elapsed += Time.deltaTime;
        yield return null;
    }

    image.anchoredPosition = targetPosition;
    }

    float EaseOutBack(float t, float s = 1.5f)
    {
        t -= 1;
        return (t * t * ((s + 1) * t + s) + 1);
    }
}
