using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for bosses
/// </summary>
public class Boss : EUnit
{
    // Boss Collider
    [SerializeField] protected Collider bossCollider;

    // Boss Animator
    [SerializeField] protected Animator bossAnimator;

    // Boss Stage
    [SerializeField, ReadOnly]
    protected int bossStage = 1;

    // Reward per-Hit
    [SerializeField] protected int pointPerHit = 1;

    // Invulnerable
    [SerializeField] protected bool bCanTakeDamage = false;

    public override void TakeDamage(float damage = 0, bool bForceDamage = false, bool bPercentDamage = false, bool bRealDamage = false)
    {
        if (!bCanTakeDamage)
        {
            return;
        }
        base.TakeDamage(damage, bForceDamage, bPercentDamage, bRealDamage);
        ShooterLevelManager.Instance.AddScore((int)(pointPerHit * damage));
    }
}
