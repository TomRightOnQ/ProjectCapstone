using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the trigger of the damage zone
/// </summary>
public class DamageZone : MonoBehaviour
{
    // Damage Taker Reference
    [SerializeField] private RotationalDamageTaker rotationalDamageTaker;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            rotationalDamageTaker.AffectPlayer();
        }
    }
}
