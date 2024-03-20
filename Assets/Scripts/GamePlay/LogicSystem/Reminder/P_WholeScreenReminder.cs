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
    public void BeginReminder(int id)
    {
        ReminderData.ReminderDataStruct remidnerData = ReminderData.GetData(id);
        tb_ReminderText.text = remidnerData.Content;

        switch (remidnerData.SpeakerType)
        {
            case Enums.CHARACTER_TYPE.Friend:
                tb_ReminderText.color = Color.yellow;
                break;
            case Enums.CHARACTER_TYPE.You:
                tb_ReminderText.color = Color.white;
                break;
            case Enums.CHARACTER_TYPE.Enemy:
                tb_ReminderText.color = Color.black;
                break;
        }

        reminderAnimator.Play("In");
        Invoke("EndWholeScreenReminder", remidnerData.Life);
    }

    // End the reminder
    private void EndWholeScreenReminder()
    {
        reminderAnimator.Play("Out");
        Invoke("ReturnWholeScreenReminder", 1f);
    }
    // Back to the pool
    private void ReturnWholeScreenReminder()
    {
        CancelInvoke("ReturnWholeScreenReminder");
        _deactivate();
    }

    private void _deactivate()
    {
        gameObject.SetActive(false);
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
