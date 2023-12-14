using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Game Manager Designed for 2D scene
/// --The life cycle of this manager only lasts for single level scene--
/// </summary>
public class GameManager2D : MonoBehaviour
{
    private static GameManager2D instance;
    public static GameManager2D Instance => instance;

    // Gamemode
    [SerializeField, ReadOnly]
    private Enums.LEVEL_TYPE gameMode;
    public Enums.LEVEL_TYPE GameMode => gameMode;

    // Game Level ID
    [SerializeField, ReadOnly]
    private int gameLevelID = -1;
    public int GameLevelID => gameLevelID;

    // Chosen Character ID
    private int characterID = 1;
    public int CharacterID => characterID;

    // UI
    [SerializeField] private UI_Level2D UI_Level2D;

    // Battle Timer
    [SerializeField, ReadOnly]
    private float battleTime = 0f;
    public float BattleTime => battleTime;

    // Game Score
    [SerializeField, ReadOnly]
    private int gameScore = 0;
    private List<(int TeamID, int Score)> gameResult = new List<(int TeamID, int Score)>();
    public List<(int TeamID, int Score)> GameResult => gameResult;

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

    private void Update()
    {
        if (!BattleObserver.Instance.BGameStarted)
        {
            return;
        }
        battleTime += Time.deltaTime;
    }

    // Private:
    // Process Score
    private void processGameScore()
    {
        gameResult.Clear();
        // Load non-player teams' preset game results
        Level2DData.Level2DDataStruct levelData = Level2DData.GetData(gameLevelID);
        // Process the array into dictionary with player's actual score
        gameResult.Add((0, gameScore));
        for (int i = 0; i < levelData.Score.Length; i += 2)
        {
            gameResult.Add((levelData.Score[i], levelData.Score[i + 1]));
        }
        // Sort
        gameResult = gameResult.OrderByDescending(result => result.Score).ToList();

        // Add score to the teams
        for (int i = 0; i < gameResult.Count; i ++)
        {
            SaveManager.Instance.SaveGuildData(gameResult[i].TeamID, gameResult[i].Score, 0, 0, false);
        }
    }

    // Public:
    // Start the game
    public void StartGame()
    {
        switch (gameMode)
        {
            case Enums.LEVEL_TYPE.Shooter:
                ShooterLevelManager.Instance.StartGame();
                break;
            default:
                break;
        }
        InputManager.Instance.UnLockInput(Enums.SCENE_TYPE.Battle, Enums.LEVEL_TYPE.Shooter);
        BattleObserver.Instance.BeginGame();
        StartCoroutine(UpdateTimerCoroutine());
    }

    // Config the game scene
    public void SetGame(int gameID, int playerCharacterID)
    {
        gameLevelID = gameID;
        characterID = playerCharacterID;
        UI_Level2D = UIManager.Instance.CreateUI("UI_Level2D").GetComponent<UI_Level2D>();

        Level2DData.Level2DDataStruct level2DData = Level2DData.GetData(gameID);
        gameMode = level2DData.Type;
        switch (gameMode)
        {
            case Enums.LEVEL_TYPE.Shooter:
                GameObject ShooterLevelManager = new GameObject();
                ShooterLevelManager.name = "ShooterLevelManager";
                ShooterLevelManager shooterLevelManager = ShooterLevelManager.AddComponent<ShooterLevelManager>();
                shooterLevelManager.Init(gameID);
                break;
            default:
                break;
        }
        HUDManager.Instance.BeginHUDTimer();
        // Show Panel after loaded
        ShowGameStartPanel();
    }

    // Update the timer
    private IEnumerator UpdateTimerCoroutine()
    {
        while (true)
        {
            HUDManager.Instance.UpdateHUDTimer(battleTime);
            yield return new WaitForSeconds(1f);
        }
    }

    // Show/Hide Game Start Panel
    public void ShowGameStartPanel()
    {
        UI_Level2D.SetLevelStartPanel(true);
    }

    public void HideGameStartPanel()
    {
        UI_Level2D.SetLevelStartPanel(false);
    }

    // Update the score
    public void UpdatePlayerScore(int score)
    {
        gameScore = score;
    }

    // End the game
    // Pass/No Pass: This only determines if the completion of the game will take the player to the next game
    // Victory/DefeatL This indicates the result of the game
    public void EndGame(bool bPass = true, bool bVictory = true)
    {
        StopCoroutine(UpdateTimerCoroutine());
        BattleObserver.Instance.EndGame();
        HUDManager.Instance.EndHUDTimer();
        InputManager.Instance.LockInput();
        CharacterManager.Instance.LockCharacter2D(characterID, gameMode);
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_2DGAME_END);
        HUDManager.Instance.HideAllHUD();
        // Config Score and notify the UI System
        processGameScore();
        UI_Level2D.SetLevelCompletePanel(true);
    }

    // Leave the game Scene
    public void LeaveGameScene()
    {
        string nextScene = Constants.SCENE_AUDIENCE_LEVEL;
        if (gameLevelID > -1)
        {
            Level2DData.Level2DDataStruct level2DData = Level2DData.GetData(gameLevelID);
            nextScene = level2DData.Next;

            TaskManager.Instance.CompleteTask(level2DData.TaskComplete);
        }
        LevelManager.Instance.LoadScene(nextScene);
    }
}
