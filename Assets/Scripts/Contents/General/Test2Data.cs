using System.Collections.Generic;

public static class Test2Data
{
    public class Test2DataStruct
    {
        public int ID;
        public string Name;
        public int Type;
        public float HP;
        public int[] BUFF;

        public Test2DataStruct(int id, string name, int type, float hp, int[] buff)
        {
            this.ID = id;
            this.Name = name;
            this.Type = type;
            this.HP = hp;
            this.BUFF = buff;
        }
    }
    public static Dictionary<int, Test2DataStruct> data = new Dictionary<int, Test2DataStruct>
    {
        {1001, new Test2DataStruct(1001, "test1", 1, 5.5f, new int[]{1,2,3,4,5})},
    };

    public static Test2DataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out Test2DataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
