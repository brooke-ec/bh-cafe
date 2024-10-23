using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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
    public GameObject startOfLevelUIPrefab;
    private GameObject startOfLevel;


    public void StartLevel()
    {
        Destroy(startOfLevel);
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
        //UI WITH DESCRIPTION OF LEVEL, AND A BUTTON TO START
        startOfLevel = Instantiate(startOfLevelUIPrefab, transform);
        startOfLevel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LEVEL " + lvlSettings.levelNum;
        startOfLevel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = lvlSettings.levelDescription;
        startOfLevel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(()=>StartLevel());

        //StartLevel();
        //StartCoroutine(WaitSeconds());
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
