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
    // Record the default gravitational feature
    [SerializeField, ReadOnly] public bool bGravity; 

    // Effects
    [SerializeField] protected string explodeVFXName = "None";

    // RigidBody
    [SerializeField] protected Rigidbody targetRigidBody;

    // Target Stats
    [SerializeField] protected float targetSpeed = 1f;
    [SerializeField] protected float targetHP = 1f;
    [SerializeField] protected float targetCurrentHP = 1f;
    [SerializeField] protected float targetLife = 5f;
    [SerializeField] protected int targetPoints = 1;
    [SerializeField] protected float damageToPlayer = 1f;

    // Materials
    [SerializeField] protected Material defaultMaterial;
    [SerializeField, ReadOnly] protected Material instanceMaterial;

    // Mesh Renderer
    [SerializeField] protected MeshRenderer targetRenderer;

    public float TargetCurrentHP => targetCurrentHP;

    // Public:
    public virtual void SetUp()
    {
        targetCurrentHP = targetHP;
        if (targetRigidBody == null)
        {
            targetRigidBody = GetComponent<Rigidbody>();
        }
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<MeshRenderer>();
        }
        if (instanceMaterial == null)
        {
            instanceMaterial = new Material(defaultMaterial);
        }
        targetRenderer.material = instanceMaterial;
        revertMaterial();
        targetRigidBody.velocity = Vector3.zero;
        bGravity = targetRigidBody.useGravity;
    }

    // Launch the Target
    public virtual void Launch(Vector3 luanchDirection, float force = 1f)
    {
        transform.up = luanchDirection;
        targetRigidBody.velocity = Vector3.zero;
        if (targetRigidBody == null)
        {
            Debug.LogWarning("Projectile: Unable to locate the Projectile RigidBody");
        }
        gameObject.SetActive(true);

        targetRigidBody.velocity = transform.up * targetSpeed * force;
        bExploded = false;
    }

    public virtual void TakeDamage(float damage, Vector3 theOtherPosition)
    {
        targetCurrentHP -= damage;
        instanceMaterial.SetFloat("_Hitted", 0);
        if (targetCurrentHP <= 0)
        {
            explode();
            return;
        }
        Invoke("revertMaterial", 0.1f);
    }

    // Private:

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain")
        {
            bExploded = true;
            if (explodeVFXName != "None")
            {
                GameEffectManager.Instance.PlayVFX(explodeVFXName,
                    new Vector3(transform.position.x, transform.position.y, 0),
                    transform.localScale);
            }
            deactivate();
        } else if (other.tag == "Player")
        {
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
    }

    protected void revertMaterial()
    {
        instanceMaterial.SetFloat("_Hitted", 1);
    }

    protected void explode()
    {
        if (bExploded)
        {
            return;
        }
        bExploded = true;

        if (explodeVFXName != "None")
        {
            GameEffectManager.Instance.PlayVFX(explodeVFXName,
                new Vector3(transform.position.x, transform.position.y, 0),
                transform.localScale);
        }

        ShooterLevelManager.Instance.AddScore(targetPoints);

        deactivate();
    }

    protected void deactivate()
    {
        CancelInvoke("revertMaterial");
        revertMaterial();
        targetRigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
