using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance => instance;

    // Player Controller
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public:
    public void SetController(PlayerController controller)
    {
        playerController = controller;
    }

    // Unlock player input
    public void UnLockInput(Enums.SCENE_TYPE worldType)
    {
        if (playerController == null)
        {
            return;
        }
        playerController.ChangePlayerInputState(true);
        playerController.ChangePlayerMovementState(true);
        playerController.ChangePlayerActiveMovementState(true);
        playerController.SetPlayerMode(worldType);
    }

    // Lock Player input
    public void LockInput()
    {
        
    }
}
