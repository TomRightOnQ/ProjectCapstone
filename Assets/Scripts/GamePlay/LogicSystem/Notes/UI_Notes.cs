using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UIWidgets;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// UI for note system
/// </summary>
public class UI_Notes : UIBase
{
    // Widget
    [SerializeField] private GameObject P_Content;
    [SerializeField] private TextMeshProUGUI TB_NameTitle;
    [SerializeField] private TextMeshProUGUI RTB_NoteContent;
    [SerializeField] private GameObject Img_DefualtImage;

    // Note List
    [SerializeField] private ListViewIcons NotesList;
    [SerializeField] private GameObject viewPort;
    private ObservableList<ListViewIconsItemDescription> notesItems = new ObservableList<ListViewIconsItemDescription>();

    // Current Displayed Type
    [SerializeField, ReadOnly]
    private Enums.NOTE_TYPE currentNoteType = Enums.NOTE_TYPE.Note;

    private void Awake()
    {
        NotesList.DataSource = notesItems;
    }

    // Private:
    // Refresh List
    private void refreshList() 
    {
        // Clear old data
        notesItems.Clear();
        // Retriving data from SaveSystem
        List<int> listData = SaveManager.Instance.GetNote(currentNoteType);
        for (int i = 0; i < listData.Count; i++)
        {
            AddToNoteList(listData[i]);
        }
    }

    // Update the detail panel
    public void refreshDetailPanel(int noteID)
    {
        Img_DefualtImage.SetActive(false);
        NotesData.NotesDataStruct notesData = NotesData.GetData(noteID);
        string resourcePath = Path.Combine(Constants.NOTES_SOURCE_PATH, notesData.Path);

        // Remove the file extension if present, Unity Resources.Load does not require it
        resourcePath = resourcePath.Replace(".txt", "");

        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);

        if (textAsset != null)
        {
            TB_NameTitle.text = notesData.Name;
            RTB_NoteContent.text = textAsset.text;
        }
        else
        {
            Debug.LogWarning("UI_Notes: Resource file not found: " + resourcePath);
        }
    }

    // Public:
    // Add to list
    public void AddToNoteList(int noteID)
    {
        if (!notesItems.Exists(item => item.Value == noteID))
        {
            NotesData.NotesDataStruct notesData = NotesData.GetData(noteID);
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = noteID, Name = notesData.Name };
            notesItems.Add(newItem);
        }
    }
    // Remove from the list
    public void RemoveFromNoteList(int noteID)
    {
        ListViewIconsItemDescription itemToRemove = notesItems.Find(item => item.Value == noteID);
        if (itemToRemove != null)
        {
            notesItems.Remove(itemToRemove);
        }
    }

    // Open and close the panel
    public void ShowNotePanel()
    {
        Img_DefualtImage.SetActive(true);
        gameObject.SetActive(true);
        refreshList();
    }

    public void CloseNotePanel()
    {
        Img_DefualtImage.SetActive(true);
        gameObject.SetActive(false);
        PersistentGameManager.Instance.ResumeGame();
    }

    // On Click Events
    public void OnClick_Btn_ShowNotes()
    {
        currentNoteType = Enums.NOTE_TYPE.Note;
        refreshList();
    }
    public void OnClick_Btn_ShowItems()
    {
        currentNoteType = Enums.NOTE_TYPE.Item;
        refreshList();
    }
    public void OnClick_Btn_ShowReports()
    {
        currentNoteType = Enums.NOTE_TYPE.Report;
        refreshList();
    }

    public void OnClick_NotesList(int index, ListViewItem item, PointerEventData eventData)
    {
        int noteID = SaveManager.Instance.GetNote(currentNoteType)[index];
        refreshDetailPanel(noteID);
    }
}
