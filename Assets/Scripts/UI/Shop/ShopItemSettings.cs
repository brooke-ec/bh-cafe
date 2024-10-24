using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item Settings", menuName = "Assets/New Shop Item Settings")]
public class ShopItemSettings : ScriptableObject
{    
    public List<ShopItem> shopItems = new List<ShopItem>();
}

public enum ShopItemType
{
    Time,
    Hearts,
    DashSpeed,
    Diamonds
} 

[Serializable]
public class ShopItem
{
    public int cost;
    public Sprite icon;
    public ShopItemType shopItemType;
    public int shopItemTypeNum;
    public int modifier;
}
