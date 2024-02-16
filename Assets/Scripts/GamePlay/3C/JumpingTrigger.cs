using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens to player's jumping detection trigger
/// </summary>
public class JumpingTrigger : MonoBehaviour
{
    // Player Controller
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerController == null)
        {
            return;
        }
        if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Barrier")
        {
            playerController.ResetJumping();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerController == null)
        {
            return;
        }
        if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Barrier")
        {
            playerController.ResetJumping();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerController == null)
        {
            return;
        }
        if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Barrier")
        {
            playerController.LockJumping();
        }
    }
}
