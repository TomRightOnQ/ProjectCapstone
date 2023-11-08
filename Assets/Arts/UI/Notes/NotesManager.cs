using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System that records useful information for the player
/// </summary>

public class NotesManager : MonoBehaviour
{
    private static NotesManager instance;
    public static NotesManager Instance => instance;

    // UI Components
    [SerializeField] private UI_Notes ui_Notes;

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
