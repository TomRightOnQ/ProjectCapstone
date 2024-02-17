using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skill Two
/// </summary>
public class Skill_2 : PlayerSkillBase
{
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

        Vector3 SummonPosition;
        if (directionToCursor.x >= 0)
        {
            SummonPosition = new Vector3(transform.position.x + 4, transform.position.y - 10, 0);
        }
        else
        {
            SummonPosition = new Vector3(transform.position.x - 4, transform.position.y - 10, 0);
        }

        // Spawn the vine and launch
        GameObject projObj = PrefabManager.Instance.Instantiate(childProjectileName, SummonPosition, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetUp(projectileID);
            projectile.Launch(Vector3.zero, null);
        }
        else
        {
            Debug.LogWarning("Player: Unable to find projectile component");
        }
    }
}
