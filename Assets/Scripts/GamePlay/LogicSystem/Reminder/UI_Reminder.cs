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
}
