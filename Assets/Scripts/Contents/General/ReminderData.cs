using System.Collections.Generic;

using UnityEngine;

public static class ReminderData
{
    public class ReminderDataStruct
    {
        public int ID;
        public string Content;
        public float Life;

        public ReminderDataStruct(int ID, string Content, float Life)
        {
            this.ID = ID;
            this.Content = Content;
            this.Life = Life;
        }
    }
    public static Dictionary<int, ReminderDataStruct> data = new Dictionary<int, ReminderDataStruct>
    {
        {1, new ReminderDataStruct(1, "WASD to Move, F to Interact", 2f)},
        {2, new ReminderDataStruct(2, "A Few Hours Later...", 2f)},
        {3, new ReminderDataStruct(3, "Use Menu - Map to walk around!", 2f)},
        {4, new ReminderDataStruct(4, "You have unlocked a new piece of Note", 2f)},
    };

    public static ReminderDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out ReminderDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
