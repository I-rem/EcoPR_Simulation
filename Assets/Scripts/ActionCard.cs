using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionCard : MonoBehaviour
{
    public string cardTitle;
    public string description;
    public int ppChange, apChange, scChange;

    public TMP_Text titleText;
    public TMP_Text descText;

    public void Setup(string title, string desc, int pp, int ap, int sc)
    {
        cardTitle = title;
        description = desc;
        ppChange = pp;
        apChange = ap;
        scChange = sc;
        titleText.text = title;
        descText.text = desc;
    }

    public void PlayCard()
    {
        GameManager.Instance.ApplyScoreChanges(ppChange, apChange, scChange);
        Destroy(gameObject); // Optional: remove after playing
    }
}
