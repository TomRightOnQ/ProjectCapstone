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
        {2, new ChatInteractionDataStruct(2, "Anyway, we do not have the actual story but a short demo to slice the game.", false, "Guide", new int[]{-1}, 3, new int[]{-1}, false)},
        {3, new ChatInteractionDataStruct(3, "As you can see, I'm an NPC that can talk with; now you can walk around and come back to me~ WASD to move. We do not have colliders on the edge of this scene, so watch your step or you need to restart.", false, "Guide", new int[]{-1}, -1, new int[]{9,10}, true)},
        {4, new ChatInteractionDataStruct(4, "Nice, now let me take you to another scene.", false, "Guide", new int[]{-1}, -1, new int[]{12,13,107}, true)},
        {5, new ChatInteractionDataStruct(5, "Some conversations can also start without clicking anything, such as a critical story point", false, "OffScreen Voice", new int[]{-1}, 6, new int[]{-1}, false)},
        {6, new ChatInteractionDataStruct(6, "In a short introduction, you are competing for a team for a grand game, and this is where your teammates rest in spare time", false, "OffScreen Voice", new int[]{-1}, 7, new int[]{-1}, false)},
        {7, new ChatInteractionDataStruct(7, "Anyway, you can walk to the door on the right side to exit the level.", false, "OffScreen Voice", new int[]{-1}, -1, new int[]{18}, true)},
        {103, new ChatInteractionDataStruct(103, "Shooter Level 1", true, "Guide", new int[]{-1}, -1, new int[]{1}, true)},
        {104, new ChatInteractionDataStruct(104, "Shooter Level 2", true, "Guide", new int[]{-1}, -1, new int[]{2}, true)},
        {105, new ChatInteractionDataStruct(105, "Shooter Level 3", true, "Guide", new int[]{-1}, -1, new int[]{3}, true)},
        {106, new ChatInteractionDataStruct(106, "Shooter Level 4", true, "Guide", new int[]{-1}, -1, new int[]{4}, true)},
        {107, new ChatInteractionDataStruct(107, "Shooter Level 5", true, "Guide", new int[]{-1}, -1, new int[]{5}, true)},
        {8, new ChatInteractionDataStruct(8, "Good to see you again! On the audience stands area for your team, you will talk with differnet NPCs", false, "Guide", new int[]{-1}, -1, new int[]{104}, true)},
        {9, new ChatInteractionDataStruct(9, "This is also the audience stands, but in low level, and you may meet some random people here", false, "Guide", new int[]{-1}, -1, new int[]{108}, true)},
        {10, new ChatInteractionDataStruct(10, "Theis scene was originally designed for the player's guild, but now it may be something else", false, "Guide", new int[]{-1}, 11, new int[]{-1}, false)},
        {11, new ChatInteractionDataStruct(11, "After we finish everything, you can use the Debug to enter differnent scenes, but since it's a force debugging choice, it may lead to undefined behaivours", false, "Guide", new int[]{-1}, -1, new int[]{19,107}, true)},
        {12, new ChatInteractionDataStruct(12, "So on each day, you are talking with various NPCs for the story, and then you will compete for several games", false, "Guide", new int[]{-1}, 13, new int[]{-1}, false)},
        {13, new ChatInteractionDataStruct(13, "What we have now is a set of shooting range level, which you must collect enough information to choose the correct character to play.", false, "Guide", new int[]{-1}, -1, new int[]{21,23,35}, true)},
        {14, new ChatInteractionDataStruct(14, "Here are some levels, try anyone except the empty level 5 ", false, "Guide", new int[]{103,104,105,106,107}, -1, new int[]{-1}, false)},
        {15, new ChatInteractionDataStruct(15, "When you start the level, you can see some sort of intro on the character; to unlock the hints for the level, you can try to talk with more NPCs", false, "Guide", new int[]{-1}, 16, new int[]{-1}, false)},
        {16, new ChatInteractionDataStruct(16, "After each level, you will score according to your performance. You can check Menu-FlashBack to see the total score ranking", false, "Guide", new int[]{-1}, 17, new int[]{-1}, false)},
        {17, new ChatInteractionDataStruct(17, "This is not complete yet, but at the end of each day, some teams with low score will get eliminated", false, "Guide", new int[]{-1}, 18, new int[]{-1}, false)},
        {18, new ChatInteractionDataStruct(18, "Thus, strive for a better score to unlock the real ending on the last day!", false, "Guide", new int[]{-1}, 19, new int[]{-1}, false)},
        {19, new ChatInteractionDataStruct(19, "After you have completed eveything for a day, you can go to the next day; just click the button to do so!", false, "Guide", new int[]{-1}, -1, new int[]{25,27,0}, true)},
        {20, new ChatInteractionDataStruct(20, "Now it's another day... you can always use Menu-FlashBack to go back to the any day, but once you go back, there is no way to return to the future!", false, "Guide", new int[]{-1}, -1, new int[]{29,30}, true)},
        {21, new ChatInteractionDataStruct(21, "This is the end of the demo, taking me about 4 hours to make, including some other tweak in coding. In the best case, we can make a few minutes of gameplay per day. I will keep making the system easier for the designer to add things. Thanks for playing! Tips: I suggest you to flashback to day 0 right now!", false, "Guide", new int[]{103,104,105,106,107}, -1, new int[]{34}, false)},
        {22, new ChatInteractionDataStruct(22, "You are back to the first day, but we are able to keep track of your time travel...this will also become an important thing.", false, "Guide", new int[]{103,104,105,106,107}, -1, new int[]{-1}, false)},
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
