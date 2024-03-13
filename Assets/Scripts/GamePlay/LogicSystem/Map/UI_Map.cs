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
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CLOSE);
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
