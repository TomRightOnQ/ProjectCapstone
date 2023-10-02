using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereObject : MEntity
{

    public void Activate()
    {
        _activate();
    }

    public void Deactivate()
    {
        _deactivate();
    }


    private void _activate()
    {
        gameObject.SetActive(true);
    }

    // Dying

    private void _deactivate()
    {
        PrefabManager.Instance.Destroy(this.gameObject);
    }
}
