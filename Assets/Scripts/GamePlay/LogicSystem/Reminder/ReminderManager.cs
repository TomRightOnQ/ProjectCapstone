using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the on-screen reminder system
/// </summary>

public class ReminderManager : MonoBehaviour
{
    private static ReminderManager instance;
    public static ReminderManager Instance => instance;

    // UIs
    [SerializeField] UI_Reminder ui_Reminder;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Private:
    // Create UI
    private void createUI()
    {
        if (ui_Reminder == null)
        {
            GameObject uiObject = UIManager.Instance.CreateUI("UI_Reminder");
            ui_Reminder = uiObject.GetComponent<UI_Reminder>();
        }
    }

    // Public:
    public void Init()
    {

    }

    // Show reminder: General
    public void ShowGeneralReminder(int id)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowGeneralReminder(id);
    }

    public void ShowGeneralReminder(string content, float life)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowGeneralReminder(content, life);
    }

    // Show reminder: Subtitle
    public void ShowSubtitleReminder(int id)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowSubtitleReminder(id);
    }

    public void ShowSubtitleReminder(string content, string speaker, float life, Enums.CHARACTER_TYPE speakerType)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowSubtitleReminder(content, speaker, life, speakerType);
    }

    public void ShowSubtitleReminder(int[] id)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        StartCoroutine(spawnSubtitleReminder(id));
    }

    private IEnumerator spawnSubtitleReminder(int[] id)
    {
        for (int i = 0; i < id.Length; i++)
        {
            ui_Reminder.ShowSubtitleReminder(id[i]);
            yield return new WaitForSeconds(ReminderData.GetData(id[i]).Life + 0.5f);
        }
    }

    // Show reminder: Level/Map Name
    public void ShowMapNameReminder(string name)
    {
        if (name == "None")
        {
            return;
        }
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowMapReminder(name);
    }

    // Show Reminder: Game Saving
    public void ShowGameSavingReminder()
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowGameSavingReminder();
    }


    // Show Reminder: Whole Screen
    public void ShowWholeScreenReminder(int reminderID)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowWholeScreenReminder(reminderID);
    }

    public void ShowWholeScreenReminder(int[] reminderID, float[] gap)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        StartCoroutine(spawnWholeScreenReminder(reminderID, gap));
    }

    private IEnumerator spawnWholeScreenReminder(int[] reminderID, float[] gap)
    {
        for (int i = 0; i < reminderID.Length; i++)
        {
            ui_Reminder.ShowWholeScreenReminder(reminderID[i]);
            yield return new WaitForSeconds(gap[i]);
        }
    }

    // Show a new achievement
    public void ShowAchievementReminder(int achID)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowAchievementReminder(achID);
    }
}
