using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;

/// <summary>
/// Monster Group - Boss 2
/// </summary>
public class Boss_2 : Boss
{
    // Shield Object
    [SerializeField] private GameObject bossShield;

    // Tree
    [SerializeField] private BehaviorDesigner.Runtime.BehaviorTree aiTree;

    protected override void Awake()
    {
        base.Awake();
        // Subscribe to Special Event C1 as the event to spawn
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_C_1, bossBegin);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_C_1, bossBegin);
    }

    private void bossBegin()
    {
        // Lock Player Input
        InputManager.Instance.LockInput();
        // Animation and await
        StartCoroutine(bossSpawning());
    }

    private IEnumerator bossSpawning()
    {
        if (GameManager2D.Instance != null)
        {
            GameManager2D.Instance.ShowBossInfo();
        }
        bossStage = 1;
        PersistentDataManager.Instance.MainPlayer.EntitySay("!!", 1f);
        yield return new WaitForSeconds(1f);

        // Set Camera
        PersistentDataManager.Instance.MainCamera.SetCameraSecondReference(gameObject);

        // Unlock Input
        InputManager.Instance.UnLockInput(Enums.SCENE_TYPE.Battle);
        InputManager.Instance.SetInputAsShooter();
        bCanTakeDamage = true;
        // Post custom event to activate weapon groups
        stage_1();
    }

    // Update HP to UIs
    protected override void checkCurrentHealth()
    {
        base.checkCurrentHealth();
        float ratio = currentHP / maxHP;
        if (GameManager2D.Instance != null)
        {
            GameManager2D.Instance.UpdateBossHPBar(ratio);
        }
        // Switch changing
        if (bossStage == 1 && ratio < 0.75)
        {
            bossStage = 2;
            stage_2();
            ShooterLevelManager.Instance.AddScore(25);
        }
        if (bossStage == 2 && ratio < 0.5)
        {
            bossStage = 3;
            bossShield.SetActive(true);
            stage_3();
            ShooterLevelManager.Instance.AddScore(50);
        }
        if (bossStage == 3 && ratio < 0.25)
        {
            bossStage = 4;
            ShooterLevelManager.Instance.AddScore(75);
        }
        if (ratio <= 0)
        {
            ShooterLevelManager.Instance.AddScore(100);
            GameManager2D.Instance.EndGame(true, true);
        }
    }

    // Public:
    // Take Damage
    public override void TakeDamage(float damage = 0, bool bForceDamage = false, bool bPercentDamage = false, bool bRealDamage = false)
    {
        base.TakeDamage(damage, bForceDamage, bPercentDamage, bRealDamage);

        // Play Effects
        if (damageParticle != null)
        {
            damageParticle.Play();
        }
        GameEffectManager.Instance.PlaySound("DMG_1", transform.position);
        checkCurrentHealth();
    }

    // Private methods for weapon and behavior group
    private void stage_1()
    {
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_C_2);
    }

    private void stage_2()
    {
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_C_3);
    }

    private void stage_3()
    {
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_C_4);
    }
}
