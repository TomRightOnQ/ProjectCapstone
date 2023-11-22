using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dummy Object launching targets
/// </summary>
public class TargetLauncher : MEntity
{
    // Target name
    [SerializeField] private string targetName;

    // Launching Time
    [SerializeField] private float minLaunchTime = 1f;
    [SerializeField] private float maxLaunchTime = 1f;

    // Launching Force
    [SerializeField] private float minLaunchForce = 1f;
    [SerializeField] private float maxLaunchForce = 1f;

    // Flags
    [SerializeField, ReadOnly]
    private bool bActivated = false;
    private Coroutine launchRoutine;

    protected override void Awake()
    {
        base.Awake();
        // Subscribe the events of shooter scene begin and end
        EventManager.Instance.AddListener(GameEvent.Event.SHOOTER_LEVEL_BEGIN, OnRecv_ShooterLevelBegin);
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_2DGAME_END, OnRecv_ShooterLevelEnd);
    }

    // Private:
    protected IEnumerator LaunchTargetsRoutine()
    {
        while (true)
        {
            if (this == null)
            {
                yield break;
            }

            float waitTime = Random.Range(minLaunchTime, maxLaunchTime);
            yield return new WaitForSeconds(waitTime);
            launchTarget();
        }
    }

    // Launch Target
    protected virtual void launchTarget()
    {
        // Spawn Target
        GameObject targetObj = PrefabManager.Instance.Instantiate(targetName, gameObject.transform.position, Quaternion.identity);
        Target target = targetObj.GetComponent<Target>();
        // Calculate Launch force muliplier
        float launchForce = Random.Range(minLaunchForce, maxLaunchForce);

        if (target != null)
        {
            target.SetUp();
            target.Launch(transform.up, launchForce);
        }
        else
        {
            Debug.LogWarning("Player: Unable to find projectile component");
        }
    }

    // Activate the launcher
    protected void activateLauncher()
    {
        bActivated = true;
        if (launchRoutine == null)
        {
            launchRoutine = StartCoroutine(LaunchTargetsRoutine());
        }
    }
    // Stop the launcer
    protected void deactivateLauncher()
    {
        if (!bActivated)
        {
            return;
        }
        bActivated = false;
        if (launchRoutine != null)
        {
            StopCoroutine(launchRoutine);
            launchRoutine = null;
        }
        EventManager.Instance.RemoveListener(GameEvent.Event.SHOOTER_LEVEL_BEGIN, OnRecv_ShooterLevelBegin);
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_2DGAME_END, OnRecv_ShooterLevelEnd);
    }

    // Event Handlers
    protected void OnRecv_ShooterLevelBegin()
    {
        activateLauncher();
    }

    protected void OnRecv_ShooterLevelEnd()
    {
        deactivateLauncher();
    }

    protected void OnDisable()
    {
        deactivateLauncher();
    }
}
