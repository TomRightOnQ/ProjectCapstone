using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// UI Component for the reminder
/// </summary>
public class UI_Reminder : UIBase
{
    // General Reminder
    [SerializeField] private GameObject p_GeneralReminder;

    // Map Name
    [SerializeField] private GameObject p_MapReminder;
    [SerializeField] private TextMeshProUGUI tb_MapName;
    [SerializeField] private Animator mapAnimator;

    // Game Saving
    [SerializeField] private GameObject p_SavingReminder;
    [SerializeField] private Animator saveAnimator;

    // Achievements
    [SerializeField] private GameObject p_AchievementReminder;
    [SerializeField] private TextMeshProUGUI tb_AchievementName;
    [SerializeField] private Image img_Icon;
    [SerializeField] private Animator achievementAnimator;

    // List to hold reminders
    [SerializeField] private GameObject sv_ReminderList;

    // Public:
    // Show General Reminder - from pooling system
    public void ShowGeneralReminder(int id)
    {
        GameObject reminderObj = PrefabManager.Instance.Instantiate("P_GeneralReminder", Vector3.zero, Quaternion.identity);
        if (reminderObj == null || reminderObj.GetComponent<P_GeneralReminder>() == null)
        {
            Debug.LogWarning("Unable to accquire P_GeneralReminder from pooling");
            return;
        }
        reminderObj.transform.SetParent(sv_ReminderList.transform, false);
        reminderObj.SetActive(true);
        P_GeneralReminder reminder = reminderObj.GetComponent<P_GeneralReminder>();
        reminder.BeginGeneralReminder(id);
    }

    public void ShowGeneralReminder(string content, float life)
    {
        GameObject reminderObj = PrefabManager.Instance.Instantiate("P_GeneralReminder", Vector3.zero, Quaternion.identity);
        if (reminderObj == null || reminderObj.GetComponent<P_GeneralReminder>() == null)
        {
            Debug.LogWarning("Unable to accquire P_GeneralReminder from pooling");
            return;
        }
        reminderObj.transform.SetParent(sv_ReminderList.transform, false);
        reminderObj.SetActive(true);
        P_GeneralReminder reminder = reminderObj.GetComponent<P_GeneralReminder>();
        reminder.BeginGeneralReminder(content, life);
    }

    // Show Subtitle Reminder - from pooling system
    public void ShowSubtitleReminder(int id)
    {
        GameObject reminderObj = PrefabManager.Instance.Instantiate("P_SubtitleReminder", Vector3.zero, Quaternion.identity);
        if (reminderObj == null || reminderObj.GetComponent<P_SubtitleReminder>() == null)
        {
            Debug.LogWarning("Unable to accquire P_SubtitleReminder from pooling");
            return;
        }
        reminderObj.transform.SetParent(this.transform, false);
        reminderObj.transform.localPosition = new Vector3(0, -370, 0);
        reminderObj.SetActive(true);
        P_SubtitleReminder reminder = reminderObj.GetComponent<P_SubtitleReminder>();
        reminder.BeginSubtitleReminder(id);
    }

    public void ShowSubtitleReminder(string content, string speaker, float life, Enums.CHARACTER_TYPE speakerType)
    {
        GameObject reminderObj = PrefabManager.Instance.Instantiate("P_SubtitleReminder", Vector3.zero, Quaternion.identity);
        if (reminderObj == null || reminderObj.GetComponent<P_SubtitleReminder>() == null)
        {
            Debug.LogWarning("Unable to accquire P_SubtitleReminder from pooling");
            return;
        }
        reminderObj.transform.SetParent(this.transform, false);
        reminderObj.transform.localPosition = new Vector3(0, -370, 0);
        reminderObj.SetActive(true);
        P_SubtitleReminder reminder = reminderObj.GetComponent<P_SubtitleReminder>();
        reminder.BeginSubtitleReminder(content, speaker, life, speakerType);
    }

    // Show Map Reminder
    public void ShowMapReminder(string name)
    {
        p_MapReminder.SetActive(true);
        tb_MapName.text = name;

        mapAnimator.Play("In");
        StartCoroutine(closeMapReminder());
    }

    private IEnumerator closeMapReminder()
    {
        yield return new WaitForSecondsRealtime(Constants.REMINDER_LEVEL_TIME);
        HideMapReminder();
    }

    // Hide Map Reminder
    public void HideMapReminder()
    {
        mapAnimator.Play("Out");
        StopCoroutine(closeMapReminder());
        Invoke("DisableMapReminder", 0.5f);
    }

    // Disable Map Reminder
    public void DisableMapReminder()
    {
        p_MapReminder.SetActive(false);
    }

    // Show GameSaving Reminder
    public void ShowGameSavingReminder()
    {
        p_SavingReminder.SetActive(true);

        saveAnimator.Play("In");
        StartCoroutine(closeGameSavingReminder());
    }

    private IEnumerator closeGameSavingReminder()
    {
        yield return new WaitForSecondsRealtime(Constants.REMINDER_LEVEL_TIME);
        HideGameSavingReminder();
    }

    // Hide GameSaving Reminder
    public void HideGameSavingReminder()
    {
        saveAnimator.Play("Out");
        StopCoroutine(closeGameSavingReminder());
        Invoke("DisableGameSavingReminder", 0.5f);
    }

    // Disable GameSaving Reminder
    public void DisableGameSavingReminder()
    {
        p_SavingReminder.SetActive(false);
    }

    // Show WholeScreen Reminder
    public void ShowWholeScreenReminder(int id)
    {
        GameObject reminderObj = PrefabManager.Instance.Instantiate("P_WholeScreenReminder", Vector3.zero, Quaternion.identity);
        if (reminderObj == null || reminderObj.GetComponent<P_WholeScreenReminder>() == null)
        {
            Debug.LogWarning("Unable to accquire P_SubtitleReminder from pooling");
            return;
        }
        reminderObj.transform.SetParent(this.transform, false);
        reminderObj.transform.localPosition = new Vector3(0, -370, 0);
        reminderObj.SetActive(true);
        P_WholeScreenReminder reminder = reminderObj.GetComponent<P_WholeScreenReminder>();
        reminder.BeginReminder(id);
    }

    // Show a new achievement
    public void ShowAchievementReminder(int achID)
    {
        AchievementData.AchievementDataStruct achData = AchievementData.GetData(achID);
        tb_AchievementName.text = achData.Name;
        Sprite achIcon = ResourceManager.Instance.LoadImage(Constants.IMAGES_SOURCE_PATH, achData.Icon);
        if (achIcon != null)
        {
            img_Icon.sprite = achIcon;
        }
        p_AchievementReminder.SetActive(true);
        achievementAnimator.Play("AchievementReminder");
    }
}
