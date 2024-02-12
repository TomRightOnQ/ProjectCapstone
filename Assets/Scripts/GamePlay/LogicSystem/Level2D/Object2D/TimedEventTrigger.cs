using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pre-Placed COntroller to trigger an event after a while
/// /// Event default set to All Direction Move begin
/// </summary>
public class TimedEventTrigger : MonoBehaviour
{
    // Begin Condition - On Awake by default
    [SerializeField] private GameEvent.Event triggerCondition = GameEvent.Event.NONE;
    // Event Posted
    [SerializeField] private GameEvent.Event triggerEvent = GameEvent.Event.EVENT_2DGAME_ALLDIR_BEGIN;

    // Timer - in secs
    [SerializeField] private float targetTime = -1f;
    // Current time for debug purpose
    [SerializeField, ReadOnly] private float timeElasped = 0f;

    // Play additional effect
    [SerializeField] private bool bPlayAdditionalEffect = true;

    private void Awake()
    {
        // Start the timer if target is more than 0
        // And the triggerCondition is None
        if (triggerCondition == GameEvent.Event.NONE && targetTime > 0)
        {
            StartCoroutine(countDown());
            return;
        }
        EventManager.Instance.AddListener(triggerCondition, beginCountDownByEvent);
    }

    // Begin Count Down
    private void beginCountDownByEvent()
    {
        StartCoroutine(countDown());
    }

    private void OnDestroy()
    {
        if (triggerCondition != GameEvent.Event.NONE)
        {
            EventManager.Instance.RemoveListener(triggerCondition, beginCountDownByEvent);
        }
    }

    // Count down
    private IEnumerator countDown()
    {
        while (targetTime > 0)
        {
            // Wait for one second
            yield return new WaitForSeconds(0.1f);
            // Decrement the target time
            targetTime -= 0.1f;
            timeElasped += 0.1f;
        }
        postEvent();
    }

    // Post Event
    private void postEvent()
    {
        switch (triggerEvent)
        {
            case GameEvent.Event.EVENT_2DGAME_ALLDIR_BEGIN:
                if (bPlayAdditionalEffect)
                {
                    PersistentDataManager.Instance.MainCamera.ShakeCamera(1.75f, 0.5f);
                    PersistentDataManager.Instance.MainPlayer.EntitySay("!!", 1f);
                }
                Invoke("allDirBegin", 2f);
                break;
            case GameEvent.Event.EVENT_2DGAME_ROTDMG_ON:
                EventManager.Instance.PostEvent(GameEvent.Event.EVENT_2DGAME_ROTDMG_ON);
                break;
            case GameEvent.Event.EVENT_2DGAME_ROTDMG_OFF:
                EventManager.Instance.PostEvent(GameEvent.Event.EVENT_2DGAME_ROTDMG_OFF);
                break;
            default:
                EventManager.Instance.PostEvent(triggerEvent);
                break;
        }
        Debug.Log("Event Triggered: " + triggerEvent.ToString());
    }

    // Effects
    private void allDirBegin()
    {
        InputManager.Instance.SetPlayerMovingAllDirection(true);
    }
}
