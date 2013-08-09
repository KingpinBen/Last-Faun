using UnityEngine;

public class BlockPushScript : GestureObject
{
    private bool _blockBeingPushed;
    private Vector3 _targetMovementPosition;
    private BlockScript _block;
    private BlockStatus _status;

    protected override void Start()
    {
        collider.isTrigger = true;
        _block = transform.parent.parent.GetComponent<BlockScript>();
    }

    protected override void Update()
    {
        if (_companionScript.IsIdle() == false &&
            (_companionScript.currentAction != Character.CharacterAction.Pushing && 
            _companionScript.currentAction != Character.CharacterAction.Pulling)) 
                return;

        base.Update();

        switch(_status)
        {
            case BlockStatus.BeingPushed:
            {
                if (aiTargetNode.objectActive)
                {

                    var direction = ( _targetMovementPosition - _block.transform.position );
                    direction.y = 0;

                    _block.transform.position += direction.normalized * Time.deltaTime;

                    if (direction.sqrMagnitude < 0.1f)
                    {
                        _block.transform.position = _targetMovementPosition;
                        _companionScript.SetCurrentAction(Character.CharacterAction.None);
                        _status = BlockStatus.Idle;
                    }
                }
                else
                {
                    _successfulGesture = false;
                    _companionScript.SetCurrentAction(Character.CharacterAction.None);
                    _status = BlockStatus.Idle;
                }
            }
                break;
            case BlockStatus.Idle:
            {
                if (_successfulGesture)
                {
                    if (aiTargetNode.objectActive)
                    {
                        if (_companionScript.waitingAtTarget)
                        {
                            RaycastHit hit;

                            if (CheckForValidTargets(out hit))
                            {
                                var targetHit =
                                    hit.transform.gameObject.GetComponent<BlockTrackTarget>();

                                if (targetHit && (targetHit.occupyingBlock == false || targetHit.occupyingBlock == _block))
                                {
                                    /* We're going to make it occupied here so that a block can't 
                                     * be pushed onto the same one if the push gets interrupted.  */
                                    _block.currentlyOccupying = targetHit;
                                    targetHit.occupyingBlock = _block;
                                    _targetMovementPosition = hit.transform.position;
                                    _targetMovementPosition.y = _block.transform.position.y;

                                    _status = BlockStatus.BeingPushed;
                                    _companionScript.SetCurrentAction(_blockBeingPushed
                                                                          ? Character.CharacterAction.Pushing
                                                                          : Character.CharacterAction.Pulling);
                                }
                                else
                                {
                                    //  Target is occupied so just return to the player
                                    _player.ActivateObject(this);
                                    _companionScript.GetEmotions().InstantiateEmoticon(Emotions.Bad);
                                }
                            }

                            _successfulGesture = false;

                            _companionScript.transform.rotation = Quaternion.LookRotation(transform.forward);
                            //  Set the angular speed to 0 to stop the companion spinning
                            _companionScript.GetAgent().angularSpeed = 0;
                        }
                    }
                    else
                    {
                        _successfulGesture = false;
                    }
                }
                else
                {
                    /* This is needed so we can have multiple objects in the same scene
                     * without constantly trying to update and set player as target */
                    if (_objectUsed && _listening == false)
                    {
                        _objectUsed = false;
                        _companionScript.GetAgent().angularSpeed = 360;
                        _player.ActivateObject(this);
                    }
                }
            }
                break;
        }
    }

    bool CheckForValidTargets(out RaycastHit hit)
    {
        var direction = _blockBeingPushed
                            ? transform.forward
                            : -transform.forward;

        return ( Physics.Raycast( _block.transform.position, direction, out hit, 12, LayerMasks.HIT_BLOCK_TARGET ) &&
                 hit.collider.tag != "BlockRay" );
    }

    protected override void ProcessReceivedGesture()
    {
        switch (_receivedGesture)
        {
            case GestureType.Up:
                SuccessfulGesture();
                _blockBeingPushed = true;
                break;
            case GestureType.Down:
                SuccessfulGesture();
                _blockBeingPushed = false;
                break;
            default:
                FailedGesture();
                break;
        }

        _receivedGesture = GestureType.None;
    }

    public enum BlockStatus
    {
        Idle,
        BeingPushed,
        Waiting
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.tag != "Player")
            return;

        other.transform.parent = transform;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.isTrigger || other.tag != "Player")
            return;

        other.transform.parent = null;
    }
}
