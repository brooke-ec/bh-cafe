using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Transform buttonsContainer;
    public Transform levelsContainer;

    [Header("Levels")]
    public Transform levelButtonsContainer;
    public Sprite levelLocked;
    public Sprite levelCompleted;
    

    public void OpenTab(Transform container)
    {
        buttonsContainer.gameObject.SetActive(false);
        levelsContainer.gameObject.SetActive(false);

        container.gameObject.SetActive(true);
    }

    public void LoadLevel(int levelNum)
    {
        SavingSystem.instance.SaveToJson();

        Scene sceneToLoad = SceneManager.GetSceneByName("Level"+levelNum);
        if(sceneToLoad.IsValid())
        {
            SceneManager.LoadScene("Level"+levelNum);
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
            int index = SavingSystem.instance.saveData.levelNums.Find(val => val == i);
            if(index == -1)
            {
                
            }
            else
            {

            }
            //level.GetChild(0).GetComponent<Image>().sprite = 
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
