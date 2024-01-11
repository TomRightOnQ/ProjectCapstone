using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Process Saving
/// </summary>
public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance => instance;

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

    /// <summary>
    /// Save and Load
    /// </summary>

    // Serialize a Save
    // --Caution--
    // This will save the scriptable object to a chosen JSON file
    // Default: Change the autosave
    public void SaveGameSave(string saveName = Constants.SAVE_CURRENT_SAVE)
    {
        if (!LevelManager.Instance.bCurrentSceneSaveable)
        {
            return;
        }
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveName);
        string json = JsonUtility.ToJson(SaveConfig.Instance);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("CurrentDaySave saved to " + Application.persistentDataPath);
    }

    public void SaveGameSave(int day = 0)
    {
        if (!LevelManager.Instance.bCurrentSceneSaveable)
        {
            return;
        }
        string saveName = Constants.SAVE_DAY_SAVE + day.ToString();
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveName);
        string json = JsonUtility.ToJson(SaveConfig.Instance);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("CoreSaveConfig saved to " + Application.persistentDataPath);
    }

    // Load a Save
    // --Caution--
    // This will load the the targeted JSON and change the scriptable object to it
    // Default: Load the autosave
    public void LoadGameSave(string saveName = Constants.SAVE_CURRENT_SAVE)
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveName);
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            // SaveConfig saveConfig = JsonUtility.FromJson<SaveConfig>(json);
            JsonUtility.FromJsonOverwrite(json, SaveConfig.Instance);
            Debug.Log("CoreSaveConfig loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found in " + saveFilePath);
        }
    }

    public void LoadGameSave(int day = 0)
    {
        string saveName = Constants.SAVE_DAY_SAVE + day.ToString();
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveName);
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            // SaveConfig saveConfig = JsonUtility.FromJson<SaveConfig>(json);
            JsonUtility.FromJsonOverwrite(json, SaveConfig.Instance);
            Debug.Log("CoreSaveConfig loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found in " + saveFilePath);
        }
    }

    // Save the Core Save Data
    // --Caution--
    // This will save the persisitent save data
    public void SaveGameCoreSave(string saveName = Constants.SAVE_CORE_SAVE)
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveName);
        string json = JsonUtility.ToJson(CoreSaveConfig.Instance);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("CoreSaveConfig saved to " + Application.persistentDataPath);
    }

    // Load the Core Save Data
    // --Caution--
    // This will load the persisitent save data
    public void LoadGameCoreSave(string saveName = Constants.SAVE_CORE_SAVE)
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveName);
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            // CoreSaveConfig coreSaveConfig = JsonUtility.FromJson<CoreSaveConfig>(json);
            JsonUtility.FromJsonOverwrite(json, CoreSaveConfig.Instance);
            Debug.Log("CoreSaveConfig loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found in " + saveFilePath);
        }
    }


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
        CoreSaveConfig.Instance.InitUnlockHintToSave();

        SaveConfig.Instance.SetPlayer(new Vector3(0f, 0.5f, 0f), Constants.SCENE_DEFAULT_LEVEL);
        SaveConfig.Instance.InitDayToSave();
        SaveConfig.Instance.InitNPCToSave();
        SaveConfig.Instance.InitGuildToSave();
        SaveConfig.Instance.InitCharacter2DToList();
        SaveConfig.Instance.InitHUDInteractionDisableList();
        SaveConfig.Instance.LockSave();

        // Load Day 0 Configs


        // Create Save File
        SaveGameSave(Constants.SAVE_CURRENT_SAVE);
        SaveGameSave(0);
        SaveGameCoreSave();
    }

    /// <summary>
    /// Methods
    /// </summary>

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
    public List<int> GetCurrentCharacterLock(Enums.LEVEL_TYPE levelType)
    {
        return SaveConfig.Instance.GetCurrentCharacterLock(levelType);
    }

    // DayCycle
    public void SaveMaxDay(int maxDay)
    {
        CoreSaveConfig.Instance.MaxDay = Math.Max(CoreSaveConfig.Instance.MaxDay, maxDay);
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
        CoreSaveConfig.Instance.AddNote(type, IDs);
        SaveGameCoreSave();
    }
    public void RemoveNote(Enums.NOTE_TYPE type, int[] IDs)
    {
        CoreSaveConfig.Instance.RemoveNote(type, IDs);
        SaveGameCoreSave();
    }

    // Get Note Data
    public List<int> GetNote(Enums.NOTE_TYPE type)
    {
        return CoreSaveConfig.Instance.GetNote(type);
    }

    // Get Hint Data
    public void UnlockHint(int hintID)
    {
        CoreSaveConfig.Instance.UnlockHint(hintID);
        SaveGameCoreSave();
    }

    public bool CheckHintUnlocked(int hintID)
    {
        return CoreSaveConfig.Instance.HintUnlockList.Contains(hintID);
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
        // Save the game
        SaveManager.Instance.SaveGameSave(Constants.SAVE_CURRENT_SAVE);
    }

    private void OnRecv_SceneUnLoaded()
    {

    }
}
