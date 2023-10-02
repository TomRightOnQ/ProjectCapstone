using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reference of all audio clips compiled in game
/// SFX and short clips
/// </summary>

[CreateAssetMenu(menuName = "GameEffect/AudioConfig")]
public class AudioConfig : GameEffectConfig<AudioClip>
{
    private static AudioConfig instance = null;

    public static AudioConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<AudioConfig>(typeof(AudioConfig).Name);
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<AudioConfig>();
                    Debug.LogWarning("Created instance of " + typeof(AudioConfig).Name + " because none was found in resources.");
                }
            }
            return instance;
        }
    }
}
