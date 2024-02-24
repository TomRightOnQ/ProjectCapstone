using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The game ends and achievements page
/// </summary>
public class UI_GameAch : UIBase
{
    // Achievement List
    [SerializeField] private ScrollRect SV_AchievementList;

    // List of GameEndInfo Buttons
    [SerializeField] private List<Btn_GameEndInfo> endInfoList = new List<Btn_GameEndInfo>();

    // MainPanel
    [SerializeField] private GameObject P_GameAchPanel;

    // Record if already inited
    [SerializeField, ReadOnly] private bool bLoaded = false;
    [SerializeField, ReadOnly]
    private List<P_Achievement> achievementList = new List<P_Achievement>();

    // Public:
    // Show Page
    public void ShowGameAchievementPage()
    {
        P_GameAchPanel.SetActive(true);
        configEndInfo();
        configAchInfo();
    }

    // Public:
    // Close Panel
    public void ClosePanel()
    {
        PersistentGameManager.Instance.ResumeGame();
        P_GameAchPanel.SetActive(false);
    }

    public void AddToAchList(int achID)
    {
        GameObject achievementItem = PrefabManager.Instance.Instantiate("P_Achievement", Vector3.zero, Quaternion.identity);
        P_Achievement achievement = achievementItem.GetComponent<P_Achievement>();
        achievement.SetUp(achID);
        achievementList.Add(achievement);
        achievementItem.transform.SetParent(SV_AchievementList.content);
        achievementItem.transform.localScale = Vector3.one;
    }

    // Private:
    // Process Achievement info - Init
    private void configAchInfo()
    {
        if (bLoaded)
        {
            // Update
            updateAchInfo();
            return;
        }
        int count = AchievementData.data.Count;
        for (int i = 0; i < count; i++)
        {
            AddToAchList(i);
        }
        bLoaded = true;
    }

    private void updateAchInfo()
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            achievementList[i].SetUp(i);
        }
    }

    // Process end info
    private void configEndInfo()
    {
        for (int i = 0; i < endInfoList.Count; i++)
        {
            // Check if the end is unlocked
            if (SaveManager.Instance.CheckGameEndUnlocked(i))
            {
                // SetUp with the index as ID
                endInfoList[i].SetUp(i);
            }
            else
            {
                // SetUp with -1
                endInfoList[i].SetUp(-1);
            }
        }
    }
}
