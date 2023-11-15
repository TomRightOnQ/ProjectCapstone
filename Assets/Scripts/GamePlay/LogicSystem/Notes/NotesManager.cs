using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System that records useful information for the player
/// </summary>

public class NotesManager : MonoBehaviour
{
    private static NotesManager instance;
    public static NotesManager Instance => instance;

    // UI Components
    [SerializeField] private UI_Notes ui_Notes;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Private:
    // Create UI
    private void createUINotes()
    {
        GameObject uiObject = UIManager.Instance.CreateUI("UI_Notes");
        ui_Notes = uiObject.GetComponent<UI_Notes>();
    }

    // Public:
    public void ShowNotePanel()
    {
        if (ui_Notes == null)
        {
            createUINotes();
        }
        UIManager.Instance.ShowUI("UI_Notes");

        ui_Notes.ShowNotePanel();
    }

    public void CloseNotePanel() 
    {
        if (ui_Notes == null)
        {
            return;
        }
        ui_Notes.CloseNotePanel();
    }
}
