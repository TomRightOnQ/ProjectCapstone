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
        {1019, new ActionDataStruct(1019, Enums.TASK_ACTION.Chat, new int[]{10038})},
        {1020, new ActionDataStruct(1020, Enums.TASK_ACTION.EnterGame, new int[]{14,5})},
        {1021, new ActionDataStruct(1021, Enums.TASK_ACTION.CompleteTask, new int[]{1006})},
        {1022, new ActionDataStruct(1022, Enums.TASK_ACTION.CompleteTask, new int[]{1013})},
        {2000, new ActionDataStruct(2000, Enums.TASK_ACTION.Chat, new int[]{20000})},
        {2001, new ActionDataStruct(2001, Enums.TASK_ACTION.Chat, new int[]{201})},
        {2002, new ActionDataStruct(2002, Enums.TASK_ACTION.CompleteTask, new int[]{2000})},
        {2003, new ActionDataStruct(2003, Enums.TASK_ACTION.Chat, new int[]{20002})},
        {2004, new ActionDataStruct(2004, Enums.TASK_ACTION.Chat, new int[]{20008})},
        {2005, new ActionDataStruct(2005, Enums.TASK_ACTION.Chat, new int[]{202})},
        {2006, new ActionDataStruct(2006, Enums.TASK_ACTION.CompleteTask, new int[]{2100})},
        {2007, new ActionDataStruct(2007, Enums.TASK_ACTION.CompleteTask, new int[]{2001})},
        {2008, new ActionDataStruct(2008, Enums.TASK_ACTION.Chat, new int[]{20070})},
        {2009, new ActionDataStruct(2009, Enums.TASK_ACTION.Chat, new int[]{20300})},
        {2010, new ActionDataStruct(2010, Enums.TASK_ACTION.CompleteTask, new int[]{2002})},
        {2011, new ActionDataStruct(2011, Enums.TASK_ACTION.CompleteTask, new int[]{2101})},
        {2012, new ActionDataStruct(2012, Enums.TASK_ACTION.Chat, new int[]{20200})},
        {2013, new ActionDataStruct(2013, Enums.TASK_ACTION.EnterGame, new int[]{6,4})},
        {2014, new ActionDataStruct(2014, Enums.TASK_ACTION.CompleteTask, new int[]{2003})},
        {2015, new ActionDataStruct(2015, Enums.TASK_ACTION.Chat, new int[]{20350})},
        {2016, new ActionDataStruct(2016, Enums.TASK_ACTION.CompleteTask, new int[]{2004})},
        {2017, new ActionDataStruct(2017, Enums.TASK_ACTION.CompleteTask, new int[]{2005})},
        {2018, new ActionDataStruct(2018, Enums.TASK_ACTION.Chat, new int[]{20020})},
        {2019, new ActionDataStruct(2019, Enums.TASK_ACTION.Chat, new int[]{20040})},
        {2020, new ActionDataStruct(2020, Enums.TASK_ACTION.Chat, new int[]{20057})},
        {2021, new ActionDataStruct(2021, Enums.TASK_ACTION.CompleteTask, new int[]{2102})},
        {2022, new ActionDataStruct(2022, Enums.TASK_ACTION.CompleteTask, new int[]{2103})},
        {2023, new ActionDataStruct(2023, Enums.TASK_ACTION.Chat, new int[]{20030})},
        {2024, new ActionDataStruct(2024, Enums.TASK_ACTION.Chat, new int[]{20100})},
        {2025, new ActionDataStruct(2025, Enums.TASK_ACTION.CompleteTask, new int[]{2104})},
        {2026, new ActionDataStruct(2026, Enums.TASK_ACTION.UnlockNotes, new int[]{10001})},
        {2027, new ActionDataStruct(2027, Enums.TASK_ACTION.UnlockNotes, new int[]{10002})},
        {2028, new ActionDataStruct(2028, Enums.TASK_ACTION.UnlockNotes, new int[]{10102})},
        {2029, new ActionDataStruct(2029, Enums.TASK_ACTION.UnlockNotes, new int[]{10104})},
        {2030, new ActionDataStruct(2030, Enums.TASK_ACTION.UnlockNotes, new int[]{10105})},
        {2031, new ActionDataStruct(2031, Enums.TASK_ACTION.UnlockNotes, new int[]{10007})},
        {3000, new ActionDataStruct(3000, Enums.TASK_ACTION.Chat, new int[]{301})},
        {3001, new ActionDataStruct(3001, Enums.TASK_ACTION.Chat, new int[]{302})},
        {3002, new ActionDataStruct(3002, Enums.TASK_ACTION.CompleteTask, new int[]{3100})},
        {3003, new ActionDataStruct(3003, Enums.TASK_ACTION.Chat, new int[]{30000})},
        {3004, new ActionDataStruct(3004, Enums.TASK_ACTION.Chat, new int[]{30001})},
        {3005, new ActionDataStruct(3005, Enums.TASK_ACTION.CompleteTask, new int[]{3000})},
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
