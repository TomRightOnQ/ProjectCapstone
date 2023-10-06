using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Independent component that allows an MEntity to be interactable
/// </summary>
public class HUDInteractionTrigger : MonoBehaviour
{
    // Trigger
    [SerializeField] private Collider trigger;
    // Data
    private HUDInteractionData.HUDInteractionDataStruct currentInteraction;
    private int currentInteractionID;

    public void SetUpTrigger(int interactionID)
    {
        trigger.enabled = true;
    }

    public void DisableTrigger()
    {
        trigger.enabled = false;
    }
}
