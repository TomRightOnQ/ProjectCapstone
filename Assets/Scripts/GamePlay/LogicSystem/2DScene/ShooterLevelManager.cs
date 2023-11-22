using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as the co-manager in 2D level
/// </summary>
public class ShooterLevelManager : MonoBehaviour
{
    private static ShooterLevelManager instance;
    public static ShooterLevelManager Instance => instance;

    // Data
    [SerializeField, ReadOnly]
    private int levelID = -1;
    [SerializeField, ReadOnly]
    private float currentTime = 999f;
    [SerializeField, ReadOnly]
    private int currentScore = 0;
    [SerializeField, ReadOnly]
    private int targetScore = 999;

    // Flags
    [SerializeField] private bool bStarted = false;
    public bool GameStarted => bStarted;

    // UI
    [SerializeField] private UI_ShooterLevel ui_ShooterLevel;

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

    // Private:
    private void Update()
    {
        if (!bStarted)
        {
            return;
        }
        currentTime -= Time.deltaTime;
        ui_ShooterLevel.UpdateTimer(currentTime);
        if (currentTime <= 0)
        {
            EndGame(false);
        }
    }

    // End Game
    private void EndGame(bool bWin)
    {
        bStarted = false;
        GameManager2D.Instance.EndGame(true, bWin);
    }

    // Public:
    // Init
    public void Init(int id)
    {
        if (ui_ShooterLevel == null)
        {
            GameObject uiObject = UIManager.Instance.CreateUI("UI_ShooterLevel");
            ui_ShooterLevel = uiObject.GetComponent<UI_ShooterLevel>();
        }
        levelID = id;
        Level2DData.Level2DDataStruct levelData = Level2DData.GetData(levelID);
        currentTime = levelData.TimeLimit;
        targetScore = levelData.ScoreGoal;
        ui_ShooterLevel.InitShooterLevelUI(currentTime, targetScore);
    }

    // Start the game
    public void StartGame()
    {
        bStarted = true;
        EventManager.Instance.PostEvent(GameEvent.Event.SHOOTER_LEVEL_BEGIN);
    }

    // Change Score
    public void AddScore(int amount)
    {
        currentScore += amount;
        ui_ShooterLevel.UpdateScore(currentScore);
        if (currentScore >= targetScore)
        {
            EndGame(true);
        }
    }

    public void RemoveScore(int amount)
    {
        currentScore += amount;
        ui_ShooterLevel.UpdateScore(currentScore);
    }
}
