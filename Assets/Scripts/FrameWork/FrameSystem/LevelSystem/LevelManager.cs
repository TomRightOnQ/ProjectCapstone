using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    private string currentScene;
    public string CurrentScene => currentScene;

    private Enums.SCENE_TYPE currentSceneType;
    public Enums.SCENE_TYPE CurrentSceneType => currentSceneType;

    [SerializeField]
    private GameEvent.Event currentTime = GameEvent.Event.TIME_NOON;
    public GameEvent.Event CurrentTime => currentTime;

    // Cache if the current scene is savable
    [SerializeField, ReadOnly]
    private bool bSaveable = true;
    public bool bCurrentSceneSaveable => bSaveable;

    // Current 2D Level Info
    private Level2DData.Level2DDataStruct current2DLevel;
    public Level2DData.Level2DDataStruct Current2DLevel => current2DLevel;

    // 2D Scene Game ID
    [SerializeField, ReadOnly]
    private int gameID2D = -1;
    [SerializeField, ReadOnly]
    private int characterID = -1;
    [SerializeField, ReadOnly]
    private int groupID = 0;

    // UI Components
    [SerializeField] private UI_Loading ui_Loading;
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
        createLoadingUI();
    }

    // Swap Scene
    public void LoadScene(int id)
    {
        LoadScene(LevelConfig.Instance.LevelCollections[id].SceneName);
        groupID = 0;
        characterID = -1;
        gameID2D = -1;
    }

    public void LoadScene(string sceneName)
    {
        // Reset pooling system to clear references
        PrefabManager.Instance.ResetPooling();
        InputManager.Instance.LockInput();
        // Load the scene async
        StartCoroutine(loadSceneAsync(sceneName));
    }

    // Enter a 2D level
    public void Load2DLevel(int levelID, int chosenCharacterID = 1)
    {
        if (levelID < 0)
        {
            return;
        }
        current2DLevel = Level2DData.GetData(levelID);
        gameID2D = levelID;
        characterID = chosenCharacterID;
        groupID = current2DLevel.GroupID;
        LevelManager.Instance.LoadScene(current2DLevel.SceneName);
    }

    // Enter a new game via the main menu
    public void CreateNewGame()
    {
        SaveConfig.Instance.AllowRewrite = true;
        SaveManager.Instance.CreateNewSave();
    }

    // Enter game via the main menu
    public void EnterGame()
    {
        SaveManager.Instance.LoadGameSave(Constants.SAVE_CURRENT_SAVE);
        // Configs the scriptable object
        if (SaveConfig.Instance.AllowRewrite)
        {
            SaveManager.Instance.CreateNewSave();
            // case for new game
            return;
        }
        SaveManager.Instance.LoadGameCoreSave();
        SaveConfig.PlayerSaveData playerData = SaveManager.Instance.GetPlayer();

        // Load Game Time
        currentTime = SaveManager.Instance.GetGameTimeFromSave();

        // Edge Cases:
        // If the player quit the game while the game is initing the next day
        // In this case, re-init
        if (SaveManager.Instance.GetIsCurrentDayInited())
        {
            string currentScene = playerData.Scene;
            LoadScene(currentScene);
        }
        else 
        {
            DayCycleManager.Instance.JumpToDay(SaveManager.Instance.GetCurrentDay());
        }
    }

    // Set the omni timeset
    // bChangeTimeNow: Change the time now
    public void SetGameTime(GameEvent.Event timeEnum, bool bChangeTimeNow = false)
    {
        currentTime = timeEnum;
        // Save the time to the save
        SaveManager.Instance.SetGameTime(timeEnum);

        if (bChangeTimeNow)
        {
            SetSceneTime(currentTime);
        }
    }

    // Post current time to the scene
    public void SetSceneTime(GameEvent.Event timeEnum)
    {
        switch (timeEnum)
        {
            case GameEvent.Event.TIME_MORNING:
                EventManager.Instance.PostEvent(GameEvent.Event.TIME_MORNING);
                break;
            case GameEvent.Event.TIME_NOON:
                EventManager.Instance.PostEvent(GameEvent.Event.TIME_NOON);
                break;
            case GameEvent.Event.TIME_SUNSET:
                EventManager.Instance.PostEvent(GameEvent.Event.TIME_SUNSET);
                break;
            case GameEvent.Event.TIME_NIGHT:
                EventManager.Instance.PostEvent(GameEvent.Event.TIME_NIGHT);
                break;
            case GameEvent.Event.TIME_DARK:
                EventManager.Instance.PostEvent(GameEvent.Event.TIME_DARK);
                break;
            default:
                break;
        }
    }

    // Private:
    // Config UIs
    private void createLoadingUI()
    {
        if (ui_Loading == null)
        {
            GameObject uiObject = UIManager.Instance.CreateUI("UI_Loading");
            ui_Loading = uiObject.GetComponent<UI_Loading>();
        }
    }

    // Show Loading page
    // Hide Loading page
    private void showLoadingUI()
    {
        if (ui_Loading != null)
        {
            UIManager.Instance.ShowUI("UI_Loading");
        }
    }

    private void hideLoadingUI()
    {
        if (ui_Loading != null)
        {
            UIManager.Instance.HideUI("UI_Loading");
        }
    }

    // Coroutines
    private IEnumerator loadSceneAsync(string sceneName)
    {
        // Start loading the scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Notify the beginning of scene loading
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_SCENE_UNLOADED);
        // Show Loading UI
        showLoadingUI();

        // Wait until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        currentScene = sceneName;

        // Retrieve level metadata
        LevelData levelData = LevelConfig.Instance.GetLevelData(sceneName);

        bSaveable = levelData.bSaveable;
        MusicManager.Instance.PlayMusic(levelData.BGMName);
        currentSceneType = levelData.SceneType;

        // Scene is loaded, now load managers
        PersistentGameManager.Instance.LoadManagers();
        // Set the scene based on the current time
        SetSceneTime(currentTime);

        // Build the scene based on the saved info
        buildScene(sceneName);
    }

    // Load the scene based on the save contents
    // -- Core Methods --
    private void buildScene(string sceneName)
    {
        // Retrieve level metadata
        LevelData levelData = LevelConfig.Instance.GetLevelData(sceneName);

        // All rebuild methods
        // For menu scenes, do not rebuild some systems
        if (levelData.SceneType == Enums.SCENE_TYPE.World)
        {
            loadWorldScene(sceneName, levelData.StringName);
        }
        else if (levelData.SceneType == Enums.SCENE_TYPE.Battle)
        {
            load2DScene(sceneName, levelData.StringName);
        }

    }

    private void loadWorldScene(string sceneName, string stringName)
    {
        placePlayer(sceneName, false);
        placeNPCs(sceneName);
        finishBuild(stringName, Enums.SCENE_TYPE.World);
    }

    private void load2DScene(string sceneName, string stringName)
    {
        // Config group
        Level2DGroup[] levelGroups = FindObjectsByType<Level2DGroup>(FindObjectsSortMode.None);
        for (int i = 0; i < levelGroups.Length; i++)
        {
            if (levelGroups[i].GroupID == groupID)
            {
                levelGroups[i].gameObject.SetActive(true);
            }
            else
            {
                levelGroups[i].gameObject.SetActive(false);
            }
        }
        placePlayer(sceneName, true);
        finishBuild(stringName, Enums.SCENE_TYPE.Battle);
    }

    // Finishing loading
    // -- At this point, all managers should be ready, and should not have null reference --
    private void finishBuild(string stringName, Enums.SCENE_TYPE worldType)
    {
        PersistentDataManager.Instance.SetCamera();
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_SCENE_LOADED);
        // Hide Loaidng UI
        hideLoadingUI();

        if (worldType == Enums.SCENE_TYPE.Battle)
        {
            if (gameID2D < 0)
            {
                return;
            }
            // Unlock according to level type
            if (current2DLevel.Type == Enums.LEVEL_TYPE.Shooter)
            {
                InputManager.Instance.SetInputAsShooter();
            }
            // Create a 2D Game Manger
            GameObject managerObject = new GameObject("GameManager2D");
            managerObject.AddComponent<GameManager2D>();

            GameManager2D.Instance.SetGame(gameID2D, characterID);
        }
        else 
        {
            InputManager.Instance.UnLockInput(worldType);
        }
        
        // Resume the game
        PersistentGameManager.Instance.ResumeGame();

        // Show the map name
        ReminderManager.Instance.ShowMapNameReminder(stringName);
    }

    // ---Methods to rebuild the scene---
    private void placePlayer(string sceneName, bool bBattleLevel = false)
    {
        if (currentSceneType != Enums.SCENE_TYPE.Battle)
        {
            groupID = 0;
        }

        string playerPrefabName = "Player";
        if (bBattleLevel && characterID != -1)
        {
            Character2DData.Character2DDataStruct characterData = Character2DData.GetData(characterID);
            playerPrefabName = characterData.PrefabName;
        }
        SaveConfig.PlayerSaveData playerData = SaveManager.Instance.GetPlayer();
        GameObject playerObject;
        if (playerData.Scene == sceneName)
        {
            playerObject = PrefabManager.Instance.Instantiate(playerPrefabName, playerData.Position, Quaternion.identity);
        }
        else 
        {
            LevelData levelData = LevelConfig.Instance.GetLevelData(sceneName);
            // Place the player and the background according to the groupID
            playerObject = PrefabManager.Instance.Instantiate(playerPrefabName, levelData.SpawnPoints[groupID], Quaternion.identity);
            if (ParallaxScrollingBG.Instance != null)
            {
                Vector3 bgPosition = new Vector3(
                    levelData.SpawnPoints[groupID].x,
                    ParallaxScrollingBG.Instance.transform.position.y, 
                    ParallaxScrollingBG.Instance.transform.position.z
                    );
                ParallaxScrollingBG.Instance.SetUp(bgPosition);
            }
        }
        // Set Player to DataManager
        PersistentDataManager.Instance.SetPlayer(playerObject.GetComponent<Player>());
    }

    private void placeNPCs(string sceneName)
    {
        List<SaveConfig.NPCSaveData> npcList = SaveManager.Instance.GetNPCInfo();
        for (int i = 0; i < npcList.Count; i++)
        {
            if (npcList[i].bActive && npcList[i].Scene == sceneName)
            {
                NPCManager.Instance.SpawnNPC(npcList[i]);
            }
        }
    }
}
