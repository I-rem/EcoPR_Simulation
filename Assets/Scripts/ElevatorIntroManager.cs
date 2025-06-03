using UnityEngine;
using System.Collections;

public class ElevatorIntroManager : MonoBehaviour
{
    public RectTransform leftDoor;
    public RectTransform rightDoor;

    public float moveDistance = 400f; // Distance to slide doors
    public float moveDuration = 1f;   // Seconds it takes to open
    public AudioSource dingSound;

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
        Destroy(leftDoor);
        Destroy(rightDoor);
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
    }
}
