using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFunctionality : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SavingSystem.instance.SaveToJson();

        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.IsValid())
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
