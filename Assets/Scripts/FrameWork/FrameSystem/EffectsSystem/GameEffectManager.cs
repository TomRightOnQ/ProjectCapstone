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
        VFXConfig.Instance.Init();
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

    public void PlaySound(string _name, Vector3 pos)
    {

    }

    public void PlayAnim(string _name, Vector3 pos, Vector3 scale)
    {
        GameObject animObject = PrefabManager.Instance.Instantiate("AnimObject", pos, Quaternion.identity);
        if (animObject == null || animObject.GetComponent<AnimObject>() == null)
        {
            Debug.LogWarning("Unable to accquire anim from pooling");
            return;
        }
        AnimObject anim = animObject.GetComponent<AnimObject>();
        anim.SetUp(_name, pos);
        animObject.transform.localScale = scale;
        animObject.SetActive(true);
    }

    public void PlayVFX(string _name, Vector3 pos, Vector3 scale)
    {
        GameObject vfxObject = PrefabManager.Instance.Instantiate("VFXObject", pos, Quaternion.identity);
        if (vfxObject == null || vfxObject.GetComponent<VFXObject>() == null)
        {
            Debug.LogWarning("Unable to accquire VFX from pooling");
            return;
        }
        VFXObject vfx = vfxObject.GetComponent<VFXObject>();
        vfx.SetUp(_name, pos);
        vfxObject.transform.localScale = scale;
        vfxObject.SetActive(true);
    }
}
