using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPCs
/// </summary>
public class NPCUnit : EUnit
{
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
}
