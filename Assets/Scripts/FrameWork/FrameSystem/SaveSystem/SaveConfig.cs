using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Save Config - Hold a full game state and interact with the serialized JSONS
/// This save portion only contains information that is changing from different save
/// </summary>
[CreateAssetMenu(menuName = "SaveSystem/SaveConfig")]
public class SaveConfig : ScriptableSingleton<SaveConfig>
{
    // Control flag
    [SerializeField] private bool bAllowRewrite = true;
    public bool AllowRewrite { get { return bAllowRewrite; } set { bAllowRewrite = value; } }

    /// <summary>
    /// Datagrams
    /// </summary>
    // PlayerInfo
    [SerializeField]
    private PlayerSaveData playerSaveData = new PlayerSaveData();

    // Current Day
    [SerializeField]
    private DaySaveData daySaveData = new DaySaveData();

    // NPC metadata
    [SerializeField]
    private List<NPCSaveData> npcSaveDataList = new List<NPCSaveData>();
    public List<NPCSaveData> NpcSaveDataList => npcSaveDataList;

    // "Guild" Data
    [SerializeField]
    private List<GuildSaveData> guildSaveDataList = new List<GuildSaveData>();
    public List<GuildSaveData> GuildSaveDataList => guildSaveDataList;

    // Character Lock Data
    [SerializeField]
    private Character2DLockData character2DLock = new Character2DLockData();
    public Character2DLockData Character2DLock => character2DLock;

    // Temp - HUD Interactions
    [SerializeField]
    private List<int> disabledHUDInteractionList = new List<int>();
    public List<int> DisabledHUDInteractionList => disabledHUDInteractionList;

    // Triggered Tasks
    [SerializeField]
    private List<int> triggeredTaskList = new List<int>();
    public List<int> TriggeredTaskList => triggeredTaskList;

    [SerializeField]
    private List<int> completeTaskList = new List<int>();
    public List<int> CompleteTaskList => completeTaskList;

    // Map Lock
    [SerializeField]
    private List<string> lockedMapList = new List<string>();
    public List<string> LockedMapList => lockedMapList;

    [SerializeField]
    private bool bTeleportLocked = false;

    // Module Lock
    [SerializeField]
    private List<bool> menuModuleLockList = new List<bool>();
    public List<bool> MenuModuleLockList => menuModuleLockList;

    // Game Time
    [SerializeField]
    private GameEvent.Event currentGameTime = GameEvent.Event.TIME_NOON;
    public GameEvent.Event CurrentGameTime => currentGameTime;

    // Special Group - Custom Variables
    private Dictionary<string, bool> globalBoolVariables = new Dictionary<string, bool>();
    public Dictionary<string, bool> GlobalBoolVariables => globalBoolVariables;

    /// <summary>
    /// Data mainMenuoduleLockList
    /// </summary>
    [System.Serializable]
    public class PlayerSaveData
    {
        public string PlayerName;
        public Vector3 Position;
        public string Scene;
    }

    [System.Serializable]
    public class NPCSaveData
    {
        public int NpcID;
        public string NpcName;
        public Vector3 Position;
        public List<int> interactionIDs;
        public string Scene;
        public bool bActive = false;
    }

    [System.Serializable]
    public class DaySaveData
    {
        public int CurrentDay;
        public bool bInited;
    }

    [System.Serializable]
    public class GuildSaveData
    {
        public int GuildID;
        public int Score;
        public int DuelWin;
        public int DuelLose;
        public bool bElminated;
    }

    [System.Serializable]
    public class Character2DLockData
    {
        public List<int> ShooterLevelLock = new List<int>();
        public List<int> PlatformerLevelLock = new List<int>();
    }

    // Methods:
    public void SetPlayer(Vector3 position, string scene)
    {
        playerSaveData.Position = position;
        playerSaveData.Scene = scene;
    }

    public PlayerSaveData GetPlayer()
    {
        return playerSaveData;
    }

    public DaySaveData GetDay()
    {
        return daySaveData;
    }

    public void LockSave()
    {
        bAllowRewrite = false;
    }

    // Modify Day status
    // --Init-- Methods
    public void InitDayToSave()
    {
        daySaveData.CurrentDay = 1;
    }

