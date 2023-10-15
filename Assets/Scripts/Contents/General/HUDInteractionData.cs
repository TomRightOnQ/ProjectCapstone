using System.Collections.Generic;

using UnityEngine;

public static class HUDInteractionData
{
    public class HUDInteractionDataStruct
    {
        public int ID;
        public string Content;
        public int[] Target;
        public Enums.INTERACTION_TYPE Action;
        public bool bOneTime;
        public int Speaker;

        public HUDInteractionDataStruct(int ID, string Content, int[] Target, Enums.INTERACTION_TYPE Action, bool bOneTime, int Speaker)
        {
            this.ID = ID;
            this.Content = Content;
            this.Target = Target;
            this.Action = Action;
            this.bOneTime = bOneTime;
            this.Speaker = Speaker;
        }
    }
    public static Dictionary<int, HUDInteractionDataStruct> data = new Dictionary<int, HUDInteractionDataStruct>
    {
        {1, new HUDInteractionDataStruct(1, "Hello", new int[]{1}, Enums.INTERACTION_TYPE.Chat, false, 1)},
        {2, new HUDInteractionDataStruct(2, "Only happended once", new int[]{2}, Enums.INTERACTION_TYPE.End, true, 1)},
        {3, new HUDInteractionDataStruct(3, "Task1", new int[]{6}, Enums.INTERACTION_TYPE.Chat, false, 1)},
        {4, new HUDInteractionDataStruct(4, "Task2", new int[]{8}, Enums.INTERACTION_TYPE.Chat, false, 1)},
    };

    public static HUDInteractionDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out HUDInteractionDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
