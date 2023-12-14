using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI for the beginning and the end of 2D level
/// </summary>

public class UI_Level2D : UIBase
{    
    // Panels - Start
    // Level
    [SerializeField] private Image Img_LevelIcon;
    [SerializeField] private TextMeshProUGUI TB_LevelTypeName;
    [SerializeField] private TextMeshProUGUI RTB_LevelInfo;
    [SerializeField] private TextMeshProUGUI TB_LevelName;

    // Character
    [SerializeField] private GameObject P_LevelStartPanel;
    [SerializeField] private TextMeshProUGUI TB_CharacterName;
    [SerializeField] private TextMeshProUGUI RTB_CharacterInfo;
    [SerializeField] private Image Img_CharacterIconSmall;

    // Panels - Complete
    [SerializeField] private GameObject P_LevelCompletePanel;
    // LeaderBoard LiseView
    [SerializeField] private ListViewIcons guildList;
    [SerializeField] private GameObject guildViewPort;
    private ObservableList<ListViewIconsItemDescription> guildItems = new ObservableList<ListViewIconsItemDescription>();

    private void Awake()
    {
        guildList.DataSource = guildItems;
    }

    // Private:
    // Config Start Panel
    private void configStartPanel()
    {
        // Load Level Info
        int levelID = GameManager2D.Instance.GameLevelID;
        Level2DData.Level2DDataStruct levelData = Level2DData.GetData(levelID);
        TB_LevelName.text = levelData.Name;
        TB_LevelTypeName.text = levelData.TypeText;
        TextAsset levelTextAsset = ResourceManager.Instance.LoadText(Constants.LEVEL2D_TEXT_SOURCE_PATH, levelData.IntroText);
        if (levelTextAsset != null)
        {
            RTB_LevelInfo.text = levelTextAsset.text;
        }

        Sprite levelIcon = ResourceManager.Instance.LoadImage(Constants.IMAGES_SOURCE_PATH, levelData.IconPath);
        if (levelIcon != null)
        {
            Img_LevelIcon.sprite = levelIcon;
        }

        // Load Character Info
        int characterID = GameManager2D.Instance.CharacterID;
        Character2DData.Character2DDataStruct characterData = Character2DData.GetData(characterID);
        TB_CharacterName.text = characterData.Name;
        TextAsset characterTextAsset = ResourceManager.Instance.LoadText(Constants.LEVEL2D_TEXT_SOURCE_PATH, characterData.InfoText);
        if (characterTextAsset != null)
        {
            RTB_CharacterInfo.text = characterTextAsset.text;
        }
        Sprite characterIcon = ResourceManager.Instance.LoadImage(Constants.IMAGES_SOURCE_PATH, characterData.IconPath);
        if (characterIcon != null)
        {
            Img_CharacterIconSmall.sprite = characterIcon;
        }
    }

    // Config End Panel
    private void configCompletePanel()
    {
        guildItems.Clear();
        // Fill leaderboard list
        List<(int, int)> guildList = GameManager2D.Instance.GameResult;
        List<SaveConfig.GuildSaveData> guildData = SaveConfig.Instance.GuildSaveDataList;
        for (int i = 0; i < guildList.Count; i++)
        {
            int currentGuildID = guildList[i].Item1;
            string currentGuildName = GuildInfoData.GetData(currentGuildID).Name;
            string itemName = string.Format(" <mspace=0.65em>{1,3}{0,-13}{1,3}{1,3}",
                i.ToString(),
                currentGuildName,
                guildList[i].Item2.ToString(),
                guildData[guildList[i].Item1].Score.ToString()
                );
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = i, Name = itemName };
            guildItems.Add(newItem);
        }
    }

    // Public:
    // Show Panels
    public void SetLevelCompletePanel(bool bShow)
    {
        configCompletePanel();
        P_LevelCompletePanel.SetActive(bShow);
    }

    public void SetLevelStartPanel(bool bShow)
    {
        configStartPanel();
        P_LevelStartPanel.SetActive(bShow);
    }

    // Process OnClick
    public void OnClick_Btn_StartGame()
    {
        GameManager2D.Instance.StartGame();
        SetLevelStartPanel(false);
    }

    public void OnClick_Btn_ReturnScene()
    {
        GameManager2D.Instance.LeaveGameScene();
    }
}
