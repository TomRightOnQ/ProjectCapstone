using System.Collections.Generic;

public static class TestData
{
    public class TestDataStruct
    {
        public int ID;
        public string Name;
        public int Type;
        public float HP;
        public int[] BUFF;

        public TestDataStruct(int ID, string Name, int Type, float HP, int[] BUFF)
        {
            this.ID = ID;
            this.Name = Name;
            this.Type = Type;
            this.HP = HP;
            this.BUFF = BUFF;
        }
    }
    public static Dictionary<int, TestDataStruct> data = new Dictionary<int, TestDataStruct>
    {
        {1001, new TestDataStruct(1001, "test1", 1, 5.5f, new int[]{1,2,3,4,5})},
    };

    public static TestDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out TestDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
