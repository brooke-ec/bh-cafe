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
    public ShopItemInfo info;
    public int modifier;
}

[Serializable]
public class ShopItemInfo
{
    public ShopItemType shopItemType;
    [Range(1, 3)]
    public int shopItemTypeNum = 1;
}
