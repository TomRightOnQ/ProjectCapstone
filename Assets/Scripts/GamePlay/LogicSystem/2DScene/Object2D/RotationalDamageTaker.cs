using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotatonal Object that damage or heal the player
/// Default set to rotational damage begin Event
/// Default stop by rotational end event
/// Defualt switch by dmg and heal event
/// </summary>
public class RotationalDamageTaker : MonoBehaviour
{
    // Rotating Part
    [SerializeField] private GameObject rotatingPart;

    // Trigger Event
    [SerializeField] private GameEvent.Event triggerEvent = GameEvent.Event.EVENT_2DGAME_ROTDMG_ON;
    // Stop Event
    [SerializeField] private GameEvent.Event endEvent = GameEvent.Event.EVENT_2DGAME_ROTDMG_OFF;
    // Switch Event
    [SerializeField] private GameEvent.Event damageEvent = GameEvent.Event.EVENT_2DGAME_ROTDMG_DMG;
    [SerializeField] private GameEvent.Event healEvent = GameEvent.Event.EVENT_2DGAME_ROTDMG_HEAL;

    // Switching
    [SerializeField] private bool bDamageMode = true;
    [SerializeField, ReadOnly] private bool bReady = false;

    [SerializeField] private bool bAutoSwitching = true;
    [SerializeField] private int minSwitchingTime = 7;
    [SerializeField] private int maxSwitchingTime = 10;

    // Size
    [SerializeField, ReadOnly] private float originalSize = 1f;
    [SerializeField] private float damageSize = 1f;
    [SerializeField] private float healSize = 1f;

    // Animator
    [SerializeField] private Animator animator;

    // Material
    [SerializeField] private Material material;

    // Damage Rate
    [SerializeField] private float damageRate = 1f;
    [SerializeField] private float healRate = 1f;

    private void Awake()
    {
        originalSize = transform.localScale.x;

        // Subscribe to events
        EventManager.Instance.AddListener(triggerEvent, damageBegin);
        EventManager.Instance.AddListener(endEvent, damageEnd);
        EventManager.Instance.AddListener(damageEvent, switchToDamage);
        EventManager.Instance.AddListener(healEvent, switchToHeal);
        // Reset Color
        material.SetFloat("_bDamage", -2);
    }

    // Remove Listener
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(triggerEvent, damageBegin);
        EventManager.Instance.RemoveListener(endEvent, damageEnd);
        EventManager.Instance.RemoveListener(damageEvent, switchToDamage);
        EventManager.Instance.RemoveListener(healEvent, switchToHeal);
    }

    // Private:
    // Activate and Deactivate
    private void damageBegin()
    {
        rotatingPart.SetActive(true);
        transform.localScale = new Vector3(originalSize, transform.localScale.y, transform.localScale.z);
        // Blink for three seconds
        StartCoroutine(damagePreparation());
    }

    private IEnumerator damagePreparation()
    {
        // SetUp
        animator.Play("DamageTakerBlink");
        ReminderManager.Instance.ShowSubtitleReminder(5);
        transform.localScale = new Vector3(originalSize * damageSize, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(3f);
        ReminderManager.Instance.ShowSubtitleReminder(6);
        animator.Play("StopBlink");
        animator.Play("DamageTakerRotate");

        bReady = true;
        // If AutoSwitching, launch the cycle
        if (bAutoSwitching)
        {
            StartCoroutine(autoSwitchingCycle());
        }
    }

    /// <summary>
    /// Cycle Loop
    /// </summary>
    private IEnumerator autoSwitchingCycle()
    {
        while (true)
        {
            // Find time of this cycle
            int waitTime = Random.Range(minSwitchingTime + 3, maxSwitchingTime + 3);
            yield return new WaitForSeconds(waitTime);
            // Now make the switch
            if (bDamageMode)
            {
                switchToHeal();
            }
            else
            {
                switchToDamage();
            }
        }
    }

    private void damageEnd()
    {
        StopAllCoroutines();
        animator.Play("StopRotate");
        rotatingPart.SetActive(false);
        bReady = false;
    }

    // Switch mode
    private void switchToDamage()
    {
        // Blink for three seconds
        animator.Play("DamageTakerBlink");
        StartCoroutine(switchMode(true));
    }
    private void switchToHeal()
    {
        // Blink for three seconds
        animator.Play("DamageTakerBlink");
        StartCoroutine(switchMode(false));
    }

    private IEnumerator switchMode(bool bDamage)
    {
        // No effect during this time
        bReady = false;

        // Change Size first
        if (bDamage)
        {
            ReminderManager.Instance.ShowSubtitleReminder(7);
            material.SetFloat("_bDamage", -2);
            transform.localScale = new Vector3(originalSize * damageSize, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            ReminderManager.Instance.ShowSubtitleReminder(8);
            material.SetFloat("_bDamage", 2);
            transform.localScale = new Vector3(originalSize * healSize, transform.localScale.y, transform.localScale.z);
        }

        yield return new WaitForSeconds(3f);
        animator.Play("StopBlink");
        bDamageMode = bDamage;
        bReady = true;
    }

    // Public:
    // Damage or Heal
    public void AffectPlayer()
    {
        if (!bReady)
        {
            return;
        }

        // Damage or Heal
        if (bDamageMode)
        {
            PersistentDataManager.Instance.MainPlayer.TakeDamage(damageRate);
            return;
        }
        PersistentDataManager.Instance.MainPlayer.TakeHeal(healRate);
    }
}
