using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Setting", menuName = "Assets/Levels/New Level Setting")]
public class LevelSettings : ScriptableObject
{
    public int levelNum;


    public int numOfHearts;
    public int lengthOfLevelMins;
    public int scoreNeededForLevel;
    public int scoreReductionForHealth;
}
