using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2D Game Scene: Triggers to end the game
/// </summary>
public class EndTrigger : MonoBehaviour
{
    // Whether the trigger passes the game
    // Whether the trigger creates a victory
    [SerializeField] private bool bPass = true;
    [SerializeField] private bool bVictory = true;

    private LayerMask interactableLayer;

    private void Awake()
    {
        // Set your layer mask to reference the "Interactable" layer
        interactableLayer = LayerMask.GetMask("Player");
    }

    // Private:
    private void OnTriggerEnter(Collider other)
    {
        if ((interactableLayer.value & 1 << other.gameObject.layer) > 0)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager2D.Instance.EndGame(bPass, bVictory);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((interactableLayer.value & 1 << other.gameObject.layer) > 0)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager2D.Instance.EndGame(bPass, bVictory);
            }
        }
    }
}
