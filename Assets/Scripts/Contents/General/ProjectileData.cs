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
        public float ProjSparsing;
        public bool bGuided;
        public bool bLaser;
        public bool bProxFuse;
        public bool bAOE;
        public float ProjProxRange;
        public float LockAngle;

        public ProjectileDataStruct(int ID, string Name, float ProjSpeed, float ProjLife, float ProjDamage, float ProjDamageRange, float ProjSparsing, bool bGuided, bool bLaser, bool bProxFuse, bool bAOE, float ProjProxRange, float LockAngle)
        {
            this.ID = ID;
            this.Name = Name;
            this.ProjSpeed = ProjSpeed;
            this.ProjLife = ProjLife;
            this.ProjDamage = ProjDamage;
            this.ProjDamageRange = ProjDamageRange;
            this.ProjSparsing = ProjSparsing;
            this.bGuided = bGuided;
            this.bLaser = bLaser;
            this.bProxFuse = bProxFuse;
            this.bAOE = bAOE;
            this.ProjProxRange = ProjProxRange;
            this.LockAngle = LockAngle;
        }
    }
    public static Dictionary<int, ProjectileDataStruct> data = new Dictionary<int, ProjectileDataStruct>
    {
        {1, new ProjectileDataStruct(1, "BulletSmall", 14f, 5f, 1f, 0.75f, 0f, false, false, false, true, -1f, 0f)},
        {2, new ProjectileDataStruct(2, "BulletLeaf", 50f, 0.5f, 0.25f, 0.25f, 0.1f, false, false, false, false, -1f, 0f)},
        {3, new ProjectileDataStruct(3, "LaserSmall", 0f, 0.5f, 2f, 0f, 0f, false, true, false, false, -1f, 0f)},
        {4, new ProjectileDataStruct(4, "MissileSmall", 24f, 2f, 2f, 1f, 0.05f, true, false, true, true, 0.75f, 60f)},
        {5, new ProjectileDataStruct(5, "BulletVT", 32f, 1f, 0.5f, 2f, 0.05f, false, false, true, true, 2f, 0f)},
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
