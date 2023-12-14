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
        {1, new StringConstDataStruct(1, "Shooting Range: Aim and destory targets to gain score", false, "???")},
        {2, new StringConstDataStruct(2, "PLACE_HOLDER", true, "???")},
        {3, new StringConstDataStruct(3, "PLACE_HOLDER", true, "???")},
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
