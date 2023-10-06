using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager that handles objects' interaction shown on the HUD
/// </summary>
public class HUDInteractionManager : UIBase
{
    private static HUDInteractionManager instance;
    public static HUDInteractionManager Instance => instance;


    // UI Components
    [SerializeField] private UI_HUDInteraction ui_HUDInteraction;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public:
    // Allow an object to be interactable
    public void EnableInteraction(MObject target, int interactionID)
    {
        MEntity targetEntity = target.GetComponent<MEntity>();
        if (targetEntity == null)
        {
            Debug.LogWarning("HUDInteractionManager: The target object for interaction is not an MEntity");
            return;
        }

        // Check if the entity already has a trigger attached. If not, instantiate and attach.
        HUDInteractionTrigger interactionTrigger = targetEntity.InteractionTrigger;
        if (interactionTrigger == null)
        {
            GameObject triggerObject = PrefabManager.Instance.Instantiate("HUDInteractionTrigger", target.transform.position, Quaternion.identity);
            interactionTrigger = triggerObject.GetComponent<HUDInteractionTrigger>();
            targetEntity.InteractionTrigger = interactionTrigger;
        }

        // Setup the trigger
        interactionTrigger.SetUpTrigger(interactionID);
    }

    // Disable the interaction trigger of an object
    public void DisableInteraction(MObject target)
    {
        MEntity targetEntity = target.GetComponent<MEntity>();
        if (targetEntity == null)
        {
            Debug.LogWarning("HUDInteractionManager: The target object for interaction is not an MEntity");
            return;
        }

        HUDInteractionTrigger interactionTrigger = targetEntity.InteractionTrigger;
        if (interactionTrigger != null)
        {
            interactionTrigger.DisableTrigger();
        }
    }

    // Begin the interaction
    public void BeginInteraction(int chatID)
    {

    }

    // End the current interaction
    public void EndInteraction()
    {

    }
}
