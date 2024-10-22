using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelUIManager : MonoBehaviour
{
    [Header("Level Settings")]
    public LevelSettings lvlSettings; 


    [Header("References")]
    public OrdersContainerUI ordersUI;
    public HeartsUI heartsUI;
    public ScoreUI scoreUI;

    public TimerUI timerUI;

    public GameObject endOfLevelUIprefab;

    public void StartLevel()
    {
        heartsUI.SetInitialHearts(lvlSettings.numOfHearts);
        timerUI.StartLevelCountdown(lvlSettings.lengthOfLevelMins);
        ordersUI.StartLevel();
    }

    public void EndLevel()
    {
        ordersUI.EndLevel();
        print("End of level");
        Instantiate(endOfLevelUIprefab, transform);
        StopAllCoroutines();
    }

    public void AddNewOrder(int totalSeconds, Sprite icon, int tableNum)
    {
        ordersUI.AddNewOrder(totalSeconds, icon, tableNum);
    }

    public void ModifyScore(int valueToModify)
    {
        scoreUI.ModifyScore(valueToModify);
    }

    public int GetScore()
    {
        return scoreUI.score;
    }

    public void LoseHeart()
    {
        heartsUI.LoseHeart();
    }

    #region Testing
    void Start()
    {
        StartLevel();
        StartCoroutine(WaitSeconds());
    }
    IEnumerator WaitSeconds()
    {
        int seconds = 0;
        while(seconds <= 3)
        {
            seconds++;
            yield return new WaitForSecondsRealtime(3);
            ordersUI.AddNewOrder(100, null, 3);
            scoreUI.ModifyScore(50);
            LoseHeart();
        }
    }
    #endregion
}
