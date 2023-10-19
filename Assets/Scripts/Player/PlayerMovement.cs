using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    float _moveSpeed;
    [SerializeField] float _runningSpeed;
    [SerializeField] float _walkSpeed;
    [SerializeField] float _sprintSpeed;
    [SerializeField] float _groundDrag = 5f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    bool _readyToJump;

    [Header("Key Bindings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _walkKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] Vector3 _offsetCheckSphere;
    [SerializeField] float _radiusCheckSphere;
    [SerializeField] LayerMask _groundLayer = 1;
    private bool _isGrounded;

    [Header("Slope Handling")]
    [SerializeField] private float _maxSlopeAngle;
    private RaycastHit _slopeHit;
    private bool _exitingSlope;

    [SerializeField] Transform _orientation;

    float _horizontalInput;
    float _verticalInput;
    private Rigidbody _rb;
    Vector3 _moveDirection;

    [SerializeField]
    float _playerHeight;

    public MovementState _state;
    public enum MovementState
    {
        walking,
        running,
        sprinting,
        air
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _readyToJump = true;
        _playerHeight = GetComponentInChildren<CapsuleCollider>().height;
    }

    private void Update()
    {
        _isGrounded = IsGrounded();

        InputUpdate();
        SpeedControl();
        StateHandler();

        // handle drag
        if (_isGrounded)
            _rb.drag = _groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void InputUpdate()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        // input jump
        if (Input.GetKey(_jumpKey) && _readyToJump && _isGrounded)
        {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void StateHandler()
    {
        // Mode – Walking
        if (_isGrounded && Input.GetKey(_walkKey))
        {
            _state = MovementState.walking;
            _moveSpeed = _walkSpeed;
        }

        // Mode – Sprinting
        else if (_isGrounded && Input.GetKey(_sprintKey))
        {
            _state = MovementState.sprinting;
            _moveSpeed = _sprintSpeed;
        }

        // Mode – Running
        else if (_isGrounded)
        {
            _state = MovementState.running;
            _moveSpeed = _runningSpeed;
        }

        // Mode - Air
        else
        {
            _state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;

        // on slope
        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection() * _moveSpeed * 20f, ForceMode.Force);

            if (_rb.velocity.y > 0)
                _rb.AddForce(Vector3.down * 50f, ForceMode.Force);
        }

        // on ground
        else if (_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        _rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if ( OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > _moveSpeed)
                _rb.velocity = _rb.velocity.normalized * _moveSpeed;
        }

        // limiting speed on ground or in air
        else {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            // limit velocity
            if (flatVel.magnitude > _moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _moveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position + _offsetCheckSphere, _radiusCheckSphere, _groundLayer);
    }

    private void Jump()
    {
        _exitingSlope = true;

        // reset Y velocity
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;

        _exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }


    // Getters for animator script

    public MovementState GetMovementState()
    {
        return _state;
    }

    public bool IsMoving()
    {
        return _moveDirection.magnitude != 0;
    }

    public bool IsJumpButtonPressed()
    {
        return Input.GetKey(_jumpKey);
    }

    // Debug Jump Information
    private void OnDrawGizmosSelected()
    {
        if (_isGrounded)
        {
            Color transparenGreen = Color.green;
            transparenGreen.a = 0.3f;
            Gizmos.color = transparenGreen;
            Gizmos.DrawSphere(transform.position + _offsetCheckSphere, _radiusCheckSphere);
        }
        else
        {
            Color transparenRed = Color.red;
            transparenRed.a = 0.3f;
            Gizmos.color = transparenRed;
            Gizmos.DrawSphere(transform.position + _offsetCheckSphere, _radiusCheckSphere);
        }
    }
}
