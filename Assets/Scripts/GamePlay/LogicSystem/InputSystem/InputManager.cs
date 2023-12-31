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
            configEventHandler();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Private:
    private void configEventHandler()
    {
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_CHAT_BEGIN, On_Recv_ChatBegin);
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_CHAT_END, On_Recv_ChatEnd);
    }

    // Evnet Handlers
    private void On_Recv_ChatBegin()
    {
        LockInput();
    }

    private void On_Recv_ChatEnd()
    {
        UnLockInput(LevelManager.Instance.CurrentSceneType);
    }

    // Public:
    public void SetController(PlayerController controller)
    {
        playerController = controller;
    }

    // Unlock player input
    public void UnLockInput(Enums.SCENE_TYPE worldType, Enums.LEVEL_TYPE levelType = Enums.LEVEL_TYPE.None)
    {
        if (playerController == null)
        {
            return;
        }
        playerController.UnlockPlayerInput();
        playerController.SetPlayerMode(worldType, levelType);
    }

    // Change Player Input mode to shooter
    public void SetInputAsShooter()
    {
        if (playerController == null)
        {
            return;
        }
        playerController.SetAsShooter();
    }

    // Lock Player input
    public void LockInput()
    {
        if (playerController == null)
        {
            return;
        }
        playerController.LockPlayerInput();
    }
}
