using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to blink Materials
public class BlinkingMaterials : MonoBehaviour
{
    // Event
    [SerializeField] private GameEvent.Event triggerEvent = GameEvent.Event.SHOOTER_LEVEL_BEGIN;
    [SerializeField] private GameEvent.Event endEvent = GameEvent.Event.EVENT_2DGAME_END;

    // Material Changing
    [SerializeField] private List<Material> materialList = new List<Material>();
    [SerializeField] private SpriteRenderer mainRenderer;

    private void Awake()
    {
        mainRenderer = GetComponent<SpriteRenderer>();
        EventManager.Instance.AddListener(triggerEvent, OnRecv_Begin);
        EventManager.Instance.AddListener(endEvent, OnRecv_End);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(triggerEvent, OnRecv_Begin);
        EventManager.Instance.RemoveListener(endEvent, OnRecv_End);
    }

    // Blinking Effect
    private IEnumerator blinkingEffectCoroutine()
    {
        if (materialList.Count == 0 || mainRenderer == null)
        {
            yield break;
        }

        float effectDuration = Random.Range(1f, 2f);
        while (true)
        {
            Material randomMaterial = materialList[Random.Range(0, materialList.Count)];
            mainRenderer.material = randomMaterial;
            yield return new WaitForSeconds(effectDuration);
        }
    }

    // Event Handlers
    protected void OnRecv_Begin()
    {
        StartCoroutine(blinkingEffectCoroutine());
    }

    protected void OnRecv_End()
    {
        StopAllCoroutines();
    }
}
