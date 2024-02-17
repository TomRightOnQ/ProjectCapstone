using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vine summoned via skill
/// </summary>
public class SummonedVine : Projectile
{
    // Random Angle - USE BEARING ANGLE
    [SerializeField] private float minLaunchAngle = 350f;
    [SerializeField] private float maxLaunchAngle = 10f;

    // Distance - Set to 20 by default
    [SerializeField] private float moveDistance = 20f;

    // Effect: Out Particle
    [SerializeField] private ParticleSystem vineOutParticle;

    // Launch the Projectile
    public override void Launch(Vector3 launchDirection, Transform targetTransform)
    {
        if (projRigidBody == null)
        {
            Debug.LogWarning("Projectile: Unable to locate the Projectile RigidBody");
            return;
        }
        // Go for its Vector.up direction after init with a random angle
        // Use ProjSpeed as the speed, and go until the movement reach 20

        // Generate a random angle considering the wrap-around
        float randomAngle = Random.Range(0, 1) < 0.5f ?
                            Random.Range(minLaunchAngle, 360) :
                            Random.Range(0, maxLaunchAngle);

        Vector3 growthDirection = angleToDirectionVector(convertBearingToUnityAngle(randomAngle));
        transform.up = growthDirection;
        StartCoroutine(GrowVine(growthDirection, moveDistance));
        GameEffectManager.Instance.PlaySound(launchSFXName, transform.position);
        Invoke("explode", projLife - 0.5f);
    }

    private IEnumerator GrowVine(Vector3 direction, float targetLength)
    {
        float currentLength = 0f;
        while (currentLength < targetLength)
        {
            // Calculate the next step in growth, ensuring we don't exceed the target length
            float growthStep = projSpeed * Time.deltaTime;
            currentLength += growthStep;
            if (currentLength > targetLength)
            {
                growthStep -= currentLength - targetLength;
            }

            // Apply the growth step to the vine's scale or position
            // For scale-based growth (uncomment the line you need):
            // transform.localScale += new Vector3(0, growthStep, 0); // Assuming Y-axis growth

            // For position-based growth:
            transform.position += direction * growthStep; // Adjust direction as needed

            yield return null;
        }
        // Shake screen after
        PersistentDataManager.Instance.MainCamera.ShakeCamera(0.25f, 1.5f);
    }

    private float convertBearingToUnityAngle(float bearingAngle)
    {
        // Convert bearing angle to Unity's angle system
        return (450 - bearingAngle) % 360;
    }

    private Vector3 angleToDirectionVector(float angle)
    {
        // Convert angle (in degrees) to radians
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculate direction vector
        Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);

        return direction;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            other.GetComponent<Target>().TakeDamage(projDamage, transform.position);
            // Notify the main camera to shake
            PersistentDataManager.Instance.MainCamera.ShakeCamera(0.1f, 0.01f);
        }
    }

    // No Damage from explosion
    protected override void explode()
    {
        vineOutParticle.Play();
        projAnimator.Play("SummonedVineOut");
        Invoke("deactivate", 0.5f);
    }
}
