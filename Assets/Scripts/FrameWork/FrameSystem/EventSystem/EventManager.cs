using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Event
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // Dictionary to hold all the delegates, keyed by event type.
    private Dictionary<GameEvent.Event, System.Action> eventDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            eventDictionary = new Dictionary<GameEvent.Event, System.Action>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add a listener for a specific event type.
    public void AddListener(GameEvent.Event eventType, System.Action listener)
    {
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = null;
        }

        eventDictionary[eventType] += listener;
    }

    // Remove a listener for a specific event type.
    public void RemoveListener(GameEvent.Event eventType, System.Action listener)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= listener;
        }
    }

    // Post an event of a specific type.
    public void PostEvent(GameEvent.Event eventType)
    {
        if (eventDictionary.ContainsKey(eventType) && eventDictionary[eventType] != null)
        {
            eventDictionary[eventType].Invoke();
        }
    }
}
