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

    // Player Audio Component
    [SerializeField] private PlayerAudio playerAudio;

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
    // Switch control to Omni-Direction
    [SerializeField] private bool bAllDirectionMovement = false;

    // Shooter Configurations
    [SerializeField] private float fireLineSize = 0.2f;
    [SerializeField] private float fireLineZ = 1.3f;

    // Missile Configuratiojn
    [SerializeField, ReadOnly]
    private float aimAngle = 45f;
    [SerializeField, ReadOnly]
    private bool bMissile = false;
    [SerializeField, ReadOnly]
    private float minRange = 1f;
    [SerializeField, ReadOnly]
    private float maxRange = 30f;
    [SerializeField, ReadOnly]
    private Transform targetTransform;
    protected LayerMask proxTargetLayerMask;

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
        if (playerAudio == null)
        {
            playerAudio = GetComponent<PlayerAudio>();
        }
        mainCamera = FindFirstObjectByType<Camera>();
        proxTargetLayerMask = 1 << LayerMask.NameToLayer("Target");
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

            playerControls.Player.Skill.started += skill;
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
        if (bInputLocked || bMovementLocked || PersistentGameManager.Instance.bGamePaused)
        {
            return;
        }

        // Config movement
        Vector3 force = Vector3.zero;
        if (moveInput.magnitude != 0)
        {
            float speedRatio = 1f;
            if (bAirBorne) 
            {
                speedRatio = 0.5f;
            }
            // Movement direction based on the scene
            if (bWorld || !bAllDirectionMovement)
            {
                // Case: Regular Move
                force = new Vector3(moveInput.x * moveRatio, 0, moveInput.y * moveRatio) * player.GetUnitAccel() * speedRatio;
            }
            else 
            {
                // Case: All Direction Move
                force = new Vector3(moveInput.x * moveRatio, moveInput.y * moveRatio, 0) * player.GetUnitAccel() * speedRatio;
            }

            UpdateAnimationState(WalkAnim);
        }
        else
        {
            UpdateAnimationState(IdleAnim);

        }
        playerRigidBody.AddForce(force);

        UpdateMovementAudioState();

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
        if (bInputLocked || PersistentGameManager.Instance.bGamePaused)
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
        if (!bShooter)
        {
            return;
        }
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

        // Config Missile
        if (bMissile)
        {
            FindTarget();
        }

        // Config Firing
        if (Input.GetMouseButton(0))
        {
            player.FireProjectile(directionToCursor, targetTransform);
        }
    }

    private void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxRange, proxTargetLayerMask);
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToCollider = (hitCollider.transform.position - transform.position).normalized;
            float angleToCollider = Vector3.Angle(transform.up, directionToCollider);

            if (angleToCollider <= aimAngle)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance && distance > minRange)
                {
                    closestDistance = distance;
                    closestTarget = hitCollider.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            targetTransform = closestTarget;

        }
        else 
        {

        }
    }

    // Manage animation and audio
    private void UpdateAnimationState(string state)
    {
        if (currentState == WalkAnim && state == IdleAnim)
        {
            spineAnimationState.SetAnimation(0, IdleAnim, true);
            spineAnimationState.TimeScale = 1;
            currentState = IdleAnim;
            playerAudio.StopWalk();
        } 
        else if (currentState == IdleAnim && state == WalkAnim) 
        {
            spineAnimationState.SetAnimation(0, WalkAnim, true);
            spineAnimationState.TimeScale = 1.5f;
            currentState = WalkAnim;
            playerAudio.StartWalk();
        }
    }

    private void UpdateMovementAudioState()
    {
        if (bAirBorne)
        {
            playerAudio.StopWalk();
            return;
        }
        if (currentState == WalkAnim)
        {
            playerAudio.SetWalk();
        }
        else if (currentState == IdleAnim)
        {
            playerAudio.SetIdle();
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
        // No jumping if the input is locked or players are in all direction mode
        if (bMovementLocked || bAllDirectionMovement)
        {
            return;
        }
        if (PersistentGameManager.Instance.bGamePaused || bWorld || bAirBorne || bDash)
        {
            return;
        }
        playerRigidBody.AddForce(new Vector3(0f, jumpRatio, 0f));
    }

    private void dash(InputAction.CallbackContext context)
    {
        if (bMovementLocked)
        {
            return;
        }
        if (PersistentGameManager.Instance.bGamePaused || bWorld || bAirBorne || bDash)
        {
            return;
        }
        // Dash
        float dashDirectionX = player.GetFacingToRight() ? 1f : -1f;
        playerRigidBody.AddForce(new Vector3(dashRatio * dashDirectionX, 0f, 0f));
        bDash = true;
        Invoke("stopDash", Constants.DASH_CD_PLAYER);
    }

    private void skill(InputAction.CallbackContext context)
    {
        if (bMovementLocked || !bShooter)
        {
            return;
        }
        if (PersistentGameManager.Instance.bGamePaused || bWorld)
        {
            return;
        }

        // Skill
        player.UseSkill();
    }

    private void stopDash()
    {
        bDash = false;
    }

    // Public:
    public void ResetJumping()
    {
        bAirBorne = false;
    }

    public void LockJumping()
    {
        bAirBorne = true;
    }

    // Methods
    // Stop all movement of the player
    public void StopPlayerMovement()
    {
        playerRigidBody.velocity = Vector3.zero;
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

    public void SetAsAllDirectionMove(bool bCanMoveAll = false)
    {
        bAllDirectionMovement = bCanMoveAll;
        // Change regidbody to remove gravity
        if (bCanMoveAll)
        {
            playerRigidBody.useGravity = false;
        }
        else 
        {
            playerRigidBody.useGravity = true;
        }
    }

    public void SetAsShooter(bool bActive)
    {
        bShooter = bActive;
        if (bShooter)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
            }

            // Check weapon type
            ProjectileData.ProjectileDataStruct projData = ProjectileData.GetData(player.ProjectileID);
            aimAngle = projData.LockAngle;
            bMissile = projData.bGuided;
        }
        else 
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        }
    }

    // Lock All Inputs
    public void LockPlayerInput()
    {
        bInputLocked = true;
        ChangePlayerMovementLockState(true);
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
        ChangePlayerMovementLockState(false);
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
    public void ChangePlayerMovementLockState(bool bState = false)
    {
        bMovementLocked = bState;
    }
}