using System.Collections.Generic;

using UnityEngine;

public static class TaskData
{
    public class TaskDataStruct
    {
        public int ID;
        public string Name;
        public string Description;
        public string SceneName;
        public int TrackTarget;
        public bool bTrackNPC;
        public bool bHidden;

        public TaskDataStruct(int ID, string Name, string Description, string SceneName, int TrackTarget, bool bTrackNPC, bool bHidden)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            this.SceneName = SceneName;
            this.TrackTarget = TrackTarget;
            this.bTrackNPC = bTrackNPC;
            this.bHidden = bHidden;
        }
    }
    public static Dictionary<int, TaskDataStruct> data = new Dictionary<int, TaskDataStruct>
    {
        {1000, new TaskDataStruct(1000, "Hey, you are finally awake!", "...?", "RoomALevel", 1000, true, false)},
        {1001, new TaskDataStruct(1001, "Where am I?", "Check out the rules on the wall", "RoomALevel", 1100, true, false)},
        {1002, new TaskDataStruct(1002, "Where am I?", "Go for a walk outside", "RoomALevel", 1200, true, false)},
        {1003, new TaskDataStruct(1003, "Me?", "Talk with NPC_1_1", "DefaultLevel", 1001, true, false)},
        {1004, new TaskDataStruct(1004, "Round 1", "Talk about today's match", "AudienceLevel", 1000, true, false)},
        {1005, new TaskDataStruct(1005, "Just a normal day", "Talk with Guide", "RoomALevel", 1000, true, false)},
        {1006, new TaskDataStruct(1006, "Good night to the world", "Go to the team room and sleep...", "RoomALevel", 1102, true, false)},
        {1100, new TaskDataStruct(1100, "Say Hi", "None", "AudienceLevel", -1, true, true)},
        {1101, new TaskDataStruct(1101, "The mysterious man", "None", "GuildScene", -1, true, true)},
    };

    public static TaskDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out TaskDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
