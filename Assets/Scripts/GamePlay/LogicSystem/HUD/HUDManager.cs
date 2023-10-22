using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behaivour of the in-game HUD
/// </summary>
public class HUDManager : MonoBehaviour
{
    private static HUDManager instance;
    public static HUDManager Instance => instance;

    // UI Components
    [SerializeField] private UI_HUD ui_HUD;

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
        if (ui_HUD == null)
        {

            GameObject uiObject = UIManager.Instance.CreateUI("UI_HUD");
            ui_HUD = uiObject.GetComponent<UI_HUD>();
        }
    }

    // Public:
    // Show all HUD
    public void ShowAllHUD()
    {
        ui_HUD.ShowAllHUD();
    }

    // Hide all HUD
    public void HideAllHUD()
    {
        ui_HUD.HideAllHUD();
    }
}
