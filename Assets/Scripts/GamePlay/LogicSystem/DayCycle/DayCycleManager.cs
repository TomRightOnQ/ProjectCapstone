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
        // The the player might get eliminated, notify it
        if (CheckPlayerEliminated())
        {
            ReminderManager.Instance.ShowSubtitleReminder(16);
        }

        // Enable Flash Back
        SaveManager.Instance.SetModuleLock(false, 1);

        if (ui_DayCycleControl != null)
        {
            // Write to save
            SaveManager.Instance.SaveMaxDay(currentDay);
            ui_DayCycleControl.ShowNextDayButton();
        }

        SaveManager.Instance.SaveGameSave(Constants.SAVE_CURRENT_SAVE);
    }

    // Jump to the beginning of a specific day
    // bBack: indicates if we are flashing back
    public void JumpToDay(int targetDay, bool bBack = true)
    {
        // Marked the day as uninited
        SaveManager.Instance.SaveCurrentDayInited(false);

        // Reset the day script
        if (currentScript != null)
        {
            Destroy(currentScript);
        }

        currentDay = targetDay;

        ///
        /// If we are jumping to the beginning of a day, we should alter to the beginning of that day by loading.
        /// Then we shall overwrite the current game save.
        /// Otherwise, we are going to a future day, save the current save as the new day's save.
        ///
        if (bBack)
        {
            // Load the target day save
            SaveManager.Instance.LoadGameSave(targetDay);
        }

        // Run the day script
        configCurrentDayScript();

        // Add a listner to determine the end of loading
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);

        // Load Scene
        string targetScene = DayData.GetData(targetDay).StartedScene;
        LevelManager.Instance.LoadScene(targetScene);
    }

    public void JumpToDay(bool bBack = true)
    {
        JumpToDay(currentDay, bBack);
    }

    // Flash to the previous day
    public void GoToPreviousDay()
    {
        DayData.DayDataStruct dayData = DayData.GetData(currentDay);
        if (dayData.PrevDayID != -1)
        {
            JumpToDay(dayData.PrevDayID, true);
        }
    }

    // Go to the next day
    public void GoToNextDay()
    {
        DayData.DayDataStruct dayData = DayData.GetData(currentDay);
        // -- If the player team is eliminated, trigger the ending --
        if (CheckPlayerEliminated())
        {
            GameEndManager.Instance.EndGame(dayData.GameEnd);
            return;
        }

        // Eliminate team if needed
        List<SaveConfig.GuildSaveData> guildList = SaveConfig.Instance.GuildSaveDataList;
        guildList.Sort((x, y) => y.Score.CompareTo(x.Score));
        for (int i = dayData.RemainTeam; i < 16; i++)
        {
            guildList[i].bElminated = true;
        }

        if (dayData.NextDayID != -1)
        {
            ui_DayCycleControl.HideNextDayButton();
            JumpToDay(dayData.NextDayID, false);
        }
    }

    // Check if the player will be eliminated
    public bool CheckPlayerEliminated()
    {
        DayData.DayDataStruct dayData = DayData.GetData(currentDay);
        List<SaveConfig.GuildSaveData> guildList = SaveConfig.Instance.GuildSaveDataList;
        guildList.Sort((x, y) => y.Score.CompareTo(x.Score));
        for (int i = dayData.RemainTeam; i < 16; i++)
        {
            if (guildList[i].GuildID == 0)
            {
                return true;
            }
        }
        return false;
    }

    // Public method for clicking the FlashBack Button from the menu
    public void FlashBack()
    {
        GoToPreviousDay();
        SaveManager.Instance.SetModuleLock(true, 1);
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
            GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_OPEN);
        }
    }

    // Private:
    // Config today's script
    private void configCurrentDayScript()
    {
        switch (currentDay)
        {
            case 0:
                currentScript = this.gameObject.AddComponent<ScriptDayZero>();
                break;
            case 1:
                currentScript = this.gameObject.AddComponent<ScriptDayOne>();
                break;
            case 2:
                currentScript = this.gameObject.AddComponent<ScriptDayTwo>();
                break;
            case 3:
                currentScript = this.gameObject.AddComponent<ScriptDayThree>();
                break;
            default:
                break;
        }
        if (!SaveManager.Instance.GetIsCurrentDayInited())
        {
            currentScript.Init();
        }
    }

    // Event Handlers
    private void OnRecv_SceneLoaded()
    {
        // First, remove the listener
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);
        SaveManager.Instance.SaveGameSave(currentDay);
        if (!SaveManager.Instance.GetIsCurrentDayInited())
        {
            // Play the first action of the day
            currentScript.BeginningAction();
        }
        // Mark the current day as inited
        SaveManager.Instance.SaveCurrentDayInited(true);
    }
}
