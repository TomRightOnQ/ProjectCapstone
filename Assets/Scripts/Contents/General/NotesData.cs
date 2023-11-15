using System.Collections.Generic;

using UnityEngine;

public static class NotesData
{
    public class NotesDataStruct
    {
        public int ID;
        public string Name;
        public Enums.NOTE_TYPE Type;
        public string Path;

        public NotesDataStruct(int ID, string Name, Enums.NOTE_TYPE Type, string Path)
        {
            this.ID = ID;
            this.Name = Name;
            this.Type = Type;
            this.Path = Path;
        }
    }
    public static Dictionary<int, NotesDataStruct> data = new Dictionary<int, NotesDataStruct>
    {
        {0, new NotesDataStruct(0, "Important Conversations...", Enums.NOTE_TYPE.Note, "SampleNote.txt")},
        {1, new NotesDataStruct(1, "Something Worth Remembering...", Enums.NOTE_TYPE.Item, "SampleItem.txt")},
        {2, new NotesDataStruct(2, "End of a Day...", Enums.NOTE_TYPE.Report, "SampleReport.txt")},
    };

    public static NotesDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out NotesDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
