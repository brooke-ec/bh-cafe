 using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

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
            shopItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shopItemInfo.shopItemType.ToString() + " "+shopItemInfo.shopItemTypeNum;
            shopItem.transform.GetChild(1).GetComponent<Image>().sprite = shopItemInfo.icon;
            shopItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = shopItemInfo.cost.ToString();

            if(totalDiamonds >= shopItemInfo.cost) 
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
    public int HasUpgradeBeenBought(ShopItem shopItemInfo)
    {
        int index = SavingSystem.instance.saveData.shopUpgradesBought.FindIndex(item => (item.shopItemType == shopItemInfo.shopItemType) && (item.shopItemTypeNum == shopItemInfo.shopItemTypeNum));
        return index;
    }

    public void BuyUpgrade(ShopItem shopItemInfo)
    {
        if(SavingSystem.instance.saveData.diamonds >= shopItemInfo.cost)
        {

            SavingSystem.instance.saveData.diamonds -= shopItemInfo.cost;
        }
        else
        {
            //Make X sound
        }
    }
}
