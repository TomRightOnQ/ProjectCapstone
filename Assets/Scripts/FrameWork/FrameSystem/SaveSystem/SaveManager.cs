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
            configEventHandlers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Config events
    private void configEventHandlers()
    {
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_UNLOADED, OnRecv_SceneUnLoaded);
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
        SaveConfig.Instance.SetPlayer(new Vector3(0f, 0.5f, 0f), Constants.SCENE_DEFAULT_LEVEL);
        SaveConfig.Instance.InitDayToSave();
        SaveConfig.Instance.InitNPCToSave();
        SaveConfig.Instance.InitGuildToSave();
        SaveConfig.Instance.InitCharacter2DToList();
        SaveConfig.Instance.InitUnlockHintToSave();
        SaveConfig.Instance.InitHUDInteractionDisableList();
        SaveConfig.Instance.LockSave();
    }

    // Flash Back to Day 0
    public void ChangeSaveToDayZero()
    {
        SaveConfig.Instance.SetPlayer(new Vector3(0f, 0.5f, 0f), Constants.SCENE_DEFAULT_LEVEL);
        SaveConfig.Instance.InitDayToSave();
        SaveConfig.Instance.InitNPCToSave();
        SaveConfig.Instance.InitGuildToSave();
        SaveConfig.Instance.InitCharacter2DToList();
        SaveConfig.Instance.InitHUDInteractionDisableList();
        SaveConfig.Instance.LockSave();
    }

    // Set the save state to a specfic day
    public void SetSaveToDay(int day)
    {
        // Set Character Lock
        SaveConfig.Instance.SetCharacter2DLockToDay(day);
    }

    // Write data to scriptable
    public void SavePlayerData()
    {
    
    }

    public void SaveNPCData()
    {
    
    }

    // Character Locking
    // Lock or Unlock a 2D character
    public void LockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        SaveConfig.Instance.LockCharacter2D(characterID, levelType);
    }

    public void UnlockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        SaveConfig.Instance.UnlockCharacter2D(characterID, levelType);
    }

    // Get the current character lock
    public List<int> GetCurrentCharacterLock(int currentDay, Enums.LEVEL_TYPE levelType)
    {
        return SaveConfig.Instance.GetCurrentCharacterLock(currentDay, levelType);
    }

    // DayCycle
    public void SaveMaxDay(int maxDay)
    {
        if (maxDay <= SaveConfig.Instance.GetDay().MaxDay)
        {
            return;
        }
        SaveConfig.Instance.GetDay().MaxDay = maxDay;
    }

    public void SaveCurrentDay(int currentDay)
    {
        SaveConfig.Instance.GetDay().CurrentDay = currentDay;
    }

    public void SaveCurrentDayInited(bool bInited)
    {
        SaveConfig.Instance.GetDay().bInited = bInited;
    }

    public void SaveGuildData(int targetID, int score, int win, int lose, bool bElminated)
    {
        List<SaveConfig.GuildSaveData> guildList = SaveConfig.Instance.GuildSaveDataList;
        for (int i = 0; i < guildList.Count; i++)
        {
            if (targetID == guildList[i].GuildID)
            {
                guildList[i].Score += score;
                guildList[i].DuelWin += win;
                guildList[i].DuelLose += lose;
                if (!guildList[i].bElminated)
                {
                    guildList[i].bElminated = bElminated;
                }
            }
        }
    }

    // Set the guild data
    public void SaveGuildData_F(int targetID, int score, int win, int lose, bool bElminated)
    {
        List<SaveConfig.GuildSaveData> guildList = SaveConfig.Instance.GuildSaveDataList;
        for (int i = 0; i < guildList.Count; i++)
        {
            if (targetID == guildList[i].GuildID)
            {
                guildList[i].Score = score;
                guildList[i].DuelWin = win;
                guildList[i].DuelLose = lose;
                guildList[i].bElminated = bElminated;
            }
        }
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

    // Get Current Day
    public int GetCurrentDay()
    {
        return SaveConfig.Instance.GetDay().CurrentDay;
    }

    public int GetMaxDay()
    {
        return SaveConfig.Instance.GetDay().MaxDay;
    }

    // Get current day init status
    public bool GetIsCurrentDayInited()
    {
        return SaveConfig.Instance.GetDay().bInited;
    }

    // Modify NPCs
    public void AddNPCToSave(int npcID, string sceneName, Vector3 position)
    {
        SaveConfig.Instance.AddNPCToSave(npcID, sceneName, position);
    }

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

    // Save or change note data
    public void AddNote(Enums.NOTE_TYPE type, int[] IDs)
    {
        SaveConfig.Instance.AddNote(type, IDs);
    }
    public void RemoveNote(Enums.NOTE_TYPE type, int[] IDs)
    {
        SaveConfig.Instance.RemoveNote(type, IDs);
    }

    // Get Note Data
    public List<int> GetNote(Enums.NOTE_TYPE type)
    {
        return SaveConfig.Instance.GetNote(type);
    }

    // Get Hint Data
    public void UnlockHint(int hintID)
    {
        SaveConfig.Instance.UnlockHint(hintID);
    }

    public bool CheckHintUnlocked(int hintID)
    {
        return SaveConfig.Instance.HintUnlockList.Contains(hintID);
    }

    // Check, Enable and Disable HUDInteraction
    public bool CheckHUDInteractionEnabled(int interactionID)
    {
        return !SaveConfig.Instance.DisabledHUDInteractionList.Contains(interactionID);
    }

    public void EnableHUDInteraction(int interactionID)
    {
        SaveConfig.Instance.EnableHUDInteraction(interactionID);
    }

    public void DisableHUDInteraction(int interactionID)
    {
        SaveConfig.Instance.DisableHUDInteraction(interactionID);
    }

    // Private:
    // Event Handlers
    private void OnRecv_SceneLoaded()
    {
        if (!LevelConfig.Instance.GetLevelData(LevelManager.Instance.CurrentScene).bSaveable)
        {
            return;
        }
        // Save data
        SaveConfig.Instance.SetPlayer(PersistentDataManager.Instance.MainPlayer.gameObject.transform.position, LevelManager.Instance.CurrentScene);

        // Begin Save Cycle every given time
    }

    private void OnRecv_SceneUnLoaded()
    {

    }
}
