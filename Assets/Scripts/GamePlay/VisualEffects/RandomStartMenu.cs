using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Randomly select a main menu scene
/// </summary>
public class RandomStartMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> sceneList = new List<GameObject>();

    private void Start()
    {
        if (sceneList.Count > 0)
        {
            int selectedIndex = Random.Range(0, sceneList.Count);

            for (int i = 0; i < sceneList.Count; i++)
            {
                if (i == selectedIndex)
                {
                    sceneList[i].SetActive(true);
                }
                else
                {
                    sceneList[i].SetActive(false);
                }
            }
        }
    }
}
