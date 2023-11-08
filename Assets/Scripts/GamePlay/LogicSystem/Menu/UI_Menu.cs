using UnityEngine;
using TMPro;

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
    

    // Public:
    // Open Menu
    public void OpenMenu()
    {
        if (bLocked)
        {
            return;
        }
        bLocked = true;
        PersistentGameManager.Instance.PauseGame();
        menuAnimator.Play("MenuInAnimation");
    }

    public void CloseMenu()
    {
        if (!bLocked)
        {
            return;
        }
        menuAnimator.Play("MenuOutAnimation");
        float length = menuAnimator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("DisableMenu", length);
        PersistentGameManager.Instance.ResumeGame();
    }

    // Set the current day
    public void SetCurrentDayText(int currentDay)
    {
        tb_DayText.text = "Day: " + currentDay.ToString();
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

    // OnClick Events:
    public void OnClick_Btn_Calender()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_FlashBack()
    {
        MenuManager.Instance.CloseMenu();
        DayCycleManager.Instance.ShowFlashBackList();
    }

    public void OnClick_Btn_Notes()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_Maps()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_Characters()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_Settings()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_CloseMenu()
    {
        MenuManager.Instance.CloseMenu();
    }

    public void OnClick_Btn_MainMenu()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_MAIN_MENU);
    }
    // Private:

}
