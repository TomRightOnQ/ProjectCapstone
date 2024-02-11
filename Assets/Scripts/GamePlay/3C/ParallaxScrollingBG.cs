using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control logic for parallax scrolling background
/// Attach the script to the parent object
/// Attach each layer and its offset to the list
/// -- This a singleton object, do not use the second one! --
/// </summary>
public class ParallaxScrollingBG : MonoBehaviour
{
    private static ParallaxScrollingBG instance;
    public static ParallaxScrollingBG Instance => instance;

    [System.Serializable]
    class backgroundSprite
    {
        // Offset Value
        public float offsetValue;
        public GameObject spriteObject;
        public Vector3 originalLocation;
    }

    [SerializeField] private List<backgroundSprite> bgList = new List<backgroundSprite>();

    // Flag to indicate ready
    [SerializeField, ReadOnly] private bool bReady = false;

    private void Awake()
    {
        // Config Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Record the original position
        for (int i = 0; i < bgList.Count; i++)
        {
            bgList[i].originalLocation = bgList[i].spriteObject.transform.position;
        }
        bReady = true;
    }

    // Recv camera movement and move the background
    public void moveBackground(float cameraOffsetX, float cameraOffsetY)
    {
        if (!bReady)
        {
            return;
        }
        // Iterate through each background layer
        for (int i = 0; i < bgList.Count; i++)
        {
            // Calculate the new position for the layer
            Vector3 newPosition = bgList[i].originalLocation + new Vector3(cameraOffsetX * bgList[i].offsetValue, cameraOffsetY * -0.1f, 0);

            // Apply the new position to the layer
            bgList[i].spriteObject.transform.position = newPosition;
        }
    }
}
