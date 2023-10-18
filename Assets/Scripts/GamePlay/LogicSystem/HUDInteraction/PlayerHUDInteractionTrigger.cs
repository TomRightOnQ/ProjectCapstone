using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interaction Trigger for the player
/// </summary>
public class PlayerHUDInteractionTrigger : MonoBehaviour
{
    // Trigger
    [SerializeField] private Collider trigger;
    private LayerMask interactableLayer;

    private void Awake()
    {
        // Set your layer mask to reference the "Interactable" layer
        interactableLayer = LayerMask.GetMask("Interactable");
    }

    // Private:
    private void OnTriggerEnter(Collider other)
    {
        if ((interactableLayer.value & 1 << other.gameObject.layer) > 0)
        {
            HUDInteractionTrigger otherTrigger = other.gameObject.GetComponent<HUDInteractionTrigger>();
            if (otherTrigger != null) 
            {
                for (int i = 0; i < otherTrigger.InteractionIDs.Count; i++)
                {
                    HUDInteractionManager.Instance.AddInteractionToUIList(otherTrigger.InteractionIDs[i]);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((interactableLayer.value & 1 << other.gameObject.layer) > 0)
        {
            HUDInteractionTrigger otherTrigger = other.gameObject.GetComponent<HUDInteractionTrigger>();
            if (otherTrigger != null)
            {
                for (int i = 0; i < otherTrigger.InteractionIDs.Count; i++)
                {
                    HUDInteractionManager.Instance.RemoveInteractionFromUIList(otherTrigger.InteractionIDs[i]);
                }
            }
        }
    }

    // Public:
    public void SetUpTrigger()
    {
        trigger.enabled = true;
    }

    public void DisableTrigger()
    {
        trigger.enabled = false;
    }
}