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

        targetRigidBody.useGravity = false;
        Vector3 incomingVec = targetRigidBody.velocity;
        Vector3 normalVec = (transform.position - theOtherPosition).normalized;

        Vector3 reflectVec = Vector3.Reflect(incomingVec, normalVec);
        float energyLossFactor = 0.75f;
        reflectVec = reflectVec.normalized * incomingVec.magnitude * energyLossFactor;

        float randomness = 0.1f;
        Vector3 randomVec = new Vector3(Random.Range(-randomness, randomness), Random.Range(-randomness, randomness), Random.Range(-randomness, randomness));
        reflectVec += randomVec;

        targetRigidBody.velocity = reflectVec;
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
        else if (bBounced && other.tag == "Shield")
        {
            targetRigidBody.useGravity = true;
            bExploded = true;
            if (explodeVFXName != "None")
            {
                GameEffectManager.Instance.PlayVFX(explodeVFXName,
                    new Vector3(transform.position.x, transform.position.y, 0),
                    transform.localScale);
            }
            // Damage the shield
            Boss_1 boss = FindFirstObjectByType<Boss_1>();
            if (boss != null)
            {
                boss.DisableShield();
            }
            deactivate();
        }
    }
}
