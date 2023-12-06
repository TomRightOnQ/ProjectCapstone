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
        public int[] Choices;
        public int Next;
        public int[] Action;
        public bool bEnd;

        public ChatInteractionDataStruct(int ID, string Content, bool bIsChoice, string Speaker, int[] Choices, int Next, int[] Action, bool bEnd)
        {
            this.ID = ID;
            this.Content = Content;
            this.bIsChoice = bIsChoice;
            this.Speaker = Speaker;
            this.Choices = Choices;
            this.Next = Next;
            this.Action = Action;
            this.bEnd = bEnd;
        }
    }
    public static Dictionary<int, ChatInteractionDataStruct> data = new Dictionary<int, ChatInteractionDataStruct>
    {
        {1, new ChatInteractionDataStruct(1, "Welcome to the debug version!", false, "Guide", new int[]{-1}, 2, new int[]{-1}, false)},
        {2, new ChatInteractionDataStruct(2, "Now we have some minigames to play with...", false, "Guide", new int[]{3,4,5,6,7}, -1, new int[]{-1}, false)},
        {3, new ChatInteractionDataStruct(3, "Shooter Level 1", true, "Guide", new int[]{-1}, -1, new int[]{1}, true)},
        {4, new ChatInteractionDataStruct(4, "Shooter Level 2", true, "Guide", new int[]{-1}, -1, new int[]{2}, true)},
        {5, new ChatInteractionDataStruct(5, "Shooter Level 3", true, "Guide", new int[]{-1}, -1, new int[]{3}, true)},
        {6, new ChatInteractionDataStruct(6, "Shooter Level 4", true, "Guide", new int[]{-1}, -1, new int[]{4}, true)},
        {7, new ChatInteractionDataStruct(7, "Shooter Level 5", true, "Guide", new int[]{-1}, -1, new int[]{5}, true)},
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
