using UnityEngine;

public class BlockPushScript : GestureObject
{
    private bool _movingBlock;
    private bool _blockBeingPushed;
    private float _movementSpeed;
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

        if (_movingBlock)
        {
            if (aiTargetNode.objectActive == false)
            {
                _successfulGesture = false;
                _movingBlock = false;
                _companionScript.SetCurrentAction(Character.CharacterAction.None);

                return;
            }

            var direction = _targetMovementPosition - _block.transform.position;
            direction.Normalize();
            _block.transform.position += direction*Time.deltaTime;

            if ((_targetMovementPosition - _block.transform.position).sqrMagnitude < 0.01f )
            {
                _block.transform.position = _targetMovementPosition;
                _movingBlock = false;

                _companionScript.SetCurrentAction(Character.CharacterAction.None);
            }
        }
        else
        {
            if (_successfulGesture)
            {
                if (aiTargetNode.objectActive)
                {
                    if (_companionScript.waitingAtTarget)
                    {
                        //  We need the forward for 2 reasons. 
                        //  1) To tell the companion to face the block
                        //  2) To decide which way the block is incase we need to move it away,
                        var forward = transform.position - _companionScript.transform.position;
                        forward.y = _block.transform.position.y;

                        var direction = _blockBeingPushed
                                            ? forward
                                            : -forward;

                        RaycastHit hit;

                        if (Physics.Raycast(_block.transform.position, direction, out hit, 12, (1 << 8))
                            && hit.collider.tag != "BlockRay")
                        {
                            var targetHit =
                                hit.transform.gameObject.GetComponent<BlockTrackTarget>();

                            if (targetHit.occupyingBlock == false || targetHit.occupyingBlock == _block)
                            {
                                /* We're going to make it occupied here so that a block can't 
                                 * be pushed onto the same one if the push gets interrupted.  */
                                _block.currentlyOccupying = targetHit;
                                targetHit.occupyingBlock = _block;
                                _targetMovementPosition = hit.transform.position;
                                _movingBlock = true;

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

                        var lookAt = Quaternion.LookRotation(forward);
                        _companionScript.transform.rotation = lookAt;
                        //  Set the angular speed to 0 to stop the companion spinning
                        _companionScript.GetAgent().angularSpeed = 0;
                    }
                }
                else
                    _successfulGesture = false;
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
