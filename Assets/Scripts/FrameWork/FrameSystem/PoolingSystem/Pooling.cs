using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Controls all poolings behaviours
/// Using prefab reference : ObjPool object pairs to keep pools
/// </summary>

public class Pooling : MonoBehaviour
{
    private static Pooling instance;
    public static Pooling Instance => instance;
    private Dictionary<string, object> pools = new Dictionary<string, object>();

    public void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Clear all pool references
    public void ClearPools()
    {
        pools.Clear();
    }

    // Add a new pool to the system
    public void CreatePool<T>(GameObject prefab, int initialSize, bool isExpandbale = false, float expRatio = 0.1f) where T : Component
    {
        var type = typeof(T).Name;
        if (pools.ContainsKey(type))
        {
            Debug.LogWarning($"Pool for {typeof(T).Name} is already in the pooling.");
            return;
        }
        var pool = new ObjPool<T>(prefab, initialSize, isExpandbale, expRatio);
        pools[type] = pool;
    }

    // Return the pool of the given type
    public ObjPool<T> GetPool<T>() where T : Component
    {
        var type = typeof(T).Name;
        pools.TryGetValue(type, out object pool);
        if (pool == null)
        {
            Debug.LogWarning($"Pool for {typeof(T).Name} not found.");
        }
        return pool as ObjPool<T>;
    }

    public ObjPoolBase GetPool(string typeName)
    {
        pools.TryGetValue(typeName, out object pool);
        if (pool == null)
        {
            Debug.LogWarning($"Pool for {typeName} not found.");
            return null;
        }

        if (pool is ObjPoolBase typedPool)
        {
            return typedPool;
        }
        else
        {
            Debug.LogWarning($"Pool found, but could not be cast to ObjPoolBase.");
            return null;
        }
    }

    // Get an object or an object's class component
    public T GetObj<T>() where T : Component
    {
        var pool = GetPool<T>();
        if (pool != null)
        {
            return pool.GetPoolObj();
        }
        else
        {
            Debug.LogWarning($"Pool for {typeof(T).Name} not found.");
            return null;
        }
    }

    public GameObject GetObj(string typeName)
    {
        var pool = GetPool(typeName);
        if (pool != null)
        {
            return pool.GetGameObject();
        }
        else
        {
            Debug.LogWarning($"Pool for " + typeName + " not found.");
            return null;
        }
    }

    // Return an object based on its reference or component
    public void ReturnObj<T>(T prefab) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogWarning("Attempting to return a null component to the pool.");
            return;
        }
        var pool = GetPool<T>();
        if (pool != null)
        {
            pool.Return(prefab);
        }
        else
        {
            Debug.LogWarning($"Pool for {typeof(T).Name} not found.");
        }
    }

    public void ReturnObj(GameObject prefab)
    {
        string typeName = PrefabManager.Instance.GetReferenceType(prefab);
        var pool = GetPool(typeName);
        if (pool != null)
        {
            pool.ReturnGameObject(prefab);
        }
        else
        {
            Debug.LogWarning($"Pool for " + typeName + " not found.");
        }
    }
}
