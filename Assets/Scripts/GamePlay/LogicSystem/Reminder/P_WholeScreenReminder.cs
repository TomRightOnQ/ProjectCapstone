using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Used for general reminder
/// </summary>

public class P_WholeScreenReminder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tb_ReminderText;
    [SerializeField] private Animator reminderAnimator;

    // Begin the reminder cycle
    public void BeginGeneralReminder(int id)
    {
        ReminderData.ReminderDataStruct remidnerData = ReminderData.GetData(id);
        tb_ReminderText.text = remidnerData.Content;

        reminderAnimator.Play("In");
        Invoke("EndGeneralReminder", remidnerData.Life);
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
