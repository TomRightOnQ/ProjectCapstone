using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Interaction panel logic
/// </summary>
public class UI_ChatInteraction : UIBase
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject p_ChatChoicePanel;
    [SerializeField] private GameObject btn_ClickAny;

    [SerializeField] private TextMeshProUGUI TB_ChatSepaker;
    [SerializeField] private TextMeshProUGUI TB_ChatContent;

    [SerializeField, ReadOnly]
    private int currentInteractionID = 0;

    private ChatInteractionData.ChatInteractionDataStruct currentInteraction;

    // Private:
    // Config current chat
    private void refreshChat(int chatID)
    {
        currentInteractionID = chatID;
        currentInteraction = ChatInteractionData.GetData(chatID);
        btn_ClickAny.SetActive(false);
        p_ChatChoicePanel.SetActive(false);

        // Set Chat text
        TB_ChatSepaker.text = currentInteraction.Speaker;
        TB_ChatContent.text = currentInteraction.Content;

        switch (currentInteraction.Action)
        {
            case Enums.INTERACTION_TYPE.Choice:
                configChoices();
                break;
            case Enums.INTERACTION_TYPE.Next:
                btn_ClickAny.SetActive(true);
                break;
            case Enums.INTERACTION_TYPE.End:
                btn_ClickAny.SetActive(true);
                break;
            case Enums.INTERACTION_TYPE.None:
                btn_ClickAny.SetActive(true);
                break;
            default:
                ChatInteractionManager.Instance.EndInteraction();
                break;
        }
    }

    // Config choices
    private void configChoices()
    {
        p_ChatChoicePanel.SetActive(true);
        for (int i = 0; i < currentInteraction.Target.Length; i++)
        {
            GameObject child = content.GetChild(i).gameObject;
            // Set choice text
            TextMeshProUGUI textMesh = child.GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
            {
                int choiceID = currentInteraction.Target[i];
                textMesh.text = ChatInteractionData.GetData(choiceID).Content;
            }
            child.SetActive(true);
        }
    }

    // Jump to choice
    private void jumpToChoice(int targetID)
    {
        refreshChat(targetID);
    }

    // Process OnClick
    private void onClickEvent(int index)
    {
        // Turn off the choices
        p_ChatChoicePanel.SetActive(false);

        int choiceID = currentInteraction.Target[index];
        ChatInteractionData.ChatInteractionDataStruct choiceInteraction = ChatInteractionData.GetData(choiceID);

        if (choiceInteraction == null)
        {
            ChatInteractionManager.Instance.EndInteraction();
        }

        // Process Event
        switch (choiceInteraction.Event)
        {
            case Enums.INTERACTION_EVENT.CompleteTask:
                TaskManager.Instance.CompleteTasks(choiceInteraction.EventTarget);
                break;
            default:
                break;
        }
        // Process Action
        switch (choiceInteraction.Action)
        {
            case Enums.INTERACTION_TYPE.Next:
                jumpToChoice(choiceInteraction.Target[0]);
                break;
            case Enums.INTERACTION_TYPE.End:
                ChatInteractionManager.Instance.EndInteraction();
                return;
            case Enums.INTERACTION_TYPE.Claim:
                ChatInteractionManager.Instance.EndInteraction();
                break;
            case Enums.INTERACTION_TYPE.None:
                break;
            case Enums.INTERACTION_TYPE.Choice:
                refreshChat(choiceInteraction.Target[index]);
                break;
            default:
                ChatInteractionManager.Instance.EndInteraction();
                break;
        }

        // Force stop check
        if (choiceInteraction.bEnd)
        {
            ChatInteractionManager.Instance.EndInteraction();
        }
    }

    private void onClickNext()
    {
        // Turn off the choices
        p_ChatChoicePanel.SetActive(false);

        if (currentInteraction == null)
        {
            ChatInteractionManager.Instance.EndInteraction();
        }
        // Process Event
        switch (currentInteraction.Event)
        {
            case Enums.INTERACTION_EVENT.CompleteTask:
                TaskManager.Instance.CompleteTasks(currentInteraction.EventTarget);
                break;
            default:
                break;
        }
        // Process Action
        switch (currentInteraction.Action)
        {
            case Enums.INTERACTION_TYPE.Next:
                jumpToChoice(currentInteraction.Target[0]);
                break;
            case Enums.INTERACTION_TYPE.End:
                ChatInteractionManager.Instance.EndInteraction();
                return;
            case Enums.INTERACTION_TYPE.Claim:
                ChatInteractionManager.Instance.EndInteraction();
                break;
            case Enums.INTERACTION_TYPE.None:
                break;
            default:
                ChatInteractionManager.Instance.EndInteraction();
                break;
        }
    }

    public void EndChat()
    {
        currentInteraction = null;
        for (int i = 0; i < 5; i++)
        {
            GameObject child = content.GetChild(i).gameObject;
            child.SetActive(false);
        }
    }

    // Public:
    // Begin a chat
    public void StartChat(int chatID)
    {
        refreshChat(chatID);
    }

    // OnClick Events
    public void OnClick_Next()
    {
        onClickNext();
    }

    public void OnClick_Choice1()
    {
        onClickEvent(0);
    }

    public void OnClick_Choice2()
    {
        onClickEvent(1);
    }

    public void OnClick_Choice3()
    {
        onClickEvent(2);
    }

    public void OnClick_Choice4()
    {
        onClickEvent(3);
    }

    public void OnClick_Choice5()
    {
        onClickEvent(4);
    }
}
