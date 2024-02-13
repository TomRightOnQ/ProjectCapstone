using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A spotlight that looks at a given object
/// </summary>
public class TrackingSpotLight : MEntity
{
    [SerializeField] private GameObject target;
    [SerializeField] private bool bTrackingPlayer = true;
    [SerializeField] private bool bReady = false;

    // Move with target or look at target
    [SerializeField] private bool bLookAt = true;

    // Animator
    [SerializeField] private Animator animator;

    protected override void Awake()
    {
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_C_1, beginLight);
    }

    private void Update()
    {
        if (target != null && bReady)
        {
            if (bLookAt)
            {
                transform.LookAt(target.transform);
            }
            else 
            {
                transform.position = new Vector3(
                    target.transform.position.x,
                    target.transform.position.y,
                    transform.position.z);

                transform.LookAt(target.transform);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_C_1, beginLight);
    }

    private void beginLight()
    {
        animator.Play("On");
        if (bTrackingPlayer)
        {
            target = PersistentDataManager.Instance.MainPlayer.gameObject;
        }
        transform.LookAt(target.transform);
        bReady = true;
    }
}
