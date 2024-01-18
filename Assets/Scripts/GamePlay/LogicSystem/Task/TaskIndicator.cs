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

    // During each update, adjust the position
    private void Update()
    {
        if (targetObject == null)
        {
            return;
        }
    }
}
