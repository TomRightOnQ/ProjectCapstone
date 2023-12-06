using System.Collections.Generic;

using UnityEngine;

public static class Character2DData
{
    public class Character2DDataStruct
    {
        public int ID;
        public string Name;
        public string PrefabName;
        public int InfoText;

        public Character2DDataStruct(int ID, string Name, string PrefabName, int InfoText)
        {
            this.ID = ID;
            this.Name = Name;
            this.PrefabName = PrefabName;
            this.InfoText = InfoText;
        }
    }
    public static Dictionary<int, Character2DDataStruct> data = new Dictionary<int, Character2DDataStruct>
    {
        {1, new Character2DDataStruct(1, "Test Character A", "Player2D_1", 1)},
        {2, new Character2DDataStruct(2, "Test Character B", "Player2D_2", 2)},
        {3, new Character2DDataStruct(3, "Test Character C", "Player2D_3", 3)},
        {4, new Character2DDataStruct(4, "Test Character D", "Player2D_4", 4)},
        {5, new Character2DDataStruct(5, "Test Character E", "Player2D_5", 5)},
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
