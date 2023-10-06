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



    // Public:
    // Init
    public void Init()
    {
        openedUIs.Clear();
        // This will find and potentially register already-open UIs at start, if needed.
        UIBase[] allUIBases = FindObjectsOfType<UIBase>();
        foreach (UIBase ui in allUIBases)
        {
            openedUIs[ui.name] = ui;
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
        }
        return openedUIs[uiName].gameObject;
    }

    // Show a UI
    public GameObject ShowUI(string uiName)
    {
        if (openedUIs.ContainsKey(uiName))
        {
            openedUIs[uiName].gameObject.SetActive(true);
        }
        else 
        {
            UIData uiData = UIConfig.Instance.GetUIData(uiName);

            GameObject uiObject = Instantiate(uiData.PrefabReference, Vector3.zero, Quaternion.identity);
            uiObject.SetActive(true);

            UIBase uiBase = uiObject.GetComponent<UIBase>();
            uiBase.SetUp(uiData);
            openedUIs[uiName] = uiBase;
        }
        return openedUIs[uiName].gameObject;
    }

    // Hide a UI
    public void HideUI(string uiName)
    {
        if (openedUIs.TryGetValue(uiName, out UIBase uiBase))
        {
            uiBase.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"UIManmager: UI '{uiName}' not found to hide.");
        }
    }

    // Destroy a UI
    public void DestroyUI(string uiName)
    {
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
