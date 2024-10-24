using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public int score = 0;
    public Color colorLevelAchieved;
    public Color colorLevelNotAchieved;

    void Start()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public void ModifyScore(int valueToModify)
    {
        score = Mathf.Clamp(score + valueToModify, 0, score+valueToModify);
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if(score < God.instance.levelUIManager.lvlSettings.scoreNeededForLevel)
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = colorLevelNotAchieved;
        }
        else
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = colorLevelAchieved;
        }
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
