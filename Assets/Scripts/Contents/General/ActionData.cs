using System.Collections.Generic;

using UnityEngine;

public static class ActionData
{
    public class ActionDataStruct
    {
        public int ID;
        public Enums.TASK_ACTION ActionType;
        public int[] ActionTarget;

        public ActionDataStruct(int ID, Enums.TASK_ACTION ActionType, int[] ActionTarget)
        {
            this.ID = ID;
            this.ActionType = ActionType;
            this.ActionTarget = ActionTarget;
        }
    }
    public static Dictionary<int, ActionDataStruct> data = new Dictionary<int, ActionDataStruct>
    {
        {1000, new ActionDataStruct(1000, Enums.TASK_ACTION.Chat, new int[]{10000})},
        {1001, new ActionDataStruct(1001, Enums.TASK_ACTION.CompleteTask, new int[]{1000})},
        {1002, new ActionDataStruct(1002, Enums.TASK_ACTION.Chat, new int[]{10007})},
        {1003, new ActionDataStruct(1003, Enums.TASK_ACTION.Chat, new int[]{10008})},
        {1004, new ActionDataStruct(1004, Enums.TASK_ACTION.CompleteTask, new int[]{1001})},
        {1005, new ActionDataStruct(1005, Enums.TASK_ACTION.Chat, new int[]{10011})},
        {1006, new ActionDataStruct(1006, Enums.TASK_ACTION.CompleteTask, new int[]{1002})},
        {1007, new ActionDataStruct(1007, Enums.TASK_ACTION.Chat, new int[]{10014})},
        {1008, new ActionDataStruct(1008, Enums.TASK_ACTION.CompleteTask, new int[]{1003})},
        {1009, new ActionDataStruct(1009, Enums.TASK_ACTION.Chat, new int[]{10030})},
        {1010, new ActionDataStruct(1010, Enums.TASK_ACTION.Chat, new int[]{10035})},
        {1011, new ActionDataStruct(1011, Enums.TASK_ACTION.CompleteTask, new int[]{1004})},
        {1012, new ActionDataStruct(1012, Enums.TASK_ACTION.CompleteTask, new int[]{1100})},
        {1013, new ActionDataStruct(1013, Enums.TASK_ACTION.Chat, new int[]{10047})},
        {1014, new ActionDataStruct(1014, Enums.TASK_ACTION.CompleteTask, new int[]{1005})},
        {1015, new ActionDataStruct(1015, Enums.TASK_ACTION.Chat, new int[]{10052})},
        {1016, new ActionDataStruct(1016, Enums.TASK_ACTION.Chat, new int[]{10053})},
        {1017, new ActionDataStruct(1017, Enums.TASK_ACTION.Chat, new int[]{10068})},
        {1018, new ActionDataStruct(1018, Enums.TASK_ACTION.CompleteTask, new int[]{1101})},
    };

    public static ActionDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out ActionDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
