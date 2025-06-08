using UnityEngine;
using UnityEngine.UI;

public class SliderColorGradient : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    void Update()
    {
        float value = slider.normalizedValue; // 0 to 1
        // Lerp from red to yellow to green
        Color color;
        if (value < 0.5f)
        {
            color = Color.Lerp(Color.red, Color.yellow, value * 2);
        }
        else
        {
            color = Color.Lerp(Color.yellow, Color.green, (value - 0.5f) * 2);
        }
        fill.color = color;
    }
}
