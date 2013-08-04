using UnityEngine;
using System.Collections;

public class LeverSwitchScript : GestureObject
{
    public InteractiveObject targetObject;
    public PivotScript leverPivot;

    private bool _waiting;

    private const float TIME_BEFORE_RESET = 2.5f;

    protected override void Update()
    {
        if (_companionScript.IsIdle() == false) return;

        base.Update();

        if (_successfulGesture)
        {
            if (aiTargetNode.objectActive)
            {
                if (_companionScript.waitingAtTarget)
                {
                    _companionScript.SetCurrentAction(Character.CharacterAction.UsingLever);
                    _companionScript.transform.rotation = Quaternion.LookRotation(
                            transform.position - _companionScript.transform.position);

                    StartCoroutine(ActivateLever());
                    StartCoroutine(FollowerPlayer());
                    StartCoroutine(ResetSwitch());

                    _successfulGesture = false;
                    _waiting = true;
                }
            }
            else
                _successfulGesture = false;
        }
        else
        {
            if (_waiting == false)
            {
                if (_objectUsed && _listening == false)
                {
                    _objectUsed = false;
                    _player.ActivateObject(this);
                }
            }
        }
    }

    protected override void ProcessReceivedGesture()
    {
        switch (_receivedGesture)
        {
            case GestureType.Down:
                SuccessfulGesture();
                break;
            default:
                FailedGesture();
                break;
        }

        _receivedGesture = GestureType.None;
    }

    protected override void SuccessfulGesture()
    {
        base.SuccessfulGesture();
        objectActive = false;
        StartCoroutine(ResetSwitch());
    }

    IEnumerator FollowerPlayer()
    {
        yield return new WaitForSeconds(1.0f);

        _player.ActivateObject(this);
        _companionScript.SetCurrentAction(Character.CharacterAction.None);
        _waiting = false;
    }

    IEnumerator ResetSwitch()
    {
        yield return new WaitForSeconds(TIME_BEFORE_RESET);

        objectActive = true;
        leverPivot.Close();
    }

    IEnumerator ActivateLever()
    {
        yield return new WaitForSeconds(.25f);

        targetObject.ToggleObject(this);
        leverPivot.Open();
    }
}
