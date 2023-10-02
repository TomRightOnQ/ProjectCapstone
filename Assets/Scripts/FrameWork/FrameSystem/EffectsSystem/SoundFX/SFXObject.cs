using UnityEngine;
using UnityEngine.Audio;
using static AudioConfig;

/// <summary>
/// Objects carrying audio clips playing
/// </summary>

public class SFXObject : GameEffect
{
    [SerializeField] private AudioSource source;

    public override void SetUp(string _name, Vector3 pos)
    {
        source.clip = AudioConfig.Instance.GetClip(_name);
        if (source.clip == null)
        {
            Deactivate();
        }
        else
        {
            life = source.clip.length;
            source.Play();
            Invoke("StopPlaying", life);
            Invoke("Deactivate", life);
        }
    }

    public override void SetUp(AudioClip clip, Vector3 pos)
    {
        source.clip = clip;
        if (source.clip == null)
        {
            Deactivate();
        }
        else
        {
            life = source.clip.length;
            source.Play();
            Invoke("StopPlaying", life);
            Invoke("Deactivate", life);
        }
    }

    public void StopPlaying()
    {
        source.Stop();
    }
}
