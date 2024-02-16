using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent Bullet of the player skill cluster projectile
/// </summary>
public class ClevProjParent : Projectile
{
    [SerializeField] private string childProjectileName = "ClevProjChild";
    [SerializeField] private int childProjectileID = 7;
    [SerializeField] private float startAngle = 110f;
    [SerializeField] private float endAngle = 200f;
    [SerializeField] private int childCount = 5;

    // Override version of the launch
    // Self-Det after a few seconds after launching
    public override void Launch(Vector3 launchDirection, Transform targetTransform)
    {
        base.Launch(launchDirection, targetTransform);
        StartCoroutine(flying());
    }

    // CountDown
    private IEnumerator flying()
    {
        yield return new WaitForSeconds(projLife);
        explode();
    }

    // Release SubProjectiles
    private void releaseChildProjectile()
    {
        float angleStep = (endAngle - startAngle) / childCount;
        float angle = startAngle;

        for (int i = 0; i < childCount; i++)
        {
            // Convert bearing angle to Unity angle
            float unityAngle = convertBearingToUnityAngle(angle);

            // Convert angle to direction vector
            Vector3 fireDirection = angleToDirectionVector(unityAngle);

            // Launch the child projectile
            launchChildProjectile(fireDirection);

            angle += angleStep;
        }
    }

    private float convertBearingToUnityAngle(float bearingAngle)
    {
        // Convert bearing angle to Unity's angle system
        return (450 - bearingAngle) % 360;
    }

    private Vector3 angleToDirectionVector(float angle)
    {
        // Convert angle (in degrees) to radians
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculate direction vector
        Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);

        return direction;
    }

    // Launch Single one
    private void launchChildProjectile(Vector3 fireDirection)
    {
        GameObject projObj = PrefabManager.Instance.Instantiate(childProjectileName, gameObject.transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (projRigidBody.velocity.x < 0)
            {
                fireDirection.x = -fireDirection.x;
            }

            projectile.SetUp(childProjectileID);
            projectile.Launch(fireDirection, null);
        }
        else
        {
            Debug.LogWarning("Player: Unable to find projectile component");
        }
    }

    protected override void explode()
    {
        StopAllCoroutines();
        CancelInvoke("explode");
        if (bExploded)
        {
            return;
        }

        bExploded = true;
        GameEffectManager.Instance.PlaySound(explodeSFXName, transform.position);
        if (explodeVFXName != "None")
        {
            GameEffectManager.Instance.PlayVFX(explodeVFXName,
                new Vector3(transform.position.x, transform.position.y, 0),
                new Vector3(projDamageRange, projDamageRange, projDamageRange));
        }

        // Apply damage to all entities within the damage range
        if (bAOE)
        {
            Collider[] affectedColliders = Physics.OverlapSphere(transform.position, projDamageRange);
            foreach (var affectedCollider in affectedColliders)
            {
                if (affectedCollider.CompareTag("Target"))
                {
                    affectedCollider.GetComponent<Target>().TakeDamage(projDamage, transform.position);
                }
                else if (affectedCollider.CompareTag("Boss"))
                {
                    affectedCollider.GetComponent<EUnit>().TakeDamage(projDamage);
                }
            }
        }
        releaseChildProjectile();
        deactivate();
    }
}
