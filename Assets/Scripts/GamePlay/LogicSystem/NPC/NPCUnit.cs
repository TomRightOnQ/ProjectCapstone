using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPCs
/// </summary>
public class NPCUnit : EUnit
{
    [SerializeField, ReadOnly]
    private int npcID;

    protected override void Awake()
    {
        base.Awake();
        gameObject.tag = "NPC";
    }

    // Private:
    // HUD Interact
    public override void HUDInteract()
    {
        ChangeFacing(!GetFacingToRight());
    }

    // Public:
    public void SetNPC(int id)
    {
        npcID = id;
        NPCData.NPCDataStruct npcData = NPCData.GetData(npcID);
        interactionTrigger.EnableTrigger();
        for (int i = 0; i < npcData.DefaultInteractionID.Length; i++)
        {
            interactionTrigger.SetUpTrigger(npcData.DefaultInteractionID[i]);
        }
    }

    public void SetNPC(SaveConfig.NPCSaveData npcData)
    {
        npcID = npcData.NpcID;
        interactionTrigger.EnableTrigger();
        for (int i = 0; i < npcData.interactionIDs.Count; i++)
        {
            interactionTrigger.SetUpTrigger(npcData.interactionIDs[i]);
        }
    }

    // Facing the Player
    public void FacingPlayer()
    {
        if (PersistentDataManager.Instance.MainPlayer.transform.position.x > transform.position.x)
        {
            ChangeFacing(true);
        }
        else 
        {
            ChangeFacing(false);
        }
    }
}
