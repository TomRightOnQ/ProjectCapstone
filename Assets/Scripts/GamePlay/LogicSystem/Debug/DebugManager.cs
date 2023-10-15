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
}
