using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    // In some cases, there could be more than one references
    [SerializeField] private GameObject targetObject_B;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraYOffset = 3f;
    [SerializeField] private float cameraZOffset = 4f;

    // Camera horizontal limit
    [SerializeField] private float camMinX = -100f;
    [SerializeField] private float camMaxX = 100f;
    [SerializeField] private float camMinY = -100f;
    [SerializeField] private float camMaxY = 100f;
    [SerializeField] private float camMaxZ = 100f;
    [SerializeField] private float camMinZ = -100f;

    // Default and Final x Angle
    [SerializeField] private float defaultAngle = -45f;
    [SerializeField] private float finalAngle = 22.5f;
    private float angleToGo = 0f;
    private bool bRotating = false;
    private bool bRotateDown = true;

    private bool bPlayerMode = true;
    private EUnit unit;

    // Settings
    [SerializeField] private bool bScreenShake = true;

    // Special Loading Effect
    [SerializeField] private GameObject cameraLoadingEffect_WaterSphere;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponent<Camera>();
        }
        if (unit == null && targetObject != null)
        {
            unit = targetObject.GetComponent<EUnit>();
        }

        transform.rotation = Quaternion.Euler(defaultAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        // Set up handlers
        configEventHandlers();
    }

    // Config events
    private void configEventHandlers()
    {
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_LOADED, OnRecv_SceneLoaded);
        EventManager.Instance.AddListener(GameEvent.Event.EVENT_SCENE_UNLOADED, OnRecv_SceneUnLoaded);
    }

    private void FixedUpdate()
    {
        if (unit == null || !bPlayerMode) return;

        float xValue = targetObject.transform.position.x;
        // Set target position based on facing direction
        if (targetObject_B != null)
        {
            xValue = (targetObject.transform.position.x + targetObject_B.transform.position.x) / 2;
        }

        Vector3 targetPosition;
        float cameraZ = Mathf.Clamp(targetObject.transform.position.z - cameraZOffset, camMinZ, camMaxZ);
        if (unit.GetFacingToRight())
        {
            float cameraX = Mathf.Clamp(xValue + Constants.CAMERA_PAN_OFFSET, camMinX, camMaxX);
            float cameraY = Mathf.Clamp(targetObject.transform.position.y + cameraYOffset, camMinY, camMaxY);
            targetPosition = new Vector3(cameraX, cameraY, cameraZ);
        }
        else
        {
            float cameraX = Mathf.Clamp(xValue - Constants.CAMERA_PAN_OFFSET, camMinX, camMaxX);
            float cameraY = Mathf.Clamp(targetObject.transform.position.y + cameraYOffset, camMinY, camMaxY);
            targetPosition = new Vector3(cameraX, cameraY, cameraZ);
        }

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Check if the camera is close enough to the target position
        if (distanceToTarget < Constants.CAMERA_SNAP_THRESHOLD)
        {
            transform.position = targetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, unit.GetUnitSpeed() * 0.1f * Time.deltaTime);
        }

        // Rotate the camera for scene load or unload
        if (!bRotating)
        {
            return;
        }

        float RotatingAmount = Mathf.Min(1f, angleToGo);
        if (angleToGo <= 1f)
        {
            RotatingAmount = angleToGo;
            bRotating = false;
        }
        else 
        {
            angleToGo -= 1f;
        }
        
        if (bRotateDown)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x + RotatingAmount, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else 
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x - RotatingAmount, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    // Public:
    public void SetCameraSecondReference(GameObject secondRerference)
    {
        targetObject_B = secondRerference;
    }

    public void RemoveCameraSecondReference()
    {
        targetObject_B = null;
    }

    public void SetCameraToPlayerMode(bool bSetToPlayerMode = true)
    {
        bPlayerMode = bSetToPlayerMode;
    }

    public void SetCameraToTarget(GameObject target, EUnit targetUnit)
    {
        transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
        targetObject = target;
        unit = targetUnit;
    }

    // Set Screen Shake
    public void SetScreenShake(bool bAllowed)
    {
        bScreenShake = bAllowed;
    }

    public void ShakeCamera(float duration = 0.1f, float magnitude = 1f)
    {
        // Read if shake is allowed
        if (!bScreenShake)
        {
            return;
        }
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        // Reset the camera's position after shaking
        transform.localPosition = originalPos;
    }

    // Play Special Effects
    public bool PlayCameraLoadingEffect_WaterSphere()
    {
        if (cameraLoadingEffect_WaterSphere != null)
        {
            cameraLoadingEffect_WaterSphere.SetActive(true);
            GameEffectManager.Instance.PlayUISound("SFX Teleoport 1");
            HUDInteractionManager.Instance.DisableHUDInteractionOnPlayer();
            TaskManager.Instance.StopIndicator();
            return true;
        }
        return false;
    }

    // Private:
    private void OnRecv_SceneLoaded()
    {
        angleToGo = finalAngle - defaultAngle;
        bRotateDown = true;
        bRotating = true;
    }

    private void OnRecv_SceneUnLoaded()
    {
        angleToGo = finalAngle - defaultAngle;
        bRotateDown = false;
        bRotating = true;
    }
}
