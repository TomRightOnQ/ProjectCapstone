using System.Collections.Generic;

using UnityEngine;

public static class TaskRingData
{
    public class TaskRingDataStruct
    {
        public int ID;
        public string Name;
        public string Description;
        public int[] Task;

        public TaskRingDataStruct(int ID, string Name, string Description, int[] Task)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            this.Task = Task;
        }
    }
    public static Dictionary<int, TaskRingDataStruct> data = new Dictionary<int, TaskRingDataStruct>
    {
        {1, new TaskRingDataStruct(1, "Test Ring 1", "Just a test", new int[]{1,2})},
    };

    public static TaskRingDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out TaskRingDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
