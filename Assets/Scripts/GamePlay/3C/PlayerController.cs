using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Attching this component to the player for control
/// </summary>

public class PlayerController : MonoBehaviour 
{
    // Core: Player Control class and the player
    private PlayerControls playerControls;
    [SerializeField] private Player player;

    // Data
    [SerializeField] private float moveRatio = 1f;
    [SerializeField] private float jumpRatio = 1f;
    [SerializeField] private float dashRatio = 1f;
    [SerializeField] private Rigidbody playerRigidBody;

    // Lock input from keyboard
    private bool bInputLocked = false;

    // Lock attack movement
    // ToDo: Add more flags...

    // Lock all movement
    private bool bMovementLocked = false;
    // Lock all active mmovement
    private bool bMovable = false;

    private void Awake()
    {
        if (playerRigidBody == null) 
        {
            playerRigidBody = gameObject.GetComponent<Rigidbody>();
        }
        playerControls = new PlayerControls();
        if (player == null)
        {
            player = GetComponent<Player>();
        }
    }

    private void Start()
    {
        // Subscribe to the events
        playerControls.Player.Move.started += move;
        playerControls.Player.Move.performed += move;

        playerControls.Player.Jump.started += jump;
        playerControls.Player.Dash.performed += dash;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Events
    private void move(InputAction.CallbackContext context)
    {
        //Player's position is locked
        if (!bMovementLocked || !bMovable)
        {
            return;
        }
        // Move
        Vector2 inputVector = context.ReadValue<Vector2>();
        Vector3 moveForce = new Vector3(inputVector.x, 0, inputVector.y) * moveRatio;
        if (inputVector.x > 0)
        {
            player.ChangeFacing(false);
        }
        else if (inputVector.x < 0)
        {
            player.ChangeFacing(true);
        }
        playerRigidBody.AddForce(moveForce);
    }

    private void jump(InputAction.CallbackContext context)
    {
        //Player's position is locked
        if (!bMovementLocked || !bMovable)
        {
            return;
        }
        playerRigidBody.AddForce(new Vector3(0f, jumpRatio, 0f));
    }

    private void dash(InputAction.CallbackContext context)
    {
        if (!bMovementLocked || !bMovable)
        {
            return;
        }
        // Dash
    }

    // Stop all movement of the player
    public void StopPlayerMovement()
    {

    }

    // All external classes attampting to move the player shall call this method
    // movement: vector indicating the movement
    // bAvtiveMove: if this movement comes from the player input
    public void MovePlayer(Vector3 movement, bool bActiveMove)
    {
        //Player's position is locked
        if (!bMovementLocked) {
            return;
        }
        // Active move banned
        if (bActiveMove && !bMovable)
        {
            return;
        }
        Vector3 moveForce = movement * moveRatio;
        playerRigidBody.AddForce(moveForce);
    }

    // Force move of the player
    public void RelocatePlayer(Vector3 targetPos)
    {
        
    }

    // Reset flags
    public void ResetPlayerController()
    {
    
    }

    // Change input and lock state
    public void ChangePlayerInputState(bool bState = false)
    {
        bInputLocked = bState;
    }

    public void ChangePlayerMovementState(bool bState = false)
    {
        bMovementLocked = bState;
    }

    public void ChangePlayerActiveMovementState(bool bState = false)
    {
        bMovable = bState;
    }
}