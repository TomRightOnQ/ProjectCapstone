using System.Collections.Generic;

using UnityEngine;

public static class ProjectileData
{
    public class ProjectileDataStruct
    {
        public int ID;
        public string Name;
        public float ProjSpeed;
        public float ProjLife;
        public float ProjDamage;
        public float ProjDamageRange;
        public bool bGuided;
        public bool bLaser;
        public bool bProxFuse;
        public float ProjProxRange;

        public ProjectileDataStruct(int ID, string Name, float ProjSpeed, float ProjLife, float ProjDamage, float ProjDamageRange, bool bGuided, bool bLaser, bool bProxFuse, float ProjProxRange)
        {
            this.ID = ID;
            this.Name = Name;
            this.ProjSpeed = ProjSpeed;
            this.ProjLife = ProjLife;
            this.ProjDamage = ProjDamage;
            this.ProjDamageRange = ProjDamageRange;
            this.bGuided = bGuided;
            this.bLaser = bLaser;
            this.bProxFuse = bProxFuse;
            this.ProjProxRange = ProjProxRange;
        }
    }
    public static Dictionary<int, ProjectileDataStruct> data = new Dictionary<int, ProjectileDataStruct>
    {
        {1, new ProjectileDataStruct(1, "BulletSmall", 20f, 1f, 1f, 0.05f, false, false, false, -1f)},
    };

    public static ProjectileDataStruct GetData(int id)
    {
        if (data.TryGetValue(id, out ProjectileDataStruct result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}
