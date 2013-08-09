using System;
using UnityEngine;
using System.Collections;

public class BoulderScript : GestureObject
{
    private float _elapsed;
    private BoulderState _state;
    private Ray ray;
    private Vector3 hitPoint;

    /// <summary>
    /// The hash values for some of the required states to check for so we're not 
    /// constantly calling the static method. Also stops us having to route through code to 
    /// change it all over the place if we restructure the Animator.
    /// </summary>
    private readonly int _idleHash = Animator.StringToHash( "Idle.Standing Idle" );
    private readonly int _emptyHand = Animator.StringToHash( "Upper.Empty" );
    private readonly int _kneelIdleHash = Animator.StringToHash( "Idle.Kneeling Idle" );
    private readonly int _motionHash = Animator.StringToHash( "Boulder Pick Up.Motion" );

    protected override void Update()
    {
        //  If the companion is using another object currently and this isn't it, we
        //  shouldn't be able to use it.
        if ( _state == BoulderState.Grounded &&
             _companionScript.IsIdle() == false )
            return;

        base.Update();

        switch ( _state )
        {
            #region Carried
            case BoulderState.Carried:
            {
                if ( Input.GetMouseButtonDown( 0 ) )
                {
                    if ( _isMouseOvered == false )
                    {
                        RaycastHit rayHit;
                        ray = Camera.main.ScreenPointToRay( Input.mousePosition );

                        if ( Physics.Raycast( ray, out rayHit, 50.0f, LayerMasks.IGNORE_RAY_GEST ) )
                        {
                            //  Work out the normal of the surface we just hit.
                            var normal = Quaternion.FromToRotation( Vector3.forward, rayHit.normal ).eulerAngles;
                            var y = 360 - normal.x;

                            if ( ( y > 60 && y <= 90 ) && rayHit.collider.tag != "Environment" )
                            {
                                //  It was a suitable angle so we'll tell the companion to drop it.
                                aiTargetNode.transform.parent = null;

                                aiTargetNode.transform.position = rayHit.point;
                                aiTargetNode.ActivateObject( this );
                                _state = BoulderState.Dropping;

                                hitPoint = rayHit.point;

                                _elapsed = 0.0f;

                                CursorManager.instance.ChangeCursor( CursorType.Normal );
                                CursorManager.instance.ChangeCursorStatus( CursorManager.CursorGestureStatus.Successful );
                            }
                        }
                    }
                }
            }
                break;
            #endregion
            #region Dropping
            case BoulderState.Dropping:
            {
                //  Check if the companion is where he's 
                //  supposed to be and drop the boulder if he's there.
                if (_companionScript.waitingAtTarget )
                {
                    var animationStateInfo = _companionScript.GetAnimator().GetCurrentAnimatorStateInfo( 0 );
                    
                    if ( animationStateInfo.nameHash == _idleHash )
                    {
                        StartCoroutine( StartCooldown() );
                        _companionScript.SetCurrentAction( Character.CharacterAction.None );

                        _player.ActivateObject( this );
                    }
                    else
                    {
                        //  This timer is just here to give a bit of a delay in the animation to
                        //  reach down to pick it up/put it down.
                        if ( _elapsed >= 0.38f )
                        {
                            transform.parent = null;
                            transform.position = new Vector3( transform.position.x, hitPoint.y, transform.position.z );
                            transform.rotation = Quaternion.identity;

                            aiTargetNode.transform.parent = transform;
                            aiTargetNode.transform.localPosition = Vector3.zero;
                            _companionScript.SetCurrentAction( Character.CharacterAction.None );
                        }
                        else
                        {
                            _elapsed += Time.deltaTime;
                        }
                    }
                }
            }
                break;
            #endregion
            #region Grounded
            case BoulderState.Grounded:
            {
                if ( _successfulGesture )
                {
                    //  If it's been deactivated, we no longer want to use this object.
                    if ( aiTargetNode.objectActive == false )
                    {
                        _successfulGesture = false;
                    }

                    if ( _companionScript.waitingAtTarget )
                    {
                        var stateInfo = _companionScript.GetAnimator().GetCurrentAnimatorStateInfo( 0 );

                        //  If they're in an idle position, we can make them start picking up the boulder.
                        if ( stateInfo.nameHash == _idleHash ||
                             stateInfo.nameHash == _kneelIdleHash )
                        {
                            _state = BoulderState.BeingLifted;
                            
                        }
                    }
                }
                else
                {
                    if ( _objectUsed &&
                         _listening == false )
                    {
                        _objectUsed = false;
                        _player.ActivateObject( this );
                    }
                }
            }
                break;
            #endregion
            case BoulderState.BeingLifted:
            {
                //  If he's still doing something with hands, we'll delay picking this up till it's finished
                //  emoting
                var handStateInfo = _companionScript.GetAnimator().GetCurrentAnimatorStateInfo( 1 );
                if (handStateInfo.nameHash != _emptyHand)
                {
                    StartCoroutine(WaitForHandAnimation());
                    return;
                }
                    
                if ( _companionScript.waitingAtTarget && IsInIdleAnimation() )
                    StartCoroutine( DelayPickUp() );
            }
                break;

            case BoulderState.Waiting:
                break;
        }
    }

    bool IsInIdleAnimation()
    {
        var stateInfo = _companionScript.GetAnimator().GetCurrentAnimatorStateInfo( 0 );
        return ( stateInfo.nameHash == _idleHash || stateInfo.nameHash == _kneelIdleHash );
    }

    IEnumerator WaitForHandAnimation()
    {
        _state = BoulderState.Waiting;

        yield return new WaitForSeconds( .4f );
        
        //  Get back into the being lifted state to be checked again.
        _state = BoulderState.BeingLifted;
    }

    IEnumerator DelayPickUp()
    {
        _companionScript.SetCurrentAction(Character.CharacterAction.Carrying);
        _state = BoulderState.Waiting;
        yield return new WaitForSeconds( .24f );

        transform.parent = _companionScript.rightHand;
        transform.localPosition = new Vector3(0.038f, -0.18f, -0.035f);
        transform.localRotation = Quaternion.Euler( 57.3f, 5.2f, 0 ); 

        _successfulGesture = false;
        _state = BoulderState.Carried;

        CursorManager.instance.ChangeCursor(CursorType.PutDown);
        CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Capturing);

        _objectUsed = false;
        _player.ActivateObject(this);
    }

    protected override void ProcessReceivedGesture()
    {
        switch ( _receivedGesture )
        {
            case GestureType.Up:
                SuccessfulGesture();
                break;
            default:
                FailedGesture();
                break;
        }

        _receivedGesture = GestureType.None;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(ray.origin, hitPoint);
    }

    private IEnumerator StartCooldown()
    {
        _state = BoulderState.Waiting;

        yield return new WaitForSeconds( 1.2f );
        _state = BoulderState.Grounded;
    }

    private enum BoulderState
    {
        Grounded,
        Dropping,
        Carried,
        BeingLifted,
        Waiting
    }
}
