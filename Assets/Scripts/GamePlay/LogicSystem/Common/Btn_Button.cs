using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Common Button
public class Btn_Button : MonoBehaviour
{
    public void PlayConfirmSound()
    {
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CONFIRM);
    }

    public void PlayCancelSound()
    {
        GameEffectManager.Instance.PlayUISound(Constants.SOUND_UI_CANCEL);
    }
}
