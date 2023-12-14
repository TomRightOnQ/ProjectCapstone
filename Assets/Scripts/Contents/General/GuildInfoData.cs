using System.Collections.Generic;

using UnityEngine;

public static class GuildInfoData
{
    public class GuildInfoDataStruct
    {
        public int ID;
        public string Name;

        public GuildInfoDataStruct(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }
    }
    public static Dictionary<int, GuildInfoDataStruct> data = new Dictionary<int, GuildInfoDataStruct>
    {
        {0, new GuildInfoDataStruct(0, "PlayerTeam")},
        {1, new GuildInfoDataStruct(1, "OtherTeamB")},
        {2, new GuildInfoDataStruct(2, "OtherTeamC")},
        {3, new GuildInfoDataStruct(3, "OtherTeamD")},
        {4, new GuildInfoDataStruct(4, "OtherTeamE")},
        {5, new GuildInfoDataStruct(5, "OtherTeamF")},
        {6, new GuildInfoDataStruct(6, "OtherTeamG")},
        {7, new GuildInfoDataStruct(7, "OtherTeamH")},
        {8, new GuildInfoDataStruct(8, "OtherTeamI")},
        {9, new GuildInfoDataStruct(9, "OtherTeamJ")},
        {10, new GuildInfoDataStruct(10, "OtherTeamK")},
        {11, new GuildInfoDataStruct(11, "OtherTeamL")},
        {12, new GuildInfoDataStruct(12, "OtherTeamM")},
        {13, new GuildInfoDataStruct(13, "OtherTeamN")},
        {14, new GuildInfoDataStruct(14, "OtherTeamO")},
        {15, new GuildInfoDataStruct(15, "OtherTeamP")},
    };

    public static GuildInfoDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out GuildInfoDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
