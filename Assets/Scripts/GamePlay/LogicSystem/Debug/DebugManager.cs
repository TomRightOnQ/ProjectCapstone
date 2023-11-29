using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GM Debug tool
/// </summary>
public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject p_DebugPanel;
    [SerializeField] private GameObject btn_DebugMenuButton;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void onClick()
    {
        p_DebugPanel.SetActive(false);
        btn_DebugMenuButton.SetActive(true);
    }

    public void ShowChat()
    {
        ChatInteractionManager.Instance.BeginInteraction(1);
        onClick();
    }

    public void SpawnNPC()
    {
        NPCManager.Instance.SpawnNPC(1);
        onClick();
    }

    public void EnterSceneAudience()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_AUDIENCE_LEVEL);
        onClick();
    }

    public void EnterSceneAudienceLow()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_AUDIENCELOW_LEVEL);
        onClick();
    }

    public void EnterEntrance()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_ENTRANCE_LEVEL);
        onClick();
    }

    public void EnterPlatformer()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_PLATFORMER_LEVEL);
        onClick();
    }

    public void EnterRoomA()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_ROOMA_LEVEL);
        onClick();
    }

    public void EnterGuild()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_GUILD_LEVEL);
        onClick();
    }

    public void EnterMatching()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_MATCHING_LEVEL);
        onClick();
    }

    public void EnterShooter()
    {
        CharacterManager.Instance.ShowCharacterPickerPanel(5);
        onClick();
    }
}
