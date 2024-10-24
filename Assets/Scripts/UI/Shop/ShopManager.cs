using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItemSettings shopItemSettings;
    public Color enoughDiamondsColour;
    public Color notEnoughDiamondsColour;

    public Transform itemsContainer;
    public TextMeshProUGUI diamondsTotal;   

    public void SetUpShop()
    {
        int totalDiamonds = SavingSystem.instance.saveData.diamonds;
        //if shop item can't be bought, change to grey, otherwise white
        for(int i=0; i<itemsContainer.childCount;i++)
        {
            /*
            ShopItem shopItem = itemsContainer.GetChild(i).GetComponent<ShopItem>();
            if(shopItem.cost <= totalDiamonds) 
            {
                shopItem.GetComponent<Image>().color = enoughDiamondsColour;
            }
            else
            {
                shopItem.GetComponent<Image>().color = notEnoughDiamondsColour;
            }*/
        }
        
    }
}
