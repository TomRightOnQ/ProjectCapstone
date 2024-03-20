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
        {3, new NotesDataStruct(3, "FlashBack", Enums.NOTE_TYPE.Note, "3.txt")},
        {10000, new NotesDataStruct(10000, "About the Game", Enums.NOTE_TYPE.Note, "10000.txt")},
        {10001, new NotesDataStruct(10001, "Veronique", Enums.NOTE_TYPE.Note, "10001.txt")},
        {10002, new NotesDataStruct(10002, "Dove", Enums.NOTE_TYPE.Note, "10002.txt")},
        {10003, new NotesDataStruct(10003, "The Game", Enums.NOTE_TYPE.Note, "10003.txt")},
        {10004, new NotesDataStruct(10004, "Darlan", Enums.NOTE_TYPE.Note, "10004.txt")},
        {10005, new NotesDataStruct(10005, "Lanuarius", Enums.NOTE_TYPE.Note, "10005.txt")},
        {10006, new NotesDataStruct(10006, "Francois", Enums.NOTE_TYPE.Note, "10006.txt")},
        {10007, new NotesDataStruct(10007, "Chesteria", Enums.NOTE_TYPE.Note, "10007.txt")},
        {10100, new NotesDataStruct(10100, "Game Rule", Enums.NOTE_TYPE.Item, "10100.txt")},
        {10101, new NotesDataStruct(10101, "Smart Collar", Enums.NOTE_TYPE.Item, "10101.txt")},
        {10102, new NotesDataStruct(10102, "\"The Godsends\"", Enums.NOTE_TYPE.Note, "10102.txt")},
        {10103, new NotesDataStruct(10103, "Samantha's Badge", Enums.NOTE_TYPE.Item, "10103.txt")},
        {10104, new NotesDataStruct(10104, "\"The Gestalt System\"", Enums.NOTE_TYPE.Note, "10104.txt")},
        {10105, new NotesDataStruct(10105, "\"The River\"", Enums.NOTE_TYPE.Note, "10105.txt")},
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
