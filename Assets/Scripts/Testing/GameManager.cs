using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MainMenu Manager
/// </summary>
/// 
public class GameManager : MonoBehaviour
{
    // Public:
    // Enter game
    public void EnterGame()
    {
        LevelManager.Instance.EnterGame();
    }


    public void PlaySound()
    {
        GameEffectManager.Instance.PlayEffect<SFXObject>("CloseShot", Vector3.zero);
        Debug.Log(Constants.TEST_INT_C1);
    }


    public void BackToTest()
    {
        LevelManager.Instance.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public void TakeObj()
    {
        PrefabManager.Instance.Instantiate("SphereObject", Vector3.zero, Quaternion.identity);     
    }
}
