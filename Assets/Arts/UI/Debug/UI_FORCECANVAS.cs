using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This canvas is not managed by UIManager, and it's not a real UIBase
/// </summary>
public class UI_FORCECANVAS : MonoBehaviour
{
    // Version
    [SerializeField] private TextMeshProUGUI TB_Version;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        TB_Version.text = "Version " + Application.version;
    }
}
