using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewScenario", menuName = "PRGame/Scenario Card")]
public class ScenarioCard : ScriptableObject
{
    public string scenarioTitle;
    public Sprite icon;
    [TextArea(2, 5)]
    public string scenarioText;
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public int publicPerception;
    public int activistPressure;
    public int stakeholderSupport;
    public int farmersProducers;
    public int governmentRelations;
    public int money;
    public ScenarioCard followUpScenario; // for chaining
}
