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

    // Public:
    // Show Map Panel
    public void ShowMapPanel()
    {
    
    }

    // Close Map Panel
    public void CloseMapPanel()
    {
    
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
}
