using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The non-generic base of object pools
/// </summary>
/// 
public class ObjPoolBase
{
    public virtual GameObject GetGameObject() { return null; }
    public virtual void ReturnGameObject(GameObject prefab) { }
}
