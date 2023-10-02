using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// References of BGM clips held here
/// Loading is handled in MusicManager.cs
/// </summary>

public class MusicConfig : ScriptableObject
{
    private static MusicConfig instance = null;

    public static MusicConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<MusicConfig>(typeof(MusicConfig).Name);
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<MusicConfig>();
                    Debug.LogWarning("Created instance of " + typeof(MusicConfig).Name + " because none was found in resources.");
                }
            }
            return instance;
        }
    }

    [System.Serializable]
    public struct MusicData
    {
        public string name;
        public string path;
        public int sceneID;
    };

    [SerializeField]
    private MusicData[] clips;

    private Dictionary<string, MusicData> musicNameDictionary;
    private Dictionary<int, List<MusicData>> sceneMusicDictionary;

    public void Init()
    {
        musicNameDictionary = new Dictionary<string, MusicData>();
        sceneMusicDictionary = new Dictionary<int, List<MusicData>>();

        foreach (var namedClip in clips)
        {
            musicNameDictionary[namedClip.name] = namedClip;

            if (!sceneMusicDictionary.ContainsKey(namedClip.sceneID))
            {
                sceneMusicDictionary[namedClip.sceneID] = new List<MusicData>();
            }

            sceneMusicDictionary[namedClip.sceneID].Add(namedClip);
        }
    }

    // Get a clip path by name (specific) or scene (random)
    public string GetClip(string name)
    {
        if (musicNameDictionary.TryGetValue(name, out MusicData music))
        {
            return music.path;
        }
        else
        {
            Debug.LogWarning("Clip with name " + name + " not found in MusicConfig");
            return null;
        }
    }

    public string GetClip(int sceneID)
    {
        if (sceneMusicDictionary.TryGetValue(sceneID, out List<MusicData> sceneMusic))
        {
            var randomTrack = sceneMusic[Random.Range(0, sceneMusic.Count)];
            return randomTrack.path;
        }
        else
        {
            Debug.LogWarning("No music clips found for sceneID: " + sceneID);
            return null;
        }
    }
}
