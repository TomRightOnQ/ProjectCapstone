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
        public bool bLockCharacter;
        public int[] Score;
        public int[] TaskComplete;
        public string BGMName;

        public Level2DDataStruct(int ID, string Name, string SceneName, Enums.LEVEL_TYPE Type, string TypeText, int GroupID, float TimeLimit, int[] Hints, string IntroText, string IconPath, string Next, bool bLockCharacter, int[] Score, int[] TaskComplete, string BGMName)
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
            this.bLockCharacter = bLockCharacter;
            this.Score = Score;
            this.TaskComplete = TaskComplete;
            this.BGMName = BGMName;
        }
    }
    public static Dictionary<int, Level2DDataStruct> data = new Dictionary<int, Level2DDataStruct>
    {
        {1, new Level2DDataStruct(1, "Platformer_1", "PlatformerLevel", Enums.LEVEL_TYPE.Platformer, "None", 0, -1f, new int[]{-1,-1,-1}, "None", "None", "AudienceLevel", true, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM REGULAR LEVEL")},
        {2, new Level2DDataStruct(2, "Track_1", "TrackLevel", Enums.LEVEL_TYPE.Track, "None", 0, -1f, new int[]{-1,-1,-1}, "None", "None", "AudienceLevel", true, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM REGULAR LEVEL")},
        {3, new Level2DDataStruct(3, "Dual_1", "DualLevel", Enums.LEVEL_TYPE.Dual, "None", 0, -1f, new int[]{-1,-1,-1}, "None", "None", "AudienceLevel", true, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM REGULAR LEVEL")},
        {4, new Level2DDataStruct(4, "Shooting Range - Stage 1", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", true, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM REGULAR LEVEL")},
        {5, new Level2DDataStruct(5, "Shooting Range - Stage 2", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 1, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", true, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM REGULAR LEVEL")},
        {6, new Level2DDataStruct(6, "Shooting Range - Stage 3", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 2, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "RoomALevel", true, new int[]{150,32,144,98,105,165,30,55,65,16,45,122,68,106,72,48}, new int[]{2003}, "BGM REGULAR LEVEL")},
        {7, new Level2DDataStruct(7, "Shooting Range - Stage 4", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 3, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", true, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM REGULAR LEVEL")},
        {8, new Level2DDataStruct(8, "Shooting Range - Stage 5", "ShooterLevel", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 4, 60f, new int[]{1,2,3}, "Text_ShootingRange", "Img_ShootingRange", "AudienceLevel", true, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM REGULAR LEVEL")},
        {9, new Level2DDataStruct(9, "Shooting Range - Stage 6", "ShooterLevel_2", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 120f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", false, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM B1")},
        {10, new Level2DDataStruct(10, "Shooting Range - Stage 7", "ShooterLevel_3", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 120f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", false, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM B1")},
        {11, new Level2DDataStruct(11, "Shooting Range - Stage 8", "ShooterLevel_4", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 180f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", false, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM B3")},
        {12, new Level2DDataStruct(12, "Shooting Range - Stage 9", "ShooterLevel_5", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 180f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", false, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM B3")},
        {13, new Level2DDataStruct(13, "Shooting Range - Stage 10", "ShooterLevel_6", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 180f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", false, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}, new int[]{-1}, "BGM B2")},
        {14, new Level2DDataStruct(14, "Shooting Range - Stage 0", "ShooterLevel_T", Enums.LEVEL_TYPE.Shooter, "SHOOTING RANGE", 0, 60f, new int[]{-1,-1,-1}, "None", "Img_ShootingRange", "AudienceLevel", true, new int[]{75,15,60,55,42,98,12,14,33,18,22,50,13,28,19,40}, new int[]{1014}, "BGM B6")},
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
