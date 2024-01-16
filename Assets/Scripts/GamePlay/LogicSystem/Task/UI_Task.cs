using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UIWidgets;

/// <summary>
/// Task System - Task Board UI Components
/// </summary>
public class UI_Task : UIBase
{
    // Task List
    [SerializeField] private ListViewIcons TaskList;
    [SerializeField] private GameObject viewPort;
    private ObservableList<ListViewIconsItemDescription> taskItems = new ObservableList<ListViewIconsItemDescription>();

    // Private:
    // Refresh List
    private void refreshList()
    {
        // Clear old data
        taskItems.Clear();
        // Retriving data from SaveSystem
        List<int> listData = SaveManager.Instance.GetTriggeredTasks();
        for (int i = 0; i < listData.Count; i++)
        {
            AddToTaskList(listData[i]);
        }
    }

    // Public:
    // Add to list
    public void AddToTaskList(int taskID)
    {
        if (!taskItems.Exists(item => item.Value == taskID))
        {
            NotesData.NotesDataStruct notesData = NotesData.GetData(taskID);
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = taskID, Name = notesData.Name };
            taskItems.Add(newItem);
        }
    }

    // Remove from the list
    public void RemoveFromTaskList(int taskID)
    {
        ListViewIconsItemDescription itemToRemove = taskItems.Find(item => item.Value == taskID);
        if (itemToRemove != null)
        {
            taskItems.Remove(itemToRemove);
        }
    }
}
