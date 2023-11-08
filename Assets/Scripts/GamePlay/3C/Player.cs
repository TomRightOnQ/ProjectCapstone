using UnityEngine;

/// <summary>
/// Player Object
/// </summary>
public class Player : EUnit
{
    [SerializeField] private PlayerController playerController;
    public PlayerController Controller => playerController;

    [SerializeField] private PlayerHUDInteractionTrigger playerInteractionTrigger;
    public PlayerHUDInteractionTrigger MainPlayerInteractionTrigger => playerInteractionTrigger;

    protected override void Awake()
    {
        base.Awake();
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

    // Update HP to UIs
    protected override void checkCurrentHealth()
    {
        base.checkCurrentHealth();
        BattleObserver.Instance.OnPlayerHPChanged(currentHP, maxHP);
    }
}