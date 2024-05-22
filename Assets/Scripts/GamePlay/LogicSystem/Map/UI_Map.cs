using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI Component of map system
/// </summary>
public class UI_Map : UIBase
{
    // UI Widgets
    [SerializeField] private GameObject P_MapPanel;

    // Task Tracker
    [SerializeField] private GameObject Img_TaskTracking;

    // List of Tracking Points
    // Use LevelID as index
    [SerializeField] private List<Vector3> trackPoints;

    // Public:
    // Show Map Panel
    public void ShowMapPanel()
    {
        gameObject.SetActive(true);
        P_MapPanel.SetActive(true);
    }

    // Close Map Panel
    public void CloseMapPanel()
    {
        P_MapPanel.SetActive(false);
        PersistentGameManager.Instance.ResumeGame();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CLOSE);
    }

    // Set the on-map tracking
    public void SetMapTaskTracking(string sceneName)
    {
        int levelID = LevelConfig.Instance.GetLevelData(sceneName).LevelID;
        Img_TaskTracking.transform.localPosition = trackPoints[levelID];
        Img_TaskTracking.SetActive(true);
    }

    public void CancelMapTracking()
    {
        Img_TaskTracking.SetActive(false);
    }

    // OnClick Events
    // Teleport Button
    public void OnClick_Btn_AudienceLow()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_AUDIENCELOW_LEVEL);
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_Audience()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_AUDIENCE_LEVEL);
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_Matching()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_MATCHING_LEVEL);
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_Entrance()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_ENTRANCE_LEVEL);
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_Guild()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_GUILD_LEVEL);
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_RoomA()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_ROOMA_LEVEL);
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }
}
