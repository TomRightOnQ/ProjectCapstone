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
        {1, new ActionDataStruct(1, Enums.TASK_ACTION.RemoveInteraction, new int[]{2,2})},
        {2, new ActionDataStruct(2, Enums.TASK_ACTION.AddInteraction, new int[]{2,3})},
        {3, new ActionDataStruct(3, Enums.TASK_ACTION.UnlockNextDay, new int[]{-1})},
        {4, new ActionDataStruct(4, Enums.TASK_ACTION.RemoveInteraction, new int[]{2,3})},
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
