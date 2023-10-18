using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core component to achieve 7-day cycle for the plot
/// </summary>
public class DayCycleManager : MonoBehaviour
{
    private static DayCycleManager instance;
    public static DayCycleManager Instance => instance;

    // Current Day
    [SerializeField, ReadOnly]
    private int currentDay = 0;
    public int CurrentDay => currentDay;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public:
    // Jump to the beginning of a specific day
    public void JumpToDay(int targetDay)
    {
    
    }

    // Go to the next day
    public void GoToNextDay()
    {
        if (currentDay < 7)
        {
            JumpToDay(currentDay++);
        }
    }

    // Show the daily report
    public void ShowReport(int targetDay)
    {
        
    }
}
