using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pre-palced Scene Manager to alter time settings
/// </summary>
public class TimeController : MObject
{
    // Preset Timesets
    [SerializeField] private GameObject timeSet_Morning;
    [SerializeField] private GameObject timeSet_Noon;
    [SerializeField] private GameObject timeSet_Sunset;
    [SerializeField] private GameObject timeSet_Night;
    [SerializeField] private GameObject timeSet_Dark;

    protected override void Awake()
    {
        base.Awake();
        // Add time event handlers
        EventManager.Instance.AddListener(GameEvent.Event.TIME_MORNING, OnRecv_Time_Morning);
        EventManager.Instance.AddListener(GameEvent.Event.TIME_NOON, OnRecv_Time_Noon);
        EventManager.Instance.AddListener(GameEvent.Event.TIME_SUNSET, OnRecv_Time_Sunset);
        EventManager.Instance.AddListener(GameEvent.Event.TIME_NIGHT, OnRecv_Time_Night);
        EventManager.Instance.AddListener(GameEvent.Event.TIME_DARK, OnRecv_Time_Dark);
    }

    // Remove all listeners
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvent.Event.TIME_MORNING, OnRecv_Time_Morning);
        EventManager.Instance.RemoveListener(GameEvent.Event.TIME_NOON, OnRecv_Time_Noon);
        EventManager.Instance.RemoveListener(GameEvent.Event.TIME_SUNSET, OnRecv_Time_Sunset);
        EventManager.Instance.RemoveListener(GameEvent.Event.TIME_NIGHT, OnRecv_Time_Night);
        EventManager.Instance.RemoveListener(GameEvent.Event.TIME_DARK, OnRecv_Time_Dark);
    }

    // Event Handlers
    private void OnRecv_Time_Morning()
    {
        timeSet_Noon.SetActive(false);
        timeSet_Sunset.SetActive(false);
        timeSet_Night.SetActive(false);
        timeSet_Dark.SetActive(false);

        timeSet_Morning.SetActive(true);
    }

    private void OnRecv_Time_Noon()
    {
        timeSet_Morning.SetActive(false);
        timeSet_Sunset.SetActive(false);
        timeSet_Night.SetActive(false);
        timeSet_Dark.SetActive(false);

        timeSet_Noon.SetActive(true);
    }

    private void OnRecv_Time_Sunset()
    {
        timeSet_Morning.SetActive(false);
        timeSet_Noon.SetActive(false);
        timeSet_Night.SetActive(false);
        timeSet_Dark.SetActive(false);

        timeSet_Sunset.SetActive(true);
    }

    private void OnRecv_Time_Night()
    {
        timeSet_Morning.SetActive(false);
        timeSet_Noon.SetActive(false);
        timeSet_Sunset.SetActive(false);
        timeSet_Dark.SetActive(false);

        timeSet_Night.SetActive(true);
    }

    private void OnRecv_Time_Dark()
    {
        timeSet_Morning.SetActive(false);
        timeSet_Noon.SetActive(false);
        timeSet_Sunset.SetActive(false);
        timeSet_Night.SetActive(false);

        timeSet_Dark.SetActive(true);
    }
}
