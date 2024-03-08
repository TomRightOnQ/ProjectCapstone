using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dummy Object launching targets
/// Begin and end when recv events
/// Events set to ShooterLevel begin and end
/// </summary>
public class TargetLauncher : MEntity
{
    // Event
    [SerializeField] private GameEvent.Event triggerEvent = GameEvent.Event.SHOOTER_LEVEL_BEGIN;
    [SerializeField] private GameEvent.Event endEvent = GameEvent.Event.EVENT_2DGAME_END;

    // Target name
    [SerializeField] private string targetName;

    // Launching Time
    [SerializeField] private float minLaunchTime = 1f;
    [SerializeField] private float maxLaunchTime = 1f;

    // Launching Force
    [SerializeField] private float minLaunchForce = 1f;
    [SerializeField] private float maxLaunchForce = 1f;

    // Launching Angle
    [SerializeField] private float minLaunchAngle = 0f;
    [SerializeField] private float maxLaunchAngle = 0f;

    // Cluster Count
    [SerializeField] private int clusterCount = 1;
    [SerializeField] private float clusterGap = 0.1f;

    // Flags
    [SerializeField, ReadOnly]
    private bool bActivated = false;
    private Coroutine launchRoutine;
    [SerializeField] private bool bTracking = false;

    // Launch SFX
    [SerializeField] private string launchSFXName = "None";

    protected override void Awake()
    {
        base.Awake();
        // Subscribe the events of shooter scene begin and end
        EventManager.Instance.AddListener(triggerEvent, OnRecv_Begin);
        EventManager.Instance.AddListener(endEvent, OnRecv_End);
    }

    // Tracking Player
    private void Update()
    {
        if (bTracking && bActivated)
        {
            Vector3 directionToPlayer = PersistentDataManager.Instance.MainPlayer.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            angle -= 90;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
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

            Vector3 randomizedDirection = new Vector3(
                transform.up.x + UnityEngine.Random.Range(-minLaunchAngle, maxLaunchAngle),
                transform.up.y + UnityEngine.Random.Range(-minLaunchAngle, maxLaunchAngle),
                transform.up.z
            ).normalized;

            for (int i = 0; i < clusterCount; i++)
            {
                launchTarget(randomizedDirection);
                yield return new WaitForSeconds(clusterGap);
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    // Launch Target
    protected virtual void launchTarget(Vector3 randomizedDirection)
    {
        // Spawn Target
        GameObject targetObj = PrefabManager.Instance.Instantiate(targetName, gameObject.transform.position, Quaternion.identity);
        Target target = targetObj.GetComponent<Target>();
        // Calculate Launch force muliplier
        float launchForce = Random.Range(minLaunchForce, maxLaunchForce);

        if (target != null)
        {
            if (launchSFXName != "None")
            {
                // Launch SFX
                GameEffectManager.Instance.PlaySound(launchSFXName);
            }

            target.SetUp();
            target.Launch(randomizedDirection, launchForce);
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
        if (bTracking)
        {
            Vector3 directionToPlayer = PersistentDataManager.Instance.MainPlayer.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            angle -= 90;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        if (launchRoutine == null && gameObject.activeSelf)
        {
            launchRoutine = StartCoroutine(LaunchTargetsRoutine());
        }
    }
    // Stop the launcer
    protected void deactivateLauncher()
    {
        EventManager.Instance.RemoveListener(triggerEvent, OnRecv_Begin);
        EventManager.Instance.RemoveListener(endEvent, OnRecv_End);
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
    }

    // Event Handlers
    protected void OnRecv_Begin()
    {
        activateLauncher();
    }

    protected void OnRecv_End()
    {
        deactivateLauncher();
    }

    protected void OnDisable()
    {
        deactivateLauncher();
    }
}
