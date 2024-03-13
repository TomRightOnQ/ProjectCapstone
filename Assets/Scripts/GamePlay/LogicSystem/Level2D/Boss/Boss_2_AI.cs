using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss_2 Behavior
/// </summary>
public class Boss_2_AI : MonoBehaviour
{
    [SerializeField] private bool bAIActivated = false;

    [SerializeField] private GameObject boss;
    [SerializeField] private Boss_2 bossComponent;

    [SerializeField] private float moveForce = 100f;
    [SerializeField] private float jumpForce = 1000f;

    // The minimum time between movements
    [SerializeField] private float movementCooldown = 0.33f;
    [SerializeField, ReadOnly] private float movementTimer = 0f;

    // References Groups
    [SerializeField] private GameObject mainPlayer;
    [SerializeField] private Rigidbody bossRigidBody;

    // Positions
    [SerializeField, ReadOnly] private float previousXDistance = 0f;
    [SerializeField, ReadOnly] private float currentXDistance = 0f;

    private void Awake()
    {
        // Subscribe to event C_2 as the boss beginning signal
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_C_2, beginAI);
    }

    private void FixedUpdate()
    {
        if (bAIActivated)
        {
            // Update the timer
            movementTimer += Time.fixedDeltaTime;

            // Check if the movement cooldown has elapsed
            if (movementTimer >= movementCooldown)
            {
                // Chance to jump if the height is different
                 if (Mathf.Abs(mainPlayer.transform.position.y - boss.transform.position.y) > 0.6f && boss.transform.position.y <= -1.65f)
                 {
                    int jumpChance = Random.Range(0, 2);
                    if (jumpChance == 0)
                    {
                        bossRigidBody.AddForce(new Vector3(0, jumpForce, 0));
                    }
                 }

                bool bMoveLeft = calculateMovementDirection();
                bossComponent.ChangeFacing(mainPlayer.transform.position.x - transform.position.x <= 0);
                if (bMoveLeft)
                {
                    bossRigidBody.AddForce(new Vector3(-moveForce, 0, 0));
                }
                else
                {
                    bossRigidBody.AddForce(new Vector3(moveForce, 0, 0));
                }

                // Reset the timer after making a movement decision
                movementTimer = 0f;
            }
            previousXDistance = currentXDistance;
            currentXDistance = mainPlayer.transform.position.x - boss.transform.position.x;
        }
    }


    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvent.Event.EVENT_C_2, beginAI);
    }

    // Private:
    // Mark as activated
    private void beginAI()
    {
        bAIActivated = true;
        mainPlayer = PersistentDataManager.Instance.MainPlayer.gameObject;
        // Calculate distance for the first time
        currentXDistance = mainPlayer.transform.position.x - boss.transform.position.x;
    }

    // Calculate movement position
    // When player approaches, move away
    // return: true if moving toward positive X (right)
    private bool calculateMovementDirection()
    {
        bool isPlayerToLeft = currentXDistance < 0;

        bool isGettingCloser = Mathf.Abs(currentXDistance) < Mathf.Abs(previousXDistance);
        if ((Mathf.Abs(currentXDistance - previousXDistance) < 0.1f && Mathf.Abs(currentXDistance) > 6f))
        {
            return isPlayerToLeft;
        }

        if (Mathf.Abs(currentXDistance) < 1f)
        {
            return !isPlayerToLeft;
        }

        if (isGettingCloser)
        {
            return !isPlayerToLeft;
        }
        else
        {
            return isPlayerToLeft;
        }
    }
}
