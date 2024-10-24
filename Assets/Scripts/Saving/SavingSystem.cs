using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        filePath = Application.persistentDataPath + filePathEnd;
        if(File.Exists(filePath))
        {
            string saveDataAsString = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<SaveData>(saveDataAsString);
        }
        else
        {
            saveData = new SaveData();
        }

    }
    public void SaveToJson()
    {
        print("saved");
        string saveDataAsString = JsonUtility.ToJson(saveData);

        File.WriteAllText(filePath, saveDataAsString);
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
    public int diamonds;

    public List<ShopItemInfo> shopUpgradesBought = new List<ShopItemInfo>();
}