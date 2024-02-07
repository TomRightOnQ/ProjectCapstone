using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To allow multiple levels within one scene, this will indicate the group ID of a cluster of level
/// </summary>
public class Level2DGroup : MonoBehaviour
{
    [SerializeField] private int groupID = 1;
    public int GroupID => groupID;
}
