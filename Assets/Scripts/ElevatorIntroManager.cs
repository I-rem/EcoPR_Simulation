using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorIntroManager : MonoBehaviour
{
    public RectTransform leftDoor;
    public RectTransform rightDoor;

    public float moveDistance = 400f; // Distance to slide doors
    public float moveDuration = 1f;   // Seconds it takes to open
    public AudioSource dingSound;

    public float typeDelay = 0.03f;
    public GameObject mascot;
    public CanvasGroup dialogueBox;
    public Text dialogueText;
    public Button continueButton;

     private string[] introLines = new string[]
    {
        "Ding! Welcome to GreenCore Solutions™!",
        "I'm Verdy, your loyal sustainability ambassador.",
        "We're here to help you *look* green, *feel* green... without necessarily *being* green.",
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
        
        // TODO: Start tutorial
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
        Destroy(leftDoor);
        Destroy(rightDoor);
        StartCoroutine(PlayDialogueLines());
    }

    IEnumerator PlayDialogueLines()
    {
    foreach (string line in introLines)
    {
        dialogueText.text = "";
        isTyping = true;
        skipRequested = false;

        // Start the typewriter effect
        yield return StartCoroutine(TypeLine(line));

        isTyping = false;

        // Wait for spacebar or click to move to the next line
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
    }

    continueButton.gameObject.SetActive(true);
    }
    bool isTyping = false;
    bool skipRequested = false;

    IEnumerator TypeLine(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (skipRequested)
            {
                dialogueText.text = line;
                yield break;
            }

            dialogueText.text += line[i];
            yield return new WaitForSeconds(typeDelay);

            // If the user tries to skip mid-animation
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
            }
        }
    }


}
