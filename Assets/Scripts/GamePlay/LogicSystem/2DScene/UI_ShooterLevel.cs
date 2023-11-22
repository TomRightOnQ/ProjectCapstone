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
    [SerializeField] private TextMeshProUGUI TB_Score;

    // Public:
    // Init
    public void InitShooterLevelUI(float timeLimit, int scoreGoal)
    {
        TB_Timer.text = timeLimit.ToString();
        TB_CurrentScore.text = "0";
        TB_Score.text = "/" + scoreGoal.ToString();
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
