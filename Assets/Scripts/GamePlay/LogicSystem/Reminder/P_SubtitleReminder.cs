using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Used for Subtitle reminder
/// </summary>
public class P_SubtitleReminder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tb_ReminderText;

    // Begin the reminder cycle
    public void BeginSubtitleReminder(int id, string speaker = "You", Enums.CHARACTER_TYPE speakerType = Enums.CHARACTER_TYPE.You)
    {
        ReminderData.ReminderDataStruct reminderData = ReminderData.GetData(id);
        BeginSubtitleReminder(reminderData.Content, reminderData.Speaker, reminderData.Life, reminderData.SpeakerType);
    }

    public void BeginSubtitleReminder(string content, string speaker = "You", float life = 2f, Enums.CHARACTER_TYPE speakerType = Enums.CHARACTER_TYPE.You)
    {
        string colorCode = "yellow";
        switch (speakerType)
        {
            case Enums.CHARACTER_TYPE.Friend:
                colorCode = "green";
                break;
            case Enums.CHARACTER_TYPE.Enemy:
                colorCode = "red";
                break;
            default:
                break;
        }
        tb_ReminderText.text = string.Format("<color={0}>{1}</color>: {2}", colorCode, speaker, content);
        Invoke("EndSubtitleReminder", life);
    }

    // End the reminder
    private void EndSubtitleReminder()
    {
        Invoke("ReturnSubtitleReminder", 0.51f);
    }
    // Back to the pool
    private void ReturnSubtitleReminder()
    {
        CancelInvoke("ReturnSubtitleReminder");
        _deactivate();
    }

    private void _deactivate()
    {
        gameObject.SetActive(false);
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
