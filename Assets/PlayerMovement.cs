using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Data")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private float sprintMultiplier = 2f;

    private Vector2 input;
    private Rigidbody body;

    [SerializeField] private Vector3 totalVelocity;
    private bool isRunning = false;

    
    private void Start()
    {
        totalVelocity = Vector3.zero;
        input = Vector2.zero;
        body = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void FixedUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        Vector3 right = Camera.main.transform.right;
        right.y = 0;

        Vector3 forwardRelative = input.y * forward;
        Vector3 rightRelative = input.x * right;

        Vector3 dir = forwardRelative + rightRelative;

        totalVelocity = dir.normalized * speed;

        if (isRunning)
            totalVelocity *= sprintMultiplier;

        body.AddForce(totalVelocity, ForceMode.Force);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started)
            isRunning = true;
        else if (context.canceled)
            isRunning = false;
    }
}
