using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof ( Animator ) ),
 RequireComponent( typeof ( CharacterController ) )]
[RequireComponent( typeof ( CharacterAnimationHashes ) )]
public class Character : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform itemHolster;

    public CharacterAction currentAction { get; protected set; }

    protected Animator _animator;

    protected CharacterAnimationHashes _hashes;
    protected readonly List< SlowZoneScript > _slowZones = new List< SlowZoneScript >();

    protected virtual void Awake()
    {
        _animator = GetComponent< Animator >();
        _hashes = GetComponent< CharacterAnimationHashes >();

        _animator.SetLayerWeight(1, 1);
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void RegisterSlowZone( SlowZoneScript script )
    {
        if ( _slowZones.Count == 0 )
            _animator.SetBool(_hashes.slowed, true );

        _slowZones.Add( script );
    }

    public virtual void RemoveSlowZone( SlowZoneScript script )
    {
        for ( var i = 0; i < _slowZones.Count; ++i )
        {
            if ( _slowZones[i] != script )
                continue;

            _slowZones.RemoveAt( i );
            break;
        }

        if ( _slowZones.Count == 0 )
            _animator.SetBool(_hashes.slowed, false);
    }

    public virtual void SetCurrentAction( CharacterAction newAction )
    {
        if ( newAction == currentAction )
            return;

        switch ( newAction )
        {
            case CharacterAction.Waiting:
            case CharacterAction.None:
                _animator.SetInteger( _hashes.currentAction, 0 );
                break;
            case CharacterAction.Carrying:
                _animator.SetInteger(_hashes.currentAction, 1);
                break;
            case CharacterAction.Pushing:
                _animator.SetInteger(_hashes.currentAction, 2);
                break;
            case CharacterAction.Climbing:
                _animator.SetInteger(_hashes.currentAction, 3);
                break;
            case CharacterAction.Boosting:
                _animator.SetInteger(_hashes.currentAction, 4);
                break;
            case CharacterAction.UsingLever:
                _animator.SetInteger(_hashes.currentAction, 5);
                break;
            case CharacterAction.Jumping:
                _animator.SetInteger(_hashes.currentAction, 6);
                break;
            case CharacterAction.Pulling:
                _animator.SetInteger(_hashes.currentAction, 7);
                break;
        }

        currentAction = newAction;
    }

    public void SetCompleteAction(bool toSet)
    {
        _animator.SetBool(_hashes.completeAction, toSet);
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public enum CharacterAction
    {
        None,
        Waiting,
        Boosting,
        Carrying,
        Pushing,
        Climbing,
        UsingLever,
        Jumping,
        Pulling
    }

    public bool IsIdle()
    {
        return currentAction == CharacterAction.None || currentAction == CharacterAction.Waiting;
    }

    public CharacterAnimationHashes GetAnimationHashes()
    {
        return _hashes;
    }
}