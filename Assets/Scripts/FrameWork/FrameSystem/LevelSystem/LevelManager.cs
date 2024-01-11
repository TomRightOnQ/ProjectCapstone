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

    // Cache if the current scene is savable
    [SerializeField, ReadOnly]
    private bool bSaveable = true;
    public bool bCurrentSceneSaveable => bSaveable;

    // Current 2D Level Info
    private Level2DData.Level2DDataStruct current2DLevel;

    // 2D Scene Game ID
    [SerializeField, ReadOnly]
    private int gameID2D = -1;
    [SerializeField, ReadOnly]
    private int characterID = -1;
    [SerializeField, ReadOnly]
    private int groupID = 0;

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

    public void EnterGame()
    {
        SaveManager.Instance.LoadGameSave(Constants.SAVE_CURRENT_SAVE);
        // Configs the scriptable object
        if (SaveConfig.Instance.AllowRewrite)
        {
            SaveManager.Instance.CreateNewSave();
        }
        SaveManager.Instance.LoadGameCoreSave();
        SaveConfig.PlayerSaveData playerData = SaveManager.Instance.GetPlayer();

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

    // Private:
    // Coroutines
    private IEnumerator loadSceneAsync(string sceneName)
    {
        // Start loading the scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Notify the beginning of scene loading
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_SCENE_UNLOADED);
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
            playerObject = PrefabManager.Instance.Instantiate(playerPrefabName, levelData.SpawnPoints[groupID], Quaternion.identity);
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
