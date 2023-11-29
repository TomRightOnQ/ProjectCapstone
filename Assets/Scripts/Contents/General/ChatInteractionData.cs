using System.Collections.Generic;

using UnityEngine;

public static class ChatInteractionData
{
    public class ChatInteractionDataStruct
    {
        public int ID;
        public string Content;
        public bool bIsChoice;
        public string Speaker;
        public int[] Target;
        public Enums.INTERACTION_TYPE Action;
        public bool bEnd;
        public Enums.INTERACTION_EVENT Event;
        public int[] EventTarget;
        public int SpeakerID;

        public ChatInteractionDataStruct(int ID, string Content, bool bIsChoice, string Speaker, int[] Target, Enums.INTERACTION_TYPE Action, bool bEnd, Enums.INTERACTION_EVENT Event, int[] EventTarget, int SpeakerID)
        {
            this.ID = ID;
            this.Content = Content;
            this.bIsChoice = bIsChoice;
            this.Speaker = Speaker;
            this.Target = Target;
            this.Action = Action;
            this.bEnd = bEnd;
            this.Event = Event;
            this.EventTarget = EventTarget;
            this.SpeakerID = SpeakerID;
        }
    }
    public static Dictionary<int, ChatInteractionDataStruct> data = new Dictionary<int, ChatInteractionDataStruct>
    {
        {1, new ChatInteractionDataStruct(1, "This is a message.", false, "ChatInteraction", new int[]{2}, Enums.INTERACTION_TYPE.Next, false, Enums.INTERACTION_EVENT.None, new int[]{-1}, 1)},
        {2, new ChatInteractionDataStruct(2, "Now we have choices.", false, "You", new int[]{3,4}, Enums.INTERACTION_TYPE.Choice, false, Enums.INTERACTION_EVENT.None, new int[]{-1}, 1)},
        {3, new ChatInteractionDataStruct(3, "Go to the level", true, "ChatInteraction", new int[]{2}, Enums.INTERACTION_TYPE.Teleport, true, Enums.INTERACTION_EVENT.None, new int[]{-1}, 1)},
        {4, new ChatInteractionDataStruct(4, "I'm a choice to jump to the next one.", true, "ChatInteraction", new int[]{5}, Enums.INTERACTION_TYPE.Next, false, Enums.INTERACTION_EVENT.None, new int[]{-1}, 1)},
        {5, new ChatInteractionDataStruct(5, "This message will end the chat.", false, "ChatInteraction", new int[]{-1}, Enums.INTERACTION_TYPE.End, true, Enums.INTERACTION_EVENT.None, new int[]{-1}, 1)},
        {6, new ChatInteractionDataStruct(6, "Wanna try a sample game?", false, "Atom", new int[]{7,8,12,13}, Enums.INTERACTION_TYPE.Choice, false, Enums.INTERACTION_EVENT.None, new int[]{-1}, 2)},
        {7, new ChatInteractionDataStruct(7, "Yes I'm ready", true, "You", new int[]{1}, Enums.INTERACTION_TYPE.StartGame, true, Enums.INTERACTION_EVENT.None, new int[]{-1}, 2)},
        {8, new ChatInteractionDataStruct(8, "Nope...", true, "You", new int[]{-1}, Enums.INTERACTION_TYPE.End, true, Enums.INTERACTION_EVENT.None, new int[]{-1}, 2)},
        {9, new ChatInteractionDataStruct(9, "Ready for the the first day?", false, "Atom", new int[]{10,11}, Enums.INTERACTION_TYPE.Choice, false, Enums.INTERACTION_EVENT.None, new int[]{-1}, 2)},
        {10, new ChatInteractionDataStruct(10, "Yes I'm ready", true, "You", new int[]{-1}, Enums.INTERACTION_TYPE.End, true, Enums.INTERACTION_EVENT.CompleteTask, new int[]{1}, 2)},
        {11, new ChatInteractionDataStruct(11, "Nope...", true, "You", new int[]{-1}, Enums.INTERACTION_TYPE.End, true, Enums.INTERACTION_EVENT.None, new int[]{-1}, 2)},
        {12, new ChatInteractionDataStruct(12, "Try Shooter Level 1", true, "You", new int[]{4}, Enums.INTERACTION_TYPE.StartGame, true, Enums.INTERACTION_EVENT.None, new int[]{-1}, 2)},
        {13, new ChatInteractionDataStruct(13, "Try Shooter Level 2", true, "You", new int[]{5}, Enums.INTERACTION_TYPE.StartGame, true, Enums.INTERACTION_EVENT.None, new int[]{-1}, 2)},
    };

    public static ChatInteractionDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out ChatInteractionDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
