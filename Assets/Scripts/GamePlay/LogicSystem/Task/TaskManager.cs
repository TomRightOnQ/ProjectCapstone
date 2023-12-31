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

    // Public:
    // Process events
    public void ProcessActions(int[] actionIDs)
    {
        configTaskActions(actionIDs);
    }

    // Private:
    // Config task actions
    private void configTaskActions(int[] actionIDs)
    {
        for (int i = 0; i < actionIDs.Length; i++)
        {
            int id = actionIDs[i];
            if (id == -1)
            {
                break;
            }
            ActionData.ActionDataStruct actionData = ActionData.GetData(id);
            switch (actionData.ActionType)
            {
                case Enums.TASK_ACTION.None:
                    break;
                case Enums.TASK_ACTION.Chat:
                    ChatInteractionManager.Instance.BeginInteraction(actionData.ActionTarget[0]);
                    break;
                case Enums.TASK_ACTION.End:
                    ChatInteractionManager.Instance.EndInteraction();
                    break;
                case Enums.TASK_ACTION.Claim:
                    ChatInteractionManager.Instance.EndInteraction();
                    break;
                case Enums.TASK_ACTION.StartGame:
                    CharacterManager.Instance.ShowCharacterPickerPanel(actionData.ActionTarget[0]);
                    break;
                case Enums.TASK_ACTION.Teleport:
                    LevelManager.Instance.LoadScene(LevelConfig.Instance.GetLevelData(actionData.ActionTarget[0]).SceneName);
                    break;
                case Enums.TASK_ACTION.AddInteraction:
                    addInteractionsFromTask(actionData.ActionTarget);
                    break;
                case Enums.TASK_ACTION.RemoveInteraction:
                    removeInteractionsFromTask(actionData.ActionTarget);
                    break;
                case Enums.TASK_ACTION.UnlockNextDay:
                    DayCycleManager.Instance.UnlockNextDay();
                    break;
                case Enums.TASK_ACTION.EnterActing:
                    HUDManager.Instance.EnterActingMode();
                    break;
                case Enums.TASK_ACTION.ExitActing:
                    HUDManager.Instance.ExitActingMode();
                    break;
                case Enums.TASK_ACTION.ShowReminder:
                    ReminderManager.Instance.ShowGeneralReminder(actionData.ActionTarget[0]);
                    break;
                case Enums.TASK_ACTION.ChangeNPCPosition:
                    NPCManager.Instance.ChangeNPCPositionAndScene(actionData.ActionTarget[0], actionData.ActionTarget[1], actionData.ActionTarget[2]);
                    break;
                default:
                    break;
            }
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

    // Unlock tasks
    public void UnlockTasks(int[] taskIDs)
    {
        for (int i = 0; i < taskIDs.Length; i++)
        {
            int id = taskIDs[i];
            if (id > -1)
            {
                UnlockTask(id);
            }
        }
    }

    // Unlock a single task
    public void UnlockTask(int taskID)
    {
        TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
        configTaskActions(taskData.PreActions);
    }

    // Complete a single task
    public void CompleteTask(int taskID)
    {
        if (taskID == -1)
        {
            return;
        }
        TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
        configTaskActions(taskData.PostActions);
        UnlockTasks(taskData.UnlockTask);
    }
}
