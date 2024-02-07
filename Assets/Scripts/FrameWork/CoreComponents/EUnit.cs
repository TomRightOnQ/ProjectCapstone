using UnityEngine;
using Spine.Unity;

/// <summary>
/// Class for all "living things"
/// </summary>
public class EUnit : MEntity
{
    // Data Section
    // Facing
    protected bool facingRight = true;
    protected bool bCanChangeFace = false;

    // If unit is inmmune to damage - notive that this unit is still considered in damage system in this case
    protected bool bInvulnerable = false;

    [SerializeField] protected Rigidbody unitRigidBody;

    // Temp Data
    [SerializeField] protected float maxHP = 0f;
    [SerializeField] protected float currentHP = 0f;
    [SerializeField] protected float defense = 0f;
    [SerializeField] protected float attack = 0f;
    [SerializeField] protected float speedBase = 1f;
    [SerializeField] protected float speedModifier = 1f;
    [SerializeField] protected float speedAccel = 1f;

    // Particle
    [SerializeField] protected ParticleSystem damageParticle;
    [SerializeField] protected ParticleSystem healParticle;

    // Animation
    protected SkeletonAnimation skeletonAnimation;
    private Spine.Skeleton skeleton;

    protected override void Awake()
    {
        base.Awake();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        if (skeletonAnimation != null)
        {
            skeleton = skeletonAnimation.Skeleton;
            bCanChangeFace = true;
        }
    }

    // Public:
    // Setup the unit
    public virtual void SetUpUnit()
    {
        // Use a UnitData or similar struct to load the data
        // To be determined
    }

    // Change the direction of sprite
    public void ChangeFacing(bool bLeft = true)
    {
        if (!bCanChangeFace)
        {
            return;
        }
        if (bLeft)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else 
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        facingRight = !bLeft;
    }

    // Get facing
    public bool GetFacingToRight()
    {
        return facingRight;
    }

    // Return velocity magnitude
    public virtual float GetUnitVelocity()
    {
        if (unitRigidBody == null)
        {
            return 0f;
        }
        return unitRigidBody.velocity.magnitude;
    }

    // Take Damage Method
    // Do NOT use negative damage for healing
    public virtual void TakeDamage(float damage = 0f, bool bForceDamage = false, bool bPercentDamage = false, bool bRealDamage = false)
    {
        float finalDamage = -1;
        if (!bDamagable && (!bInvulnerable || bForceDamage))
        {
            if (damage == 0f)
            {
                finalDamage = 0;
            }
            else if (bPercentDamage)
            {
                damage = Mathf.Clamp(damage, 0f, 1f);
                finalDamage = damage * maxHP;
            }
            else
            {
                finalDamage = damage;
            }
        }
        if (finalDamage >= 0)
        {
            if (bRealDamage)
            {
                currentHP -= finalDamage;
            }
            else
            {
                currentHP -= finalDamage;
            }
            // Show damage number
            // ...
        }
        // Determine current health
        checkCurrentHealth();
    }

    public virtual void TakeHeal(float heal)
    {
        if (currentHP <= 0)
        {
            return;
        }
        currentHP = Mathf.Clamp(currentHP, maxHP, currentHP + heal);
    }

    // Kill the unit instantly -> Reduce HP to 0 or force kill
    public virtual void KillUnit(bool bForce = false)
    {
        if (bForce)
        {

        }
        else
        {
            currentHP = 0f;
            checkCurrentHealth();
        }
    }

    // Revive a unit
    public virtual void ReviveUnit()
    {

    }

    // Get unit speed
    public virtual float GetUnitSpeed()
    {
        return speedBase * speedModifier;
    }

    public virtual float GetUnitAccel()
    {
        return speedAccel;
    }

    // Protected and private:

    // Determmine currnent HP status
    protected virtual void checkCurrentHealth()
    {
        // Check if current HP will lead to something
        if (currentHP <= 0)
        {
            // Post Event of player death
        }
    }
}
