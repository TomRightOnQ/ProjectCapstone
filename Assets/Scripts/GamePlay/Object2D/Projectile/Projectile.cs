using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Projectiles for 2D scenes
/// </summary>
public class Projectile : MEntity
{
    // Proj ID
    [SerializeField, ReadOnly]
    protected int projectileID = -1;

    // State flags
    [SerializeField, ReadOnly] protected bool bExploded = true;

    // Effects
    [SerializeField] protected string explodeVFXName = "None";

    // RigidBody
    [SerializeField] protected Rigidbody projRigidBody;

    // Projectile Stats
    [SerializeField] protected float projSpeed = 1f;
    [SerializeField] protected float projLife = 1f;
    [SerializeField] protected float projDamage = 1f;
    [SerializeField] protected float projDamageRange = 1f;
    [SerializeField] protected float projSparsing = 1f;

    [SerializeField] protected bool bPlayerProj = true;
    [SerializeField] protected bool bGuided = false;
    [SerializeField] protected bool bLaser = false;
    [SerializeField] protected bool bAOE = true;

    // Special Group 
    // VT
    [SerializeField] protected bool bProxFuse = false;
    [SerializeField] protected float projProxRange = 1f;
    protected LayerMask proxTargetLayerMask;

    // Missile:
    [SerializeField] protected float lockAngle = 60f;

    // Public:
    // SetUp the Projectile
    public void SetUp(int projID = 1, bool bPlayer = false)
    {
        if (projRigidBody == null)
        {
            projRigidBody = GetComponent<Rigidbody>();
        }

        proxTargetLayerMask = 1 << LayerMask.NameToLayer("Target");
        // Load and setup
        ProjectileData.ProjectileDataStruct projectileData = ProjectileData.GetData(projID);
        if (projectileData != null)
        {
            projectileID = projID;
            projSpeed = projectileData.ProjSpeed;
            projLife = projectileData.ProjLife;
            projDamage = projectileData.ProjDamage;
            projDamageRange = projectileData.ProjDamageRange;
            projSparsing = projectileData.ProjSparsing;

            bPlayerProj = bPlayer;
            bGuided = projectileData.bGuided;
            bLaser = projectileData.bLaser;
            bAOE = projectileData.bAOE;

            bProxFuse = projectileData.bProxFuse;
            projProxRange = projectileData.ProjProxRange;
            lockAngle = projectileData.LockAngle;

            bExploded = false;
        }
        else 
        {
            Debug.LogWarning(string.Format("Projectile/ProjectileData: Unable to find projectile data with id {0}", projectileID));
        }
    }

    // Launch the Projectile
    public virtual void Launch(Vector3 launchDirection, Transform targetTransform)
    {
        if (projRigidBody == null)
        {
            Debug.LogWarning("Projectile: Unable to locate the Projectile RigidBody");
            return;
        }

        projRigidBody.velocity = Vector3.zero;

        Vector3 randomizedDirection = new Vector3(
            launchDirection.x + UnityEngine.Random.Range(-projSparsing, projSparsing),
            launchDirection.y + UnityEngine.Random.Range(-projSparsing, projSparsing),
            launchDirection.z
        ).normalized;

        transform.up = randomizedDirection;
        projRigidBody.velocity = transform.up * projSpeed;

        Invoke("explode", projLife);
        gameObject.SetActive(true);
    }

    // Private:
    protected virtual void FixedUpdate()
    {
        if (bProxFuse)
        {
            checkProximity();
        }
    }

    protected void checkProximity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, projProxRange, proxTargetLayerMask);
        if (hitColliders.Length > 0)
        {
            explode();
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            explode();
            if (!bAOE)
            {
                other.GetComponent<Target>().TakeDamage(projDamage);
            }
        } else if (other.tag == "Terrain") 
        {
            explode();
        }
    }

    protected void explode()
    {
        CancelInvoke("explode");
        if (bExploded)
        {
            return;
        }

        bExploded = true;
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
                    affectedCollider.GetComponent<Target>().TakeDamage(projDamage);
                }
            }
        }

        deactivate();
    }

    protected void deactivate()
    {
        projRigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
