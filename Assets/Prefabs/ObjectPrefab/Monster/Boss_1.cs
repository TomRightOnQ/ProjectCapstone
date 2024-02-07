using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monster Group - Boss 1
/// </summary>
public class Boss_1 : EUnit
{
    // Shield Object
    [SerializeField] private GameObject bossShield;

    protected override void Awake()
    {
        base.Awake();
        // Subscribe to Special Event C1 as the event to spawn
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_C_1, bossBegin);
    }

    // Begin to work
    private void bossBegin()
    {
        // Lock Player Input
        InputManager.Instance.LockInput();
        // Animation and await
        StartCoroutine(bossSpawning());
    }

    private IEnumerator bossSpawning()
    {
        PersistentDataManager.Instance.MainCamera.ShakeCamera(1.75f, 0.5f);
        yield return new WaitForSeconds(3f);
        // Spawn the Shield
        ReminderManager.Instance.ShowSubtitleReminder(9);
        bossShield.SetActive(true);

        // Unlock Input
        InputManager.Instance.UnLockInput(Enums.SCENE_TYPE.Battle);
        InputManager.Instance.SetInputAsShooter();

        yield return new WaitForSeconds(2.1f);
        ReminderManager.Instance.ShowSubtitleReminder(10);
        yield return new WaitForSeconds(2.1f);
        ReminderManager.Instance.ShowSubtitleReminder(11);
    }
}
