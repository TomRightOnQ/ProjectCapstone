using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager to control game config and settings
/// </summary>
public class SettingManager : MonoBehaviour
{
    private static SettingManager instance;
    public static SettingManager Instance => instance;

    // UI Components
    [SerializeField] private UI_Settings ui_Settings;

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

    // Config UI
    private void createUISetting()
    {
        GameObject uiObject = UIManager.Instance.CreateUI("UI_Settings");
        ui_Settings = uiObject.GetComponent<UI_Settings>();
    }

    // Public:
    // Open UI
    public void ShowSettingPanel()
    {
        if (ui_Settings == null)
        {
            createUISetting();
        }
        // Load Configuration to the panel
        ui_Settings.ShowSettingPanel();
        UIManager.Instance.ShowUI("UI_Settings");
    }
    public void HideSettingPanel()
    {
        UIManager.Instance.HideUI("UI_Settings");
        PersistentGameManager.Instance.ResumeGame();
    }
}
