using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Independent component that allows an MEntity to be interactable
/// </summary>
public class HUDInteractionTrigger : MonoBehaviour
{
    // Object
    [SerializeField] public MObject objectBase;

    // Trigger
    [SerializeField] private Collider trigger;
    // Data
    private HUDInteractionData.HUDInteractionDataStruct currentInteraction;

    [SerializeField]
    private List<int> interactionIDs = new List<int>();
    public List<int> InteractionIDs => interactionIDs;

    private void Start()
    {
        if (objectBase == null)
        {
            objectBase = GetComponentInParent<MObject>();
        }
    }

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
