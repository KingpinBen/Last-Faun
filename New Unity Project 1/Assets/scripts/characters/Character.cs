using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator)),
RequireComponent(typeof(NavMeshAgent)),
RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform itemHolster;

    public CharacterAction currentAction { get; protected set; }

    protected Animator _animator;
    protected NavMeshAgent _navAgent;
    protected readonly List<SlowZoneScript> _slowZones = new List<SlowZoneScript>();

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

	protected virtual void Start ()
	{
	    
	}

    protected virtual void Update()
    {
	
	}

    public virtual void RegisterSlowZone(SlowZoneScript script)
    {
        if (_slowZones.Count == 0)
            _animator.SetBool("Slowed", true);


        _slowZones.Add(script);
    }

    public virtual void RemoveSlowZone(SlowZoneScript script)
    {
        for(var i = 0; i < _slowZones.Count; ++i)
        {
            if (_slowZones[i] != script) continue;

            _slowZones.RemoveAt(i);
            break;
        }

        if (_slowZones.Count == 0)
            _animator.SetBool("Slowed", false);
    }

    public virtual void SetCurrentAction(CharacterAction newAction)
    {
        if (newAction == currentAction) return;

        switch (newAction)
        {
            case CharacterAction.Waiting:
            case CharacterAction.None:
                _animator.SetInteger("CurrentAction", 0);
                break;
            case CharacterAction.Carrying:
                _animator.SetInteger("CurrentAction", 1);
                break;
            case CharacterAction.Pushing:
                _animator.SetInteger("CurrentAction", 2);
                break;
            case CharacterAction.Climbing:
                _animator.SetInteger("CurrentAction", 3);
                break;
            case CharacterAction.Boosting:
                _animator.SetInteger("CurrentAction", 4);
                break;
            case CharacterAction.UsingLever:
                _animator.SetInteger("CurrentAction", 5);
                break;
            case CharacterAction.Jumping:
                _animator.SetInteger("CurrentAction", 6);
                break;
            case CharacterAction.Pulling:
                _animator.SetInteger("CurrentAction", 7);
                break;
        }

        currentAction = newAction;
    }

    public NavMeshAgent GetAgent()
    {
        return _navAgent;
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
}
