using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behaivor of settings and menu
/// </summary>
public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance => instance;

    // UI Components
    [SerializeField] private UI_Menu ui_Menu;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        if (ui_Menu == null && LevelManager.Instance.CurrentSceneType != Enums.SCENE_TYPE.Outside)
        {
            GameObject uiObject = UIManager.Instance.CreateUI("UI_Menu");
            ui_Menu = uiObject.GetComponent<UI_Menu>();

            SetCurrentDayText(DayCycleManager.Instance.CurrentDay);

            uiObject.SetActive(false);
        }
    }

    // Public:
    // Open Menu
    public void OpenMenu()
    {
        if (ui_Menu != null)
        {
            ui_Menu.gameObject.SetActive(true);
            ui_Menu.OpenMenu();
            HUDManager.Instance.HideUpperRightHUD();
        }
    }

    // Close Menu
    public void CloseMenu()
    {
        if (ui_Menu != null)
        {
            ui_Menu.CloseMenu();
            HUDManager.Instance.ShowUpperRightHUD();
        }
    }

    // Toggle Menu
    public void ToggleMenu()
    {
        if (ui_Menu != null)
        {
            ui_Menu.ToggleMenu();
        }
    }

    // Set Current Day
    public void SetCurrentDayText(int currentDay) 
    {
        if (ui_Menu != null)
        {
            ui_Menu.SetCurrentDayText(currentDay);
        }
    }
}