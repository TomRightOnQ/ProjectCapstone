using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIWidgets;

/// <summary>
/// UI Logic of the interaction list on HUD
/// </summary>
public class UI_HUDInteraction : UIBase
{
    // List of interactions
    [SerializeField, ReadOnly]
    private List<int> interactionIDs = new List<int>();

    // UI List
    [SerializeField] private ListViewIcons InteractionList;
    private ObservableList<ListViewIconsItemDescription> interactionItems = new ObservableList<ListViewIconsItemDescription>();

    private void Awake()
    {
        InteractionList.DataSource = interactionItems;
    }

    // Public:
    // Add interactions
    public void AddInteraction(int interactionID)
    {
        if (!interactionIDs.Contains(interactionID))
        {
            interactionIDs.Add(interactionID);
        }
        if (!interactionItems.Exists(item => item.Value == interactionID))
        {
            HUDInteractionData.HUDInteractionDataStruct interactionData = HUDInteractionData.GetData(interactionID);
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = interactionID , Name = interactionData.Content };
            interactionItems.Add(newItem);
        }
    }

    // Remove interactions
    public void RemoveInteraction(int interactionID)
    {
        if (interactionIDs.Contains(interactionID))
        {
            interactionIDs.Remove(interactionID);
        }
        ListViewIconsItemDescription itemToRemove = interactionItems.Find(item => item.Value == interactionID);
        if (itemToRemove != null)
        {
            interactionItems.Remove(itemToRemove);
        }
        if (interactionItems.Count == 0)
        {
            UIManager.Instance.HideUI("UI_HUDInteraction");
        }
    }
}
