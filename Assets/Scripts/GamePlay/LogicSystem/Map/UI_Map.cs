using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI Component of map system
/// </summary>
public class UI_Map : UIBase
{
    // Data
    // Record the currently selected location
    [SerializeField] private string currentSelectedLocation = Constants.SCENE_NONE;

    // UI Widgets
    [SerializeField] private GameObject P_MapPanel;

    // Public:
    // Show Map Panel
    public void ShowMapPanel()
    {
        P_MapPanel.SetActive(true);
    }

    // Close Map Panel
    public void CloseMapPanel()
    {
        P_MapPanel.SetActive(false);
        PersistentGameManager.Instance.ResumeGame();
    }

    // Show the level Detail Panel
    public void ShowMapDetail(string mapName)
    {
        currentSelectedLocation = mapName;
    }

    // Close the level Detail Panel
    public void CloseMapDetail()
    {
        currentSelectedLocation = Constants.SCENE_NONE;
    }

    // OnClick Events
    // Location selected
    public void OnClick_Btn_Location(string mapName)
    {
        ShowMapDetail(mapName);
    }

    // Teleport Button
    public void OnClick_Teleport()
    {
        MapManager.Instance.TravelToLevel(currentSelectedLocation);
    }

    public void OnClick_Teleport_Temp_1()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_AUDIENCELOW_LEVEL);
    }

    public void OnClick_Teleport_Temp_2()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_AUDIENCE_LEVEL);
    }

    public void OnClick_Teleport_Temp_3()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_ENTRANCE_LEVEL);
    }

    public void OnClick_Teleport_Temp_4()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_GUILD_LEVEL);
    }

    public void OnClick_Teleport_Temp_5()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_ROOMA_LEVEL);
    }
}
