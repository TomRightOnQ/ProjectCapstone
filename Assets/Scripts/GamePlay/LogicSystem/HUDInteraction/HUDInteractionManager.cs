using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager that handles objects' interaction shown on the HUD
/// </summary>
public class HUDInteractionManager : MonoBehaviour
{
    private static HUDInteractionManager instance;
    public static HUDInteractionManager Instance => instance;

    // UI Components
    [SerializeField] private UI_HUDInteraction ui_HUDInteraction;

    [SerializeField] private bool bInteractionAllowed = true;
    public bool InteractionAllowed => bInteractionAllowed;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            configEventHandlers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Private:
    // Config events
    public void configEventHandlers()
    {
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_CHAT_BEGIN, OnRecv_ChatBegin);
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_CHAT_END, OnRecv_ChatEnd);
    }

    // Public:
    public void Init()
    {
        if (ui_HUDInteraction == null)
        { 
            GameObject uiObject = UIManager.Instance.CreateUI("UI_HUDInteraction");
            ui_HUDInteraction = uiObject.GetComponent<UI_HUDInteraction>();
        }
        bInteractionAllowed = true;
    }

    // Add interaction to the list
    public void AddInteractionToUIList(int interactionID, MObject interactionCarrier)
    {
        // Check the type of the interaction before proceeding
        HUDInteractionData.HUDInteractionDataStruct interactionData = HUDInteractionData.GetData(interactionID);
        if (!SaveManager.Instance.CheckHUDInteractionEnabled(interactionID))
        {
            return;
        }

        if (interactionData.bNoneClicking)
        {
            TaskManager.Instance.ProcessActions(interactionData.Action);
            // Remove one-time interaction
            if (interactionData.bOneTime)
            {
                SaveManager.Instance.DisableHUDInteraction(interactionID);
            }
        }
        else 
        {
            ui_HUDInteraction.AddInteraction(interactionID, interactionCarrier);
        }
    }

    // Remove interaction from the list
    public void RemoveInteractionFromUIList(int interactionID)
    {
        ui_HUDInteraction.RemoveInteraction(interactionID);
    }

    // Enable the UI for interaction
    public void EnableHUDInteractionUI()
    {
        ui_HUDInteraction.ChangeHUDINteractionState(true);
    }

    // Disable the UI
    public void DisableHUDInteractionUI()
    {
        ui_HUDInteraction.ChangeHUDINteractionState(false);
    }

    // Enable and Disable Player's trigger
    public void EnableHUDInteractionOnPlayer()
    {
        PersistentDataManager.Instance.MainPlayer.GetComponentInChildren<PlayerHUDInteractionTrigger>().SetUpTrigger();
        bInteractionAllowed = true;
    }

    public void DisableHUDInteractionOnPlayer()
    {
        PersistentDataManager.Instance.MainPlayer.GetComponentInChildren<PlayerHUDInteractionTrigger>().DisableTrigger();
        bInteractionAllowed = false;
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

    // Refresh Interaction List
    public void RefreshTriggerList()
    {
        Player player = PersistentDataManager.Instance.MainPlayer;
        if (player != null)
        {
            player.MainPlayerInteractionTrigger.DisableTrigger();
            player.MainPlayerInteractionTrigger.SetUpTrigger();
        }
    }

    // Event Handler
    public void OnRecv_ChatBegin()
    {
        DisableHUDInteractionUI();
    }

    public void OnRecv_ChatEnd()
    {
        if (bInteractionAllowed)
        {
            EnableHUDInteractionUI();
            RefreshTriggerList();
        }
    }
}
