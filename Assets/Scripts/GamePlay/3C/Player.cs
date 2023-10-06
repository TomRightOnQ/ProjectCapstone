using UnityEngine;

/// <summary>
/// Player Object
/// </summary>
public class Player : EUnit
{
    private PlayerController playerController;
    [SerializeField] private PlayerHUDInteractionTrigger playerInteractionTrigger;

    void Awake()
    {
        // Check Controller's existence
        PlayerController playerController = this.gameObject.GetComponent<PlayerController>();
        if (playerController == null) 
        {
            this.gameObject.AddComponent<PlayerController>();
        }
    }

    // Terminate the player
    // Since the player is not poolable yet not destroyable, we will not inherit the base method
    public override void DeactivateEntity()
    {
        this.gameObject.SetActive(false);
        playerController.ResetPlayerController();
    }
}