using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core component to achieve 7-day cycle for the plot
/// </summary>
public class DayCycleManager : MonoBehaviour
{
    private static DayCycleManager instance;
    public static DayCycleManager Instance => instance;

    // UI Components
    [SerializeField] private UI_DayCycleControl ui_DayCycleControl;

    // Current Day
    [SerializeField, ReadOnly]
    private int currentDay = 0;
    public int CurrentDay => currentDay;
    private DayScriptBase currentScript;

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
    public void Init()
    {
        currentDay = SaveManager.Instance.GetCurrentDay();
        if (ui_DayCycleControl == null && LevelManager.Instance.CurrentSceneType != Enums.SCENE_TYPE.Outside)
        {
            GameObject uiObject = UIManager.Instance.CreateUI("UI_DayCycleControl");
            ui_DayCycleControl = uiObject.GetComponent<UI_DayCycleControl>();
        }
    }

    // Init the day 0
    public void LoadDayZero()
    {
        currentScript = new ScriptDayZero();
        currentScript.Init();
    }

    // Unlock Next Day
    public void UnlockNextDay()
    {
        ShowReport(currentDay);
        if (ui_DayCycleControl != null && currentDay < 7)
        {
            // Write to save
            SaveManager.Instance.SaveMaxDay(currentDay);
            ui_DayCycleControl.ShowNextDayButton();
        }
    }

    // Jump to the beginning of a specific day
    // bBack: indicates if we are flashing back
    public void JumpToDay(int targetDay, bool bBack = true)
    {
        // Reset the day script
        if (currentScript != null)
        {
           currentScript = null;
        }
        // Change the save Status
        SaveManager.Instance.SaveCurrentDay(targetDay);

        // Marked the day as uninited
        SaveManager.Instance.SaveCurrentDayInited(false);

        currentDay = targetDay;
        // Show Animation

        // Add a listner to determine the end of loading
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);

        // Run the day script
        switch (targetDay)
        {
            case 0:
                currentScript = new ScriptDayZero();
                currentScript.Init();
                break;
            case 1:
                currentScript = new ScriptDayOne();
                currentScript.Init();
                break;
            default:
                break;
        }

        ///
        /// If we are jumping to the beginning of a day, we should alter to the beginning of that day by loading.
        /// Then we shall overwrite the current game save.
        /// Otherwise, we are going to a future day, save the current save as the new day's save.
        ///
        if (bBack)
        {
            // Load the target day save
            SaveManager.Instance.LoadGameSave(targetDay);
            // Overwrite the beginning of the new day to the current save
            SaveManager.Instance.SaveGameSave(Constants.SAVE_CURRENT_SAVE);
        }

        // Load Scene
        string targetScene = DayData.GetData(targetDay).StartedScene;
        LevelManager.Instance.LoadScene(targetScene);
    }

    // Go to the next day
    public void GoToNextDay()
    {
        if (currentDay < 7)
        {
            ui_DayCycleControl.HideNextDayButton();
            JumpToDay(currentDay + 1, false);
        }
    }

    // Show the daily report
    public void ShowReport(int targetDay)
    {
        
    }

    // Config Task Actions According to the day
    public void ConfigTaskAction(int taskID, bool bPre)
    {
        if (currentScript == null)
        {
            configCurrentDayScript();
        }
        currentScript.ConfigTaskAction(taskID, bPre);
    }

    // Show list of flash back
    public void ShowDayPanel()
    {
        if (ui_DayCycleControl != null)
        {
            ui_DayCycleControl.ShowDayPanel();
        }
    }

    // Private:
    // Config today's script
    private void configCurrentDayScript()
    {
        // Run the day script
        switch (currentDay)
        {
            case 0:
                currentScript = new ScriptDayZero();
                break;
            case 1:
                currentScript = new ScriptDayOne();
                break;
            default:
                break;
        }
    }

    // Event Handlers
    private void OnRecv_SceneLoaded()
    {
        // First, remove the listener
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);
        if (!SaveManager.Instance.GetIsCurrentDayInited())
        {
            SaveManager.Instance.SaveGameSave(currentDay);
        }
        // Mark the current day as inited
        SaveManager.Instance.SaveCurrentDayInited(true);
    }
}
