using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //Events
    public static Transform InstanceTransform { get; private set; }
    public static event Action<Transform> OnPlayerSpawned;

    //Player Movement Variables
    [SerializeField] private float movementSpeed = 5f;

    //Player Associated Variables
    private Rigidbody rb;
    private Vector2 moveInput;
    public Vector3 rotatedDirection;

    //Ability handler variables
    private AbilityHandler abilityHandler;

    private void Awake()
    {
        InstanceTransform = transform;

        rb = GetComponent<Rigidbody>();
        abilityHandler = GetComponent<AbilityHandler>();
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
        if (abilityHandler.GetDashingState()) return;

        Quaternion rotation = Quaternion.Euler(0f, 45f, 0f);
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        rotatedDirection = rotation * direction;

        rb.MovePosition(rb.position + rotatedDirection * movementSpeed * Time.fixedDeltaTime);
    }
}