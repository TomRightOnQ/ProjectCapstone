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
    public void LoadScene(string sceneName)
    {
        // Reset pooling system to clear references
        PrefabManager.Instance.ResetPooling();
        InputManager.Instance.LockInput();
        // Load the scene async
        StartCoroutine(loadSceneAsync(sceneName));
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
        string currentScene = playerData.Scene;
        LoadScene(currentScene);
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
        if (levelData.SceneType != Enums.SCENE_TYPE.Outside)
        {
            placePlayer(sceneName);
        }
        placeNPCs(sceneName);
        finishBuild(sceneName);
    }

    // Finishing loading
    // -- At this point, all managers should be ready, and should not have null reference --
    private void finishBuild(string sceneName)
    {
        PersistentDataManager.Instance.SetCamera();
        InputManager.Instance.UnLockInput();

        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_SCENE_LOADED);
    }

    // ---Methods to rebuild the scene---
    private void placePlayer(string sceneName)
    {
        SaveConfig.PlayerSaveData playerData = SaveManager.Instance.GetPlayer();
        GameObject playerObject;
        if (playerData.Scene == sceneName)
        {
            playerObject = PrefabManager.Instance.Instantiate("Player", playerData.Position, Quaternion.identity);
        }
        else 
        {
            LevelData levelData = LevelConfig.Instance.GetLevelData(sceneName);
            playerObject = PrefabManager.Instance.Instantiate("Player", levelData.SpawnPoints[0], Quaternion.identity);
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
