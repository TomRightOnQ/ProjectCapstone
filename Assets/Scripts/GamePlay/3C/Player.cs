using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // Skill Component
    [SerializeField] private PlayerSkillBase playerSkill;
    // SkillCD
    [SerializeField] private float skillCD = 1f;
    [SerializeField] private bool bSkillReady = true;

    // Position to move the background
    private Vector3 originalPosition;

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
            gameObject.AddComponent<PlayerController>();
        }
        if (projectileID > 0)
        {
            projectileName = ProjectileData.GetData(projectileID).Name;
        }
        originalPosition = transform.position;
        if (playerSkill == null)
        {
            playerSkill = GetComponent<PlayerSkillBase>();
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

    private void FixedUpdate()
    {
        // Calculate the total offset from the original position
        float totalOffsetX = transform.position.x - originalPosition.x;
        float totalOffsetY = transform.position.y - originalPosition.y;
        // Use this total offset for the parallax effect
        if (ParallaxScrollingBG.Instance != null)
        {
            ParallaxScrollingBG.Instance.MoveBackground(totalOffsetX, totalOffsetY);
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
        GameEffectManager.Instance.PlaySound("DMG_1", transform.position);
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
        checkCurrentHealth();
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

    // Use Skill
    public void UseSkill()
    {
        if (!bSkillReady)
        {
            return;
        }
        bSkillReady = false;

        if (playerSkill != null)
        {
            playerSkill.SkillBegin();
        }

        // Clear CD
        BattleObserver.Instance.OnPlayerSkillCDChanged(0, skillCD);
        // StartCD
        StartCoroutine(skillCoolDown());
    }

    private IEnumerator skillCoolDown()
    {
        float currentTime = 0;
        while (currentTime <= skillCD)
        {
            yield return new WaitForSeconds(0.1f);
            currentTime += 0.1f;
            BattleObserver.Instance.OnPlayerSkillCDChanged(currentTime, skillCD);
        }
        bSkillReady = true;
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