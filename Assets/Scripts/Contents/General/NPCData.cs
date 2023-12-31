using System.Collections.Generic;

using UnityEngine;

public static class NPCData
{
    public class NPCDataStruct
    {
        public int ID;
        public string Name;
        public string Scene;
        public Vector3 Position;
        public int[] DefaultInteractionID;
        public int[] DefaultRingID;
        public bool bActiveByDefault;

        public NPCDataStruct(int ID, string Name, string Scene, Vector3 Position, int[] DefaultInteractionID, int[] DefaultRingID, bool bActiveByDefault)
        {
            this.ID = ID;
            this.Name = Name;
            this.Scene = Scene;
            this.Position = Position;
            this.DefaultInteractionID = DefaultInteractionID;
            this.DefaultRingID = DefaultRingID;
            this.bActiveByDefault = bActiveByDefault;
        }
    }
    public static Dictionary<int, NPCDataStruct> data = new Dictionary<int, NPCDataStruct>
    {
        {1, new NPCDataStruct(1, "Chatter", "DefaultLevel", new Vector3(2f,0.5f,0f), new int[]{8}, new int[]{1}, true)},
    };

    public static NPCDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out NPCDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
