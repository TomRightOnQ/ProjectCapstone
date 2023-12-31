using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scripts marks what will happen on the beginning of Day one
/// ---Caution---
/// This is a hardcoded script
/// </summary>

public class ScriptDayOne : DayScriptBase
{
    // Public:
    public override void Init() 
    {
        // Clear all NPC things...
        SaveConfig.Instance.ClearAllNPC();

        // NPCs...
        NPCManager.Instance.AddNewNPCToSave(1, 2, 1);
        NPCManager.Instance.AddInteractionToNPC(1, 17);
    }
}
