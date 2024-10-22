using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndOfLevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelNumText;
    public TextMeshProUGUI completedText;

    public Color failedLevelColour;
    public Color successLevelColour;

    public Transform contentTransform;

    private LevelSettings lvlSettings;

    void Start()
    {
       lvlSettings = God.instance.levelUIManager.lvlSettings; 
       SetUpTitle();
       SetUpContent();
    }

    private void SetUpContent()
    {
        //SCORES AREA
        int currentScore = God.instance.levelUIManager.GetScore();
        int highscoreIndex = SavingSystem.instance.saveData.levelNums.FindIndex(num => num == lvlSettings.levelNum);
        int previousHigh = 0;
        if (highscoreIndex < 0)
        {
            SavingSystem.instance.saveData.levelNums.Add(lvlSettings.levelNum);
            SavingSystem.instance.saveData.highscoreLevel.Add(currentScore);
            highscoreIndex = SavingSystem.instance.saveData.highscoreLevel.Count - 1;
        }
        else
        {
            previousHigh = SavingSystem.instance.saveData.highscoreLevel[highscoreIndex];
        }
        //NEW HIGHSCORE SET
        contentTransform.GetChild(0).gameObject.SetActive(currentScore > previousHigh);
        contentTransform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Target: " + lvlSettings.scoreNeededForLevel + "\nYour score: " + currentScore;
        
        if(currentScore > previousHigh)
        {
            contentTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Best: " + currentScore.ToString();
        }
        else
        {
            contentTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Best: " + previousHigh.ToString();
        }

        //MISSING SHOP POINTS
    }

    private void SetUpTitle()
    {
        levelNumText.text = "LEVEL "+lvlSettings.levelNum.ToString();
        if (IsLevelComplete())
        {
            completedText.text = "COMPLETED";
            completedText.color = successLevelColour;
        }
        else
        {
            completedText.text = "FAILED";
            completedText.color = failedLevelColour;
        }
    }

    private bool IsLevelComplete()
    {
        return God.instance.levelUIManager.GetScore() >= lvlSettings.scoreNeededForLevel;
    }
}
