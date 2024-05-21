using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scripts marks what will happen on the beginning of Day two
/// ---Caution---
/// This is a hardcoded script
/// </summary>
public class ScriptDayTwo : DayScriptBase
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
        TaskManager.Instance.UnlockTask(2000);

        // NPC
        // Remove the old connectivity check
        NPCManager.Instance.ClearInteractionFromNPC(1103);
        NPCManager.Instance.AddInteractionToNPC(1103, 20001);
        // Remove old NPCs
        NPCManager.Instance.RemoveNPCFromSave(1003);
    }

    // Day 1 Methods:
    public override void ConfigTaskAction(int taskID, bool bPre)
    {
        switch (taskID)
        {
            case 2000:
                task_2_0(bPre);
                break;
            case 2001:
                task_2_1(bPre);
                break;
            case 2002:
                task_2_2(bPre);
                break;
            case 2003:
                task_2_3(bPre);
                break;
            case 2004:
                task_2_4(bPre);
                break;
            case 2005:
                task_2_5(bPre);
                break;
            case 2100:
                task_2_1_0(bPre);
                break;
            case 2101:
                task_2_1_1(bPre);
                break;
            case 2102:
                task_2_1_2(bPre);
                break;
            case 2103:
                task_2_1_3(bPre);
                break;
            case 2104:
                task_2_1_4(bPre);
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
        ReminderManager.Instance.ShowWholeScreenReminder(10002);
    }

    // Task Actions
    // Talk With Guide in Team Room
    public void task_2_0(bool bPre)
    {
        if (bPre)
        {
            // Place the guide
            NPCManager.Instance.AddNewNPCToSave(1000, Constants.SCENE_ROOMA_LEVEL, new Vector3(4.95f, 0.65f, -1.4f));
            SaveManager.Instance.SetNPCActive(1000, true);
            // Gives interaction to the guide
            NPCManager.Instance.AddInteractionToNPC(1000, 20000);
            return;
        }
        // Change NPCs
        NPCManager.Instance.RemoveInteractionFromNPC(1000, 20000);
        NPCManager.Instance.DespawnNPC(1000);
        TaskManager.Instance.UnlockTask(2001);
    }

    // Talk with Guide in Tarvern
    public void task_2_1(bool bPre)
    {
        if (bPre)
        {
            // Place the guide
            NPCManager.Instance.ChangeNPCPositionAndScene(1000, Constants.SCENE_GUILD_LEVEL, new Vector3(2.39f, 0.62f, -0.21f));
            SaveManager.Instance.SetNPCActive(1000, true);

            // Place Other NPCs
            NPCManager.Instance.AddNewNPCToSave(1004, Constants.SCENE_GUILD_LEVEL, new Vector3(-1.13f, 0.6f, 4.6f));
            SaveManager.Instance.SetNPCActive(1004, true);
            NPCManager.Instance.AddNewNPCToSave(1007, Constants.SCENE_GUILD_LEVEL, new Vector3(-0.47f, 0.59f, 4.11f));
            SaveManager.Instance.SetNPCActive(1007, true);

            // Unlock the catowner objective
            TaskManager.Instance.UnlockTask(2102);

            // Two different results based on the previous day
            if (SaveManager.Instance.CheckGlobalVariable("DAY_1_TALKED_WITH_WEIRD_GUY"))
            {
                NPCManager.Instance.AddInteractionToNPC(1000, 20002);
            }
            else 
            {
                NPCManager.Instance.AddInteractionToNPC(1000, 20008);
            }

            // Add interaction to Darlan
            NPCManager.Instance.AddInteractionToNPC(1004, 20010);
            return;
        }
        // Clear all interactions
        NPCManager.Instance.RemoveInteractionFromNPC(1000, 20002);
        NPCManager.Instance.RemoveInteractionFromNPC(1000, 20008);
        // Remove NPCs
        NPCManager.Instance.RemoveNPCFromSave(1000);
        NPCManager.Instance.RemoveNPCFromSave(1004);
        NPCManager.Instance.RemoveNPCFromSave(1007);
        // Unlock Conversation with Dove and the hidden text to future
        TaskManager.Instance.UnlockTask(2100);
        TaskManager.Instance.UnlockTask(2101);
        TaskManager.Instance.UnlockTask(2002);
    }

    // Talk with Dove
    public void task_2_2(bool bPre)
    {
        if (bPre)
        {
            // Place the NPC
            NPCManager.Instance.AddNewNPCToSave(1001, Constants.SCENE_AUDIENCE_LEVEL, new Vector3(-3.9f, 7.28f, -1.65f));
            SaveManager.Instance.SetNPCActive(1001, true);
            NPCManager.Instance.AddInteractionToNPC(1001, 20009);

            // For later interaction
            NPCManager.Instance.AddNewNPCToSave(1007, Constants.SCENE_AUDIENCE_LEVEL, new Vector3(-2.1f, 7.28f, -1.65f));
            SaveManager.Instance.SetNPCActive(1007, true);
            return;
        }
        NPCManager.Instance.RemoveInteractionFromNPC(1001, 20009);
        NPCManager.Instance.RemoveNPCFromSave(1001);
        TaskManager.Instance.UnlockTask(2003);
    }

    // Match with suggestion
    public void task_2_3(bool bPre)
    {
        if (bPre)
        {
            // There is no food!
            TaskManager.Instance.DeleteTask(2100);
            NPCManager.Instance.RemoveInteractionFromNPC(2200, 21000);
            // Add interaction
            NPCManager.Instance.AddInteractionToNPC(1007, 20011);
            return;
        }
        // Lock Map
        SaveManager.Instance.SetModuleLock(true, 3);
        NPCManager.Instance.RemoveNPCFromSave(1007);
        // Move player
        SaveConfig.Instance.SetPlayer(new Vector3(16.25f, 0.6f, -1.4f), Constants.SCENE_ROOMA_LEVEL);
        TaskManager.Instance.UnlockTask(2004);
    }

    public void task_2_4(bool bPre)
    {
        if (bPre)
        {
            // Add all NPCs
            NPCManager.Instance.AddNewNPCToSave(1004, Constants.SCENE_ROOMA_LEVEL, new Vector3(14f, 0.65f, 1f));
            SaveManager.Instance.SetNPCActive(1004, true);
            NPCManager.Instance.AddNewNPCToSave(1005, Constants.SCENE_ROOMA_LEVEL, new Vector3(15.65f, 0.65f, 1f));
            SaveManager.Instance.SetNPCActive(1005, true);
            NPCManager.Instance.AddNewNPCToSave(1006, Constants.SCENE_ROOMA_LEVEL, new Vector3(14.49f, 0.6f, 1.26f));
            SaveManager.Instance.SetNPCActive(1006, true);
            NPCManager.Instance.AddNewNPCToSave(1007, Constants.SCENE_ROOMA_LEVEL, new Vector3(13.39f, 0.62f, 0.64f));
            SaveManager.Instance.SetNPCActive(1007, true);

            NPCManager.Instance.AddInteractionToNPC(1007, 20012);
            return;
        }
        // Set Time
        LevelManager.Instance.SetGameTime(GameEvent.Event.TIME_SUNSET, true);
        // Unlock Map
        SaveManager.Instance.SetModuleLock(false, 3);
        // Remove NPCs
        NPCManager.Instance.RemoveInteractionFromNPC(1007, 20012);
        NPCManager.Instance.RemoveNPCFromSave(1004);
        NPCManager.Instance.RemoveNPCFromSave(1005);
        NPCManager.Instance.RemoveNPCFromSave(1006);
        NPCManager.Instance.RemoveNPCFromSave(1007);

        NPCManager.Instance.DespawnNPC(1004);
        NPCManager.Instance.DespawnNPC(1005);
        NPCManager.Instance.DespawnNPC(1006);
        NPCManager.Instance.DespawnNPC(1007);
        // Subtitle
        // Hide UI seconds
        HUDManager.Instance.HideAllHUD();
        InputManager.Instance.LockInput();

        ReminderManager.Instance.ShowWholeScreenReminder(0);
        ReminderManager.Instance.ShowSubtitleReminder(new int[] { 26, 27, 28, 29});

        Invoke("resumeUI", 6f);

        // Change time
        LevelManager.Instance.SetGameTime(GameEvent.Event.TIME_NIGHT, true);
        TaskManager.Instance.UnlockTask(2005);
        TaskManager.Instance.UnlockTask(2104);
    }

    private void resumeUI()
    {
        InputManager.Instance.UnLockInput(Enums.SCENE_TYPE.World);
        HUDManager.Instance.ShowAllHUD();
    }

    public void task_2_5(bool bPre)
    {
        if (bPre)
        {
            // Spawn the bed trigger
            NPCManager.Instance.AddNewNPCToSave(1102, Constants.SCENE_ROOMA_LEVEL, new Vector3(4.35f, 0.6f, -1.4f));
            NPCManager.Instance.SpawnNPC(1102, new Vector3(4.35f, 0.6f, -1.4f), Quaternion.identity);
            NPCManager.Instance.AddInteractionToNPC(1102, 20013);
            SaveManager.Instance.SetNPCActive(1102, true);
            return;
        }
        NPCManager.Instance.RemoveNPCFromSave(1102);
        DayCycleManager.Instance.GoToNextDay();
    }

    // Breakfast
    public void task_2_1_0(bool bPre)
    {
        if (bPre)
        {
            NPCManager.Instance.AddNewNPCToSave(2200, Constants.SCENE_GUILD_LEVEL, new Vector3(8.2f, 0.8f, 0.6f));
            SaveManager.Instance.SetNPCActive(2200, true);
            NPCManager.Instance.SpawnNPC(2200, new Vector3(8.2f, 0.8f, 0.6f), Quaternion.identity);
            // Add to the trigger
            NPCManager.Instance.AddInteractionToNPC(2200, 21000);
            return;
        }
        ReminderManager.Instance.ShowGeneralReminder(":>", 3f);
        SaveManager.Instance.SetGlobalVariable("FOOD_TAKEN_DAY2", true);
        NPCManager.Instance.RemoveInteractionFromNPC(2200, 21000);
    }

    // Text Future
    public void task_2_1_1(bool bPre)
    {
        if (bPre)
        {
            return;
        }
        // Unlock Achievement
        SaveManager.Instance.SetGlobalVariable("TEXT_TO_FUTURE", true);
        GameEndManager.Instance.UnlockAhievements(3);
    }

    // Talk with the cat owner
    public void task_2_1_2(bool bPre)
    {
        if (bPre)
        {
            // Place the Cat with its default interaction
            NPCManager.Instance.AddNewNPCToSave(2000, Constants.SCENE_GUILD_LEVEL, new Vector3(5.57f, 1.17f, 0.31f));
            SaveManager.Instance.SetNPCActive(2000, true);
            NPCManager.Instance.AddInteractionToNPC(2000, 20014);


            // Spawn the cat owner
            NPCManager.Instance.AddNewNPCToSave(2001, Constants.SCENE_GUILD_LEVEL, new Vector3(6.57f, 0.59f, 0f));
            SaveManager.Instance.SetNPCActive(2001, true);
            NPCManager.Instance.AddInteractionToNPC(2001, 20017);
            return;
        }
        // Unlock the talk with the cat
        TaskManager.Instance.UnlockTask(2103);
        // Unlock the collar
        SaveManager.Instance.AddNote(Enums.NOTE_TYPE.Note, new int[] { 10101 });
    }

    // Talk with the cat
    public void task_2_1_3(bool bPre)
    {
        if (bPre)
        {
            // Remove the cat owner'sinteraction and from the save
            NPCManager.Instance.RemoveInteractionFromNPC(2001, 20017);
            NPCManager.Instance.RemoveNPCFromSave(2001);

            // Change interaction of the Cat
            // Rmeove the default interaction
            NPCManager.Instance.RemoveInteractionFromNPC(2000, 20014);
            NPCManager.Instance.AddInteractionToNPC(2000, 20015);
            return;
        }

        // Remove the interaction and set to a new default
        NPCManager.Instance.RemoveInteractionFromNPC(2000, 20015);
        NPCManager.Instance.AddInteractionToNPC(2000, 20016);
    }

    // Talk with Samantha
    public void task_2_1_4(bool bPre)
    {
        if (bPre)
        {
            // Spawn NPC
            NPCManager.Instance.AddNewNPCToSave(2002, Constants.SCENE_MATCHING_LEVEL, new Vector3(10.55f, 3.58f, 1f));
            SaveManager.Instance.SetNPCActive(2002, true);
            NPCManager.Instance.AddInteractionToNPC(2002, 20018);
            return;
        }

        // Remove the NPC non instantly
        NPCManager.Instance.RemoveInteractionFromNPC(2002, 20018);
        NPCManager.Instance.RemoveNPCFromSave(2002);

        // Record 
        SaveManager.Instance.SetGlobalVariable("MEET_SAMANTHA", true);
    }
}
