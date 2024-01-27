using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager class for the map system
/// </summary>
public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance => instance;

    // UI Component
    [SerializeField] private UI_Map ui_Map;

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

    // Private:
    // Init UI
    private void createUIMap()
    {
        if (ui_Map == null)
        {
            GameObject uiObject = UIManager.Instance.CreateUI("UI_Map");
            ui_Map = uiObject.GetComponent<UI_Map>();
        }
    }

    // Public:
    // Show Map Panel
    public void ShowMapPanel()
    {
        if (ui_Map == null)
        {
            createUIMap();
        }
        ui_Map.ShowMapPanel();
    }

    // Set Map teleport locked or unlocked
    public void SetMapTravelLockStatus(bool bLocked)
    {
        SaveManager.Instance.SetMapTravelLockStatus(bLocked);
    }

    // Set Map locked or unlocked
    public void SetMapLockStatus(bool bLocked, string mapName)
    {
        if (bLocked)
        {
            SaveManager.Instance.LockMap(mapName);
        }
        else 
        {
            SaveManager.Instance.UnlockMap(mapName);
        }
    }

    // Teleport via map
    public void TravelToLevel(string mapName)
    {
        // Check if Travel allowed
        if (SaveManager.Instance.CheckMapTravelLockStatus())
        {
            // Hint the player that teleport is banned
            return;
        }

        // Check if map locked
        if (SaveManager.Instance.CheckMapLocked(mapName))
        {
            // Hint the player with the lock
            return;
        }
        LevelManager.Instance.LoadScene(mapName);
    }
}
