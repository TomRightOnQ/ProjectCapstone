using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Core Object lives through the entire game run-time
/// Manager the game's persisitent data
/// </summary>

public class PersistentDataManager : MonoBehaviour
{
    private static PersistentDataManager instance;
    public static PersistentDataManager Instance => instance;
    private long MObjectID = -1;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Return a unique id
    public long GetMObjectID()
    {
        return ++MObjectID;
    }
}
