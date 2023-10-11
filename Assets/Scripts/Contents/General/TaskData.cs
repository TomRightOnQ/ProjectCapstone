using System.Collections.Generic;

using UnityEngine;

public static class TaskData
{
    public class TaskDataStruct
    {
        public int ID;
        public string Name;
        public string Description;
        public Enums.TASK_TYPE Type;
        public int TergetObject;
        public int[] RingID;

        public TaskDataStruct(int ID, string Name, string Description, Enums.TASK_TYPE Type, int TergetObject, int[] RingID)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            this.Type = Type;
            this.TergetObject = TergetObject;
            this.RingID = RingID;
        }
    }
    public static Dictionary<int, TaskDataStruct> data = new Dictionary<int, TaskDataStruct>
    {
        {1, new TaskDataStruct(1, "Task1", "Task1_Desc", Enums.TASK_TYPE.Chat, 1, new int[]{1})},
        {2, new TaskDataStruct(2, "Task2", "Task2_Desc", Enums.TASK_TYPE.Chat, 1, new int[]{1})},
    };

    public static TaskDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out TaskDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
