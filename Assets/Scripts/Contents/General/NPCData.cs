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
        public bool bActiveByDefault;

        public NPCDataStruct(int ID, string Name, string Scene, Vector3 Position, int[] DefaultInteractionID, bool bActiveByDefault)
        {
            this.ID = ID;
            this.Name = Name;
            this.Scene = Scene;
            this.Position = Position;
            this.DefaultInteractionID = DefaultInteractionID;
            this.bActiveByDefault = bActiveByDefault;
        }
    }
    public static Dictionary<int, NPCDataStruct> data = new Dictionary<int, NPCDataStruct>
    {
        {1000, new NPCDataStruct(1000, "NPC_1_0", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1001, new NPCDataStruct(1001, "NPC_1_1", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1002, new NPCDataStruct(1002, "NPC_1_2", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1003, new NPCDataStruct(1003, "NPC_1_3", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1100, new NPCDataStruct(1100, "DNPC_Item_0", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1101, new NPCDataStruct(1101, "DNPC_Item_1", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1102, new NPCDataStruct(1102, "DNPC_Item_2", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1103, new NPCDataStruct(1103, "DNPC_Item_3", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
        {1200, new NPCDataStruct(1200, "DNPC_Trigger_0", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false)},
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
