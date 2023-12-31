using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Scriptable Singleton observer of 2D scene
/// </summary>
[CreateAssetMenu(menuName = "Observer/BattleObserver")]
public class BattleObserver : ScriptableSingleton<BattleObserver>
{
    [SerializeField, ReadOnly]
    private bool bGameStarted = false;
    public bool BGameStarted => bGameStarted;

    // Begin and end the game
    public void BeginGame()
    {
        if (bGameStarted)
        {
            return;
        }
        bGameStarted = true;
    }

    public void EndGame()
    {
        if (!bGameStarted)
        {
            return;
        }
        bGameStarted = false;
    }

    // Player HP
    public void OnPlayerHPChanged(float value, float maxValue)
    {
        // Update the HP to HUD
        HUDManager.Instance.A_Recv_PlayerHPChanged(value, maxValue);
    }
}
