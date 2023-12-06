using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Panel of choosing characters
/// </summary>
public class UI_CharacterPicker : UIBase
{
    // Character List
    [SerializeField] private ListViewIcons CharactersList;
    [SerializeField] private GameObject viewPort;
    private ObservableList<ListViewIconsItemDescription> characterItems = new ObservableList<ListViewIconsItemDescription>();

    // UIs
    [SerializeField] private TextMeshProUGUI TB_CharacterName;
    [SerializeField] private GameObject TB_CharacterLocked;
    [SerializeField] private TextMeshProUGUI TB_CharacterInfo;
    [SerializeField] private Button Btn_Confirmed;

    // Data
    [SerializeField, ReadOnly]
    private int levelID = 1;
    [SerializeField, ReadOnly]
    private int characterID = 1;
    [SerializeField, ReadOnly]
    private Enums.LEVEL_TYPE levelType;

    // Lock info
    private List<int> lockList;

    private void Awake()
    {
        CharactersList.DataSource = characterItems;
    }

    // Private:
    // Refresh List
    private void refreshList()
    {
        // Get locking info from save system
        lockList = SaveManager.Instance.GetCurrentCharacterLock(SaveManager.Instance.GetCurrentDay(), levelType);
        if (lockList == null)
        {
            lockList = new List<int>();
        }
        // Clear old data
        characterItems.Clear();
        for (int i = 1; i <= Character2DData.data.Count; i++)
        {
            AddToCharacterList(i);
        }
    }

    // Refresh Panel
    private void refreshPanel(int characterID)
    {
        // Config character availability
        if (!lockList.Contains(characterID))
        {
            Btn_Confirmed.interactable = true;
            TB_CharacterLocked.SetActive(false);
        }
        else 
        {
            Btn_Confirmed.interactable = false;
            TB_CharacterLocked.SetActive(true);
        }
        Character2DData.Character2DDataStruct characterData = Character2DData.GetData(characterID);
        TB_CharacterName.text = characterData.Name;
        TB_CharacterInfo.text = StringConstData.GetData(characterData.InfoText).Content;
    }

    // Public:
    // Set up
    public void SetUpPanel(int _levelID)
    {
        levelID = _levelID;
        levelType = Level2DData.GetData(levelID).Type;
        refreshList();
        refreshPanel(1);
    }

    // Enter Level
    public void EnterLevel()
    {
        if (characterID != -1)
        {
            LevelManager.Instance.Load2DLevel(levelID, characterID);
        }
    }

    // Add to list
    public void AddToCharacterList(int characterID)
    {
        if (!characterItems.Exists(item => item.Value == characterID))
        {
            Character2DData.Character2DDataStruct characterData = Character2DData.GetData(characterID);
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = characterID, Name = characterData.Name };
            characterItems.Add(newItem);
        }
    }

    // OnClick Events:
    public void OnClick_CharactersList(int index, ListViewItem item, PointerEventData eventData)
    {
        characterID = characterItems[index].Value;
        refreshPanel(characterID);
    }

    public void OnClick_Btn_Confirm()
    {
        EnterLevel();
    }

    public void OnClick_Btn_Cancel()
    {
        CharacterManager.Instance.HideCharacterPickerPanel();
    }
}
