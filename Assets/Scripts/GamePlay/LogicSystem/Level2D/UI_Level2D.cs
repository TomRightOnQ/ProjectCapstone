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

    // Panels - Character
    [SerializeField] private GameObject P_LevelStartPanel;
    [SerializeField] private TextMeshProUGUI TB_CharacterName;
    [SerializeField] private TextMeshProUGUI RTB_CharacterInfo;
    [SerializeField] private Image Img_CharacterIconSmall;

    // Panels - Complete
    [SerializeField] private GameObject P_LevelCompletePanel;
    [SerializeField] private TextMeshProUGUI TB_LevelName_E;
    // LeaderBoard LiseView
    [SerializeField] private ListViewIcons guildList;
    [SerializeField] private GameObject guildViewPort;
    private ObservableList<ListViewIconsItemDescription> guildItems = new ObservableList<ListViewIconsItemDescription>();

    // 2D UI - Boss HP and Boss Info
    [SerializeField] private GameObject P_BossInfo;
    [SerializeField] private Slider S_BossHPBar;
    [SerializeField] private TextMeshProUGUI TB_BossName;
    [SerializeField] private Animator bossInfoAnimator;

    // Text - Game Begin Counter
    [SerializeField] private GameObject TB_LevelBeginCountDown;
    [SerializeField] private TextMeshProUGUI TB_LevelBeginCountDown_text;
    // Text - Game End Text
    [SerializeField] private TextMeshProUGUI TB_GameOverText;

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

        int levelID = GameManager2D.Instance.GameLevelID;
        Level2DData.Level2DDataStruct levelData = Level2DData.GetData(levelID);
        TB_LevelName_E.text = levelData.Name;
        // Fill leaderboard list
        List<(int, int)> guildList = GameManager2D.Instance.GameResult;
        List<SaveConfig.GuildSaveData> guildData = SaveConfig.Instance.GuildSaveDataList;
        for (int i = 0; i < guildList.Count; i++)
        {
            int currentGuildID = guildList[i].Item1;
            string currentGuildName = GuildInfoData.GetData(currentGuildID).Name;
            string itemName = string.Format(" <mspace=0.65em>{0,-3}{1,-13} {2,3} {3,3}",
                (i + 1).ToString(),
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
    public void SetLevelCompletePanel(bool bShow, bool bVic = false)
    {
        // Show GameOver and then config the panel
        // Set the timer and await for game over
        // Active the string object
        if (bVic)
        {
            TB_GameOverText.text = "Level Clear";
        }
        else 
        {
            TB_GameOverText.text = "Game Over";
        }
        TB_GameOverText.gameObject.SetActive(true);
        StartCoroutine(endGameCoroutine(bShow));
    }

    private IEnumerator endGameCoroutine(bool bShow)
    {
        float currentTime = 2f;
        while (currentTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            currentTime -= 1f;
        }
        TB_GameOverText.gameObject.SetActive(false);
        configCompletePanel();
        P_LevelCompletePanel.SetActive(bShow);
    }

    public void SetLevelStartPanel(bool bShow)
    {
        configStartPanel();
        P_LevelStartPanel.SetActive(bShow);
    }

    // Show boss info
    public void ShowBossInfo()
    {
        S_BossHPBar.value = 1f;
        P_BossInfo.SetActive(true);
        Invoke("DisableBossHPAnimator", 2f);
    }

    // Disable the animator
    public void DisableBossHPAnimator() 
    {
        bossInfoAnimator.enabled = false;
    }

    // Hide boss info
    public void HideBossINfo()
    {
        P_BossInfo.SetActive(false);
        bossInfoAnimator.enabled = true;
    }

    // Update Boss HP
    public void UpdateBossHPBar(float hpRatio)
    {
        S_BossHPBar.value = Mathf.Clamp(hpRatio, 0f, 1f);
    }

    // Process OnClick
    public void OnClick_Btn_StartGame()
    {
        SetLevelStartPanel(false);
        // Set the timer and await for begin
        // Active the string object
        TB_LevelBeginCountDown.SetActive(true);
        StartCoroutine(beginGameCoroutine());
    }

    private IEnumerator beginGameCoroutine()
    {
        int currentTime = 3;
        while (currentTime > 0)
        {
            // Update the text
            TB_LevelBeginCountDown_text.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime -= 1;
        }
        TB_LevelBeginCountDown.SetActive(false);
        GameManager2D.Instance.StartGame();
    }

    public void OnClick_Btn_ReturnScene()
    {
        GameManager2D.Instance.LeaveGameScene();
    }
}
