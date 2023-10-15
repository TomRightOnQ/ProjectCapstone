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
        public Enums.TASK_ACTION Action_1;
        public int[] Action_1_Target;
        public Enums.TASK_ACTION Action_2;
        public int[] Action_2_Target;
        public int[] UnlockTask;

        public TaskDataStruct(int ID, string Name, string Description, Enums.TASK_TYPE Type, Enums.TASK_ACTION Action_1, int[] Action_1_Target, Enums.TASK_ACTION Action_2, int[] Action_2_Target, int[] UnlockTask)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            this.Type = Type;
            this.Action_1 = Action_1;
            this.Action_1_Target = Action_1_Target;
            this.Action_2 = Action_2;
            this.Action_2_Target = Action_2_Target;
            this.UnlockTask = UnlockTask;
        }
    }
    public static Dictionary<int, TaskDataStruct> data = new Dictionary<int, TaskDataStruct>
    {
        {1, new TaskDataStruct(1, "Task1", "Task1_Desc", Enums.TASK_TYPE.Chat, Enums.TASK_ACTION.AddInteraction, new int[]{1,4}, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,3}, new int[]{-1})},
        {2, new TaskDataStruct(2, "Task2", "Task2_Desc", Enums.TASK_TYPE.Chat, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,4}, Enums.TASK_ACTION.None, new int[]{-1}, new int[]{-1})},
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
