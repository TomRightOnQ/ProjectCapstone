using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // UI - Timer
    [SerializeField] private GameObject hudTimer;
    [SerializeField] private TextMeshProUGUI timerText;
    // HUD Animation
    [SerializeField] private Animator upperRightAnimator;

    // Public:
    // Hide all HUD
    public void HideAllHUD()
    {
        if (bHidden)
        {
            return;
        }
        bHidden = true;
        HideUpperRightHUD();
    }

    public void ShowAllHUD()
    {
        if (!bHidden)
        {
            return;
        }
        bHidden = false;
        ShowUpperRightHUD();
    }

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

    // OnClick Event:
    public void OnClick_Btn_Menu()
    {
        MenuManager.Instance.OpenMenu();
        HUDManager.Instance.HideAllHUD();
    }

    // Reveal Upper Right HUD
    public void ShowUpperRightHUD()
    {
        upperRightAnimator.Play("HUDUpperRightShow");
    }

    // Hide Upper Right HUD
    public void HideUpperRightHUD() 
    {
        upperRightAnimator.Play("HUDUpperRightHide");
    }

    // Reveal Upper Left HUD
    public void ShowUpperLeftHUD()
    {
        upperLeftHUD.SetActive(true);
    }

    // Hide Upper Left HUD
    public void HideUpperLeftHUD()
    {
        upperLeftHUD.SetActive(false);
    }
}
