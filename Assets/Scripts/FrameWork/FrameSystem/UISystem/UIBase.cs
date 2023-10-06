using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class of UIs
public class UIBase : MonoBehaviour
{
    protected string uiName;

    public virtual void SetUp(UIData uiData)
    {
        uiName = uiData.name;
    }
}
