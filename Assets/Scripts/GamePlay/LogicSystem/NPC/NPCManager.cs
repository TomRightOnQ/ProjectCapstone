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

    // Save the references of all instantiated NPCs
    private Dictionary<int, NPCUnit> npcMap = new Dictionary<int, NPCUnit>();
    public Dictionary<int, NPCUnit> NpcMap => npcMap;

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

    // Private:
    // Change an NPC's interaction list
    private void addInteractionToNPCInField(int npcID, int interactionID)
    {
        if (npcMap.ContainsKey(npcID))
        {
            NPCUnit npc = npcMap[npcID];
            if (npc != null)
            {
                HUDInteractionManager.Instance.AddInteraction(npc, interactionID);
            }
        }
    }

    private void removeInteractionFromNPCInField(int npcID, int interactionID)
    {
        if (npcMap.ContainsKey(npcID))
        {
            NPCUnit npc = npcMap[npcID];
            HUDInteractionManager.Instance.RemoveInteraction(npc, interactionID);
            HUDInteractionManager.Instance.RemoveInteractionFromUIList(interactionID);
        }
    }

    // Public:
    public void Init()
    {
        npcMap.Clear();
    }

    // Spawn an NPC
    public void SpawnNPC(int npcID)
    {
        NPCData.NPCDataStruct npcData = NPCData.GetData(npcID);
        SpawnNPC(npcID, npcData.Position, Quaternion.identity);
    }

    public void SpawnNPC(int npcID, Vector3 position, Quaternion rotation)
    {
        NPCData.NPCDataStruct npcData = NPCData.GetData(npcID);
        GameObject npcObject = PrefabManager.Instance.Instantiate(npcData.Name, position, rotation);
        if (npcObject != null)
        {
            NPCUnit npc = npcObject.GetComponent<NPCUnit>();
            npcMap[npcID] = npc;
            npc.SetNPC(npcID);
        }
    }

    public void SpawnNPC(SaveConfig.NPCSaveData npcData)
    {
        GameObject npcObject = PrefabManager.Instance.Instantiate(npcData.NpcName, npcData.Position, Quaternion.identity);
        if (npcObject != null)
        {
            NPCUnit npc = npcObject.GetComponent<NPCUnit>();
            npcMap[npcData.NpcID] = npc;
            npc.SetNPC(npcData);
        }
    }

    public void DespawnNPC(int npcID)
    {
        if (npcMap.ContainsKey(npcID))
        {
            GameObject npcObject = npcMap[npcID].gameObject;
            Destroy(npcObject);
            npcMap.Remove(npcID);
        }
    }

    // Modify NPCs in save
    public void AddNewNPCToSave(int npcID, int sceneID, int positionID)
    {
        Vector3 position = Vector3PositionData.GetData(positionID).Position;
        string sceneName = LevelConfig.Instance.GetLevelData(sceneID).SceneName;
        SaveManager.Instance.AddNPCToSave(npcID, sceneName, position);
    }

    public void AddNewNPCToSave(int npcID, string sceneName, Vector3 NPCPosition)
    {

        SaveManager.Instance.AddNPCToSave(npcID, sceneName, NPCPosition);
    }

    public void RemoveNPCFromSave(int npcID)
    {
        SaveManager.Instance.RemoveNPCFromSave(npcID);
    }

    public void AddInteractionToNPC(int npcID, int interactionID)
    {
        SaveManager.Instance.AddInteractionToNPC(npcID, interactionID);
        addInteractionToNPCInField(npcID, interactionID);
    }

    public void RemoveInteractionFromNPC(int npcID, int interactionID)
    {
        SaveManager.Instance.RemoveInteractionFromNPC(npcID, interactionID);
        removeInteractionFromNPCInField(npcID, interactionID);
    }

    public void ChangeNPCPositionAndScene(int npcID, string sceneName, Vector3 position)
    {
        SaveManager.Instance.ChangeNPCPositionAndScene(npcID, sceneName, position);
    }

    public void ChangeNPCPositionAndScene(int npcID, int sceneID, int positionID)
    {
        Vector3 position = Vector3PositionData.GetData(positionID).Position;
        string sceneName = LevelConfig.Instance.GetLevelData(sceneID).SceneName;
        SaveManager.Instance.ChangeNPCPositionAndScene(npcID, sceneName, position);
    }

    public void SetNPCActive(int npcID, bool bActive)
    {
        SaveManager.Instance.SetNPCActive(npcID, bActive);
    }
}
