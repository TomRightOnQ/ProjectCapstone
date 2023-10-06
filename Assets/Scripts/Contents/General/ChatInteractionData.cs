using System.Collections.Generic;

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

        public ChatInteractionDataStruct(int ID, string Content, bool bIsChoice, string Speaker, int[] Target, Enums.INTERACTION_TYPE Action, bool bEnd)
        {
            this.ID = ID;
            this.Content = Content;
            this.bIsChoice = bIsChoice;
            this.Speaker = Speaker;
            this.Target = Target;
            this.Action = Action;
            this.bEnd = bEnd;
        }
    }
    public static Dictionary<int, ChatInteractionDataStruct> data = new Dictionary<int, ChatInteractionDataStruct>
    {
        {1, new ChatInteractionDataStruct(1, "This is a message.", false, "ChatInteraction", new int[]{2}, Enums.INTERACTION_TYPE.Next, false)},
        {2, new ChatInteractionDataStruct(2, "Now we have choices.", false, "You", new int[]{3,4}, Enums.INTERACTION_TYPE.Choice, false)},
        {3, new ChatInteractionDataStruct(3, "Here's a choice to end the message.", true, "ChatInteraction", new int[]{0}, Enums.INTERACTION_TYPE.End, true)},
        {4, new ChatInteractionDataStruct(4, "I'm a choice to jump to the next one.", true, "ChatInteraction", new int[]{5}, Enums.INTERACTION_TYPE.Next, false)},
        {5, new ChatInteractionDataStruct(5, "This message will end the chat.", false, "ChatInteraction", new int[]{0}, Enums.INTERACTION_TYPE.End, true)},
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
