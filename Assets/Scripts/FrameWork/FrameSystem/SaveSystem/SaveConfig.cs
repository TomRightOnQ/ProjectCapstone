using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveSystem/SaveConfig")]
public class SaveConfig : ScriptableSingleton<SaveConfig>
{
    // Control flag
    [SerializeField] private bool bAllowRewrite = true;
    public bool AllowRewrite { get { return bAllowRewrite; } }

    // Data
    [SerializeField]
    private PlayerSaveData playerSaveData = new PlayerSaveData();

    [SerializeField]
    private DaySaveData daySaveData = new DaySaveData();

    [SerializeField]
    private List<NPCSaveData> npcSaveDataList = new List<NPCSaveData>();
    public List<NPCSaveData> NpcSaveDataList => npcSaveDataList;

    [SerializeField]
    private List<GuildSaveData> guildSaveDataList = new List<GuildSaveData>();
    public List<GuildSaveData> GuildSaveDataList => guildSaveDataList;

    [SerializeField]
    private NoteData noteData = new NoteData();

    [SerializeField]
    private List<Character2DLockData> character2DLockList = new List<Character2DLockData>();
    public List<Character2DLockData> Character2DLockList => character2DLockList;

    // Datagrams
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
        public int MaxDay;
        public bool bInited;
    }

    [System.Serializable]
    public class NoteData
    {
        public List<int> NoteIDs;
        public List<int> ItemIDs;
        public List<int> ReportIDs;
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
        public List<int> ShooterLevelLock;
        public List<int> PlatformerLevelLock;
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

    public List<int> GetNote(Enums.NOTE_TYPE type)
    {
        switch (type)
        {
            case Enums.NOTE_TYPE.Note:
                return noteData.NoteIDs;
            case Enums.NOTE_TYPE.Item:
                return noteData.ItemIDs;
            case Enums.NOTE_TYPE.Report:
                return noteData.ReportIDs;
            default:
                return noteData.NoteIDs;
        }
    }

    public void AddNote(Enums.NOTE_TYPE type, int[] IDs)
    {
        switch (type) 
        {
            case Enums.NOTE_TYPE.Note:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.NoteIDs.Contains(IDs[i]))
                    {
                        noteData.NoteIDs.Add(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Item:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.ItemIDs.Contains(IDs[i]))
                    {
                        noteData.ItemIDs.Add(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Report:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.ReportIDs.Contains(IDs[i]))
                    {
                        noteData.ReportIDs.Add(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            default:
                break;
        }
    }

    public void RemoveNote(Enums.NOTE_TYPE type, int[] IDs)
    {
        switch (type)
        {
            case Enums.NOTE_TYPE.Note:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.NoteIDs.Contains(IDs[i]))
                    {
                        noteData.NoteIDs.Remove(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Item:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.ItemIDs.Contains(IDs[i]))
                    {
                        noteData.ItemIDs.Remove(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Report:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.ReportIDs.Contains(IDs[i]))
                    {
                        noteData.ReportIDs.Remove(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            default:
                break;
        }
    }

    public void LockSave()
    {
        bAllowRewrite = false;
    }

    // Modify Day status
    // --Init-- Methods
    public void InitDayToSave()
    {
        daySaveData.CurrentDay = 0;
        daySaveData.MaxDay = 0;
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

    // Modify NPC status
    // Add NPC to Save List
    // --Init-- Methods
    public void InitNPCToSave()
    {
        //npcSaveDataList.Clear();
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
    }

    // Modify character lock states
    // --Init-- Methods
    public void InitCharacter2DToList()
    {
        character2DLockList.Clear();
        for (int i = 0; i < 7; i++)
        {
            character2DLockList.Add(new Character2DLockData());
        }
    }

    // Lock or Unlock a 2D character
    public void LockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case (Enums.LEVEL_TYPE.Shooter):
                if (!character2DLockList[daySaveData.CurrentDay].ShooterLevelLock.Contains(characterID))
                {
                    character2DLockList[daySaveData.CurrentDay].ShooterLevelLock.Add(characterID);
                }
                break;
            case (Enums.LEVEL_TYPE.Platformer):
                if (!character2DLockList[daySaveData.CurrentDay].PlatformerLevelLock.Contains(characterID))
                {
                    character2DLockList[daySaveData.CurrentDay].PlatformerLevelLock.Add(characterID);
                }
                break;
        }
    }

    public void UnlockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case (Enums.LEVEL_TYPE.Shooter):
                if (character2DLockList[daySaveData.CurrentDay].ShooterLevelLock.Contains(characterID))
                {
                    character2DLockList[daySaveData.CurrentDay].ShooterLevelLock.Remove(characterID);
                }
                break;
            case (Enums.LEVEL_TYPE.Platformer):
                if (character2DLockList[daySaveData.CurrentDay].PlatformerLevelLock.Contains(characterID))
                {
                    character2DLockList[daySaveData.CurrentDay].PlatformerLevelLock.Remove(characterID);
                }
                break;
        }
    }

    // Set the data to the begining of a day
    public void SetCharacter2DLockToDay(int currentDay)
    {
        for (int i = currentDay; i < 7; i++)
        {
            character2DLockList[i].ShooterLevelLock = new List<int>();
            character2DLockList[i].PlatformerLevelLock = new List<int>();
        }
        if (currentDay >= 1)
        {
            character2DLockList[currentDay].ShooterLevelLock = DeepCopyList(character2DLockList[currentDay - 1].ShooterLevelLock);
            character2DLockList[currentDay].PlatformerLevelLock = DeepCopyList(character2DLockList[currentDay - 1].PlatformerLevelLock);
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
    public List<int> GetCurrentCharacterLock(int currentDay, Enums.LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case (Enums.LEVEL_TYPE.Shooter):
                return character2DLockList[currentDay].ShooterLevelLock;
            case (Enums.LEVEL_TYPE.Platformer):
                return character2DLockList[currentDay].PlatformerLevelLock;
        }
        return null;
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

    public void RemoveInteractionFromNPC(int npcID, int interactionID)
    {
        var npc = npcSaveDataList.FirstOrDefault(n => n.NpcID.GetHashCode() == npcID);
        if (npc != null)
        {
            if (!npc.interactionIDs.Remove(interactionID))
            {
                Debug.LogWarning($"InteractionID {interactionID} not found in NPC with ID {npcID}.");
            }
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcID} not found.");
        }
    }

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
}

