using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all in-game objects such as players, monsters ...
/// </summary>

public class MEntity : MObject
{
    // If the entity is considered in damage system
    protected bool bDamagable = false;

    // End an entity
    public virtual void DeactivateEntity()
    {
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
