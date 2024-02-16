using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of PlayerSkills
/// </summary>
public class PlayerSkillBase : MonoBehaviour
{
    // Data
    [SerializeField] protected float SkillDamage = 1f;
    [SerializeField] protected string childProjectileName = "ClevProjParent";
    [SerializeField] protected int projectileID = 0;

    // Public:
    public virtual void SkillBegin()
    {
        
    }
}
