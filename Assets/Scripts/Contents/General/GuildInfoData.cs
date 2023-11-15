using System.Collections.Generic;

using UnityEngine;

public static class GuildInfoData
{
    public class GuildInfoDataStruct
    {
        public int ID;
        public string Name;

        public GuildInfoDataStruct(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }
    }
    public static Dictionary<int, GuildInfoDataStruct> data = new Dictionary<int, GuildInfoDataStruct>
    {
        {1, new GuildInfoDataStruct(1, "You")},
        {2, new GuildInfoDataStruct(2, "B")},
        {3, new GuildInfoDataStruct(3, "C")},
        {4, new GuildInfoDataStruct(4, "D")},
        {5, new GuildInfoDataStruct(5, "E")},
        {6, new GuildInfoDataStruct(6, "F")},
        {7, new GuildInfoDataStruct(7, "G")},
        {8, new GuildInfoDataStruct(8, "H")},
        {9, new GuildInfoDataStruct(9, "I")},
        {10, new GuildInfoDataStruct(10, "J")},
        {11, new GuildInfoDataStruct(11, "K")},
        {12, new GuildInfoDataStruct(12, "L")},
        {13, new GuildInfoDataStruct(13, "M")},
        {14, new GuildInfoDataStruct(14, "N")},
        {15, new GuildInfoDataStruct(15, "O")},
        {16, new GuildInfoDataStruct(16, "P")},
    };

    public static GuildInfoDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out GuildInfoDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
