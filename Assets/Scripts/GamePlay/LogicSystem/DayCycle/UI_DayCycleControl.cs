using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIWidgets;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_DayCycleControl : UIBase
{
    [SerializeField] private Button btn_NextDay;

    // Panels
    [SerializeField] private GameObject p_GuildRankingPanel;

    // Texts
    [SerializeField] private TextMeshProUGUI tb_Day_Text;

    // LeaderBoard LiseView
    [SerializeField] private ListViewIcons guildList;
    [SerializeField] private GameObject guildViewPort;
    private ObservableList<ListViewIconsItemDescription> guildItems = new ObservableList<ListViewIconsItemDescription>();

    // Data
    [SerializeField, ReadOnly] 
    private int flashBackTarget = 0;

    private void Awake()
    {
        guildList.DataSource = guildItems;
        CloseDayPanel();
    }

    // Public:
    // Show list of flashback
    public void ShowDayPanel()
    {
        p_GuildRankingPanel.SetActive(true);
        tb_Day_Text.text = "DAY: " + DayCycleManager.Instance.CurrentDay.ToString();
        initList();
    }

    // Close the list
    public void CloseDayPanel()
    {
        // Clear items
        guildItems.Clear();
        p_GuildRankingPanel.SetActive(false);
        PersistentGameManager.Instance.ResumeGame();
    }

    // Show button to the next day
    public void ShowNextDayButton()
    {
        btn_NextDay.gameObject.SetActive(true);
    }

    // Hide button to the next day
    public void HideNextDayButton()
    {
        btn_NextDay.gameObject.SetActive(false);
    }

    // Private:
    // Populate the list
    private void initList()
    {
        // Fill leaderboard list
        List<SaveConfig.GuildSaveData> guildList = SaveConfig.Instance.GuildSaveDataList;
        guildList.Sort((x, y) => y.Score.CompareTo(x.Score));

        int remainTeam = DayData.GetData(DayCycleManager.Instance.CurrentDay).RemainTeam;
        int eliminatedTeam = 16 - remainTeam;

        int rankingNumber = 1;

        for (int i = 0; i < 16; i++)
        {
            // Do not show the eliminated teams
            int currentGuildID = guildList[i].GuildID;

            if (SaveManager.Instance.CheckGuildEliminated(currentGuildID))
            {
                continue;
            }

            string itemName = string.Format(" <mspace=0.65em>{0,-4}{1,-13}{2,3}",
                rankingNumber.ToString(),
                GuildInfoData.GetData(guildList[i].GuildID).Name, 
                guildList[i].Score.ToString());
            // Highlight player team
            if (guildList[i].GuildID == 0)
            {
                itemName = "<color=orange>" + itemName + "</color>";
            }
            // Highlight the team that are on the edge of elimination
            if (i > 15 - eliminatedTeam)
            {
                itemName = "<color=red>" + itemName + "</color>";
            }

            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = i, Name = itemName };
            guildItems.Add(newItem);
            rankingNumber += 1;
        }
    }

    // OnClick Event:
    // Next Day
    public void OnClick_Btn_NextDay()
    {
        DayCycleManager.Instance.GoToNextDay();
    }

    // List of days
    public void OnClick_ConfirmFlashBack()
    {
        DayCycleManager.Instance.JumpToDay(flashBackTarget);
    }
}
