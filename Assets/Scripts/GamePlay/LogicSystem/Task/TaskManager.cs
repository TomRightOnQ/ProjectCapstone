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

    // Current Tracked Task
    [SerializeField, ReadOnly]
    private int currentTrackedTaskID = -1;
    public int CurrentTrackedTaskID => currentTrackedTaskID;

    // UI Components
    [SerializeField] private UI_Task ui_Task;

    /// <summary>
    /// References of tracking indicators
    /// Tracking NPC or a position
    /// </summary>
    private Dictionary<int, TaskIndicator> npcTrackDictionary = new Dictionary<int, TaskIndicator>();
    private Dictionary<int, TaskIndicator> positionTrackDictionary = new Dictionary<int, TaskIndicator>();

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            configEventHandlers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Private:
    // Config scene loading
    private void configEventHandlers()
    {
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_UNLOADED, OnRecv_SceneUnloaded);
    }

    // Config UIs
    private void createTaskUI()
    {
        if (ui_Task == null)
        {
            GameObject uiObject = UIManager.Instance.CreateUI("UI_Task");
            ui_Task = uiObject.GetComponent<UI_Task>();
            UIManager.Instance.ShowUI("UI_Task");
        }
    }

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
                case Enums.TASK_ACTION.EnterGame:
                    LevelManager.Instance.Load2DLevel(actionData.ActionTarget[0], actionData.ActionTarget[1]);
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
                case Enums.TASK_ACTION.TriggerTask:
                    UnlockTasks(actionData.ActionTarget);
                    break;
                case Enums.TASK_ACTION.CompleteTask:
                    CompleteTask(actionData.ActionTarget);
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
                case Enums.TASK_ACTION.SaveGame:
                    SaveManager.Instance.SaveGameSave(Constants.SAVE_CURRENT_SAVE);
                    SaveManager.Instance.SaveGameCoreSave();
                    break;
                case Enums.TASK_ACTION.UnlockNotes:
                    SaveManager.Instance.AddNote(Enums.NOTE_TYPE.Note, actionData.ActionTarget);
                    break;
                case Enums.TASK_ACTION.UnlockItems:
                    SaveManager.Instance.AddNote(Enums.NOTE_TYPE.Item, actionData.ActionTarget);
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

    // Config Tracking
    private void configTaskTracking(int taskID)
    {
        // Do not change tracking if a valid task is already tracked
        if (currentTrackedTaskID != -1 &&
            !TaskData.GetData(currentTrackedTaskID).bHidden &&
            SaveManager.Instance.CheckTaskStatus(currentTrackedTaskID) == Enums.TASK_STATUS.Triggered)
        {
            /*
             Right after changeing a scene with a valid tracking, we need to 
            re-configure the HUD tracking since all UI widgets are reset upon
            scene changes;
            we also have to spawn the tracking object
             */
            HUDManager.Instance.UpdateHUDTaskTracking(currentTrackedTaskID);
            setTrackOnTarget();
            return;
        }

        HUDManager.Instance.ClearTracking();
        // If no tasks are tracked, choose the first one on list
        if (currentTrackedTaskID == -1 && taskID != -1)
        {
            if (!TaskData.GetData(taskID).bHidden && SaveManager.Instance.CheckTaskStatus(taskID) == Enums.TASK_STATUS.Triggered)
            {
                currentTrackedTaskID = taskID;
                HUDManager.Instance.UpdateHUDTaskTracking(taskID);
                setTrackOnTarget();
                return;
            }
        }
        else if (currentTrackedTaskID == -1 && SaveManager.Instance.GetTriggeredTasks().Count > 0)
        {
            for (int i = 0; i < SaveManager.Instance.GetTriggeredTasks().Count; i++)
            {
                int tempID = SaveManager.Instance.GetTriggeredTasks()[i];
                if (!TaskData.GetData(tempID).bHidden)
                {
                    currentTrackedTaskID = tempID;
                    HUDManager.Instance.UpdateHUDTaskTracking(currentTrackedTaskID);
                    setTrackOnTarget();
                    return;
                }
            }
        }
        else if (currentTrackedTaskID != -1)
        {
            if (!TaskData.GetData(currentTrackedTaskID).bHidden && 
                SaveManager.Instance.CheckTaskStatus(currentTrackedTaskID) == Enums.TASK_STATUS.Triggered)
            {
                HUDManager.Instance.UpdateHUDTaskTracking(taskID);
                setTrackOnTarget();
                return;
            }
        }
        // Other wise, clear the map tracking
        MapManager.Instance.CancelMapTracking();
    }

    // Set Tracking
    private void setTrackOnTarget()
    {
        // Remove all old tracking
        foreach (KeyValuePair<int, TaskIndicator> i in npcTrackDictionary)
        {
            Destroy(i.Value.gameObject);
        }
        npcTrackDictionary.Clear();
        MapManager.Instance.CancelMapTracking();

        // Get data for the current tracking
        TaskData.TaskDataStruct taskData = TaskData.GetData(currentTrackedTaskID);
        // Set Map Tracking
        MapManager.Instance.SetMapTaskTracking(taskData.SceneName);
        // Only track when the target is in the scene
        if (taskData.SceneName != LevelManager.Instance.CurrentScene && !taskData.bHidden)
        {
            return;
        }
        // Track Position or NPC
        if (taskData.TrackTarget != -1)
        {
            if (taskData.bTrackNPC)
            {
                TrackNPC(taskData.TrackTarget);
            }
            else 
            {
                TrackPosition(taskData.TrackTarget);
            }
        }
    }

    // Public:
    /// <summary>
    /// UI_Task Methods
    /// </summary>
    // Show Task Panels
    public void ShowTaskPanel()
    {
        if (ui_Task == null)
        {
            createTaskUI();
        }
        ui_Task.ShowTaskPanel();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_OPEN);
    }

    public void CloseTaskPanel()
    {
        if (ui_Task == null)
        {
            return;
        }
        ui_Task.CloseTaskPanel();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CLOSE);
    }

    // Manually track a task
    public void TrackTask(int taskID)
    {
        currentTrackedTaskID = -1;
        configTaskTracking(taskID);
    }

    // Process events
    public void ProcessActions(int[] actionIDs)
    {
        configTaskActions(actionIDs);
    }

    // Complete a series of tasks
    public void CompleteTask(int[] taskIDs)
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
        if (SaveManager.Instance.CheckTaskStatus(taskID) == Enums.TASK_STATUS.NotTriggered)
        {
            SaveManager.Instance.TriggerTask(taskID);
            TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
            DayCycleManager.Instance.ConfigTaskAction(taskID, true);
        }
        configTaskTracking(taskID);
    }

    // Destroy the indicator
    public void StopIndicator()
    {
        StopIndicator(currentTrackedTaskID);
    }
    public void StopIndicator(int taskID)
    {
        if (taskID == -1)
        {
            return;
        }
        if (SaveManager.Instance.CheckTaskStatus(taskID) == Enums.TASK_STATUS.Triggered)
        {
            TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
            // Reset Tracking
            // Clear current Tracking
            if (taskData.bTrackNPC && npcTrackDictionary.ContainsKey(taskData.TrackTarget))
            {
                PrefabManager.Instance.Destroy(npcTrackDictionary[taskData.TrackTarget].gameObject);
                npcTrackDictionary.Remove(taskData.TrackTarget);
            }
            else if (positionTrackDictionary.ContainsKey(taskData.TrackTarget))
            {
                PrefabManager.Instance.Destroy(npcTrackDictionary[taskData.TrackTarget].gameObject);
                positionTrackDictionary.Remove(taskData.TrackTarget);
            }
        }
    }

    // Complete a single task
    public void CompleteTask(int taskID)
    {
        if (taskID == -1)
        {
            return;
        }
        if (SaveManager.Instance.CheckTaskStatus(taskID) == Enums.TASK_STATUS.Triggered)
        {
            TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
            SaveManager.Instance.CompleteTask(taskID);

            // Reset Tracking
            // Clear current Tracking
            if (taskData.bTrackNPC && npcTrackDictionary.ContainsKey(taskData.TrackTarget))
            {
                PrefabManager.Instance.Destroy(npcTrackDictionary[taskData.TrackTarget].gameObject);
                npcTrackDictionary.Remove(taskData.TrackTarget);
            }
            else if (positionTrackDictionary.ContainsKey(taskData.TrackTarget))
            {
                PrefabManager.Instance.Destroy(npcTrackDictionary[taskData.TrackTarget].gameObject);
                positionTrackDictionary.Remove(taskData.TrackTarget);
            }

        }
        // Reset tracking
        currentTrackedTaskID = -1;
        configTaskTracking(currentTrackedTaskID);

        DayCycleManager.Instance.ConfigTaskAction(taskID, false);

        // Auto Save
        SaveManager.Instance.SaveGameSave(Constants.SAVE_CURRENT_SAVE);
    }

    // Delete a single task
    public void DeleteTask(int taskID)
    {
        if (taskID == -1)
        {
            return;
        }
        if (SaveManager.Instance.CheckTaskStatus(taskID) == Enums.TASK_STATUS.Triggered)
        {
            TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
            SaveManager.Instance.RemoveTriggeredTask(taskID);

            // Reset Tracking
            // Clear current Tracking
            if (taskData.bTrackNPC && npcTrackDictionary.ContainsKey(taskData.TrackTarget))
            {
                PrefabManager.Instance.Destroy(npcTrackDictionary[taskData.TrackTarget].gameObject);
                npcTrackDictionary.Remove(taskData.TrackTarget);
            }
            else if (positionTrackDictionary.ContainsKey(taskData.TrackTarget))
            {
                PrefabManager.Instance.Destroy(npcTrackDictionary[taskData.TrackTarget].gameObject);
                positionTrackDictionary.Remove(taskData.TrackTarget);
            }
            currentTrackedTaskID = -1;
            configTaskTracking(currentTrackedTaskID);
        }

        // Auto Save
        SaveManager.Instance.SaveGameSave(Constants.SAVE_CURRENT_SAVE);
    }

    /// <summary>
    /// Tracking methods
    /// </summary>
    // Track an NPC
    public void TrackNPC(int npcID)
    {
        // Make sure NPC in current scene and not already tracked
        if (!NPCManager.Instance.NpcMap.ContainsKey(npcID) || npcTrackDictionary.ContainsKey(npcID))
        {
            return;
        }
        // Create a TaskIndicator on Task UI Canvas and config it
        GameObject indicatorObj = PrefabManager.Instance.Instantiate("TaskIndicator", new Vector3(-1000f, -1000f, -1000f), Quaternion.identity);
        if (indicatorObj == null || indicatorObj.GetComponent<TaskIndicator>() == null)
        {
            return;
        }
        HUDManager.Instance.SetTrackIndicator(indicatorObj.transform);
        indicatorObj.SetActive(true);
        TaskIndicator indicator = indicatorObj.GetComponent<TaskIndicator>();
        indicator.ConfigIndicator(NPCManager.Instance.NpcMap[npcID].gameObject);

        // Hold the reference
        npcTrackDictionary[npcID] = indicator;
    }

    public void TrackPosition(int positionID)
    {
        // Make sure the position is not tracked yet
        if (positionTrackDictionary.ContainsKey(positionID))
        {
            return;
        }
        // Create a TaskIndicator on Task UI Canvas and config it
        GameObject indicatorObj = PrefabManager.Instance.Instantiate("TaskIndicator", Vector3.zero, Quaternion.identity);
        if (indicatorObj == null || indicatorObj.GetComponent<TaskIndicator>() == null)
        {
            return;
        }
        HUDManager.Instance.SetTrackIndicator(indicatorObj.transform);
        indicatorObj.SetActive(true);
        TaskIndicator indicator = indicatorObj.GetComponent<TaskIndicator>();
        indicator.ConfigIndicator(Vector3PositionData.GetData(positionID).Position);
        positionTrackDictionary[positionID] = indicator;
    }

    // Private:
    // Event Handlers
    private void OnRecv_SceneLoaded()
    {
        createTaskUI();
        // When sceneloaded, track task status
        configTaskTracking(currentTrackedTaskID);
    }

    private void OnRecv_SceneUnloaded()
    {
        // When scene unloaded, clear the tracking to avoid null references.
        npcTrackDictionary.Clear();
        positionTrackDictionary.Clear();
    }
}
