using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager Class for UIs
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    // All opened UIs
    private Dictionary<string, UIBase> openedUIs = new Dictionary<string, UIBase>();

    // UI Exclusion State
    private Dictionary<int, bool> uiExclusion = new Dictionary<int, bool>();

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Private:
    // Check Exclusion Condition
    public void TryActiveUI(UIData uiData, GameObject UI)
    {
        if (uiData.UIExclusionGroup == -1 || !uiExclusion[uiData.UIExclusionGroup])
        {
            UI.SetActive(true);
            uiExclusion[uiData.UIExclusionGroup] = true;
        }
    }

    public void TryDeactiveUI(UIData uiData, GameObject UI)
    {
        UI.SetActive(false);
        uiExclusion[uiData.UIExclusionGroup] = false;
    }

    // Public:
    // Init
    public void Init()
    {
        openedUIs.Clear();
        uiExclusion.Clear();
        // This will find and potentially register already-open UIs at start, if needed.
        UIBase[] allUIBases = FindObjectsOfType<UIBase>();
        foreach (UIBase ui in allUIBases)
        {
            openedUIs[ui.name] = ui;
            UIData uiData = UIConfig.Instance.GetUIData(ui.name);
            uiExclusion[uiData.UIExclusionGroup] = false;
        }
    }

    // Create a UI
    public GameObject CreateUI(string uiName)
    {
        if (!openedUIs.ContainsKey(uiName))
        {
            UIData uiData = UIConfig.Instance.GetUIData(uiName);
            GameObject uiObject = Instantiate(uiData.PrefabReference, Vector3.zero, Quaternion.identity);
            UIBase uiBase = uiObject.GetComponent<UIBase>();
            uiBase.SetUp(uiData);
            openedUIs[uiName] = uiBase;
            if (!uiExclusion.ContainsKey(uiData.UIExclusionGroup))
            {
                uiExclusion[uiData.UIExclusionGroup] = false;
            }
        }
        return openedUIs[uiName].gameObject;
    }

    // Show a UI
    public GameObject ShowUI(string uiName)
    {
        UIData uiData = UIConfig.Instance.GetUIData(uiName);
        if (openedUIs.ContainsKey(uiName))
        {
            TryActiveUI(uiData, openedUIs[uiName].gameObject);
        }
        else 
        {
            GameObject uiObject = Instantiate(uiData.PrefabReference, Vector3.zero, Quaternion.identity);
            TryActiveUI(uiData, uiObject);

            UIBase uiBase = uiObject.GetComponent<UIBase>();
            uiBase.SetUp(uiData);
            openedUIs[uiName] = uiBase;
        }
        return openedUIs[uiName].gameObject;
    }

    // Hide a UI
    public void HideUI(string uiName)
    {
        UIData uiData = UIConfig.Instance.GetUIData(uiName);
        if (openedUIs.TryGetValue(uiName, out UIBase uiBase))
        {
            TryDeactiveUI(uiData, uiBase.gameObject);
        }
        else
        {
            Debug.LogWarning($"UIManmager: UI '{uiName}' not found to hide.");
        }
    }

    // Destroy a UI
    public void DestroyUI(string uiName)
    {
        HideUI(uiName);
        if (openedUIs.TryGetValue(uiName, out UIBase uiBase))
        {
            openedUIs.Remove(uiName);
            Destroy(uiBase.gameObject);
        }
        else
        {
            Debug.LogWarning($"UIManmager: UI '{uiName}' not found to destroy.");
        }
    }
}
