using UnityEngine;
using UnityEngine.InputSystem;
using Spine.Unity;

/// <summary>
/// Attching this component to the player for control
/// </summary>

public class PlayerController : MonoBehaviour
{
    // Core: Player Control class and the player
    private PlayerControls playerControls;
    [SerializeField] private Player player;

    // Animation Controllers
    SkeletonAnimation skeletonAnimation;

    private Spine.Skeleton skeleton;
    private Spine.AnimationState spineAnimationState;
    private Vector2 moveInput;

    [SerializeField] private string IdleAnim = "Idle";
    [SerializeField] private string WalkAnim = "Walk";
    [SerializeField] private string currentState = "Idle";

    // Data
    [SerializeField] private float moveRatio = 1f;
    [SerializeField] private float jumpRatio = 1f;
    [SerializeField] private float dashRatio = 1f;
    [SerializeField] private Rigidbody playerRigidBody;

    // Flags
    private bool bAirBorne = false;
    private bool bDash = false;

    // Mode Switcher
    [SerializeField] private bool bWorld = true;

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

    private void init(bool bWorldMode = true)
    {
        bWorld = bWorldMode;

        // Subscribe to the events
        playerControls.Player.Move.performed += move;
        playerControls.Player.Move.canceled += stopMove;

        if (!bWorld)
        {
            playerControls.Player.Jump.started += jump;
            playerControls.Player.Dash.performed += dash;
        }

        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        if (skeletonAnimation != null)
        {
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;
        }
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
        if (!bMovementLocked || !bMovable || PersistentGameManager.Instance.bGamePaused)
        {
            return;
        }

        Vector3 force = Vector3.zero;

        if (moveInput.magnitude != 0)
        {
            force = new Vector3(moveInput.x * moveRatio, 0, moveInput.y * moveRatio) * player.GetUnitSpeed();
            UpdateAnimationState(WalkAnim);
        }
        else
        {
            UpdateAnimationState(IdleAnim);
            
        }
        playerRigidBody.AddForce(force);
    }

    private void UpdateAnimationState(string state)
    {
        if (currentState == WalkAnim && state == IdleAnim)
        {
            spineAnimationState.SetAnimation(0, IdleAnim, true);
            spineAnimationState.TimeScale = 1;
            currentState = IdleAnim;
        } 
        else if (currentState == IdleAnim && state == WalkAnim) 
        {
            spineAnimationState.SetAnimation(0, WalkAnim, true);
            spineAnimationState.TimeScale = 1.5f;
            currentState = WalkAnim;
        }
    }

    // Events
    private void move(InputAction.CallbackContext context)
    {
        if (PersistentGameManager.Instance.bGamePaused)
        {
            return;
        }
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
        if (bWorld || bAirBorne || bDash || !bMovementLocked || !bMovable || PersistentGameManager.Instance.bGamePaused)
        {
            return;
        }
        playerRigidBody.AddForce(new Vector3(0f, jumpRatio, 0f));
    }

    private void dash(InputAction.CallbackContext context)
    {
        if (bWorld ||¡¡bAirBorne || bDash || !bMovementLocked || !bMovable || PersistentGameManager.Instance.bGamePaused)
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

    // Set mode
    public void SetPlayerMode(Enums.SCENE_TYPE worldType)
    {
        if (worldType == Enums.SCENE_TYPE.World)
        {
            bWorld = true;
        }
        else 
        {
            bWorld = false;
        }
        init(bWorld);
    }

    // All external classes attampting to move the player shall call this method
    // movement: vector indicating the movement
    // bAvtiveMove: if this movement comes from the player input
    public void MovePlayer(Vector3 movement, bool bActiveMove)
    {
        //Player's position is locked
        if (!bMovementLocked)
        {
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