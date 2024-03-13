using System.Collections.Generic;

using UnityEngine;

public static class StringConstData
{
    public class StringConstDataStruct
    {
        public int ID;
        public string Content;
        public bool bLocked;
        public string AlterContent;

        public StringConstDataStruct(int ID, string Content, bool bLocked, string AlterContent)
        {
            this.ID = ID;
            this.Content = Content;
            this.bLocked = bLocked;
            this.AlterContent = AlterContent;
        }
    }
    public static Dictionary<int, StringConstDataStruct> data = new Dictionary<int, StringConstDataStruct>
    {
        {0, new StringConstDataStruct(0, "You", false, "???")},
        {1, new StringConstDataStruct(1, "Shooting Range: Aim and destory targets to gain score", false, "???")},
        {2, new StringConstDataStruct(2, "PLACE_HOLDER", true, "???")},
        {3, new StringConstDataStruct(3, "PLACE_HOLDER", true, "???")},
        {4, new StringConstDataStruct(4, "Are you sure to flash back to the previous day?", false, "???")},
        {5, new StringConstDataStruct(5, "Are you sure to flash back to the morning of today?", false, "???")},
        {6, new StringConstDataStruct(6, "Are you sure to quit to the main menu? Unsaved game progress may be lost.", false, "???")},
        {999, new StringConstDataStruct(999, "Rules", false, "???")},
        {998, new StringConstDataStruct(998, "Connectio Check", false, "???")},
        {1000, new StringConstDataStruct(1000, "Guide", false, "???")},
        {1001, new StringConstDataStruct(1001, "Sponsor", false, "???")},
        {1002, new StringConstDataStruct(1002, "Friend", false, "???")},
        {1003, new StringConstDataStruct(1003, "Weird Guy", false, "???")},
        {1020, new StringConstDataStruct(1020, "Cat", false, "???")},
        {1021, new StringConstDataStruct(1021, "Cat Owner", false, "???")},
    };

    public static StringConstDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out StringConstDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
