using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reference of all 2D animation clips compiled in game
/// </summary>

[CreateAssetMenu(menuName = "GameEffect/AnimConfig")]
public class AnimConfig : GameEffectConfig<AnimationClip>
{
    private static AnimConfig instance = null;

    public static AnimConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<AnimConfig>(typeof(AnimConfig).Name);
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<AnimConfig>();
                    Debug.LogWarning("Created instance of " + typeof(AnimConfig).Name + " because none was found in resources.");
                }
            }
            return instance;
        }
    }
}
