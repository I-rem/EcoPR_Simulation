using UnityEngine;

public class WiggleEffect : MonoBehaviour
{
    public float wiggleAmount = 5f; 
    public float wiggleSpeed = 1f;  

    private RectTransform rectTransform;
    private Vector3 initialPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.localPosition;  
    }

    private void Update()
    {
        float offsetX = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        float offsetY = Mathf.Cos(Time.time * wiggleSpeed) * wiggleAmount;

        rectTransform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);
    }
}
