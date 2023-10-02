using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController.ChangePlayerInputState(true);
        playerController.ChangePlayerMovementState(true);
        playerController.ChangePlayerActiveMovementState(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound()
    {
        GameEffectManager.Instance.PlayEffect<SFXObject>("CloseShot", Vector3.zero);
        Debug.Log(Constants.TEST_INT_C1);
    }

    public void SwapScene()
    {
        PersistentGameManager.Instance.LoadScene(Constants.SCENE_DEFAULT_LEVEL);
    }

    public void BackToTest()
    {
        PersistentGameManager.Instance.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public void TakeObj()
    {
        PrefabManager.Instance.Instantiate("SphereObject", Vector3.zero, Quaternion.identity);     
    }
}
