using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skill Five
/// </summary>
public class Skill_5 : PlayerSkillBase
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

        // Spawn the skill parent projectile and launch
        GameObject projObj = PrefabManager.Instance.Instantiate(childProjectileName, gameObject.transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetUp(projectileID);
            projectile.Launch(directionToCursor, null);
        }
        else
        {
            Debug.LogWarning("Player: Unable to find projectile component");
        }
    }
}
