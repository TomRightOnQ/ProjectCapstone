using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object defining all config files
/// </summary>
/// <typeparam name="TClip"></typeparam>

public class GameEffectConfig<TClip> : ScriptableObject where TClip : Object
{
    [System.Serializable]
    public class NamedClip
    {
        public string name;
        public TClip clip;
    }

    [SerializeField]
    private NamedClip[] clips;

    private Dictionary<string, TClip> clipDictionary;

    public void Init()
    {
        clipDictionary = new Dictionary<string, TClip>();
        foreach (var namedClip in clips)
        {
            clipDictionary[namedClip.name] = namedClip.clip;
        }
    }

    public TClip GetClip(string name)
    {
        if (name == "None")
        {
            return null;
        }
        if (clipDictionary.TryGetValue(name, out TClip clip))
        {
            return clip;
        }
        else
        {
            Debug.LogWarning("Clip with name " + name + " not found in MusicConfig");
            return null;
        }
    }
}
