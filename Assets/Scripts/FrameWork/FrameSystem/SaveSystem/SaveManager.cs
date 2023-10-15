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
        SaveConfig.Instance.SetPlayer("You", new Vector3(0f, 0.5f, 0f), Constants.SCENE_DEFAULT_LEVEL);
        SaveConfig.Instance.InitNPCToSave();
        SaveConfig.Instance.LockSave();
    }

    // Write data to scriptable
    public void SavePlayerData()
    {
    
    }

    public void SaveNPCData()
    {
    
    }

    // Read Data
    // Load Save to the scriptable object
    public void LoadSave()
    {
        if (bLoaded)
        {
            return;
        }
    }

    // Get Player Info
    public SaveConfig.PlayerSaveData GetPlayer()
    {
        return SaveConfig.Instance.GetPlayer();
    }

    // Get NPC Info
    public List<SaveConfig.NPCSaveData> GetNPCInfo()
    {
        return SaveConfig.Instance.NpcSaveDataList;
    }


    // Modify NPCs
    public void AddInteractionToNPC(int npcID, int interactionID)
    {
        SaveConfig.Instance.AddInteractionToNPC(npcID, interactionID);
    }

    public void RemoveInteractionFromNPC(int npcID, int interactionID)
    {
        SaveConfig.Instance.RemoveInteractionFromNPC(npcID, interactionID);
    }

    public void ChangeNPCPositionAndScene(int npcID, string sceneName, Vector3 position)
    {
        SaveConfig.Instance.ChangeNPCPositionAndScene(npcID, sceneName, position);
    }

    public void SetNPCActive(int npcID, bool bActive)
    {
        SaveConfig.Instance.SetNPCActive(npcID, bActive);
    }
}
