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


    // Private:
    private void OnTriggerEnter(Collider other)
    {
        
    }


    // Public:
    public void SetUpTrigger(int interactionID)
    {
        trigger.enabled = true;
    }

    public void DisableTrigger()
    {
        trigger.enabled = false;
    }
}
