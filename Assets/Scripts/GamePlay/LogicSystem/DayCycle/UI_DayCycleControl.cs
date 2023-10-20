using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DayCycleControl : UIBase
{
    [SerializeField] private Button btn_NextDay;
    [SerializeField] private TextMeshProUGUI tb_DayText;

    // Public:
    // Set the current day
    public void SetCurrentDayText(int currentDay)
    {
        tb_DayText.text = currentDay.ToString();
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

    // OnClick Event:
    public void OnClick_Btn_NextDay()
    {
        DayCycleManager.Instance.GoToNextDay();
    }
}
