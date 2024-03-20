using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This scripts marks what will happen on the beginning of Day three
/// ---Caution---
/// This is a hardcoded script
/// </summary>
public class ScriptDayThree : DayScriptBase
{
    // Public:
    public override void Init()
    {
        // Disable Flash Back
        SaveManager.Instance.SetModuleLock(true, 1);
        // Set time
        LevelManager.Instance.SetGameTime(GameEvent.Event.TIME_NOON, true);
        // Move player
        SaveConfig.Instance.SetPlayer(new Vector3(4.35f, 0.6f, -1.4f), Constants.SCENE_ROOMA_LEVEL);
        // Run first Task
        TaskManager.Instance.UnlockTask(3000);

        // NPC
        // Remove the old connectivity check
        NPCManager.Instance.ClearInteractionFromNPC(1103);
        // NPCManager.Instance.AddInteractionToNPC(1103, 20001);
    }

    // Day 1 Methods:
    public override void ConfigTaskAction(int taskID, bool bPre)
    {
        switch (taskID)
        {
            case 3000:
                task_3_0(bPre);
                break;
            default:
                break;
        }
    }

    // Private:
    // Beginning Actions
    public override void BeginningAction()
    {
        // Whole Screen Subtitle
        ReminderManager.Instance.ShowWholeScreenReminder(10003);
    }

    // Task Actions
    public void task_3_0(bool bPre)
    {
        
    }
}
