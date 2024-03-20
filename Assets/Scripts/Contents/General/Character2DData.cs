using System.Collections.Generic;

using UnityEngine;

public static class Character2DData
{
    public class Character2DDataStruct
    {
        public int ID;
        public string Name;
        public string PrefabName;
        public string InfoText;
        public string IconPath;
        public string AvatarPath;

        public Character2DDataStruct(int ID, string Name, string PrefabName, string InfoText, string IconPath, string AvatarPath)
        {
            this.ID = ID;
            this.Name = Name;
            this.PrefabName = PrefabName;
            this.InfoText = InfoText;
            this.IconPath = IconPath;
            this.AvatarPath = AvatarPath;
        }
    }
    public static Dictionary<int, Character2DDataStruct> data = new Dictionary<int, Character2DDataStruct>
    {
        {1, new Character2DDataStruct(1, "Darlan", "Player2D_1", "Text_Gravity", "Icon_Gravity", "Icon_P1_2D")},
        {2, new Character2DDataStruct(2, "Lanuarius", "Player2D_2", "Text_Leaf", "Icon_Leaf", "Icon_P2_2D")},
        {3, new Character2DDataStruct(3, "Francois", "Player2D_3", "Text_Laser", "Icon_Laser", "Icon_P3_2D")},
        {4, new Character2DDataStruct(4, "Chesteria", "Player2D_4", "Text_Missile", "Icon_Missile", "Icon_P4_2D")},
        {5, new Character2DDataStruct(5, "You", "Player2D_5", "Text_VT", "Icon_VT", "Icon_C1_2D")},
    };

    public static Character2DDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out Character2DDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
