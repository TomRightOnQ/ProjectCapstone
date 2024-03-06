using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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

    // FMOD Components
    [SerializeField, ReadOnly] private string HIT_EVENT_NAME = "event:/Hits/Player Hit";
    private EventInstance playerHit;

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
