using System.Collections.Generic;

using UnityEngine;

public static class AchievementData
{
    public class AchievementDataStruct
    {
        public int ID;
        public string Name;
        public string Detail;
        public bool bHidden;
        public string Icon;

        public AchievementDataStruct(int ID, string Name, string Detail, bool bHidden, string Icon)
        {
            this.ID = ID;
            this.Name = Name;
            this.Detail = Detail;
            this.bHidden = bHidden;
            this.Icon = Icon;
        }
    }
    public static Dictionary<int, AchievementDataStruct> data = new Dictionary<int, AchievementDataStruct>
    {
        {0, new AchievementDataStruct(0, "TestAchievement_1", "This is a test achievement", false, "Icon_Gravity")},
        {1, new AchievementDataStruct(1, "TestAchievement_2", "This is a test achievement", false, "Icon_Leaf")},
        {2, new AchievementDataStruct(2, "TestAchievement_3", "This is a test achievement", true, "Icon_Laser")},
        {3, new AchievementDataStruct(3, "Back To The Future", "Send yourself a futrue text", false, "Icon_Leaf")},
    };

    public static AchievementDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out AchievementDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
