using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Straming music according ton the music config
/// </summary>

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    // FMOD Audio Event
    [SerializeField] private StudioEventEmitter BGMEmmiter;
    [SerializeField, ReadOnly] private string BGM_EVENT_PREFIX = "event:/Music/";
    [SerializeField, ReadOnly] private string currentBGMEvent = "event:/Music/BGM Main Menu 1";

    // FMOD event instance
    private EventInstance bgmInstance;

    // FMOD bus
    private static string MASTER_BUS = "bus:/";
    private static string MUSIC_BUS = "bus:/Music";
    private static string SFX_BUS = "bus:/SFX";

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

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
            configFMODBuses();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Set Up buses
    private void configFMODBuses()
    {
        masterBus = RuntimeManager.GetBus(MASTER_BUS);
        musicBus = RuntimeManager.GetBus(MUSIC_BUS);
        sfxBus = RuntimeManager.GetBus(SFX_BUS);
        // Auto set when boot up
        SetMasterBus(PlayerPrefs.GetFloat("audioMaster"));
        SetMusicBus(PlayerPrefs.GetFloat("audioMusic"));
        SetSFXBus(PlayerPrefs.GetFloat("audioSFX"));
    }

    // Set Bus Value
    public void SetMasterBus(float value)
    {
        masterBus.setVolume(value);
    }

    public void SetMusicBus(float value)
    {
        musicBus.setVolume(value);
    }

    public void SetSFXBus(float value)
    {
        sfxBus.setVolume(value);
    }

    // Play a specific music clip or play randomly
    public void PlayMusic(string name)
    {
        bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentBGMEvent = BGM_EVENT_PREFIX + name;
        bgmInstance = RuntimeManager.CreateInstance(currentBGMEvent);
        bgmInstance.setParameterByName("BGMStatus", 0);
        bgmInstance.start();
        //bgmInstance.release();
    }

    // Switch music parameter
    public void ChangeMusicState(int state)
    {
        bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bgmInstance = RuntimeManager.CreateInstance(currentBGMEvent);
        bgmInstance.setParameterByName("BGMStatus", state);
        bgmInstance.start();
        //bgmInstance.release();
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
