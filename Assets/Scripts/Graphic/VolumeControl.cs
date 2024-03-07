using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

// Attach to HDRP Volume to conrtol by graphic settings
public class VolumeControl : MonoBehaviour
{
    // Volume
    [SerializeField] private Volume volumeComponent;

    // Volume Component
    private Fog fogComponent;

    private void Awake()
    {
        if (volumeComponent == null)
        {
            volumeComponent = GetComponent<Volume>();
        }
        configListener();

        // Find all volume
        volumeComponent.profile.TryGet<Fog>(out fogComponent);
    }

    // Private:
    private void configListener()
    {
        EventManager.Instance.AddListener(GameEvent.Event.GRAPHICS_LEVEL_CHANGED, ChangeLevel);

        int graphicLevel = PlayerPrefs.GetInt("graphicLevel", 0);
        switch (graphicLevel)
        {
            case 0:
                SetAsLow();
                break;
            case 1:
                SetAsMedium();
                break;
            case 2:
                SetAsHigh();
                break;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvent.Event.GRAPHICS_LEVEL_CHANGED, ChangeLevel);
    }

    // Public:
    public void ChangeLevel()
    {
        int graphicLevel = PlayerPrefs.GetInt("graphicLevel", 0);
        switch (graphicLevel)
        {
            case 0:
                SetAsLow();
                break;
            case 1:
                SetAsMedium();
                break;
            case 2:
                SetAsHigh();
                break;
        }
    }

    public void SetAsLow()
    {
        if (fogComponent != null)
        {
            fogComponent.enabled.Override(false);
        }
    }

    public void SetAsMedium()
    {
        if (fogComponent != null)
        {
            fogComponent.enabled.Override(true);
        }
    }

    public void SetAsHigh()
    {
        if (fogComponent != null)
        {
            fogComponent.enabled.Override(true);
        }
    }
}
