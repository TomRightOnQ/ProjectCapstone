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
    public void Init()
    {
    
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
            npc.SetNPC(npcID);
        }
    }
}
