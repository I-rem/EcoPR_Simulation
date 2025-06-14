using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scenarios : MonoBehaviour
{
    
    public ScenarioCard[] cards;
    public GameObject clipboardPrefab;
    public RectTransform clipboardRect;
    public GameObject TitleText;
    public GameObject DescriptionText;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;

    private ScenarioCard currentCard;

    public Slider publicSlider;
    public Slider activistSlider;
    public Slider stakeholderSlider;
    public Slider farmersSlider;
    public Slider govSlider;
    public Slider moneySlider;

    public int publicPerception;
    public int activistPressure;
    public int stakeholderSupport;
    public int farmersProducers;
    public int governmentRelations;
    public int money;

    public IEnumerator PlayCard(int index)
    {
        if (index < 0 || index >= cards.Length)
            yield break;

        currentCard = cards[index];
       // GameObject oldClipboard = Instantiate(clipboardPrefab, this.gameObject.transform.position, Quaternion.identity);
       Destroy(GameObject.FindGameObjectWithTag("Remove"));
        GameObject oldClipboard = Instantiate(GameObject.FindGameObjectWithTag("Copy"), transform);
        TitleText.GetComponent<Text>().text = currentCard.scenarioTitle;
        DescriptionText.GetComponent<Text>().text = currentCard.scenarioText;

        SetupChoiceButton(Button1, currentCard.choices[0]);
        SetupChoiceButton(Button2, currentCard.choices[1]);
        SetupChoiceButton(Button3, currentCard.choices[2]);

        AudioManager.instance.Play("Paper");
        yield return StartCoroutine(FlyOutImage(oldClipboard.GetComponent<RectTransform>(), new Vector3(0, -700, 0), 1.5f));
        Destroy(oldClipboard);

    }

    void SetupChoiceButton(GameObject buttonObj, Choice choice)
    {
        
        var btn = buttonObj.GetComponent<Button>();
        var txt = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        btn.interactable = true;
        txt.text = choice.choiceText;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            ApplyChoice(choice);
        });
        buttonObj.SetActive(true);
    }

    void ApplyChoice(Choice choice)
    {
        UpdateScores(choice);

        if (choice.followUpScenario != null)
        {
            PlayScenarioCard(choice.followUpScenario);
        }
        else
        {
            int nextIndex = Random.Range(0, cards.Length);
            StartCoroutine(PlayCard(nextIndex));
        }
    }

    void PlayScenarioCard(ScenarioCard scenario)
    {
        // can just do same stuff as PlayCard() with this one scenario
        currentCard = scenario;

        //AudioManager.instance.Play("Paper");


        TitleText.GetComponent<Text>().text = currentCard.scenarioTitle;
        DescriptionText.GetComponent<Text>().text = currentCard.scenarioText;

        SetupChoiceButton(Button1, currentCard.choices[0]);
        SetupChoiceButton(Button2, currentCard.choices[1]);
        SetupChoiceButton(Button3, currentCard.choices[2]);
    }

    public void PlayScenario(int index)
    {
        StartCoroutine(PlayCard(index));
    }
    public IEnumerator FlyOutImage(RectTransform image, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = image.anchoredPosition;
        // image.anchoredPosition = startPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float smoothT = Mathf.SmoothStep(0, 1, t);

            float easedT = EaseOutBack(t, 1.5f); // same settle style

            image.anchoredPosition = Vector3.LerpUnclamped(startPosition, targetPosition, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        image.anchoredPosition = targetPosition;
        //Destroy(image.gameObject);
    }
    
    float EaseOutBack(float t, float s = 1.5f)
    {
        t -= 1;
        return (t * t * ((s + 1) * t + s) + 1);
    }

    public IEnumerator SmoothChangeSlider(Slider slider, float target, float time)
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

    public void UpdateScores(Choice choice)
    {
        StartCoroutine(SmoothChangeSlider(publicSlider, publicPerception + choice.publicPerception, 1.0f));
        StartCoroutine(SmoothChangeSlider(activistSlider, activistPressure + choice.activistPressure, 1.0f));
        StartCoroutine(SmoothChangeSlider(stakeholderSlider, stakeholderSupport + choice.stakeholderSupport, 1.0f));
        StartCoroutine(SmoothChangeSlider(farmersSlider, farmersProducers + choice.farmersProducers, 1.0f));
        StartCoroutine(SmoothChangeSlider(govSlider, governmentRelations + choice.governmentRelations, 1.0f));
        StartCoroutine(SmoothChangeSlider(moneySlider, money + choice.money, 1.0f));

        publicPerception += choice.publicPerception;
        activistPressure += choice.activistPressure;
        stakeholderSupport += choice.stakeholderSupport;
        farmersProducers += choice.farmersProducers;
        governmentRelations += choice.governmentRelations;
        money += choice.money;
    }

}
