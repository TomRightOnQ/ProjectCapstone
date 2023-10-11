using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All references of UI prefabs
/// </summary>

[CreateAssetMenu(menuName = "Configs/UIConfig")]
public class UIConfig : ScriptableSingleton<UIConfig>
{
    [SerializeField]
    private List<UIData> uiCollections = new List<UIData>();
    private Dictionary<string, UIData> uiMap;

    public List<UIData> UICollections => uiCollections;

    // Return prefabData

    public void Init()
    {
        uiMap = new Dictionary<string, UIData>();
        foreach (var uiData in uiCollections)
        {
            uiMap.Add(uiData.name, uiData);
        }
    }

    public UIData GetUIData(string name)
    {
        if (uiMap.TryGetValue(name, out UIData uiData))
        {
            return uiData;
        }
        else
        {
            Debug.LogError($"Prefab {name} not found.");
            return default;
        }
    }
}

[System.Serializable]
public struct UIData
{
    public string name;
    public string PrefabPath;
    public string TypeName;
    public GameObject PrefabReference;
    public int UIExclusionGroup;
}
