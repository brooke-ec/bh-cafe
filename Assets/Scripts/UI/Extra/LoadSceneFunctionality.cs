using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFunctionality : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SavingSystem.instance.SaveToJson();

        int index = SceneUtility.GetBuildIndexByScenePath("Scenes/" + sceneName);
        if (index != -1)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            print("Scene doesn't exist");
        }
    }

    public void LoadLevel(bool loadThisLevel)
    {
        if (loadThisLevel)
        {
            LoadScene("Level"+God.instance.levelUIManager.lvlSettings.levelNum);
        }
        else
        {
            LoadScene("Level" + God.instance.levelUIManager.lvlSettings.levelNum+1);
        }

    }
}
