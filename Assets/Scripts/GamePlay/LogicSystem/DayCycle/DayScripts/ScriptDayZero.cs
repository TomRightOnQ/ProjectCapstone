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
        // Config NPCs
        NPCManager.Instance.AddInteractionToNPC(2, 2);
    }
}
