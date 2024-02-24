using System.Collections.Generic;

using UnityEngine;

public static class DayData
{
    public class DayDataStruct
    {
        public int ID;
        public string StartedScene;
        public int RealDay;
        public int PrevDayID;
        public int NextDayID;
        public int RemainTeam;
        public int GameEnd;

        public DayDataStruct(int ID, string StartedScene, int RealDay, int PrevDayID, int NextDayID, int RemainTeam, int GameEnd)
        {
            this.ID = ID;
            this.StartedScene = StartedScene;
            this.RealDay = RealDay;
            this.PrevDayID = PrevDayID;
            this.NextDayID = NextDayID;
            this.RemainTeam = RemainTeam;
            this.GameEnd = GameEnd;
        }
    }
    public static Dictionary<int, DayDataStruct> data = new Dictionary<int, DayDataStruct>
    {
        {0, new DayDataStruct(0, "DefaultLevel", 0, -1, -1, 16, 1)},
        {1, new DayDataStruct(1, "RoomALevel", 1, 0, 2, 1, 1)},
        {2, new DayDataStruct(2, "AudienceLevel", 2, 1, 3, 16, 1)},
        {3, new DayDataStruct(3, "AudienceLevel", 3, 2, 4, 16, 1)},
        {4, new DayDataStruct(4, "AudienceLevel", 4, 3, 5, 8, 1)},
        {5, new DayDataStruct(5, "AudienceLevel", 5, 4, 6, 4, 2)},
        {6, new DayDataStruct(6, "AudienceLevel", 6, 5, 7, 2, 3)},
        {7, new DayDataStruct(7, "AudienceLevel", 7, 6, -1, 1, 4)},
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
