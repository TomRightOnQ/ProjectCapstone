using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scripts marks what will happen on the beginning of Day one
/// ---Caution---
/// This is a hardcoded script
/// </summary>

public class ScriptDayZero : DayScriptBase
{
    // Public:
    public override void Init() 
    {
        // NPCs
        NPCManager.Instance.RemoveInteractionFromNPC(1, 8);
        NPCManager.Instance.AddInteractionToNPC(1, 19);
    }
}
