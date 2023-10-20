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
    private List<NPCSaveData> npcSaveDataList = new List<NPCSaveData>();
    public List<NPCSaveData> NpcSaveDataList => npcSaveDataList;

    /// Day System
    [SerializeField] private int currentDay = 0;
    public int CurrentDay { get { return currentDay; } set { currentDay = value; } }

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

    public void LockSave()
    {
        bAllowRewrite = false;
    }

    // Modify Day status
    // --Init-- Methods
    public void InitDayToSave()
    {
        currentDay = 0;
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

