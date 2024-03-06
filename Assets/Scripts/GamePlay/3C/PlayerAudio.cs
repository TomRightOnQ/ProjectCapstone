using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Controller of player's sound
/// </summary>
public class PlayerAudio : MonoBehaviour
{
    // Surface Type
    [SerializeField] private string surfaceType = "Carpet";

    // Event Name
    [SerializeField, ReadOnly] private string WALK_EVENT_NAME = "event:/Character/Player Footsteps";
    [SerializeField, ReadOnly] private string HIT_EVENT_NAME = "event:/Hits/Player Hit";

    // FMOD event instance
    private EventInstance foosteps;
    private EventInstance playerHit;

    // Time Config
    [SerializeField] private float walkTimeDiff = 0.5f;
    float timer = 1f;

    [SerializeField, ReadOnly] private bool bWalking = false;

    // Player RigidBody
    [SerializeField] private Rigidbody playerRigidBody;

    private void Update()
    {
        if (bWalking)
        {
            if (timer > walkTimeDiff)
            {
                PlayerWalk();
                timer = 0.0f;
            }

            timer += Time.deltaTime;
        }
    }

    // Public:
    // Walk
    public void SetWalk()
    {
        // Only if player moving
        if (playerRigidBody.velocity.magnitude > 0.05)
        {
            bWalking = true;
        }
    }

    public void SetIdle()
    {
        bWalking = false;
    }

    public void StartWalk()
    {
        if (!bWalking)
        {
            // Only if player moving
            if (playerRigidBody.velocity.magnitude > 0.05)
            {
                timer = 1f;
                bWalking = true;
            }
        }
    }

    public void StopWalk()
    {

        if (bWalking)
        {
            timer = 1f;
            bWalking = false;
            foosteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void PlayerWalk()
    {
        foosteps = RuntimeManager.CreateInstance(WALK_EVENT_NAME);
        foosteps.setParameterByName("Surface", 2);
        foosteps.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }

    public void PlayerHit()
    {
        playerHit = RuntimeManager.CreateInstance(HIT_EVENT_NAME);
        playerHit.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        playerHit.start();
        playerHit.release();
    }
}
