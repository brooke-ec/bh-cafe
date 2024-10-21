using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    /// <summary>
    /// In minutes
    /// </summary>
    public int maxLevelTime;

    /// <summary>
    /// In seconds
    /// </summary>
    public float currentLevelTime;
    private Transform arrow;

    void Start()
    {
        arrow = transform.GetChild(0);
        //StartLevelCountdown();
    }

    public void StartLevelCountdown()
    {
        StartCoroutine(RotateClockStick());
    }

    //It goes from 45 degrees to -315
    IEnumerator RotateClockStick()
    {
        int totalLevelSeconds = maxLevelTime*60;

        print(-360f/totalLevelSeconds);
        float rotationToApply = -360f/totalLevelSeconds; //rotation between the max time in seconds
        
        while(currentLevelTime < totalLevelSeconds)
        {
            currentLevelTime += 1;
            arrow.Rotate(new Vector3(0, 0, rotationToApply));

            print("rotate");
            yield return new WaitForSecondsRealtime(1);
        }
        print("stop");
    }

}
