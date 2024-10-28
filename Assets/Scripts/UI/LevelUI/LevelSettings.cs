using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Setting", menuName = "Assets/Levels/New Level Setting")]
public class LevelSettings : ScriptableObject
{
    public int levelNum;
    public string levelDescription;
    public int numOfHearts;
    public float lengthOfLevelMins;
    public int scoreNeededForLevel;
    public int scoreReductionForHealth;
    /// <summary>
    /// For every 50 extra score, get this % of extra diamonds
    /// </summary>
    [Range(20, 50)]
    public int extraDiamondPercentage = 5;
}
