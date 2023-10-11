using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Process Saving
/// </summary>
public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance => instance;

    // When in-game, no repeating loading is allowed
    [SerializeField, ReadOnly]
    private bool bLoaded = false;

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

    // Public:
    // Init a new save
    // --Caution--
    // This will completely rewrite the saving scriptable
    public void CreateNewSave()
    {
        if (!SaveConfig.Instance.AllowRewrite)
        {
            return;
        }
        // Now load data for new game...
    }

    // Write data to scriptable
    public void SavePlayerData()
    {
    
    }

    public void SaveNPCData()
    {
    
    }

    // Read Data
    // When loading save, call all other modules' loading from here
    public void LoadSave()
    {
        if (bLoaded)
        {
            return;
        }
        // Call other managers to load...
    }
}
