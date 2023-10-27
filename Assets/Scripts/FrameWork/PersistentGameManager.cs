using UnityEngine;


/// <summary>
/// Core Object lives through the entire game run-time
/// Manager the game session's manager init
/// </summary>
 
public class PersistentGameManager : MonoBehaviour
{
    private static PersistentGameManager instance;
    public static PersistentGameManager Instance => instance;

    [SerializeField] private bool bPaused = false;
    public bool bGamePaused => bPaused;

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

    // Private Methods

    // Enter Loading Stage
    // Only triggered once in boot up
    private void EnterLoadGameScene()
    {
        InitConfigs();
        InitManagers();
        LevelManager.Instance.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    // Init Configs
    private void InitConfigs() 
    {
        // Configs
        Debug.Log("0. MusicConfig Loading");
        MusicConfig.Instance.Init();

        Debug.Log("1. PrefabConfig Loading");
        PrefabConfig.Instance.Init();
        Debug.Log("2. LevelConfig Loading");
        LevelConfig.Instance.Init();
        Debug.Log("3. UIConfig Loading");
        UIConfig.Instance.Init();
        Debug.Log("PersistentGameManager Init: Configs Ready!");
    }

    // Init Managers
    private void InitManagers()
    {
        Debug.Log("1. GameEffectManager - Configs Loading");
        GameEffectManager.Instance.InitGameEffectConfigs();
        Debug.Log("PersistentGameManager Init: Managers Ready!");
    }

    // Public:
    // Load Managers
    public void LoadManagers()
    {
        // Managers
        Debug.Log("1. PrefabManager Loading");
        PrefabManager.Instance.InitPooling();

        Debug.Log("2. UIManager Loading");
        UIManager.Instance.Init();

        Debug.Log("3. NPCManager Loading");
        NPCManager.Instance.Init();

        Debug.Log("3. ChatInteractionManager Loading");
        ChatInteractionManager.Instance.Init();

        Debug.Log("4. HUDInteractionManager Loading");
        HUDInteractionManager.Instance.Init();

        Debug.Log("5. DayCycleManager Loading");
        DayCycleManager.Instance.Init();

        Debug.Log("6 HUDManager Loading");
        HUDManager.Instance.Init();

        Debug.Log("7 MenuManager Loading");
        MenuManager.Instance.Init();

        Debug.Log("PersistentGameManager Load: Managers Ready!");
        // SceneManager.LoadScene("MainMenu");
    }

    // Pause Game
    public void PauseGame()
    {
        bPaused = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        bPaused = false;
        Time.timeScale = 1;
    }
}
