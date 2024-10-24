using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    // In seconds
    public float currentLevelTime;
    private Transform arrow;

    void Awake()
    {
        arrow = transform.GetChild(0);
    }

    public void StartLevelCountdown(int lengthOfLevelMins)
    {
        StartCoroutine(RotateClockStick(lengthOfLevelMins));
    }

    //It goes from 45 degrees to -315
    IEnumerator RotateClockStick(int lengthOfLevelMins)
    {
        int totalLevelSeconds = lengthOfLevelMins*60;

        print(-360f/totalLevelSeconds);
        float rotationToApply = -360f/totalLevelSeconds; //rotation between the max time in seconds
        
        while(currentLevelTime < totalLevelSeconds)
        {
            currentLevelTime += 1;
            arrow.Rotate(new Vector3(0, 0, rotationToApply));
            yield return new WaitForSecondsRealtime(1);
        }

        God.instance.levelUIManager.EndLevel();
    }

}
