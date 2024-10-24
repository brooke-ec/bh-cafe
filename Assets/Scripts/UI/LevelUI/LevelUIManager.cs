using DG.Tweening;
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
        transform.gameObject.SetActive(true);
        AddUpgradeBonuses();
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

    private void AddUpgradeBonuses()
    {
        ShopItemSettings shopItemSettings = Resources.Load<ShopItemSettings>("ShopSettings");
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        foreach(ShopItemInfo shopItem in SavingSystem.instance.saveData.shopUpgradesBought)
        {
            //switch(shopItem.)
            switch(shopItem.shopItemType)
            {
                case ShopItemType.Time:
                    //lvlSettings.lengthOfLevelMins += shopItemSettings.shopItems.Find(item => item.modifier)
                    break;
                case ShopItemType.Hearts:
                    break;
                case ShopItemType.Diamonds:
                    break;
                case ShopItemType.DashSpeed:
                    break;
                default:
                    break;
            }
        }
    }

    #region Testing
    void Start()
    {
        //UI WITH DESCRIPTION OF LEVEL, AND A BUTTON TO START
        startOfLevel = Instantiate(startOfLevelUIPrefab, transform.parent);
        startOfLevel.transform.DOScale(transform.localScale * 1.1f, 0.3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
        startOfLevel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LEVEL " + lvlSettings.levelNum;
        startOfLevel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = lvlSettings.levelDescription;
        startOfLevel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(()=>StartLevel());

        transform.gameObject.SetActive(false);
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
