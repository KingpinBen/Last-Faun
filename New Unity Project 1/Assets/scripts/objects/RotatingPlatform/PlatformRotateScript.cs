using System;
using UnityEngine;

public class PlatformRotateScript : GestureObject
{

    public Transform rotatingBody;
    public float rotationSpeed = 1.0f;
    public bool rotateForward = true;
    public bool rotateBack = true;
    public bool rotateLeft = true;
    public bool rotateRight = false;

    private float _targetRotation;
    private Vector3 _direction;

    protected override void Start()
    {
        _targetRotation = transform.localRotation.y;
    }

    protected override void Update()
    {
        if (_companionScript.IsIdle() == false) return;

        base.Update();

        if (_successfulGesture)
        {
            if (_companionScript.waitingAtTarget)
            {
                switch (_receivedGesture)
                {
                    case GestureType.CWSemiCircle:
                        TriggerRotation(true); 
                        break;
                    case GestureType.CCWSemiCircle:
                        TriggerRotation(false); 
                        break;
                }
            }
        }
        else
        {
            /* This is needed so we can have multiple objects in the same scene
             * without constantly trying to update and set player as target */
            if (_objectUsed && _listening == false)
            {
                _player.ActivateObject(this);
                _objectUsed = false;
            }
        }

        HandleRotation();
    }

    protected override void ProcessReceivedGesture()
    {
        switch (_receivedGesture)
        {
            case GestureType.CWSemiCircle:
            case GestureType.CWFullCircle:
            case GestureType.CCWSemiCircle:
            case GestureType.CCWFullCircle:
                SuccessfulGesture();
                break;
            default:
                FailedGesture();
                break;
        }
    }

    /// <summary>
    /// Triggers the rotation.
    /// </summary>
    /// <param name='direction'>
    /// The direction to turn the object. True = CW, False = CCW.
    /// </param>
    private void TriggerRotation(bool direction)
    {
        if (!objectActive) return;

        var rot = _targetRotation;

        if (direction)
        {
            #region CW Handling

            if (rot >= 270)
            {
                if (rotateForward)
                    HandleRotationChange(0.0f);
                else
                {
                    if (rotateRight)
                        HandleRotationChange(90.0f);
                    else
                    {
                        if (rotateBack)
                            HandleRotationChange(180.0f);
                    }
                }
            }
            else
            {
                if (rot >= 180)
                {
                    if (rotateLeft)
                        HandleRotationChange(270.0f);
                    else
                    {
                        if (rotateForward)
                            HandleRotationChange(0.0f);
                        else
                        {
                            if (rotateRight)
                                HandleRotationChange(90.0f);
                        }
                    }
                }
                else
                {
                    if (rot >= 90)
                    {
                        if (rotateBack)
                            HandleRotationChange(180.0f);
                        else
                        {
                            if (rotateLeft)
                                HandleRotationChange(270.0f);
                            else
                            {
                                if (rotateForward)
                                    HandleRotationChange(0.0f);
                            }
                        }
                    }
                    else
                    {
                        if (rot >= 0)
                        {
                            if (rotateRight)
                                HandleRotationChange(90.0f);
                            else
                            {
                                if (rotateBack)
                                    HandleRotationChange(180.0f);
                                else
                                {
                                    if (rotateLeft)
                                        HandleRotationChange(270.0f);
                                }
                            }
                        }
                    }
                }
            }

            #endregion
        }
        else
        {
            #region CCW Handling

            if (rot >= 270)
            {
                if (rotateBack)
                    HandleRotationChange(180.0f);
                else
                {
                    if (rotateRight)
                        HandleRotationChange(90.0f);
                    else
                    {
                        if (rotateForward)
                            HandleRotationChange(0.0f);
                    }
                }
            }
            else if (rot >= 180)
            {
                if (rotateRight)
                    HandleRotationChange(90.0f);
                else
                {
                    if (rotateForward)
                        HandleRotationChange(0.0f);
                    else
                    {
                        if (rotateLeft)
                            HandleRotationChange(270.0f);
                    }
                }
            }
            else if (rot >= 90)
            {
                if (rotateForward)
                    HandleRotationChange(0.0f);
                else
                {
                    if (rotateLeft)
                        HandleRotationChange(270.0f);
                    else
                    {
                        if (rotateBack)
                            HandleRotationChange(180.0f);
                    }
                }
            }
            else if (rot >= 0)
            {
                if (rotateLeft)
                    HandleRotationChange(270.0f);
                else
                {
                    if (rotateBack)
                        HandleRotationChange(180.0f);
                    else
                    {
                        if (rotateRight)
                            HandleRotationChange(90.0f);
                    }
                }
            }

            #endregion
        }

        _objectUsed = true;
        _successfulGesture = false;
        _receivedGesture = GestureType.None;
    }

    /// <summary>
    /// Handles the changing of the target rotation
    /// </summary>
    /// <param name='newRotation'>
    /// The target rotation to change to.
    /// </param>
    private void HandleRotationChange(float newRotation)
    {

        //	Lets keep the numbers nice. SpinDoctor ruined rotating
        //	for me.
        if (newRotation >= 360)
            newRotation -= 360;
        else
        {
            if (newRotation < 0)
                newRotation += 360;
        }

        //	Set the new target rotation for managing 
        //	the rotation over time.
        _targetRotation = newRotation;

        if (newRotation >= 270)
            _direction = Vector3.left;
        else if (newRotation >= 180)
            _direction = Vector3.back;
        else if (newRotation >= 90)
            _direction = Vector3.right;
        else if (newRotation >= 0)
            _direction = Vector3.forward;
    }

    private void HandleRotation()
    {
        var rot = transform.rotation;

        if (Math.Abs(rot.y - _targetRotation) > Mathf.Epsilon)
        {
            var targetRot = Quaternion.identity;
            if (_direction != Vector3.zero)
                targetRot = Quaternion.LookRotation(_direction);

            rot = Quaternion.Slerp(rot, targetRot, Time.deltaTime*rotationSpeed);
            Vector3 euler = rot.eulerAngles;
            euler.x = euler.z = 0;
            rot = Quaternion.Euler(euler);

            transform.rotation = rot;
        }
    }
}
