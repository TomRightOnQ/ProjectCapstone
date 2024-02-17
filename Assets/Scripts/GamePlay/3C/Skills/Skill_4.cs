using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skill Four
/// </summary>
public class Skill_4 : PlayerSkillBase
{
    [SerializeField] private float startAngle = 20f;
    [SerializeField] private float endAngle = 40f;
    [SerializeField] private int childCount = 3;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = FindFirstObjectByType<Camera>();
    }

    public override void SkillBegin()
    {
        // First read the direction
        Vector3 cursorPosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(cursorPosition);
        Vector3 playerPos = transform.position;
        Vector3 directionToCursor = (worldPosition - playerPos).normalized;

        StartCoroutine(releaseMissile(directionToCursor));
    }

    private IEnumerator releaseMissile(Vector3 directionToCursor)
    {
        float angleStep = (endAngle - startAngle) / childCount;
        float angle = startAngle;

        for (int i = 0; i < childCount; i++)
        {
            // Convert bearing angle to Unity angle
            float unityAngle = convertBearingToUnityAngle(angle);

            // Convert angle to direction vector
            Vector3 fireDirection = angleToDirectionVector(unityAngle);

            if (directionToCursor.x >= 0)
            {
                fireDirection = new Vector3(fireDirection.x, fireDirection.y, 0);
            }
            else
            {
                fireDirection = new Vector3(-fireDirection.x, fireDirection.y, 0);
            }

            // Launch the child projectile
            launchChildProjectile(fireDirection);

            angle += angleStep;
            yield return new WaitForSeconds(0.2f);
        }
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

    // Launch Single one
    private void launchChildProjectile(Vector3 fireDirection)
    {
        GameObject projObj = PrefabManager.Instance.Instantiate(childProjectileName, gameObject.transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetUp(projectileID);
            projectile.Launch(fireDirection, null);
        }
        else
        {
            Debug.LogWarning("Player: Unable to find projectile component");
        }
    }
}
