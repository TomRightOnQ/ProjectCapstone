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
        // Set time
        LevelManager.Instance.SetGameTime(GameEvent.Event.TIME_NOON, true);
        // Move player
        SaveConfig.Instance.SetPlayer(new Vector3(4.35f, 0.6f, -1.4f), Constants.SCENE_ROOMA_LEVEL);
        // Disable all Menu UIs
        SaveManager.Instance.InitModuleLock(new bool[] { true, true, true, true, true, false }) ;
        // Run first Task
        TaskManager.Instance.UnlockTask(1000);
    }

    // Day 1 Methods:
    public override void ConfigTaskAction(int taskID, bool bPre)
    {
        switch (taskID)
        {
            case 1000:
                task_1_0(bPre);
                break;
            case 1001:
                task_1_1(bPre);
                break;
            case 1002:
                task_1_2(bPre);
                break;
            case 1003:
                task_1_3(bPre);
                break;
            case 1013:
                task_1_3_b(bPre);
                break;
            case 1004:
                task_1_4(bPre);
                break;
            case 1014:
                task_1_4_b(bPre);
                break;
            case 1005:
                task_1_5(bPre);
                break;
            case 1006:
                task_1_6(bPre);
                break;
            case 1100:
                task_1_1_0(bPre);
                break;
            case 1101:
                task_1_1_1(bPre);
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
        ReminderManager.Instance.ShowWholeScreenReminder(new int[] { 17, 18, 19, 20, 21, 0, 10001},
                                                         new float[] { 5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f});
    }
    // Task Actions
    public void task_1_0(bool bPre)
    {
        if (bPre)
        {
            // Place the guide
            NPCManager.Instance.AddNewNPCToSave(1000, Constants.SCENE_ROOMA_LEVEL, new Vector3(4.75f, 0.6f, -1.4f));
            SaveManager.Instance.SetNPCActive(1000, true);
            // Gives interaction to the guide
            NPCManager.Instance.AddInteractionToNPC(1000, 10000);
            return;
        }
        ReminderManager.Instance.ShowGeneralReminder(1);
        ReminderManager.Instance.ShowSubtitleReminder(new int[] { 22, 23 });
        NPCManager.Instance.RemoveInteractionFromNPC(1000, 10000);
        NPCManager.Instance.AddInteractionToNPC(1000, 10001);
        TaskManager.Instance.UnlockTask(1001);
    }
    public void task_1_1(bool bPre)
    {
        if (bPre)
        {
            // Place the rules
            NPCManager.Instance.AddNewNPCToSave(1100, Constants.SCENE_ROOMA_LEVEL, new Vector3(6.191f, 1.22f, 1.077f));
            NPCManager.Instance.SpawnNPC(1100, new Vector3(6.191f, 1.22f, 1.077f), Quaternion.identity);
            SaveManager.Instance.SetNPCActive(1100, true);
            NPCManager.Instance.AddInteractionToNPC(1100, 10002);
            return;
        }
        TaskManager.Instance.UnlockTask(1002);
    }

    public void task_1_2(bool bPre)
    {
        if (bPre)
        {
            // Player talk to self
            ChatInteractionManager.Instance.BeginInteraction(10012);
            // Place the connection check and trigger to exit
            NPCManager.Instance.AddNewNPCToSave(1200, Constants.SCENE_ROOMA_LEVEL, new Vector3(17.35f, 0.12f, 2.75f));
            NPCManager.Instance.SpawnNPC(1200, new Vector3(17.35f, 0.12f, 2.75f), Quaternion.identity);
            SaveManager.Instance.SetNPCActive(1200, true);
            NPCManager.Instance.AddNewNPCToSave(1103, Constants.SCENE_ROOMA_LEVEL, new Vector3(9.303f, 0.44f, -0.268f));
            NPCManager.Instance.SpawnNPC(1103, new Vector3(9.303f, 0.44f, -0.268f), Quaternion.identity);
            SaveManager.Instance.SetNPCActive(1103, true);
            NPCManager.Instance.AddInteractionToNPC(1103, 10003);
            return;
        }
        // Move Player to outsie
        TaskManager.Instance.UnlockTask(1003);
        // Remove exit door trigger
        NPCManager.Instance.RemoveNPCFromSave(1200);
        LevelManager.Instance.LoadScene(Constants.SCENE_ENTRANCE_LEVEL);
    }

    public void task_1_3(bool bPre)
    {
        if (bPre)
        {
            // Place NPC2
            NPCManager.Instance.AddNewNPCToSave(1001, Constants.SCENE_ENTRANCE_LEVEL, new Vector3(13.725f, 0.68f, 0.67f));
            SaveManager.Instance.SetNPCActive(1001, true);
            NPCManager.Instance.AddInteractionToNPC(1001, 10005);
            return;
        }
        // Enable Note system
        SaveManager.Instance.InitModuleLock(new bool[] { false, true, false, true, false, false });
        SaveManager.Instance.AddNote(Enums.NOTE_TYPE.Note, new int[] { 10000 });
        SaveManager.Instance.AddNote(Enums.NOTE_TYPE.Note, new int[] { 3 });
        ReminderManager.Instance.ShowSubtitleReminder(new int[] { 24, 25 });
        ReminderManager.Instance.ShowGeneralReminder(4);
        HUDInteractionManager.Instance.DisableHUDInteractionUI();
        HUDManager.Instance.HideAllHUD();
        TaskManager.Instance.UnlockTask(1013);
    }

    public void task_1_3_b(bool bPre)
    {
        if (bPre)
        {
            NPCManager.Instance.RemoveInteractionFromNPC(1001, 10005);
            NPCManager.Instance.AddInteractionToNPC(1001, 10013);
            return;
        }
        NPCManager.Instance.RemoveNPCFromSave(1001);
        TaskManager.Instance.UnlockTask(1004);
    }

    public void task_1_4(bool bPre)
    {
        if (bPre)
        {
            // Modify Guide NPC
            NPCManager.Instance.RemoveNPCFromSave(1000);
            NPCManager.Instance.AddNewNPCToSave(1000, Constants.SCENE_AUDIENCE_LEVEL, new Vector3(-1.235f, 7.28f, -2.351f));
            SaveManager.Instance.SetNPCActive(1000, true);
            NPCManager.Instance.AddInteractionToNPC(1000, 10006);

            // Teleport the player to the audience seat
            LevelManager.Instance.LoadScene(Constants.SCENE_AUDIENCE_LEVEL);
            return;
        }
        // Remove NPC_1_2 if still there
        NPCManager.Instance.DespawnNPC(1002);
        // Remove NPC
        NPCManager.Instance.RemoveNPCFromSave(1002);
        NPCManager.Instance.RemoveInteractionFromNPC(1000, 10006);

        // Send player to the matching scene
        LevelManager.Instance.LoadScene(Constants.SCENE_MATCHING_LEVEL);
        TaskManager.Instance.UnlockTask(1014);
    }

    public void task_1_4_b(bool bPre)
    {
        if (bPre)
        {
            // Add a trggier to the game
            NPCManager.Instance.AddNewNPCToSave(1201, Constants.SCENE_MATCHING_LEVEL, new Vector3(10.33f, 3.23f, 0));
            SaveManager.Instance.SetNPCActive(1201, true);
            return;
        }
        TaskManager.Instance.UnlockTask(1005);
        NPCManager.Instance.RemoveNPCFromSave(1201);
        LevelManager.Instance.SetGameTime(GameEvent.Event.TIME_SUNSET, true);
    }

    public void task_1_5(bool bPre)
    {
        if (bPre)
        {
            // Give NPC more things to talk about
            NPCManager.Instance.AddInteractionToNPC(1000, 10008);
            return;
        }
        // Remove Guide
        NPCManager.Instance.RemoveInteractionFromNPC(1000, 10008);
        NPCManager.Instance.RemoveNPCFromSave(1000);
        TaskManager.Instance.UnlockTask(1006);

        ReminderManager.Instance.ShowGeneralReminder(3);
        // Set Time
        LevelManager.Instance.SetGameTime(GameEvent.Event.TIME_NIGHT);
        // Enable Map System
        SaveManager.Instance.InitModuleLock(new bool[] { false, true, false, false, false, false });
    }

    public void task_1_6(bool bPre)
    {
        if (bPre)
        {
            // Spawn the bed trigger
            NPCManager.Instance.AddNewNPCToSave(1102, Constants.SCENE_ROOMA_LEVEL, new Vector3(4.35f, 0.6f, -1.4f));
            NPCManager.Instance.AddInteractionToNPC(1102, 10009);
            SaveManager.Instance.SetNPCActive(1102, true);
            TaskManager.Instance.UnlockTask(1101);
            return;
        }
        NPCManager.Instance.RemoveNPCFromSave(1102);
        DayCycleManager.Instance.GoToNextDay();
    }

    public void task_1_1_0(bool bPre)
    {
        if (bPre)
        {
            // Add NPC_1_2
            NPCManager.Instance.AddNewNPCToSave(1002, Constants.SCENE_AUDIENCE_LEVEL, new Vector3(-3.9f, 7.28f, -1.65f));
            SaveManager.Instance.SetNPCActive(1002, true);
            NPCManager.Instance.AddInteractionToNPC(1002, 10007);
            return;
        }
        // Remove NPC
        NPCManager.Instance.RemoveInteractionFromNPC(1002, 10007);
        NPCManager.Instance.RemoveNPCFromSave(1002);
    }

    public void task_1_1_1(bool bPre)
    {
        if (bPre)
        {
            // Add NPC_1_2
            NPCManager.Instance.AddNewNPCToSave(1003, Constants.SCENE_GUILD_LEVEL, new Vector3(4.6f, 0.56f, 0.22f));
            SaveManager.Instance.SetNPCActive(1003, true);
            NPCManager.Instance.AddInteractionToNPC(1003, 10010);
            return;
        }
        // Remove NPC
        NPCManager.Instance.AddInteractionToNPC(1003, 10011);
        NPCManager.Instance.RemoveInteractionFromNPC(1003, 10010);
        // Save the indicator of completion
        SaveManager.Instance.SetGlobalVariable("DAY_1_TALKED_WITH_WEIRD_GUY", true);
    }
}
