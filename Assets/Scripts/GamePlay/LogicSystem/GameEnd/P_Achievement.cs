using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Single Achievement Item
/// </summary>
public class P_Achievement : MonoBehaviour
{
    [SerializeField] public int AchID;

    // UI Widgets
    [SerializeField] private Image Img_Icon;
    [SerializeField] private GameObject Img_Covering;
    [SerializeField] private TextMeshProUGUI TB_AchievementName;
    [SerializeField] private TextMeshProUGUI TB_Detail;

    public void SetUp(int achID)
    {
        AchID = achID;
        AchievementData.AchievementDataStruct achData = AchievementData.GetData(achID);
        // Show the gray covering if not unlocked
        if (!CoreSaveConfig.Instance.AchUnlockList.Contains(achID))
        {
            Img_Covering.SetActive(true);
            // For a hidden achievement that is not unlocked, do not say anything
            if (achData.bHidden)
            {
                Img_Icon.sprite = null;
                TB_AchievementName.text = "???";
                TB_Detail.text = "???";
                return;
            }
        }
        else 
        {
            Img_Covering.SetActive(false);
        }
        // Load info
        Sprite achIcon = ResourceManager.Instance.LoadImage(Constants.IMAGES_SOURCE_PATH, achData.Icon);
        if (achIcon != null)
        {
            Img_Icon.sprite = achIcon;
        }
        TB_AchievementName.text = achData.Name;
        TB_Detail.text = achData.Detail;
    }
}
