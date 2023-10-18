using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI showed when a stage is cleared
/// </summary>

public class UI_Level2DComplete : UIBase
{
    // Process OnClick
    public void OnClick_Btn_ReturnScene()
    {
        GameManager2D.Instance.LeaveGameScene();
    }
}
