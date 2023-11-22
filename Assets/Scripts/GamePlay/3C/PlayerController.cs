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

    // MainCamera
    [SerializeField, ReadOnly]
    private Camera mainCamera;

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
    // Lock all movement
    [SerializeField] private bool bMovementLocked = false;
    // Control fire mode
    [SerializeField] private bool bShooter = false;

    // Shooter Configurations
    [SerializeField] private float fireLineSize = 0.2f;
    [SerializeField] private float fireLineZ = 1.3f;
    // Prev Renderer
    private LineRenderer lineRenderer;

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
        mainCamera = FindFirstObjectByType<Camera>();
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
        if (bInputLocked || !bMovementLocked || PersistentGameManager.Instance.bGamePaused)
        {
            return;
        }

        // Config movement
        Vector3 force = Vector3.zero;
        if (moveInput.magnitude != 0 && !bAirBorne)
        {
            force = new Vector3(moveInput.x * moveRatio, 0, moveInput.y * moveRatio) * player.GetUnitAccel();
            UpdateAnimationState(WalkAnim);
        }
        else if (moveInput.magnitude != 0 && bAirBorne)
        {
            force = new Vector3(moveInput.x * moveRatio, 0, moveInput.y * moveRatio) * player.GetUnitAccel() * 0.5f;
            UpdateAnimationState(WalkAnim);
        }
        else
        {
            UpdateAnimationState(IdleAnim);

        }
        playerRigidBody.AddForce(force);

        // Clamp the max speed
        Vector3 horizontalVelocity = playerRigidBody.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.magnitude > player.GetUnitSpeed())
        {
            horizontalVelocity = horizontalVelocity.normalized * player.GetUnitSpeed();
            playerRigidBody.velocity = new Vector3(horizontalVelocity.x, playerRigidBody.velocity.y, horizontalVelocity.z);
        }
    }

    private void Update()
    {
        if (bInputLocked)
        {
            return;
        }
        // Config Shooting and Attacking
        if (bShooter)
        {
            AimCursor();
        }
    }

    // Config firing Angle
    private void AimCursor()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 playerPos = player.transform.position;
        playerPos.z = fireLineZ;
        worldPosition.z = fireLineZ;

        Vector3 directionToCursor = (worldPosition - playerPos).normalized;

        // Config fireline preview
        float lineLength = (worldPosition - playerPos).magnitude * fireLineSize;
        Vector3 startLine = playerPos;
        Vector3 endLine = startLine + directionToCursor * lineLength;

        lineRenderer.SetPosition(0, startLine);
        lineRenderer.SetPosition(1, endLine);

        // Config Firing
        if (Input.GetMouseButton(0))
        {
            player.FireProjectile(directionToCursor);
        }
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
        if (bWorld || bAirBorne || bDash || !bMovementLocked || PersistentGameManager.Instance.bGamePaused)
        {
            return;
        }
        playerRigidBody.AddForce(new Vector3(0f, jumpRatio, 0f));
    }

    private void dash(InputAction.CallbackContext context)
    {
        if (bWorld ||¡¡bAirBorne || bDash || !bMovementLocked || PersistentGameManager.Instance.bGamePaused)
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
    public void SetPlayerMode(Enums.SCENE_TYPE worldType, Enums.LEVEL_TYPE levelType = Enums.LEVEL_TYPE.None)
    {
        bWorld = false;
        if (worldType == Enums.SCENE_TYPE.World)
        {
            bWorld = true;
        }
        switch (levelType)
        {
            case Enums.LEVEL_TYPE.Shooter:
                bShooter = true;
                break;
            case Enums.LEVEL_TYPE.None:
                break;
        }
        init(bWorld);
    }

    public void SetAsShooter()
    {
        bShooter = true;
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }
    }

    // Lock All Inputs
    public void LockPlayerInput()
    {
        bInputLocked = true;
        ChangePlayerMovementState(false);
        bShooter = false;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    // Unlock All Inputs
    public void UnlockPlayerInput()
    {
        bInputLocked = false;
        ChangePlayerMovementState(true);
    }

    // All external classes attampting to move the player shall call this method
    // movement: vector indicating the movement
    // bAvtiveMove: if this movement comes from the player input
    public void MovePlayer(Vector3 movement)
    {
        //Player's position is locked
        if (!bMovementLocked)
        {
            return;
        }

        Vector3 moveForce = movement * player.GetUnitSpeed();
        playerRigidBody.AddForce(moveForce);
    }

    // Reset flags
    public void ResetPlayerController()
    {

    }

    // Change input and lock state
    public void ChangePlayerMovementState(bool bState = false)
    {
        bMovementLocked = bState;
    }
}