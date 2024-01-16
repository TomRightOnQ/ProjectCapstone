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
        if (ui_HUD == null && LevelManager.Instance.CurrentSceneType != Enums.SCENE_TYPE.Outside)
        {

            GameObject uiObject = UIManager.Instance.CreateUI("UI_HUD");
            ui_HUD = uiObject.GetComponent<UI_HUD>();
            configEventHandlers();
        }
    }

    // Config events
    private void configEventHandlers()
    {
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);
    }

    // Public:
    // Acting Mode
    public void EnterActingMode()
    {
        HUDInteractionManager.Instance.DisableHUDInteractionUI();
        InputManager.Instance.LockInput();
        HideAllHUD();
    }

    public void ExitActingMode()
    {
        HUDInteractionManager.Instance.EnableHUDInteractionUI();
        InputManager.Instance.UnLockInput(LevelManager.Instance.CurrentSceneType);
        ShowAllHUD();
    }

    // Track Task
    public void UpdateHUDTaskTracking(int taskID)
    {
        ui_HUD.UpdateHUDTaskTracking(taskID);
    }

    // Clear Tracking
    public void ClearTracking()
    {
        ui_HUD.ClearTracking();
    }

    // Battle Timer:
    public void BeginHUDTimer()
    {
        ui_HUD.BeginHUDTimer();
    }

    public void EndHUDTimer()
    {
        ui_HUD.EndHUDTimer();
    }

    public void UpdateHUDTimer(float time)
    {
        ui_HUD.UpdateTimer(time);
    }

    // HUD visibility
    public void ShowAllHUD()
    {
        ui_HUD.ShowAllHUD();
    }

    public void HideAllHUD()
    {
        ui_HUD.HideAllHUD();
    }

    public void ShowUpperRightHUD()
    {
        ui_HUD.ShowUpperRightHUD();
    }

    public void HideUpperRightHUD()
    {
        ui_HUD.HideUpperRightHUD();
    }

    public void ShowUpperLeftHUD()
    {
        ui_HUD.ShowUpperLeftHUD();
    }

    public void HideUpperLeftHUD()
    {
        ui_HUD.HideUpperLeftHUD();
    }

    // Private:
    // Event Handlers
    private void OnRecv_SceneLoaded()
    {
        // Show upperleft UI in battle scene
        if (LevelManager.Instance.CurrentSceneType == Enums.SCENE_TYPE.Battle)
        {
            ShowUpperLeftHUD();
        }
    }

    public void A_Recv_PlayerHPChanged(float value, float maxValue)
    {
        
    }
}
