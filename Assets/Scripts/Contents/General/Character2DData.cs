using System.Collections.Generic;

using UnityEngine;

public static class Character2DData
{
    public class Character2DDataStruct
    {
        public int ID;
        public string Name;
        public string PrefabName;

        public Character2DDataStruct(int ID, string Name, string PrefabName)
        {
            this.ID = ID;
            this.Name = Name;
            this.PrefabName = PrefabName;
        }
    }
    public static Dictionary<int, Character2DDataStruct> data = new Dictionary<int, Character2DDataStruct>
    {
        {1, new Character2DDataStruct(1, "A", "Player2D_1")},
        {2, new Character2DDataStruct(2, "B", "Player2D_2")},
        {3, new Character2DDataStruct(3, "C", "Player2D_3")},
        {4, new Character2DDataStruct(4, "D", "Player2D_4")},
        {5, new Character2DDataStruct(5, "E", "Player2D_5")},
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
