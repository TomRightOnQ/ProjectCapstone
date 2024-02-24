using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

/// <summary>
/// UI Components for the game's ending
/// This UI should be placed at the top layer
/// </summary>
public class UI_GameEnd : UIBase
{
    // UI Widgets
    [SerializeField] private TextMeshProUGUI TB_GameEndText;
    [SerializeField] private TextMeshProUGUI TB_EndGame;
    [SerializeField] private TextMeshProUGUI TB_GameEndDetails;

    [SerializeField] private Button Btn_ReturnToMorning;
    [SerializeField] private Button Btn_Continue;

    // Animator
    [SerializeField] private Animator gameEndAnimator;

    // SetUp a game end
    public void SetUp(int gameEndID)
    {
        GameEndData.GameEndDataStruct gameEndData = GameEndData.GetData(gameEndID);
        TB_EndGame.text = gameEndData.Name;
        TB_GameEndDetails.text = gameEndData.Detail;
        StartCoroutine(showButtons(gameEndData.bTrueEnd));
    }

    private IEnumerator showButtons(bool bTrueEnd)
    {
        yield return new WaitForSeconds(4f);
        // Play Animation
        if (!bTrueEnd)
        {
            Btn_ReturnToMorning.gameObject.SetActive(true);
        }
        else
        {
            Btn_Continue.gameObject.SetActive(true);
        }
    }

    // OnClick Event
    public void OnClick_Btn_ReturnToMorning()
    {
        DayCycleManager.Instance.JumpToDay(DayCycleManager.Instance.CurrentDay);
    }

    public void OnClick_Btn_Continue()
    {
        DayCycleManager.Instance.JumpToDay(DayCycleManager.Instance.CurrentDay);
    }
}
