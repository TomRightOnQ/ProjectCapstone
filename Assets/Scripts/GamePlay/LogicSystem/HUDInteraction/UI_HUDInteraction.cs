using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI Logic of the interaction list on HUD
/// </summary>
public class UI_HUDInteraction : UIBase
{
    // List of interactions
    [SerializeField, ReadOnly]
    private List<int> interactionIDs = new List<int>();

    // Public:
    // Add interactions
    public void AddInteraction(int interactionID)
    {
        if (!interactionIDs.Contains(interactionID))
        {
            interactionIDs.Add(interactionID);
        }
    }

    // Remove interactions
    public void RemoveInteraction(int interactionID)
    {
        if (interactionIDs.Contains(interactionID))
        {
            interactionIDs.Remove(interactionID);
        }
    }

    // Hide Interactions
    public void EndChat()
    {
        gameObject.SetActive(false);
    }

    // Show Interactions
    public void StartChat(int chatID)
    {
        gameObject.SetActive(true);
    }
}
