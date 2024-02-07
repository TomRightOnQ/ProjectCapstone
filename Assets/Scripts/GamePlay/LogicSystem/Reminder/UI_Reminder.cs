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

    // Whole Screen
    [SerializeField] private GameObject p_WholeScreenReminder;
    [SerializeField] private TextMeshProUGUI tb_WholeScreenText;
    [SerializeField] private Animator wholeAnimator;

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
        reminderObj.transform.SetParent(this.transform, false);
        reminderObj.SetActive(true);
        P_GeneralReminder reminder = reminderObj.GetComponent<P_GeneralReminder>();
        reminder.BeginGeneralReminder(id);
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
    public void ShowWholeScreenReminder(int reminderID)
    {
        tb_WholeScreenText.text = ReminderData.GetData(reminderID).Content;
        p_WholeScreenReminder.SetActive(true);
        wholeAnimator.Play("In");
        StartCoroutine(closeWholeScreenReminder());
    }

    private IEnumerator closeWholeScreenReminder()
    {
        yield return new WaitForSecondsRealtime(Constants.REMINDER_LEVEL_TIME);
        HideWholeScreenReminder();
    }

    // Hide WholeScreen Reminder
    public void HideWholeScreenReminder()
    {
        wholeAnimator.Play("Out");
        StopCoroutine(closeWholeScreenReminder());
        Invoke("DisableWholeScreenReminder", 0.5f);
    }

    // Disable WholeScreen Reminder
    public void DisableWholeScreenReminder()
    {
        p_WholeScreenReminder.SetActive(false);
    }
}
