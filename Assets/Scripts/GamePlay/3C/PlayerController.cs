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

    private Vector2 moveInput;

    // Data
    [SerializeField] private float moveRatio = 1f;
    [SerializeField] private float jumpRatio = 1f;
    [SerializeField] private float dashRatio = 1f;
    [SerializeField] private Rigidbody playerRigidBody;

    // Flags
    private bool bAirBorne = false;
    private bool bDash = false;

    // Lock input from keyboard
    [SerializeField] private bool bInputLocked = false;
    // Lock attack movement
    // Lock all movement
    [SerializeField] private bool bMovementLocked = false;
    // Lock all active mmovement
    [SerializeField] private bool bMovable = false;

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
        playerControls.Player.Move.performed += move;
        playerControls.Player.Move.canceled += stopMove;

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

    private void FixedUpdate()
    {
        // If player's position is not locked and movable
        if (!bMovementLocked || !bMovable)
        {
            return;
        }

        // Apply continuous force based on moveInput
        if (moveInput != Vector2.zero && !bAirBorne)
        {
            Vector3 force = new Vector3(moveInput.x * moveRatio, 0, moveInput.y * moveRatio) * player.GetUnitSpeed();
            playerRigidBody.AddForce(force);
        }

    }

    // Events
    private void move(InputAction.CallbackContext context)
    {
        // Update moveInput with the current input value
        moveInput = context.ReadValue<Vector2>();
        // Flip the player
        if (moveInput.x < 0)
        {
            player.ChangeFacing(true);
        }
        else if (moveInput.x > 0)
        {
            player.ChangeFacing(false);
        }
    }

    private void stopMove(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void jump(InputAction.CallbackContext context)
    {
        //Player's position is locked
        if (bAirBorne || bDash || !bMovementLocked || !bMovable)
        {
            return;
        }
        // playerRigidBody.AddForce(new Vector3(0f, jumpRatio, 0f));
    }

    private void dash(InputAction.CallbackContext context)
    {
        if (bAirBorne || bDash || !bMovementLocked || !bMovable)
        {
            return;
        }
        // Dash
        float dashDirectionX = player.GetFacingToRight() ? 1f : -1f;
        playerRigidBody.AddForce(new Vector3(dashRatio * dashDirectionX, 0f, 0f));
        bDash = true;
        Invoke("stopDash", Constants.DASH_CD_PLAYER);
    }

    private void stopDash()
    {
        bDash = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        bAirBorne = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        bAirBorne = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bAirBorne = false;
    }

    // Methods
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
        Vector3 moveForce = movement * player.GetUnitSpeed();
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