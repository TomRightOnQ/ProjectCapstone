using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Light Rotation
/// </summary>
public class RotationalLight : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.01f; // Adjust the rotation speed as needed

    private void Update()
    {
        // Rotate the object continuously around its local X-axis
        transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
    }
}
