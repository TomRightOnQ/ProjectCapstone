using System.Collections.Generic;

using UnityEngine;

public static class Level2DData
{
    public class Level2DDataStruct
    {
        public int ID;
        public string Name;
        public string SceneName;
        public Enums.LEVEL_TYPE Type;
        public string Next;
        public int Complete;
        public float TimeLimit;
        public int ScoreGoal;

        public Level2DDataStruct(int ID, string Name, string SceneName, Enums.LEVEL_TYPE Type, string Next, int Complete, float TimeLimit, int ScoreGoal)
        {
            this.ID = ID;
            this.Name = Name;
            this.SceneName = SceneName;
            this.Type = Type;
            this.Next = Next;
            this.Complete = Complete;
            this.TimeLimit = TimeLimit;
            this.ScoreGoal = ScoreGoal;
        }
    }
    public static Dictionary<int, Level2DDataStruct> data = new Dictionary<int, Level2DDataStruct>
    {
        {1, new Level2DDataStruct(1, "Platformer_1", "PlatformerLevel", Enums.LEVEL_TYPE.Platformer, "AudienceLevel", 8, -1f, -1)},
        {2, new Level2DDataStruct(2, "Track_1", "TrackLevel", Enums.LEVEL_TYPE.Track, "AudienceLevel", -1, -1f, -1)},
        {3, new Level2DDataStruct(3, "Dual_1", "DualLevel", Enums.LEVEL_TYPE.Dual, "AudienceLevel", -1, -1f, -1)},
        {4, new Level2DDataStruct(4, "Shooter_1", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "AudienceLevel", -1, 60f, 30)},
    };

    public static Level2DDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out Level2DDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
