using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraYOffset = 3f;
    [SerializeField] private float cameraZOffset = 4f;

    // Camera horizontal limit
    [SerializeField] private float camMinX = -100f;
    [SerializeField] private float camMaxX = 100f;
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

        // Set target position based on facing direction
        Vector3 targetPosition;
        float cameraZ = Mathf.Clamp(targetObject.transform.position.z - cameraZOffset, camMinZ, camMaxZ);
        if (unit.GetFacingToRight())
        {
            float cameraX = Mathf.Clamp(targetObject.transform.position.x + Constants.CAMERA_PAN_OFFSET, camMinX, camMaxX);
            targetPosition = new Vector3(cameraX, targetObject.transform.position.y + cameraYOffset, cameraZ);
        }
        else
        {
            float cameraX = Mathf.Clamp(targetObject.transform.position.x - Constants.CAMERA_PAN_OFFSET, camMinX, camMaxX);
            targetPosition = new Vector3(cameraX, targetObject.transform.position.y + cameraYOffset, cameraZ);
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

    public void SetCameraToPlayerMode(bool bSetToPlayerMode = true)
    {
        bPlayerMode = bSetToPlayerMode;
    }

    public void SetCameraToTarget(GameObject target, EUnit targetUnit)
    {
        targetObject = target;
        unit = targetUnit;
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
