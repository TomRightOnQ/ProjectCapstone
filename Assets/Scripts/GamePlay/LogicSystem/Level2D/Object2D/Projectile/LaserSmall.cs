using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Laser
/// </summary>
public class LaserSmall : Projectile
{
    [SerializeField] private LineRenderer lineRenderer;
    
    public override void Launch(Vector3 launchDirection, Transform targetTransform)
    {
        Vector3 start = transform.position;
        Vector3 end = start + launchDirection * projLife;

        RaycastHit[] hits = Physics.SphereCastAll(start, projDamageRange, launchDirection, 100);
        hits = hits.OrderBy(h => h.distance).ToArray();
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Target"))
            {
                hit.collider.GetComponent<Target>().TakeDamage(projDamage, transform.position);
            }
            else if (hit.collider.CompareTag("Boss"))
            {
                hit.collider.GetComponent<Boss>().TakeDamage(projDamage);
            }
            else if (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Barrier"))
            {
                end = hit.point;
                break;
            }
        }

        // Set up the line renderer
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.enabled = true;
        Invoke("deactivate", 0.1f);

        // Launch SFX
        GameEffectManager.Instance.PlaySound(launchSFXName, transform.position);
    }
}
