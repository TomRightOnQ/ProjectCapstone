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
        public string TypeText;
        public int GroupID;
        public float TimeLimit;
        public int[] Hints;
        public string IntroText;
        public string IconPath;
        public string Next;
        public int[] Score;
        public int TaskComplete;

        public Level2DDataStruct(int ID, string Name, string SceneName, Enums.LEVEL_TYPE Type, string TypeText, int GroupID, float TimeLimit, int[] Hints, string IntroText, string IconPath, string Next, int[] Score, int TaskComplete)
        {
            this.ID = ID;
            this.Name = Name;
            this.SceneName = SceneName;
            this.Type = Type;
            this.TypeText = TypeText;
            this.GroupID = GroupID;
            this.TimeLimit = TimeLimit;
            this.Hints = Hints;
            this.IntroText = IntroText;
            this.IconPath = IconPath;
            this.Next = Next;
            this.Score = Score;
            this.TaskComplete = TaskComplete;
        }
    }
    public static Dictionary<int, Level2DDataStruct> data = new Dictionary<int, Level2DDataStruct>
    {
        {1, new Level2DDataStruct(1, "Platformer_1", "PlatformerLevel", Enums.LEVEL_TYPE.Platformer, "None", 0, -1f, new int[]{-1,-1,-1}, "None", "None", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {2, new Level2DDataStruct(2, "Track_1", "TrackLevel", Enums.LEVEL_TYPE.Track, "None", 0, -1f, new int[]{-1,-1,-1}, "None", "None", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {3, new Level2DDataStruct(3, "Dual_1", "DualLevel", Enums.LEVEL_TYPE.Dual, "None", 0, -1f, new int[]{-1,-1,-1}, "None", "None", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {4, new Level2DDataStruct(4, "Shooting Range - Stage 1", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {5, new Level2DDataStruct(5, "Shooting Range - Stage 2", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 1, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {6, new Level2DDataStruct(6, "Shooting Range - Stage 3", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 2, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {7, new Level2DDataStruct(7, "Shooting Range - Stage 4", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 3, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {8, new Level2DDataStruct(8, "Shooting Range - Stage 5", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 4, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {9, new Level2DDataStruct(9, "Shooting Range - Stage 6", "ShooterLevel_2", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 120f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {10, new Level2DDataStruct(10, "Shooting Range - Stage 7", "ShooterLevel_3", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 120f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {11, new Level2DDataStruct(11, "Shooting Range - Stage 8", "ShooterLevel_4", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 180f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {12, new Level2DDataStruct(12, "Shooting Range - Stage 9", "ShooterLevel_5", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 180f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
        {13, new Level2DDataStruct(13, "Shooting Range - Stage 10", "ShooterLevel_6", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 180f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", new int[]{2,30,3,20,4,25,5,16,6,17,7,18,8,19,9,12,10,10,11,6,12,7,13,5,14,7,15,8,1,7}, -1)},
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
