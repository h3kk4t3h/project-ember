using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static Transform InstanceTransform { get; private set; }
    public static event Action<Transform> OnPlayerSpawned;

    [SerializeField] private float movementSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        InstanceTransform = transform;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        OnPlayerSpawned?.Invoke(transform);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 45f, 0f);
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        Vector3 rotatedDirection = rotation * direction;

        rb.MovePosition(rb.position + rotatedDirection * movementSpeed * Time.fixedDeltaTime);
    }
}