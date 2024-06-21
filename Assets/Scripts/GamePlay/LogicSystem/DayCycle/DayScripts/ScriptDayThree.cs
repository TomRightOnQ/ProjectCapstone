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
        LevelManager.Instance.SetGameTime(GameEvent.Event.TIME_MORNING, true);
        // Move player
        SaveConfig.Instance.SetPlayer(new Vector3(4.35f, 0.6f, -1.4f), Constants.SCENE_ROOMA_LEVEL);

        // Remove potential outdated NPC
        SaveManager.Instance.ClearNPCSave();

        // NPC
        // Add the new connectivity check
        NPCManager.Instance.AddNewNPCToSave(1103, Constants.SCENE_ROOMA_LEVEL, new Vector3(9.303f, 0.44f, -0.268f));
        SaveManager.Instance.SetNPCActive(1103, true);
        NPCManager.Instance.AddInteractionToNPC(1103, 20019);

        // Breakfast
        TaskManager.Instance.UnlockTask(3100);
        // Run first Task
        TaskManager.Instance.UnlockTask(3000);
    }

    // Day 1 Methods:
    public override void ConfigTaskAction(int taskID, bool bPre)
    {
        switch (taskID)
        {
            case 3000:
                task_3_0(bPre);
                break;
            case 3001:
                task_3_1(bPre);
                break;
            case 3100:
                task_3_1_0(bPre);
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
        if (bPre)
        {
            // Place Samantha
            NPCManager.Instance.AddNewNPCToSave(2002, Constants.SCENE_AUDIENCELOW_LEVEL, new Vector3(2.32f, 11.77f, 9.46f));
            SaveManager.Instance.SetNPCActive(2002, true);

            // Two different results based on the previous day
            if (SaveManager.Instance.CheckGlobalVariable("MEET_SAMANTHA"))
            {
                NPCManager.Instance.AddInteractionToNPC(2002, 30000);
            }
            else
            {
                NPCManager.Instance.AddInteractionToNPC(2002, 30001);
            }
            // Save As the beginning of a day
            SaveManager.Instance.SaveGameSave(DayCycleManager.Instance.CurrentDay);
            SaveManager.Instance.SaveCurrentDay(DayCycleManager.Instance.CurrentDay);
            return;
        }
        // Remove the NPC non instantly
        NPCManager.Instance.RemoveInteractionFromNPC(2002, 30000);
        NPCManager.Instance.RemoveInteractionFromNPC(2002, 30001);
        NPCManager.Instance.RemoveNPCFromSave(2002);

        // Unlock the badge
        SaveManager.Instance.AddNote(Enums.NOTE_TYPE.Item, new int[] { 10103 });
        TaskManager.Instance.UnlockTask(3001);
    }

    // Bering the connector
    public void task_3_1(bool bPre)
    {
        if (bPre)
        {
            // Place Dove and Samantha
            NPCManager.Instance.AddNewNPCToSave(1001, Constants.SCENE_ENTRANCE_LEVEL, new Vector3(13.725f, 0.68f, 0.67f));
            SaveManager.Instance.SetNPCActive(1001, true);
            NPCManager.Instance.AddInteractionToNPC(1001, 30002);

            NPCManager.Instance.AddNewNPCToSave(1008, Constants.SCENE_ENTRANCE_LEVEL, new Vector3(5.725f, 0.68f, 0.67f));
            SaveManager.Instance.SetNPCActive(1008, true);
            NPCManager.Instance.AddInteractionToNPC(1008, 30003);

            return;
        }
        // Remove the NPC non instantly
        NPCManager.Instance.RemoveInteractionFromNPC(2002, 30000);
        NPCManager.Instance.RemoveInteractionFromNPC(2002, 30001);
        NPCManager.Instance.RemoveNPCFromSave(1001);
        NPCManager.Instance.RemoveNPCFromSave(1008);
        // Take player to the control room and turn off the map
        SaveManager.Instance.InitModuleLock(new bool[] { false, true, false, true, false, false });
        LevelManager.Instance.LoadScene(Constants.SCENE_CONTROL_LEVEL);
    }

    // Breakfast
    public void task_3_1_0(bool bPre)
    {
        if (bPre)
        {
            NPCManager.Instance.AddNewNPCToSave(2200, Constants.SCENE_GUILD_LEVEL, new Vector3(8.2f, 0.8f, 0.6f));
            SaveManager.Instance.SetNPCActive(2200, true);
            // Add to the trigger
            NPCManager.Instance.AddInteractionToNPC(2200, 31000);
            // Save As the beginning of a day
            SaveManager.Instance.SaveGameSave(DayCycleManager.Instance.CurrentDay);
            SaveManager.Instance.SaveCurrentDay(DayCycleManager.Instance.CurrentDay);
            return;
        }
        ReminderManager.Instance.ShowGeneralReminder(":>", 3f);
        SaveManager.Instance.SetGlobalVariable("FOOD_TAKEN_DAY3", true);
        NPCManager.Instance.RemoveInteractionFromNPC(2200, 31000);
    }
}
