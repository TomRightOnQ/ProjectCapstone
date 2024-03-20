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
        {10009, new HUDInteractionDataStruct(10009, "Sleep", new int[]{1021}, false, false)},
        {10010, new HUDInteractionDataStruct(10010, "???", new int[]{1016}, false, false)},
        {10011, new HUDInteractionDataStruct(10011, "Weird Guy", new int[]{1017}, false, false)},
        {10012, new HUDInteractionDataStruct(10012, "Enter Game", new int[]{1019}, false, false)},
        {10013, new HUDInteractionDataStruct(10013, "Go to the Audience Area", new int[]{1022}, false, false)},
        {20000, new HUDInteractionDataStruct(20000, "Good Morning", new int[]{2000}, false, true)},
        {20001, new HUDInteractionDataStruct(20001, "Check Connection Stability", new int[]{2001}, false, false)},
        {20002, new HUDInteractionDataStruct(20002, "Talk", new int[]{2003}, false, false)},
        {20008, new HUDInteractionDataStruct(20008, "Talk", new int[]{2004}, false, false)},
        {20009, new HUDInteractionDataStruct(20009, "Good to hear from you", new int[]{2008}, false, false)},
        {20010, new HUDInteractionDataStruct(20010, "Talk", new int[]{2009}, false, false)},
        {20011, new HUDInteractionDataStruct(20011, "Talk", new int[]{2012}, false, false)},
        {20012, new HUDInteractionDataStruct(20012, "...", new int[]{2015}, false, false)},
        {20013, new HUDInteractionDataStruct(20013, "Sleep", new int[]{2017}, false, false)},
        {21000, new HUDInteractionDataStruct(21000, "Take Food", new int[]{2005}, true, false)},
        {31000, new HUDInteractionDataStruct(31000, "Take Food", new int[]{-1}, true, false)},
        {41000, new HUDInteractionDataStruct(41000, "Take Food", new int[]{-1}, true, false)},
        {51000, new HUDInteractionDataStruct(51000, "Take Food", new int[]{-1}, true, false)},
        {61000, new HUDInteractionDataStruct(61000, "Take Food", new int[]{-1}, true, false)},
        {71000, new HUDInteractionDataStruct(71000, "Take Food", new int[]{-1}, true, false)},
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
