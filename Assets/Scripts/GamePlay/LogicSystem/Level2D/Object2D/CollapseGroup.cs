using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects Group
/// Trigger a Collapse effect when recv an event
/// Event default set to All Direction Move begin
/// </summary>
public class CollapseGroup : MonoBehaviour
{
    // Event
    [SerializeField] private GameEvent.Event triggerEvent = GameEvent.Event.EVENT_2DGAME_ALLDIR_BEGIN;

    // Animation
    [SerializeField] private Animator collapseAnimator;

    private void Awake()
    {
        // Subscribe to the event
        EventManager.Instance.AddListener(triggerEvent, OnRecv_AllDirBegin);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(triggerEvent, OnRecv_AllDirBegin);
    }

    // Event Handlers
    private void OnRecv_AllDirBegin()
    {
        if (collapseAnimator != null)
        {
            collapseAnimator.Play("Fall");
        }
    }
}
