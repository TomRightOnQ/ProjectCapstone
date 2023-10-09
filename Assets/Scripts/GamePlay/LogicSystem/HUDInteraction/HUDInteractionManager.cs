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
    public void Init()
    {
        if (ui_HUDInteraction == null)
        {
            ui_HUDInteraction = UIManager.Instance.CreateUI("UI_HUDInteraction").GetComponent<UI_HUDInteraction>();
        }
    }

    // Add interaction to the list
    public void AddInteractionToUIList(int interactionID)
    {
        EnableHUDInteractionUI();
        ui_HUDInteraction.AddInteraction(interactionID);
    }

    // Remove interaction from the list
    public void RemoveInteractionFromUIList(int interactionID)
    {
        ui_HUDInteraction.RemoveInteraction(interactionID);
    }

    // Enable the UI for interaction
    public void EnableHUDInteractionUI()
    {
        UIManager.Instance.ShowUI("UI_HUDInteraction");
    }

    // Disable the UI
    public void DisableHUDInteractionUI()
    {
        UIManager.Instance.HideUI("UI_HUDInteraction");
    }

    // Allow an object to be interactable
    public void AddInteraction(MObject target, int interactionID)
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
    public void RemoveInteraction(MObject target, int interactionID)
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
            interactionTrigger.RemoveTrigger(interactionID);
        }
    }

    // Begin the interaction
    public void EnableInteraction(MObject target)
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
            interactionTrigger.EnableTrigger();
        }
    }

    // End the current interaction
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
}
