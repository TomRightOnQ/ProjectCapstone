using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skill Three
/// </summary>
public class Skill_3 : PlayerSkillBase
{
    public override void SkillBegin()
    {
        // Spawn the Laser and launch
        GameObject projObj = PrefabManager.Instance.Instantiate(childProjectileName, transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetUp(projectileID);
            projectile.Launch(Vector3.zero, null);
        }
        else
        {
            Debug.LogWarning("Player: Unable to find projectile component");
        }
    }
}
