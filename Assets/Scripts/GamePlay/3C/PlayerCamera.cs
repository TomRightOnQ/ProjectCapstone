using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Camera System
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    // Player Mode: The camera will follow the player
    private bool bPlayerMode = true;
    [SerializeField] private float cameraYOffset = 3f;
    [SerializeField] private float cameraZOffset = 4f;
    [SerializeField] private GameObject player;

    private void Update()
    {
        // Exit if player is null or camera set to not following
        if (player == null || !bPlayerMode)
        {
            return;
        }
        this.gameObject.transform.position = new Vector3(player.transform.position.x , cameraYOffset, player.transform.position.z - cameraZOffset);
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    public void SetCameraToPlayerMode(bool bSetToPlayerMode = true)
    {
        bPlayerMode = bSetToPlayerMode;
    }
}
