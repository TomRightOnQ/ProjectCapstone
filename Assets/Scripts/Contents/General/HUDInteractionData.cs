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

        public HUDInteractionDataStruct(int ID, string Content, int[] Target, Enums.INTERACTION_TYPE Action, bool bOneTime)
        {
            this.ID = ID;
            this.Content = Content;
            this.Target = Target;
            this.Action = Action;
            this.bOneTime = bOneTime;
        }
    }
    public static Dictionary<int, HUDInteractionDataStruct> data = new Dictionary<int, HUDInteractionDataStruct>
    {
        {1, new HUDInteractionDataStruct(1, "Hello", new int[]{1}, Enums.INTERACTION_TYPE.Chat, false)},
        {2, new HUDInteractionDataStruct(2, "Sample Game", new int[]{6}, Enums.INTERACTION_TYPE.Chat, false)},
        {3, new HUDInteractionDataStruct(3, "First Day", new int[]{9}, Enums.INTERACTION_TYPE.Chat, false)},
        {4, new HUDInteractionDataStruct(4, "Reminder", new int[]{1}, Enums.INTERACTION_TYPE.ShowReminder, false)},
        {5, new HUDInteractionDataStruct(5, "Enter Team Room", new int[]{6}, Enums.INTERACTION_TYPE.Teleport, false)},
        {6, new HUDInteractionDataStruct(6, "Exit Team Room", new int[]{2}, Enums.INTERACTION_TYPE.Teleport, false)},
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
