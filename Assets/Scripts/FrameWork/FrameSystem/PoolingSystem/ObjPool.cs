using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Independent pool holding given type of objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjPool<T> : ObjPoolBase where T : Component
{
    // Data holder
    private Queue<T> objects;
    private GameObject prefab;
    private Type componentType;
    // Controlling parameters
    private bool isExpanding = false; // Whether the pool will expand
    private float expRatio = 0.1f; // How much will the size increase
    private int count = 0; // Record the max size

    public ObjPool(GameObject prefab, int count, bool isExp = false, float expR = 0.1f)
    {
        this.prefab = prefab;
        this.objects = new Queue<T>();
        this.isExpanding = isExp;
        this.expRatio = expR;
        this.count = count;

        Expand(count);
    }

    // Special for non-generic
    public override GameObject GetGameObject()
    {
        var poolObj = GetPoolObj();
        return poolObj?.gameObject;
    }

    public override void ReturnGameObject(GameObject prefab)
    {
        Return(prefab);
    }

    public T GetPoolObj()
    {
        if (objects.Count == 0)
        {
            if (isExpanding)
            {
                Expand(Mathf.Max(1, Mathf.RoundToInt(count * expRatio)));
            }
            else
            {
                Debug.LogWarning("Pool is empty and not expandable.");
                return null;
            }
        }

        T objectToReturn = objects.Dequeue();
        if (objectToReturn == null)
        {
            return null;
        }
        objectToReturn.gameObject.SetActive(true);
        return objectToReturn;
    }

    public void Return(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }

    public void Return(GameObject objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn.GetComponent<T>());
    }

    // Expand
    public void Expand(int additionalCount)
    {
        for (int i = 0; i < additionalCount; i++)
        {
            GameObject newObject = GameObject.Instantiate(prefab);
            newObject.SetActive(false);
            objects.Enqueue(newObject.GetComponent<T>());
        }
        count += additionalCount;
    }
}
