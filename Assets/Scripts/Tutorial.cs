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
        string[] dotStates = { ".", "..", "..." };
        string[] doneStates = { "", "..." };
        int index = 0;

        while (typingDotsText && isTyping)
        {
            if (index > 2)
                index = 0;
            typingDotsText.text = dotStates[index];
            //index = (index + 1) % dotStates.Length;
            yield return new WaitForSeconds(0.4f);
        }
        while (typingDotsText && !isTyping)
        {   
            if (index > 1)
                index = 0;
            typingDotsText.text = doneStates[index];
            yield return new WaitForSeconds(0.4f);
        }
        //typingDotsText.text = ""; // clear after done typing
    }

    private string[] dashBoardLines = new string[]
   {
        "Here is your first ~Brand Risk Evaluation & Mitigation Scenario!~",
        "You'll be presented with situations and decisions that require corporate finesse",
        "Each choice will influence the forces tugging at our little green empire.",
   };
    IEnumerator PlayDialogueLines(string[] Lines)
    {
        foreach (string line in Lines)
        {
            dialogueText.text = "";
            //skipRequested = false;
            isTyping = true;
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
        //typingDotsText.gameObject.SetActive(false);

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
                isTyping = false;
                yield break;
            }

        }
    }

    public GameObject[] tutorialObjects;

    // To do maybe add glow effect
    public IEnumerator PlayAllDialogues()
    {
        yield return StartCoroutine(PlayDialogueLines(dashBoardLines));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Here’s what you’ll be juggling"
        }));
        tutorialObjects[1].SetActive(true);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[1].GetComponent<RectTransform>(), new Vector3(-10.96f, 4.09f, 0f), 1.0f, 360f));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Farmers & Producers: Our upstream partners. Without them, there’s no “organic supply chain” to overstate."
        }));
        //tutorialObjects[1].SetActive(false);
        //tutorialObjects[2].SetActive(true);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[1].GetComponent<RectTransform>(), new Vector3(-7.53f, 4.09f, 0f), 1.0f));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Activist Pressure: If they get too angry we might need a donation campaign."
        }));
        // tutorialObjects[2].SetActive(false);
        //tutorialObjects[3].SetActive(true);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[1].GetComponent<RectTransform>(), new Vector3(-3.5f, 4.09f, 0f), 1.0f));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Public Perception: The image our brand holds in the people's eye."
        }));
        //tutorialObjects[3].SetActive(false);
        //tutorialObjects[4].SetActive(true);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[1].GetComponent<RectTransform>(), new Vector3(0.98f, 4.09f, 0f), 1.0f));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Stakeholder Confidence: The people in suits trust you to keep stock prices high. Don't let them down."
        }));
        //tutorialObjects[4].SetActive(false);
        //tutorialObjects[5].SetActive(true);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[1].GetComponent<RectTransform>(), new Vector3(4.94f, 4.09f, 0f), 1.0f));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Government & Regulations: Audits and laws are annoying... unless they are by our side."
        }));
        // tutorialObjects[5].SetActive(false);
        // tutorialObjects[6].SetActive(true);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[1].GetComponent<RectTransform>(), new Vector3(10.07f, 4.09f, 0f), 1.0f));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Money: Every campaign, every pivot, every apology… costs."
        }));
        //tutorialObjects[6].SetActive(false);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[1].GetComponent<RectTransform>(), new Vector3(15.07f, 4.09f, 0f), 1.0f, 360f));
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Keep all the sliders in the green, or you’ll be dragged to the Boardroom of Accountability™ and nobody comes back from that."
        }));
        tutorialObjects[0].SetActive(false);
        yield return StartCoroutine(PlayDialogueLines(new string[] {
            "Alright, rookie. Make us proud!"
        }));
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(tutorialObjects[7].GetComponent<RectTransform>(), new Vector3(-4f, -126f, 0f), 1.0f));
    }
    public void TriggerHappyReaction()
    {

        StopAllCoroutines();
        rectTransform = mascot.GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;
        originalColor = mascotImage.color;
        StartCoroutine(HappyJump());
        typingDotsText.gameObject.SetActive(true);
        StartCoroutine(FlyInImage(clipBoard, new Vector3(0, -538, 0), 1.0f));
        StartCoroutine(FlyInImage(scoresPanel, new Vector3(0, 0, 0), 1.0f));
        StartCoroutine(PlayAllDialogues());
        // yield return new WaitForSeconds(0.5f);
        //StartCoroutine(PlayDialogueLines(dashBoardLines));
        //StartCoroutine(PlayDialogueLines(dashBoardLines));
        //StartCoroutine(PlayDialogueLines(dashBoardLines));
    }

    public void TriggerAngryReaction()
    {
        StopAllCoroutines();
        rectTransform = mascot.GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;
        originalColor = mascotImage.color;
        StartCoroutine(AngryShakeAndColor());
    }

    public void TriggerTest(int selection)
    {
        StartCoroutine(EndTutorial(selection));
    }
    IEnumerator EndTutorial(int selection)
    {
        dialogueText.text = "";
        if (selection == 0)
        {
            StartCoroutine(HappyJump());
            yield return StartCoroutine(PlayDialogueLines(new string[] {
                "There you go, wasn't that easy?"
            }));
        }
        else if (selection == 1)
        {
            StartCoroutine(HappyJump());
            yield return StartCoroutine(PlayDialogueLines(new string[] {
                "Hey, don't fix what isn't broken right?"
            }));
        }
        else
        {
            yield return StartCoroutine(PlayDialogueLines(new string[] {
                "Hey, don't fix what isn't broken right?"
            }));
        }
        yield return StartCoroutine(PlayDialogueLines(new string[] {
                "Keep it up and don't make us go bankrupt. I'll be back!"
            }));
        typingDotsText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        dialogueBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        AudioManager.instance.Play("Air");
        StartCoroutine(FlyInImage(mascot.GetComponent<RectTransform>(), new Vector3(1200, 40, 0), 2.0f, 1120f));
       // StartCoroutine(FlyInImage(dialogueBox.GetComponent<RectTransform>(), new Vector3(1500, -125, 0), 1.0f, 360f));
        // To do change slider values smoothly
        // To do play new music
        // To do change backgoound color smoothly
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


    public IEnumerator FlyInImage(RectTransform image, Vector3 targetPosition, float duration, float startRotation)
    {

        // Start offscreen or from above
        //Vector3 startPosition = targetPosition + new Vector3(1160f, 771f, 0f); // flying in from top-right
        Vector3 startPosition = image.anchoredPosition;
        // image.anchoredPosition = startPosition;

        //float startRotation = 720f; // 2 full spins
        //float startRotation = 360f;
        float endRotation = 0f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float smoothT = Mathf.SmoothStep(0, 1, t);

            //image.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);
            float easedT = EaseOutBack(t, 1.5f); // same settle style

            //mascot.anchoredPosition = Vector3.LerpUnclamped(startPosition, targetPosition, easedT);
            image.anchoredPosition = Vector3.LerpUnclamped(startPosition, targetPosition, easedT);
            image.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, endRotation, smoothT));
            elapsed += Time.deltaTime;
            yield return null;
        }

        image.anchoredPosition = targetPosition;
        image.localRotation = Quaternion.Euler(0, 0, endRotation);
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
    /***
    public IEnumerator SmoothChangeSlider(float target, float time)
    {
        float startValue = slider.value;
        float timeElapsed = 0f;

        while (timeElapsed < time)
        {
            slider.value = Mathf.Lerp(startValue, target, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        slider.value = target;
    }
    ***/
}
