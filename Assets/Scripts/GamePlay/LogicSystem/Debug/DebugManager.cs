using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GM Debug tool
/// </summary>
public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject p_DebugPanel;
    [SerializeField] private GameObject btn_DebugMenuButton;

    private void onClick()
    {
        p_DebugPanel.SetActive(false);
        btn_DebugMenuButton.SetActive(true);
    }

    public void ShowChat()
    {
        ChatInteractionManager.Instance.BeginInteraction(1);
        onClick();
    }
}
