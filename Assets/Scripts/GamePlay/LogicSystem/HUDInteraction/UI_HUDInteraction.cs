using UnityEngine;
using UIWidgets;
using UnityEngine.EventSystems;

/// <summary>
/// UI Logic of the interaction list on HUD
/// </summary>
public class UI_HUDInteraction : UIBase
{
    // UI List
    [SerializeField] private ListViewIcons InteractionList;
    [SerializeField] private GameObject viewPort;
    private ObservableList<ListViewIconsItemDescription> interactionItems = new ObservableList<ListViewIconsItemDescription>();

    private void Awake()
    {
        InteractionList.DataSource = interactionItems;
    }

    // Public:
    // Change visibility of the list
    public void ChangeHUDINteractionState(bool bShow)
    {
        viewPort.SetActive(bShow);
    }

    // Add interactions
    public void AddInteraction(int interactionID)
    {
        if (!interactionItems.Exists(item => item.Value == interactionID))
        {
            HUDInteractionData.HUDInteractionDataStruct interactionData = HUDInteractionData.GetData(interactionID);
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = interactionID , Name = interactionData.Content };
            interactionItems.Add(newItem);
        }
        if (interactionItems.Count != 0 && gameObject.activeSelf)
        {
            ChangeHUDINteractionState(true);
        }
    }

    // Remove interactions
    public void RemoveInteraction(int interactionID)
    {
        ListViewIconsItemDescription itemToRemove = interactionItems.Find(item => item.Value == interactionID);
        if (itemToRemove != null)
        {
            interactionItems.Remove(itemToRemove);
        }
        if (interactionItems.Count == 0 && gameObject.activeSelf)
        {
            ChangeHUDINteractionState(false);
        }
    }

    // Events:
    public void OnClick_InteractionList(int index, ListViewItem item, PointerEventData eventData)
    {
        if (item != null)
        {
            HUDInteractionData.HUDInteractionDataStruct interactionData = HUDInteractionData.GetData(interactionItems[item.Index].Value);
            if (interactionData == null)
            {
                return;
            }
            if (interactionData.Action[0] > 0)
            {
                TaskManager.Instance.ProcessActions(interactionData.Action);
            }
            // Remove one-time interaction
            if (interactionData.bOneTime)
            {
                SaveManager.Instance.DisableHUDInteraction(interactionItems[item.Index].Value);
            }
        }              
    }
}
