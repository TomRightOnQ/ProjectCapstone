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
    [SerializeField] protected HUDInteractionTrigger interactionTrigger;
    public HUDInteractionTrigger InteractionTrigger { get { return interactionTrigger; } set { interactionTrigger = value; } }

    // Bubble
    [SerializeField] protected EntityBubble bubble;

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

    // Say something
    public virtual void EntitySay(string content = "", float time = 2f)
    {
        if (bubble != null)
        {
            bubble.BeginBubble(content, time);
        }
    }
}
