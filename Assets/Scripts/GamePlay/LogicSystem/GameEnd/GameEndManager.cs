using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager Class of any game ending and the achievement system
/// </summary>
public class GameEndManager : MonoBehaviour
{
    private static GameEndManager instance;
    public static GameEndManager Instance => instance;

    // Game End UI Components
    [SerializeField] private UI_GameEnd ui_GameEnd;

    // Game Achievement Page Components
    [SerializeField] private UI_GameAch ui_GameAch;

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

    // Config UI
    private void createUIGameEnd()
    {
        GameObject uiObject = UIManager.Instance.CreateUI("UI_GameEnd");
        ui_GameEnd = uiObject.GetComponent<UI_GameEnd>();
    }

    private void createUIGameAch()
    {
        GameObject uiObject = UIManager.Instance.CreateUI("UI_GameAch");
        ui_GameAch = uiObject.GetComponent<UI_GameAch>();
    }

    // Show the Game Achievement Page
    public void ShowGameAchievementPage()
    {
        if (ui_GameAch == null)
        {
            createUIGameAch();
        }
        ui_GameAch.ShowGameAchievementPage();
    }

    // Trigger an game End
    public void EndGame(int gameEndID)
    {
        if (ui_GameEnd == null)
        {
            createUIGameEnd();
        }
        ui_GameEnd.SetUp(gameEndID);
        // Save the unlocked end to the save system
        SaveManager.Instance.UnlockGameEnds(gameEndID);
        SaveManager.Instance.SaveGameCoreSave();
    }

    // Trigger an game achievement
    public void UnlockAhievements (int gameAchID)
    {
        // If the achievement has not been acquired before, remind the player
        if (!CoreSaveConfig.Instance.AchUnlockList.Contains(gameAchID))
        {
            ReminderManager.Instance.ShowAchievementReminder(gameAchID);
            // Save the unlocked achievement to the save system
            SaveManager.Instance.UnlockGameAch(gameAchID);
            SaveManager.Instance.SaveGameCoreSave();
        }
    }
}
