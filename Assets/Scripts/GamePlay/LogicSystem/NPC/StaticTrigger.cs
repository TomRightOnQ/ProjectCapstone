using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Static Trigger objects
/// </summary>
public class StaticTrigger : EUnit
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.tag = "StaticTrigger";
    }
}
