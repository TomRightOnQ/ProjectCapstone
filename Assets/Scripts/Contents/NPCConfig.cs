using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hold the Information of NPC Prefabs -> Replace PrefabConfig
/// </summary>
[CreateAssetMenu(menuName = "Configs/NPCConfig")]
public class NPCConfig : ScriptableSingleton<NPCConfig>
{
    [SerializeField]
    private List<NPCPrefabData> prefabCollections = new List<NPCPrefabData>();
    private Dictionary<string, NPCPrefabData> prefabMap;

    public List<NPCPrefabData> PrefabCollections => prefabCollections;

    // Return prefabData

    public void Init()
    {
        prefabMap = new Dictionary<string, NPCPrefabData>();
        foreach (var prefabData in prefabCollections)
        {
            prefabMap.Add(prefabData.name, prefabData);
        }
    }

    public NPCPrefabData GetPrefabData(string name)
    {
        if (prefabMap.TryGetValue(name, out NPCPrefabData prefabData))
        {
            return prefabData;
        }
        else
        {
            Debug.LogError($"Prefab {name} not found.");
            return default;
        }
    }
}

[System.Serializable]
public struct NPCPrefabData
{
    public string name;
    public GameObject PrefabReference;
}
