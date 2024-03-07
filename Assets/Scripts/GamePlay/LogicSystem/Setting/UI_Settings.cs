using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Setting page panel
/// </summary>
public class UI_Settings : UIBase
{
    // Panels
    [SerializeField] private List<GameObject> panelList = new List<GameObject>();

    // Audio Sliders
    [SerializeField] private Slider SL_MasterVolumeSlider;
    [SerializeField] private Slider SL_MusicVolumeSlider;
    [SerializeField] private Slider SL_SFXVolumeSlider;

    [SerializeField] private TextMeshProUGUI TB_MasterVolume;
    [SerializeField] private TextMeshProUGUI TB_MusicVolume;
    [SerializeField] private TextMeshProUGUI TB_SFXVolume;
    // GrphicsLevel
    [SerializeField] private Toggle screenShake;
    [SerializeField] private ToggleGroupIndex graphicsLevelGroup;

    // Private:
    // Switch to certain setting panel
    private void switchToPanel(int index)
    {
        for (int i = 0; i < panelList.Count; i++)
        {
            if (i == index)
            {
                panelList[i].SetActive(true);
            }
            else 
            {
                panelList[i].SetActive(false);
            }
        }
    }

    // Private:
    // Apply Graphic Level
    private void applyGraphicLevel()
    {
        EventManager.Instance.PostEvent(GameEvent.Event.GRAPHICS_LEVEL_CHANGED);

        foreach (ScriptableLight lightComponnet in FindObjectsByType<ScriptableLight>(FindObjectsSortMode.None))
        {
            if (lightComponnet.gameObject.activeSelf)
            {
                lightComponnet.ChangeLevel();
            }
        }
    }

    // Public:
    // Show and Hide Panel
    public void ShowSettingPanel()
    {
        switchToPanel(0);
        LoadData();
    }

    public void HideSettingPanel()
    {
        SettingManager.Instance.HideSettingPanel();
    }

    // Save Data
    public void SaveData()
    {
        // Audio Levels
        PlayerPrefs.SetFloat("audioMaster", SL_MasterVolumeSlider.value);
        PlayerPrefs.SetFloat("audioMusic", SL_MusicVolumeSlider.value);
        PlayerPrefs.SetFloat("audioSFX", SL_SFXVolumeSlider.value);

        // Graphic Levels
        PlayerPrefs.SetInt("graphicLevel", graphicsLevelGroup.CurrentIndex);
        // Screen Shake
        PlayerPrefs.SetInt("bScreenShake", (screenShake.isOn ? 1 : 0));

        ApplySettings();
        applyGraphicLevel();
    }

    // Load Data
    public void LoadData()
    {
        // Audio Levels
        SL_MasterVolumeSlider.value = PlayerPrefs.GetFloat("audioMaster", SL_MasterVolumeSlider.value);
        SL_MusicVolumeSlider.value = PlayerPrefs.GetFloat("audioMusic", SL_MasterVolumeSlider.value);
        SL_SFXVolumeSlider.value = PlayerPrefs.GetFloat("audioSFX", SL_MasterVolumeSlider.value);

        // Graphic Levels
        int graphicLevel = PlayerPrefs.GetInt("graphicLevel", graphicsLevelGroup.CurrentIndex);
        graphicsLevelGroup.SetIndex(graphicLevel);

        // Screen Shake
        int currentScreenShake = PlayerPrefs.GetInt("bScreenShake", (screenShake.isOn ? 1 : 0));
        screenShake.isOn = (currentScreenShake == 1);

        ApplySettings();
        applyGraphicLevel();
    }

    // Apply Settings
    public void ApplySettings()
    {
        // Audio Levels
        TB_MasterVolume.text = ((int)Mathf.Floor(SL_MasterVolumeSlider.value * 100)).ToString();
        TB_MusicVolume.text = ((int)Mathf.Floor(SL_MusicVolumeSlider.value * 100)).ToString();
        TB_SFXVolume.text = ((int)Mathf.Floor(SL_SFXVolumeSlider.value * 100)).ToString();
        MusicManager.Instance.SetMasterBus(SL_MasterVolumeSlider.value);
        MusicManager.Instance.SetMusicBus(SL_MusicVolumeSlider.value);
        MusicManager.Instance.SetSFXBus(SL_SFXVolumeSlider.value);

        // Screen Shake
        if (PersistentDataManager.Instance.MainCamera != null)
        {
            PersistentDataManager.Instance.MainCamera.SetScreenShake(screenShake.isOn);
        }
    }

    // Click Event
    // Choose panel options
    public void OnClick_Options(int index)
    {
        switchToPanel(index);
    }

    public void OnClick_Btn_Save()
    {
        SaveData();
        HideSettingPanel();
    }

    public void OnClick_Btn_Cancel()
    {
        // Revert the volume
        MusicManager.Instance.SetMasterBus(PlayerPrefs.GetFloat("audioMaster"));
        MusicManager.Instance.SetMusicBus(PlayerPrefs.GetFloat("audioMusic"));
        MusicManager.Instance.SetSFXBus(PlayerPrefs.GetFloat("audioSFX"));
    }

    // Change Volume
    public void OnChange_SL_Volume()
    {
        ApplySettings();
    }

    // Toggle Screen Shake
    public void OnToggle_TG_GLevel_P(int index)
    {
        graphicsLevelGroup.SetIndex(index);
    }
}
