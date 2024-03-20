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
        public string Comment;

        public NPCDataStruct(int ID, string Name, string Scene, Vector3 Position, int[] DefaultInteractionID, bool bActiveByDefault, string Comment)
        {
            this.ID = ID;
            this.Name = Name;
            this.Scene = Scene;
            this.Position = Position;
            this.DefaultInteractionID = DefaultInteractionID;
            this.bActiveByDefault = bActiveByDefault;
            this.Comment = Comment;
        }
    }
    public static Dictionary<int, NPCDataStruct> data = new Dictionary<int, NPCDataStruct>
    {
        {1000, new NPCDataStruct(1000, "NPC_S0_Guide", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_S0_Guide")},
        {1001, new NPCDataStruct(1001, "NPC_C3_Dove", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_C3_Dove")},
        {1002, new NPCDataStruct(1002, "NPC_S1_Jack", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_S1_Jack")},
        {1003, new NPCDataStruct(1003, "NPC_S2_Bacon", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_S2_Bacon")},
        {1004, new NPCDataStruct(1004, "NPC_P1_Darlan", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_P1_Darlan")},
        {1005, new NPCDataStruct(1005, "NPC_P2_Lanuarius", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_P2_Lanuarius")},
        {1006, new NPCDataStruct(1006, "NPC_P3_Francois", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_P3_Francois")},
        {1007, new NPCDataStruct(1007, "NPC_P4_Chesteria", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_P4_Chesteria")},
        {1008, new NPCDataStruct(1008, "NPC_C2_Veronique", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NPC_C2_Veronique")},
        {1100, new NPCDataStruct(1100, "DNPC_Item_0", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "Rules")},
        {1101, new NPCDataStruct(1101, "DNPC_Item_1", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "NULL")},
        {1102, new NPCDataStruct(1102, "DNPC_Item_2", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "Sleep")},
        {1103, new NPCDataStruct(1103, "DNPC_Item_3", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "Connection Check")},
        {1200, new NPCDataStruct(1200, "DNPC_Trigger_0", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "Walk Outside")},
        {1201, new NPCDataStruct(1201, "DNPC_Trigger_1", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "Join Game (Day 1)")},
        {2200, new NPCDataStruct(2200, "DNPC_Trigger_TakeFood", "DefaultLevel", new Vector3(0f,0f,0f), new int[]{-1}, false, "Take Food Trigger")},
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
