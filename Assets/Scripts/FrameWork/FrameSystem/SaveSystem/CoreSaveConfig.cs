using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core Save Config - Saves information that persists among different saves
/// </summary>
[CreateAssetMenu(menuName = "SaveSystem/CoreSaveConfig")]
public class CoreSaveConfig : ScriptableSingleton<CoreSaveConfig>
{
    // Control flag
    [SerializeField] private bool bAllowRewrite = true;
    public bool AllowRewrite { get { return bAllowRewrite; } }

    // NotesSystem information
    /// <summary>
    /// Including Notes and Hints
    /// </summary>
    [SerializeField]
    private NoteData noteData = new NoteData();

    [SerializeField]
    private List<int> hintUnlockList = new List<int>();
    public List<int> HintUnlockList => hintUnlockList;


    [System.Serializable]
    public class NoteData
    {
        public List<int> NoteIDs = new List<int>();
        public List<int> ItemIDs = new List<int>();
        public List<int> ReportIDs = new List<int>();
    }

    // DayCycle - Max Day
    [SerializeField]
    private int maxDay = 0;
    public int MaxDay { set; get; }

    // Public:
    // Modify Unlocked Hints
    // --Init-- Methods
    public void InitUnlockHintToSave()
    {
        hintUnlockList.Clear();
    }

    // Add a Hint
    public void UnlockHint(int hintID)
    {
        if (!hintUnlockList.Contains(hintID))
        {
            hintUnlockList.Add(hintID);
        }
    }

    // Control a note
    public List<int> GetNote(Enums.NOTE_TYPE type)
    {
        switch (type)
        {
            case Enums.NOTE_TYPE.Note:
                return noteData.NoteIDs;
            case Enums.NOTE_TYPE.Item:
                return noteData.ItemIDs;
            case Enums.NOTE_TYPE.Report:
                return noteData.ReportIDs;
            default:
                return noteData.NoteIDs;
        }
    }

    public void AddNote(Enums.NOTE_TYPE type, int[] IDs)
    {
        switch (type)
        {
            case Enums.NOTE_TYPE.Note:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.NoteIDs.Contains(IDs[i]))
                    {
                        noteData.NoteIDs.Add(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Item:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.ItemIDs.Contains(IDs[i]))
                    {
                        noteData.ItemIDs.Add(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Report:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (!noteData.ReportIDs.Contains(IDs[i]))
                    {
                        noteData.ReportIDs.Add(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            default:
                break;
        }
    }

    public void RemoveNote(Enums.NOTE_TYPE type, int[] IDs)
    {
        switch (type)
        {
            case Enums.NOTE_TYPE.Note:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.NoteIDs.Contains(IDs[i]))
                    {
                        noteData.NoteIDs.Remove(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Item:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.ItemIDs.Contains(IDs[i]))
                    {
                        noteData.ItemIDs.Remove(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            case Enums.NOTE_TYPE.Report:
                for (int i = 0; i < IDs.Length; i++)
                {
                    if (noteData.ReportIDs.Contains(IDs[i]))
                    {
                        noteData.ReportIDs.Remove(IDs[i]);
                    }
                }
                noteData.NoteIDs.Sort();
                break;
            default:
                break;
        }
    }
}
