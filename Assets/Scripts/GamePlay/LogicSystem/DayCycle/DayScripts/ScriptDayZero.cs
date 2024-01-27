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

    }

    // Day 0 Methods:
    public override void ConfigTaskAction(int taskID, bool bPre)
    {
        switch (taskID)
        {
            case 0:
                task_0_0(bPre);
                break;
            default:
                break;
        }
    }

    // Private:
    // Task Actions
    public void task_0_0(bool bPre)
    {
        if (bPre)
        {
            return;
        }

    }
}
