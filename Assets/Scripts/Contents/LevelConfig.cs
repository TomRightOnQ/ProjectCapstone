using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls how scenes/levels defined
/// </summary>

[CreateAssetMenu(menuName = "Configs/LevelConfig")]
public class LevelConfig : ScriptableSingleton<LevelConfig>
{
    [SerializeField]
    private List<LevelData> levelCollections = new List<LevelData>();
    private Dictionary<string, LevelData> uiMap;

    public List<LevelData> LevelCollections => levelCollections;

    public void Init()
    {
        uiMap = new Dictionary<string, LevelData>();
        foreach (var levelData in levelCollections)
        {
            uiMap.Add(levelData.SceneName, levelData);
        }
    }

    public LevelData GetLevelData(string name)
    {
        if (uiMap.TryGetValue(name, out LevelData levelData))
        {
            return levelData;
        }
        else
        {
            Debug.LogError($"Level {name} not found.");
            return default;
        }
    }

    public LevelData GetLevelData(int index)
    {
        return levelCollections[index];
    }
}

[System.Serializable]
public struct LevelData
{
    public int LevelID;
    public string SceneName;
    public string StringName; // Actual name
    public string NextScene; // The next scene after this level - used for 2D levels only
    public Enums.SCENE_TYPE SceneType;
    public bool bSaveable; // Whether the player in this scene will be saved
    public List<Vector3> SpawnPoints;
    public string BGMName;
}
