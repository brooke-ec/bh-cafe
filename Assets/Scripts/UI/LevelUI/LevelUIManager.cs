using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [HideInInspector] public int diamondsEarnedSoFar = 0;

    void Awake()
    {
        lvlSettings = Instantiate(lvlSettings);
    }
    public void StartLevel()
    {
        Destroy(startOfLevel);
        transform.gameObject.SetActive(true);
        AddUpgradeBonuses();
        heartsUI.SetInitialHearts(lvlSettings.numOfHearts);
        timerUI.StartLevelCountdown(lvlSettings.lengthOfLevelMins);
        ordersUI.StartLevel();
        CustomerSpawner.SetActive(true);
        if (God.instance.playerController != null) God.instance.playerController.SetActive(true);
    }

    public void EndLevel()
    {
        ordersUI.EndLevel();
        print("End of level");
        Instantiate(endOfLevelUIprefab, transform);
        StopAllCoroutines();
        CustomerSpawner.SetActive(false);
        if (God.instance.playerController != null) God.instance.playerController.SetActive(false);
    }

    public void AddNewOrder(int totalSeconds, Sprite icon, int tableNum)
    {
        ordersUI.AddNewOrder(totalSeconds, icon, tableNum);
    }

    public void RemoveOrder(int tableNum)
    {
        ordersUI.RemoveOrder(tableNum);
    }

    public void ModifyScore(int valueToModify)
    {
        scoreUI.ModifyScore(valueToModify);
    }

    public int GetScore()
    {
        return scoreUI.score;
    }

    public void GainDiamond(int amount)
    {
        diamondsEarnedSoFar += amount;
    }

    public void LoseHeart()
    {
        heartsUI.LoseHeart();
    }
    public void GainHeart()
    {
        heartsUI.GainHeart();
    }
    public void AddTime(float time)
    {
        timerUI.AddTime(time);
    }

    private void AddUpgradeBonuses()
    {
        ShopItemSettings shopItemSettings = Resources.Load<ShopItemSettings>("ShopSettings");
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        foreach(ShopItemInfo upgradeBought in SavingSystem.instance.saveData.shopUpgradesBought)
        {
            ShopItem shopItemRef = shopItemSettings.shopItems.Find(item => (item.info.shopItemType == upgradeBought.shopItemType) && (item.info.shopItemTypeNum == upgradeBought.shopItemTypeNum));
            if(shopItemRef != null)
            {
                switch(upgradeBought.shopItemType)
                {
                    case ShopItemType.Time:
                        lvlSettings.lengthOfLevelMins += shopItemRef.modifier;
                        break;
                    case ShopItemType.Hearts:
                        lvlSettings.numOfHearts += shopItemRef.modifier;
                        break;
                    case ShopItemType.Diamonds:
                        lvlSettings.extraDiamondPercentage += shopItemRef.modifier;
                        break;
                    case ShopItemType.Speed:
                        playerController.moveSpeed += shopItemRef.modifier;
                        break;
                    default:
                        break;
                }
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
        if (God.instance.playerController != null) God.instance.playerController.SetActive(false);
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
    IEnumerator WaitSeconds1()
    {
        int seconds = 0;
        while (seconds <= 3)
        {
            seconds++;
            yield return new WaitForSecondsRealtime(5);
            GainHeart();
        }
    }
    IEnumerator WaitSeconds2()
    {
        int seconds = 0;
        while (seconds <= 3)
        {
            seconds++;
            yield return new WaitForSecondsRealtime(10);
            AddTime(5);
        }
    }
    #endregion
}
