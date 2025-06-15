using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorIntroManager : MonoBehaviour
{
    public RectTransform leftDoor;
    public RectTransform rightDoor;
    public RectTransform textBubble;

    public float moveDistance = 400f; // Distance to slide doors
    public float moveDuration = 1f;   // Seconds it takes to open
    public AudioSource dingSound;

    public float typeDelay = 0.03f;
    public RectTransform mascot;
    public CanvasGroup dialogueBox;
    public Text dialogueText;
    public GameObject yesNoButtons;
    public Text typingDotsText;
    //public TextMeshProUGUI typingDotsText;

    private string[] introLines = new string[]
   {
        "Ding! Welcome to the Headquarters.",
        "You must be the new... Perception Architect? Brand Strategist? Whatever...",
        "Here at Gaia Inc. we are like a family",
        "And I'm your loyal sustainability ambassador Cornelius Jr.",
        "But you can call me CJ.",
        "I'm here to help you look green, feel green and earn us that green.",
        "Ready to save the planet... one PR campaign at a time?"
   };

    void Start()
    {
        StartCoroutine(PlayIntroSequence());


    }

    IEnumerator PlayIntroSequence()
    {
        yield return new WaitForSeconds(0.5f);
        dingSound.Play();

        yield return new WaitForSeconds(0.5f); // Pause after ding
        yield return StartCoroutine(OpenDoors());

    }

    IEnumerator OpenDoors()
    {
        Vector2 leftStart = leftDoor.anchoredPosition;
        Vector2 rightStart = rightDoor.anchoredPosition;

        Vector2 leftTarget = leftStart + Vector2.left * moveDistance;
        Vector2 rightTarget = rightStart + Vector2.right * moveDistance;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            leftDoor.anchoredPosition = Vector2.Lerp(leftStart, leftTarget, t);
            rightDoor.anchoredPosition = Vector2.Lerp(rightStart, rightTarget, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        leftDoor.anchoredPosition = leftTarget;
        rightDoor.anchoredPosition = rightTarget;
        Destroy(leftDoor.gameObject);
        Destroy(rightDoor.gameObject);
        AudioManager.instance.Stop("Ambiance");
        AudioManager.instance.Play("CorporateIntro");
        StartCoroutine(FlyInImage(mascot, new Vector3(-490, -68, 0), 1.2f));
        //StartCoroutine(FlyInTextBubble(textBubble, new Vector3(0f, 0f, 0f), 0.8f));
        StartCoroutine(FlyInTextBubble(textBubble, new Vector3(-140f, 200f, 0f), 0.8f)); // Leaving space for the clipboard
        StartCoroutine(PlayDialogueLines());
    }

    IEnumerator PlayDialogueLines()
    {
        foreach (string line in introLines)
        {
            dialogueText.text = "";
            isTyping = true;
            //skipRequested = false;

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
    //bool skipRequested = false;

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

    public IEnumerator FlyInImage(RectTransform image, Vector3 targetPosition, float duration)
    {
        image.gameObject.SetActive(true);

        // Start offscreen or from above
        Vector3 startPosition = targetPosition + new Vector3(1160f, 771f, 0f); // flying in from top-right
        image.anchoredPosition = startPosition;

        //float startRotation = 720f; // 2 full spins
        float startRotation = 360f;
        float endRotation = 0f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float smoothT = Mathf.SmoothStep(0, 1, t);

            //image.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);
            float easedT = EaseOutBack(t, 1.5f); // same settle style

            mascot.anchoredPosition = Vector3.LerpUnclamped(startPosition, targetPosition, easedT);

            image.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, endRotation, smoothT));
            elapsed += Time.deltaTime;
            yield return null;
        }

        image.anchoredPosition = targetPosition;
        image.localRotation = Quaternion.Euler(0, 0, endRotation);
    }

    public IEnumerator FlyInTextBubble(RectTransform bubble, Vector3 targetPosition, float duration)
    {
        bubble.gameObject.SetActive(true);
        dialogueBox.alpha = 0f;

        // Start position: off to the right
        Vector3 startPosition = targetPosition + new Vector3(1000f, 0f, 0f);
        bubble.anchoredPosition = startPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            dialogueBox.alpha = smoothT;
            // bubble.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);
            float easedT = EaseOutBack(t);

            bubble.anchoredPosition = Vector3.LerpUnclamped(startPosition, targetPosition, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bubble.anchoredPosition = targetPosition;
    }

    float EaseOutBack(float t, float s = 1.5f)
    {
        t -= 1;
        return (t * t * ((s + 1) * t + s) + 1);
    }
    /**
    private IEnumerator AnimateTypingDots()
    {
        string[] dotStates = { ".", "..", "..." };
        string[] doneStates = {"", "..."};
        int index = 0;

        while (typingDotsText && isTyping) // this flag is already used in your script
        {
            typingDotsText.text = dotStates[index];
            index += 1;
            if (index == 3)
                index = 0;
            yield return new WaitForSeconds(0.4f);
        }
        while (typingDotsText && !isTyping)
        {
            if (index > 1)
                index = 0;
            typingDotsText.text = doneStates[index];
            index += 1;
            yield return new WaitForSeconds(0.4f);
        }
        //typingDotsText.text = ""; // clear after done typing
    }
    **/
    private IEnumerator AnimateTypingDots()
    {
        string[] dotStates = { ".", "..", "..." };
        string[] doneStates = { "", "..." };
        int index = 0;

        while (typingDotsText && isTyping)
        {
            // if (index > 2)
            //     index = 0;
            // typingDotsText.text = dotStates[index];
            if (typingDotsText.text == "")
                typingDotsText.text = ".";
            else if (typingDotsText.text == ".")
                typingDotsText.text = "..";
            else if (typingDotsText.text == "..")
                typingDotsText.text = "...";
            else if (typingDotsText.text == "...")
                typingDotsText.text = ".";
            //index = (index + 1) % dotStates.Length;
            yield return new WaitForSeconds(0.4f);
        }
        while (typingDotsText && !isTyping)
        {
            //if (index > 1)
            //    index = 0;
            //typingDotsText.text = doneStates[index];
            if (typingDotsText.text == "")
                typingDotsText.text = "...";
            else if (typingDotsText.text == ".")
                typingDotsText.text = "..";
            else if (typingDotsText.text == "..")
                typingDotsText.text = "...";
            else if (typingDotsText.text == "...")
                typingDotsText.text = "";
            yield return new WaitForSeconds(0.4f);
        }
        //typingDotsText.text = ""; // clear after done typing
    }
}
