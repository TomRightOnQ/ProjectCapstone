using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Objects carrying VFX clips playing
/// </summary>

public class VFXObject : GameEffect
{
    [SerializeField] private VisualEffect source;

    public override void SetUp(string _name, Vector3 pos)
    {
        source.visualEffectAsset = VFXConfig.Instance.GetClip(_name);
        if (source.visualEffectAsset == null)
        {
            Deactivate();
        }
        else
        {
            life = 2f;
            Invoke("Deactivate", life);
        }
    }

    public override void SetUp(AudioClip clip, Vector3 pos)
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
