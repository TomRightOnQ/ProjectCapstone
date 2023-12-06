using System.Collections.Generic;

using UnityEngine;

public static class StringConstData
{
    public class StringConstDataStruct
    {
        public int ID;
        public string Content;

        public StringConstDataStruct(int ID, string Content)
        {
            this.ID = ID;
            this.Content = Content;
        }
    }
    public static Dictionary<int, StringConstDataStruct> data = new Dictionary<int, StringConstDataStruct>
    {
        {1, new StringConstDataStruct(1, "Rapid-fired projectile influenced by gravity")},
        {2, new StringConstDataStruct(2, "Bursting small bullets with less damage")},
        {3, new StringConstDataStruct(3, "Laser with high damage and low ROF")},
        {4, new StringConstDataStruct(4, "Launching missiles dealing great damage")},
        {5, new StringConstDataStruct(5, "Shooting bullets with proximity fuze")},
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
