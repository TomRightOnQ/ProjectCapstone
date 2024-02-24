using System.Collections.Generic;

using UnityEngine;

public static class GameEndData
{
    public class GameEndDataStruct
    {
        public int ID;
        public string Name;
        public string Detail;
        public bool bTrueEnd;

        public GameEndDataStruct(int ID, string Name, string Detail, bool bTrueEnd)
        {
            this.ID = ID;
            this.Name = Name;
            this.Detail = Detail;
            this.bTrueEnd = bTrueEnd;
        }
    }
    public static Dictionary<int, GameEndDataStruct> data = new Dictionary<int, GameEndDataStruct>
    {
        {0, new GameEndDataStruct(0, "We are the Champions", "Everything ends here?", true)},
        {1, new GameEndDataStruct(1, "Better than Home", "Some of our buddies can only watch this at home, and being here is better than a streaming channel anyway.", false)},
        {2, new GameEndDataStruct(2, "Not the Last One", "Don't be sad, at least we are not the worst ones going home on the first day.", false)},
        {3, new GameEndDataStruct(3, "Close to the Summit", "We have prove that we have the great potentials to be the best, maybe next time we have more fortune.", false)},
        {4, new GameEndDataStruct(4, "Finalist is also the Champion", "If I'm standing on the stage as a finalist, then I'm sharing the joy from the winner, so I'm also the champion.", false)},
    };

    public static GameEndDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out GameEndDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
