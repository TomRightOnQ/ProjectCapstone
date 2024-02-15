using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LevelEvent that bans the player from firing, and adds points per second
/// </summary>
public class LevelEvent_ScorePerSec : LevelEvent
{
    [SerializeField] private int pointPerSec = 0;

    protected override void levelEvent()
    {
        InputManager.Instance.SetInputAsShooter(false);
        // Begin to yield point
        if (ShooterLevelManager.Instance != null)
        {
            StartCoroutine(givePoints());
            StartCoroutine(giveHints());
        }
    }

    private IEnumerator giveHints()
    {
        ReminderManager.Instance.ShowSubtitleReminder(13);
        yield return new WaitForSeconds(3.5f);
        ReminderManager.Instance.ShowSubtitleReminder(14);
        yield return new WaitForSeconds(3.5f);
        ReminderManager.Instance.ShowSubtitleReminder(15);
    }

    private IEnumerator givePoints()
    {
        while (gameObject.activeSelf)
        {
            // Wait for one second
            yield return new WaitForSeconds(1f);
            ShooterLevelManager.Instance.AddScore(pointPerSec);
        }
    }
}
