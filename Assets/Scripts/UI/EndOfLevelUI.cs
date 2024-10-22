using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndOfLevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelNumText;
    public Transform contentTransform;
    public Transform titleTransform;

    private LevelSettings lvlSettings;

    void Start()
    {
       lvlSettings = God.instance.levelUIManager.lvlSettings; 
    }

    private void SetUpContent()
    {

    }

    private void SetUpTitle()
    {
        titleTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lvlSettings.levelNum.ToString();
        //IF FAILED OR NOT
        //titleTransform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 
    }
}
