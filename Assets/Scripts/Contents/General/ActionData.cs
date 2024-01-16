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
        {0, new ActionDataStruct(0, Enums.TASK_ACTION.UnlockNextDay, new int[]{-1})},
        {1, new ActionDataStruct(1, Enums.TASK_ACTION.StartGame, new int[]{4})},
        {2, new ActionDataStruct(2, Enums.TASK_ACTION.StartGame, new int[]{5})},
        {3, new ActionDataStruct(3, Enums.TASK_ACTION.StartGame, new int[]{6})},
        {4, new ActionDataStruct(4, Enums.TASK_ACTION.StartGame, new int[]{7})},
        {5, new ActionDataStruct(5, Enums.TASK_ACTION.StartGame, new int[]{8})},
        {6, new ActionDataStruct(6, Enums.TASK_ACTION.None, new int[]{-1})},
        {7, new ActionDataStruct(7, Enums.TASK_ACTION.None, new int[]{-1})},
        {8, new ActionDataStruct(8, Enums.TASK_ACTION.Chat, new int[]{1})},
        {9, new ActionDataStruct(9, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,8})},
        {10, new ActionDataStruct(10, Enums.TASK_ACTION.AddInteraction, new int[]{1,9})},
        {11, new ActionDataStruct(11, Enums.TASK_ACTION.Chat, new int[]{4})},
        {12, new ActionDataStruct(12, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,9})},
        {13, new ActionDataStruct(13, Enums.TASK_ACTION.ChangeNPCPosition, new int[]{1,2,1})},
        {14, new ActionDataStruct(14, Enums.TASK_ACTION.Chat, new int[]{5})},
        {101, new ActionDataStruct(101, Enums.TASK_ACTION.Teleport, new int[]{0})},
        {102, new ActionDataStruct(102, Enums.TASK_ACTION.Teleport, new int[]{1})},
        {103, new ActionDataStruct(103, Enums.TASK_ACTION.Teleport, new int[]{2})},
        {104, new ActionDataStruct(104, Enums.TASK_ACTION.Teleport, new int[]{3})},
        {105, new ActionDataStruct(105, Enums.TASK_ACTION.Teleport, new int[]{4})},
        {106, new ActionDataStruct(106, Enums.TASK_ACTION.Teleport, new int[]{5})},
        {107, new ActionDataStruct(107, Enums.TASK_ACTION.Teleport, new int[]{6})},
        {108, new ActionDataStruct(108, Enums.TASK_ACTION.Teleport, new int[]{7})},
        {109, new ActionDataStruct(109, Enums.TASK_ACTION.Teleport, new int[]{8})},
        {15, new ActionDataStruct(15, Enums.TASK_ACTION.Chat, new int[]{8})},
        {16, new ActionDataStruct(16, Enums.TASK_ACTION.Chat, new int[]{9})},
        {17, new ActionDataStruct(17, Enums.TASK_ACTION.Chat, new int[]{10})},
        {18, new ActionDataStruct(18, Enums.TASK_ACTION.AddInteraction, new int[]{1,11})},
        {19, new ActionDataStruct(19, Enums.TASK_ACTION.AddInteraction, new int[]{1,14})},
        {20, new ActionDataStruct(20, Enums.TASK_ACTION.Chat, new int[]{12})},
        {21, new ActionDataStruct(21, Enums.TASK_ACTION.AddInteraction, new int[]{1,15})},
        {22, new ActionDataStruct(22, Enums.TASK_ACTION.Chat, new int[]{14})},
        {23, new ActionDataStruct(23, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,14})},
        {24, new ActionDataStruct(24, Enums.TASK_ACTION.AddInteraction, new int[]{1,16})},
        {25, new ActionDataStruct(25, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,16})},
        {26, new ActionDataStruct(26, Enums.TASK_ACTION.Chat, new int[]{15})},
        {27, new ActionDataStruct(27, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,15})},
        {28, new ActionDataStruct(28, Enums.TASK_ACTION.Chat, new int[]{20})},
        {29, new ActionDataStruct(29, Enums.TASK_ACTION.RemoveInteraction, new int[]{1,17})},
        {30, new ActionDataStruct(30, Enums.TASK_ACTION.AddInteraction, new int[]{1,18})},
        {31, new ActionDataStruct(31, Enums.TASK_ACTION.Chat, new int[]{21})},
        {32, new ActionDataStruct(32, Enums.TASK_ACTION.Chat, new int[]{22})},
        {34, new ActionDataStruct(34, Enums.TASK_ACTION.UnlockInteraction, new int[]{19})},
        {35, new ActionDataStruct(35, Enums.TASK_ACTION.TriggerTask, new int[]{9})},
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
