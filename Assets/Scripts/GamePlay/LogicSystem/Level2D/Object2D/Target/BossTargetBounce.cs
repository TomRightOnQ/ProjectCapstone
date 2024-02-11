using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss bullets that can be bounce back
/// </summary>
public class BossTargetBounce : Target
{
    // if the target is already bounced back
    // in this case, do not take any further damage
    [SerializeField, ReadOnly] private bool bBounced = false;

    public override void SetUp()
    {
        base.SetUp();
        bBounced = false;
        targetRigidBody = GetComponent<Rigidbody>();
    }

    // Override the take damage
    public override void TakeDamage(float damage, Vector3 theOtherPosition)
    {
        if (bBounced || targetRigidBody == null)
        {
            return;
        }
        bBounced = true;

        instanceMaterial.SetFloat("_Hitted", 0.25f);

        // No gravity for bounced bullets
        targetRigidBody.useGravity = false;

        Vector3 difference = (transform.position - theOtherPosition).normalized;
        targetRigidBody.velocity = targetRigidBody.velocity.magnitude * difference;
    }

    // Override to be able to damage the boss
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain")
        {
            targetRigidBody.useGravity = true;
            bExploded = true;
            if (explodeVFXName != "None")
            {
                GameEffectManager.Instance.PlayVFX(explodeVFXName,
                    new Vector3(transform.position.x, transform.position.y, 0),
                    transform.localScale);
            }
            deactivate();
        }
        else if (other.tag == "Player")
        {
            targetRigidBody.useGravity = true;
            bExploded = true;
            if (explodeVFXName != "None")
            {
                GameEffectManager.Instance.PlayVFX(explodeVFXName,
                    new Vector3(transform.position.x, transform.position.y, 0),
                    transform.localScale);
            }
            deactivate();
            // Damage the player
            PersistentDataManager.Instance.MainPlayer.TakeDamage(damageToPlayer);
        }
        else if (bBounced && other.tag == "Boss") 
        {
            targetRigidBody.useGravity = true;
            bExploded = true;
            if (explodeVFXName != "None")
            {
                GameEffectManager.Instance.PlayVFX(explodeVFXName,
                    new Vector3(transform.position.x, transform.position.y, 0),
                    transform.localScale);
            }
            // Damage the boss
            EUnit boss = other.GetComponent<EUnit>();
            if (boss != null)
            {
                boss.TakeDamage(damageToPlayer);
                PersistentDataManager.Instance.MainCamera.ShakeCamera(0.1f, 0.01f);
            }
            deactivate();
        }
    }
}
