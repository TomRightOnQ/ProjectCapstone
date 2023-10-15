using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Task System
/// </summary>
public class TaskManager : MonoBehaviour
{
    private static TaskManager instance;
    public static TaskManager Instance => instance;
    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Private:
    // Config task actions
    private void configTaskActions(Enums.TASK_ACTION action, int[] target)
    {
        switch (action)
        {
            case Enums.TASK_ACTION.AddInteraction:
                addInteractionsFromTask(target);
                break;
            case Enums.TASK_ACTION.RemoveInteraction:
                removeInteractionsFromTask(target);
                break;
            case Enums.TASK_ACTION.None:
                break;
            default:
                break;
        }

    }

    // Config interactions:
    private void addInteractionsFromTask(int[] target)
    {
        for (int i = 0; i < target.Length; i += 2)
        {
            NPCManager.Instance.AddInteractionToNPC(target[i], target[i+1]);
        }
    }

    private void removeInteractionsFromTask(int[] target)
    {
        for (int i = 0; i < target.Length; i += 2)
        {
            NPCManager.Instance.RemoveInteractionFromNPC(target[i], target[i + 1]);
        }
    }

    // Public:
    // Complete a mission
    public void CompleteTasks(int[] taskIDs)
    {
        for (int i = 0; i < taskIDs.Length; i++)
        {
            CompleteTask(taskIDs[i]);
        }
    }

    // Complete a single task
    public void CompleteTask(int taskID)
    {
        TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
        configTaskActions(taskData.Action_1, taskData.Action_1_Target);
        configTaskActions(taskData.Action_2, taskData.Action_2_Target);
    }
}
