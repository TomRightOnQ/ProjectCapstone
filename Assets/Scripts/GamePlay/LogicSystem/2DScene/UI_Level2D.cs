using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI for the beginning and the end of 2D level
/// </summary>

public class UI_Level2D : UIBase
{
    // Panels
    [SerializeField] private GameObject P_LevelCompletePanel;
    [SerializeField] private GameObject P_LevelStartPanel;

    // Public:
    // Show Panels
    public void SetLevelCompletePanel(bool bShow)
    {
        P_LevelCompletePanel.SetActive(bShow);
    }

    public void SetLevelStartPanel(bool bShow)
    {
        P_LevelStartPanel.SetActive(bShow);
    }

    // Process OnClick
    public void OnClick_Btn_StartGame()
    {
        GameManager2D.Instance.StartGame();
        SetLevelStartPanel(false);
    }

    public void OnClick_Btn_ReturnScene()
    {
        GameManager2D.Instance.LeaveGameScene();
    }
}
