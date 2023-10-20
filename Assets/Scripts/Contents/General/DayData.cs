using System.Collections.Generic;

using UnityEngine;

public static class DayData
{
    public class DayDataStruct
    {
        public int ID;
        public string StartedScene;

        public DayDataStruct(int ID, string StartedScene)
        {
            this.ID = ID;
            this.StartedScene = StartedScene;
        }
    }
    public static Dictionary<int, DayDataStruct> data = new Dictionary<int, DayDataStruct>
    {
        {0, new DayDataStruct(0, "DefaultLevel")},
        {1, new DayDataStruct(1, "AudienceLevel")},
        {2, new DayDataStruct(2, "AudienceLevel")},
        {3, new DayDataStruct(3, "AudienceLevel")},
        {4, new DayDataStruct(4, "AudienceLevel")},
        {5, new DayDataStruct(5, "AudienceLevel")},
        {6, new DayDataStruct(6, "AudienceLevel")},
        {7, new DayDataStruct(7, "AudienceLevel")},
    };

    public static DayDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out DayDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
