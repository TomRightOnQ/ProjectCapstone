using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Bubble of saying something
/// </summary>
public class EntityBubble : MonoBehaviour
{
    // Text
    [SerializeField] private TextMeshProUGUI bubbleText;
    // Timer
    [SerializeField] private float targetTime = 0f;
    [SerializeField] private float timeElapsed = 0f;

    // Private:
    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (timeElapsed >= targetTime)
        {
            // Turn off the bubble
            gameObject.SetActive(false);
        }
        else 
        {
            timeElapsed += Time.deltaTime;
        }
    }

    // Public:
    public void BeginBubble(string content = "", float time = 2f)
    {
        // Active if not
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        // Reset timer
        targetTime = time;
        timeElapsed = 0f;

        bubbleText.text = content;
    }
}
