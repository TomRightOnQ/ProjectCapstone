using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// This file is manages the references of all objects, communicating with the pooling system.
/// </summary>

public class PrefabManager : MonoBehaviour
{
    private static PrefabManager instance;
    public static PrefabManager Instance => instance;

    // Structures to map references around
    private Dictionary<string, PrefabData> prefabReferences = new Dictionary<string, PrefabData>();
    private Dictionary<string, string> prefabTypes = new Dictionary<string, string>();
    public Dictionary<string, PrefabData> PrefabReferences => prefabReferences;

    [SerializeField] private const string FILE_NAME = "Prefabs.json";
    [SerializeField] private const string SUFFIX = "(Clone)";

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            GameObject.Destroy(gameObject);
        }
    }

    // Add to the pooling system
    public void SetPoolUp(string typeName, GameObject prefabReference, int count, bool isExpandable, float expandableRatio)
    {
        Type componentType = Type.GetType(typeName);
        if (componentType == null)
        {
            Debug.LogWarning("Failed to get the type for component: " + typeName);
            return;
        }
        MethodInfo createPoolMethod = typeof(Pooling).GetMethod("CreatePool");
        MethodInfo genericCreatePoolMethod = createPoolMethod.MakeGenericMethod(componentType);
        if (Pooling.Instance != null)
        {
            genericCreatePoolMethod.Invoke(Pooling.Instance, new object[] { prefabReference, count, isExpandable, expandableRatio });
        }
        else
        {
            Debug.LogWarning("Pooling.Instance is null");
        }
    }

    public void ResetPooling()
    {
        Pooling.Instance.ClearPools();
    }

    public void InitPooling()
    {
        foreach (var prefabData in PrefabConfig.Instance.PrefabCollections)
        {
            if (prefabData.bPoolByDefault && prefabData.bPoolable)
            {
                SetPoolUp(prefabData.TypeName, prefabData.PrefabReference, prefabData.Count, prefabData.IsExpandable, prefabData.ExpandableRatio);
                prefabReferences[prefabData.name] = prefabData;
                prefabTypes[prefabData.name] = prefabData.TypeName;
            }
        }
    }

    // Call this method at the start of your game to reset the state
    public void ResetState()
    {
        prefabReferences.Clear();
    }

    // Overloading of native Instantiate
    public GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation)
    {
        PrefabData prefabData = PrefabConfig.Instance.GetPrefabData(prefabName);
        if (!prefabData.Equals(default(PrefabData)))
        {
            if (prefabData.bPoolable)
            {
                // Check if pool exists
                if (Pooling.Instance.GetPool(prefabData.TypeName) == null)
                {
                    // Create new pool
                    SetPoolUp(prefabData.TypeName, prefabData.PrefabReference, prefabData.Count, prefabData.IsExpandable, prefabData.ExpandableRatio);
                    prefabReferences[prefabData.name] = prefabData;
                    prefabTypes[prefabData.name] = prefabData.TypeName;
                }

                GameObject instance = Pooling.Instance.GetObj(prefabName);
                if (instance == null)
                {
                    return null;
                }
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                instance.SetActive(true);
                return instance;
            }
        }
        return GameObject.Instantiate(prefabData.PrefabReference, position, rotation);
    }

    // With this variation, skip the pooling and place directly
    public GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, bool bPlacingNPC = true)
    {
        NPCPrefabData prefabData = NPCConfig.Instance.GetPrefabData(prefabName);
        return GameObject.Instantiate(prefabData.PrefabReference, position, rotation);
    }

    public void Destroy(GameObject gameObject)
    {
        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
        PrefabData prefabData = PrefabConfig.Instance.GetPrefabData(prefabName);

        // If the prefab is not fiound, remove it directly
        if (prefabData.Equals(default(PrefabData)))
        {
            GameObject.Destroy(gameObject);
            return;
        }

        // Check if pool exists
        if (Pooling.Instance.GetPool(prefabData.TypeName) != null)
        {
            if (!prefabData.Equals(default(PrefabData)) && prefabData.bPoolable)
            {
                Pooling.Instance.ReturnObj(gameObject);
                return;
            }
        }
        GameObject.Destroy(gameObject);
    }


    public GameObject GetReference(string name)
    {
        return prefabReferences[name].PrefabReference;
    }

    public string GetReferenceType(string name)
    {
        return prefabReferences[name].TypeName;
    }

    public string GetReferenceType(GameObject prefab)
    {
        prefabTypes.TryGetValue(prefab.name.Replace("(Clone)", "").Trim(), out string componentName);
        return componentName;
    }
}
