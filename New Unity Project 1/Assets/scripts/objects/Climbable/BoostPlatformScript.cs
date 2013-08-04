using System;
using UnityEngine;
using System.Collections;

public class BoostPlatformScript : GestureObject
{
    public Transform model;
    public MatchTargetScript matchTarget;

    private bool _playerInRange;
    private float _elapsedWaiting;

    private BoostPlatStatus _status = BoostPlatStatus.Idle;

    private const float TIME_BEFORE_RESET = 2.5f;

    protected override void Update()
    {
        if (_playerInRange)
        {
            base.Update();
        }

        switch(_status)
        {
            case BoostPlatStatus.Idle:
                {
                    if (aiTargetNode.objectActive)
                    {
                        if (_companionScript.CheckAtTargetByDistance())
                        {
                            //  Force the companion to look perpendicular to the platform and wait.
                            var lookAway = Quaternion.LookRotation(
                                transform.position - model.transform.position);

                            _companionScript.transform.rotation = lookAway;
                            _companionScript.SetCurrentAction(Character.CharacterAction.Boosting);

                            _status = BoostPlatStatus.Waiting;
                        }
                    }
                }
                break;
            case BoostPlatStatus.Waiting:
                {
                    if (_elapsedWaiting > TIME_BEFORE_RESET)
                        _status = BoostPlatStatus.Resetting;
                    else 
                        _elapsedWaiting += Time.deltaTime;

                    if (_successfulGesture)
                    {
                        if (aiTargetNode.objectActive == false)
                        {
                            _successfulGesture = false;
                        }
                        else
                        {
                            if (_playerInRange)
                            {
                                _status = BoostPlatStatus.Boosting;
                                _successfulGesture = false;
                            }
                            else
                            {
                                _status = BoostPlatStatus.Resetting;
                            }
                        }
                    }
                    else
                    {
                        if (_objectUsed && _listening == false)
                        {
                            _objectUsed = false;
                            _status = BoostPlatStatus.Resetting;
                        }
                    }
                }
                break;
            case BoostPlatStatus.Boosting:
                {
                    if (_playerInRange)
                    {
                        var lookAway = Quaternion.LookRotation(
                            _player.transform.position - aiTargetNode.transform.position);

                        _companionScript.transform.rotation = lookAway;

                        aiTargetNode.agent.enabled = false;
                        _companionScript.GetAnimator().SetBool("CompleteAction", true);

                        matchTarget.SetMatchTarget();
                        _playerInRange = false;
                        StartCoroutine(ResetAfterUse());
                    }
                }
                break;
            case BoostPlatStatus.Resetting:
                {
                    _elapsedWaiting = 0.0f; 
                    _successfulGesture = false;
                    aiTargetNode.agent.enabled = true;

                    //  We'll check to see if it is still active before we reactivate the player
                    //  incase we've already switched to a new object.
                    if (aiTargetNode.objectActive)
                        _player.ActivateObject(this);

                    _companionScript.SetCurrentAction(Character.CharacterAction.None);

                    _status = BoostPlatStatus.Idle;
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

    protected override void OnMouseEnter()
    {
        if (!objectActive) return;
        _isMouseOvered = true;
        CheckInRange();
    }

    protected override void OnMouseExit()
    {
        if (!objectActive) return;
        _isMouseOvered = false;
        CheckInRange();
    }

    private void OnTriggerEnter(Collider body)
    {
        if (body.tag != "Player") 
            return;

        _playerInRange = true;
        CheckInRange();
    }
    private void OnTriggerExit(Collider body)
    {
        if (body.tag != "Player") return;
        _playerInRange = false;
        CheckInRange();
    }

    private void CheckInRange()
    {
        if (_playerInRange)
            CursorManager.instance.ChangeCursor(_isMouseOvered ? mouseOverCursor : CursorType.Normal);
        else
            CursorManager.instance.ChangeCursor(CursorType.Normal);
    }

    IEnumerator ResetAfterUse()
    {
        yield return new WaitForSeconds(.5f);

        _status = BoostPlatStatus.Resetting;
        _companionScript.GetAnimator().SetBool("CompleteAction", false);
    }

    internal enum BoostPlatStatus
    {
        Idle,
        Waiting,
        Boosting,
        Resetting
    }

    protected override void FailedGesture()
    {
        base.FailedGesture();

        StartCoroutine(ResetAfterUse());
    }
}
