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

    // Screen Interact
    protected HUDInteractionTrigger interactionTrigger;
    public HUDInteractionTrigger InteractionTrigger { get { return interactionTrigger; } set { interactionTrigger = value; } }

    // HUDInteraction
    // Override this to interact
    public virtual void HUDInteract()
    {
    
    }

    // End an entity
    public virtual void DeactivateEntity()
    {
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
