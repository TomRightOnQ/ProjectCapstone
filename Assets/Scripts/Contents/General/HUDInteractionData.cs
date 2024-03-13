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
        {10000, new HUDInteractionDataStruct(10000, "Hi...?", new int[]{1000}, false, true)},
        {10001, new HUDInteractionDataStruct(10001, "Guide", new int[]{1002}, false, false)},
        {10002, new HUDInteractionDataStruct(10002, "Read", new int[]{1003}, false, false)},
        {10003, new HUDInteractionDataStruct(10003, "Check Connection Stability", new int[]{1005}, false, false)},
        {10004, new HUDInteractionDataStruct(10004, "Walk outside", new int[]{1006}, false, false)},
        {10005, new HUDInteractionDataStruct(10005, "Talk", new int[]{1007}, false, false)},
        {10006, new HUDInteractionDataStruct(10006, "Guide", new int[]{1009}, false, false)},
        {10007, new HUDInteractionDataStruct(10007, "?", new int[]{1010}, false, false)},
        {10008, new HUDInteractionDataStruct(10008, "Looks Good!", new int[]{1013}, false, false)},
        {10009, new HUDInteractionDataStruct(10009, "Sleep!", new int[]{1015}, false, false)},
        {10010, new HUDInteractionDataStruct(10010, "???", new int[]{1016}, false, false)},
        {10011, new HUDInteractionDataStruct(10011, "Weird Guy", new int[]{1017}, false, false)},
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
