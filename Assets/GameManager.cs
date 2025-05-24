using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int publicPerception = 50;
    public int activistPressure = 50;
    public int shareholderConfidence = 50;

    public Slider ppSlider, apSlider, scSlider;

    void Update()
    {
        ppSlider.value = publicPerception;
        apSlider.value = activistPressure;
        scSlider.value = shareholderConfidence;

        CheckLoseCondition();
    }

    void CheckLoseCondition()
    {
        if (publicPerception <= 0 || activistPressure <= 0 || shareholderConfidence <= 0)
        {
            Debug.Log("Game Over You couldn't offset the truth...");
            // TODO: Trigger Game Over screen
        }
    }
}
