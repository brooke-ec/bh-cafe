using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    // In seconds
    public float currentLevelTime;
    private Transform arrow;
    private int totalLevelSeconds;

    void Awake()
    {
        arrow = transform.GetChild(0);
    }

    public void StartLevelCountdown(int lengthOfLevelMins)
    {
        totalLevelSeconds = lengthOfLevelMins*60;
        StartCoroutine(RotateClockStick());
    }

    //It goes from 45 degrees to -315
    IEnumerator RotateClockStick()
    {

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

    public void AddTime(float time)
    {
        if (currentLevelTime >= 0)
        {


            currentLevelTime -= time;
            float rotationToApply = -360f / totalLevelSeconds;
            arrow.Rotate(new Vector3(0, 0, -rotationToApply * time));
        }
    }

}
