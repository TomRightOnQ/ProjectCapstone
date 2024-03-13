using System.Collections.Generic;

using UnityEngine;

public static class ChatInteractionData
{
    public class ChatInteractionDataStruct
    {
        public int ID;
        public string Content;
        public bool bIsChoice;
        public int Speaker;
        public int[] Choices;
        public int Next;
        public int[] Action;
        public bool bEnd;

        public ChatInteractionDataStruct(int ID, string Content, bool bIsChoice, int Speaker, int[] Choices, int Next, int[] Action, bool bEnd)
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
        {100, new ChatInteractionDataStruct(100, "All players shall praise Sportsmanship over competition, and avoid conflicts with the opposing players during the match.Players may check their connection stability to the system in own team rooms. Please make sure the stability is higher than 90.", false, 999, new int[]{-1}, 101, new int[]{-1}, false)},
        {101, new ChatInteractionDataStruct(101, "In the virtual environment for the matches, death is also impossible. However, all feelings are 100% resembled as the reality; do not touch any players currently in connection to avoid unexpected severe results!", false, 999, new int[]{-1}, 102, new int[]{-1}, false)},
        {102, new ChatInteractionDataStruct(102, "Please keep the time and do not arrive late. All players' abilities and magic are re-balanced and limited during the matches.", false, 999, new int[]{-1}, 103, new int[]{-1}, false)},
        {103, new ChatInteractionDataStruct(103, "Report any problem to your guild leader first, then to the game administration.", false, 999, new int[]{-1}, 104, new int[]{-1}, false)},
        {104, new ChatInteractionDataStruct(104, "For safety, do not use any abilities or magic in the real world unless specified.", false, 999, new int[]{-1}, -1, new int[]{-1}, true)},
        {10000, new ChatInteractionDataStruct(10000, "Hey buddy, is everything going alright? What are you daydreaming about?", false, 1000, new int[]{-1}, 10001, new int[]{-1}, false)},
        {10001, new ChatInteractionDataStruct(10001, "...? Where am I?", false, 0, new int[]{-1}, 10002, new int[]{-1}, false)},
        {10002, new ChatInteractionDataStruct(10002, "Lost your memory, huh? It's already morning. Hurry up and get ready to go out. Our guild will soon have a match. NPC_0_1 needs your help. Go find her quickly!", false, 1000, new int[]{10003,10004}, -1, new int[]{-1}, false)},
        {10003, new ChatInteractionDataStruct(10003, "What are you talking about? I don't understand.", true, 0, new int[]{-1}, 10005, new int[]{-1}, false)},
        {10004, new ChatInteractionDataStruct(10004, "Oh, okay, I'll go find her now.", true, 0, new int[]{-1}, 10006, new int[]{-1}, false)},
        {10005, new ChatInteractionDataStruct(10005, "PLAYER_NAME, have you lost your mind? You don't even recognize me, your guild leader, anymore. No wonder npc1 said you've been acting weird lately, always forgetting things. We'll check it out after the match! By the way, the rules of the match are posted on the wall. Make sure to check them and link them to your neural network.", false, 1000, new int[]{-1}, -1, new int[]{1001}, true)},
        {10006, new ChatInteractionDataStruct(10006, "Right, the rules of the match are posted on the wall. Make sure to check them. Remember to link them to your neural network.", false, 1000, new int[]{-1}, -1, new int[]{1001}, true)},
        {10007, new ChatInteractionDataStruct(10007, "What's going on? It's the time for the match.", false, 1000, new int[]{-1}, -1, new int[]{-1}, true)},
        {10008, new ChatInteractionDataStruct(10008, "DAY 1 SCHEDULE", false, 999, new int[]{-1}, 10009, new int[]{-1}, false)},
        {10009, new ChatInteractionDataStruct(10009, "MORNING MATCH: MOBILITY", false, 999, new int[]{-1}, 10010, new int[]{-1}, false)},
        {10010, new ChatInteractionDataStruct(10010, "AFTERNOON MATCH: SHOOTING RANGE", false, 999, new int[]{-1}, 100, new int[]{1004}, false)},
        {10011, new ChatInteractionDataStruct(10011, "TODAY's Stability: [90]", false, 998, new int[]{-1}, -1, new int[]{-1}, true)},
        {10012, new ChatInteractionDataStruct(10012, "What is really going on... I need to calm down for a bit.", false, 0, new int[]{-1}, 10012, new int[]{-1}, false)},
        {10013, new ChatInteractionDataStruct(10013, "Anyway I'll go meet whoever that person juse mentioned.", false, 0, new int[]{-1}, -1, new int[]{1006}, true)},
        {10014, new ChatInteractionDataStruct(10014, "The organizers really did a number on us with such last-minute notice...", false, 1001, new int[]{10015,10016}, -1, new int[]{-1}, false)},
        {10015, new ChatInteractionDataStruct(10015, "What happened?", true, 0, new int[]{-1}, 10017, new int[]{-1}, false)},
        {10016, new ChatInteractionDataStruct(10016, "Got banned from matching since your company is a sponsor?", true, 0, new int[]{-1}, 10018, new int[]{-1}, false)},
        {10017, new ChatInteractionDataStruct(10017, "Well, I was told that since my company is a sponsor of the event, I cannot compete in any match.", false, 1001, new int[]{-1}, 10018, new int[]{-1}, false)},
        {10018, new ChatInteractionDataStruct(10018, "Don't you find it strange? Everything was set, they even sent me the schedule. Apparently, someone anonymously sent a protest letter to the organizers. Anyway, our guild only has you on site now. It sounds strange to ask you, who hasn't been trained, to replace me in the games... we are sending OTHER_CHARACTER and OTHER_CHARACTER for today, don't worry for today.", false, 1001, new int[]{-1}, 10019, new int[]{-1}, false)},
        {10019, new ChatInteractionDataStruct(10019, "O...Kay, so may I have a more detailed schedule? I mean I did see a brief one for today.", false, 0, new int[]{-1}, 10020, new int[]{-1}, false)},
        {10020, new ChatInteractionDataStruct(10020, "[Passing a calendar] Here it is! We have to compete for 5 days, and the other two days are free time. [You can always go to the Menu to check the current day and the score of each team.]", false, 1001, new int[]{-1}, 10021, new int[]{-1}, false)},
        {10021, new ChatInteractionDataStruct(10021, "[You may want to check your Notes in the Menu, which records some important information.]", false, 1001, new int[]{-1}, 10022, new int[]{-1}, false)},
        {10022, new ChatInteractionDataStruct(10022, "Most important thing, during the event, we are prohibitited from using any magic, even during the two days without matches.", false, 1001, new int[]{-1}, 10023, new int[]{-1}, false)},
        {10023, new ChatInteractionDataStruct(10023, "I see...", false, 0, new int[]{-1}, 10024, new int[]{-1}, false)},
        {10024, new ChatInteractionDataStruct(10024, "It's mostly a safety thing; during the matches we are in a virtual environment with balance, but in reality many magical spells are deadly. If someone accidentally turns an area into vapors or warps the time, things will be messy...", false, 1001, new int[]{-1}, 10025, new int[]{-1}, false)},
        {10025, new ChatInteractionDataStruct(10025, "Well, your time-travel ability is not that harmful, and perhaps no one can sense it, but please do not use it; we are all **** up if we got caught.", false, 1001, new int[]{-1}, 10026, new int[]{-1}, false)},
        {10026, new ChatInteractionDataStruct(10026, "Wait, time-travel? How did you know I can flash back to the past?", false, 0, new int[]{-1}, 10027, new int[]{-1}, false)},
        {10027, new ChatInteractionDataStruct(10027, "Yeah, your hidden ability is one of the secrets of our guild, because it's something affects the rule of the world. Didn't you remember I was one of the witnesses?", false, 1001, new int[]{-1}, 10028, new int[]{-1}, false)},
        {10028, new ChatInteractionDataStruct(10028, "(My memory... is still partial... I have to keep silence to avoid more troubles...) Oof I almost forget about it; I mean it's a secret I really cannot say, even cannot think.", false, 0, new int[]{-1}, 10029, new int[]{-1}, false)},
        {10029, new ChatInteractionDataStruct(10029, "It's fabulous if you are not losing any memory. Anyway, we should get prepared for the first match!", false, 1001, new int[]{-1}, -1, new int[]{1008}, true)},
        {10030, new ChatInteractionDataStruct(10030, "You've talked to her, right? Tomorrow you'll match for us. Today we can just relax and cheer for the team.", false, 1000, new int[]{-1}, 10031, new int[]{-1}, false)},
        {10031, new ChatInteractionDataStruct(10031, "We have already become the best guild over the nation, but you know our real strengths are forcily balanced in games, and in the previous years we always lost for some strange reasons.", false, 1000, new int[]{-1}, 10032, new int[]{-1}, false)},
        {10032, new ChatInteractionDataStruct(10032, "Anyway you are one of our strongest, this time we will win, even without special training.", false, 1000, new int[]{-1}, 10033, new int[]{-1}, false)},
        {10033, new ChatInteractionDataStruct(10033, "I see... so some opponents are specially trained for these matches, but in reality they are not that strong. ", false, 0, new int[]{10034}, -1, new int[]{-1}, false)},
        {10034, new ChatInteractionDataStruct(10034, "Let the show begin!", true, 0, new int[]{-1}, -1, new int[]{1011}, true)},
        {10035, new ChatInteractionDataStruct(10035, "Hey bro! Good to see you again. They told me you are gonna match for NPC_1_1", false, 1002, new int[]{-1}, 10036, new int[]{-1}, false)},
        {10036, new ChatInteractionDataStruct(10036, "Bruh why suddenly the entire world knows it, but it's not surprising to see you in our guild's VIP area.Is your guild also in the game?", false, 0, new int[]{-1}, 10037, new int[]{-1}, false)},
        {10037, new ChatInteractionDataStruct(10037, "Not for this year, our master is out for a conference. Who knows what happened to our emperor... he asks us to form a national team to join the match.", false, 1002, new int[]{-1}, 10038, new int[]{-1}, false)},
        {10038, new ChatInteractionDataStruct(10038, "All of my homies are here, but the team tile is Team Wozen National; kinda weird for a \"national team\" appearing in a game with teams formed by guilds.", false, 0, new int[]{-1}, 10039, new int[]{-1}, false)},
        {10039, new ChatInteractionDataStruct(10039, "(I know who he is, but... my brain it's still foggy)", false, 0, new int[]{-1}, 10040, new int[]{-1}, false)},
        {10040, new ChatInteractionDataStruct(10040, "(Perhaps I've got to ask more about the guilds.) Anyway we can still hangout in spare time. Well, what's going on with other guilds?", false, 0, new int[]{-1}, 10041, new int[]{-1}, false)},
        {10041, new ChatInteractionDataStruct(10041, "You haven't read about them yet? I thought you're always prepared. This time thirteen teams are in the world championship... Most of them are our old friends.", false, 1002, new int[]{-1}, 10042, new int[]{-1}, false)},
        {10042, new ChatInteractionDataStruct(10042, "Except that team... the... SPECIAL_TEAM_NAME, do you know it?", false, 1002, new int[]{-1}, 10043, new int[]{-1}, false)},
        {10043, new ChatInteractionDataStruct(10043, "Nah, me either.", false, 0, new int[]{-1}, 10044, new int[]{-1}, false)},
        {10044, new ChatInteractionDataStruct(10044, "They are a bit suspicious; I'll keep an eye on them. Buddy, you've better been prepared, we might get drawn to the same game.", false, 1002, new int[]{-1}, 10045, new int[]{-1}, false)},
        {10045, new ChatInteractionDataStruct(10045, "I will be ready. If I saw you in the field I would not show mercy.", false, 0, new int[]{-1}, 10046, new int[]{-1}, false)},
        {10046, new ChatInteractionDataStruct(10046, "XD, me too. Good luck bro, I have to go back to my team's VIP area.", false, 1002, new int[]{-1}, -1, new int[]{1012}, true)},
        {10047, new ChatInteractionDataStruct(10047, "Good game; I mean a really good game, not GG. (Notice: After Dynamic Time is ready, this scene will be altered to sunset time)", false, 0, new int[]{-1}, 10048, new int[]{-1}, false)},
        {10048, new ChatInteractionDataStruct(10048, "I was so nervous... we should celebrate that this year we did not make stupid mistakes on the first day!", false, 1000, new int[]{-1}, 10049, new int[]{-1}, false)},
        {10049, new ChatInteractionDataStruct(10049, "Easier than I thought. I guess tomorrow will be fun.", false, 0, new int[]{-1}, 10050, new int[]{-1}, false)},
        {10050, new ChatInteractionDataStruct(10050, "I have a feeling that we can win the champion this time!", false, 1000, new int[]{-1}, 10051, new int[]{-1}, false)},
        {10051, new ChatInteractionDataStruct(10051, "Well, take a good rest and get ready for tomorrow. Matches will become harder and harder!", false, 1000, new int[]{-1}, -1, new int[]{1014}, true)},
        {10052, new ChatInteractionDataStruct(10052, "This is the end of the first day story, more contents are in-progress", false, 0, new int[]{-1}, -1, new int[]{-1}, true)},
        {10053, new ChatInteractionDataStruct(10053, "Anybody here?", false, 0, new int[]{-1}, 10054, new int[]{-1}, false)},
        {10054, new ChatInteractionDataStruct(10054, "Are you... here for the game?", false, 1003, new int[]{-1}, 10055, new int[]{-1}, false)},
        {10055, new ChatInteractionDataStruct(10055, "Ye", false, 0, new int[]{10056,10057}, -1, new int[]{-1}, false)},
        {10056, new ChatInteractionDataStruct(10056, "Do we know each other before?", true, 0, new int[]{-1}, 10058, new int[]{-1}, false)},
        {10057, new ChatInteractionDataStruct(10057, "May I ask what's your name?", true, 0, new int[]{-1}, 10058, new int[]{-1}, false)},
        {10058, new ChatInteractionDataStruct(10058, "It doesn't matter; I'm just an old and outdated audience", false, 1003, new int[]{-1}, 10059, new int[]{-1}, false)},
        {10059, new ChatInteractionDataStruct(10059, "It's pretty late... What are you up to?", false, 0, new int[]{-1}, 10060, new int[]{-1}, false)},
        {10060, new ChatInteractionDataStruct(10060, "Nothing but drinking", false, 1003, new int[]{-1}, 10061, new int[]{-1}, false)},
        {10061, new ChatInteractionDataStruct(10061, "I don't know they are selling that...", false, 0, new int[]{-1}, 10062, new int[]{-1}, false)},
        {10062, new ChatInteractionDataStruct(10062, "Only at night, secretly", false, 1003, new int[]{-1}, 10063, new int[]{-1}, false)},
        {10063, new ChatInteractionDataStruct(10063, "Sounds like you know here well", false, 0, new int[]{-1}, 10064, new int[]{-1}, false)},
        {10064, new ChatInteractionDataStruct(10064, "Yeah, I'm here every once a while", false, 1003, new int[]{-1}, 10065, new int[]{-1}, false)},
        {10065, new ChatInteractionDataStruct(10065, "Just for drinking?", false, 0, new int[]{-1}, 10066, new int[]{-1}, false)},
        {10066, new ChatInteractionDataStruct(10066, "Looking for my sister...", false, 1003, new int[]{-1}, 10067, new int[]{-1}, false)},
        {10067, new ChatInteractionDataStruct(10067, "Is she here?", false, 0, new int[]{-1}, 10068, new int[]{1018}, false)},
        {10068, new ChatInteractionDataStruct(10068, "...", false, 1003, new int[]{-1}, -1, new int[]{-1}, true)},
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
