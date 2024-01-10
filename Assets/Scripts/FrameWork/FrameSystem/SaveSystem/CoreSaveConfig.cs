using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core Save Config - Saves information that persists among different saves
/// </summary>
[CreateAssetMenu(menuName = "SaveSystem/CoreSaveConfig")]
public class CoreSaveConfig : ScriptableSingleton<CoreSaveConfig>
{
    // Control flag
    [SerializeField] private bool bAllowRewrite = true;
    public bool AllowRewrite { get { return bAllowRewrite; } }

    // NotesSystem information
    [SerializeField]
    private List<int> hintUnlockList = new List<int>();
    public List<int> HintUnlockList => hintUnlockList;

    // DayCycle - Max Day
    [SerializeField]
    private int maxDay = 0;
    public int MaxDay { set; get; }

    // Public:
    // Modify Unlocked Hints
    // --Init-- Methods
    public void InitUnlockHintToSave()
    {
        hintUnlockList.Clear();
    }

    // Add a Hint
    public void UnlockHint(int hintID)
    {
        if (!hintUnlockList.Contains(hintID))
        {
            hintUnlockList.Add(hintID);
        }
    }
}
