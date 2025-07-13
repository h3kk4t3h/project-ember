using System.Security.AccessControl;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    private Rigidbody rb;

    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        UnityEngine.Debug.Log("Input: " + moveInput);
    }

    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 45f, 0f);
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        Vector3 rotatedDirection = rotation * direction;

        rb.MovePosition(rb.position + rotatedDirection * movementSpeed * Time.fixedDeltaTime);
    }

    // CameraController integration
    void Start()
    {
        CameraController cameraController = FindFirstObjectByType<CameraController>();
        if (cameraController != null)
        {
            cameraController.SetTarget(transform);
        }
    }
}
