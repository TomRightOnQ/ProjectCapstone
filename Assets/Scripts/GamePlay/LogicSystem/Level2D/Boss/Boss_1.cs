using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monster Group - Boss 1
/// </summary>
public class Boss_1 : Boss
{
    // Shield Object
    [SerializeField] private GameObject bossShield;
    [SerializeField] private float noShieldLow = 1f;
    [SerializeField] private float noShieldHigh = 3f;
    [SerializeField] private GameObject damageZone;

    // Material Changing
    [SerializeField] private List<Material> materialList = new List<Material>();
    [SerializeField] private MeshRenderer mainRenderer;

    [SerializeField, ReadOnly] private bool bShieldDown = false;

    protected override void Awake()
    {
        base.Awake();
        // Subscribe to Special Event C1 as the event to spawn
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_C_1, bossBegin);
    }

    protected void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_C_1, bossBegin);
    }

    // Public:
    // Disable Shield
    public void DisableShield()
    {
        float disableTime = Random.Range(noShieldLow, noShieldHigh);
        bossShield.SetActive(false);
        bShieldDown = true;
        Invoke("ActiveShield", disableTime);
    }

    public void ActiveShield()
    {
        bossShield.SetActive(true);
        bShieldDown = false;
    }

    // Begin to work
    private void bossBegin()
    {
        // Lock Player Input
        InputManager.Instance.LockInput();
        // Animation and await
        StartCoroutine(bossSpawning());
        StartCoroutine(blinkingEffectCoroutine());
    }

    private IEnumerator bossSpawning()
    {
        PersistentDataManager.Instance.MainCamera.ShakeCamera(2.75f, 0.5f);
        bossAnimator.Play("BossSpawn");
        if (GameManager2D.Instance != null)
        {
            GameManager2D.Instance.ShowBossInfo();
        }
        bossStage = 1;
        PersistentDataManager.Instance.MainPlayer.EntitySay("!!", 1f);
        yield return new WaitForSeconds(5f);
        // Spawn the Shield
        ReminderManager.Instance.ShowSubtitleReminder(9);
        bossShield.SetActive(true);
        damageZone.SetActive(true);
        bossCollider.enabled = true;

        // Unlock Input
        InputManager.Instance.UnLockInput(Enums.SCENE_TYPE.Battle);
        InputManager.Instance.SetInputAsShooter();

        bCanTakeDamage = true;

        // Post custom event to activate weapon groups
        stage_1();

        yield return new WaitForSeconds(2.5f);
        ReminderManager.Instance.ShowSubtitleReminder(10);
        yield return new WaitForSeconds(2.5f);
        ReminderManager.Instance.ShowSubtitleReminder(11);
    }

    // If damaged with shield, remove it
    public override void TakeDamage(float damage = 0, bool bForceDamage = false, bool bPercentDamage = false, bool bRealDamage = false)
    {
        base.TakeDamage(damage, bForceDamage, bPercentDamage, bRealDamage);
        if (!bShieldDown)
        {
            DisableShield();
        }
    }

    // Blinking Effect
    private IEnumerator blinkingEffectCoroutine()
    {
        if (materialList.Count == 0 || mainRenderer == null)
        {
            yield break;
        }

        float effectDuration = Random.Range(0f, 1.5f);
        while (true)
        {
            Material randomMaterial = materialList[Random.Range(0, materialList.Count)];
            mainRenderer.material = randomMaterial;
            yield return new WaitForSeconds(effectDuration);
        }
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
        }
        if (bossStage == 2 && ratio < 0.5)
        {
            bossStage = 3;
            stage_3();
        }
        if (bossStage == 3 && ratio < 0.25)
        {
            bossStage = 4;
        }
        if (ratio <= 0)
        {
            ShooterLevelManager.Instance.AddScore(150);
            GameManager2D.Instance.EndGame(true, true);
        }
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
