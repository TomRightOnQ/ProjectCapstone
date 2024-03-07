using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Light with controllable stats
/// </summary>
public class ScriptableLight : MonoBehaviour
{
    // Light
    [SerializeField] private HDAdditionalLightData lightComponent;

    // Resolution
    [SerializeField] private int LOW_SHAODW_RESOLUTION = 512;
    [SerializeField] private int MEDIUM_SHAODW_RESOLUTION = 1024;
    [SerializeField] private int HIGH_SHAODW_RESOLUTION = 2048;

    // Diable when low
    [SerializeField] private bool bDisableAtLow = false;
    // No Shadow
    [SerializeField] private bool bNoShadow = false;

    private void Start()
    {
        if (lightComponent == null)
        {
            lightComponent = GetComponent<HDAdditionalLightData>();
        }
        ChangeLevel();
    }

    // Private:
    private void OnEnable()
    {
        ChangeLevel();
    }

    // Public:
    public void ChangeLevel()
    {
        int graphicLevel = PlayerPrefs.GetInt("graphicLevel", 0);

        switch (graphicLevel)
        {
            case 0:
                SetAsLow();
                break;
            case 1:
                SetAsMedium();
                break;
            case 2:
                SetAsHigh();
                break;
        }
    }

    public void SetAsLow()
    {
        if (!bNoShadow)
        {
            lightComponent.SetShadowResolution(LOW_SHAODW_RESOLUTION);
        }

        if (bDisableAtLow)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetAsMedium()
    {
        if (!bNoShadow)
        {
            lightComponent.SetShadowResolution(MEDIUM_SHAODW_RESOLUTION);
        }

        if (bDisableAtLow)
        {
            gameObject.SetActive(true);
        }
    }

    public void SetAsHigh()
    {
        if (!bNoShadow)
        {
            lightComponent.SetShadowResolution(HIGH_SHAODW_RESOLUTION);
        }

        if (bDisableAtLow)
        {
            gameObject.SetActive(true);
        }
    }
}
