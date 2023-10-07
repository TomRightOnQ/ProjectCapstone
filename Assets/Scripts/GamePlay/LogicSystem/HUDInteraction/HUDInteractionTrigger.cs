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

    [SerializeField, ReadOnly]
    private List<int> interactionIDs = new List<int>();
    public List<int> InteractionIDs => interactionIDs;

    //Public:

    public void SetUpTrigger(int interactionID)
    {
        if (!interactionIDs.Contains(interactionID))
        {
            interactionIDs.Add(interactionID);
        }
    }

    public void RemoveTrigger(int interactionID)
    {
        if (interactionIDs.Contains(interactionID))
        {
            interactionIDs.Remove(interactionID);
        }
    }

    public void EnableTrigger()
    {
        trigger.enabled = true;
    }

    public void DisableTrigger()
    {
        trigger.enabled = false;
    }
}
