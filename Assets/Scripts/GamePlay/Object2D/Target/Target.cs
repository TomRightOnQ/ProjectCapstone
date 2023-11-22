using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Target for the shooter gamemode
/// </summary>
public class Target : MEntity
{
    // Target ID
    [SerializeField, ReadOnly]
    protected int targetID = -1;

    // State flags
    [SerializeField, ReadOnly] protected bool bExploded = true;

    // RigidBody
    [SerializeField] protected Rigidbody targetRigidBody;

    // Target Stats
    [SerializeField] protected float targetSpeed = 1f;
    [SerializeField] protected float targetHP = 1f;
    [SerializeField] protected float targetCurrentHP = 1f;
    [SerializeField] protected float targetLife = 5f;
    [SerializeField] protected int targetPoints = 1;

    // Public:
    public void SetUp()
    {
        targetCurrentHP = targetHP;
    }

    // Launch the Target
    public virtual void Launch(Vector3 luanchDirection, float force = 1f)
    {
        targetRigidBody.velocity = Vector3.zero;
        if (targetRigidBody == null)
        {
            Debug.LogWarning("Projectile: Unable to locate the Projectile RigidBody");
        }
        gameObject.SetActive(true);

        targetRigidBody.velocity = luanchDirection * targetSpeed * force;
        bExploded = false;
    }

    public void TakeDamage(float damage)
    {
        targetCurrentHP -= damage;
        if (targetCurrentHP <= 0)
        {
            explode();
        }
    }

    // Private:

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain")
        {
            bExploded = true;
            deactivate();
        }
    }

    protected void explode()
    {
        if (bExploded)
        {
            return;
        }
        bExploded = true;

        ShooterLevelManager.Instance.AddScore(targetPoints);

        deactivate();
    }

    protected void deactivate()
    {
        targetRigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
