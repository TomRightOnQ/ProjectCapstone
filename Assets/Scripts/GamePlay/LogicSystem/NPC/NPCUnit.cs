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

    void Awake()
    {
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
}