    // Modify Guild status
    // --Init-- Methods
    public void InitGuildToSave()
    {
        guildSaveDataList.Clear();
        foreach (var pair in GuildInfoData.data)
        {
            GuildInfoData.GuildInfoDataStruct defaultGuild = pair.Value;

            GuildSaveData newGuildSaveData = new GuildSaveData()
            {
                GuildID = defaultGuild.ID,
                Score = 0,
                DuelWin = 0,
                DuelLose = 0,
                bElminated = false
            };

            guildSaveDataList.Add(newGuildSaveData);
        }
    }

    // Modify Hidden HUD Interaction
    // --Init-- Methods
    public void InitHUDInteractionDisableList()
    {
        disabledHUDInteractionList.Clear();
    }

    // Add and remove from this list
    public void EnableHUDInteraction(int interactionID)
    {
        if (disabledHUDInteractionList.Contains(interactionID))
        {
            disabledHUDInteractionList.Remove(interactionID);
        }
    }

    public void DisableHUDInteraction(int interactionID)
    {
        if (!disabledHUDInteractionList.Contains(interactionID))
        {
            disabledHUDInteractionList.Add(interactionID);
        }
    }

    // Modify NPC status
    // Add NPC to Save List
    // --Init-- Methods
    public void InitNPCToSave()
    {
        npcSaveDataList.Clear();
        /*
         * Currently no need to init NPC like this
        foreach (var pair in NPCData.data)
        {
            NPCData.NPCDataStruct defaultNPC = pair.Value;

            NPCSaveData newNPCSaveData = new NPCSaveData()
            {
                NpcID = defaultNPC.ID,
                NpcName = defaultNPC.Name,
                Position = defaultNPC.Position,
                Scene = defaultNPC.Scene,
                bActive = defaultNPC.bActiveByDefault,
                interactionIDs = defaultNPC.DefaultInteractionID != null ? defaultNPC.DefaultInteractionID.ToList() : new List<int>()
            };

            npcSaveDataList.Add(newNPCSaveData);
        }
        */
    }

