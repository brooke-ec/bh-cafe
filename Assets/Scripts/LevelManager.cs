using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void StartLevel()
    {
        God.instance.levelUIManager.StartLevel();
    }
}
