using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        upperLeftHUD.SetActive(false);
    }

    // Hide Upper Left HUD
    public void HideUpperLeftHUD()
    {
        upperLeftHUD.SetActive(true);
    }
}
