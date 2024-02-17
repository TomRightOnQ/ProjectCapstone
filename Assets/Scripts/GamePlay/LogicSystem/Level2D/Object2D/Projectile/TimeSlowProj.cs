using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special Projectile that slows down all enemies inside
/// Cannot deal damage
/// </summary>
public class TimeSlowProj : Projectile
{
    [SerializeField] private float flyTime = 0.5f;
    [SerializeField] private float effectTime = 3f;
    [SerializeField] private float slowRatio = 0.1f;

    public override void Launch(Vector3 launchDirection, Transform targetTransform)
    {
        base.Launch(launchDirection, targetTransform);
        StartCoroutine(triggerEffect());
    }

    private IEnumerator triggerEffect()
    {
        yield return new WaitForSeconds(flyTime);
        projAnimator.Play("In");

        // Set the RigidBody of the projectile
        projRigidBody.useGravity = false;
        projRigidBody.velocity = Vector3.zero;

        // Reset
        yield return new WaitForSeconds(effectTime);
        projAnimator.Play("Out");

        yield return new WaitForSeconds(0.21f);
        projRigidBody.useGravity = true;

        if (explodeVFXName != "None")
        {
            GameEffectManager.Instance.PlayVFX(explodeVFXName,
                new Vector3(transform.position.x, transform.position.y, 0),
                Vector3.one);
        }
        projAnimator.Play("Default");
        deactivate();
    }

    // Slow and Return speed
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            // Slow the other object
            Rigidbody targetRigidBody = other.GetComponent<Rigidbody>();
            if (targetRigidBody != null)
            {
                targetRigidBody.velocity *= slowRatio;
                targetRigidBody.useGravity = false;
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Target")
        {
            // Slow the other object
            Rigidbody targetRigidBody = other.GetComponent<Rigidbody>();
            if (targetRigidBody != null)
            {
                targetRigidBody.velocity *= 1/slowRatio;
                targetRigidBody.useGravity = other.GetComponent<Target>().bGravity;
            }
        }
    }

    // Nullified explode
    // Do not use
    protected override void explode()
    {
        
    }
}
