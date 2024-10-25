using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Transform buttonsContainer;
    public Transform levelsContainer;

    [Header("Levels")]
    public Transform levelButtonsContainer;
    public Sprite levelLocked;
    public Sprite levelCompleted;

    private void Start()
    {
        levelsContainer.gameObject.SetActive(true);
        UpdateLevelsIcons();
        levelsContainer.gameObject.SetActive(false);
    }

    public void OpenTab(Transform container)
    {
        buttonsContainer.gameObject.SetActive(false);
        levelsContainer.gameObject.SetActive(false);

        container.gameObject.SetActive(true);
    }

    public void LoadLevel(LevelSettings lvlSettings)
    {
        SavingSystem.instance.SaveToJson();

        int index = SceneUtility.GetBuildIndexByScenePath("Scenes/"+ "Level" + lvlSettings.levelNum);
        if(index != -1)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            print("Scene doesn't exist");
        }
    }

    private void UpdateLevelsIcons()
    {
        for(int i=0;i<levelButtonsContainer.childCount; i++)
        {
            Transform level = levelButtonsContainer.GetChild(i);

            Sprite spriteUsed;
            int index = SavingSystem.instance.saveData.levelNums.FindIndex(val => val == i+1);
            if(index == -1)
            {
                spriteUsed = levelLocked;
            }
            else
            {
                int scoreAchieved = SavingSystem.instance.saveData.highscoreLevel[index];
                if(scoreAchieved > level.GetComponent<MenuLevel>().levelSettings.scoreNeededForLevel)
                {
                    spriteUsed = levelCompleted;
                }
                else
                {
                    spriteUsed = levelLocked;
                }
            }
            level.GetChild(0).GetComponent<Image>().sprite = spriteUsed;

            level.GetChild(1).GetComponent<TextMeshProUGUI>().text = "LEVEL " + level.GetComponent<MenuLevel>().levelSettings.levelNum;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
