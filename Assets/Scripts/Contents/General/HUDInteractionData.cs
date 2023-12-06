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
        {1, new HUDInteractionDataStruct(1, "Enter Team Room", new int[]{6}, false, false)},
        {2, new HUDInteractionDataStruct(2, "Exit Team Room", new int[]{7}, false, false)},
        {3, new HUDInteractionDataStruct(3, "ShooterLevel 1", new int[]{1}, false, false)},
        {4, new HUDInteractionDataStruct(4, "ShooterLevel 2", new int[]{2}, false, false)},
        {5, new HUDInteractionDataStruct(5, "ShooterLevel 3", new int[]{3}, false, false)},
        {6, new HUDInteractionDataStruct(6, "ShooterLevel 4", new int[]{4}, false, false)},
        {7, new HUDInteractionDataStruct(7, "ShooterLevel 5", new int[]{5}, false, false)},
        {8, new HUDInteractionDataStruct(8, "Welcome to the Debug version!", new int[]{8}, false, false)},
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
