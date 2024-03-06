using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Laser Blade Summoned by player skill
/// </summary>
public class SummonedLaser : Projectile
{
    [SerializeField] private LineRenderer lineRenderer;

    // Rotational Data
    [SerializeField] private float maxRot = 360f;
    [SerializeField, ReadOnly] private float totalRotation = 0f;
    [SerializeField, ReadOnly] private bool continueRotation = true;

    public override void Launch(Vector3 launchDirection, Transform targetTransform)
    {
        if (projRigidBody == null)
        {
            Debug.LogWarning("Projectile: Unable to locate the Projectile RigidBody");
            return;
        }
        // Launch SFX
        playProjectileSound(launchSFXName);
        continueRotation = true;
    }

    protected override void FixedUpdate()
    {
        transform.position = PersistentDataManager.Instance.MainPlayer.transform.position;
        if (continueRotation)
        {
            float rotationThisFrame = projSpeed * Time.fixedDeltaTime;
            transform.RotateAround(transform.position, Vector3.forward, rotationThisFrame);

            totalRotation += rotationThisFrame;
            if (totalRotation >= maxRot)
            {
                continueRotation = false; 
                totalRotation = 0f;
                deactivate();
            }
        }

        // Perform raycasting at the specified frequency
        PerformRaycast();
    }

    private void PerformRaycast()
    {
        Vector3 start = transform.position;
        Vector3 end = start + transform.up * projLife;
        Vector3 direction = transform.up; // Assuming the 'up' direction of the laser GameObject is the forward direction of the laser
        float maxDistance = projLife; // Maximum distance of the raycast

        // Perform the raycast
        RaycastHit[] hits = Physics.RaycastAll(start, direction, maxDistance);
        hits = hits.OrderBy(h => h.distance).ToArray();

        foreach (RaycastHit hit in hits.OrderBy(h => h.distance))
        {
            if (hit.collider.CompareTag("Target"))
            {
                // Play Effect At the End and Each Collision
                GameEffectManager.Instance.PlayVFX("LaserExplode", hit.point, Vector3.one);
                hit.collider.GetComponent<Target>().TakeDamage(projDamage * 2, transform.position);
            }
            else if (hit.collider.CompareTag("Boss"))
            {
                // Play Effect At the End and Each Collision
                GameEffectManager.Instance.PlayVFX("LaserExplode", hit.point, Vector3.one);
                hit.collider.GetComponent<Boss>().TakeDamage(projDamage);
                // Boss will block the skill
                end = hit.point;
                break;
            }
            else if (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Barrier"))
            {
                // Play Effect At the End and Each Collision
                GameEffectManager.Instance.PlayVFX("LaserExplode", hit.point, Vector3.one);
                end = hit.point;
                break;
            }
        }
        // Update the laser's visual representation as needed
        UpdateLaserVisual(start, end);
    }

    private void UpdateLaserVisual(Vector3 start, Vector3 end)
    {
        // Adjust the line renderer to match the laser's current orientation and extent
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;
    }
}
