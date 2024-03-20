using System.Collections;
using UnityEngine;

/// <summary>
/// Special Script for level one
/// </summary>
public class Level_T_Script : MonoBehaviour
{
    // Detect events to activate
    private void Awake()
    {
        EventManager.Instance.AddListener(GameEvent.Event.SHOOTER_LEVEL_BEGIN, Begin);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvent.Event.SHOOTER_LEVEL_BEGIN, Begin);
    }

    public void Begin()
    {
        StartCoroutine(begin());
    }

    private IEnumerator begin()
    {
        yield return new WaitForSeconds(3f);
        ReminderManager.Instance.ShowSubtitleReminder("Use A and D to move, and Space to Jump", "Guide", 
                                                      3f, Enums.CHARACTER_TYPE.Friend);
        yield return new WaitForSeconds(3.5f);
        ReminderManager.Instance.ShowSubtitleReminder("Hold Left Mouse Button to fire", "Guide",
                                                      3f, Enums.CHARACTER_TYPE.Friend);
        yield return new WaitForSeconds(3.5f);
        ReminderManager.Instance.ShowSubtitleReminder("Destroy those shining targets to earn score", "Guide",
                                                      3f, Enums.CHARACTER_TYPE.Friend);
        yield return new WaitForSeconds(3.5f);
        ReminderManager.Instance.ShowSubtitleReminder("But don't get hit... At the end of the game, you may see all teams' score", "Guide",
                                                      3f, Enums.CHARACTER_TYPE.Friend);
        yield return new WaitForSeconds(3.5f);
        ReminderManager.Instance.ShowSubtitleReminder("I can't use my full power in game, but can still slow down enemies' time", "You",
                                                      3f, Enums.CHARACTER_TYPE.You);
        yield return new WaitForSeconds(3.5f);
        ReminderManager.Instance.ShowSubtitleReminder("Press Q to use you skill", "Hint",
                                                      3f, Enums.CHARACTER_TYPE.You);
    }
}
