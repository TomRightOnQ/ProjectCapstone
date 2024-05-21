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
        {1003, new TaskDataStruct(1003, "Me?", "Talk with Dove", "EntranceLevel", 1001, true, false)},
        {1004, new TaskDataStruct(1004, "Round 1", "Talk about today's match", "AudienceLevel", 1000, true, false)},
        {1005, new TaskDataStruct(1005, "Just a normal day", "Talk with Guide", "AudienceLevel", 1000, true, false)},
        {1006, new TaskDataStruct(1006, "Good night to the world", "Go to the team room and sleep...", "RoomALevel", 1102, true, false)},
        {1100, new TaskDataStruct(1100, "Say Hi", "None", "AudienceLevel", -1, true, true)},
        {1101, new TaskDataStruct(1101, "The mysterious man", "None", "GuildScene", -1, true, true)},
        {1013, new TaskDataStruct(1013, "Locked and Loaded", "Compete in the game", "EntranceLevel", 1001, true, false)},
        {1014, new TaskDataStruct(1014, "Locked and Loaded", "Compete in the game", "MatchingLevel", 1201, true, false)},
        {2000, new TaskDataStruct(2000, "A brave new day", "Talk with Guide", "RoomALevel", 1000, true, false)},
        {2001, new TaskDataStruct(2001, "A brave new day", "Head to Summit Feast", "GuildScene", 1000, true, false)},
        {2002, new TaskDataStruct(2002, "See you again", "Dove has something important for you, it's time to see her - again", "AudienceLevel", 1001, true, false)},
        {2003, new TaskDataStruct(2003, "So-called insider", "Match with Dove's suggestion", "AudienceLevel", 1004, true, false)},
        {2004, new TaskDataStruct(2004, "Fake News", "Our intellegence is incorrect", "RoomALevel", 1007, true, false)},
        {2005, new TaskDataStruct(2005, "Tomorrow will be better", "Go to the team room and sleep...", "RoomALevel", 1102, true, false)},
        {2100, new TaskDataStruct(2100, "Free food", "Don't forget to grab your free food, bro", "GuildScene", 2200, true, false)},
        {2101, new TaskDataStruct(2101, "Text Future", "Text Future", "AudienceLevel", -1, true, true)},
        {2102, new TaskDataStruct(2102, "Animal Crossing I", "Talk with the cat owner", "GuildScene", -1, true, true)},
        {2103, new TaskDataStruct(2103, "Animal Crossing II", "Talk with the cat", "GuildScene", -1, true, true)},
        {2104, new TaskDataStruct(2104, "The mysterious lady", "Talk with Samantha", "MatchingLevel", -1, true, true)},
        {3000, new TaskDataStruct(3000, "Postal Service", "Someone unexpected found you...", "AudienceLowLevel", 2002, true, false)},
        {9999, new TaskDataStruct(9999, "End Of Demo", "Here is the end of the demo version, the rest of the stories will be complete in the following months. Use Debug menu to try more levels and features.", "RoomALevel", -1, true, false)},
        {3100, new TaskDataStruct(3100, "Take Food", "Take Food", "GuildScene", -1, true, true)},
        {4100, new TaskDataStruct(4100, "Take Food", "Take Food", "GuildScene", -1, true, true)},
        {5100, new TaskDataStruct(5100, "Take Food", "Take Food", "GuildScene", -1, true, true)},
        {6100, new TaskDataStruct(6100, "Take Food", "Take Food", "GuildScene", -1, true, true)},
        {7100, new TaskDataStruct(7100, "Take Food", "Take Food", "GuildScene", -1, true, true)},
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
