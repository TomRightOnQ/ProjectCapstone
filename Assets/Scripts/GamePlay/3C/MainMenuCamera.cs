using System.Collections;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private Camera mainMenuCamera;

    [SerializeField] private float rotationSpeed = 2.0f; // Adjust the rotation speed as needed
    [SerializeField] private float minRotationAngleY = 8.0f; // Minimum allowed rotation angle around Y-axis
    [SerializeField] private float maxRotationAngleY = 36.0f; // Maximum allowed rotation angle around Y-axis

    private float currentRotationY = 0.0f;

    private void Awake()
    {
        if (mainMenuCamera == null)
        {
            mainMenuCamera = GetComponent<Camera>();
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        currentRotationY += mouseX * rotationSpeed * 0.1f;
        currentRotationY = Mathf.Clamp(currentRotationY, minRotationAngleY, maxRotationAngleY);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentRotationY, 0.0f);
    }
}
