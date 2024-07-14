using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Data")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float _maxStamina = 3f; //seconds based

    public float MaxStamina { get { return _maxStamina; } }

    private float _currentStamina;

    public float CurrentStamina { get { return _currentStamina; } }

    private Vector2 input;
    private Rigidbody body;

    [SerializeField] private Vector3 totalVelocity;
    private bool _isRunning = false;
    public bool IsRunning { get { return _isRunning;} }


    private bool canRun = true;
    private bool canMove = false;


    private void Start()
    {
        totalVelocity = Vector3.zero;
        input = Vector2.zero;
        body = GetComponent<Rigidbody>();


        _currentStamina = _maxStamina;

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_GAME_STARTED, () => canMove = true );

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_GAME_OVER, () => canMove = false );

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_TIMES_UP, () => canRun = false );

    }

    private Vector3 previousVelocity = Vector3.zero;

    private void FixedUpdate()
    {
        previousVelocity = body.velocity;


        Vector3 forwardRelative = input.y * Camera.main.transform.forward;
        Vector3 rightRelative = input.x * Camera.main.transform.right;

        totalVelocity = (forwardRelative + rightRelative).normalized * speed * (_isRunning && _currentStamina > 0 ? sprintMultiplier : 1f);
        totalVelocity.y = 0;

        body.AddForce(totalVelocity, ForceMode.Force);
    }

    private void Update()
    {
        if (_isRunning && isSpeedingUp())
        {
            _currentStamina -= Time.deltaTime;
        }
        else _currentStamina += Time.deltaTime * 0.25f;

        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
    }


    public void Move(InputAction.CallbackContext context)
    {

        if (canMove)
        {
            input = context.ReadValue<Vector2>();
        }
        
    }

    public void Run(InputAction.CallbackContext context)
    {

        if (!canRun)
            return;

        if (context.started && _currentStamina > 0 && isSpeedingUp())
        {
            _isRunning = true;
            EventBroadcaster.Instance.PostEvent(EventNames.PLAYER_ACTIONS.ON_PLAYER_SPRINT_STARTED);
        }    
        else if (context.canceled)
        {
            _isRunning = false;
            EventBroadcaster.Instance.PostEvent(EventNames.PLAYER_ACTIONS.ON_PLAYER_SPRINT_ENDED);
        }
            
    }

    private bool isSpeedingUp()
    {

        /*
        Vector3 xzBody = body.velocity;
        xzBody.y = 0;

        Vector3 xzPrevious = previousVelocity;
        xzPrevious.y = 0;
     

        return xzBody.magnitude > xzPrevious.magnitude;
           */

        Vector3 v = body.velocity;
        v.y = 0;
        return v.magnitude > 1f;

    }

}
