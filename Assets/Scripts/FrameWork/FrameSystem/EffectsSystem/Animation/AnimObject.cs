using UnityEngine;

/// <summary>
/// Objects carraying animation clips
/// </summary>

public class AnimObject : GameEffect
{
    [SerializeField] private Animation anim;

    public override void SetUp(string _name, Vector3 pos)
    {
        anim.clip = AnimConfig.Instance.GetClip(_name);
        if (anim.clip == null)
        {
            Deactivate();
        }
        else
        {
            life = anim.clip.length;
            transform.position = pos;
            transform.localRotation = Quaternion.Euler(45, 0, 0);
            anim.Play();
            Invoke("StopPlaying", life);
            Invoke("Deactivate", life);
        }
    }

    public override void SetUp(AudioClip clip, Vector3 pos)
    {
        return;
    }

    public void StopPlaying()
    {
        anim.Stop();
    }
}
