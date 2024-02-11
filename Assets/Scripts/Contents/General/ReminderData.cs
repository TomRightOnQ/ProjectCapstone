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
        {1, new ReminderDataStruct(1, "WASD to Move, F to Interact", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {2, new ReminderDataStruct(2, "A Few Hours Later...", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {3, new ReminderDataStruct(3, "Use Menu - Map to walk around!", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {4, new ReminderDataStruct(4, "You have unlocked a new piece of Note", 2f, "None", Enums.CHARACTER_TYPE.You)},
        {5, new ReminderDataStruct(5, "What is that red zone? I guess we should not touch it.", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {6, new ReminderDataStruct(6, "Maybe we can touch it when it's safe...", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {7, new ReminderDataStruct(7, "Avoid the Unstable Zone!", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {8, new ReminderDataStruct(8, "Now it's safe!", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {9, new ReminderDataStruct(9, "I need to find a way to break this shield", 2f, "You", Enums.CHARACTER_TYPE.You)},
        {10, new ReminderDataStruct(10, "They didn't tell me they are sending this for the match...", 2f, "Guide", Enums.CHARACTER_TYPE.Friend)},
        {11, new ReminderDataStruct(11, "Our bullets cannot get through that shield!", 2f, "Guide", Enums.CHARACTER_TYPE.Friend)},
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