    // Change NPC in save
    public void ChangeNPCPositionAndScene(int npcID, string sceneName, Vector3 position)
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);

        if (npc != null)
        {
            npc.Scene = sceneName;
            npc.Position = position;
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcID} not found.");
        }
    }

    public void AddNPCToSave(int npcID, string sceneName, Vector3 position)
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);

        if (npc == null)
        {
            NPCData.NPCDataStruct defaultNPC = NPCData.GetData(npcID);

            NPCSaveData newNPCSaveData = new NPCSaveData()
            {
                NpcID = defaultNPC.ID,
                NpcName = defaultNPC.Name,
                Position = position,
                Scene = sceneName,
                bActive = defaultNPC.bActiveByDefault,
                interactionIDs = new List<int>()
            };

            npcSaveDataList.Add(newNPCSaveData);
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcID} already in save.");
        }
    }

    public void RemoveNPCFromSave(int npcID)
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);

        if (npc != null)
        {
            npcSaveDataList.Remove(npc);
        }
    }

    public void ClearAllNPC()
    {
        npcSaveDataList.Clear();
    }

    // Modify character lock states
    // --Init-- Methods
    public void InitCharacter2DToList()
    {
        character2DLock.ShooterLevelLock.Clear();
        character2DLock.PlatformerLevelLock.Clear();
    }

    // Lock or Unlock a 2D character
    public void LockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case (Enums.LEVEL_TYPE.Shooter):
                if (!character2DLock.ShooterLevelLock.Contains(characterID))
                {
                    character2DLock.ShooterLevelLock.Add(characterID);
                }
                break;
            case (Enums.LEVEL_TYPE.Platformer):
                if (!character2DLock.PlatformerLevelLock.Contains(characterID))
                {
                    character2DLock.PlatformerLevelLock.Add(characterID);
                }
                break;
        }
    }

    public void UnlockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case (Enums.LEVEL_TYPE.Shooter):
                if (character2DLock.ShooterLevelLock.Contains(characterID))
                {
                    character2DLock.ShooterLevelLock.Remove(characterID);
                }
                break;
            case (Enums.LEVEL_TYPE.Platformer):
                if (character2DLock.PlatformerLevelLock.Contains(characterID))
                {
                    character2DLock.PlatformerLevelLock.Remove(characterID);
                }
                break;
        }
    }

    private List<int> DeepCopyList(List<int> sourceList)
    {
        List<int> newList = new List<int>();
        foreach (int item in sourceList)
        {
            newList.Add(item);
        }
        return newList;
    }

    // Get the current character lock
    public List<int> GetCurrentCharacterLock(Enums.LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case (Enums.LEVEL_TYPE.Shooter):
                return character2DLock.ShooterLevelLock;
            case (Enums.LEVEL_TYPE.Platformer):
                return character2DLock.PlatformerLevelLock;
        }
        return null;
    }

    // Modify Task lists
    // --Init Methods--
    public void InitTaskLists()
    {
        triggeredTaskList.Clear();
        completeTaskList.Clear();
    }

    // Trigger a Task
    public void TriggerTask(int taskID)
    {
        if (!triggeredTaskList.Contains(taskID) && !completeTaskList.Contains(taskID))
        {
            triggeredTaskList.Add(taskID);
        }
    }

    // Remove a Task that is already triggered
    public void RemoveTriggeredTask(int taskID)
    {
        if (triggeredTaskList.Contains(taskID))
        {
            triggeredTaskList.Remove(taskID);
        }
    }

    // Complete a Task
    public void CompleteTask(int taskID)
    {
        if (triggeredTaskList.Contains(taskID) && !completeTaskList.Contains(taskID))
        {
            triggeredTaskList.Remove(taskID);
            completeTaskList.Add(taskID);
        }
    }

    // Modify Map lock lists
    // --Init Methods--
    public void InitMapLockList()
    {
        lockedMapList.Clear();
    }

    // Lock a map
    public void LockMap(string mapName)
    {
        if (!lockedMapList.Contains(mapName))
        {
            lockedMapList.Add(mapName);
        }
    }

    // Unlock a map
    public void UnlockMap(string mapName)
    {
        if (lockedMapList.Contains(mapName))
        {
            lockedMapList.Remove(mapName);
        }
    }

    // Set map teleport lock 
    public void SetMapTravelLockStatus(bool bLocked)
    {
        bTeleportLocked = bLocked;
    }

    // Get map teleport lock
    public bool CheckMapTravelLockStatus()
    {
        return bTeleportLocked;
    }

    // Modify Interactions
    public void AddInteractionToNPC(int npcID, int interactionID) 
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);
        if (npc != null)
        {
            if (!npc.interactionIDs.Contains(interactionID))
            {
                npc.interactionIDs.Add(interactionID);
            }
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcID} not found.");
        }
    }

    // Modify Menu module options list
    // --Init Methods--
    public void InitMenuModuleLockList()
    {
        menuModuleLockList.Clear();
    }

    // Init module lock via array
    public void InitModuleLock(bool[] lockList)
    {
        menuModuleLockList = new List<bool>(lockList);
    }

    // Lock or unlock a module
    public void SetModuleLock(bool bLock, int moduleID)
    {
        menuModuleLockList[moduleID] = bLock;
    }

    // Modify the in-game time
    public void SetGameTime(GameEvent.Event timeEnum = GameEvent.Event.TIME_NOON)
    {
        currentGameTime = timeEnum;
    }

    // Modify Global Variables
    public void SetGlobalVariable(string _name, bool bTrue)
    {
        if (globalBoolVariables.ContainsKey(_name))
        {
            globalBoolVariables[_name] = bTrue;
        }
        else 
        {
            globalBoolVariables.Add(_name, bTrue);
        }
    }

    public void RemoveInteractionFromNPC(int npcID, int interactionID)
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);
        if (npc != null)
        {
            if (!npc.interactionIDs.Remove(interactionID))
            {
                Debug.Log($"InteractionID {interactionID} not found in NPC with ID {npcID}.");
            }
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcID} not found.");
        }
    }

    // Clear All
    public void RemoveInteractionFromNPC(int npcID)
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);
        if (npc != null)
        {
            npc.interactionIDs.Clear();
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcID} not found.");
        }
    }

    public void SetNPCActive(int npcID, bool bActive)
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);

        if (npc != null)
        {
            npc.bActive = bActive;
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcID} not found.");
        }
    }

    public void ClearNPCSave()
    {
        npcSaveDataList.Clear();
    }
}

