using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int publicPerception = 50;
    public int activistPressure = 50;
    public int shareholderConfidence = 50;

    public Slider ppSlider, apSlider, scSlider;
    
    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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

    public void ApplyScoreChanges(int ppChange, int apChange, int scChange)
    {
        publicPerception += ppChange;
        activistPressure += apChange;
        shareholderConfidence += scChange;
    }

}
