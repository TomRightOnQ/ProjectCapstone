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
        {1, new ActionDataStruct(1, Enums.TASK_ACTION.StartGame, new int[]{4})},
        {2, new ActionDataStruct(2, Enums.TASK_ACTION.StartGame, new int[]{5})},
        {3, new ActionDataStruct(3, Enums.TASK_ACTION.StartGame, new int[]{6})},
        {4, new ActionDataStruct(4, Enums.TASK_ACTION.StartGame, new int[]{7})},
        {5, new ActionDataStruct(5, Enums.TASK_ACTION.StartGame, new int[]{8})},
        {6, new ActionDataStruct(6, Enums.TASK_ACTION.Teleport, new int[]{6})},
        {7, new ActionDataStruct(7, Enums.TASK_ACTION.Teleport, new int[]{2})},
        {8, new ActionDataStruct(8, Enums.TASK_ACTION.Chat, new int[]{1})},
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
