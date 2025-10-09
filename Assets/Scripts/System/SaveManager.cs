using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string savePath = "Assets\\Scripts\\Save.json";

    public void SaveAll(List<BuildingSaveData> buildings)
    {
        if (!File.Exists(savePath))
            File.Create(savePath);

        SaveWrapper wrapper = new SaveWrapper { buildings = buildings };
        string json = JsonUtility.ToJson(wrapper, true);

        File.WriteAllText(savePath, json);

        Debug.Log("Buildings were saved");
    }
    public List<BuildingSaveData> LoadAll()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("There is no save file");
            return new List<BuildingSaveData> {};
        }

        string json = File.ReadAllText(savePath);
        SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);
        
        return wrapper.buildings;
    }

    void Awake()
    {
        Instance = this;
    }
}

[Serializable]
class SaveWrapper
{
    public List<BuildingSaveData> buildings;
}