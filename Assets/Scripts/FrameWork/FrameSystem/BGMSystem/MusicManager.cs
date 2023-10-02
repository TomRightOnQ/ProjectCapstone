using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Straming music according ton the music config
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private static string MUSIC_PATH = "Audio/BGM";

    private string currentClip = "";

    public static MusicManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Current BGM
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            MusicConfig.Instance.Init();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Play a specific music clip or play randomly
    public void PlayMusic(string name)
    {
        audioSource.clip = null;
        string path = MusicConfig.Instance.GetClip(name);
        if (path != null)
        {
            StartCoroutine(LoadMusicAndPlay(path));
        }
        else
        {
            Debug.LogWarning("No music found for name: " + name);
        }
    }

    public void PlayRandomMusic(int sceneID)
    {
        string path = MusicConfig.Instance.GetClip(sceneID);

        // Prevent double-loading if music unchanged
        if (audioSource.clip != null)
        {
            if (path != currentClip)
            {
                audioSource.clip = null;
            }
            else {
                Play();
                return;
            }
        }

        if (path != null)
        {
            currentClip = path;
            StartCoroutine(LoadMusicAndPlay(path));
        }
        else
        {
            Debug.LogWarning("No music found for sceneID: " + sceneID);
        }
    }

    public void PlayMusicLocal(string name)
    {
        string path = MusicConfig.Instance.GetClip(name);
        if (path != null)
        {
            StartCoroutine(LoadMusicAndPlayLocal(path));
        }
        else
        {
            Debug.LogWarning("No music found for name: " + name);
        }
    }

    // Load and play
    private IEnumerator LoadMusicAndPlay(string path)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, Path.Combine(MUSIC_PATH, path));
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    audioSource.clip = clip;
                    Play();
                }
                else
                {
                    Debug.LogError("Failed to create AudioClip from path: " + path);
                }
            }
            else
            {
                Debug.LogError("Failed to load music clip: " + path);
            }
        }
    }

    // Load and play in local space
    private IEnumerator LoadMusicAndPlayLocal(string path)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, Path.Combine(MUSIC_PATH, path));
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    GameEffectManager.Instance.PlayEffect<SFXObject>(clip, Vector3.zero);
                }
                else
                {
                    Debug.LogError("Failed to create AudioClip from path: " + path);
                }
            }
            else
            {
                Debug.LogError("Failed to load music clip: " + path);
            }
        }
    }

    // Wrappers or Play() and Stop()
    public void Play()
    {
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
