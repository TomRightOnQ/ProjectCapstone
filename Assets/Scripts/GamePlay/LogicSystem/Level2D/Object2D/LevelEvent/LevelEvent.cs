using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Indicate am event triggered by event or auto
/// CANNOT POST EVENT
/// </summary>
public class LevelEvent : MonoBehaviour
{
    // Begin Condition - On Awake by default
    [SerializeField] protected GameEvent.Event triggerCondition = GameEvent.Event.NONE;

    // Timer - in secs
    [SerializeField] private float targetTime = -1f;
    // Current time for debug purpose
    [SerializeField, ReadOnly] private float timeElasped = 0f;

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
            // Wait
            yield return new WaitForSeconds(0.1f);
            // Decrement the target time
            targetTime -= 0.1f;
            timeElasped += 0.1f;
        }
        levelEvent();
    }

    // Methods to run
    protected virtual void levelEvent()
    {
    
    }
}
