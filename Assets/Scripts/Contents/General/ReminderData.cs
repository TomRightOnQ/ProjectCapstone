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
        {1, new ReminderDataStruct(1, "Hey there!", 2f)},
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
