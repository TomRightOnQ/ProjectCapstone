using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers - Controls the chat and corresponding interaction with NPCs
/// </summary>
public class ChatInteractionManager : MonoBehaviour
{
    private static ChatInteractionManager instance;
    public static ChatInteractionManager Instance => instance;

    // Flags
    private bool bInChat = false;

    // UI Components
    [SerializeField] private UI_ChatInteraction ui_ChatInteraction;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        if (ui_ChatInteraction == null)
        {

            GameObject uiObject = UIManager.Instance.CreateUI("UI_ChatInteraction");
            ui_ChatInteraction = uiObject.GetComponent<UI_ChatInteraction>();
            uiObject.SetActive(false);
        }
    }


    // Public:
    // Begin the interaction
    public void BeginInteraction(int chatID)
    {
        if (bInChat)
        {
            Debug.Log("ChatInteractionManager: Chatting");
            return;
        }
        UIManager.Instance.ShowUI("UI_ChatInteraction");
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_CHAT_BEGIN);
        ui_ChatInteraction.StartChat(chatID);
        bInChat = true;

        // Hide HUD
        HUDManager.Instance.HideAllHUD();
    }

    // End the current interaction
    public void EndInteraction()
    {
        if (!bInChat)
        {
            Debug.Log("ChatInteractionManager: No chat going on");
            return;
        }
        ui_ChatInteraction.EndChat();
        UIManager.Instance.HideUI("UI_ChatInteraction");
        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_CHAT_END);
        bInChat = false;

        // Show HUD
        HUDManager.Instance.ShowAllHUD();
    }
}
