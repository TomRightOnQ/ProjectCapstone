using System.Collections.Generic;

using UnityEngine;

public static class ReminderData
{
    public class ReminderDataStruct
    {
        public int ID;
        public string Content;
        public float Life;
        public string Speaker;
        public Enums.CHARACTER_TYPE SpeakerType;

        public ReminderDataStruct(int ID, string Content, float Life, string Speaker, Enums.CHARACTER_TYPE SpeakerType)
        {
            this.ID = ID;
            this.Content = Content;
            this.Life = Life;
            this.Speaker = Speaker;
            this.SpeakerType = SpeakerType;
        }
    }
    public static Dictionary<int, ReminderDataStruct> data = new Dictionary<int, ReminderDataStruct>
    {
        {0, new ReminderDataStruct(0, ".", 4f, "None", Enums.CHARACTER_TYPE.Enemy)},
        {1, new ReminderDataStruct(1, "WASD to Move, F to Interact", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {2, new ReminderDataStruct(2, "A Few Hours Later...", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {3, new ReminderDataStruct(3, "Use Menu - Map to walk around", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {4, new ReminderDataStruct(4, "Use Menu - Notes to check your notes", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {5, new ReminderDataStruct(5, "What is that red zone? I guess we should not touch it.", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {6, new ReminderDataStruct(6, "Maybe we can touch it when it's safe...", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {7, new ReminderDataStruct(7, "Avoid the Unstable Zone!", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {8, new ReminderDataStruct(8, "Now it's safe!", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {9, new ReminderDataStruct(9, "I need to find a way to break this shield", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {10, new ReminderDataStruct(10, "They didn't tell me they are sending this for the match...", 2f, "Guide", Enums.CHARACTER_TYPE.Friend)},
        {11, new ReminderDataStruct(11, "Our bullets cannot get through that shield!", 2f, "Guide", Enums.CHARACTER_TYPE.Friend)},
        {12, new ReminderDataStruct(12, "Who should we choose for this match...", 4f, "You", Enums.CHARACTER_TYPE.You)},
        {13, new ReminderDataStruct(13, "Wait a minute, why couldn't I fire at all?", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {14, new ReminderDataStruct(14, "This match is a challenge to your speed, and you shall dodge all attacks", 3f, "*The Auditorium*", Enums.CHARACTER_TYPE.Enemy)},
        {15, new ReminderDataStruct(15, "...", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {16, new ReminderDataStruct(16, "I don't think our team has enough score to proceed to the next day", 5f, "You", Enums.CHARACTER_TYPE.You)},
        {17, new ReminderDataStruct(17, "Auch ich habe den Wert der Zeit einst überschatzt, darum wollte ich hundert Jahre alt werden. In der Ewigkeit aber, siehst du, gibt es keine Zeit; die Ewigkeit ist bloss ein Augenblick, gerade lange genug fur einen Spass.\n\t\t\t\t\t Hermann Hesse, Steppenwolf", 6f, "None", Enums.CHARACTER_TYPE.You)},
        {18, new ReminderDataStruct(18, "To Whom Playing this Game", 4f, "None", Enums.CHARACTER_TYPE.Friend)},
        {19, new ReminderDataStruct(19, "...", 4f, "None", Enums.CHARACTER_TYPE.You)},
        {20, new ReminderDataStruct(20, "Where am I?", 4f, "None", Enums.CHARACTER_TYPE.You)},
        {21, new ReminderDataStruct(21, "\"Wake Up\"", 4f, "None", Enums.CHARACTER_TYPE.Friend)},
        {22, new ReminderDataStruct(22, "So I'm supposed to find that person named \"Dove\"... I can bearly remember what's going on.", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {23, new ReminderDataStruct(23, "I guess I'm in some sort of games, perhaps I should check the rules out.", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {24, new ReminderDataStruct(24, "At least I finally recall that I can do some sort of time travel.", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {25, new ReminderDataStruct(25, "Did I lose my memory for this reason? Well, I need to be cautious about it.", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {26, new ReminderDataStruct(26, "Maybe I can walk around before going back to the teamroom, how about the Matching Platform?", 6f, "You", Enums.CHARACTER_TYPE.You)},
        {27, new ReminderDataStruct(27, "It's almost the third day, I still cannot recall why I am here", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {28, new ReminderDataStruct(28, "Wait, why did I mention \"third\" in my mind... am I here for some reason?", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {29, new ReminderDataStruct(29, "I don't think I'd just time travel to simply win a match...", 3f, "You", Enums.CHARACTER_TYPE.You)},
        {10001, new ReminderDataStruct(10001, "DAY 1", 3f, "None", Enums.CHARACTER_TYPE.You)},
        {10002, new ReminderDataStruct(10002, "DAY 2", 3f, "None", Enums.CHARACTER_TYPE.You)},
        {10003, new ReminderDataStruct(10003, "DAY 3", 3f, "None", Enums.CHARACTER_TYPE.You)},
        {10004, new ReminderDataStruct(10004, "DAY 4", 3f, "None", Enums.CHARACTER_TYPE.You)},
        {10005, new ReminderDataStruct(10005, "DAY 5", 3f, "None", Enums.CHARACTER_TYPE.You)},
        {10006, new ReminderDataStruct(10006, "DAY 6", 3f, "None", Enums.CHARACTER_TYPE.You)},
        {10007, new ReminderDataStruct(10007, "DAY 7", 3f, "None", Enums.CHARACTER_TYPE.You)},
    };

    public static ReminderDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out ReminderDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
