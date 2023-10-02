using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core base class
/// </summary>

public class MObject : MonoBehaviour
{
    // Unique ID as serialized private field
    [SerializeField, ReadOnly]
    private long uuid;
    [SerializeField, ReadOnly]
    private string objectName;

    public long UUID { get { return uuid; } private set { uuid = value; } }
    public string ObjectName { get { return objectName; } set { objectName = value; } }

    void Awake()
    {
        uuid = PersistentDataManager.Instance.GetMObjectID();
    }
}

