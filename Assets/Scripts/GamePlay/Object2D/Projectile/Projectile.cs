using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // RigidBody
    [SerializeField] protected Rigidbody projRigidBody;

    // Projectile Stats
    [SerializeField] protected float projSpeed = 1f;
    [SerializeField] protected float projLife = 1f;
    [SerializeField] protected float projDamage = 1f;
    [SerializeField] protected float projDamageRange = 1f;

    [SerializeField] protected bool bPlayerProj = true;
    [SerializeField] protected bool bGuided = false;
    [SerializeField] protected bool bLaser = false;

    // Special Group 
    // VT
    [SerializeField] protected bool bProxFuse = false;
    [SerializeField] protected float projProxRange = 1f;
    [SerializeField] protected string[] proxTargetLayers;
    protected LayerMask proxTargetLayerMask;

    // Public:
    // SetUp the Projectile
    public void SetUp(int projID = 1, bool bPlayer = false)
    {
        if (bProxFuse)
        {
            foreach (var layerName in proxTargetLayers)
            {
                proxTargetLayerMask |= (1 << LayerMask.NameToLayer(layerName));
            }
        }
        // Load and setup
        ProjectileData.ProjectileDataStruct projectileData = ProjectileData.GetData(projID);
        if (projectileData != null)
        {
            projectileID = projID;
            projSpeed = projectileData.ProjSpeed;
            projLife = projectileData.ProjLife;
            projDamage = projectileData.ProjDamage;
            projDamageRange = projectileData.ProjDamageRange;

            bPlayerProj = bPlayer;
            bGuided = projectileData.bGuided;
            bLaser = projectileData.bLaser;

            bProxFuse = projectileData.bProxFuse;
            projProxRange = projectileData.ProjProxRange;

            bExploded = false;
        }
        else 
        {
            Debug.LogWarning(string.Format("Projectile/ProjectileData: Unable to find projectile data with id {0}", projectileID));
        }
    }

    // Launch the Projectile
    public void Launch(Vector3 luanchDirection)
    {
        projRigidBody.velocity = Vector3.zero;
        if (projRigidBody == null)
        {
            Debug.LogWarning("Projectile: Unable to locate the Projectile RigidBody");
        }
        Invoke("explode", projLife);
        gameObject.SetActive(true);

        projRigidBody.velocity = luanchDirection * projSpeed;
    }

    // Private:
    protected void FixedUpdate()
    {
        if (bProxFuse)
        {
            checkProximity();
            bProxFuse = false;
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
        if (other.tag == "Target" || other.tag == "Terrain")
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
        // Apply damage to all entities within the damage range
        Collider[] affectedColliders = Physics.OverlapSphere(transform.position, projDamageRange);
        foreach (var affectedCollider in affectedColliders)
        {
            if (affectedCollider.CompareTag("Target"))
            {
                affectedCollider.GetComponent<Target>().TakeDamage(projDamage);
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
