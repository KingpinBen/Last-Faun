using UnityEngine;

public class BoulderScript : GestureObject
{
    private float _elapsed;
    private BoulderState _state;

    /// <summary>
    /// The hash values for some of the required states to check for so we're not 
    /// constantly calling the static method. Also stops us having to route through code to 
    /// change it all over the place if we restructure the Animator.
    /// </summary>
    private readonly int _idleHash = Animator.StringToHash("Idle.Standing Idle");
    private readonly int _kneelIdleHash = Animator.StringToHash("Idle.Kneeling Idle");
    private readonly int _motionHash = Animator.StringToHash("Boulder Pick Up.Motion");

    protected override void Update()
    {
        //  If the companion is using another object currently and this isn't it, we
        //  shouldn't be able to use it.
        if (_state == BoulderState.Grounded && 
            _companionScript.IsIdle() == false) return;

        base.Update();

        switch(_state)
        {
            case BoulderState.Carried:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //  TODO: Maybe remove this and change the layer of the object to be ignored by the ray.
                        if (_isMouseOvered == false)
                        {
                            RaycastHit rayHit;
                            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                            if (Physics.Raycast(ray, out rayHit, 50.0f, ~(1 << 2)))
                            {
                                //  Work out the normal of the surface we just hit.
                                var normal = Quaternion.FromToRotation(rayHit.normal, Vector3.forward).eulerAngles;

                                /*  We're checking to see if the normal of the 
                                 * surface hit isn't too sloped.   90 is up
                                 * 
                                 * We'll also check the tag for 'Environment' so we know not to allow it to be placed there
                                 */
                                if (normal.x >= 75f && rayHit.collider.tag != "Environment")
                                {
                                    //  It was a suitable angle so we'll tell the companion to drop it.
                                    aiTargetNode.transform.parent = null;
                                    aiTargetNode.transform.position = rayHit.point;
                                    aiTargetNode.ActivateObject(this);
                                    _state = BoulderState.Dropping;

                                    CursorManager.instance.ChangeCursor(CursorType.Normal);
                                    CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Successful);
                                }
                            }
                        }
                    }
                }
                break;
            case BoulderState.Dropping:
                {
                    //  Check if the companion is where he's 
                    //  supposed to be and drop the boulder if he's there.
                    if (aiTargetNode.objectActive &&
                        _companionScript.waitingAtTarget)
                    {
                        var animationStateInfo = _companionScript.GetAnimator().GetCurrentAnimatorStateInfo(0);

                        if (animationStateInfo.nameHash == _idleHash)
                        {
                            _elapsed = 0.0f;
                            _state = BoulderState.Grounded;
                            _companionScript.SetCurrentAction(Character.CharacterAction.None);

                            _player.ActivateObject(this);
                        }
                        else
                        {
                            //  This timer is just here to give a bit of a delay in the animation to
                            //  reach down to pick it up/put it down.
                            if (_elapsed >= 0.38f)
                            {
                                RaycastHit hit;

                                Physics.Raycast(aiTargetNode.transform.position, Vector3.down, out hit, 2.0f);

                                transform.parent = null;
                                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                                transform.rotation = Quaternion.identity;

                                aiTargetNode.transform.parent = transform;
                                aiTargetNode.transform.localPosition = Vector3.zero;
                                _companionScript.SetCurrentAction(Character.CharacterAction.None);
                            }
                            else
                                _elapsed += Time.deltaTime;
                        }
                    }
                }
                break;
            case BoulderState.Grounded:
                {
                    if (_successfulGesture)
                    {
                        if (aiTargetNode.objectActive == false)
                        {
                            //  If it's been deactivated, we no longer want to use this object.
                            _successfulGesture = false;
                        }
                            
                        if (_companionScript.waitingAtTarget)
                        {
                            var stateInfo = _companionScript.GetAnimator().GetCurrentAnimatorStateInfo(0);

                            //  If they're in an idle position, we can make them start picking up the boulder.
                            if (stateInfo.nameHash == _idleHash ||
                                stateInfo.nameHash ==  _kneelIdleHash)
                            {
                                _state = BoulderState.BeingLifted;
                                _companionScript.SetCurrentAction(Character.CharacterAction.Carrying);
                            }
                        }
                    }
                    else
                    {
                        if (_objectUsed && 
                            _listening == false)
                        {
                            _objectUsed = false;
                            _player.ActivateObject(this);
                        }
                    }
                }
                break; 
            case BoulderState.BeingLifted:
                {
                    var stateInfo = _companionScript.GetAnimator().GetCurrentAnimatorStateInfo(0);

                    if (stateInfo.nameHash == _motionHash)
                    {
                        /* Animation, picking up object is complete.
                         * Begin retargetting to player */
                        _successfulGesture = false;
                        _elapsed = 0f;

                        _state = BoulderState.Carried;

                        CursorManager.instance.ChangeCursor(CursorType.PutDown);
                        CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Capturing);
                        
                        _objectUsed = false;
                        _player.ActivateObject(this);
                    }
                    else
                    {
                        //  This timer is just here to give a bit of a delay in the animation to
                        //  reach down to pick it up/put it down.
                        if (_elapsed >= 0.25f)
                        {
                            transform.parent = _companionScript.rightHand;
                            transform.localPosition = new Vector3(0.038f, -0.18f, -0.035f);
                            transform.rotation =
                                Quaternion.Euler(new Vector3(Mathf.Deg2Rad*51.4f, Mathf.Deg2Rad*345.5f,
                                                             Mathf.Deg2Rad*347.2f));
                        }
                        else
                            _elapsed += Time.deltaTime;
                    }
                }
                break;
        }
    }
    protected override void ProcessReceivedGesture()
    {
        switch (_receivedGesture)
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

    protected override void FailedGesture()
    {
        base.FailedGesture();

        _companionScript.GetEmotions().InstantiateEmoticon(Emotions.Bad);
    }
    protected override void SuccessfulGesture()
    {
        base.SuccessfulGesture();

        _companionScript.GetEmotions().InstantiateEmoticon(Emotions.Good);
    }

    private enum BoulderState
    {
        Grounded,
        Dropping, 
        Carried, 
        BeingLifted
    }
}
