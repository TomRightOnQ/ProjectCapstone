using UnityEngine.UI;
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

    // Skill CD Wheel
    [SerializeField] private GameObject P_SkillCD;
    [SerializeField] private Slider S_CDWheel;

    // Animator
    [SerializeField] private Animator animator;

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

    // Show/Hide Skill CD
    public void SetSkillCDSlider(bool bShow)
    {
        P_SkillCD.SetActive(bShow);
    }

    // Set SliderCD
    public void UpdateSkillCD(float currentCD, float maxCD)
    {
        S_CDWheel.value = currentCD / maxCD;
        if (currentCD >= maxCD)
        {
            animator.Play("SkillCDReady");
        }
    }
}
