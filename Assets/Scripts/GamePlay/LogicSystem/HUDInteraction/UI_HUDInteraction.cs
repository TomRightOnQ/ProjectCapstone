using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI Logic of the interaction list on HUD
/// </summary>
public class UI_HUDInteraction : MonoBehaviour
{
    // public:
    // Hide Interactions
    public void EndChat()
    {
        gameObject.SetActive(false);
    }

    // Show Interactions
    public void StartChat(int chatID)
    {
        gameObject.SetActive(true);
    }
}
