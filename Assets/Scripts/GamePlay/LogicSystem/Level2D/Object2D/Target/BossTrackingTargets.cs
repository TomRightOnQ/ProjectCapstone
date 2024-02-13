using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Target spawned in clusters
/// </summary>
public class BossTrackingTargets : Target
{
    public override void SetUp()
    {
        base.SetUp();
        transform.localRotation = Quaternion.Euler(45, 45, 45);
    }

    private void OnDisable()
    {
        transform.localRotation = Quaternion.identity;
    }
}
