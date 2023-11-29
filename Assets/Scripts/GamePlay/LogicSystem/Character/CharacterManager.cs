using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager class of character info and picker
/// </summary>
public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;
    public static CharacterManager Instance => instance;

    // UI Components
    [SerializeField] private UI_CharacterPicker UI_CharacterPicker;

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

    // Private:
    // Create UI
    private void createUI_CharacterPicker()
    {
        GameObject uiObject = UIManager.Instance.CreateUI("UI_CharacterPicker");
        UI_CharacterPicker = uiObject.GetComponent<UI_CharacterPicker>();
    }

    // Public:
    // Lock a character
    public void LockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        SaveManager.Instance.LockCharacter2D(characterID, levelType);
    }

    // Unlock a character
    public void UnockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)
    {
        SaveManager.Instance.UnlockCharacter2D(characterID, levelType);
    }

    // Show character picker
    public void ShowCharacterPickerPanel(int levelID = 1)
    {
        if (UI_CharacterPicker == null)
        {
            createUI_CharacterPicker();
        }
        UI_CharacterPicker.SetUpPanel(levelID);
        UIManager.Instance.ShowUI("UI_CharacterPicker");
    }

    // Hide character Picker
    public void HideCharacterPickerPanel()
    {
        if (UI_CharacterPicker == null)
        {
            return;
        }
        UIManager.Instance.HideUI("UI_CharacterPicker");
    }
}
