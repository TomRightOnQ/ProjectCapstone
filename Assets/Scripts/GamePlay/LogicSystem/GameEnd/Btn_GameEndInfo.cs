using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Button of each game end
/// </summary>
public class Btn_GameEndInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // End ID
    [SerializeField, ReadOnly] private int buttonGameEndID = -1;

    [SerializeField, ReadOnly] private bool bInteractable = false;

    // UI Widgets
    [SerializeField] private TextMeshProUGUI TB_GameEndName;
    [SerializeField] private Button Btn_Button;

    [SerializeField] private GameObject P_EndDetail;
    [SerializeField] private TextMeshProUGUI TB_EndDetail;

    // SetUp
    public void SetUp(int gameEndID = -1)
    {
        // Locked by default
        if (gameEndID == -1)
        {
            bInteractable = false;
            TB_GameEndName.text = "???";
            return;
        }
        bInteractable = true;
        GameEndData.GameEndDataStruct gameEndData = GameEndData.GetData(gameEndID);
        TB_GameEndName.text = gameEndData.Name;
        TB_EndDetail.text = gameEndData.Detail;
        buttonGameEndID = gameEndID;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bInteractable)
        {
            P_EndDetail.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        P_EndDetail.SetActive(false);
    }
}
