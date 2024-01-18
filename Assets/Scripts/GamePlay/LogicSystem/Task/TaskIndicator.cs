using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Indicator object - Tracking a specific position or object
/// </summary>
public class TaskIndicator : MObject
{
    // Target Reference
    private GameObject targetObject;

    // Target Position
    private Vector3 targetPosition;

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
        if (targetObject == null)
        {
            return;
        }
    }
}
