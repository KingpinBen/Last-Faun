using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Used as a general all rounder for the player.
/// Contains members that could be useful for other components, player or not.
/// </summary>
public class PlayerScript : Character
{

    public Transform jumpTarget;
    public bool canJump = true;
    public float jumpHeight = .5f;

    public float speedSmoothing = 10.0f;
    public float movementSpeed = 3.4f;

    private CameraScript _camera;
    private float _verticalSpeed = 0.0f;
    private float _moveSpeed = 0.0f;
    private bool _isBeingThrown = false;
    private bool _hasBeenThrown = false;
    private Vector3 _inAirVelocity = Vector3.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _appliedForce = Vector3.zero;
    private CollisionFlags _collisionFlags;
    private CharacterController _characterController;
    private Transform _cameraTransform;
    private Transform _matchTarget;

    private readonly int _boostAnimHash = Animator.StringToHash( "Base Layer.Boost" );

    public PlayerScript()
    {
        IsControllable = true;
    }

    private const float GROUNDED_TIMEOUT = 0.25f;
    private const float MASS = 45.0f;
    private const float GRAVITY = 20.0f;

    protected override void Awake()
    {
        base.Awake();

        _characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
        _camera = Camera.main.GetComponent<CameraScript>();
    }

    protected override void Start()
    {
        _moveDirection = transform.TransformDirection(Vector3.forward);
    }

    protected override void Update()
    {
        if (!IsControllable)
        {
            Input.ResetInputAxes();

            if (_isBeingThrown)
            {
                var animationState = _animator.GetCurrentAnimatorStateInfo( 0 );

                if (animationState.nameHash == _boostAnimHash)
                {
                    //  The kid is still being boosted   
                    _hasBeenThrown = true;
                    SetCurrentAction(CharacterAction.None);

                    _animator.MatchTarget(_matchTarget.position, _matchTarget.rotation, AvatarTarget.Root,
                                      new MatchTargetWeightMask(new Vector3(0, 1, 1), 0), 0.1f, 1f);
                }
                else
                {
                    //  Finished animation but still in boost logic.
                    if (_hasBeenThrown)
                    {
                        _hasBeenThrown = false;
                        _isBeingThrown = false;
                        IsControllable = true;
                        _animator.applyRootMotion = false;
                    }
                }
            }
        }
    }

    protected void FixedUpdate()
    {
        if (!IsControllable)
            return;

        var rhv = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical");
        if (rhv < .1f)
        {
            _cameraTransform = _camera.GetCameraTransform(true);
        }

        UpdateSmoothedMovementDirection();
        ApplyGravity();

        var speed = _moveSpeed*((_slowZones.Count == 0)
                                    ? 1
                                    : .5f);
        var movement = _moveDirection * speed + new Vector3(0, _verticalSpeed, 0) + _inAirVelocity;
        movement *= Time.deltaTime;

        if (_characterController.enabled)
            _collisionFlags = _characterController.Move(movement);

        if (IsGrounded())
        {
            _inAirVelocity = Vector3.zero;
                
            HandleRotation(_moveDirection);
        }
        else
        {
            var xzMove = movement;
            xzMove.y = 0;

            if (xzMove.sqrMagnitude > 0.001)
                HandleRotation(xzMove);
        }

        if (_appliedForce.magnitude > 0.2f)
        {
            _characterController.Move(_appliedForce*Time.deltaTime);
            _appliedForce = Vector3.Slerp(_appliedForce, Vector3.zero, Time.deltaTime*3.0f);
        }
    }

    void HandleRotation(Vector3 lookTowards)
    {
        var rotation = Quaternion.LookRotation( lookTowards );
        rotation = Quaternion.Lerp( transform.rotation, rotation, 15 * Time.deltaTime );

        transform.rotation = rotation;
    }

    private void UpdateSmoothedMovementDirection()
    {
        if (!IsControllable) return;
        
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var av = Mathf.Abs(v);
        var ah = Mathf.Abs(h);
        var s = ah > av ? ah : av;


        //  We'll just dampen the start a bit so the animation lines up a bit better
        _animator.SetFloat(_hashes.speed, s, 0.1f, Time.deltaTime);

        var forward = _cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward.Normalize();

        var right = new Vector3(forward.z, 0, -forward.x);
        var targetDirection = h*right + v*forward;

        if (IsGrounded())
        {
            if (targetDirection != Vector3.zero)
                _moveDirection = targetDirection.normalized;

            var curSmooth = speedSmoothing*Time.deltaTime;
            var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f)*movementSpeed*s;

            _moveSpeed = Mathf.Lerp(_moveSpeed, targetSpeed, curSmooth);
        }
        else
        {
            if (jumpTarget)
            {
                var dir = (jumpTarget.position - transform.position).normalized;
                dir.y = 0;
                _moveDirection = dir;
            }

            _inAirVelocity += _moveDirection*Time.deltaTime;
        }
    }

    private void ApplyGravity()
    {
        if (IsControllable)
        {
            _verticalSpeed = Mathf.Clamp(_verticalSpeed - (GRAVITY * Time.deltaTime), -10, 10);
        }
    }

    public void ApplyForce(Vector3 direction, float force)
    {
        direction.Normalize();
        direction.y = Mathf.Abs(direction.y);

        _appliedForce = (direction.normalized*force)/MASS;
    }

    private float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        return Mathf.Sqrt(2*targetJumpHeight*GRAVITY);
    }

    public bool IsGrounded()
    {
        return (_collisionFlags & 
            CollisionFlags.Below) == CollisionFlags.Below;
    }

    public void Push(Vector3 direction)
    {
        _characterController.Move(direction);
    }

    public void Jump()
    {
        if (!canJump) 
            return;

        StopAllCoroutines();
        StartCoroutine(ResetJump());

        _verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
        SetCurrentAction(CharacterAction.Jumping);
    }

    public bool IsControllable { get; set; }

    public void BoostJump(Transform matchTarget)
    {
        IsControllable = false;
        SetCurrentAction(CharacterAction.Boosting);
        _matchTarget = matchTarget;
        _animator.applyRootMotion = true;
        
        _isBeingThrown = true;
        _verticalSpeed = 0.0f;

        /* This will just change the looking direction of the player to 
         * face where he needs to. */
        _moveDirection = ( matchTarget.position - transform.position).normalized;
        _moveDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(_moveDirection);
        _moveSpeed = 0.0f;
        _animator.SetFloat(_hashes.speed, 0);
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(.25f);

        SetCurrentAction(CharacterAction.None);
    }
}