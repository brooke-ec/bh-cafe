using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public Transform OrderUI;
    public Transform HealthUI;
    public Transform ScoreUI;

    public Transform ClockUI;

    public void StartLevel()
    {
        ClockUI.GetComponent<TimerUI>().StartLevelCountdown();
    }
}
