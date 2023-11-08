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

    // 2D Scene Game ID
    private int gameID2D = -1;

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
    public void Load2DLevel(int levelID)
    {
        if (levelID < 0)
        {
            return;
        }
        Level2DData.Level2DDataStruct level2DData = Level2DData.GetData(levelID);
        gameID2D = levelID;
        LevelManager.Instance.LoadScene(level2DData.SceneName);
    }

    // For single save mode - Check and see if there's a save
    public void EnterGame()
    {
        // Configs the scriptable object
        if (!SaveConfig.Instance.AllowRewrite)
        {
            SaveManager.Instance.LoadSave();
        }
        else
        {
            SaveManager.Instance.CreateNewSave();
        }
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
        placePlayer(sceneName, true);
        finishBuild(stringName, Enums.SCENE_TYPE.Battle);
    }

    // Finishing loading
    // -- At this point, all managers should be ready, and should not have null reference --
    private void finishBuild(string stringName, Enums.SCENE_TYPE worldType)
    {
        PersistentDataManager.Instance.SetCamera();
        InputManager.Instance.UnLockInput(worldType);
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_SCENE_LOADED);

        // Resume the game
        PersistentGameManager.Instance.ResumeGame();

        if (worldType == Enums.SCENE_TYPE.Battle)
        {
            if (gameID2D < 0)
            {
                return;
            }
            // Create a 2D Game Manger
            GameObject managerObject = new GameObject("GameManager2D");
            managerObject.AddComponent<GameManager2D>();

            GameManager2D.Instance.SetGame(gameID2D);
        }

        // Show the map name
        ReminderManager.Instance.ShowMapNameReminder(stringName);
    }

    // ---Methods to rebuild the scene---
    private void placePlayer(string sceneName, bool bBattleLevel = false)
    {
        string playerPrefabName = "Player";
        if (bBattleLevel)
        {
            playerPrefabName = "2DPlayer";
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
            playerObject = PrefabManager.Instance.Instantiate(playerPrefabName, levelData.SpawnPoints[0], Quaternion.identity);
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
