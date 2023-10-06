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
            ui_ChatInteraction = UIManager.Instance.CreateUI("UI_ChatInteraction").GetComponent<UI_ChatInteraction>();
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
        ui_ChatInteraction.StartChat(chatID);
        bInChat = true;
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
        bInChat = false;
    }
}
