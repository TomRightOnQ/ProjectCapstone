using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In-game Menu
/// </summary>
public class UI_Menu : UIBase
{
    // UI Menu Animation
    [SerializeField] private Animator menuAnimator;

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
    
    }

    public void OnClick_Btn_FlashBack()
    {

    }

    public void OnClick_Btn_Games()
    {

    }

    public void OnClick_Btn_Characters()
    {

    }

    public void OnClick_Btn_Settings()
    {

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
