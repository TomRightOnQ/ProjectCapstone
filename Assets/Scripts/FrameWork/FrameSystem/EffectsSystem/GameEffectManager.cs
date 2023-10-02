using UnityEngine;

public class GameEffectManager : MonoBehaviour
{
    private static GameEffectManager instance;
    public static GameEffectManager Instance => instance;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitGameEffectConfigs()
    {
        AudioConfig.Instance.Init();
        AnimConfig.Instance.Init();
    }

    public void PlayEffect<T>(string _name, Vector3 pos) where T : Component, ISetup
    {
        T effect = Pooling.Instance.GetObj<T>();
        if (effect == null)
        {
            Debug.LogWarning($"Effect Object for {typeof(T).Name} not found.");
            return;
        }
        effect.SetUp(_name, pos);
    }

    public void PlayEffect<T>(AudioClip clip, Vector3 pos) where T : Component, ISetup
    {
        T effect = Pooling.Instance.GetObj<T>();
        if (effect == null)
        {
            Debug.LogWarning($"Effect Object for {typeof(T).Name} not found.");
            return;
        }
        effect.SetUp(clip, pos);
    }

    public void PlaySound(string _name, Vector3 pos)
    {
        GameObject sfxObj = PrefabManager.Instance.GetReference("SFXObject");
        if (sfxObj == null || sfxObj.GetComponent<SFXObject>() == null)
        {
            Debug.LogWarning("Unable to accquire SFX from pooling");
            return;
        }
        SFXObject sfx = sfxObj.GetComponent<SFXObject>();
        sfx.SetUp(_name, pos);
    }

    public void PlayAnim(string _name, Vector3 pos, Vector3 scale)
    {
        GameObject animObject = PrefabManager.Instance.GetReference("AnimObject");
        if (animObject == null || animObject.GetComponent<AnimObject>() == null)
        {
            Debug.LogWarning("Unable to accquire anim from pooling");
            return;
        }
        AnimObject anim = animObject.GetComponent<AnimObject>();
        anim.SetUp(_name, pos);
        animObject.transform.localScale = scale;
    }
}
