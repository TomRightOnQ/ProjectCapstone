using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// In-game Menu
/// </summary>
public class UI_Menu : UIBase
{
    // UI Menu Animation
    [SerializeField] private Animator menuAnimator;

    // Data
    [SerializeField] private TextMeshProUGUI tb_DayText;

    // Flags
    [SerializeField, ReadOnly]
    private bool bLocked = false;

    // List of functional buttons
    [SerializeField]
    private List<Button> functionalButtonList = new List<Button>();

    // Confirm Window
    [SerializeField] private GameObject P_ConfirmWindow;
    [SerializeField] private TextMeshProUGUI TB_Content;

    // Assign different actions to the same confirm window
    private System.Action confirmAction;

    // Open Menu
    public void OpenMenu()
    {
        if (bLocked)
        {
            return;
        }
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_OPEN);
        bLocked = true;
        PersistentGameManager.Instance.PauseGame();
        // Check which option to show
        configLockedModule();
        menuAnimator.Play("MenuInAnimation");
    }

    public void CloseMenu()
    {
        if (!bLocked)
        {
            return;
        }
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CLOSE);
        menuAnimator.Play("MenuOutAnimation");
        float length = menuAnimator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("DisableMenu", length);
        PersistentGameManager.Instance.ResumeGame();
    }

    public void CloseMenuNoResume()
    {
        if (!bLocked)
        {
            return;
        }
        menuAnimator.Play("MenuOutAnimation");
        float length = menuAnimator.GetCurrentAnimatorStateInfo(0).length;
        DisableMenu();
    }

    // Set all buttons except the main menu button locked or unlocked
    public void SetFunctionalButtonState(bool bActive)
    {
        for (int i = 0; i < functionalButtonList.Count; i++)
        {
            functionalButtonList[i].interactable = bActive;
        }
    }

    // Set the current day
    public void SetCurrentDayText(int currentDay)
    {
        tb_DayText.text = "DAY: " + currentDay.ToString();
    }

    void PauseGame()
    {
        PersistentGameManager.Instance.PauseGame();
    }

    void DisableMenu()
    {
        bLocked = false;
        gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        if (!bLocked)
        {
            OpenMenu();
        }
        else 
        {
            CloseMenu();
        }
    }

    // Event delegates
    private void PerformFlashBack()
    {
        DayCycleManager.Instance.FlashBack();
        CloseMenu();
    }

    private void QuitToMainMenu()
    {
        CloseMenu();
        LevelManager.Instance.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    // OnClick Events:
    public void OnClick_Btn_Calender()
    {
        DayCycleManager.Instance.ShowDayPanel();
        MenuManager.Instance.CloseMenuNoResume();
    }

    public void OnClick_Btn_FlashBack()
    {
        confirmAction = PerformFlashBack;
        TB_Content.text = StringConstData.GetData(4).Content;
        P_ConfirmWindow.SetActive(true);
    }

    public void OnClick_Btn_Notes()
    {
        NotesManager.Instance.ShowNotePanel();
        MenuManager.Instance.CloseMenuNoResume();
    }

    public void OnClick_Btn_Maps()
    {
        MapManager.Instance.ShowMapPanel();
        MenuManager.Instance.CloseMenuNoResume();
    }

    public void OnClick_Btn_Characters()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_Settings()
    {
        SettingManager.Instance.ShowSettingPanel();
        MenuManager.Instance.CloseMenuNoResume();
    }

    public void OnClick_Btn_CloseMenu()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_MainMenu()
    {
        confirmAction = QuitToMainMenu;
        TB_Content.text = StringConstData.GetData(6).Content;
        P_ConfirmWindow.SetActive(true);
    }

    public void OnClick_Btn_Achievements()
    {
        MenuManager.Instance.CloseMenuNoResume();
        GameEndManager.Instance.ShowGameAchievementPage();
    }

    public void OnClick_Btn_ConfirmWindow()
    {
        MenuManager.Instance.CloseMenu();
        confirmAction?.Invoke();
    }

    // Private:
    // Check if options are locked
    private void configLockedModule()
    {
        if (SaveConfig.Instance.MenuModuleLockList.Count >= functionalButtonList.Count)
        {
            for (int i = 0; i < functionalButtonList.Count; i++)
            {
                if (SaveConfig.Instance.MenuModuleLockList[i])
                {
                    functionalButtonList[i].gameObject.SetActive(false);
                }
                else 
                {
                    functionalButtonList[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
