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
            case 1004:
                task_1_4(bPre);
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
            default:
                break;
        }
    }

    // Private:
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
        LevelManager.Instance.LoadScene(Constants.SCENE_DEFAULT_LEVEL);
    }

    public void task_1_3(bool bPre)
    {
        if (bPre)
        {
            // Place NPC2
            NPCManager.Instance.AddNewNPCToSave(1001, Constants.SCENE_DEFAULT_LEVEL, new Vector3(4f, 0.5f, 0.25f));
            SaveManager.Instance.SetNPCActive(1001, true);
            NPCManager.Instance.AddInteractionToNPC(1001, 10005);
            return;
        }
        // Enable Note system
        SaveManager.Instance.InitModuleLock(new bool[] { false, false, false, true, false, false });
        SaveManager.Instance.AddNote(Enums.NOTE_TYPE.Note, new int[] { 10000 });
        ReminderManager.Instance.ShowGeneralReminder(4);
        NPCManager.Instance.RemoveNPCFromSave(1001);
        TaskManager.Instance.UnlockTask(1100);
        TaskManager.Instance.UnlockTask(1004);
    }

    public void task_1_4(bool bPre)
    {
        if (bPre)
        {
            // Modify Guide NPC
            NPCManager.Instance.RemoveNPCFromSave(1000);
            NPCManager.Instance.AddNewNPCToSave(1000, Constants.SCENE_AUDIENCE_LEVEL, new Vector3(-1.235f, 7.2f, -2.351f));
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
        // Play screen
        NPCManager.Instance.RemoveInteractionFromNPC(1000, 10006);
        ReminderManager.Instance.ShowWholeScreenReminder(2);
        TaskManager.Instance.UnlockTask(1005);
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
        // Enable Map System
        SaveManager.Instance.InitModuleLock(new bool[] { false, false, false, false, false, false });
    }

    public void task_1_6(bool bPre)
    {
        if (bPre)
        {
            // Spawn the bed trigger
            NPCManager.Instance.AddNewNPCToSave(1102, Constants.SCENE_ROOMA_LEVEL, new Vector3(4.35f, 0.6f, -1.4f));
            NPCManager.Instance.AddInteractionToNPC(1102, 10009);
            SaveManager.Instance.SetNPCActive(1102, true);
            return;
        }
    }

    public void task_1_1_0(bool bPre)
    {
        if (bPre)
        {
            // Add NPC_1_2
            NPCManager.Instance.AddNewNPCToSave(1002, Constants.SCENE_AUDIENCE_LEVEL, new Vector3(-3.9f, 7.2f, -1.65f));
            SaveManager.Instance.SetNPCActive(1002, true);
            NPCManager.Instance.AddInteractionToNPC(1002, 10007);
            return;
        }
        // Remove NPC
        NPCManager.Instance.RemoveInteractionFromNPC(1002, 10007);
        NPCManager.Instance.RemoveNPCFromSave(1002);
    }
}
