using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public static SavingSystem instance;

    public SaveData saveData = new SaveData();
    public string filePathEnd = "/save.json";
    private string filePath;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        filePath = Application.persistentDataPath + filePathEnd;
        print("d");
        LoadFromJson();
    }

    public void LoadFromJson()
    {
        print("loaded");
        string saveDataAsString = System.IO.File.ReadAllText(filePath);
        saveData = JsonUtility.FromJson<SaveData>(saveDataAsString);
    }
    public void SaveToJson()
    {
        string saveDataAsString = JsonUtility.ToJson(saveData);

        System.IO.File.WriteAllText(filePath, saveDataAsString);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}

[System.Serializable]
public class SaveData
{
    public List<int> levelNums = new List<int>();
    public List<int> highscoreLevel = new List<int>();
}