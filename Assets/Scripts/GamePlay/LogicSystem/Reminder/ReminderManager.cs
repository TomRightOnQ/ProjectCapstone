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

    // Show reminder: Subtitle
    public void ShowSubtitleReminder(int id)
    {
        if (ui_Reminder == null)
        {
            createUI();
        }
        ui_Reminder.ShowSubtitleReminder(id);
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
}
