using UnityEngine;

/// <summary>
/// Player Object
/// </summary>
public class Player : EUnit
{
    [SerializeField] private PlayerController playerController;
    public PlayerController Controller => playerController;

    [SerializeField] private PlayerHUDInteractionTrigger playerInteractionTrigger;
    public PlayerHUDInteractionTrigger MainPlayerInteractionTrigger => playerInteractionTrigger;

    // Weapon Group
    [SerializeField] private int projectileID = 1;
    [SerializeField] private float rateOfFire = 1f;
    [SerializeField] private bool bFireCDReady = false;
    private float fireCD = 0f;

    // Damage and Heal Countdown
    [SerializeField, ReadOnly]
    private float damageCD = 0f;
    [SerializeField] private bool bCanTakeDamage = false;
    [SerializeField, ReadOnly]
    private float healCD = 0f;
    [SerializeField] private bool bCanTakeHeal = false;


    [SerializeField, ReadOnly]
    private string projectileName;

    public int ProjectileID => projectileID;
    public float RateOfFire => rateOfFire;

    protected override void Awake()
    {
        base.Awake();
        // Check Controller's existence
        PlayerController playerController = this.gameObject.GetComponent<PlayerController>();
        if (playerController == null) 
        {
            this.gameObject.AddComponent<PlayerController>();
        }
        if (projectileID > 0)
        {
            projectileName = ProjectileData.GetData(projectileID).Name;
        }
    }

    // Private:
    private void Update()
    {
        if (!bFireCDReady) 
        {
            fireCD += Time.deltaTime;
        }
        if (fireCD >= rateOfFire)
        {
            fireCD = 0;
            bFireCDReady = true;
        }
        // Update damage taking CD
        if (!bCanTakeDamage)
        {
            damageCD += Time.deltaTime;
        }
        if (damageCD >= Constants.PLAYER_DAMAGE_CD)
        {
            damageCD = 0;
            bCanTakeDamage = true;
        }
        // Update heal taking CD
        if (!bCanTakeHeal)
        {
            healCD += Time.deltaTime;
        }
        if (healCD >= Constants.PLAYER_DAMAGE_CD)
        {
            healCD = 0;
            bCanTakeHeal = true;
        }
    }

    // Public:
    // Take Damage
    public override void TakeDamage(float damage = 0, bool bForceDamage = false, bool bPercentDamage = false, bool bRealDamage = false)
    {
        // Do not take damage if the CD is not ready
        if (!bCanTakeDamage)
        {
            return;
        }
        bCanTakeDamage = false;

        // Notify the main camera to shake
        PersistentDataManager.Instance.MainCamera.ShakeCamera(0.1f, 0.1f);
        // Play HUD Effect
        HUDManager.Instance.PlayPlayerDamagedScreenEffect();
        base.TakeDamage(damage, bForceDamage, bPercentDamage, bRealDamage);

        // Play Effects
        if (damageParticle != null)
        {
            damageParticle.Play();
        }

        checkCurrentHealth();
    }

    // Take Heal
    public override void TakeHeal(float heal)
    {
        // Do not take heal if the CD is not ready
        if (!bCanTakeHeal)
        {
            return;
        }
        bCanTakeHeal = false;
        base.TakeHeal(heal);

        // Play Effects
        if (healParticle != null)
        {
            healParticle.Play();
        }
    }

    // Fire
    public void FireProjectile(Vector3 fireDirection, Transform targetTransform)
    {
        if (!bFireCDReady)
        {
            return;
        }
        bFireCDReady = false;

        // Spawn Projectile
        GameObject projObj = PrefabManager.Instance.Instantiate(projectileName, gameObject.transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetUp(projectileID);
            projectile.Launch(fireDirection, targetTransform);
        }
        else
        {
            Debug.LogWarning("Player: Unable to find projectile component");
        }
    }

    // Terminate the player
    // Since the player is not poolable yet not destroyable, we will not inherit the base method
    public override void DeactivateEntity()
    {
        this.gameObject.SetActive(false);
        playerController.ResetPlayerController();
    }

    // Update HP to UIs
    protected override void checkCurrentHealth()
    {
        base.checkCurrentHealth();
        BattleObserver.Instance.OnPlayerHPChanged(currentHP, maxHP);
    }
}