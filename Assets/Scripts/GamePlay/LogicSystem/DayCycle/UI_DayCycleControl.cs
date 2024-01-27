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
    [SerializeField] private GameObject p_FlashBackPanel;

    // Texts
    [SerializeField] private TextMeshProUGUI tb_Day_Text;

    // Day ListVIew
    [SerializeField] private ListViewIcons dayList;
    [SerializeField] private GameObject viewPort;
    private ObservableList<ListViewIconsItemDescription> dayItems = new ObservableList<ListViewIconsItemDescription>();

    // LeaderBoard LiseView
    [SerializeField] private ListViewIcons guildList;
    [SerializeField] private GameObject guildViewPort;
    private ObservableList<ListViewIconsItemDescription> guildItems = new ObservableList<ListViewIconsItemDescription>();

    // Data
    [SerializeField, ReadOnly] 
    private int flashBackTarget = 0;

    private void Awake()
    {
        dayList.DataSource = dayItems;
        guildList.DataSource = guildItems;
        CloseDayPanel();
    }

    // Public:
    // Show list of flashback
    public void ShowDayPanel()
    {
        p_FlashBackPanel.SetActive(true);
        tb_Day_Text.text = "DAY: " + DayCycleManager.Instance.CurrentDay.ToString();
        initList();
    }

    // Close the list
    public void CloseDayPanel()
    {
        // Clear items
        dayItems.Clear();
        guildItems.Clear();
        p_FlashBackPanel.SetActive(false);
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
        int maxDay = SaveManager.Instance.GetCurrentDay();
        // Add unlocked days to list
        for (int i = 1; i < maxDay + 1; i++)
        {
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = i, Name = "Day " + i.ToString() };
            dayItems.Add(newItem);
        }

        // Fill leaderboard list
        List<SaveConfig.GuildSaveData> guildList = SaveConfig.Instance.GuildSaveDataList;
        guildList.Sort((x, y) => y.Score.CompareTo(x.Score));
        for (int i = 0; i < guildList.Count; i++)
        {
            string itemName = string.Format(" <mspace=0.65em>{0,-13}{1,3}  {2,3}/{3,-3}", 
                GuildInfoData.GetData(guildList[i].GuildID).Name, 
                guildList[i].Score.ToString(), 
                guildList[i].DuelWin.ToString(), 
                guildList[i].DuelLose.ToString());
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = i, Name = itemName };
            guildItems.Add(newItem);
        }
    }

    // OnClick Event:
    // Next Day
    public void OnClick_Btn_NextDay()
    {
        DayCycleManager.Instance.GoToNextDay();
    }

    // List of days
    public void OnClick_DayList(int index, ListViewItem item, PointerEventData eventData)
    {
        if (item != null)
        {
            flashBackTarget = dayItems[item.Index].Value;
        }
    }

    public void OnClick_ConfirmFlashBack()
    {
        DayCycleManager.Instance.JumpToDay(flashBackTarget);
    }
}
