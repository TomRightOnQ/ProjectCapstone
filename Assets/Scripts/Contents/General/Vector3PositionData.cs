using System.Collections.Generic;

using UnityEngine;

public static class Vector3PositionData
{
    public class Vector3PositionDataStruct
    {
        public int ID;
        public Vector3 Position;

        public Vector3PositionDataStruct(int ID, Vector3 Position)
        {
            this.ID = ID;
            this.Position = Position;
        }
    }
    public static Dictionary<int, Vector3PositionDataStruct> data = new Dictionary<int, Vector3PositionDataStruct>
    {
        {1, new Vector3PositionDataStruct(1, new Vector3(6.191f,1.22f,1.077f))},
        {2, new Vector3PositionDataStruct(2, new Vector3(-2.5f,7.25f,-2f))},
    };

    public static Vector3PositionDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out Vector3PositionDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
