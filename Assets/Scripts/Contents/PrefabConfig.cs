using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All references of game prefabs
/// </summary>

[CreateAssetMenu(menuName = "Configs/PrefabConfig")]
public class PrefabConfig : ScriptableSingleton<PrefabConfig>
{
    [SerializeField]
    private List<PrefabData> prefabCollections = new List<PrefabData>();
    private Dictionary<string, PrefabData> prefabMap;

    public List<PrefabData> PrefabCollections => prefabCollections;

    // Return prefabData

    public void Init()
    {
        prefabMap = new Dictionary<string, PrefabData>();
        foreach (var prefabData in prefabCollections)
        {
            prefabMap.Add(prefabData.name, prefabData);
        }
    }

    public PrefabData GetPrefabData(string name)
    {
        if (prefabMap.TryGetValue(name, out PrefabData prefabData))
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
public struct PrefabData
{
    public string name;
    public string PrefabPath;
    public string TypeName;
    public int Count;
    public bool IsExpandable;
    public float ExpandableRatio;
    public bool bPoolable;
    public bool bPoolByDefault;
    public GameObject PrefabReference;
}
