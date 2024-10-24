 using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    public ShopItemSettings shopItemSettings;
    public Color enoughDiamondsColour;
    public Color notEnoughDiamondsColour;
    public Color boughtColour;
    public GameObject shopItemPrefab;


    public Transform itemsContainer;
    public TextMeshProUGUI diamondsText;   

    void Start()
    {
        SetUpShop();
    }
    public void SetUpShop()
    {
        int totalDiamonds = SavingSystem.instance.saveData.diamonds;
        diamondsText.text = totalDiamonds.ToString();

        for(int i=0;i< shopItemSettings.shopItems.Count;i++)
        {
            GameObject shopItem = Instantiate(shopItemPrefab, itemsContainer);
            ShopItem shopItemInfo = shopItemSettings.shopItems[i];
            shopItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shopItemInfo.info.shopItemType.ToString() + " "+shopItemInfo.info.shopItemTypeNum;
            shopItem.transform.GetChild(1).GetComponent<Image>().sprite = shopItemInfo.icon;
            shopItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = shopItemInfo.cost.ToString();

            shopItem.GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(shopItemInfo, shopItem.GetComponent<Image>()));
            
            if (HasUpgradeBeenBought(shopItemInfo.info.shopItemType, shopItemInfo.info.shopItemTypeNum) != -1)
            {
                shopItem.GetComponent<Image>().color = boughtColour;
            }
            else if(totalDiamonds >= shopItemInfo.cost) 
            {
                shopItem.GetComponent<Image>().color = enoughDiamondsColour;
            }
            else
            {
                shopItem.GetComponent<Image>().color = notEnoughDiamondsColour;
            }
        }
    }

/// <returns>-1 if not bought, otherwise the index in the saved list</returns>
    public int HasUpgradeBeenBought(ShopItemType shopItemType, int shopItemNum)
    {
        int index = SavingSystem.instance.saveData.shopUpgradesBought.FindIndex(item => (item.shopItemType == shopItemType) && (item.shopItemTypeNum == shopItemNum));
        return index;
    }

    public bool PreviousUpgradesBought(ShopItemType shopItemType, int shopItemNum)
    {
        if(shopItemNum == 1)
        {
            return true;
        }
        else
        {
            return (HasUpgradeBeenBought(shopItemType, shopItemNum-1) != -1);
        }
    }

    public void BuyUpgrade(ShopItem shopItemInfo, Image itemImage)
    {
        if (SavingSystem.instance.saveData.diamonds >= shopItemInfo.cost && PreviousUpgradesBought(shopItemInfo.info.shopItemType, shopItemInfo.info.shopItemTypeNum))
        {
            //if not bought yet, buy
            if (HasUpgradeBeenBought(shopItemInfo.info.shopItemType, shopItemInfo.info.shopItemTypeNum) == -1)
            {
                SavingSystem.instance.saveData.shopUpgradesBought.Add(shopItemInfo.info);
                SavingSystem.instance.saveData.diamonds -= shopItemInfo.cost;
                diamondsText.transform.DOScale(transform.localScale * 1.1f, 0.3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
                diamondsText.text = SavingSystem.instance.saveData.diamonds.ToString();
                itemImage.color = boughtColour;

                AudioManager.instance.PlaySound(AudioManager.SoundEnum.bought);
                return;
            }
        }
        AudioManager.instance.PlaySound(AudioManager.SoundEnum.error);
    }
}
