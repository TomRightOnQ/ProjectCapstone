using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UIWidgets;
using UnityEngine.EventSystems;

/// <summary>
/// Task System - Task Board UI Components
/// </summary>
public class UI_Task : UIBase
{
    // Current Tracked Task
    private int currentTrackedTask = -1;

    // Widget
    [SerializeField] private GameObject P_Content;
    [SerializeField] private TextMeshProUGUI TB_NameTitle;
    [SerializeField] private TextMeshProUGUI RTB_TaskContent;
    [SerializeField] private GameObject Img_DefualtImage;

    // Task List
    [SerializeField] private ListViewIcons TaskList;
    [SerializeField] private GameObject viewPort;
    private ObservableList<ListViewIconsItemDescription> taskItems = new ObservableList<ListViewIconsItemDescription>();

    private void Awake()
    {
        TaskList.DataSource = taskItems;
    }

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

    // Refresh Task Detail
    private void refreshDetailPanel(int taskID)
    {
        Img_DefualtImage.SetActive(false);
        TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
        // TextAsset textAsset = ResourceManager.Instance.LoadText(Constants.NOTES_SOURCE_PATH, notesData.Path);
        TB_NameTitle.text = taskData.Name;
        RTB_TaskContent.text = taskData.Description;
    }

    // Public:
    // Show Panel
    public void ShowTaskPanel()
    {
        refreshList();
    }

    // Add to list
    public void AddToTaskList(int taskID)
    {
        if (!taskItems.Exists(item => item.Value == taskID))
        {
            TaskData.TaskDataStruct taskData = TaskData.GetData(taskID);
            ListViewIconsItemDescription newItem = new ListViewIconsItemDescription() { Value = taskID, Name = taskData.Name };
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

    // On Click Events
    public void OnClick_Btn_ClosePanel()
    {
        TaskManager.Instance.CloseTaskPanel();
    }

    public void OnClick_Btn_TrackTask()
    {
        TaskManager.Instance.TrackTask(currentTrackedTask);
        TaskManager.Instance.CloseTaskPanel();
    }

    public void OnClick_NotesList(int index, ListViewItem item, PointerEventData eventData)
    {
        currentTrackedTask = taskItems[index].Value;
        refreshDetailPanel(taskItems[index].Value);
    }
}
