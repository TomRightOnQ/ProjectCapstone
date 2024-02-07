using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Used for general reminder
/// </summary>
public class P_GeneralReminder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tb_ReminderText;
    [SerializeField] private Animator reminderAnimator;

    // Begin the reminder cycle
    public void BeginGeneralReminder(int id)
    {
        ReminderData.ReminderDataStruct reminderData = ReminderData.GetData(id);
        tb_ReminderText.text = reminderData.Content;

        reminderAnimator.Play("In");
        Invoke("EndGeneralReminder", reminderData.Life);
    }

    public void BeginGeneralReminder(string content, float life = 2f)
    {
        tb_ReminderText.text = content;

        reminderAnimator.Play("In");
        Invoke("EndGeneralReminder", life);
    }

    // End the reminder
    private void EndGeneralReminder()
    {
        reminderAnimator.Play("Out");
        Invoke("ReturnGeneralReminder", 0.51f);
    }
    // Back to the pool
    private void ReturnGeneralReminder()
    {
        CancelInvoke("ReturnGeneralReminder");
        _deactivate();
    }

    private void _deactivate()
    {
        gameObject.SetActive(false);
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
