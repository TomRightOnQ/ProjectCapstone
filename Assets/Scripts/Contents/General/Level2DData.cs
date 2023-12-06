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
        public int GroupID;
        public int Complete;
        public float TimeLimit;
        public int ScoreGoal;
        public string Next;
        public int[] Score;

        public Level2DDataStruct(int ID, string Name, string SceneName, Enums.LEVEL_TYPE Type, int GroupID, int Complete, float TimeLimit, int ScoreGoal, string Next, int[] Score)
        {
            this.ID = ID;
            this.Name = Name;
            this.SceneName = SceneName;
            this.Type = Type;
            this.GroupID = GroupID;
            this.Complete = Complete;
            this.TimeLimit = TimeLimit;
            this.ScoreGoal = ScoreGoal;
            this.Next = Next;
            this.Score = Score;
        }
    }
    public static Dictionary<int, Level2DDataStruct> data = new Dictionary<int, Level2DDataStruct>
    {
        {1, new Level2DDataStruct(1, "Platformer_1", "PlatformerLevel", Enums.LEVEL_TYPE.Platformer, 0, 8, -1f, -1, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
        {2, new Level2DDataStruct(2, "Track_1", "TrackLevel", Enums.LEVEL_TYPE.Track, 0, -1, -1f, -1, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
        {3, new Level2DDataStruct(3, "Dual_1", "DualLevel", Enums.LEVEL_TYPE.Dual, 0, -1, -1f, -1, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
        {4, new Level2DDataStruct(4, "Shooter_1", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, 0, -1, 60f, 30, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
        {5, new Level2DDataStruct(5, "Shooter_2", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, 1, -1, 60f, 30, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
        {6, new Level2DDataStruct(6, "Shooter_3", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, 2, -1, 60f, 30, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
        {7, new Level2DDataStruct(7, "Shooter_4", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, 3, -1, 60f, 30, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
        {8, new Level2DDataStruct(8, "Shooter_5", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, 4, -1, 60f, 30, "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,16,7})},
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
