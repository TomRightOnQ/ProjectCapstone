using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Reference of all VFX compiled in game
/// VFX
/// </summary>
[CreateAssetMenu(menuName = "GameEffect/VFXConfig")]
public class VFXConfig : GameEffectConfig<VisualEffectAsset>
{
    private static VFXConfig instance = null;

    public static VFXConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<VFXConfig>(typeof(VFXConfig).Name);
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<VFXConfig>();
                    Debug.LogWarning("Created instance of " + typeof(VFXConfig).Name + " because none was found in resources.");
                }
            }
            return instance;
        }
    }
}
