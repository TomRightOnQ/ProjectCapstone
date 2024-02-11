using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// HUD UI
/// </summary>
/// 
public class UI_HUD : UIBase
{
    // Flags
    [SerializeField, ReadOnly]
    private bool bHidden = false;

    // UI - Upper Right Section
    [SerializeField] private GameObject upperRightHUD;

    // UI - Upper Left Section
    [SerializeField] private GameObject upperLeftHUD;

    // UI - Virtual Group of task indicators
    [SerializeField] private GameObject P_TaskIndicatorGroup;

    // UI - Timer
    [SerializeField] private GameObject hudTimer;
    [SerializeField] private TextMeshProUGUI timerText;

    // UI - HP
    [SerializeField] private Image Img_HP;

    // UI - Damage
    [SerializeField] private GameObject P_PlayerDamaged;

    // HUD Animation
    [SerializeField] private Animator upperRightAnimator;
    [SerializeField] private Animator upperLeftAnimator;
    [SerializeField] private Animator playerHPAnimator;
    [SerializeField] private Animator wideScreenAnimator;
    [SerializeField] private Animation playerDamaged;

    // UI - Task Information
    [SerializeField] private TextMeshProUGUI TB_TaskName;
    [SerializeField] private TextMeshProUGUI TB_TaskScene;
    [SerializeField] private TextMeshProUGUI TB_TaskBrief;

    // Events of removing wide screen
    private GameEvent.Event wideScreenShowEvent = GameEvent.Event.EVENT_WIDE_SCREEN_BEGIN;
    private GameEvent.Event wideScreenHideEvent = GameEvent.Event.EVENT_WIDE_SCREEN_END;

    // Wide screen objects
    [SerializeField] private GameObject P_WideScreen;

    private void Awake()
    {
        EventManager.Instance.AddListener(wideScreenShowEvent, ShowWideScreen);
        EventManager.Instance.AddListener(wideScreenHideEvent, HideWideScreen);
    }

    // Private:
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(wideScreenShowEvent, ShowWideScreen);
        EventManager.Instance.RemoveListener(wideScreenHideEvent, HideWideScreen);
    }

    // Public:
    /// <summary>
    /// HUD Main Clusters Controls
    /// </summary>
    // Hide all HUD
    public void HideAllHUD()
    {
        if (bHidden)
        {
            return;
        }
        bHidden = true;
        HideUpperRightHUD();
        P_TaskIndicatorGroup.SetActive(false);
    }

    public void ShowAllHUD()
    {
        if (!bHidden)
        {
            return;
        }
        bHidden = false;
        ShowUpperRightHUD();
        P_TaskIndicatorGroup.SetActive(true);
    }

    // Reveal Upper Right HUD
    public void ShowUpperRightHUD()
    {
        P_TaskIndicatorGroup.SetActive(true);
        upperRightAnimator.Play("HUDUpperRightShow");
    }

    // Hide Upper Right HUD
    public void HideUpperRightHUD() 
    {
        P_TaskIndicatorGroup.SetActive(false);
        upperRightAnimator.Play("HUDUpperRightHide");
    }

    // Reveal Upper Left HUD
    public void ShowUpperLeftHUD()
    {
        upperLeftHUD.SetActive(true);
        upperLeftAnimator.Play("HUDUpperLeftShow");
    }

    // Hide Upper Left HUD
    public void HideUpperLeftHUD()
    {
        upperLeftHUD.SetActive(false);
        upperLeftAnimator.Play("HUDUpperLeftHide");
    }

    // Set Task Indicator
    public void SetTrackIndicator(Transform indicatorTransform)
    {
        indicatorTransform.SetParent(P_TaskIndicatorGroup.transform, false);
    }

    /// <summary>
    /// HUD Functional Controls
    /// </summary>
    // Battle Timer:
    public void BeginHUDTimer()
    {
        hudTimer.SetActive(true);
    }

    public void EndHUDTimer()
    {
        hudTimer.SetActive(false);
    }

    public void UpdateTimer(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        string formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";
        timerText.text = formattedTime;
    }

    // Player Damaged
    public void PlayPlayerDamagedScreenEffect()
    {
        P_PlayerDamaged.SetActive(true);
        playerDamaged.Play();
    }

    // HP Speed
    public void AdjustHPSpeed(float ratio)
    {
        // Use 1 - ratio times constant as the new speed
        playerHPAnimator.speed = (1 - ratio) * 2 + 1;

        // Adjust HP Color based on the ratio
        float redValue = 1 - ratio;
        float greenValue = ratio;
        Img_HP.color = new Color(redValue, greenValue, 0);
    }

    // Task Tracking
    // Update Tracking
    public void UpdateHUDTaskTracking(int taskID)
    {
        // Retrieve Task Metadata
        TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
        TB_TaskName.text = taskData.Name;
        TB_TaskBrief.text = taskData.Description;
        if (taskData.SceneName != "None")
        {
            TB_TaskScene.text = LevelConfig.Instance.GetLevelData(taskData.SceneName).StringName;
        }
        else 
        {
            TB_TaskScene.text = "";
        }
    }

    // Clear Tracking
    public void ClearTracking()
    {
        TB_TaskName.text = "";
        TB_TaskScene.text = "";
        TB_TaskBrief.text = "";
    }

    // Event Handlers
    public void HideWideScreen()
    {
        wideScreenAnimator.Play("Hide");
    }

    public void ShowWideScreen()
    {
        wideScreenAnimator.Play("Show");
    }

    // OnClick Events
    // TaskInfo - Button to open the task board
    public void OnClick_Btn_OpenTask()
    {
        TaskManager.Instance.ShowTaskPanel();
    }

    // Menu - Open Menu
    public void OnClick_Btn_Menu()
    {
        MenuManager.Instance.OpenMenu();
        HUDManager.Instance.HideAllHUD();
    }
}
