using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Indicator object - Tracking a specific position or object
/// </summary>
public class TaskIndicator : MObject
{
    // Target Reference
    [SerializeField, ReadOnly]
    private GameObject targetObject;

    [SerializeField, ReadOnly]
    // Target Position
    private Vector3 targetPosition;

    [SerializeField, ReadOnly]
    // Tracking mod
    private bool bTrackByPosition = false;

    // Config the indicator
    // via object or position
    public void ConfigIndicator(GameObject target)
    {
        targetObject = target;
        bTrackByPosition = false;
    }

    public void ConfigIndicator(Vector3 position)
    {
        targetPosition = position;
        bTrackByPosition = true;
    }

    // During each update, adjust the position
    private void Update()
    {
        if (bTrackByPosition)
        {
            UpdateIndicatorPosition(targetPosition);
        }
        else if (targetObject != null)
        {
            UpdateIndicatorPosition(targetObject.transform.position);
        }
    }

    private void UpdateIndicatorPosition(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
        screenPosition.y = Mathf.Clamp(screenPosition.y + 100f, 0, Screen.height);

        // Set the indicator's position
        transform.position = screenPosition;
    }
}
