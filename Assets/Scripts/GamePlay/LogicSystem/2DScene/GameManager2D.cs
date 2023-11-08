using System.Collections;
using System.Collections.Generic;
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

    // Result UI
    [SerializeField] private UI_Level2DComplete UI_Level2DComplete;

    // Battle Timer
    [SerializeField, ReadOnly]
    private float battleTime = 0f;
    public float BattleTime => battleTime;

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

    // Public:
    // Config the game scene
    public void SetGame(int gameID)
    {
        gameLevelID = gameID;
        UI_Level2DComplete = UIManager.Instance.CreateUI("UI_Level2DComplete").GetComponent<UI_Level2DComplete>();

        Level2DData.Level2DDataStruct level2DData = Level2DData.GetData(gameID);
        Enums.LEVEL_TYPE gameMode = level2DData.Type;
        switch (gameMode)
        {
            default:
                break;
        }
        HUDManager.Instance.BeginHUDTimer();
        BattleObserver.Instance.BeginGame();
        StartCoroutine(UpdateTimerCoroutine());
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

    // End the game
    // Pass/No Pass: This only determines if the completion of the game will take the player to the next game
    // Victory/DefeatL This indicates the result of the game
    public void EndGame(bool bPass = true, bool bVictory = true)
    {
        StartCoroutine(UpdateTimerCoroutine());
        BattleObserver.Instance.EndGame();
        HUDManager.Instance.EndHUDTimer();
        InputManager.Instance.LockInput();
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_2DGAME_END);
        UIManager.Instance.ShowUI("UI_Level2DComplete");
    }

    // Leave the game Scene
    public void LeaveGameScene()
    {
        string nextScene = Constants.SCENE_AUDIENCE_LEVEL;
        if (gameLevelID > -1)
        {
            Level2DData.Level2DDataStruct level2DData = Level2DData.GetData(gameLevelID);
            nextScene = level2DData.Next;

            TaskManager.Instance.CompleteTask(level2DData.Complete);
        }
        LevelManager.Instance.LoadScene(nextScene);
    }
}
