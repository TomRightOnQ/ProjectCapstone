using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIWidgets;
using UnityEngine.EventSystems;

public class UI_DayCycleControl : UIBase
{
    [SerializeField] private Button btn_NextDay;

    // Panels
    [SerializeField] private GameObject p_FlashBackPanel;
    [SerializeField] private bool bListInited = false; // Because the list is thrown away upon scene changes or day changes, use a flag to mark inited

    // Day ListVIew
    [SerializeField] private ListViewIcons dayList;
    [SerializeField] private GameObject viewPort;
    private ObservableList<ListViewIconsItemDescription> dayItems = new ObservableList<ListViewIconsItemDescription>();

    // Data
    [SerializeField, ReadOnly] 
    private int flashBackTarget = 0;

    private void Awake()
    {
        dayList.DataSource = dayItems;
        CloseFlashBackList();
    }


    // Public:
    // Show list of flashback
    public void ShowFlashBackList()
    {
        p_FlashBackPanel.SetActive(true);
        if (!bListInited)
        {
            bListInited = true;
            initList();
        }
    }

    // Close the list
    public void CloseFlashBackList()
    {
        p_FlashBackPanel.SetActive(false);
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
        int currentDay = DayCycleManager.Instance.CurrentDay;
        // Add to list
        for (int i = 0; i < currentDay + 1; i++)
        {
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = i, Name = "Day " + i.ToString() };
            dayItems.Add(newItem);
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
