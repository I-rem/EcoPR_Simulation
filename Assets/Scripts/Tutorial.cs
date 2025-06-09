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

     private string[] dashBoardLines = new string[]
    {
        "Here is your first *Brand Risk Evaluation & Mitigation Scenario!* ✨",
        "I'm Verdy, your loyal sustainability ambassador.",
        "We're here to help you *look* green, *feel* green... without necessarily *being* green.",
        "Each choice will influence the forces tugging at our little green empire."
    };
/**
Welcome to your first *Brand Risk Evaluation & Mitigation Scenario!* ✨

Each day, you’ll be presented with a real-world situation that requires corporate finesse...  
You’ll get multiple response options. Each will impact the world around you.

Take a deep breath — it’s time to juggle **the five forces of greenwashing**:

🧑🌾 Farmers & Producers — keep them on your side. No farmers, no future.
🏛️ Government — audits and laws are annoying... but unavoidable.
💰 Money — no campaign runs on air. Spend wisely.
🗣️ Public Perception — the internet loves a villain. Don’t be it.
✊ Activist Pressure — they’re watching. Closely.

Every decision you make shifts these balances. Some may help one side… and hurt another.

🎯 Your goal? Keep them all *just satisfied enough* to keep operations smooth.

Got it? Okay. Let’s start with your first dilemma.

Here’s what you’ll be juggling:
🧑🌾 Farmers & Producers – Our upstream partners. Without them, there’s no “organic supply chain” to overstate.
🏛️ Government & Regulations – Bureaucrats with clipboards and the power to ruin a fiscal quarter.
💰 Money – Every campaign, every pivot, every apology… costs.
🗣️ Public Perception – Social media loves authenticity, and we’re very good at faking it.
✊ Activist Pressure – They chant. They dig. They tweet. Keep them just distracted enough.
________________________________________
[Mascot leans in, faux-conspiratorially.]
VERDY:
Balance is key. Make one group too happy, and another might come asking questions.
Let any of them drop too low, and you’ll be dragged to the Boardroom of Accountability™ — and nobody comes back from that.
________________________________________
[Mascot walks toward the clipboard, tapping it.]
VERDY:
Oh, and here's a trade secret: sometimes, one decision opens up another.
Say, agree to a tiny harmless label tweak today… and maybe that makes regulators more likely to approve your next green initiative.
Ever heard of foot-in-the-door? Classic manipulation tactic. We’ve trademarked it as “Stepping Stone Advocacy™.”
________________________________________
[Mascot gives a little wink. Event card animates in — your first task.]
VERDY:
Alright, rookie. Time to play with power.
Here’s your first situation — make it count. And remember:
If it sounds ethical... you’re probably not trying hard enough.
Let the game begin.

**/
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
        yesNoButtons.gameObject.SetActive(true);
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
        yield return new WaitForSeconds(0.5f);
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
