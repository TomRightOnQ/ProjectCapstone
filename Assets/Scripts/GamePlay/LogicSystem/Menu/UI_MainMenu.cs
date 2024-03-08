using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main Menu Components
/// </summary>
public class UI_MainMenu : UIBase
{
    // Main Menu Canvas is not supposed to appear anywhere except the main menu
    // Thus it's not managed by UIManager
    public override void SetUp(UIData uiData)
    {
        uiName = "UI_MainMenu";
    }

    // Main Menu Animator
    [SerializeField] private Animator mainMenuAnimator;

    // OnClick Events
    public void OnClick_Btn_EnterGame()
    {
        mainMenuAnimator.Play("MainMenuEnterClicked");
    }

    // Center Cluster
    public void OnClick_Btn_NewGame()
    {
        LevelManager.Instance.CreateNewGame();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_ContinueGame()
    {
        LevelManager.Instance.EnterGame();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_Settings()
    {
        SettingManager.Instance.ShowSettingPanel();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_Quit()
    {
        PersistentGameManager.Instance.QuitGame();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    // Side Cluster
    public void OnClick_Btn_Achievements()
    {
        GameEndManager.Instance.ShowGameAchievementPage();
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void OnClick_Btn_Credit()
    {

    }
}
