using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guided missile
/// </summary>
public class MissileSmall : Projectile
{
    // Missile stats
    [SerializeField] private float baseTurnSpeed = 0.1f;
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float trackMinRange = 1f;
    [SerializeField] private float trackMaxRange = 5f;

    [SerializeField, ReadOnly]
    private Transform target = null;

    // flag
    [SerializeField, ReadOnly] 
    private bool bLocked = false;

    public override void Launch(Vector3 launchDirection, Transform targetTransform)
    {
        base.Launch(launchDirection, targetTransform);
        target = targetTransform;
    }

    protected override void FixedUpdate()
    {
        if (bProxFuse)
        {
            checkProximity();
        }
        if (target != null && bLocked)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;


            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, angleToTarget)) > lockAngle)
            {
                target = null;
                bLocked = false;
                return;
            }

            // Calculate LOS rate of change
            float losRate = Mathf.DeltaAngle(transform.eulerAngles.z, angleToTarget) / Time.deltaTime;

            // Apply Proportional Navigation logic
            float turnRate = Mathf.Clamp(losRate * baseTurnSpeed, -turnSpeed, turnSpeed);

            // Smoothly adjust the rotation
            Quaternion targetRotation = Quaternion.Euler(0, 0, angleToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Abs(turnRate) * Time.deltaTime);

            projRigidBody.velocity = transform.up * projSpeed;
        }
        else if (!bLocked)
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, trackMaxRange, proxTargetLayerMask);
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToCollider = (hitCollider.transform.position - transform.position).normalized;
            float angleToCollider = Vector3.Angle(transform.up, directionToCollider);

            if (angleToCollider <= lockAngle)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance && distance > trackMinRange)
                {
                    closestDistance = distance;
                    closestTarget = hitCollider.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
            bLocked = true;
        }
    }
}
