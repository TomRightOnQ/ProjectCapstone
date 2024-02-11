using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Panel for remaining time and current/goal score in shooter levels
/// </summary>
public class UI_ShooterLevel : UIBase
{
    // Text fields
    [SerializeField] private TextMeshProUGUI TB_Timer;
    [SerializeField] private TextMeshProUGUI TB_CurrentScore;

    // Public:
    // Init
    public void InitShooterLevelUI(float timeLimit)
    {
        TB_Timer.text = timeLimit.ToString();
        TB_CurrentScore.text = "0";

        EventManager.Instance.PostEvent(GameEvent.Event.EVENT_WIDE_SCREEN_BEGIN);
    }

    // Hide All
    public void HideShooterLevelUI()
    {
        if (gameObject == null)
        {
            return;
        }
        gameObject.SetActive(false);
    }

    // Set Timer
    public void UpdateTimer(float time)
    {
        if (Mathf.Round(time) <= 10f)
        {
            TB_Timer.color = Color.red;
        }
        TB_Timer.text = Mathf.Round(time).ToString();
    }

    // Set Score
    public void UpdateScore(int time)
    {
        TB_CurrentScore.text = time.ToString();
    }
}
