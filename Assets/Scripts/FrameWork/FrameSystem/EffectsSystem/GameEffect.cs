using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects holding effects
/// </summary>

public interface ISetup
{
    void SetUp(string name, Vector3 pos);
    void SetUp(AudioClip clip, Vector3 pos);
}

public abstract class GameEffect : MObject, ISetup
{
    [SerializeField] protected int id = 0;
    [SerializeField] protected float life = 1f;
    [SerializeField] protected bool isOnetime = false;

    public bool IsOnetime { set { isOnetime = value; } }

    public abstract void SetUp(string _name, Vector3 pos);
    public abstract void SetUp(AudioClip clip, Vector3 pos);

    public virtual void Activate()
    {
        _activate();
    }

    public virtual void Deactivate()
    {
        CancelInvoke("Deactivate");
        _deactivate();
    }

    protected virtual void _activate()
    {
        gameObject.SetActive(true);
    }

    protected virtual void _deactivate()
    {
        gameObject.SetActive(false);
        if (isOnetime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            PrefabManager.Instance.Destroy(this.gameObject);
        }
    }
}
