using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Customized Toggle that interact with number index
/// </summary>
public class ToggleGroupIndex : ToggleGroup
{
    [SerializeField] private List<Toggle> toggleList = new List<Toggle>();

    [SerializeField] private int currentIndex = 0;
    public int CurrentIndex => currentIndex;

    // Public:
    public void SetIndex(int index)
    {
        currentIndex = index;
    }

    public void LoadIndex(int index)
    {
        toggleList[index].isOn = true;
    }
}
