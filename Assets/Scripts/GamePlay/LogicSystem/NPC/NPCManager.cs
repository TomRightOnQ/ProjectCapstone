using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls NPCs in game
/// </summary>
public class NPCManager : MonoBehaviour
{
    private static NPCManager instance;
    public static NPCManager Instance => instance;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
