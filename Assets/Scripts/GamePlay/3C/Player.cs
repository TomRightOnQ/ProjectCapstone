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
    }

    // Public:
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