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
    public void JumpToDay(int targetDay)
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
            JumpToDay(currentDay + 1);
        }
    }

    // Show the daily report
    public void ShowReport(int targetDay)
    {
        
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
    // Event Handlers
    private void OnRecv_SceneLoaded()
    {
        // First, remove the listener
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);
        // Run the day script
        switch (currentDay)
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
        // Marked the current day as inited
        SaveManager.Instance.SaveCurrentDayInited(true);
    }
}
