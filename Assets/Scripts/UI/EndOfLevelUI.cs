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
    }

    private void SetUpContent()
    {
        //SCORES AREA
        int currentScore = God.instance.levelUIManager.GetScore();
        int highscoreIndex = SavingSystem.instance.saveData.levelNums.FindIndex(num => num == lvlSettings.levelNum);
        if (highscoreIndex >= 0)
        {
            int previousHigh = SavingSystem.instance.saveData.highscoreLevel[highscoreIndex];
            //NEW HIGHSCORE SET
            if (currentScore > previousHigh)
            {

            }
            else
            {

            }
        }
        else
        {
            SavingSystem.instance.saveData.levelNums.Add(lvlSettings.levelNum);
            SavingSystem.instance.saveData.highscoreLevel.Add(God.instance.levelUIManager.GetScore());
        }
    }

    private void SetUpTitle()
    {
        levelNumText.text = lvlSettings.levelNum.ToString();
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
