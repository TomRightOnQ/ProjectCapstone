using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraYOffset = 3f;
    [SerializeField] private float cameraZOffset = 4f;

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
    }

    private void FixedUpdate()
    {
        if (unit == null || !bPlayerMode) return;

        // Set target position based on facing direction
        Vector3 targetPosition;

        if (unit.GetFacingToRight())
        {
            targetPosition = new Vector3(targetObject.transform.position.x + Constants.CAMERA_PAN_OFFSET, targetObject.transform.position.y + cameraYOffset, targetObject.transform.position.z - cameraZOffset);
        }
        else
        {
            targetPosition = new Vector3(targetObject.transform.position.x - Constants.CAMERA_PAN_OFFSET, targetObject.transform.position.y + cameraYOffset, targetObject.transform.position.z - cameraZOffset);
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
}
