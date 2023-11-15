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
    private NoteData noteData = new NoteData();

    [SerializeField]
    private List<GuildSaveData> guildSaveDataList = new List<GuildSaveData>();
    public List<GuildSaveData> GuildSaveDataList => guildSaveDataList;

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
                        noteData.NoteIDs.Add(i);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Item:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.ItemIDs.Contains(IDs[i]))
                    {
                        noteData.ItemIDs.Add(i);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Report:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.ReportIDs.Contains(IDs[i]))
                    {
                        noteData.ReportIDs.Add(i);
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
                        noteData.NoteIDs.Remove(i);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Item:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.ItemIDs.Contains(IDs[i]))
                    {
                        noteData.ItemIDs.Remove(i);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Report:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.ReportIDs.Contains(IDs[i]))
                    {
                        noteData.ReportIDs.Remove(i);
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
        npcSaveDataList.Clear();
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

    // Only public to SaveManager
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

