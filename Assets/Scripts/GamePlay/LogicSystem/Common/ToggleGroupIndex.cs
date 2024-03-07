using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Customized Toggle that interact with number index
/// </summary>
public class ToggleGroupIndex : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggleList = new List<Toggle>();

    [SerializeField] private int currentIndex = 0;
    public int CurrentIndex => currentIndex;

    // Public:
    public void SetIndex(int index)
    {
        toggleList[index].isOn = true;
        currentIndex = index;
        OnToggle(index);
    }

    // Events
    public void OnToggle(int index)
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            if (i != index)
            {
                toggleList[i].isOn = false;
            }
        }
    }
}
