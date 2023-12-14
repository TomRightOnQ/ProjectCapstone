using System.Collections.Generic;

using UnityEngine;

public static class HUDInteractionData
{
    public class HUDInteractionDataStruct
    {
        public int ID;
        public string Content;
        public int[] Action;
        public bool bOneTime;
        public bool bNoneClicking;

        public HUDInteractionDataStruct(int ID, string Content, int[] Action, bool bOneTime, bool bNoneClicking)
        {
            this.ID = ID;
            this.Content = Content;
            this.Action = Action;
            this.bOneTime = bOneTime;
            this.bNoneClicking = bNoneClicking;
        }
    }
    public static Dictionary<int, HUDInteractionDataStruct> data = new Dictionary<int, HUDInteractionDataStruct>
    {
        {1, new HUDInteractionDataStruct(1, "Enter Team Room", new int[]{107}, false, false)},
        {2, new HUDInteractionDataStruct(2, "Exit Team Room", new int[]{103}, false, false)},
        {3, new HUDInteractionDataStruct(3, "ShooterLevel 1", new int[]{1}, false, false)},
        {4, new HUDInteractionDataStruct(4, "ShooterLevel 2", new int[]{2}, false, false)},
        {5, new HUDInteractionDataStruct(5, "ShooterLevel 3", new int[]{3}, false, false)},
        {6, new HUDInteractionDataStruct(6, "ShooterLevel 4", new int[]{4}, false, false)},
        {7, new HUDInteractionDataStruct(7, "ShooterLevel 5", new int[]{5}, false, false)},
        {8, new HUDInteractionDataStruct(8, "Greetings!", new int[]{8}, false, false)},
        {9, new HUDInteractionDataStruct(9, "I'm ready!", new int[]{11}, false, false)},
        {10, new HUDInteractionDataStruct(10, "Dummy - TriggerChatAutomatically", new int[]{14}, true, true)},
        {11, new HUDInteractionDataStruct(11, "Dummy - TriggerChatAutomatically", new int[]{15}, true, true)},
        {12, new HUDInteractionDataStruct(12, "Dummy - TriggerChatAutomatically", new int[]{16}, true, true)},
        {13, new HUDInteractionDataStruct(13, "Dummy - TriggerChatAutomatically", new int[]{17}, true, true)},
        {14, new HUDInteractionDataStruct(14, "Hi", new int[]{20}, false, false)},
        {15, new HUDInteractionDataStruct(15, "Try some games!", new int[]{22}, false, false)},
        {16, new HUDInteractionDataStruct(16, "I have tried a game...", new int[]{26}, false, false)},
        {17, new HUDInteractionDataStruct(17, "Good Morning!", new int[]{28}, false, false)},
        {18, new HUDInteractionDataStruct(18, "Anything else?", new int[]{31}, false, false)},
        {19, new HUDInteractionDataStruct(19, "Welcome Back?", new int[]{32}, false, false)},
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
