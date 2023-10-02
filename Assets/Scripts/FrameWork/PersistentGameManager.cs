using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Core Object lives through the entire game run-time
/// Manager the game session's manager init
/// </summary>
 
public class PersistentGameManager : MonoBehaviour
{
    private static PersistentGameManager instance;
    public static PersistentGameManager Instance => instance;

    // public static event Action OnSceneLoaded;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
            EnterLoadGameScene();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public Methods

    // Load Scene
    public void LoadScene(string sceneName)
    {
        // Reset pooling system to clear references
        PrefabManager.Instance.ResetPooling();
        // Load the scene async
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // OnSceneLoaded?.Invoke();
        // Scene is loaded, now load managers
        LoadManagers();
    }

    // Private Methods

    // Enter Loading Stage
    // Only triggered once in boot up
    private void EnterLoadGameScene()
    {
        InitConfigs();
        InitManagers();
        PersistentGameManager.Instance.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    // Init Configs
    private void InitConfigs() 
    {
        // Configs
        Debug.Log("1. PrefabConfig Loading");
        PrefabConfig.Instance.Init();
        Debug.Log("PersistentGameManager Init: Configs Ready!");
    }

    // Init Managers
    private void InitManagers()
    {
        Debug.Log("1. GameEffectManager - Configs Loading");
        GameEffectManager.Instance.InitGameEffectConfigs();
        Debug.Log("PersistentGameManager Init: Managers Ready!");
    }

    // Load Managers
    private void LoadManagers()
    {
        // Managers
        Debug.Log("1. PrefabManager Loading");
        PrefabManager.Instance.InitPooling();

        Debug.Log("PersistentGameManager Load: Managers Ready!");
        // SceneManager.LoadScene("MainMenu");
    }
}
