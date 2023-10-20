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
        public int[] PreActions;
        public int[] PostActions;
        public int[] UnlockTask;

        public TaskDataStruct(int ID, string Name, string Description, Enums.TASK_TYPE Type, int[] PreActions, int[] PostActions, int[] UnlockTask)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            this.Type = Type;
            this.PreActions = PreActions;
            this.PostActions = PostActions;
            this.UnlockTask = UnlockTask;
        }
    }
    public static Dictionary<int, TaskDataStruct> data = new Dictionary<int, TaskDataStruct>
    {
        {1, new TaskDataStruct(1, "Unlock Day 1", "Time for the next day!", Enums.TASK_TYPE.Chat, new int[]{2}, new int[]{3}, new int[]{-1})},
        {2, new TaskDataStruct(2, "Unlock Day 2", "Time for the next day!", Enums.TASK_TYPE.Chat, new int[]{-1}, new int[]{3}, new int[]{-1})},
        {3, new TaskDataStruct(3, "Unlock Day 3", "Time for the next day!", Enums.TASK_TYPE.Chat, new int[]{-1}, new int[]{-1}, new int[]{-1})},
        {4, new TaskDataStruct(4, "Unlock Day 4", "Time for the next day!", Enums.TASK_TYPE.Chat, new int[]{-1}, new int[]{-1}, new int[]{-1})},
        {5, new TaskDataStruct(5, "Unlock Day 5", "Time for the next day!", Enums.TASK_TYPE.Chat, new int[]{-1}, new int[]{-1}, new int[]{-1})},
        {6, new TaskDataStruct(6, "Unlock Day 6", "Time for the next day!", Enums.TASK_TYPE.Chat, new int[]{-1}, new int[]{-1}, new int[]{-1})},
        {7, new TaskDataStruct(7, "Unlock Day 7", "Time for the next day!", Enums.TASK_TYPE.Chat, new int[]{-1}, new int[]{-1}, new int[]{-1})},
        {8, new TaskDataStruct(8, "Complete a platformer stage", "Go and try a mini game", Enums.TASK_TYPE.Game, new int[]{-1}, new int[]{1}, new int[]{1})},
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
